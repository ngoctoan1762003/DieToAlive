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

    public void ShowInventoryNeed()
    {
        weaponInventory.Show();
    }
    
    public void HideInventoryNeed()
    {
        weaponInventory.Hide();
    }

    public void ShowDice(string name, Unit u, Action<int> onComplete)
    {
        int finalVal = Random.Range(1, 21);
  
        u.ShowDiceAnim(name, finalVal, onComplete);
    }

    public void ClashAnim(string clashName, Unit clash, string readyName, Unit ready, Action<bool, int> onComplete)
    {
        int clashVal = Random.Range(1, 21);
        int readyVal = Random.Range(1, 21);
        
        Debug.Log(clash + " " + clashVal);
        Debug.Log(ready + " " + readyVal);
        clash.ShowClash(true);
        ready.ShowClash(true);
        clash.ShowDiceAnim(clashName, clashVal, null);
        ready.ShowDiceAnim(readyName, readyVal, null);
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