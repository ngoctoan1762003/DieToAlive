using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameTXT;
    [SerializeField] private TextMeshProUGUI descriptionTXT;
    [SerializeField] private GameObject lockGO;
    [SerializeField] private Button unlockBtn;

    private UnitID unitID;
    private CardID cardID;
    private PassiveID passiveID;

    private void Start()
    {
        unlockBtn.onClick.AddListener(Unlock);
    }

    public void Init(bool unlocked, UnitID unitID, Sprite iconSprite, string name, string desc)
    {
        this.unitID = unitID;
        lockGO.SetActive(!unlocked);
        icon.sprite = iconSprite;
        nameTXT.text = name;
        descriptionTXT.text = desc;
    }

    private void Unlock()
    {
        if (cardID != CardID.None)
        {
            LibraryManager.Instance.UnlockSkill(unitID, cardID);
            Init(true, unitID, icon.sprite, GameDatabase.Instance.GetCard(cardID).cardName, GameDatabase.Instance.GetCard(cardID).description);
            return;
        }
        
        if (passiveID != PassiveID.None)
        {
            LibraryManager.Instance.UnlockPassive(unitID, passiveID);
            Init(true, unitID, icon.sprite, GameDatabase.Instance.GetPassive(passiveID).passiveName, GameDatabase.Instance.GetPassive(passiveID).description);
        }
    }

    public void SetCardID(CardID cardID)
    {
        this.cardID = cardID;
        this.passiveID = PassiveID.None;
    }
    
    public void SetPassiveID(PassiveID passiveID)
    {
        this.passiveID = passiveID;
        this.cardID = CardID.None;
    }
}
