using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    private string SavePath => Application.persistentDataPath + "/save.json";

    private GameSaveData cachedData;

    private void Awake()
    {
        Instance = this;
        LoadFromDisk();
    }

    void LoadFromDisk()
    {
        try
        {
            if (!File.Exists(SavePath))
            {
                cachedData = new GameSaveData();
                SaveToDisk();
                return;
            }

            string json = File.ReadAllText(SavePath);
            cachedData = JsonUtility.FromJson<GameSaveData>(json);

            if (cachedData == null)
                cachedData = new GameSaveData();
        }
        catch
        {
            cachedData = new GameSaveData();
            SaveToDisk();
        }
    }

    void SaveToDisk()
    {
        try
        {
            string json = JsonUtility.ToJson(cachedData, true);
            File.WriteAllText(SavePath, json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Save failed: " + e);
        }
    }

    // ================= LIBRARY =================

    public LibrarySaveData GetLibraryData()
    {
        return cachedData.libraryData ?? new LibrarySaveData();
    }

    public void SaveLibraryData(LibrarySaveData data)
    {
        cachedData.libraryData = data;
        SaveToDisk();
    }
    
    public void SaveDoneTutorial(string tutorialKey)
    {
        cachedData.playerData.doneTutorials.Add(tutorialKey);
        SaveToDisk();
    }

    public bool CheckTutorialDone(string tutorialKey)
    {
        return cachedData.playerData.doneTutorials.Contains(tutorialKey);
    }
}

[System.Serializable]
public class GameSaveData
{
    public LibrarySaveData libraryData = new LibrarySaveData();
    // public InventorySaveData inventory;
    public PlayerSaveData playerData = new PlayerSaveData();
}

[System.Serializable]
public class PlayerSaveData
{
    public List<string> doneTutorials = new List<string>();
}