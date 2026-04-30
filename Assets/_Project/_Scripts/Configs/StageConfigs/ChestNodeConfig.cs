using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Chest Node", menuName = "Stage/Chest")]
public class ChestNodeConfig : NodeConfigs
{
    public List<CardReward> cardRewards;
    public List<WeaponReward> weaponRewards;
    public List<ToolReward> toolRewards;
}

[System.Serializable]
public class CardReward
{
    public CardID cardID;
    public int quantity;
}

[System.Serializable]
public class ToolReward
{
    public ToolID toolID;
    public int quantity;
}

[System.Serializable]
public class WeaponReward
{
    public WeaponID weaponID;
    public int quantity;
}