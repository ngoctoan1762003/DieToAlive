using TMPro;
using UnityEngine;

public class CardShopItem : ShopItem
{
    private CardID cardID;
    public TextMeshProUGUI ItemNameTxt;

    public void SetUp(CardID id, int price, int quantity)
    {
        cardID = id;
        base.SetUpItem(price, quantity);
        ItemNameTxt.text = id.ToString();
    }

    protected override void OnBuySuccess()
    {
        Debug.Log("Buy Card: " + cardID);

    }
}