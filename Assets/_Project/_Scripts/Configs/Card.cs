using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IInPool
{
    [SerializeField] private CardID cardID;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    private CardLogic cardLogic;
    public CardLogic CardLogic => cardLogic;
    private InventoryItem source;
    public InventoryItem Source => source;
    public RectTransform rectTransform;
    private Vector3 origin;
    
    public void Setup(Unit unit, CardID cardID, InventoryItem inventoryItem)
    {
        this.cardID = cardID;
        nameText.text = cardID.ToString();
        descriptionText.text = DataManager.Instance.GetLocalization(cardID.ToString() + "Des"); 
        source = inventoryItem;
        cardLogic = DataManager.Instance.GetCardLogic(cardID);
        cardLogic.Setup(unit, this, cardID);
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameSystem.Instance.IsInHand(this)) return;
        GameSystem.Instance.ChooseCard(this);
    }

    public void Execute(Unit enemy)
    {
        GameSystem.Instance.Player.onStartAction?.Invoke();
        cardLogic.Execute(enemy);
    }

    public void OnSpawn()
    {
    }

    public void OnDead()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardLogic.ClashCard != null)
        {
            cardLogic.ClashCard.Unit.GlowActionCard();
            return;
        }
        if (transform.parent != UIManager.Instance.handCardsTransform) return;
        if (GameSystem.Instance.IsInHand(this))
        {
            rectTransform.DOAnchorPosY(origin.y + 90f, 0.3f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardLogic.ClashCard != null)
        {
            cardLogic.ClashCard.Unit.HideGlowActionCard();
        }
        else if (GameSystem.Instance.IsInHand(this))
        {
            rectTransform.DOAnchorPosY(origin.y, 0.5f);
        }
    }
}
