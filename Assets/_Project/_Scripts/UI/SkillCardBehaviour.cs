using UnityEngine;
using UnityEngine.EventSystems; // Required for event interfaces

public class SkillCardBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] private Unit unit;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowDescription(unit.ActionCard.CardConfig, eventData.position);
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