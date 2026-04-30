using System.Collections.Generic;
using UnityEngine;

public class FlurryLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);
        target.AddStatusEffect(new FragileStatusEffect(StatusID.Fragile, target).SetValue(new List<float>() { 2 }), 1);
    }
}