using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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

    [SerializeField] private Transform statusEffectTrans;

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
    public void SetHP(float hp)
    {
        _currentHP = Mathf.Clamp(hp, 0, maxHP.value);
        healthBarBehaviour.SetHP(_currentHP, maxHP.value, 0);
        onChangeStat?.Invoke();
    }

    private int actionCount;

    public int ActionCount
    {
        get { return actionCount; }
        set
        {
            actionCount = value;
            actionCountText.text = actionCount.ToString();
            if (actionCount <= 0 && actionCard != null)
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
    [SerializeField] private Transform readyActionTransform;
    [SerializeField] private DiceUIBehaviour diceUIBehaviour;
    [SerializeField] private Image currentActionIcon;
    [SerializeField] private SpriteRenderer clash;
    [SerializeField] private SpriteRenderer loseClash;
    private bool canExceedCapMaxHP = true;
    
    // Card
    private CardLogic actionCard;
    public CardLogic ActionCard => actionCard;
    private CardID[] cardMechanics;
    private int currentMechanicIndex;
    private CardID priorityCard;
    private List<CardLogic> readyCards = new();
    public List<CardLogic> ReadyCards => readyCards;

    // Passive Assign Activation
    public Action onEndAction;
    public Action onStartAction;
    public Action onEvadeSuccess;
    public Action onBlockSuccess;
    public Action onClash;
    public Action onChangeStat;
    public Action onTakeDamage;
    private UnitID unitID;
    public UnitID UnitID => unitID;

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
        diceTargetBtn.onClick.AddListener(() =>
        {
            if (GameSystem.Instance.SelectedCard == null) return;
            if (diceTarget.enabled == false) return;
            GameSystem.Instance.ReadyCard(GameSystem.Instance.SelectedCard, actionCard);
        });
    }

    public List<int> GetDiceBuff()
    {
        List<int> buffs = new();
        foreach (var statusEffectHolder in statusEffectHolders.Where(s => s.StatusEffect.GetID() == StatusID.BuffStrength))
        {
            buffs.Add((int)statusEffectHolder.StatusEffect.GetValues()[0]);
        }
        foreach (var statusEffectHolder in statusEffectHolders.Where(s => s.StatusEffect.GetID() == StatusID.Paralyze))
        {
            buffs.Add((int)-statusEffectHolder.StatusEffect.GetValues()[0]);
        }

        return buffs;
    }

    public void ShowClash(bool val)
    {
        clash.gameObject.SetActive(val);
    }
    
    public void ShowLoseClash()
    {
        loseClash.gameObject.SetActive(true);
        DOVirtual.DelayedCall(0.5f, () =>
        {
            loseClash.gameObject.SetActive(false);
        });
    }

    public void Setup(UnitID id, bool resetHP = true)
    {
        ResetCardAction();
        ResetPassiveTrigger();

        this.unitID = id;
        UnitConfigs config = DataManager.Instance.GetUnitConfig(id);
        SetupStat(config, resetHP);
        SetupPassive(config);
        onStartAction += CountdownDOTStatusEffect;

        if (this != GameSystem.Instance.Player) SetupActionCard();
    }

    private void SetupStat(UnitConfigs config, bool resetHP)
    {
        maxHP = new Stat();
        strength = new Stat();
        dexterity = new Stat();
        defense = new Stat();
        magic = new Stat();

        maxHP.baseValue = config.maxHP;

        if (resetHP || _currentHP <= 0)
        {
            CurrentHP = maxHP.value;
        }
        else
        {
            SetHP(_currentHP);
        }

        sprite.sprite = config.sprite;
        cardMechanics = config.mechanics;
        canExceedCapMaxHP = config.canExceedCapMaxHP;
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
        onBlockSuccess = null;
        onClash = null;
        onChangeStat = null;
        onTakeDamage = null;
    }

    private void ResetCardAction()
    {
        currentMechanicIndex = 0;
        priorityCard = CardID.None;
        actionCard = null;
    }

    public void Heal(float amount)
    {
        CurrentHP += amount;
        if (!canExceedCapMaxHP)
        {
            CurrentHP = Mathf.Clamp(CurrentHP, 0, maxHP.value);
        }
        UIManager.Instance.ShowDamage("+" + amount, transform.position);
    }

    public void GlowActionCard()
    {
        currentActionIcon.material = DataManager.Instance.GlowUIMat;
    }
    
    public void HideGlowActionCard()
    {
        currentActionIcon.material = null;
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

        if (cardMechanics.Length == 0)
        {
            currentActionIcon.transform.parent.gameObject.SetActive(false);
            return;
        }
        else
        {
            currentActionIcon.transform.parent.gameObject.SetActive(true);
        }
        CardConfig config = DataManager.Instance.GetCardConfig(cardMechanics[currentMechanicIndex]);
        actionCard = DataManager.Instance.GetCardLogic(cardMechanics[currentMechanicIndex]);
        actionCard.Setup(this, null, cardMechanics[currentMechanicIndex]);
        ActionCount = config.actionNeed;
        currentMechanicIndex++;
        if (currentMechanicIndex >= cardMechanics.Length) currentMechanicIndex = 0;
        currentActionIcon.sprite = DataManager.Instance.GetActionIcon(config);
    }

    public void ShowTarget(bool val, bool showDiceOnly = false)
    {
        if (!showDiceOnly) target.enabled = val;
        diceTarget.enabled = val;
    }

    public void ShowDiceAnim(string name, int targetVal, List<int> buffs, Action<int> onComplete)
    {
        diceUIBehaviour.gameObject.SetActive(true);
        diceUIBehaviour.ShowDiceAnim(name, targetVal, buffs, onComplete);
    }

    public float CalculateDamage(float damage)
    {
        int weakenStack = (int)statusEffectHolders
            .Where(s => s.StatusEffect.GetID() == StatusID.Weaken)
            .Sum(s => s.StatusEffect.GetValues()[0]);
        damage -= damage * weakenStack / 10;
        
        CountdownBuffDamageStatusEffect();
        return damage;
    }

    public void TakeDamage(Unit dealDmgUnit, float damage)
    {
        int fragileStack = (int)statusEffectHolders
            .Where(s => s.StatusEffect.GetID() == StatusID.Fragile)
            .Sum(s => s.StatusEffect.GetValues()[0]);
        damage += damage * fragileStack / 10;
        
        int protectionStack = (int)statusEffectHolders
            .Where(s => s.StatusEffect.GetID() == StatusID.Protection)
            .Sum(s => s.StatusEffect.GetValues()[0]);
        damage -= damage * protectionStack / 10;
        
        CurrentHP -= Mathf.CeilToInt(damage);
        UIManager.Instance.ShowDamage(damage.ToString(), transform.position);
        CountdownTakeDamageStatusEffect();
        onTakeDamage?.Invoke();
    }

    public void Dead()
    {
        gameObject.SetActive(false);
        GameSystem.Instance.OnUnitDead(this);
    }

    public void DecreaseAction()
    {
        var status = statusEffectHolders.FirstOrDefault(s => s.StatusEffect.GetID() == StatusID.Stun);
        if (status != null)
        {
            status.TakeTurn();
            return;
        }
        ActionCount--;

        if (ActionCount < 0)
            ActionCount = 0;
    }

    // for enemies
    public void ReadyCard(CardLogic card)
    {
        readyCards.Add(card);
        ReadyActionUIBehaviour readyAction = GameSystem.Instance.GetReadyActionPrefab(readyActionTransform);
        readyAction.Setup(card.CardConfig, this);
        readyAction.transform.localScale = Vector3.one;
        card.onUsed += readyAction.Use;
    }

    public void RemoveReadyCard(CardLogic logic)
    {
        readyCards.Remove(logic);
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
        StatusEffectHolder statusEffectHolder = gameObject.AddComponent<StatusEffectHolder>();
        StatusEffectUIBehaviour statusEffectUIBehaviour = GameSystem.Instance.GetStatusEffectUI();
        statusEffectUIBehaviour.Setup(statusEffect, lifeTurn);
        statusEffectUIBehaviour.transform.SetParent(statusEffectTrans);
        statusEffectUIBehaviour.transform.localScale = Vector3.one;
        
        switch (statusEffect.GetID())
        {
            case StatusID.Poison:
                statusEffectHolder.Init(statusEffectUIBehaviour,
                    new StatusEffect(statusEffect.GetID(), this, statusEffect.MaxStack()),
                    lifeTurn);
                break;
            case StatusID.Bleed:
                statusEffectHolder.Init(statusEffectUIBehaviour,
                    new BleedStatusEffect(statusEffect.GetID(), this, statusEffect.MaxStack()).SetValue(statusEffect.GetValues()),
                    lifeTurn);
                break;
            case StatusID.BuffStrength:
                statusEffectHolder.Init(statusEffectUIBehaviour,
                    new BuffStrengthStatusEffect(statusEffect.GetID(), this, statusEffect.MaxStack()).SetValue(statusEffect.GetValues()),
                    lifeTurn);
                break;
            case StatusID.Paralyze:
                statusEffectHolder.Init(statusEffectUIBehaviour,
                    new ParalyzeStatusEffect(statusEffect.GetID(), this, statusEffect.MaxStack()).SetValue(statusEffect.GetValues()),
                    lifeTurn);
                break;
            case StatusID.Protection:
                statusEffectHolder.Init(statusEffectUIBehaviour,
                    new ProtectionStatusEffect(statusEffect.GetID(), this, statusEffect.MaxStack()).SetValue(statusEffect.GetValues()),
                    lifeTurn);
                break;
            case StatusID.Stun:
                statusEffectHolder.Init(statusEffectUIBehaviour,
                    new StunStatusEffect(statusEffect.GetID(), this, statusEffect.MaxStack()).SetValue(statusEffect.GetValues()),
                    lifeTurn);
                break;
            case StatusID.Fragile:
                statusEffectHolder.Init(statusEffectUIBehaviour,
                    new FragileStatusEffect(statusEffect.GetID(), this, statusEffect.MaxStack()).SetValue(statusEffect.GetValues()),
                    lifeTurn);
                break;
            case StatusID.Weaken:
                statusEffectHolder.Init(statusEffectUIBehaviour,
                    new WeakenStatusEffect(statusEffect.GetID(), this, statusEffect.MaxStack()).SetValue(statusEffect.GetValues()),
                    lifeTurn);
                break;
            case StatusID.Wound:
                statusEffectHolder.Init(statusEffectUIBehaviour,
                    new WoundStatusEffect(statusEffect.GetID(), this, statusEffect.MaxStack()).SetValue(statusEffect.GetValues()),
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
        statusEffectHolder.UIEffect.gameObject.SetActive(false); 
        Destroy(statusEffectHolder);
    }
    
    public void RemoveStatusEffectByID(StatusID id)
    {
        for (int i = statusEffectHolders.Count - 1; i >= 0; i--)
        {
            if (statusEffectHolders[i].StatusEffect.GetID() != id) continue;
            var statusEffectHolder = statusEffectHolders[i];
            statusEffectHolders.Remove(statusEffectHolder);
            statusEffectHolder.StatusEffect.EndEffect();
            statusEffectHolder.UIEffect.gameObject.SetActive(false); 
            Destroy(statusEffectHolder);
        }
    }

    public void CountdownClashStatusEffect()
    {
        for (int i = statusEffectHolders.Count - 1; i >= 0; i--)
        {
            if (statusEffectHolders[i].StatusEffect.GetID() is StatusID.BuffStrength or StatusID.Paralyze) statusEffectHolders[i].TakeTurn();
        }
    }
    
    public void CountdownDOTStatusEffect()
    {
        for (int i = statusEffectHolders.Count - 1; i >= 0; i--)
        {
            if (statusEffectHolders[i].StatusEffect.GetID() is StatusID.Bleed or StatusID.Burn or StatusID.Poison or StatusID.Wound) statusEffectHolders[i].TakeTurn();
        }
    }    
        
    public void CountdownBuffDamageStatusEffect()
    {
        for (int i = statusEffectHolders.Count - 1; i >= 0; i--)
        {
            if (statusEffectHolders[i].StatusEffect.GetID() is StatusID.Weaken) statusEffectHolders[i].TakeTurn();
        }
    }
    
    public void CountdownTakeDamageStatusEffect()
    {
        for (int i = statusEffectHolders.Count - 1; i >= 0; i--)
        {
            if (statusEffectHolders[i].StatusEffect.GetID() is StatusID.Fragile or StatusID.Protection) statusEffectHolders[i].TakeTurn();
        }
    }

    public void SetPriorityCard(CardID cardID)
    {
        priorityCard = cardID;
    }

    public void Push(Vector3 direction, float distance, float duration = 0.25f)
    {
        Vector3 destination = transform.position + (direction.normalized * distance);
        destination.x = Mathf.Clamp(destination.x, -10f, 10f);
        transform.DOMove(destination, duration)
            .SetEase(Ease.OutQuad);
    }

    public void ResetUnit()
    {
        for (int i = statusEffectHolders.Count - 1; i >= 0; i--)
        {
            RemoveStatusEffect(statusEffectHolders[i]);
        }

        ResetPassiveTrigger();

        ResetCardAction();

        readyCards.Clear();
        for (int i = 0; i < readyActionTransform.childCount; i++)
        {
            readyActionTransform.GetChild(i).gameObject.SetActive(false);
        }
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