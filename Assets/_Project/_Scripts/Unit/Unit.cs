using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IDamagable, IInPool
{
    public Stat strength;
    public Stat dexterity;
    public Stat defense;
    public Stat magic;

    public Stat maxHP;

    private float _currentHP;

    public float CurrentHP
    {
        get { return _currentHP; }
        set
        {
            _currentHP = Mathf.Max(0, value);
            if (_currentHP <= 0) Dead();
            healthBarBehaviour.SetHP(_currentHP, maxHP.value, 0);
            onChangeStat?.Invoke();
        }
    }

    private int actionCount;

    public int ActionCount
    {
        get { return actionCount; }
        set
        {
            actionCount = value;
            actionCountText.text = actionCount.ToString();
            if (actionCount == 0)
            {
                GameSystem.Instance.AddActionRequest(this);
            }
        }
    }

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Image target;
    [SerializeField] private Button targetBtn;
    [SerializeField] private Image diceTarget;
    [SerializeField] private Button diceTargetBtn;
    [SerializeField] private TextMeshProUGUI actionCountText;
    [SerializeField] private HealthBarBehaviour healthBarBehaviour;

    // Card
    private CardLogic actionCard;
    public CardLogic ActionCard => actionCard;
    private CardID[] cardMechanics;
    private int currentMechanicIndex;
    private CardID priorityCard;
    private List<CardLogic> readyCards = new();

    // Passive Assign Activation
    public Action onEndAction;
    public Action onStartAction;
    public Action onEvadeSuccess;
    public Action onClash;
    public Action onChangeStat;

    private List<StatusEffectHolder> statusEffectHolders = new();
    public List<StatusEffectHolder> GetListStatusEffectHolders => statusEffectHolders;

    private void Start()
    {
        targetBtn.onClick.AddListener(() =>
        {
            if (GameSystem.Instance.SelectedCard == null) return;
            if (target.enabled == false) return;
            GameSystem.Instance.UseCard(this);
        });
    }

    public void Setup(UnitID id)
    {
        ResetCardAction();
        ResetPassiveTrigger();

        UnitConfigs config = DataManager.Instance.GetUnitConfig(id);
        SetupStat(config);
        SetupPassive(config);

        if (this != GameSystem.Instance.Player) SetupActionCard();
    }

    private void SetupStat(UnitConfigs config)
    {
        maxHP = new Stat();
        strength = new Stat();
        dexterity = new Stat();
        defense = new Stat();
        magic = new Stat();
        maxHP.baseValue = config.maxHP;
        CurrentHP = maxHP.value;
        sprite.sprite = config.sprite;
        cardMechanics = config.mechanics;
    }

    private void SetupPassive(UnitConfigs config)
    {
        PassiveLogic passive = new PassiveLogic();
        foreach (var passiveID in config.passiveIDs)
        {
            switch (passiveID)
            {
                case PassiveID.WolfEquipBlock:
                    passive = new WolfEquipBlockLogic();
                    break;
                case PassiveID.WolfEquipEvade:
                    passive = new WolfEquipEvadeLogic();
                    break;
                case PassiveID.WolfEvadeSuccess:
                    passive = new WolfEvadeSuccessLogic();
                    break;
                case PassiveID.WolfLeaderRoar:
                    passive = new WolfLeaderRoarLogic();
                    break;
                case PassiveID.WolfRoar:
                    passive = new WolfRoarLogic();
                    break;
            }
            
            passive.Setup(this);
        }
        
    }

    private void ResetPassiveTrigger()
    {
        onEndAction = null;
        onStartAction = null;
        onEvadeSuccess = null;
        onClash = null;
    }

    private void ResetCardAction()
    {
        currentMechanicIndex = 0;
        priorityCard = CardID.None;
        actionCard = null;
    }

    public void SetupActionCard()
    {
        if (priorityCard != CardID.None)
        {
            CardConfig priorityConfig = DataManager.Instance.GetCardConfig(priorityCard);
            actionCard = DataManager.Instance.GetCardLogic(priorityCard);
            actionCard.Setup(this, null, priorityCard);
            ActionCount = priorityConfig.actionNeed;
            priorityCard = CardID.None;
            return;
        }
        CardConfig config = DataManager.Instance.GetCardConfig(cardMechanics[currentMechanicIndex]);
        actionCard = DataManager.Instance.GetCardLogic(cardMechanics[currentMechanicIndex]);
        actionCard.Setup(this, null, cardMechanics[currentMechanicIndex]);
        ActionCount = config.actionNeed;
        currentMechanicIndex++;
        if (currentMechanicIndex >= cardMechanics.Length) currentMechanicIndex = 0;
        onStartAction?.Invoke();
    }

    public void ShowTarget(bool val)
    {
        target.enabled = val;
        diceTarget.enabled = val;
    }

    public void TakeDamage(Unit dealDmgUnit, float damage)
    {
        CurrentHP -= damage;
        UIManager.Instance.ShowDamage(damage, transform.position);
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }

    public void DecreaseAction()
    {
        ActionCount--;
    }

    public void ReadyCard(CardLogic card)
    {
        readyCards.Add(card);
    }

    public void Execute()
    {
        actionCard.Execute(GameSystem.Instance.Player);
    }

    public void AddStatusEffect(StatusEffect statusEffect, int lifeTurn)
    {
        if (CheckCanAdd(statusEffect.GetID(), statusEffect.MaxStack()))
        {
            AddStatusEffectHolder(statusEffect, lifeTurn);
        }
    }

    private void AddStatusEffectHolder(StatusEffect statusEffect, int lifeTurn)
    {
        var statusEffectHolder = gameObject.AddComponent<StatusEffectHolder>();

        switch (statusEffect.GetID())
        {
            case StatusID.Poison:
                statusEffectHolder.Init(
                    new StatusEffect(statusEffect.GetID(), this, statusEffect.MaxStack()),
                    lifeTurn);
                break;
            case StatusID.BuffStrength:
                statusEffectHolder.Init(
                    new BuffStrengthStatusEffect(statusEffect.GetID(), this, statusEffect.MaxStack()).SetValue(statusEffect.GetValues()),
                    lifeTurn);
                break;
        }
        
        statusEffectHolders.Add(statusEffectHolder);
        statusEffectHolder.StatusEffect.StartEffect();
    }

    private bool CheckCanAdd(StatusID statusID, int maxStack)
    {
        if (maxStack == -1) return true;

        return statusEffectHolders.Where(s => s.StatusEffect.GetID() == statusID).Count() < maxStack;
    }
    
    public void RemoveStatusEffect(StatusEffectHolder statusEffectHolder)
    {
        statusEffectHolders.Remove(statusEffectHolder);
        statusEffectHolder.StatusEffect.EndEffect();
        Destroy(statusEffectHolder);
    }

    public void SetPriorityCard(CardID cardID)
    {
        priorityCard = cardID;
    }

    public void Highlight()
    {
        sprite.sortingLayerName = "UI";
        sprite.sortingOrder = 0;
    }
    
    public void DeHighlight()
    {
        sprite.sortingLayerName = "Default";
        sprite.sortingOrder = 10;
    }

    public void OnSpawn()
    {
    }

    public void OnDead()
    {
    }
}