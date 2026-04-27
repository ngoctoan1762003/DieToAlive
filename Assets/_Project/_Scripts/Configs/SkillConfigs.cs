using UnityEngine;

[CreateAssetMenu(fileName = "SkillConfigs", menuName = "Scriptable Objects/SkillConfigs")]
public class SkillConfigs : ScriptableObject
{
    public CardID id;
    public Sprite icon;
    public string cardName;
    [TextArea] public string description;
}
