using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    [SerializeField] private RectTransform hp;
    [SerializeField] private RectTransform shield;
    [SerializeField] private TextMeshProUGUI currentHPText;
    private float width = 30f;
    
    public void SetHP(float currentHP, float maxHP, float shieldVal)
    {
        float percentHP = 0;
        float percentShield = 0;
        if (currentHP + shieldVal > maxHP)
        {
            percentHP = currentHP / (currentHP + shieldVal);
            percentShield = 1 - percentHP;
        }
        else
        {
            percentHP = currentHP / maxHP;
            percentShield = shieldVal / maxHP;
        }
        hp.sizeDelta = new Vector2(width * percentHP, hp.sizeDelta.y);
        shield.sizeDelta = new Vector2(width * percentShield, hp.sizeDelta.y);
        shield.transform.localPosition = new Vector3(hp.sizeDelta.x, 0, 0);
        currentHPText.text = currentHP + "/" + maxHP;
    }
}
