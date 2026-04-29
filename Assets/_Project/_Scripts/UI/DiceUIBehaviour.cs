using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceUIBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI diceText;
        
    [Header("Settings")]
    [SerializeField] private float rollDuration = 0.7f;
    [SerializeField] private int shuffleCount = 10;
    [SerializeField] private TextMeshProUGUI skillCardName;

    private Coroutine diceAnim;
    
    public void ShowDiceAnim(string name, int targetVal, Action<int> onComplete)
    {
        skillCardName.text = name;
        if (diceAnim != null) StopCoroutine(diceAnim);
        diceAnim = StartCoroutine(DiceAnim(targetVal, onComplete));
    }
    
    IEnumerator DiceAnim(int targetVal, Action<int> onComplete)
    {
        float waitTime = rollDuration / shuffleCount;

        for (int i = 0; i < shuffleCount; i++)
        {
            diceText.text = Random.Range(1, 21).ToString();
            
            yield return new WaitForSeconds(waitTime);
        }

        diceText.text = targetVal.ToString();
        
        diceText.transform.localScale = Vector3.one * 1.2f;
        yield return new WaitForSeconds(0.1f);
        diceText.transform.localScale = Vector3.one;
        yield return new WaitForSeconds(1f);
        onComplete?.Invoke(targetVal);
        gameObject.SetActive(false);
    }
}
