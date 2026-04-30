using UnityEngine;
using UnityEngine.UI;

public class UILine : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image image;

    [Header("Colors")]
    public Color lockedColor = Color.gray;
    public Color unlockedColor = Color.white;
    public Color visitedColor = Color.yellow;

    private LineState state;

    public string fromNodeID;
    public string toNodeID;

    public void Setup(Vector2 start, Vector2 end, LineState state)
    {
        this.state = state;

        Vector2 dir = end - start;

        float distance = dir.magnitude;

        if (distance < 0.01f)
            return;

        rectTransform.sizeDelta = new Vector2(distance, 5f);
        rectTransform.anchoredPosition = start + dir * 0.5f;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        UpdateColor();
    }

    public void SetState(LineState newState)
    {
        state = newState;
        UpdateColor();
    }

    void UpdateColor()
    {
        switch (state)
        {
            case LineState.Locked:
                image.color = lockedColor;
                break;

            case LineState.Unlocked:
                image.color = unlockedColor;
                break;

            case LineState.Visited:
                image.color = visitedColor;
                break;
        }
    }

    public void Refresh(StageNodeData fromNode, StageNodeData toNode)
    {
        string current = MapManager.Instance.currentNode.nodeID;
        string previous = MapManager.Instance.GetPreviousNodeID();
        string selected = MapManager.Instance.GetSelectedNodeID();

        if (fromNode.nodeID == previous && toNode.nodeID == current)
        {
            SetState(LineState.Visited);
            return;
        }

        if (fromNode.isVisited)
        {
            SetState(LineState.Unlocked);
            return;
        }

        if (fromNode.nodeID == current)
        {
            SetState(LineState.Locked);
            return;
        }

        SetState(LineState.Locked);
    }

}