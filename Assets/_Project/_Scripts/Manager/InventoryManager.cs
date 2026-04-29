using System.Collections.Generic;
using UnityEngine;
using System;
public class InventoryManager : MonoBehaviour
{
    //Singleton
    public static InventoryManager Instance;
    public List<InventoryItem> items = new();
    public float MaxWeight = 15f;
    public Action OnInventoryChanged;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        Instance = this;
    }

    public float CurrentWeight
    {
        get
        {
            float total = 0;
            foreach (var item in items)
            {
                total += item.config.weight * item.quantity;
            }
            return total;
        }
    }

    // ADD ITEM
    public void AddItem(InventoryItemConfigs config, int amount = 1)
    {
        var existing = items.Find(i => i.config == config);

        if (existing != null)
        {
            existing.quantity += amount;
        }
        else
        {
            items.Add(new InventoryItem(config, amount));
        }
        OnInventoryChanged?.Invoke();
    }

    // REMOVE ITEM
    public void RemoveItem(InventoryItemConfigs config, int amount = 1)
    {
        var item = items.Find(i => i.config == config);
        if (item == null) return;

        item.quantity -= amount;

        if (item.quantity <= 0)
            items.Remove(item);

        OnInventoryChanged?.Invoke();
    }

    // CHECK WEIGHT
    public bool CanCarry(InventoryItemConfigs config, int amount = 1)
    {
        return CurrentWeight + config.weight * amount <= MaxWeight;
    }

    public List<InventoryItem> GetAllWeaponHave()
    {
        return items.FindAll(i => i.config.type == InventoryItemType.Weapon);
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
    
    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}