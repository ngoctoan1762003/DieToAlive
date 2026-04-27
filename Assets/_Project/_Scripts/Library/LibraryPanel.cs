using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class LibraryPanel : MonoBehaviour
{
    [SerializeField] private Transform contentRoot;
    [SerializeField] private Button backButton;
    [SerializeField] private LibraryItemUI libraryItemUIPrefab;
    [SerializeField] private TextMeshProUGUI observationPointTXT;
    [SerializeField] private SkillAndPassivePanel skillAndPassivePanel;

    private void OnEnable()
    {
        observationPointTXT.text = "Observation Point: " + LibraryManager.Instance.ObservationPoints.ToString();
        skillAndPassivePanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        RenderAll();
        backButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnDestroy()
    {
        backButton.onClick.RemoveAllListeners();
    }

    public void RenderAll()
    {
        RenderInternal(LibraryManager.Instance.allUnits);
    }

    private void RenderInternal(List<UnitConfigs> list)
    {
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        foreach (var unit in list)
        {
            var item = Instantiate(libraryItemUIPrefab, contentRoot);
            item.Init(unit);
            item.OnClick += OnItemClicked;
        }
    }
    private void OnItemClicked(UnitConfigs unit)
    {
        skillAndPassivePanel.gameObject.SetActive(true);
        skillAndPassivePanel.Init(unit);
    }
}