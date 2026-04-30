using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DiceBuffUIBehaviour : MonoBehaviour, IInPool
{
    [SerializeField] private TextMeshProUGUI buffTxt;
    
    public void ShowBuff(string buff)
    {
        buffTxt.text = buff;
    }
    
    public void OnSpawn()
    {
    }

    public void OnDead()
    {
    }
}
