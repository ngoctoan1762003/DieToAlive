using UnityEngine;

public class BleedStatusEffect : StatusEffect
{
    public BleedStatusEffect(StatusID statusID, Unit unit, int maxStack = -1) : base (statusID, unit, maxStack)
    {
    }

    public override void TakeTurn()
    {
        base.TakeTurn();
        
        unit.TakeDamage(null, values[0]);
    }
}
