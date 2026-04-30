using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponItemUIBehaviour : MonoBehaviour, IInPool
{
    [SerializeField] private Button itemButton;
    [SerializeField] private TextMeshProUGUI itemName;
    private InventoryItem item;
    
    private void Start()
    {
        itemButton.onClick.AddListener(Use);
    }

    public void Setup(InventoryItem item)
    {
        itemName.text = item.config.name;
        this.item = item;
    }
    
    private void Use()
    {
        switch (item.config.type)
        {
            case InventoryItemType.Weapon:
                GameSystem.Instance.EquipWeapon(item);
                GameSystem.Instance.ToDiscard(GameSystem.Instance.SelectedCard);
                UIManager.Instance.HideInventoryNeed();
                break;
            case InventoryItemType.Tool:
                break;
        }
    }
    
    public void OnSpawn()
    {
    }

    public void OnDead()
    {
    }
}
