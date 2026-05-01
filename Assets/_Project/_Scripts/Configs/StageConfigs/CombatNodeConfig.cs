using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combat Node", menuName = "Stage/Combat")]
public class CombatNodeConfig : NodeConfigs
{
    public List<UnitID> enemies;
    public Sprite BackGroundImage;
}