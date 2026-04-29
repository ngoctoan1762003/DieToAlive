using System;
using DG.Tweening;
using UnityEngine;

public class CardLogic
{
    private CardConfig cardConfig;
    public CardConfig CardConfig => cardConfig;
    private Card card;
    private Unit unit;
    public Unit Unit => unit;
    private Unit target;
    public Action onUsed;
    private CardLogic clashCard;
    public CardLogic ClashCard => clashCard;
    private bool decreaseEnemyAction;
    private bool unopposedAttack;

    public virtual void Setup(Unit unit, Card card, CardID cardID)
    {
        this.unit = unit;
        this.card = card;
        cardConfig = DataManager.Instance.GetCardConfig(cardID);
        decreaseEnemyAction = true;
        unopposedAttack = false;
    }

    public void SetClashCard(CardLogic cardLogic)
    {
        if (unit == GameSystem.Instance.Player)
        {
            if (cardLogic == null)
            {
                if (card != null && !GameSystem.Instance.IsInDiscard(card))
                {
                    GameSystem.Instance.AddToHand(card);
                }
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

        // Define your threshold and target position
        float attackThreshold = 3f;
        Vector3 targetPos = target.transform.position;

        // Calculate distance
        float distance = Vector3.Distance(unit.transform.position, targetPos);

        if (distance <= attackThreshold)
        {
            PerformActionLogic();
        }
        else
        {
            Vector3 offsetPos = targetPos + (unit.transform.position - targetPos).normalized * (attackThreshold * 0.8f);

            unit.transform.DOMove(offsetPos, 0.5f) // Adjust duration (0.5f) as needed
                .SetEase(Ease.OutQuad)
                .OnComplete(() => { PerformActionLogic(); });
        }
    }

    private void PerformActionLogic()
    {
        switch (cardConfig.cardType)
        {
            case CardType.Offensive:
                if (clashCard != null)
                {
                    UIManager.Instance.ClashAnim(cardConfig.cardID.ToString(), unit,
                        clashCard.CardConfig.cardID.ToString(), target, OnCompletedClash);
                    return;
                }
                else if (target.ReadyCards.Count > 0)
                {
                    CardLogic targetCard = target.ReadyCards[0];
                    target.ReadyCards.RemoveAt(0);
                    targetCard.SetClashCard(this);
                    SetClashCard(targetCard);
                    targetCard.onUsed?.Invoke();
                    unopposedAttack = true;
                    UIManager.Instance.ClashAnim(cardConfig.cardID.ToString(), unit,
                        clashCard.CardConfig.cardID.ToString(), target, OnCompletedClash);
                }
                else
                {
                    UIManager.Instance.ShowDice(cardConfig.cardID.ToString(), unit, OnCompleted);
                }

                break;

            case CardType.Defensive:
                if (unit == GameSystem.Instance.Player) GameSystem.Instance.ToDiscard(card);
                UIManager.Instance.ShowDamage(cardConfig.cardID.ToString(), unit.transform.position);
                GameSystem.Instance.CompletedAction();
                UIManager.Instance.BlackCover.gameObject.SetActive(false);
                if (cardConfig.cardID.ToString().Contains("Evade")) unit.onEvadeSuccess?.Invoke();
                break;
        }
    }

    protected virtual void OnCompleted(int val)
    {
        if (unit == GameSystem.Instance.Player) GameSystem.Instance.ToDiscard(card, decreaseEnemyAction);
        else unit.SetupActionCard();
        UIManager.Instance.BlackCover.gameObject.SetActive(false);
        unit.DeHighlight();
        target.DeHighlight();
        target.TakeDamage(unit, val);
        GameSystem.Instance.CompletedAction();
        unit.onEndAction?.Invoke();
        unit.RemoveReadyCard(this);
        decreaseEnemyAction = true;
        target.Push(target.transform.position.x > unit.transform.position.x ? Vector3.right : Vector3.left, 1);
        unopposedAttack = false;
    }

    protected virtual void OnCompletedClash(bool win, int val)
    {
        if (unit == GameSystem.Instance.Player) GameSystem.Instance.ToDiscard(card, false);
        else if (unit.ActionCount <= 0) unit.SetupActionCard();
        if (target == GameSystem.Instance.Player) GameSystem.Instance.ToDiscard(clashCard.card, false);
        else if (target.ActionCount <= 0) target.SetupActionCard();
        UIManager.Instance.BlackCover.gameObject.SetActive(false);

        if (win)
        {
            clashCard.SetClashCard(null);
            decreaseEnemyAction = unopposedAttack || unit != GameSystem.Instance.Player;
            clashCard = null;
            Execute(target);
            target.ShowLoseClash();
            target.Push(target.transform.position.x > unit.transform.position.x ? Vector3.right : Vector3.left, 3);
        }
        else
        {
            clashCard.SetClashCard(null);
            clashCard.decreaseEnemyAction = clashCard.unopposedAttack || target != GameSystem.Instance.Player;
            if (unit == GameSystem.Instance.Player) GameSystem.Instance.ToDiscard(card, decreaseEnemyAction);
            clashCard.Execute(unit);
            clashCard = null;
            unit.ShowLoseClash();
            unit.Push(target.transform.position.x > unit.transform.position.x ? Vector3.left : Vector3.right, 3);
        }
    }
}