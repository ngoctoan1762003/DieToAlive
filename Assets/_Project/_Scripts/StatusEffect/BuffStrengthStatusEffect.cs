using UnityEngine;

public class BuffStrengthStatusEffect : StatusEffect
{
    private StatModifier strengthModifier;
    
    public BuffStrengthStatusEffect(StatusID statusID, Unit unit, int maxStack = -1) : base (statusID, unit, maxStack)
    {
    }

    public override void StartEffect()
    {
        base.StartEffect();

        strengthModifier = new StatModifier(values[0], StatModType.Flat);
        unit.strength.AddModifier(strengthModifier);
    }
    
    public override void EndEffect()
    {
        base.EndEffect();

        unit.strength.RemoveModifier(strengthModifier);
    }
}
