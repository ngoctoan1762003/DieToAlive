using System.Collections.Generic;
using UnityEngine;

public class WolfEvadeSuccessLogic : PassiveLogic
{
    private WolfEvadeSuccess config;
    
    public override void Setup(Unit u)
    {
        base.Setup(u);

        config = DataManager.Instance.GetPassiveConfig(PassiveID.WolfEvadeSuccess) as WolfEvadeSuccess;
        AssignEvent(config.passiveActivationType);
    }

    public override void Execute()
    {
        base.Execute();

        unit.AddStatusEffect(new BuffStrengthStatusEffect(StatusID.BuffStrength, unit, 99).SetValue(new List<float>(){config.strengthGain}), -1);
    }
}
