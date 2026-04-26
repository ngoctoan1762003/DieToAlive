using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public InventoryItemConfigs config;
    public int quantity;

    public InventoryItem(InventoryItemConfigs config, int quantity)
    {
        this.config = config;
        this.quantity = quantity;
    }
}
