using UnityEngine;

public class WolfRoarLogic : PassiveLogic
{
    private WolfRoar config;
    private bool used;
    
    public override void Setup(Unit u)
    {
        base.Setup(u);
        used = false;
        config = DataManager.Instance.GetPassiveConfig(PassiveID.WolfRoar) as WolfRoar;
        AssignEvent(config.passiveActivationType);
    }

    public override void Execute()
    {
        base.Execute();

        if (used) return;
        if (config.hpThreshold >= unit.CurrentHP / unit.maxHP.value)
        {
            unit.SetPriorityCard(CardID.WolfRoar);
            used = true;
        }
    }
}
