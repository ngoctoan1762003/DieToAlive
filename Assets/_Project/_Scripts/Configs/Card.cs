using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IInPool
{
    [SerializeField] private CardID cardID;
    [SerializeField] private TextMeshProUGUI nameText;
    private CardLogic cardLogic;
    public CardLogic CardLogic => cardLogic;
    private InventoryItem source;
    public InventoryItem Source => source;
    
    public void Setup(Unit unit, CardID cardID, InventoryItem inventoryItem)
    {
        this.cardID = cardID;
        nameText.text = cardID.ToString();
        source = inventoryItem;
        cardLogic = DataManager.Instance.GetCardLogic(cardID);
        cardLogic.Setup(unit, this, cardID);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GameSystem.Instance.IsInHand(this)) return;
        GameSystem.Instance.ChooseCard(this);
    }

    public void Execute(Unit enemy)
    {
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
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardLogic.ClashCard != null)
        {
            cardLogic.ClashCard.Unit.HideGlowActionCard();
        }
    }
}
