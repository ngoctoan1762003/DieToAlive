using UnityEngine;
using TMPro;

public class ChestUIItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    public void Init(string text)
    {
        itemName.text = text;
    }
}
