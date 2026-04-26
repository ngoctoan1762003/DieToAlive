using UnityEngine;
using System;

[CreateAssetMenu(fileName = "List Card Config", menuName = "Configs/ListCardConfigs")]
public class ListCardConfigs : ScriptableObject
{
    public CardConfig[] configs;
}

[Serializable]
public class CardConfig
{
    public CardID cardID;
    public StatType[] statType;
    public int actionNeed;
    public bool chooseBetterDice;
    public CardType cardType;
    public DamageType damageType;
}
