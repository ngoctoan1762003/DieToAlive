using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RestUI : MonoBehaviour
{
    public static RestUI Instance;

    public GameObject RestUIPanel;
    public GameObject RestUISuccessPanel;

    public Button RestButton;
    public Button ContinueButton;

    [SerializeField] private float animDuration = 0.25f;
    [SerializeField] private Ease easeType = Ease.OutBack;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowRestUI()
    {
        ShowPanel(RestUIPanel);
    }

    public void ShowRestSuccessUI()
    {
        HidePanel(RestUIPanel, () =>
        {
            ShowPanel(RestUISuccessPanel);
        });
    }

    private void Start()
    {
        RestButton.onClick.AddListener(() =>
        {
            float healAmount = GameSystem.Instance.Player.maxHP.value * 0.3f;
            GameSystem.Instance.Player.Heal(healAmount);

            GameSystem.Instance.SaveCurrentHP();
            ShowRestSuccessUI();
        });

        ContinueButton.onClick.AddListener(() =>
        {
            HidePanel(RestUISuccessPanel, () =>
            {
                MapManager.Instance.CompleteNode();
            });
        });
    }


    void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.localScale = new Vector3(1, 0, 1);

        rect.DOScaleY(1f, animDuration)
            .SetEase(easeType);
    }

    void HidePanel(GameObject panel, TweenCallback onComplete = null)
    {
        RectTransform rect = panel.GetComponent<RectTransform>();

        rect.DOScaleY(0f, animDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                panel.SetActive(false);
                onComplete?.Invoke();
            });
    }
}