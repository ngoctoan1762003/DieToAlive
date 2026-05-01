using System.Collections.Generic;
using UnityEngine;

public class ParasiteEvolveSkillLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);

        for (int i = GameSystem.Instance.Enemies.Count - 1; i >= 0; i--)
        {
            if (GameSystem.Instance.Enemies[i].UnitID == UnitID.Corpse)
            {
                GameSystem.Instance.Enemies[i].Dead();
                unit.maxHP.AddModifier(new StatModifier(20, StatModType.PercentAdd));
                unit.Heal(unit.maxHP.value / 5);
                GameSystem.Instance.Player.AddStatusEffect(new BuffStrengthStatusEffect(StatusID.BuffStrength, GameSystem.Instance.Player).SetValue(new List<float>(){ 1 }), -1);
                return;
            }
        }
    }
}
