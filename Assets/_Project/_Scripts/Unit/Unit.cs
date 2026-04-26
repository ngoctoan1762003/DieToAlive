using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IDamagable, IInPool
{
    private Stat strength;
    private Stat dexterity;
    private Stat defense;
    private Stat magic;

    private Stat maxHP;

    private float _currentHP;

    public float CurrentHP
    {
        get { return _currentHP; }
        set
        {
            _currentHP = Mathf.Max(0, value);
            if (_currentHP <= 0) Dead();
            healthBarBehaviour.SetHP(_currentHP, maxHP.value, 0);
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
    [SerializeField] private CardLogic actionCard;
    public CardLogic ActionCard => actionCard;
    [SerializeField] private CardID[] cardMechanics;
    [SerializeField] private int currentMechanicIndex;
    [SerializeField] private CardID priorityCard;

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
        maxHP = new Stat();
        strength = new Stat();
        dexterity = new Stat();
        defense = new Stat();
        magic = new Stat();
        UnitConfigs config = DataManager.Instance.GetUnitConfig(id);
        maxHP.baseValue = config.maxHP;
        CurrentHP = maxHP.value;
        sprite.sprite = config.sprite;
        cardMechanics = config.mechanics;
        currentMechanicIndex = 0;
        priorityCard = CardID.None;
        actionCard = null;
        if (this != GameSystem.Instance.Player) SetupActionCard();
    }

    public void SetupActionCard()
    {
        CardConfig config = DataManager.Instance.GetCardConfig(cardMechanics[currentMechanicIndex]);
        actionCard = new CardLogic();
        actionCard.Setup(this, null, cardMechanics[currentMechanicIndex]);
        ActionCount = config.actionNeed;
        currentMechanicIndex++;
    }

    public void ShowTarget(bool val)
    {
        target.enabled = val;
        diceTarget.enabled = val;
    }
    
    public void TakeDamage(Unit dealDmgUnit, float damage)
    {
        CurrentHP -= damage;
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }

    public void DecreaseAction()
    {
        ActionCount--;
    }

    public void Execute()
    {
        actionCard.Execute(GameSystem.Instance.Player);
    }

    public void OnSpawn()
    {
    }

    public void OnDead()
    {
    }
}
