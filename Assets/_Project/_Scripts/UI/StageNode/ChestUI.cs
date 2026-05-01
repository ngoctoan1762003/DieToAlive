using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour
{
    public static ChestUI Instance;

    public GameObject ChestPanel;
    public GameObject ChestRewardPanel;

    public Button OpenButton;
    public Button ClaimButton;

    public GameObject ChestImage;

    private int openClickCount = 0;
    [SerializeField] private int requiredClicks = 3;
    [SerializeField] private float baseShakeStrength = 10f;
    [SerializeField] private float shakeDuration = 0.2f;

    public Transform Content;
    public ChestUIItem ChestUIItemPrefab;

    private ChestNodeConfig currentChest;

    public TextMeshProUGUI GoldRewardTXT;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowChestPanel(ChestNodeConfig chest)
    {
        currentChest = chest;
        ChestPanel.SetActive(true);
    }

    public void ShowChestRewardPanel()
    {
        ChestPanel.SetActive(false);
        ChestRewardPanel.SetActive(true);

        GenerateRewardsUI();
    }


    private void Start()
    {
        OpenButton.onClick.AddListener(OnOpenClicked);

        ClaimButton.onClick.AddListener(delegate
        {
            GiveRewards();

            ChestPanel.SetActive(false);
            ChestRewardPanel.SetActive(false);

            MapManager.Instance.CompleteNode();
        });
    }

    private void GenerateRewardsUI()
    {
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }

        foreach (var card in currentChest.cardRewards)
        {
            var item = Instantiate(ChestUIItemPrefab, Content);
            item.Init($"Card: {card.cardID} x{card.quantity}");
        }

        foreach (var weapon in currentChest.weaponRewards)
        {
            var item = Instantiate(ChestUIItemPrefab, Content);
            item.Init($"Weapon: {weapon.weaponID} x{weapon.quantity}");
        }

        foreach (var tool in currentChest.toolRewards)
        {
            var item = Instantiate(ChestUIItemPrefab, Content);
            item.Init($"Tool: {tool.toolID} x{tool.quantity}");
        }

        GoldRewardTXT.text  = "+" + currentChest.GoldAmount.ToString();
    }

    private void OnOpenClicked()
    {
        openClickCount++;

        float strength = baseShakeStrength * openClickCount;

        ChestImage.transform.DOShakePosition(
            shakeDuration,
            strength,
            vibrato: 20,
            randomness: 90f
        );
        if (openClickCount >= requiredClicks)
        {
            openClickCount = 0;
            ShowChestRewardPanel();
        }
    }

    private void GiveRewards()
    {
        if (currentChest.GoldAmount > 0)
        {
            ShopManager.Instance.AddGold(currentChest.GoldAmount);
        }

        foreach (var card in currentChest.cardRewards)
        {
            Debug.Log("Reward " + card.cardID);
        }

        foreach (var weapon in currentChest.weaponRewards)
        {
            Debug.Log("Reward " + weapon.weaponID);
            InventoryManager.Instance.AddItem(DataManager.Instance.GetWeaponConfig(weapon.weaponID), weapon.quantity);
        }

        foreach (var tool in currentChest.toolRewards)
        {
            Debug.Log("Reward " + tool.toolID);
            InventoryManager.Instance.AddItem(DataManager.Instance.GetToolConfig(tool.toolID), tool.quantity);
        }
    }
}
