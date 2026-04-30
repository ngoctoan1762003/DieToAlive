using UnityEngine;
using UnityEngine.UI;

public class StageNodeView : MonoBehaviour
{
    private StageNodeData data;

    public Image Image;
    public GameObject DoneIcon;

    public Button button;

    private NodeType nodeType;

    public void Setup(StageNodeData data)
    {
        this.data = data;

        nodeType = data.node.nodeType;

        switch (nodeType)
        {
            case NodeType.Combat:
                Image.sprite = Resources.Load<Sprite>("NodeIcon/Combat");
                break;

            case NodeType.Rest:
                Image.sprite = Resources.Load<Sprite>("NodeIcon/Rest");
                break;

            case NodeType.Shop:
                Image.sprite = Resources.Load<Sprite>("NodeIcon/Shop");
                break;

            case NodeType.Chest:
                Image.sprite = Resources.Load<Sprite>("NodeIcon/Chest");
                break;
        }

        RefreshState();
    }

    public void RefreshState()
    {
        if (data == null) return;

        if (data.isVisited)
        {
            DoneIcon.SetActive(true);
            SetInteractable(false);
            return;
        }

        DoneIcon.SetActive(false);

        if (!data.isUnlocked)
        {
            SetInteractable(false);
            return;
        }

        SetInteractable(true);
    }

    void SetInteractable(bool value)
    {
        if (button != null)
            button.interactable = value;
    }

    public void OnClick()
    {
        if (data == null) return;

        if (!data.isUnlocked || data.isVisited)
        {
            Debug.Log("Node cannot be clicked!");
            return;
        }

        MapManager.Instance.MoveToNode(data.nodeID, true);
        gameObject.transform.localScale = Vector3.one;
    }
}