using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopNodeConfig", menuName = "Stage/Shop")]
public class ShopNodeConfig : NodeConfigs
{
    public List<ShopCardItem> cardItems;
    public List<ShopWeaponItem> weaponItems;
    public List<ShopToolItem> toolItems;
}

[System.Serializable]
public class ShopCardItem
{
    public CardID cardID;
    public int price;
    public int quantity;
}

[System.Serializable]
public class ShopWeaponItem
{
    public WeaponID weaponID;
    public int price;
    public int quantity;
}

[System.Serializable]
public class ShopToolItem
{
    public ToolID toolID;
    public int price;
    public int quantity;
}