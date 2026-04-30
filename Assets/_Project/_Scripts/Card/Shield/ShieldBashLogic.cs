using System.Collections.Generic;
using UnityEngine;

public class ShieldBashLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);
        target.AddStatusEffect(new StunStatusEffect(StatusID.Stun, target), 1);
    }
}