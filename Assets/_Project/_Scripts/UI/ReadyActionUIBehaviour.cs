using UnityEngine;
using UnityEngine.EventSystems;

public class ReadyActionUIBehaviour : MonoBehaviour, IInPool, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    private CardConfig config;
    private Unit unit;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.ShowDescription(config, eventData.position);
    }
    
    public void OnPointerMove(PointerEventData eventData)
    {
        UIManager.Instance.UpdatePopupPosition(eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideDescription();
    }

    public void Setup(CardConfig config, Unit u)
    {
        this.config = config;
        unit = u;
    }
    
    public void Use()
    {
        gameObject.SetActive(false);
    }
    
    public void OnSpawn()
    {
    }

    public void OnDead()
    {
    }
}
