using UnityEngine;

public class WolfEquipEvadeLogic : PassiveLogic
{
    private WolfEquipEvadeConfig config;
    
    public override void Setup(Unit u)
    {
        base.Setup(u);

        config = DataManager.Instance.GetPassiveConfig(PassiveID.WolfEquipEvade) as WolfEquipEvadeConfig;
        AssignEvent(config.passiveActivationType);
    }

    public override void Execute()
    {
        base.Execute();

        CardLogic logic = DataManager.Instance.GetCardLogic(CardID.WolfEvade);
        logic.Setup(unit, null, CardID.WolfEvade);
        unit.ReadyCard(logic);
    }
}