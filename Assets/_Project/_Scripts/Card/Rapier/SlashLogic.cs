using System.Collections.Generic;
using UnityEngine;

public class FlecheLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);
        target.AddStatusEffect(new WoundStatusEffect(StatusID.Wound, target).SetValue(new List<float>() { 2 }), 2);
    }
}