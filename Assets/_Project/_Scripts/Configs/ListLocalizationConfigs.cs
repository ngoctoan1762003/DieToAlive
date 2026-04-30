using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Localization Config", menuName = "Configs/Localization Config")]
public class ListLocalizationConfigs : ScriptableObject
{
    public LocalizationConfig[] configs;

    public string GetValue(string key)
    {
        return configs.FirstOrDefault(c => c.key == key)?.en;
    }
}

[Serializable]
public class LocalizationConfig
{
    public string key;
    public string en;
}