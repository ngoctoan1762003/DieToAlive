using System.Collections.Generic;
using UnityEngine;

public class BulwarkLogic : CardLogic
{
    protected override void OnDefense()
    {
        base.OnDefense();
        unit.Heal(Random.Range(8, 12));
    }
}