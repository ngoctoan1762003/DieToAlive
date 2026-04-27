using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectHolder : MonoBehaviour
{
    [SerializeField] StatusEffect statusEffect;
    [SerializeField] Unit unit;
    public StatusEffect StatusEffect => statusEffect;

    private Cooldown endEffectCooldown;
    
    private StatusID[] showStatus = new StatusID[] {};
    
    private bool isInfinite;
    public bool IsInfinite => isInfinite;

    private bool isTickEffect = false;
    private Cooldown tickEffectCooldown;

    public void Init(StatusEffect statusEffect, float lifeTurn)
    {
        this.statusEffect = statusEffect;
        isInfinite = lifeTurn == -1;
        endEffectCooldown = new Cooldown(lifeTurn);
        unit = gameObject.GetComponent<Unit>();
        isTickEffect = statusEffect.IsTickEffect;
        tickEffectCooldown = new Cooldown(statusEffect.GetTickDuration());
    }

    public StatusEffectHolder TakeTurn()
    {
        if (isTickEffect && tickEffectCooldown.IsAvailable)
        {
            statusEffect.TakeTurn();
            tickEffectCooldown.ResetClock();
        }
        if (!isInfinite && endEffectCooldown.IsAvailable)
        {
            unit.RemoveStatusEffect(this);
            statusEffect.EndEffect();
            return this;
        }
        return null;
    }

    public void ForceEnd()
    {
        unit.RemoveStatusEffect(this);
        statusEffect.EndEffect();
    }
}
