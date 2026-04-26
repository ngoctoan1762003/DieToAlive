using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TotalWeightTXT;
    [SerializeField] private InventoryItemUI InventoryItemUIPrefab;
    [SerializeField] private Transform contentRoot;
    [SerializeField] private Button BackButton;

    private List<InventoryItemUI> uiItems = new();

    private void Start()
    {
        InventoryManager.Instance.OnInventoryChanged += Refresh;
        Refresh();

        BackButton.onClick.AddListener(delegate
        {
            gameObject.SetActive(false);
        });
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= Refresh;

        BackButton.onClick.RemoveAllListeners();
    }

    public void Refresh()
    {
        ClearUI();

        var items = InventoryManager.Instance.items;

        foreach (var item in items)
        {
            var ui = Instantiate(InventoryItemUIPrefab, contentRoot);
            ui.Init(item);
            uiItems.Add(ui);
        }

        UpdateWeight();
    }

    private void ClearUI()
    {
        foreach (var ui in uiItems)
        {
            Destroy(ui.gameObject);
        }

        uiItems.Clear();
    }

    private void UpdateWeight()
    {
        float current = InventoryManager.Instance.CurrentWeight;
        float max = InventoryManager.Instance.MaxWeight;

        TotalWeightTXT.text = $"Weight: {current} / {max}";
    }
}