using UnityEngine;

public class WeakenStatusEffect : StatusEffect
{
    public WeakenStatusEffect(StatusID statusID, Unit unit, int maxStack = -1) : base (statusID, unit, maxStack)
    {
    }
}
