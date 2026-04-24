using System;
using UnityEngine;

public class CardLogic 
{
    private CardConfig cardConfig;
    private Card card;
    private Unit unit;
    private Unit target;

    public virtual void Setup(Unit unit, Card card, CardID cardID)
    {
        this.unit = unit;
        this.card = card;
        cardConfig = DataManager.Instance.GetCardConfig(cardID);
    }
    
    public virtual void Execute(Unit enemy)
    {
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

    private void OnCompleted(int val)
    {
        GameSystem.Instance.ToDiscard(card);
        target.TakeDamage(unit, val);
    }
}
