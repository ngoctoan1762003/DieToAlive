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

    // Icon
    [SerializeField] private Sprite slash;
    [SerializeField] private Sprite pierce;
    [SerializeField] private Sprite blunt;
    [SerializeField] private Sprite block;
    [SerializeField] private Sprite evade;
    
    [SerializeField] private List<StatusEffectIconConfig> configs;
    [SerializeField] private ListLocalizationConfigs localizationConfigs;
    
    [SerializeField] private Material glowUIMat;
    public Material GlowUIMat => glowUIMat;
    
    public List<InventoryItemConfigs> toolConfigs;
    public List<InventoryItemConfigs> weaponConfigs;
    
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

    public InventoryItemConfigs GetToolConfig(ToolID toolID)
    {
        return toolConfigs.FirstOrDefault(t => t.toolID == toolID);
    }
    
    public InventoryItemConfigs GetWeaponConfig(WeaponID weaponID)
    {
        return weaponConfigs.FirstOrDefault(t => t.weaponID == weaponID);
    }

    public string GetLocalization(string key)
    {
        return localizationConfigs.GetValue(key);
    }

    public Sprite GetStatusEffectIcon(StatusID id)
    {
        return configs.FirstOrDefault(c => c.id == id)?.icon;
    }
    
    public ListCardConfigs GetCardListByWeapon(WeaponID id)
    {
        return cardConfigs.FirstOrDefault(c => c.weaponID == id);
    }

    public Sprite GetActionIcon(CardConfig config)
    {
        switch (config.cardType)
        {
            case CardType.Offensive:
                switch (config.damageType)
                {
                    case DamageType.Slash: return slash;
                    case DamageType.Pierce: return pierce;
                    case DamageType.Blunt: return blunt;
                }

                break;
            case CardType.Defensive:
                if (config.cardID.ToString().Contains("Evade")) return evade;
                if (config.cardID.ToString().Contains("Block")) return block;
                break;
        }

        return null;
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
            case CardID.ThrowBow:
            case CardID.ThrowClaymore:
            case CardID.ThrowDagger:
            case CardID.ThrowRapier:
            case CardID.ThrowShield:
            case CardID.ThrowSpear:
            case CardID.ThrowSword:
            case CardID.ThrowStaff:
                return new ThrowCardLogic();
            case CardID.RetrieveBow:
            case CardID.RetrieveClaymore:
            case CardID.RetrieveDagger:
            case CardID.RetrieveRapier:
            case CardID.RetrieveShield:
            case CardID.RetrieveSpear:
            case CardID.RetrieveSword:
            case CardID.RetrieveStaff:
                return new RetrieveCardLogic();
            case CardID.SneakAttack:
                return new SneakAttackLogic();
            case CardID.Puncture:
                return new PunctureLogic();
            case CardID.Slash:
                return new SlashLogic();
            case CardID.SwordCounter:
                return new CounterLogic();
            case CardID.Thrust:
                return new ThrustLogic();
            case CardID.Fleche:
                return new FlecheLogic();
            case CardID.RapierCounter:
                return new RapierCounterLogic();
            case CardID.Flurry:
                return new FlurryLogic();
            case CardID.Bulwark:
                return new BulwarkLogic();
            case CardID.ShieldBash:
                return new ShieldBashLogic();
            case CardID.Piercing:
                return new PiercingLogic();
            case CardID.Sweep:
                return new SweepLogic();
            case CardID.Evolve:
                return new ParasiteEvolveSkillLogic();
            case CardID.Absorb:
                return new ParasiteAbsorbSkillLogic();
            case CardID.Throw:
                return new ParasiteThrowSkillLogic();
            case CardID.Roll:
                return new ParasiteRollSkillLogic();
            case CardID.QuickBite:
                return new SmallParasiteBiteSkillLogic();
        }

        return new CardLogic();
    }
}

[Serializable]
public class StatusEffectIconConfig
{
    public StatusID id;
    public Sprite icon;
}