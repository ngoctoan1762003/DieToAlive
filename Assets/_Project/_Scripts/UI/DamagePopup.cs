using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        rectTransform.localScale = Vector3.one;
        
        rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + 30, 1f);
        rectTransform.DOScale(Vector3.one * 1.2f, 0.3f);
        DOVirtual.DelayedCall(0.3f, () =>
        {
            rectTransform.DOScale(Vector3.one * 0.5f, 0.3f);
        });
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
