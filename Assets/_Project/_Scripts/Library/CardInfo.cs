using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameTXT;
    [SerializeField] private TextMeshProUGUI descriptionTXT;

    public void Init(Sprite iconSprite, string name, string desc)
    {
        icon.sprite = iconSprite;
        nameTXT.text = name;
        descriptionTXT.text = desc;
    }
}
