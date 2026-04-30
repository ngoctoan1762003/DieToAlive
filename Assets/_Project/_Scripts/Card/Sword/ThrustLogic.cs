using System.Collections.Generic;
using UnityEngine;

public class ThrustLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);
        target.AddStatusEffect(new WeakenStatusEffect(StatusID.Weaken, target).SetValue(new List<float>() { 1 }), 1);
    }
}