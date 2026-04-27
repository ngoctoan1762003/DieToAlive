using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance => instance;
    [SerializeField] private List<UnitConfigs> unitConfigs;
    [SerializeField] private List<PassiveConfig> passiveConfigs;
    [SerializeField] private List<ListCardConfigs> cardConfigs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public UnitConfigs GetUnitConfig(UnitID unitID)
    {
        return unitConfigs.FirstOrDefault(u => u.unitID == unitID);
    }
    
    public PassiveConfig GetPassiveConfig(PassiveID passiveID)
    {
        return passiveConfigs.FirstOrDefault(u => u.id == passiveID);
    }
    
    public CardConfig GetCardConfig(CardID cardID)
    {
        foreach (var listConfig in cardConfigs)
        {
            foreach (var config in listConfig.configs)
            {
                if (config.cardID == cardID) return config;
            }
        }
        Debug.Log(cardID + " not have config");
        return null;
    }
    
    public CardLogic GetCardLogic(CardID cardID)
    {
        switch (cardID)
        {
            case CardID.WolfLeaderRoar:
                return new WolfLeaderRoarSkillLogic();
            case CardID.WolfRoar:
                return new WolfRoarSkillLogic();
        }

        return new CardLogic();
    }
}
