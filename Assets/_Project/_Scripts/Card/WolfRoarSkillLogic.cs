using UnityEngine;

public class WolfRoarSkillLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);
        GameSystem.Instance.SetupUnit(UnitID.LittleWolf);
    }
}
