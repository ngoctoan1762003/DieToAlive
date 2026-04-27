using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LibraryItemUI : MonoBehaviour
{
    public Action<UnitConfigs> OnClick;

    [SerializeField] private Button button;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameTXT;
    [SerializeField] private GameObject lockPanel;

    private UnitConfigs config;

    public void Init(UnitConfigs unit)
    {
        this.config = unit;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            OnClick?.Invoke(this.config);
        });

        bool discovered = LibraryManager.Instance.IsDiscovered(config.unitID);

        if (discovered)
        {
            icon.sprite = config.sprite;
            nameTXT.text = config.unitID.ToString();
            lockPanel.SetActive(false);
        }
        else
        {
            icon.gameObject.SetActive(false);
            nameTXT.text = config.unitID.ToString();
            lockPanel.SetActive(true);
        }
    }
}