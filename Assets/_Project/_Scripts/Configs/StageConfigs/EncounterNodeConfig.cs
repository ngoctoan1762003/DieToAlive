using UnityEngine;

[CreateAssetMenu(fileName = "Encounter Node", menuName = "Stage/Encounter")]
public class EncounterNodeConfig : NodeConfigs
{
    [TextArea]
    public string description;

    public string[] options;
}