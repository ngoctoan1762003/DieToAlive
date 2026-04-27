using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillAndPassivePanel : MonoBehaviour
{
    [SerializeField] private Transform SkillContainer;
    [SerializeField] private Transform PassiveContainer;
    [SerializeField] private CardInfo CardInfoPrefab;
    [SerializeField] private Transform ContentContainter;
    [SerializeField] private Button BackButton;

    private void Start()
    {
        BackButton.onClick.AddListener(delegate
        {
            gameObject.SetActive(false);
        });
    }

    private void OnDestroy()
    {
        BackButton.onClick.RemoveAllListeners();
    }

    public void Init(UnitConfigs unit)
    {
        Clear(SkillContainer);
        Clear(PassiveContainer);

        foreach (var cardID in unit.cardIDs)
        {
            var data = GameDatabase.Instance.GetCard(cardID);
            if (data == null) continue;

            var item = Instantiate(CardInfoPrefab, SkillContainer);

            bool unlocked = LibraryManager.Instance.IsSkillUnlocked(unit.unitID, cardID);

            item.Init(
                data.icon,
                data.cardName,
                unlocked ? data.description : "???"
            );
        }

        foreach (var passiveID in unit.passiveIDs)
        {
            var data = GameDatabase.Instance.GetPassive(passiveID);
            if (data == null) continue;

            var item = Instantiate(CardInfoPrefab, PassiveContainer);

            bool unlocked = LibraryManager.Instance.IsPassiveUnlocked(unit.unitID, passiveID);

            item.Init(
                data.icon,
                data.passiveName,
                unlocked ? data.description : "???"
            );
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(ContentContainter as RectTransform);
    }

    private void Clear(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
}
