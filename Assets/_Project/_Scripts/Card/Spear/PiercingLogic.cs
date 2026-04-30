using System.Collections.Generic;
using UnityEngine;

public class PiercingLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);
        target.AddStatusEffect(new FragileStatusEffect(StatusID.Fragile, target).SetValue(new List<float>() { 1 }), 2);
    }
}