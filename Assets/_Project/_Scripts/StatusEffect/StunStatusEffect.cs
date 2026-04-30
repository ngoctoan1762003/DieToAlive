using UnityEngine;

public class StunStatusEffect : StatusEffect
{
    public StunStatusEffect(StatusID statusID, Unit unit, int maxStack = -1) : base (statusID, unit, maxStack)
    {
    }
}
