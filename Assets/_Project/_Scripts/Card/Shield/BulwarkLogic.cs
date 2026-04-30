using System.Collections.Generic;
using UnityEngine;

public class BulwarkLogic : CardLogic
{
    protected override void OnCompleted(int val)
    {
        base.OnCompleted(val);
        unit.Heal(Random.Range(8, 12));
    }
}