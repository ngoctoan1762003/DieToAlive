using System.Collections.Generic;
using UnityEngine;

public class SlashLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);
        target.AddStatusEffect(new BleedStatusEffect(StatusID.Bleed, target).SetValue(new List<float>() { 2 }), 2);
    }
}