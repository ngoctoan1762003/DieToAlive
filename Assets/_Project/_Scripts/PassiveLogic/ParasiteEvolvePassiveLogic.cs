using System.Linq;
using UnityEngine;

public class ParasiteEvolvePassiveLogic : PassiveLogic
{
    private ParasiteEvolveConfig config;
    
    public override void Setup(Unit u)
    {
        base.Setup(u);
        config = DataManager.Instance.GetPassiveConfig(PassiveID.Evolve) as ParasiteEvolveConfig;
        AssignEvent(config.passiveActivationType);
    }

    public override void Execute()
    {
        base.Execute();

        if (config.hpThreshold >= unit.CurrentHP / unit.maxHP.value && GameSystem.Instance.Enemies.Where(u => u.UnitID == UnitID.Corpse).Any())
        {
            unit.SetPriorityCard(CardID.Evolve);
        }
    }
}
