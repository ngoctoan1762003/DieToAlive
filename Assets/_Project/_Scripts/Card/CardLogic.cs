using System;
using UnityEngine;

public class CardLogic 
{
    private CardConfig cardConfig;
    public CardConfig CardConfig => cardConfig;
    private Card card;
    private Unit unit;
    private Unit target;
    public Action onUsed;
    private CardLogic clashCard;
    public CardLogic ClashCard => clashCard;

    public virtual void Setup(Unit unit, Card card, CardID cardID)
    {
        this.unit = unit;
        this.card = card;
        cardConfig = DataManager.Instance.GetCardConfig(cardID);
    }

    public void SetClashCard(CardLogic cardLogic)
    {
        Debug.Log(unit + " " + cardLogic);
        if (unit == GameSystem.Instance.Player)
        {
            if (cardLogic == null)
            {
                GameSystem.Instance.AddToHand(card);
            }
            else
            {
                GameSystem.Instance.RemoveCardFromHand(card);
            }
        }
        this.clashCard = cardLogic;
    }

    public void AddReadyCard(CardLogic targetCard)
    {
        if (targetCard.clashCard != null)
        {
            targetCard.clashCard.SetClashCard(null);
            targetCard.SetClashCard(null);
        }
        SetClashCard(targetCard);
        targetCard.SetClashCard(this);
    }
    
    public virtual void Execute(Unit enemy)
    {
        UIManager.Instance.BlackCover.gameObject.SetActive(true);
        unit.Highlight();
        enemy.Highlight();
        target = enemy;
        switch (cardConfig.cardType)
        {
            case CardType.Offensive:
                UIManager.Instance.ShowDice(OnCompleted);
                break;
            case CardType.Defensive:
                break;
        }
    }

    protected virtual void OnCompleted(int val)
    {
        if (unit == GameSystem.Instance.Player) GameSystem.Instance.ToDiscard(card);
        else unit.SetupActionCard();
        UIManager.Instance.BlackCover.gameObject.SetActive(false);
        unit.DeHighlight();
        target.DeHighlight();
        target.TakeDamage(unit, val);
        GameSystem.Instance.CompletedAction();
        unit.onEndAction?.Invoke();
        onUsed?.Invoke();
    }
}
