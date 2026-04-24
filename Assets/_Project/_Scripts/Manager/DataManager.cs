using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance => instance;
    [SerializeField] private List<UnitConfigs> unitConfigs;
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
    
    public CardConfig GetCardConfig(CardID cardID)
    {
        foreach (var listConfig in cardConfigs)
        {
            foreach (var config in listConfig.configs)
            {
                if (config.cardID == cardID) return config;
            }
        }

        return null;
    }
}
