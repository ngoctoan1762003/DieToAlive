using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LibraryItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameTXT;
    [SerializeField] private GameObject lockPanel;

    private UnitConfigs config;

    public void Init(UnitConfigs unit)
    {
        this.config = unit;

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