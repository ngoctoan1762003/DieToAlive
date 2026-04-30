using System.Collections.Generic;
using UnityEngine;

public class SweepLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);
        unit.AddStatusEffect(new ProtectionStatusEffect(StatusID.Fragile, unit).SetValue(new List<float>() { 1 }), 3);
    }
}