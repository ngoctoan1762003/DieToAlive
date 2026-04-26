using System;
using System.Collections.Generic;
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
    
    [Header("Cards")]
    [SerializeField] private List<CardID> deckIDs;
    [SerializeField] private List<Card> decks;
    [SerializeField] private List<Card> handCards;
    [SerializeField] private List<Card> drawPileCards;
    [SerializeField] private List<Card> discardCards;
    [SerializeField] private AssetReference cardPrefab;
    private AddressablesPool<Card> cardPool;
    private Card selectedCard;
    public Card SelectedCard => selectedCard;

    [SerializeField] private Transform drawPileCardsTransform;
    [SerializeField] private Transform discardPileCardsTransform;
    [SerializeField] private Transform handCardsTransform;
    
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
                actionQueue[0].Execute();
                actionQueue.RemoveAt(0);
                isInAction = true;
            }
        }
    }

    public bool IsInHand(Card card)
    {
        return handCards.Contains(card);
    }

    public void SetupUnit(UnitID unitID)
    {
        Unit u = unitPool.GetObjectAndActive(mainGameTrans);
        u.transform.localPosition = new Vector3(2.5f, -2.5f, 0);
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
    
    public void ToDiscard(Card card)
    {
        drawPileCards.Remove(card);
        discardCards.Add(card);
        card.transform.SetParent(discardPileCardsTransform);
        DecreaseActionEnemy();
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
                cardToDraw.GetComponent<RectTransform>().SetParent(handCardsTransform);
                cardToDraw.transform.localPosition = Vector3.zero;
                handCards.Add(cardToDraw);
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
        actionQueue.Add(unit);
    }
    
	public void UseCard(Unit enemy)
    {
        selectedCard.Execute(enemy);
        selectedCard = null;
        ShowTarget(false);
    }

	public void ReadyCard()
	{
	}
}