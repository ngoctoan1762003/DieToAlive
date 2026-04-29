using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverDOTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Target")]
    public RectTransform visual;
    public Image targetImage;

    [Header("Settings")]
    public float moveX = 20f;
    public float duration = 0.2f;

    private Vector2 originalPos;

    private Tween moveTween;
    private Tween fadeTween;

    private void Awake()
    {
        originalPos = visual.anchoredPosition;

        if (targetImage != null)
        {
            Color c = targetImage.color;
            c.a = 0f;
            targetImage.color = c;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        moveTween?.Kill();
        fadeTween?.Kill();

        moveTween = visual.DOAnchorPosX(originalPos.x + moveX, duration)
            .SetEase(Ease.OutQuad);

        if (targetImage != null)
        {
            fadeTween = targetImage.DOFade(1f, duration);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        moveTween?.Kill();
        fadeTween?.Kill();

        moveTween = visual.DOAnchorPosX(originalPos.x, duration)
            .SetEase(Ease.OutQuad);

        if (targetImage != null)
        {
            fadeTween = targetImage.DOFade(0f, duration);
        }
    }
}