using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectHolder : MonoBehaviour
{
    [SerializeField] StatusEffect statusEffect;
    [SerializeField] Unit unit;
    public StatusEffect StatusEffect => statusEffect;
    private StatusEffectUIBehaviour uiEffect;
    public StatusEffectUIBehaviour UIEffect => uiEffect;

    private int turnLeft;

    private StatusID[] showStatus = new StatusID[] { };

    private bool isInfinite;
    public bool IsInfinite => isInfinite;

    private bool isTickEffect = false;

    public void Init(StatusEffectUIBehaviour uiEffect, StatusEffect statusEffect, int lifeTurn)
    {
        this.uiEffect = uiEffect;
        this.statusEffect = statusEffect;
        isInfinite = lifeTurn == -1;
        turnLeft = lifeTurn;
        unit = gameObject.GetComponent<Unit>();
        isTickEffect = statusEffect.IsTickEffect;
    }

    public void TakeTurn()
    {
        statusEffect.TakeTurn();
        if (turnLeft == -1) return;
        turnLeft--;
        uiEffect.Setup(statusEffect, turnLeft);
        if (!isInfinite && turnLeft <= 0)
        {
            unit.RemoveStatusEffect(this);
            statusEffect.EndEffect();
        }
    }

    public void ForceEnd()
    {
        unit.RemoveStatusEffect(this);
        statusEffect.EndEffect();
    }
}