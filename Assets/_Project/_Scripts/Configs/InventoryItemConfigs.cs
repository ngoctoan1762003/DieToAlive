using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Item Config", menuName = "Configs/InventoryItemConfig")]
public class InventoryItemConfigs : ScriptableObject
{
    public string itemName;
    public InventoryItemType type;
    public WeaponID weaponID;
    public Sprite icon;

    [Header("Core")]
    public float weight;
}
