using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHint : MonoBehaviour
{
    private static PanelHint instance;
    public static PanelHint Instance => instance;

    public List<GameObject> firstHint;
    public List<GameObject> secondHint;
    public List<GameObject> thirdHint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowHint(int index)
    {
        switch (ScriptController.Instance.CurrentTutorialID)
        {
            case TutorialID.First:
                firstHint[index].SetActive(true);
                break;
            case TutorialID.Second:
                secondHint[index].SetActive(true);
                break;
            case TutorialID.Third:
                thirdHint[index].SetActive(true);
                break;
        }
    }

    public void HideAll()
    {
        switch (ScriptController.Instance.CurrentTutorialID)
        {
            case TutorialID.First:
                foreach (GameObject hint in firstHint)
                {
                    hint.SetActive(false);
                }
                break;
            case TutorialID.Second:
                foreach (GameObject hint in secondHint)
                {
                    hint.SetActive(false);
                }
                break;
            case TutorialID.Third:
                foreach (GameObject hint in thirdHint)
                {
                    hint.SetActive(false);
                }
                break;
        }
    }
}
