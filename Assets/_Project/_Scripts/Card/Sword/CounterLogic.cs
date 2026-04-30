using System.Collections.Generic;
using UnityEngine;

public class CounterLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);
        target.AddStatusEffect(new ParalyzeStatusEffect(StatusID.Paralyze, target).SetValue(new List<float>() { 2 }), 2);
    }
}