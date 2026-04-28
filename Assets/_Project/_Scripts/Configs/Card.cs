using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IInPool
{
    [SerializeField] private CardID cardID;
    [SerializeField] private TextMeshProUGUI nameText;
    private CardLogic cardLogic;
    public CardLogic CardLogic => cardLogic;
    
    public void Setup(Unit unit, CardID cardID)
    {
        this.cardID = cardID;
        nameText.text = cardID.ToString();
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
