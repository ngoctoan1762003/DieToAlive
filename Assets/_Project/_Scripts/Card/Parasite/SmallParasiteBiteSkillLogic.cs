using System.Collections.Generic;
using UnityEngine;

public class SmallParasiteBiteSkillLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);

        unit.Heal(val);
        GameSystem.Instance.Player.AddStatusEffect(new BleedStatusEffect(StatusID.Bleed, GameSystem.Instance.Player).SetValue(new List<float>(){2}), 2);
    }
}