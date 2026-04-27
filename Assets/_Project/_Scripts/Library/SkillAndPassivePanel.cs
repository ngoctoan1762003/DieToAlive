using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillAndPassivePanel : MonoBehaviour
{
    [SerializeField] private Transform SkillContainer;
    [SerializeField] private Transform PassiveContainer;
    [SerializeField] private CardInfo CardInfoPrefab;
    [SerializeField] private Transform ContentContainter;

    public void Init(UnitConfigs unit)
    {
        Clear(SkillContainer);
        Clear(PassiveContainer);

        foreach (var cardID in unit.cardIDs)
        {
            var data = GameDatabase.Instance.GetCard(cardID);
            if (data == null) continue;

            var item = Instantiate(CardInfoPrefab, SkillContainer);
            item.Init(data.icon, data.cardName, data.description);
        }

        foreach (var passiveID in unit.passiveIDs)
        {
            var data = GameDatabase.Instance.GetPassive(passiveID);
            if (data == null) continue;

            var item = Instantiate(CardInfoPrefab, PassiveContainer);
            item.Init(data.icon, data.passiveName, data.description);
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
