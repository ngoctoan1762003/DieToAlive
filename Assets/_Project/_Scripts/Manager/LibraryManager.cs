using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LibraryManager : MonoBehaviour
{
    public static LibraryManager Instance;

    [Header("Configs")]
    public List<UnitConfigs> allUnits;

    private Dictionary<string, LibraryItemState> states = new();

    private string SavePath => Application.persistentDataPath + "/library_save.json";

    private int observationPoints;
    public int ObservationPoints => observationPoints;

    private void Awake()
    {
        Instance = this;
        Init();
        Load();
        SyncWithConfig();
    }

    void Init()
    {
        foreach (var unit in allUnits)
        {
            string id = unit.unitID.ToString();

            if (!states.ContainsKey(id))
            {
                var newState = new LibraryItemState
                {
                    id = id,
                    isDiscovered = false
                };

                foreach (var card in unit.cardIDs)
                {
                    newState.skills.Add(new SkillState
                    {
                        id = card.ToString(),
                        isUnlocked = false
                    });
                }

                foreach (var passive in unit.passiveIDs)
                {
                    newState.passives.Add(new PassiveState
                    {
                        id = passive.ToString(),
                        isUnlocked = false
                    });
                }

                states.Add(id, newState);
            }
        }
    }

    // Unlock
    public void Discover(UnitID unitID)
    {
        string id = unitID.ToString();

        if (!states.ContainsKey(id)) return;

        if (!states[id].isDiscovered)
        {
            states[id].isDiscovered = true;
            Save();
        }
    }

    // Unlock skill
    public void UnlockSkill(UnitID unitID, CardID cardID)
    {
        if (!states.TryGetValue(unitID.ToString(), out var unit)) return;

        var skill = unit.skills.Find(s => s.id == cardID.ToString());
        if (skill == null) return;

        if (!skill.isUnlocked)
        {
            skill.isUnlocked = true;
            Save();
        }
    }

    // Unlock passive
    public void UnlockPassive(UnitID unitID, PassiveID passiveID)
    {
        if (!states.TryGetValue(unitID.ToString(), out var unit)) return;

        var p = unit.passives.Find(s => s.id == passiveID.ToString());
        if (p == null) return;

        if (!p.isUnlocked)
        {
            p.isUnlocked = true;
            Save();
        }
    }

    public bool IsDiscovered(UnitID unitID)
    {
        return states.TryGetValue(unitID.ToString(), out var state) && state.isDiscovered;
    }

    public bool IsSkillUnlocked(UnitID unitID, CardID cardID)
    {
        if (!states.TryGetValue(unitID.ToString(), out var unit)) return false;

        var skill = unit.skills.Find(s => s.id == cardID.ToString());
        return skill != null && skill.isUnlocked;
    }

    public bool IsPassiveUnlocked(UnitID unitID, PassiveID passiveID)
    {
        if (!states.TryGetValue(unitID.ToString(), out var unit)) return false;

        var p = unit.passives.Find(s => s.id == passiveID.ToString());
        return p != null && p.isUnlocked;
    }

    // ================= SAVE =================

    public void Save()
    {
        try
        {
            LibrarySaveData data = new LibrarySaveData
            {
                observationPoints = observationPoints,
                states = new List<LibraryItemState>(states.Values)
            };

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Save failed: " + e);
        }
    }

    public void Load()
    {
        try
        {
            if (!File.Exists(SavePath))
            {
                Save();
                return;
            }

            string json = File.ReadAllText(SavePath);
            LibrarySaveData data = JsonUtility.FromJson<LibrarySaveData>(json);

            if (data == null || data.states == null)
            {
                Save();
                return;
            }

            foreach (var entry in data.states)
            {
                if (!states.ContainsKey(entry.id)) continue;

                var local = states[entry.id];

                // load unit
                local.isDiscovered = entry.isDiscovered;

                // load skills
                foreach (var savedSkill in entry.skills)
                {
                    var skill = local.skills.Find(s => s.id == savedSkill.id);
                    if (skill != null)
                        skill.isUnlocked = savedSkill.isUnlocked;
                }

                // load passives
                foreach (var savedPassive in entry.passives)
                {
                    var p = local.passives.Find(s => s.id == savedPassive.id);
                    if (p != null)
                        p.isUnlocked = savedPassive.isUnlocked;
                }
            }

            observationPoints = data.observationPoints;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Load failed: " + e);
            Save();
        }
    }

    // For expansion
    void SyncWithConfig()
    {
        foreach (var unit in allUnits)
        {
            string id = unit.unitID.ToString();

            if (!states.ContainsKey(id)) continue;

            var state = states[id];

            // sync skills
            foreach (var card in unit.cardIDs)
            {
                if (!state.skills.Exists(s => s.id == card.ToString()))
                {
                    state.skills.Add(new SkillState
                    {
                        id = card.ToString(),
                        isUnlocked = false
                    });
                }
            }

            // sync passives
            foreach (var passive in unit.passiveIDs)
            {
                if (!state.passives.Exists(p => p.id == passive.ToString()))
                {
                    state.passives.Add(new PassiveState
                    {
                        id = passive.ToString(),
                        isUnlocked = false
                    });
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void AddObservationPoints(int amount)
    {
        if (amount <= 0) return;

        observationPoints += amount;
        Save();
    }

    public bool SpendObservationPoints(int amount)
    {
        if (amount <= 0) return false;
        if (observationPoints < amount) return false;

        observationPoints -= amount;
        Save();
        return true;
    }
}