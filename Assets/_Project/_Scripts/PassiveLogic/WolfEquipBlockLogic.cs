using UnityEngine;

public class WolfEquipBlockLogic : PassiveLogic
{
    private WolfEquipBlock config;
    private int turnCount;
    
    public override void Setup(Unit u)
    {
        base.Setup(u);

        config = DataManager.Instance.GetPassiveConfig(PassiveID.WolfEquipBlock) as WolfEquipBlock;
        AssignEvent(config.passiveActivationType);
        turnCount = 0;
    }

    public override void Execute()
    {
        base.Execute();

        turnCount++;
        if (turnCount >= config.activationAction)
        {
            CardLogic logic = DataManager.Instance.GetCardLogic(CardID.WolfBlock);
            logic.Setup(unit, null, CardID.WolfBlock);
            unit.ReadyCard(logic);
            turnCount = 0;
        }
    }
}
