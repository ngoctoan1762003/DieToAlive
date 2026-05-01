using System.Collections.Generic;
using UnityEngine;

public class ParasiteRollSkillLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);
        GameSystem.Instance.Player.AddStatusEffect(new ParalyzeStatusEffect(StatusID.Paralyze, GameSystem.Instance.Player).SetValue(new List<float>(){ 1 }), 5);
        GameSystem.Instance.Player.AddStatusEffect(new WoundStatusEffect(StatusID.Wound, GameSystem.Instance.Player).SetValue(new List<float>(){ 2 }), 5);
    }
}
