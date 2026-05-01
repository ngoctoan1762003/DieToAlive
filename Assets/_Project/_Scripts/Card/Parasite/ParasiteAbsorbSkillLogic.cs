using System.Collections.Generic;
using UnityEngine;

public class ParasiteAbsorbSkillLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);

        for (int i = GameSystem.Instance.Enemies.Count - 1; i >= 0; i--)
        {
            if (GameSystem.Instance.Enemies[i].UnitID == UnitID.SmallParasite)
            {
                GameSystem.Instance.Enemies[i].Dead();
                unit.Heal(GameSystem.Instance.Enemies[i].CurrentHP);
            }
        }
    }
}
