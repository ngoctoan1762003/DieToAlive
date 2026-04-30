using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    [SerializeField] private RectTransform canvasRect;
    
    [Header("UI References")]
    [SerializeField] private Button drawButton;
    [SerializeField] private CardPopupDescription popupDescription;
    [SerializeField] private Image blackCover;
    public Image BlackCover => blackCover;
    [SerializeField] private AssetReference damagePopupAsset;
    private AddressablesPool<DamagePopup> damagePopupPool;
    [SerializeField] private WeaponInventory weaponInventory;
    [SerializeField] private GameObject cardContainer;
    public GameObject CardContainer => cardContainer;
    
    // Inventory
    [SerializeField] private Button inventoryButton;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        drawButton.onClick.AddListener(() => GameSystem.Instance.Draw(1));
        popupDescription.UpdateTransform(new Vector3(1000, 1000));
        damagePopupPool = new AddressablesPool<DamagePopup>(damagePopupAsset, 10, canvasRect);
        inventoryButton.onClick.AddListener(() => InventoryManager.Instance.Show());
    }

    public void ShowInventoryNeed(InventoryItemType type)
    {
        weaponInventory.Show(type);
    }
    
    public void HideInventoryNeed()
    {
        weaponInventory.Hide();
    }

    public void ShowDice(string name, Unit u, Action<int> onComplete)
    {
        int finalVal = Random.Range(1, 21);
        List<int> buffs = u.GetDiceBuff();
  
        u.ShowDiceAnim(name, finalVal, buffs, onComplete);
    }

    public void ClashAnim(string clashName, Unit clash, string readyName, Unit ready, Action<bool, int> onComplete)
    {
        int clashVal = Random.Range(1, 21);
        int readyVal = Random.Range(1, 21);
        
        List<int> clashBuffs = clash.GetDiceBuff();
        List<int> readyBuffs = ready.GetDiceBuff();
        clash.ShowClash(true);
        ready.ShowClash(true);
        clash.ShowDiceAnim(clashName, clashVal, clashBuffs, null);
        ready.ShowDiceAnim(readyName, readyVal, readyBuffs, null);

        foreach (var clashBuff in clashBuffs)
        {
            clashVal += clashBuff;
        }
        
        foreach (var readyBuff in readyBuffs)
        {
            readyVal += readyBuff;
        }
        
        DOVirtual.DelayedCall(2, () =>
        {
            onComplete?.Invoke(clashVal >= readyVal, 0);
            clash.ShowClash(false);
            ready.ShowClash(false);
        });
    }

    public void ShowDamage(string damage, Vector2 trans)
    {
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, trans);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, 
            screenPos, 
            Camera.main, 
            out Vector2 localPoint
        );

        var damagePopup = damagePopupPool.GetObjectAndActive();
        damagePopup.ShowDamage(damage, localPoint);
    }

    public void ShowDescription(CardConfig config, Vector2 mousePos)
    {
        popupDescription.gameObject.SetActive(true);
        popupDescription.ShowInformation(config);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, 
            mousePos, 
            Camera.main, 
            out Vector2 localPoint
        );

        popupDescription.UpdateTransform(localPoint);
    }
    
    public void ShowDescription(StatusEffect statusEffect, Vector2 mousePos)
    {
        popupDescription.gameObject.SetActive(true);
        popupDescription.ShowInformation(statusEffect);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, 
            mousePos, 
            Camera.main, 
            out Vector2 localPoint
        );

        popupDescription.UpdateTransform(localPoint);
    }

    public void HideDescription()
    {
        popupDescription.UpdateTransform(new Vector3(1000, 1000));
    }
    
    public void UpdatePopupPosition(Vector2 mousePos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, 
            mousePos, 
            Camera.main, 
            out Vector2 localPoint
        );
        popupDescription.UpdateTransform(localPoint);
    }
}