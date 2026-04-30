using UnityEngine;

public class WoundStatusEffect : StatusEffect
{
    public WoundStatusEffect(StatusID statusID, Unit unit, int maxStack = -1) : base (statusID, unit, maxStack)
    {
    }
}
