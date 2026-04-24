using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class Stat
{
    public float baseValue;
    public Action onChange;

    public virtual float value
    {
        get
        {
            if (isDirty || baseValue != lastBaseValue)
            {
                lastBaseValue = baseValue;
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        }
    }

    protected bool isDirty = true;
    protected float _value;
    protected float lastBaseValue = float.MinValue;

    protected readonly List<StatModifier> statModifiers;
    //public readonly ReadOnlyCollection<StatModifiers> StatModifiers;

    public Stat(float value) : this()
    {
        baseValue = value;
    }

    public Stat()
    {
        statModifiers = new();
        //StatModifiers = statModifiers.AsReadOnly();
    }

    public virtual void AddModifier(StatModifier mod)
    {
        statModifiers.Add(mod);
        isDirty = true;
        statModifiers.Sort(CompareModifierOrder);
        onChange?.Invoke();
    }

    protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.order > b.order) return 1;
        else if (a.order < b.order) return -1;
        return 0;
    }

    public virtual bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;
        int i = statModifiers.Count - 1;

        while (i >= 0)
        {
            if (statModifiers[i].source == source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
                Debug.Log("remove at " + i);
                Debug.Log(source);
            }
            i--;
        }
        onChange?.Invoke();

        return didRemove;
    }

    public bool RemoveModifier(StatModifier mod)
    {
        if (mod == null) return false;
        foreach (var modifier in statModifiers)
        {
            if (modifier.value == mod.value && modifier.type == mod.type)
            {
                statModifiers.Remove(modifier);
                isDirty = true;
                return true;
            }
        }
        onChange?.Invoke();

        return false;
    }

    protected float CalculateFinalValue()
    {
        float finalValue = baseValue;
        float sumPercentAdd = 0;

        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];
            if (mod.type == StatModType.Flat)
            {
                finalValue += mod.value;
            }
            else if (mod.type == StatModType.PercentAdd)
            {
                sumPercentAdd += mod.value;

                if (i + 1 >= statModifiers.Count || statModifiers[i + 1].type != StatModType.PercentAdd)
                {
                    finalValue *= 1 + sumPercentAdd / 100;
                    sumPercentAdd = 0;
                }
            }
            else if (mod.type == StatModType.PercentMul)
            {
                finalValue *= 1 + mod.value / 100;
            }
        }

        return finalValue;
    }
}
