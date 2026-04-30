using UnityEngine;

public class FragileStatusEffect : StatusEffect
{
    public FragileStatusEffect(StatusID statusID, Unit unit, int maxStack = -1) : base (statusID, unit, maxStack)
    {
    }
}
