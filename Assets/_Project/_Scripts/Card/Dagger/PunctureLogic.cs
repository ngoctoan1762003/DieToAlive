using System.Collections.Generic;
using UnityEngine;

public class PunctureLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);
        target.TakeDamage(unit, Random.Range(1, 3));
    }
}