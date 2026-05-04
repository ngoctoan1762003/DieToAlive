using UnityEngine;
using UnityEngine.EventSystems; 

public class SkillCardBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private Unit unit;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (unit.ActionCard == null) return;
        UIManager.Instance.ShowDescription(unit.UnitID, unit.ActionCard.CardConfig, eventData.position);
    }
    
    public void OnPointerMove(PointerEventData eventData)
    {
        UIManager.Instance.UpdatePopupPosition(eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideDescription();
    }
}