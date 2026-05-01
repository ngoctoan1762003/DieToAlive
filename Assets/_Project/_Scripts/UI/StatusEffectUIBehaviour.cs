using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusEffectUIBehaviour : MonoBehaviour, IInPool, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI value;
    [SerializeField] private TextMeshProUGUI turn;
    private StatusEffect statusEffect;
    
    public void Setup(StatusEffect statusEffect, int turn)
    {
        this.statusEffect = statusEffect;
        icon.sprite = DataManager.Instance.GetStatusEffectIcon(statusEffect.GetID());
        if (statusEffect.GetValues() != null && statusEffect.GetValues().Count > 0)
        {
            value.text = statusEffect.GetValues()[0].ToString();
            value.gameObject.SetActive(true);
        }
        else
        {
            value.gameObject.SetActive(false);
        }
        this.turn.text = turn == -1 ? "∞" : turn.ToString();
    }
    
    public void OnSpawn()
    {
    }

    public void OnDead()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowDescription(statusEffect, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideDescription();
    }
}
