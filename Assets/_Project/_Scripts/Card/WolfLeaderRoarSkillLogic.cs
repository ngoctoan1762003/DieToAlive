using UnityEngine;

public class WolfLeaderRoarSkillLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);
        GameSystem.Instance.SetupUnit(UnitID.Wolf);
    }
}
