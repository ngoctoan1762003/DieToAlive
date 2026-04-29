using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button SettingButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private Button BackButton;
    [SerializeField] private GameObject SelectModePanel;
    [SerializeField] private GameObject SettingPanel;
    [SerializeField] private GameObject Heading;
    [SerializeField] private Button StandardModeButton;

    [Header("Settings")]
    [SerializeField] private float offsetX = 800f;
    [SerializeField] private float duration = 0.4f;
    [SerializeField] private float delayStep = 0.1f;
    [SerializeField] private float headingOffsetY = 100f;
    [SerializeField] private float bounceOffsetY = 40f;

    private RectTransform[] rects;
    private Vector2[] originalPos;
    private Tween[] tweens;
    private RectTransform backRect;
    private Vector2 backOriginalPos;
    private Tween backTween;
    private GameObject CurrentPanel;
    private RectTransform headingRect;
    private Vector2 headingOriginalPos;
    private Tween headingTween;
    private RectTransform standardRect;
    private Vector2 standardOriginalPos;
    private Tween standardTween;


    private void Awake()
    {
        rects = new RectTransform[]
        {
            PlayButton.GetComponent<RectTransform>(),
            SettingButton.GetComponent<RectTransform>(),
            QuitButton.GetComponent<RectTransform>()
        };

        originalPos = new Vector2[rects.Length];
        tweens = new Tween[rects.Length];

        for (int i = 0; i < rects.Length; i++)
        {
            originalPos[i] = rects[i].anchoredPosition;
        }

        backRect = BackButton.GetComponent<RectTransform>();
        backOriginalPos = backRect.anchoredPosition;

        headingRect = Heading.GetComponent<RectTransform>();
        headingOriginalPos = headingRect.anchoredPosition;

        standardRect = StandardModeButton.GetComponent<RectTransform>();
        standardOriginalPos = standardRect.anchoredPosition;

        TweenHeadingIn();
        TweenIn();
    }

    public void TweenIn()
    {
        for (int i = 0; i < rects.Length; i++)
        {
            int index = i;
            tweens[index]?.Kill();

            rects[index].anchoredPosition = new Vector2(
                originalPos[index].x - offsetX,
                originalPos[index].y
            );

            tweens[index] = rects[index]
                .DOAnchorPos(originalPos[index], duration)
                .SetDelay(index * delayStep)
                .SetEase(Ease.OutCubic);
        }

        backTween?.Kill();

        backRect.anchoredPosition = backOriginalPos;

        backTween = backRect
            .DOAnchorPos(
                new Vector2(backOriginalPos.x - offsetX, backOriginalPos.y),
                duration
            )
            .SetEase(Ease.InCubic);
    }

    public void TweenOut()
    {
        for (int i = 0; i < rects.Length; i++)
        {
            int index = i;

            tweens[index]?.Kill();

            tweens[index] = rects[index]
                .DOAnchorPos(
                    new Vector2(originalPos[index].x - offsetX, originalPos[index].y),
                    duration
                )
                .SetDelay(index * delayStep)
                .SetEase(Ease.InCubic);
        }

        backTween?.Kill();

        backRect.anchoredPosition = new Vector2(
            backOriginalPos.x - offsetX,
            backOriginalPos.y
        );

        backTween = backRect
            .DOAnchorPos(backOriginalPos, duration)
            .SetEase(Ease.OutCubic);
    }

    private void Start()
    {
        PlayButton.onClick.AddListener(delegate
        {
            CurrentPanel = SelectModePanel;
            CurrentPanel.SetActive(true);
            PlayStandardButtonBounce();
            TweenOut();
        });
        SettingButton.onClick.AddListener(delegate
        {
            CurrentPanel = SettingPanel;
            CurrentPanel.SetActive(true);
            TweenOut();
        });
        QuitButton.onClick.AddListener(delegate
        {
            TweenOut();
            Application.Quit();
        });
        BackButton.onClick.AddListener(delegate
        {
            CurrentPanel.SetActive(false);
            CurrentPanel = null;
            TweenIn();
        });
    }
    private void TweenHeadingIn()
    {
        headingTween?.Kill(true);

        headingRect.anchoredPosition = new Vector2(
            headingOriginalPos.x,
            headingOriginalPos.y + headingOffsetY
        );

        headingTween = headingRect
            .DOAnchorPos(headingOriginalPos, duration)
            .SetEase(Ease.OutCubic);
    }
    private void PlayStandardButtonBounce()
    {
        standardTween?.Kill(true);

        standardRect.anchoredPosition = new Vector2(
            standardOriginalPos.x,
            standardOriginalPos.y - bounceOffsetY
        );

        standardTween = standardRect
            .DOAnchorPosY(standardOriginalPos.y, 0.3f)
            .SetEase(Ease.OutCubic);
    }

    private void OnDestroy()
    {
        PlayButton.onClick.RemoveAllListeners();
        SettingButton.onClick.RemoveAllListeners();
        QuitButton.onClick.RemoveAllListeners();
        BackButton.onClick.RemoveAllListeners();
    }
}