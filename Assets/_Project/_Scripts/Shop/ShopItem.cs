using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public TextMeshProUGUI PriceTxt;
    public Button buyButton;

    protected int price;
    protected int quantity;


    public virtual void SetUpItem(int price, int quantity)
    {
        this.price = price;
        this.quantity = quantity;

        PriceTxt.text = price.ToString();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);
    }

    public virtual void BuyItem()
    {
        if (quantity <= 0) return;

        bool success = ShopManager.Instance.TryBuy(price);

        if (!success) return;

        quantity--;

        OnBuySuccess();

        Refresh();
    }

    protected virtual void OnBuySuccess()
    {

    }

    public virtual void Refresh()
    {
        buyButton.interactable = quantity > 0 && ShopManager.Instance.playerGold >= price;
    }
}