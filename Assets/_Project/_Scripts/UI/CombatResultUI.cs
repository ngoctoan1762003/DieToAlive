using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatResultUI : MonoBehaviour
{
    public static CombatResultUI Instance;

    public GameObject Visual;

    public TextMeshProUGUI HeadingTXT;
    public Button ReplayButton;
    public Button ContinueButton;
    public TextMeshProUGUI GoldTXT;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ContinueButton.onClick.AddListener(delegate
        {
            Visual.SetActive(false);
            MapManager.Instance.CompleteNode();
        });

        ReplayButton.onClick.AddListener(delegate
        {
            Visual.SetActive(false);
            GameSystem.Instance.ReplayCombat();
        });
    }

    public void Win()
    {
        if (!SaveLoadManager.Instance.CheckTutorialDone("Tutorial2"))
        {
            ScriptController.Instance.RunProcessGuideStage2();
        }
        else
        {
            HeadingTXT.text = "Victory";
            int goldEarn = Random.RandomRange(10, 20);
            GoldTXT.text = goldEarn.ToString();
            ShopManager.Instance.AddGold(goldEarn);
            ContinueButton.gameObject.SetActive(true);
            ReplayButton.gameObject.SetActive(false);
            Visual.SetActive(true);
            LibraryManager.Instance.AddObservationPoints(1);
        }
    }

    public void Lose()
    {
        if (!SaveLoadManager.Instance.CheckTutorialDone("Tutorial3"))
        {
            ScriptController.Instance.RunProcessGuideStage3();
        }
        else
        {
            HeadingTXT.text = "Lose";
            int goldEarn = Random.RandomRange(0, 6);
            GoldTXT.text = goldEarn.ToString();
            ShopManager.Instance.AddGold(goldEarn);
            ReplayButton.gameObject.SetActive(true);
            ContinueButton.gameObject.SetActive(false);
            Visual.SetActive(true);
        }
    }

}
