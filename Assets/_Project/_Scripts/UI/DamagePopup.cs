using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour, IInPool
{
    [SerializeField] private float randomRange;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI damageTxt;
    
    public void ShowDamage(string damage, Vector2 pos)
    {
        damageTxt.text = damage;
        rectTransform.localPosition = Vector3.zero;
        rectTransform.anchoredPosition = pos + new Vector2(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange));
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    
    public void OnSpawn()
    {
    }

    public void OnDead()
    {
    }
}
