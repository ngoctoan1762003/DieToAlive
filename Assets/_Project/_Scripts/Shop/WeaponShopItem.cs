using TMPro;
using UnityEngine;

public class WeaponShopItem : ShopItem
{
    private WeaponID weaponID;
    public TextMeshProUGUI ItemNameTxt;

    public void SetUp(WeaponID id, int price, int quantity)
    {
        weaponID = id;
        base.SetUpItem(price, quantity);
        ItemNameTxt.text = id.ToString();
    }

    protected override void OnBuySuccess()
    {
        Debug.Log("Buy Weapon: " + weaponID);
    }
}