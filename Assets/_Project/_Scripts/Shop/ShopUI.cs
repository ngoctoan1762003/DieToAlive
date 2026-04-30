using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance;

    public GameObject ShopVisual;

    public Transform  CardItemContent;
    public Transform  WeaponItemContent;
    public Transform  ToolItemContent;

    public TextMeshProUGUI goldText;

    public CardShopItem CardShopItemPrefab;
    public WeaponShopItem WeaponShopItem;
    public ToolShopItem ToolShopItem;

    public Button FinishShoppingButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        FinishShoppingButton.onClick.AddListener(delegate
        {
            Hide();
            MapManager.Instance.CompleteNode();
        });
    }

    public void Show(ShopNodeConfig config)
    {
        ShopVisual.SetActive(true);

        RefreshGold();

        ClearAll();

        // CARD
        foreach (var item in config.cardItems)
        {
            var ui = Instantiate(CardShopItemPrefab, CardItemContent);
            ui.SetUp(item.cardID, item.price, item.quantity);
        }

        // WEAPON
        foreach (var item in config.weaponItems)
        {
            var ui = Instantiate(WeaponShopItem, WeaponItemContent);
            ui.SetUp(item.weaponID, item.price, item.quantity);
        }

        // TOOL
        foreach (var item in config.toolItems)
        {
            var ui = Instantiate(ToolShopItem, ToolItemContent);
            ui.SetUp(item.toolID, item.price, item.quantity);
        }
    }

    public void Hide()
    {
        ShopVisual.SetActive(false);
    }

    public void Refresh()
    {
        RefreshGold();

        foreach (var item in GetComponentsInChildren<ShopItem>())
        {
            item.Refresh();
        }
    }

    void RefreshGold()
    {
        goldText.text = ShopManager.Instance.playerGold.ToString();
    }

    void ClearAll()
    {
        foreach (Transform t in CardItemContent)
            Destroy(t.gameObject);

        foreach (Transform t in WeaponItemContent)
            Destroy(t.gameObject);

        foreach (Transform t in ToolItemContent)
            Destroy(t.gameObject);
    }
}