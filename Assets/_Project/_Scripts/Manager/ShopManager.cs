using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("Runtime Gold")]
    public int playerGold;

    [Header("Config")]
    public int startGold = 100;

    private ShopNodeConfig currentShop;

    private void Awake()
    {
        Instance = this;
    }

    public void StartRun()
    {
        playerGold = startGold;
        OnGoldChanged();
    }

    public void OpenShop(ShopNodeConfig config)
    {
        currentShop = config;
        ShopUI.Instance.Show(config);
    }

    public bool TryBuy(int price)
    {
        if (!CanAfford(price))
            return false;

        SpendGold(price);
        return true;
    }

    public bool CanAfford(int amount)
    {
        return playerGold >= amount;
    }

    public void SpendGold(int amount)
    {
        playerGold -= amount;
        if (playerGold < 0) playerGold = 0;

        OnGoldChanged();
    }

    public void AddGold(int amount)
    {
        playerGold += amount;
        OnGoldChanged();
    }

    public void SetGold(int amount)
    {
        playerGold = Mathf.Max(0, amount);
        OnGoldChanged();
    }

    void OnGoldChanged()
    {
        ShopUI.Instance.Refresh();
    }
}