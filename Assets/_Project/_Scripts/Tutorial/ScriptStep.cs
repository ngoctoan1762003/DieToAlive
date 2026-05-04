using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScriptStep
{
    // Flow như sau
    // OnStepStart -> lock -> OnUnlockTut -> CheckCompletion -> OnStepComplete -> CheckNextStep -> OnStepStart(next step)
    public int stepIndex { get; private set; }
    public float lockTutTime { get; private set; }
    public float delayTime { get; private set; }
    protected bool isComplete;
    protected bool isStartStep;
    protected bool isLockStep;
    protected bool isCanNextStep;

    protected Action startAction = null;
    protected Action onCompleteAction = null;

    public ScriptStep(int step, float delayStartTime, float lockTutTime, Action startAction,
        Action onCompleteAction = null)
    {
        stepIndex = step;
        this.lockTutTime = lockTutTime;
        this.startAction = startAction;
        this.onCompleteAction = onCompleteAction;
        delayTime = delayStartTime;
    }

    public bool IsComplete
    {
        get { return isComplete; }
    }


    public bool IsStartStep
    {
        get { return isStartStep; }
    }


    public bool IsCanNextStep
    {
        get { return isCanNextStep; }
    }


    public virtual bool CheckCompletion()
    {
        if (isStartStep == true && isComplete == false && isLockStep == false) return true;
        else return false;
    }

    public virtual void OnStepStart(bool isUnScaleTime = false)
    {
        if (isStartStep) return;
        isLockStep = true;
        isComplete = false;
        isStartStep = true;
        isCanNextStep = false;

        if (delayTime > 0)
            DOVirtual.DelayedCall(delayTime, () =>
            {
                try
                {
                    if (startAction != null)
                    {
                        startAction(); // If this throws, the next lines will never run.
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Exception in startAction: " + ex);
                }

                DOVirtual.DelayedCall(lockTutTime, OnUnlockTut);
            }).SetUpdate(isUnScaleTime);

        else
        {
            if (startAction != null) startAction();
            DOVirtual.DelayedCall(lockTutTime, OnUnlockTut);
        }
    }


    public virtual void OnUnlockTut()
    {
        isLockStep = false;
    }

    public virtual void OnStepComplete()
    {
        isComplete = true;
        if (onCompleteAction != null) onCompleteAction();
    }

    public void SetCanNextStep(bool canNextStep)
    {
        isCanNextStep = canNextStep;
    }
}

public class ScriptAnyActionNext : ScriptStep
{
    public ScriptAnyActionNext(int step, float delayStartTime, float lockTutTime, Action startAction,
        Action onCompleteAction) : base(step,
        delayStartTime, lockTutTime, startAction, onCompleteAction)
    {
    }

    public override bool CheckCompletion()
    {
        if (base.CheckCompletion())
        {
            if (Input.anyKey)
            {
                return true;
            }

            return false;
        }

        return false;
    }
}

public class ScriptChooseCard : ScriptStep
{
    private bool isSelect;

    public ScriptChooseCard(int step, float delayStartTime, float lockTutTime, Action startAction,
        Action onCompleteAction) : base(step,
        delayStartTime, lockTutTime, startAction, onCompleteAction)
    {
    }

    public override bool CheckCompletion()
    {
        return base.CheckCompletion() && isSelect;
    }
    
    public void OnChooseCard()
    {
        if (IsStartStep && !IsComplete) isSelect = true;
    }
}

public class ScriptOpenLibrary : ScriptStep
{
    private bool isSelect;

    public ScriptOpenLibrary(int step, float delayStartTime, float lockTutTime, Action startAction,
        Action onCompleteAction) : base(step,
        delayStartTime, lockTutTime, startAction, onCompleteAction)
    {
    }

    public override bool CheckCompletion()
    {
        return base.CheckCompletion() && isSelect;
    }
    
    public void OnOpenLibrary()
    {
        if (IsStartStep && !IsComplete) isSelect = true;
    }
}

public class ScriptOpenUnitInformation : ScriptStep
{
    private bool isSelect;

    public ScriptOpenUnitInformation(int step, float delayStartTime, float lockTutTime, Action startAction,
        Action onCompleteAction) : base(step,
        delayStartTime, lockTutTime, startAction, onCompleteAction)
    {
    }

    public override bool CheckCompletion()
    {
        return base.CheckCompletion() && isSelect;
    }
    
    public void OnOpenUnitInformation()
    {
        if (IsStartStep && !IsComplete) isSelect = true;
    }
}

public class ScriptUnlockInformation : ScriptStep
{
    private bool isSelect;

    public ScriptUnlockInformation(int step, float delayStartTime, float lockTutTime, Action startAction,
        Action onCompleteAction) : base(step,
        delayStartTime, lockTutTime, startAction, onCompleteAction)
    {
    }

    public override bool CheckCompletion()
    {
        return base.CheckCompletion() && isSelect;
    }
    
    public void OnUnlockUnitInformation()
    {
        if (IsStartStep && !IsComplete) isSelect = true;
    }
}

public class ScriptCloseLibrary : ScriptStep
{
    private bool isSelect;

    public ScriptCloseLibrary(int step, float delayStartTime, float lockTutTime, Action startAction,
        Action onCompleteAction) : base(step,
        delayStartTime, lockTutTime, startAction, onCompleteAction)
    {
    }

    public override bool CheckCompletion()
    {
        return base.CheckCompletion() && isSelect;
    }
    
    public void OnCloseLibrary()
    {
        if (IsStartStep && !IsComplete) isSelect = true;
    }
}

public class ScriptUseCard : ScriptStep
{
    private bool isSelect;

    public ScriptUseCard(int step, float delayStartTime, float lockTutTime, Action startAction,
        Action onCompleteAction) : base(step,
        delayStartTime, lockTutTime, startAction, onCompleteAction)
    {
    }

    public override bool CheckCompletion()
    {
        return base.CheckCompletion() && isSelect;
    }
    
    public void OnUseCard()
    {
        if (IsStartStep && !IsComplete) isSelect = true;
    }
}

public class ScriptReadyCard : ScriptStep
{
    private bool isSelect;

    public ScriptReadyCard(int step, float delayStartTime, float lockTutTime, Action startAction,
        Action onCompleteAction) : base(step,
        delayStartTime, lockTutTime, startAction, onCompleteAction)
    {
    }

    public override bool CheckCompletion()
    {
        return base.CheckCompletion() && isSelect;
    }
    
    public void OnReadyCard()
    {
        if (IsStartStep && !IsComplete) isSelect = true;
    }
}

public class ScriptDrawCard : ScriptStep
{
    private bool isSelect;

    public ScriptDrawCard(int step, float delayStartTime, float lockTutTime, Action startAction,
        Action onCompleteAction) : base(step,
        delayStartTime, lockTutTime, startAction, onCompleteAction)
    {
    }

    public override bool CheckCompletion()
    {
        return base.CheckCompletion() && isSelect;
    }
    
    public void OnDrawCard()
    {
        if (IsStartStep && !IsComplete) isSelect = true;
    }
}