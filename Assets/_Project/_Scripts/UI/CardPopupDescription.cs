using TMPro;
using UnityEngine;

public class CardPopupDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector2 offset;
    
    public void ShowInformation(UnitID id, CardConfig config)
    {
        nameText.text = LibraryManager.Instance.IsSkillUnlocked(id, config.cardID) ? GameDatabase.Instance.GetCard(config.cardID).cardName : "???";
        description.text = LibraryManager.Instance.IsSkillUnlocked(id, config.cardID) ? GameDatabase.Instance.GetCard(config.cardID).description : "???";
    }
    
    public void ShowInformation(StatusEffect statusEffect)
    {
        nameText.text = statusEffect.GetID().ToString();
        description.text = DataManager.Instance.GetLocalization(statusEffect.GetID() + "Des");
    }

    public void UpdateTransform(Vector2 trans)
    {
        rectTransform.anchoredPosition = trans + offset;
    }
}
