using System.Collections.Generic;
using UnityEngine;

public class ParasiteThrowSkillLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);

        for (int i = 0; i < 3; i++)
        {
            if (unit.CurrentHP > 15)
            {
                GameSystem.Instance.SetupUnit(UnitID.SmallParasite);
                unit.TakeDamage(null, 15);
            }
        }
    }
}
