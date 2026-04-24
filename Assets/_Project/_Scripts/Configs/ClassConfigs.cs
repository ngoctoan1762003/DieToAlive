using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ClassConfigs", menuName = "Configs/ClassConfigs")]
public class ClassConfigs : ScriptableObject
{
    public Class classID;
    public BonusStat[] bonusStat;
}

[Serializable]
public class BonusStat
{
    public StatType statType;
    public float statValue;
}