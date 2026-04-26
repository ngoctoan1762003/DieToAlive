using TMPro;
using UnityEngine;

public class CardPopupDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector2 offset;
    
    public void ShowInformation(CardConfig config)
    {
        nameText.text = config.cardID.ToString();
        description.text = config.cardID.ToString();
    }

    public void UpdateTransform(Vector2 trans)
    {
        rectTransform.anchoredPosition = trans + offset;
    }
}
