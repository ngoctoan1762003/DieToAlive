using UnityEngine;

[CreateAssetMenu(fileName = "NodeConfigs", menuName = "Scriptable Objects/NodeConfigs")]
public class NodeConfigs : ScriptableObject
{
    public string nodeID;
    public NodeType nodeType;
}
