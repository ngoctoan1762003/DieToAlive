using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillAndPassivePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI EnemyNameTXT;
    [SerializeField] private Image EnemyIcon;
    [SerializeField] private Transform SkillContainer;
    [SerializeField] private Transform PassiveContainer;
    [SerializeField] private CardInfo CardInfoPrefab;
    [SerializeField] private Transform ContentContainter;
    [SerializeField] private Button BackButton;

    private UnitID currentUnitID;

    private void Start()
    {
        BackButton.onClick.AddListener(() =>
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
        if (currentUnitID == unit.unitID)
            return;

        currentUnitID = unit.unitID;

        EnemyNameTXT.text = unit.unitID.ToString();
        EnemyIcon.sprite = unit.sprite;

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