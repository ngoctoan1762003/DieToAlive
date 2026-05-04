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

    public Button DrawButton => drawButton;
    [SerializeField] private CardPopupDescription popupDescription;
    public CardPopupDescription PopupDescription => popupDescription;
    [SerializeField] private Image blackCover;
    public Image BlackCover => blackCover;
    [SerializeField] private AssetReference damagePopupAsset;
    private AddressablesPool<DamagePopup> damagePopupPool;
    [SerializeField] private AssetReference statisticAsset;
    [SerializeField] private Transform statisticTrans;
    private AddressablesPool<StatisticText> statisticPool;
    [SerializeField] private WeaponInventory weaponInventory;
    [SerializeField] private GameObject cardContainer;
    public GameObject CardContainer => cardContainer;
    
    // Inventory
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button libraryButton;
    public Button LibraryButton => libraryButton;
    public Transform LibraryUnitContainer;
    public Transform LibraryCardContainer;
    public Transform LibraryBackButton;
    public Transform SkillAndPassivePanel;
    [SerializeField] private CanvasGroup libraryCanvas;
    public CanvasGroup LibraryCanvas => libraryCanvas;
    
    public Transform drawPileCardsTransform;
    public Transform discardPileCardsTransform;
    public Transform handCardsTransform;
    public Transform readyCardsTransform;
    
    public bool LockReadyCard { get; private set; }
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        drawButton.onClick.AddListener(() =>
        {
            GameSystem.Instance.Draw(1);
            ObserverManager.Invoke(GameEventID.OnDrawCard);
        });
        popupDescription.UpdateTransform(new Vector3(1000, 1000));
        damagePopupPool = new AddressablesPool<DamagePopup>(damagePopupAsset, 10, canvasRect);
        statisticPool = new AddressablesPool<StatisticText>(statisticAsset, 10, statisticTrans);
        inventoryButton.onClick.AddListener(() => InventoryManager.Instance.Show());
        libraryButton.onClick.AddListener(() =>
        {
            libraryCanvas.alpha = 1;
            libraryCanvas.blocksRaycasts = true;
            ObserverManager.Invoke(GameEventID.OnOpenLibrary);
        });
    }

    public void SetLockReadyCard(bool val)
    {
        LockReadyCard = val;
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
            clash.ShowClash(false);
            ready.ShowClash(false);
            onComplete?.Invoke(clashVal >= readyVal, 0);
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

    public void ShowDescription(UnitID unitID, CardConfig config, Vector2 mousePos)
    {
        popupDescription.gameObject.SetActive(true);
        popupDescription.ShowInformation(unitID, config);

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

    public void ResetUI()
    {
        BlackCover.gameObject.SetActive(false);
        cardContainer.gameObject.SetActive(true);
    }

    public void UpdateStatistic()
    {
        for (int i = 0; i < statisticTrans.childCount; i++)
        {
            statisticTrans.GetChild(i).gameObject.SetActive(false);
        }
        var statistic = statisticPool.GetObjectAndActive();
        statistic.SetText("Is In Action: " + GameSystem.Instance.IsInAction);
        statistic.transform.localPosition = Vector3.zero;
    }
}