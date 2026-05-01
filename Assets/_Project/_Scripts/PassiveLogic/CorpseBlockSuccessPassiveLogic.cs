using UnityEngine;

public class CorpseBlockSuccessPassiveLogic : PassiveLogic
{
    private CorpseBlockSuccessConfig config;
    
    public override void Setup(Unit u)
    {
        base.Setup(u);
        config = DataManager.Instance.GetPassiveConfig(PassiveID.CorpeBlockSuccess) as CorpseBlockSuccessConfig;
        AssignEvent(config.passiveActivationType);
    }

    public override void Execute()
    {
        base.Execute();

        unit.Heal(10);
    }
}
