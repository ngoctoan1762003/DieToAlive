using System;
using UnityEngine;
using UnityEngine.UI;

public class DisableBG : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            GameSystem.Instance.ShowTarget(false);
        });
    }
}
