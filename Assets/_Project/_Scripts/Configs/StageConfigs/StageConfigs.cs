using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageConfigs", menuName = "Stage/StageConfigs")]
public class StageConfigs : ScriptableObject
{
    public List<StageNodeData> nodes;

}

[Serializable]
public class StageNodeData
{
    public string nodeID;
    public NodeConfigs node;

    public Vector2 position;

    public List<string> nextNodeIDs;

    public bool isUnlocked;
    public bool isVisited;
}