using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

public class GameSystem : MonoBehaviour
{
    [Header("Units")] [SerializeField] private AssetReference unitPrefab;
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
    [SerializeField] private AssetReference diceBuffPrefab;
    private AddressablesPool<DiceBuffUIBehaviour> diceBuffPool;

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
    
    private bool isInDraw;

    private InventoryItemConfigs currentEquipWeapon;

    private Unit currentEnemyAction;

    private CombatNodeConfig currentCombatConfig;
    private bool combatEnded = false;

    private Vector3 playerStartPos;
    private Transform playerStartParent;

    private int savedHP;
    private int savedMaxHP;
    private bool isReplay = false;

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
        diceBuffPool = new AddressablesPool<DiceBuffUIBehaviour>(diceBuffPrefab, 10);
        Setup();
        enemies = new List<Unit>();
        actionQueue = new List<Unit>();
        isInAction = false;
        //player.Setup(UnitID.Main);
        //SetupUnit(UnitID.WolfLeader);

        playerStartPos = player.transform.localPosition;
        playerStartParent = player.transform.parent;
    }
    public void StartCombat(CombatNodeConfig config)
    {
        UIManager.Instance.ResetUI();
        InventoryManager.Instance.SaveSnapshot();
        ResetToInitialState();
        player.transform.SetParent(playerStartParent);
        player.transform.localPosition = playerStartPos;
        player.transform.localRotation = Quaternion.identity;

        combatEnded = false;
        currentCombatConfig = config;

        ClearCombat();
        ClearAllCards();

        player.Setup(UnitID.Main, false);

        if (!isReplay)
        {
            savedHP = (int)player.CurrentHP;
        }


        if (isReplay)
        {
            player.SetHP(savedHP);
        }

        foreach (var enemyID in config.enemies)
        {
            SetupUnit(enemyID);
        }

        Setup();
    }

    public void ReplayCombat()
    {
        if (currentCombatConfig == null) return;

        Debug.Log("Replay combat");

        isReplay = true;

        InventoryManager.Instance.RestoreSnapshot();
        ResetToInitialState();
        StartCombat(currentCombatConfig);

        isReplay = false;
    }

    public void ClearCombat()
    {
        foreach (var enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
        enemies.Clear();

        actionQueue.Clear();

        // clear cards
        foreach (var c in handCards) c.gameObject.SetActive(false);
        foreach (var c in drawPileCards) c.gameObject.SetActive(false);
        foreach (var c in discardCards) c.gameObject.SetActive(false);

        handCards.Clear();
        drawPileCards.Clear();
        discardCards.Clear();
        readyCards.Clear();
    }

    private void ResetUI()
    {
        UIManager.Instance.HideInventoryNeed();
        ShowTarget(false);
        selectedCard = null;
    }

    public void ResetToInitialState()
    {
        combatEnded = false;
        isInAction = false;
        isInDraw = false;

        actionQueue.Clear();
        currentEnemyAction = null;
        selectedCard = null;

        player.gameObject.SetActive(true);
        player.ResetUnit();

        player.transform.SetParent(playerStartParent);
        player.transform.localPosition = playerStartPos;
        player.transform.localRotation = Quaternion.identity;

        foreach (var enemy in enemies)
        {
            enemy.ResetUnit();
            enemy.gameObject.SetActive(false);
        }
        enemies.Clear();

        ClearAllCards();

        currentEquipWeapon = null;

        ResetUI();
    }

    private void ClearAllCards()
    {
        List<Card> allCards = new();
        allCards.AddRange(handCards);
        allCards.AddRange(drawPileCards);
        allCards.AddRange(discardCards);
        allCards.AddRange(readyCards);

        foreach (var c in allCards)
        {
            c.gameObject.SetActive(false);
            c.transform.SetParent(null);
        }

        handCards.Clear();
        drawPileCards.Clear();
        discardCards.Clear();
        readyCards.Clear();
    }


    public void OnUnitDead(Unit unit)
    {
        if (unit == player)
        {
            if (enemies.Any(e => e.gameObject.activeSelf))
            {
                EndCombat(false);
            }
            return;
        }

        enemies.Remove(unit);

        if (!enemies.Any(e => e.gameObject.activeSelf))
        {
            EndCombat(true);
        }
    }
    private void EndCombat(bool isWin)
    {
        if (combatEnded) return;
        combatEnded = true;

        isInAction = true;

        if (isWin)
        {
            CombatResultUI.Instance.Win();
            Debug.Log("Win");
        }
        else
        {
            CombatResultUI.Instance.Lose();
            Debug.Log("Lose");
        }

        CombatResultUI.Instance.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!isInAction)
        {
            if (actionQueue.Count > 0)
            {
                currentEnemyAction = actionQueue[0];
                actionQueue.Remove(currentEnemyAction);
                currentEnemyAction.onStartAction?.Invoke();
                currentEnemyAction.Execute();
                isInAction = true;
            }
        }
    }

    public DiceBuffUIBehaviour GetDiceBuff()
    {
        return diceBuffPool.GetObjectAndActive();
    }
    
    public void EquipWeapon(InventoryItem item)
    {
        currentEquipWeapon = item.config;
        ListCardConfigs config = DataManager.Instance.GetCardListByWeapon(item.config.weaponID);
        foreach (var c in config.configs)
        {
            if (c.cardID.ToString().Contains("Retrieve")) continue;
            AddToDrawPile(c.cardID, item);
        }

        List<Card> newCards = drawPileCards.Where(c => c.Source == item).ToList();
        Draw(2, false);
        InventoryManager.Instance.RemoveItem(currentEquipWeapon);
    }

    public void UseItem(InventoryItemConfigs config)
    {
        switch (config.toolID)
        {
            case ToolID.Bandage:
                player.RemoveStatusEffectByID(StatusID.Bleed);
                break;
            case ToolID.Bomb:
                foreach (var enemy in enemies.ToList())
                {
                    enemy.TakeDamage(null, 10);
                }
                break;
            case ToolID.Cursed:
                foreach (var enemy in enemies.ToList())
                {
                    enemy.AddStatusEffect(new FragileStatusEffect(StatusID.Fragile, enemy).SetValue(new List<float>() { 1 }), 5);
                }
                break;
            case ToolID.Energy:
                player.AddStatusEffect(new BuffStrengthStatusEffect(StatusID.BuffStrength, player).SetValue(new List<float>(){1}), 5);
                break;
            case ToolID.HealPotion:
                player.Heal(10);
                break;
            case ToolID.Needle:
                player.RemoveStatusEffectByID(StatusID.Wound);
                break;
            case ToolID.Trap:
                player.AddStatusEffect(new StunStatusEffect(StatusID.Stun, player), 1);
                break;
        }
        InventoryManager.Instance.RemoveItem(config);
    }

    public void AddToDrawPile(CardID cardID, InventoryItem item)
    {
        Card card = cardPool.GetObjectAndActive(drawPileCardsTransform);
        card.Setup(player, cardID, item);
        card.transform.localScale = Vector3.one;
        card.transform.localPosition = Vector3.zero;
        drawPileCards.Add(card);
        CalculateDrawTransform();
    }

    public void ThrowWeapon()
    {
        for (int i = drawPileCards.Count - 1; i >= 0; i--)
        {
            var drawPileCard = drawPileCards[i];
            if (drawPileCard.Source != null && drawPileCard.Source.config == currentEquipWeapon)
            {
                drawPileCards.Remove(drawPileCard);
                drawPileCard.gameObject.SetActive(false);
                drawPileCard.transform.SetParent(null);
            }
        }
        for (int i = handCards.Count - 1; i >= 0; i--)
        {
            var handCard = handCards[i];
            if (handCard.Source != null && handCard.Source.config == currentEquipWeapon)
            {
                handCards.Remove(handCard);
                handCard.gameObject.SetActive(false);
                handCard.transform.SetParent(null);
            }
        }
        for (int i = discardCards.Count - 1; i >= 0; i--)
        {
            var discardCard = discardCards[i];
            if (discardCard.Source != null && discardCard.Source.config == currentEquipWeapon)
            {
                discardCards.Remove(discardCard);
                discardCard.gameObject.SetActive(false);
                discardCard.transform.SetParent(null);
            }
        }
        for (int i = readyCards.Count - 1; i >= 0; i--)
        {
            var readyCard = readyCards[i];
            if (readyCard.Source.config == currentEquipWeapon)
            {
                readyCards.Remove(readyCard);
                readyCard.gameObject.SetActive(false);
                readyCard.CardLogic.ClashCard.SetClashCard(null);
                readyCard.CardLogic.SetClashCard(null);
                readyCard.transform.SetParent(null);
            }
        }
        
        CalculateDiscardTransform();
        CalculateDrawTransform();
        CalculateHandTransform();
        CalculateReadyTransform();
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
        DOVirtual.DelayedCall(1f, () =>
        {
            isInAction = false;
            currentEnemyAction.onEndAction?.Invoke();
            currentEnemyAction = null;
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

    private void Setup()
    {
        drawPileCards = new List<Card>();

        foreach (CardID cardID in deckIDs)
        {
            AddToDrawPile(cardID, null);
        }

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
        if (card.CardLogic.CardConfig.cardType == CardType.Defensive ||
            card.CardLogic.CardConfig.cardType == CardType.Offensive ||
            card.CardLogic.CardConfig.cardType == CardType.ThrowWeapon)
        {
            UIManager.Instance.HideInventoryNeed();
            ShowTarget(true);
        }
        else if (card.CardLogic.CardConfig.cardType == CardType.UseItem)
        {
            UIManager.Instance.ShowInventoryNeed(InventoryItemType.Tool);
        }
        else if (card.CardLogic.CardConfig.cardType == CardType.UseWeapon)
        {
            UIManager.Instance.ShowInventoryNeed(InventoryItemType.Weapon);
        }
        else if (card.CardLogic.CardConfig.cardType == CardType.UnInterruptable)
        {
            card.Execute(null);
        }
    }

    private void CalculateDrawTransform()
    {
        Vector2 left = new Vector2(0f, 0.5f);
        for (int i = 0; i < drawPileCardsTransform.childCount; i++)
        {
            RectTransform trans = drawPileCardsTransform.GetChild(i).GetComponent<RectTransform>();
            trans.pivot = left;
            trans.DOAnchorPos(new Vector3(-380 + 10 * i, 0, 0), 0.5f);
        }
    }
    
    public void CalculateHandTransform()
    {
        float start = -handCardsTransform.childCount * 50 / 2;
        for (int i = 0; i < handCardsTransform.childCount; i++)
        {
            RectTransform trans = handCardsTransform.GetChild(i).GetComponent<RectTransform>();
            trans.pivot = Vector2.one * 0.5f;
            trans.DOAnchorPos(new Vector3(start + 50 * i, 0, 0), 0.5f);
            trans.GetComponent<Card>().SetOrigin(new Vector3(start + 50 * i, 0, 0));
        }
    }
    
    private void CalculateReadyTransform()
    {
        float start = -readyCardsTransform.childCount * 50 / 2;
        for (int i = 0; i < readyCardsTransform.childCount; i++)
        {
            RectTransform trans = readyCardsTransform.GetChild(i).GetComponent<RectTransform>();
            trans.pivot = Vector2.one * 0.5f;
            trans.DOAnchorPos(new Vector3(start + 50 * i, 0, 0), 0.5f);
        }
    }

    private void CalculateDiscardTransform()
    {
        Vector2 right = new Vector2(1f, 0.5f);
        for (int i = discardPileCardsTransform.childCount - 1; i >= 0; i--)
        {
            RectTransform trans = discardPileCardsTransform.GetChild(i).GetComponent<RectTransform>();
            trans.pivot = right;
            trans.DOAnchorPos(new Vector3(355 - 10 * i, 0, 0), 0.5f);
        }
    }

    public void ToDiscard(Card card, bool decreaseEnemyAction = true)
    {
        handCards.Remove(card);
        discardCards.Add(card);
        card.transform.SetParent(discardPileCardsTransform);
        CalculateHandTransform();
        CalculateDiscardTransform();
        if (decreaseEnemyAction) DecreaseActionEnemy();
    }

    public void ShowTarget(bool val)
    {
        foreach (var enemy in enemies)
        {
            enemy.ShowTarget(val,
                selectedCard != null && 
                selectedCard.CardLogic.CardConfig.cardType is CardType.Defensive);
        }
    }

    public void Draw(int number, bool decreaseEnemyAction = true)
    {
        if (isInDraw) return;
        isInDraw = true;
        DOVirtual.DelayedCall(0.5f, () => isInDraw = false);
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

        CalculateDrawTransform();
        if (decreaseEnemyAction) DOVirtual.DelayedCall(0.5f, DecreaseActionEnemy);
    }

    public void Draw(Card card)
    {
        Card cardToDraw = drawPileCards[drawPileCards.Count - 1];
        AddToHand(cardToDraw);
        drawPileCards.RemoveAt(drawPileCards.Count - 1);
    }

    public void AddToHand(Card cardToDraw)
    {
        cardToDraw.GetComponent<RectTransform>().SetParent(handCardsTransform);
        handCards.Add(cardToDraw);
        CalculateHandTransform();
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

        CalculateDrawTransform();
        CalculateDiscardTransform();
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
        CalculateHandTransform();
        CalculateReadyTransform();
        ShowTarget(false);
    }
}