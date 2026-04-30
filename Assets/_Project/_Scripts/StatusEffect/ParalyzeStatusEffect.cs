using UnityEngine;

public class ParalyzeStatusEffect : StatusEffect
{
    public ParalyzeStatusEffect(StatusID statusID, Unit unit, int maxStack = -1) : base (statusID, unit, maxStack)
    {
    }
}
