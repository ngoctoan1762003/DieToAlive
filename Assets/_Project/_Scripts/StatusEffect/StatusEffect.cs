using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect
{
    [SerializeField] protected int maxStack;
    [SerializeField] protected StatusID statusID;
    [SerializeField] protected Unit unit;
    [SerializeField] protected List<Unit> target;
    [SerializeField] protected List<float> values;
    private bool isTickEffect = false;

    public bool IsTickEffect
    {
        get => isTickEffect;
        set => isTickEffect = value;
    }

    private float tickDuration;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="statusID"></param>
    /// <param name="unit">Get status effect unit</param>
    /// <param name="maxStack">-1 for infinity stack</param>
    public StatusEffect(StatusID statusID, Unit unit, int maxStack = -1)
    {
        this.statusID = statusID;
        this.maxStack = maxStack;
        this.unit = unit;
    }

    public Unit GetUnit()
    {
        return unit;
    }

    public StatusEffect SetTargetUnit(List<Unit> unit)
    {
        this.target = unit;
        return this;
    }

    public StatusEffect SetValue(List<float> values)
    {
        this.values = values;
        return this;
    }

    public StatusEffect SetTickValue(float duration)
    {
        isTickEffect = true;
        tickDuration = duration;
        return this;
    }

    public virtual StatusID[] GetConflictEffect()
    {
        return null;
    }

    public StatusID GetID()
    {
        return statusID;
    }
    
    public virtual string GetDescription()
    {
        return "";
    }

    public int MaxStack()
    {
        return maxStack;
    }

    public virtual void StartEffect()
    {
    }

    public virtual void EndEffect()
    {
    }

    public virtual void TakeTurn()
    {
    }

    public List<Unit> GetTargetUnit()
    {
        return target;
    }

    public List<float> GetValues()
    {
        return values;
    }

    public float GetTickDuration()
    {
        return tickDuration;
    }
}
