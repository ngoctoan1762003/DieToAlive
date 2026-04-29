using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonHoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    [SerializeField] private float scaleUp = 1.1f;
    [SerializeField] private float duration = 0.15f;

    private RectTransform rect;
    private Vector3 originalScale;
    private Tween scaleTween;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalScale = rect.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        scaleTween?.Kill(true);

        scaleTween = rect
            .DOScale(originalScale * scaleUp, duration)
            .SetEase(Ease.OutCubic);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scaleTween?.Kill(true);

        scaleTween = rect
            .DOScale(originalScale, duration)
            .SetEase(Ease.OutCubic);
    }
}