using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("Config")]
    public StageConfigs stageConfigs;

    private Dictionary<string, StageNodeData> nodeDict;

    public StageNodeData currentNode;

    private string previousNodeID;
    private string selectedNodeID;

    private void Awake()
    {
        Instance = this;

        nodeDict = new Dictionary<string, StageNodeData>();

        foreach (var node in stageConfigs.nodes)
        {
            nodeDict[node.nodeID] = node;
        }
    }

    private void Start()
    {
        if (stageConfigs.nodes == null || stageConfigs.nodes.Count == 0)
        {
            Debug.LogError("StageConfigs is empty!");
            return;
        }

        StartMap(stageConfigs.nodes[0].nodeID);
    }


    public void StartMap(string startNodeID)
    {
        foreach (var node in nodeDict.Values)
        {
            node.isUnlocked = false;
            node.isVisited = false;
        }

        currentNode = nodeDict[startNodeID];
        currentNode.isUnlocked = true;
        MapUIManager.Instance.RefreshLocalLines(currentNode, nodeDict);
        MapUIManager.Instance.RefreshNodeView(currentNode.nodeID);
    }


    public void MoveToNode(string nodeID, bool execute = false)
    {
        if (!nodeDict.TryGetValue(nodeID, out var node))
        {
            Debug.LogError("Node not found: " + nodeID);
            return;
        }

        if (!node.isUnlocked)
        {
            Debug.Log("Node is locked!");
            return;
        }

        previousNodeID = currentNode != null ? currentNode.nodeID : null;
        currentNode = node;

        selectedNodeID = node.nodeID;

        if (execute)
            ExecuteNode(currentNode.node);

        LockSiblingNodes();

        MapUIManager.Instance.RefreshLocalLines(currentNode, nodeDict);
        MapUIManager.Instance.RefreshNodeView(currentNode.nodeID);

        if (previousNodeID != null)
        {
            MapUIManager.Instance.RefreshLocalLines(nodeDict[previousNodeID], nodeDict);
        }

    }
    
    public void ResetRun()
    {
        if (stageConfigs.nodes == null || stageConfigs.nodes.Count == 0)
        {
            Debug.LogError("StageConfigs is empty!");
            return;
        }

        previousNodeID = null;
        selectedNodeID = null;

        StartMap(stageConfigs.nodes[0].nodeID);
        MapUIManager.Instance.OpenMap();
    }
    
    public void CompleteNode()
    {
        currentNode.isVisited = true;

        UnlockNextNodes();

        MapUIManager.Instance.RefreshLocalLines(currentNode, nodeDict);
        MapUIManager.Instance.RefreshNodeView(currentNode.nodeID);
        MapUIManager.Instance.OpenMap();
    }


    void UnlockNextNodes()
    {
        foreach (var id in currentNode.nextNodeIDs)
        {
            if (nodeDict.TryGetValue(id, out var node))
            {
                node.isUnlocked = true;
                MapUIManager.Instance.RefreshNodeView(id);
            }
        }
    }

    public List<StageNodeData> GetNextNodes()
    {
        List<StageNodeData> result = new();

        foreach (var id in currentNode.nextNodeIDs)
        {
            if (nodeDict.ContainsKey(id))
                result.Add(nodeDict[id]);
        }

        return result;
    }

    public StageNodeData GetNodeConfigs(string keys)
    {
        return nodeDict[keys];
    }

    void ExecuteNode(NodeConfigs config)
    {
        switch (config.nodeType)
        {
            case NodeType.Combat:
                MapUIManager.Instance.CloseMap();

                var combatConfig = currentNode.node as CombatNodeConfig;

                if (combatConfig != null)
                {
                    GameSystem.Instance.StartCombat(combatConfig);
                    BackGroundManager.Instance.SetUpBG(combatConfig.BackGroundImage);
                }
                else
                {
                    Debug.LogError("Node is Combat but config is wrong!");
                }

                break;

            case NodeType.Rest:
                MapUIManager.Instance.CloseMap();
                RestUI.Instance.ShowRestUI();
                Debug.Log("Rest node");
                break;

            case NodeType.Shop:
                MapUIManager.Instance.CloseMap();

                var shopConfig = currentNode.node as ShopNodeConfig;

                if (shopConfig != null)
                {
                    ShopManager.Instance.OpenShop(shopConfig);
                }
                else
                {
                    Debug.LogError("Node is Shop but config is not ShopNodeConfig!");
                }

                break;

            case NodeType.Chest:
                MapUIManager.Instance.CloseMap();

                var chestConfig = currentNode.node as ChestNodeConfig;

                if (chestConfig != null)
                {
                    ChestUI.Instance.ShowChestPanel(chestConfig);
                }
                else
                {
                    Debug.LogError("Chest config sai!");
                }
                break;
        }
    }

    void LockSiblingNodes()
    {
        if (previousNodeID == null) return;

        var previousNode = nodeDict[previousNodeID];

        foreach (var id in previousNode.nextNodeIDs)
        {
            if (id == currentNode.nodeID) continue;

            if (nodeDict.TryGetValue(id, out var node))
            {
                node.isUnlocked = false;

                MapUIManager.Instance.RefreshNodeView(id);
            }
        }
    }

    public string GetPreviousNodeID()
    {
        return previousNodeID;
    }

    public string GetSelectedNodeID()
    {
        return selectedNodeID;
    }
}