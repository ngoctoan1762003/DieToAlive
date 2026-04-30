using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private AssetReference weaponItems;
    [SerializeField] private Transform weaponItemsTrans;
    private AddressablesPool<WeaponItemUIBehaviour> weaponItemsPool;

    private void Start()
    {
        weaponItemsPool = new AddressablesPool<WeaponItemUIBehaviour>(weaponItems, 10, weaponItemsTrans);
    }

    public void Show(InventoryItemType type)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        for (int i = 0; i < weaponItemsTrans.childCount; i++)
        {
            weaponItemsTrans.GetChild(i).gameObject.SetActive(false);
        }

        List<InventoryItem> weapons = InventoryManager.Instance.GetAllItemByType(type);
        for (int i = 0; i < weapons.Count; i++)
        {
            var weaponItem = weaponItemsPool.GetObjectAndActive();
            weaponItem.Setup(weapons[i]);
        }
    }
    
    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
