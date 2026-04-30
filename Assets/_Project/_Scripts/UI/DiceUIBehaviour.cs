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
    [SerializeField] private Transform diceBonusTransform;

    private Coroutine diceAnim;
    
    public void ShowDiceAnim(string name, int targetVal, List<int> buffs, Action<int> onComplete)
    {
        skillCardName.text = name;
        if (diceAnim != null) StopCoroutine(diceAnim);
        diceAnim = StartCoroutine(DiceAnim(targetVal, buffs, onComplete));
    }
    
    IEnumerator DiceAnim(int targetVal, List<int> buffs, Action<int> onComplete)
    {
        for (int i = 0; i < diceBonusTransform.childCount; i++)
        {
            diceBonusTransform.GetChild(i).gameObject.SetActive(false);
        }
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
        yield return new WaitForSeconds(0.1f);

        foreach (var buff in buffs)
        {
            targetVal += buff;
            DiceBuffUIBehaviour diceBuff = GameSystem.Instance.GetDiceBuff();
            diceBuff.ShowBuff((buff > 0 ? "+" : "-") + Mathf.Abs(buff));
            diceBuff.transform.SetParent(diceBonusTransform);
            diceBuff.transform.localScale = Vector3.one * 1.2f;
            diceText.transform.localScale = Vector3.one * 1.2f;
            diceText.text = targetVal.ToString();
            yield return new WaitForSeconds(0.1f);
            diceBuff.transform.localScale = Vector3.one;
            diceText.transform.localScale = Vector3.one;
            yield return new WaitForSeconds(0.05f);
        }
        
        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i < diceBonusTransform.childCount; i++)
        {
            diceBonusTransform.GetChild(i).gameObject.SetActive(false);
        }
        onComplete?.Invoke(targetVal);
        gameObject.SetActive(false);
    }
}
