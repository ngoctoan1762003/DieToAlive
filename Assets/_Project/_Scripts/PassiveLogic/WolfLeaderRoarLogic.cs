using UnityEngine;

public class WolfLeaderRoarLogic : PassiveLogic
{
    private WolfLeaderRoar config;
    private int index;
    
    public override void Setup(Unit u)
    {
        base.Setup(u);
        index = 0;
        config = DataManager.Instance.GetPassiveConfig(PassiveID.WolfLeaderRoar) as WolfLeaderRoar;
        AssignEvent(config.passiveActivationType);
    }

    public override void Execute()
    {
        base.Execute();

        if (index >= config.hpThreshold.Length) return;
        if (config.hpThreshold[index] / 100 >= unit.CurrentHP / unit.maxHP.value)
        {
            unit.SetPriorityCard(CardID.WolfLeaderRoar);
            index++;
        }
    }
}
