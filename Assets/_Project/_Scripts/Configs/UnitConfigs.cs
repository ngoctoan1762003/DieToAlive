using UnityEngine;

[CreateAssetMenu(fileName = "Unit Config", menuName = "Configs/UnitConfig")]
public class UnitConfigs : ScriptableObject
{
    public Sprite sprite;
    public UnitID unitID;
    public CardID[] cardIDs;
    public PassiveID[] passiveIDs;
    public int maxHP;
    public CardID[] mechanics;
}