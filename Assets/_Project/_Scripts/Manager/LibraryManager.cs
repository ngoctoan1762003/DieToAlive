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
    }

    void Init()
    {
        foreach (var unit in allUnits)
        {
            string id = unit.unitID.ToString();

            if (!states.ContainsKey(id))
            {
                states.Add(id, new LibraryItemState
                {
                    id = id,
                    isDiscovered = false
                });
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
            observationPoints++;
            Save();
        }
    }

    public bool IsDiscovered(UnitID unitID)
    {
        return states.TryGetValue(unitID.ToString(), out var state) && state.isDiscovered;
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

            // load states
            foreach (var entry in data.states)
            {
                if (states.ContainsKey(entry.id))
                {
                    states[entry.id].isDiscovered = entry.isDiscovered;
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

    private void OnApplicationQuit()
    {
        Save();
    }
}