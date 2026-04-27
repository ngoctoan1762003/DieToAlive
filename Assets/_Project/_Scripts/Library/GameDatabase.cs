using System.Collections.Generic;
using UnityEngine;

public class GameDatabase : MonoBehaviour
{
    public static GameDatabase Instance;

    public List<SkillConfigs> skills;
    public List<PassiveConfig> passives;

    private Dictionary<CardID, SkillConfigs> cardDict;
    private Dictionary<PassiveID, PassiveConfig> passiveDict;

    private void Awake()
    {
        Instance = this;

        cardDict = new();
        foreach (var c in skills)
            cardDict[c.id] = c;

        passiveDict = new();
        foreach (var p in passives)
            passiveDict[p.id] = p;
    }

    public SkillConfigs GetCard(CardID id)
        => cardDict.TryGetValue(id, out var c) ? c : null;

    public PassiveConfig GetPassive(PassiveID id)
        => passiveDict.TryGetValue(id, out var p) ? p : null;
}