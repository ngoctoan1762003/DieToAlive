using TMPro;
using UnityEngine;

public class StatisticText : MonoBehaviour, IInPool
{
    [SerializeField] private TextMeshProUGUI statisticText;
    
    public void SetText(string text)
    {
        statisticText.text = text;
    }
    
    public void OnSpawn()
    {
    }

    public void OnDead()
    {
    }
}
