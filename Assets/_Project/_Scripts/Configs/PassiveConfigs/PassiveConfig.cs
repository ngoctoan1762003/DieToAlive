using UnityEngine;

public class PassiveConfig : ScriptableObject
{
    public PassiveID id;
    public Sprite icon;
    public string passiveName;
    [TextArea] public string description;
    public PassiveActivationType passiveActivationType;
}
