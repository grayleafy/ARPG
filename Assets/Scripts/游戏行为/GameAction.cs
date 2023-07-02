using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class GameAction
{
    UnityAction callback = null;
    bool isCompleted = true;
    bool isInterupted = false;

    public void SetCallback(UnityAction callback)
    {
        this.callback = callback;
    }

    public void StartExecute()
    {
        Init();
        MonoMgr.GetInstance().StartCoroutine(ReallyExecute());
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }

    IEnumerator ReallyExecute()
    {
        isCompleted = false;
        isInterupted = false;
        while (true)
        {
            if (isInterupted)
            {
                Finish();
                break;
            }
            if (isCompleted || ExecuteAndCheckActionComplete())
            {
                isCompleted = true;
                Finish();
                if (callback != null) callback();
                break;
            }
            else
            {
                yield return null;
            }
        }
    }

    bool ExecuteAndCheckActionComplete()
    {
        ExecutePerFrame();
        return CheckActionComplete();
    }


    /// <summary>
    /// 强制打断，不会执行回调
    /// </summary>
    public virtual void ForceInterrupt()
    {
        isInterupted = true;
    }
    protected abstract void Init();

    /// <summary>
    /// 结束后执行的逻辑
    /// </summary>
    protected abstract void Finish();

    protected abstract void ExecutePerFrame();

    protected abstract bool CheckActionComplete();
}
