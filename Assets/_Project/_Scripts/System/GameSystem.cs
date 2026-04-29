using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

public class GameSystem : MonoBehaviour
{
    [Header("Units")]
    [SerializeField] private AssetReference unitPrefab;
    private AddressablesPool<Unit> unitPool;
    [SerializeField] private Transform mainGameTrans;
    
    [SerializeField] private Unit player;
    public Unit Player => player;
    
    [Header("Effect")]
    [SerializeField] private AssetReference statusEffectPrefab;
    private AddressablesPool<StatusEffectUIBehaviour> statusEffectPool;
    
    [Header("Cards")]
    [SerializeField] private List<CardID> deckIDs;
    [SerializeField] private List<Card> decks;
    [SerializeField] private List<Card> handCards;
    [SerializeField] private List<Card> drawPileCards;
    [SerializeField] private List<Card> discardCards;
    
    [SerializeField] private AssetReference cardPrefab;
    private AddressablesPool<Card> cardPool;
    [SerializeField] private AssetReference readyActionPrefab;
    private AddressablesPool<ReadyActionUIBehaviour> readyActionPool;
    
    private Card selectedCard;
    public Card SelectedCard => selectedCard;

    private List<Card> readyCards = new();

    [SerializeField] private Transform drawPileCardsTransform;
    [SerializeField] private Transform discardPileCardsTransform;
    [SerializeField] private Transform handCardsTransform;
    [SerializeField] private Transform readyCardsTransform;
    
    private List<Unit> enemies;
    private List<Unit> actionQueue;
    
    private static GameSystem instance;
    public static GameSystem Instance => instance;

    private bool isInAction;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        unitPool = new AddressablesPool<Unit>(unitPrefab, 10);
        cardPool = new AddressablesPool<Card>(cardPrefab, 10);
        readyActionPool = new AddressablesPool<ReadyActionUIBehaviour>(readyActionPrefab, 10);
        statusEffectPool = new AddressablesPool<StatusEffectUIBehaviour>(statusEffectPrefab, 10);
        Setup();
        enemies = new List<Unit>();
        actionQueue = new List<Unit>();
        isInAction = false;
        player.Setup(UnitID.Main);
        SetupUnit(UnitID.WolfLeader);
    }

    private void Update()
    {
        if (!isInAction)
        {
            if (actionQueue.Count > 0)
            {
                Unit u = actionQueue[0];
                actionQueue.Remove(u);
                u.Execute();
                isInAction = true;
            }
        }
    }

    public ReadyActionUIBehaviour GetReadyActionPrefab(Transform parent)
    {
        return readyActionPool.GetObjectAndActive(parent);
    }

    public StatusEffectUIBehaviour GetStatusEffectUI()
    {
        return statusEffectPool.GetObjectAndActive();
    }

    public void CompletedAction()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            isInAction = false;
        });
    }

    public bool IsInHand(Card card)
    {
        return handCards.Contains(card);
    }
    
    public bool IsInDiscard(Card card)
    {
        return discardCards.Contains(card);
    }

    public void SetupUnit(UnitID unitID)
    {
        Unit u = unitPool.GetObjectAndActive(mainGameTrans);
        u.transform.localPosition = new Vector3(Random.Range(-1, 8f), Random.Range(-4, -2), 0);
        u.Setup(unitID);
        enemies.Add(u);
    }

    public void Setup()
    {
        foreach (CardID cardID in deckIDs)
        {
            Card card = cardPool.GetObjectAndActive(drawPileCardsTransform);
            card.Setup(player, cardID);
            card.transform.localScale = Vector3.one;
            card.transform.localPosition = Vector3.zero;
            decks.Add(card);
        }
        
        drawPileCards = new List<Card>(decks);

        for (int i = drawPileCards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (drawPileCards[i], drawPileCards[randomIndex]) = (drawPileCards[randomIndex], drawPileCards[i]);
        }
        
        Draw(6, false);
    }

    public void ChooseCard(Card card)
    {
        selectedCard = card;
        ShowTarget(true);
    }
    
    public void ToDiscard(Card card, bool decreaseEnemyAction = true)
    {
        handCards.Remove(card);
        discardCards.Add(card);
        card.transform.SetParent(discardPileCardsTransform);
        if (decreaseEnemyAction) DecreaseActionEnemy();
    }

    public void ShowTarget(bool val)
    {
        foreach (var enemy in enemies)
        {
            enemy.ShowTarget(val);
        }
    }

    public void Draw(int number, bool decreaseEnemyAction = true)
    {
        for (int i = 0; i < number; i++)
        {
            if (drawPileCards.Count > 0)
            {
                Card cardToDraw = drawPileCards[drawPileCards.Count - 1];
                AddToHand(cardToDraw);
                drawPileCards.RemoveAt(drawPileCards.Count - 1);
            }
            else
            {
                if (discardCards.Count > 0)
                {
                    ReshuffleDiscardIntoDraw();
                    i--;
                }
                else
                {
                    Debug.LogWarning("No more cards in deck or discard pile!");
                    return;
                }
            }
        }
        if (decreaseEnemyAction) DecreaseActionEnemy();
    }

    public void AddToHand(Card cardToDraw)
    {
        cardToDraw.GetComponent<RectTransform>().SetParent(handCardsTransform);
        cardToDraw.transform.localPosition = Vector3.zero;
        handCards.Add(cardToDraw);
    }

    public void RemoveCardFromHand(Card cardToDraw)
    {
        handCards.Remove(cardToDraw);
    }

    private void ReshuffleDiscardIntoDraw()
    {
        foreach (var card in discardCards)
        {
            card.transform.SetParent(drawPileCardsTransform);
        }
        drawPileCards.AddRange(discardCards);
        discardCards.Clear();
        
        for (int i = drawPileCards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (drawPileCards[i], drawPileCards[randomIndex]) = (drawPileCards[randomIndex], drawPileCards[i]);
        }
        Debug.Log("Discard pile reshuffled into Draw pile.");
    }

    private void DecreaseActionEnemy()
    {
        foreach (var enemy in enemies)
        {
            enemy.DecreaseAction();
        }
    }

    public void AddActionRequest(Unit unit)
    {
        Debug.Log("add action");
        actionQueue.Add(unit);
    }
    
	public void UseCard(Unit enemy)
    {
        selectedCard.Execute(enemy);
        selectedCard = null;
        ShowTarget(false);
    }

    // for player
	public void ReadyCard(Card readyCard, CardLogic targetCard)
	{
        readyCards.Add(readyCard);
        readyCard.CardLogic.AddReadyCard(targetCard);
        readyCard.transform.SetParent(readyCardsTransform);
        ShowTarget(false);
    }
}