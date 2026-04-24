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
    public float diceValue;
    public StatType[] statType;
    public int range;
    public bool chooseBetterDice;
    public CardType cardType;
    public DamageType damageType;
}
