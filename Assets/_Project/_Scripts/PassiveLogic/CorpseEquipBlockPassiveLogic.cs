using UnityEngine;

public class CorpseEquipBlockPassiveLogic : PassiveLogic
{
    private CorpseEquipBlockConfig config;
    private int count;
    
    public override void Setup(Unit u)
    {
        base.Setup(u);
        count = 0;
        config = DataManager.Instance.GetPassiveConfig(PassiveID.CorpeEquipBlock) as CorpseEquipBlockConfig;
        AssignEvent(config.passiveActivationType);
    }

    public override void Execute()
    {
        base.Execute();

        count++;
        if (count >= config.takeDamageTime)
        {
            count = 0;
            unit.ReadyCard(DataManager.Instance.GetCardLogic(CardID.CorpseBlock));
        }
    }
}
