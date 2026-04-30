using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUIManager : MonoBehaviour
{
    public static MapUIManager Instance;

    public StageConfigs stageConfigs;
    public RectTransform content;
    public GameObject nodePrefab;
    public ScrollRect scrollRect;

    private Dictionary<string, RectTransform> nodeMap = new();

    public GameObject linePrefab;

    private List<UILine> lines = new();
    private Dictionary<string, StageNodeView> viewMap = new();

    private Tween mapTween;
    private Vector3 originalScale;

    private void Awake()
    {
        Instance = this;
        originalScale = transform.localScale;
    }

    public void OpenMap()
    {
        gameObject.SetActive(true);
        scrollRect.enabled = false;

        mapTween?.Kill();

        transform.localScale = new Vector3(originalScale.x, 0f, originalScale.z);

        mapTween = transform.DOScaleY(1f, 0.25f)
            .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    scrollRect.enabled = true;
                });
    }

    public void CloseMap()
    {
        mapTween?.Kill();
        scrollRect.enabled = false;

        transform.localScale = new Vector3(originalScale.x, 1f, originalScale.z);

        mapTween = transform.DOScaleY(0f, 0.2f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                scrollRect.enabled = true;
            });
    }


    private void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 max = new Vector2(float.MinValue, float.MinValue);

        nodeMap.Clear();

        foreach (var node in stageConfigs.nodes)
        {
            var go = Instantiate(nodePrefab, content);
            var rect = go.GetComponent<RectTransform>();

            rect.anchoredPosition = node.position;

            go.GetComponent<StageNodeView>().Setup(node);

            nodeMap[node.nodeID] = rect;

            Vector2 size = rect.rect.size;

            Vector2 nodeMin = node.position - size * 0.5f;
            Vector2 nodeMax = node.position + size * 0.5f;

            min = Vector2.Min(min, nodeMin);
            max = Vector2.Max(max, nodeMax);

            var view = go.GetComponent<StageNodeView>();
            view.Setup(node);

            viewMap[node.nodeID] = view;
        }

        float padding = 50f;
        min -= Vector2.one * padding;
        max += Vector2.one * padding;

        ResizeContent(min, max);

        StartCoroutine(AfterBuild());
    }

    void ResizeContent(Vector2 min, Vector2 max)
    {
        float height = max.y - min.y;

        Vector2 size = content.sizeDelta;
        size.y = height;
        content.sizeDelta = size;

        content.anchoredPosition = new Vector2(
            content.anchoredPosition.x,
            -min.y
        );
    }

    IEnumerator ScrollAnimation()
    {
        yield return null;
        yield return new WaitForEndOfFrame();

        scrollRect.verticalNormalizedPosition = 1f;

        yield return new WaitForSeconds(0.2f);

        DOTween.To(
            () => scrollRect.verticalNormalizedPosition,
            x => scrollRect.verticalNormalizedPosition = x,
            0f,
            1.5f
        ).SetEase(Ease.InOutCubic);
    }

    public void ScrollAnimationROutine()
    {
        StartCoroutine(ScrollAnimation());
    }

    void DrawLines()
    {
        foreach (var node in stageConfigs.nodes)
        {
            if (!nodeMap.TryGetValue(node.nodeID, out RectTransform from))
                continue;

            foreach (var nextID in node.nextNodeIDs)
            {
                if (!nodeMap.TryGetValue(nextID, out RectTransform to))
                    continue;

                GameObject lineObj = Instantiate(linePrefab, content);
                var lineRect = lineObj.GetComponent<RectTransform>();
                var line = lineObj.GetComponent<UILine>();

                line.fromNodeID = node.nodeID;
                line.toNodeID = nextID;

                line.Setup(from.anchoredPosition, to.anchoredPosition, LineState.Locked);

                lines.Add(line);

                lineRect.SetAsFirstSibling();
            }
        }

        foreach (var node in nodeMap.Values)
        {
            node.SetAsLastSibling();
        }
    }

    IEnumerator AfterBuild()
    {
        yield return null;
        yield return new WaitForEndOfFrame();

        DrawLines();
    }

    public void RefreshLocalLines(StageNodeData node, Dictionary<string, StageNodeData> nodes)
    {
        foreach (var nextID in node.nextNodeIDs)
        {
            var line = lines.Find(l => l.fromNodeID == node.nodeID && l.toNodeID == nextID);
            if (line == null) continue;

            var from = nodes[node.nodeID];
            var to = nodes[nextID];

            line.Refresh(from, to);
        }
    }
    public void RefreshNodeView(string nodeID)
    {
        if (viewMap.TryGetValue(nodeID, out var view))
        {
            view.RefreshState();
        }
    }
}