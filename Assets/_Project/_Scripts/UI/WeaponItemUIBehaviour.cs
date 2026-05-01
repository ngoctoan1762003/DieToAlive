using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponItemUIBehaviour : MonoBehaviour, IInPool
{
    [SerializeField] private Button itemButton;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI quantity;
    [SerializeField] private TextMeshProUGUI description;
    private InventoryItem item;
    
    private void Start()
    {
        itemButton.onClick.AddListener(Use);
    }

    public void Setup(InventoryItem item)
    {
        itemName.text = item.config.name;
        quantity.text = item.quantity.ToString();
        this.item = item;
        switch (item.config.type)
        {
            case InventoryItemType.Weapon:
                description.gameObject.SetActive(false);
                break;
            case InventoryItemType.Tool:
                description.gameObject.SetActive(true);
                description.text = DataManager.Instance.GetLocalization(item.config.toolID + "Des");
                break;
        }
    }
    
    private void Use()
    {
        switch (item.config.type)
        {
            case InventoryItemType.Weapon:
                if (GameSystem.Instance.CurrentEquipWeapon)
                {
                    UIManager.Instance.ShowDamage("Throw current weapon first", GameSystem.Instance.Player.transform.position);
                    return;
                }
                GameSystem.Instance.EquipWeapon(item);
                break;
            case InventoryItemType.Tool:
                GameSystem.Instance.UseItem(item.config);
                break;
        }
        GameSystem.Instance.ToDiscard(GameSystem.Instance.SelectedCard);
        UIManager.Instance.HideInventoryNeed();
    }
    
    public void OnSpawn()
    {
    }

    public void OnDead()
    {
    }
}
