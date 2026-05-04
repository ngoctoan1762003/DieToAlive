using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelHighlight : MonoBehaviour
{
    public const int MODE_NORMAL = 0;
    public const int MODE_SCALE = 1;
    public const int SUPER_HIGHLIGHT = 2;
    public const int SUPER_HIGHLIGHT_TOP_OBJECT = 3;
    
    private static PanelHighlight instance;
    public static PanelHighlight Instance => instance;

    [SerializeField] private Transform container;
    private List<UIHightLightPack> listUIHightLightPack = new();
    private Dictionary<Transform, Sequence> _dictionaryAnim;

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

    public void SetActive(bool active)
    {
        if (active)
        {
            listUIHightLightPack = new List<UIHightLightPack>();
            _dictionaryAnim = new Dictionary<Transform, Sequence>();
        }
        else
        {
            AllComeToOldParent();
            listUIHightLightPack = null;
        }
        container.gameObject.SetActive(active);
    }
    
    public void AddHightLightObjEndAction(Transform uiObj, int showMode, TweenCallback callback)
    {
        UIHightLightPack uiHightLightPack = new UIHightLightPack(uiObj);
        listUIHightLightPack.Add(uiHightLightPack);
        uiObj.SetParent(container);
        CheckShowModeHighlight(uiObj,showMode,callback);
    }
   
   
    public void AddHightLightObj(Transform uiObj,int showMode = MODE_NORMAL)
    {
        UIHightLightPack uiHightLightPack = new UIHightLightPack(uiObj);
        listUIHightLightPack.Add(uiHightLightPack);
        uiObj.SetParent(container);
        CheckShowModeHighlight(uiObj,showMode);
    }

    public void ComeToOldParent(UIHightLightPack uiHightLightPack)
    {
        Transform obj = uiHightLightPack.uiObj;
        obj.SetParent(uiHightLightPack.lastParent);
        obj.SetSiblingIndex(uiHightLightPack.lastIndex);
        StopAnim(obj);
    }

    public bool CheckIsHighlight(Transform obj)
    {
        if (listUIHightLightPack == null) return false;
        return listUIHightLightPack.Any(ui => ui.uiObj == obj);
    }

    public void ReturnObjToParent(Transform obj)
    {
        foreach (var hightLight in listUIHightLightPack)
        {
            if (hightLight.uiObj == obj) ComeToOldParent(hightLight);
        }
    }

    public void AllComeToOldParent()
    {
        if(listUIHightLightPack == null) return;
        foreach (var hightLight in listUIHightLightPack)
        {
            ComeToOldParent(hightLight);
        }
        listUIHightLightPack.Clear();
    }
    
    public void CheckShowModeHighlight(Transform trans,int mode, TweenCallback callback = null)
    {
        switch (mode)
        {
            case MODE_SCALE:
            {
                AnimScaleMode(trans);
                break;
            }
            case SUPER_HIGHLIGHT:
            {
                AnimSuperHighlight(trans,callback);
                break;
            }
            case SUPER_HIGHLIGHT_TOP_OBJECT:
            {
                AnimSuperHighlightBottomPoint(trans,callback);
                break;
            }
            default:
                break;
        }
    }
    
    public void AnimScaleMode(Transform trans)
   {
      StopAnim(trans);
      Sequence mySequence = DOTween.Sequence();
      mySequence.Append(trans.transform.DOScale(new Vector2(1.3f,1.3f),0.25f)).SetEase(Ease.InBounce);
      mySequence.Append(trans.transform.DOScale(new Vector2(1,1), 1f));
      mySequence.SetLoops(-1);
      mySequence.Play();
      mySequence.OnKill(() => { trans.transform.localScale = new Vector2(1, 1); });
      _dictionaryAnim.Add(trans,mySequence);
   }

   public void AnimSuperHighlight(Transform trans, TweenCallback callback  = null)
   {
      StopAnim(trans);
      var savePos = trans.position;
      trans.position = Vector3.zero - new Vector3(0,3,0);
      trans.localScale = Vector3.one;
      
      Sequence mySequence1 = DOTween.Sequence();
      mySequence1.Append(trans.DOScale(Vector3.one * 2.5f, 0.5f).SetEase(Ease.OutBack));
      mySequence1.Join(trans.DOMove(Vector3.zero, 0.5f));
      mySequence1.Append(trans.DOScale(Vector3.one, 0.5f));
      mySequence1.Append(trans.DOMove(savePos,  0.5f));

      Sequence mySequence2 = DOTween.Sequence();
      mySequence2.Append(trans.transform.DOScale(new Vector2(1.5f, 1.5f), 0.5f)).SetEase(Ease.OutBack);
      mySequence2.AppendCallback(() => {
         trans.transform.DOScale(new Vector2(1, 1), 1f);
      });
      mySequence2.SetLoops(-1);

      Sequence mySequence = DOTween.Sequence();
      mySequence.Append(mySequence1);
      mySequence.Append(mySequence2);
      if(callback != null)  mySequence.AppendCallback(callback);
      mySequence.Play();
      mySequence.OnKill(() => { trans.transform.localScale = new Vector2(1, 1); });
      mySequence.OnComplete(() =>
      {

      });
      
      _dictionaryAnim.Add(trans,mySequence);
   }
    
    public void AnimSuperHighlightBottomPoint(Transform trans, TweenCallback callback  = null)
   {
      StopAnim(trans);
      var savePos = trans.position;
      trans.position = Vector3.zero - new Vector3(0,3,0);
      trans.localScale = Vector3.one;
      
      Sequence mySequence1 = DOTween.Sequence();
      mySequence1.Append(trans.DOScale(Vector3.one * 2.5f, 0.5f).SetEase(Ease.OutBack));
      mySequence1.Join(trans.DOMove(Vector3.zero, 0.5f));
      mySequence1.Append(trans.DOScale(Vector3.one, 0.5f));
      mySequence1.Append(trans.DOMove(savePos,  0.5f));

      Sequence mySequence2 = DOTween.Sequence();
      mySequence2.Append(trans.transform.DOScale(new Vector2(1.5f, 1.5f), 0.5f)).SetEase(Ease.OutBack);
      mySequence2.AppendCallback(() => {
         trans.transform.DOScale(new Vector2(1, 1), 1f);
      });
      mySequence2.SetLoops(-1);

      Sequence mySequence = DOTween.Sequence();
      mySequence.Append(mySequence1);
      mySequence.Append(mySequence2);
      if(callback != null)  mySequence.AppendCallback(callback);
      mySequence.Play();
      mySequence.OnKill(() => { trans.transform.localScale = new Vector2(1, 1); });
      mySequence.OnComplete(() =>
      {
         
      });
      
      _dictionaryAnim.Add(trans,mySequence);
   }

   public void StopAnim(Transform transform)
   {
      if (_dictionaryAnim.ContainsKey(transform))
      {
         _dictionaryAnim[transform].Kill();
         _dictionaryAnim.Remove(transform);
      }
   }
}

public class UIHightLightPack
{
    public Transform lastParent;
    public int lastIndex;
    public Transform uiObj;
    public UIHightLightPack(Transform uiObj)
    {
        this.uiObj = uiObj;
        lastIndex = uiObj.GetSiblingIndex();
        lastParent = uiObj.parent;
    }
}