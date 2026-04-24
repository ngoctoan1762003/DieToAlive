using System;
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

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Image target;
    [SerializeField] private Button targetBtn;
    [SerializeField] private Image diceTarget;
    [SerializeField] private Button diceTargetBtn;
    [SerializeField] private HealthBarBehaviour healthBarBehaviour;

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

    public void OnSpawn()
    {
    }

    public void OnDead()
    {
    }
}
