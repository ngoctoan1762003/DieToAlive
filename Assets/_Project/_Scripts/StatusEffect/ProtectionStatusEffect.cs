using UnityEngine;

public class ProtectionStatusEffect : StatusEffect
{
    public ProtectionStatusEffect(StatusID statusID, Unit unit, int maxStack = -1) : base (statusID, unit, maxStack)
    {
    }
}
