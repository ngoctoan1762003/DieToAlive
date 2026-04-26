using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private Image InventoryItemIcon;
    [SerializeField] private TextMeshProUGUI InventoryItemNameTXT;
    [SerializeField] private TextMeshProUGUI InventoryItemQuantityTXT;

    private InventoryItem item;

    public void Init(InventoryItem itemData)
    {
        item = itemData;

        InventoryItemIcon.sprite = item.config.icon;
        InventoryItemNameTXT.text = item.config.itemName;

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (item.quantity >= 1)
            InventoryItemQuantityTXT.text = "x" + item.quantity;
        else
            InventoryItemQuantityTXT.text = "";
    }
}