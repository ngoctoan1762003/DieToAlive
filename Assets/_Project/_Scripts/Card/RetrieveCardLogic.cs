using UnityEngine;

public class RetrieveCardLogic : CardLogic
{
    protected override void RetrieveWeapon()
    {
        base.RetrieveWeapon();
        InventoryManager.Instance.AddItem(card.Source.config);
        GameSystem.Instance.CalculateHandTransform();
    }
}
