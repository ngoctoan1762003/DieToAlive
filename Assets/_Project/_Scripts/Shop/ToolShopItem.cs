using TMPro;
using UnityEngine;

public class ToolShopItem : ShopItem
{
    private ToolID toolID;
    public TextMeshProUGUI ItemNameTxt;

    public void SetUp(ToolID id, int price, int quantity)
    {
        toolID = id;
        base.SetUpItem(price, quantity);
        ItemNameTxt.text = id.ToString();
    }

    protected override void OnBuySuccess()
    {
        Debug.Log("Buy Tool: " + toolID);
    }
}