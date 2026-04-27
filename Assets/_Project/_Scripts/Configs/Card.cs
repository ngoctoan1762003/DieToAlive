using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler, IInPool
{
    [SerializeField] private CardID cardID;
    [SerializeField] private TextMeshProUGUI nameText;
    private CardLogic cardLogic;
    
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
}
