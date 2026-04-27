using UnityEngine;

public class PassiveLogic
{
    protected Unit unit;
    
    public virtual void Setup(Unit u)
    {
        unit = u;
    }
    
    public virtual void Execute() { }


    public virtual void AssignEvent(PassiveActivationType passiveActivationType)
    {
        switch (passiveActivationType)
        {
            case PassiveActivationType.OnStartAction:
                unit.onStartAction += Execute;
                break;
            case PassiveActivationType.OnEndAction:
                unit.onEndAction += Execute;
                break;
            case PassiveActivationType.OnEvadeSuccess:
                unit.onEvadeSuccess += Execute;
                break;
            case PassiveActivationType.OnClash:
                unit.onClash += Execute;
                break;
            case PassiveActivationType.OnChangeStat:
                unit.onChangeStat += Execute;
                break;
        }
    }
}
