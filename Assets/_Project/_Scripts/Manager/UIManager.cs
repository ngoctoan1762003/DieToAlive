using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    [SerializeField] private RectTransform canvasRect;
    
    [Header("UI References")]
    [SerializeField] private GameObject diceContainer;
    [SerializeField] private TextMeshProUGUI diceText;
    [SerializeField] private Button drawButton;
    [SerializeField] private CardPopupDescription popupDescription;
    
    [Header("Settings")]
    [SerializeField] private float rollDuration = 0.7f;
    [SerializeField] private int shuffleCount = 10;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        drawButton.onClick.AddListener(() => GameSystem.Instance.Draw(1));
        popupDescription.UpdateTransform(new Vector3(1000, 1000));
    }

    public void ShowDice(Action<int> onComplete)
    {
        int finalVal = Random.Range(1, 21);
        
        StopAllCoroutines();
        StartCoroutine(DiceAnim(finalVal, onComplete));
    }

    IEnumerator DiceAnim(int targetVal, Action<int> onComplete)
    {
        diceContainer.gameObject.SetActive(true);
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
        diceContainer.gameObject.SetActive(false);
    }

    public void ShowDescription(CardConfig config, Vector2 mousePos)
    {
        popupDescription.gameObject.SetActive(true);
        popupDescription.ShowInformation(config);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, 
            mousePos, 
            Camera.main, 
            out Vector2 localPoint
        );

        popupDescription.UpdateTransform(localPoint);
    }

    public void HideDescription()
    {
        popupDescription.UpdateTransform(new Vector3(1000, 1000));
    }
    
    public void UpdatePopupPosition(Vector2 mousePos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, 
            mousePos, 
            Camera.main, 
            out Vector2 localPoint
        );
        popupDescription.UpdateTransform(localPoint);
    }
}