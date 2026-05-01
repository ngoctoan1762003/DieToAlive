using System.Collections.Generic;
using UnityEngine;

public class ShieldBashLogic : CardLogic
{
    protected override void OnDefense()
    {
        base.OnDefense();
        target.AddStatusEffect(new StunStatusEffect(StatusID.Stun, target), 1);
    }
}