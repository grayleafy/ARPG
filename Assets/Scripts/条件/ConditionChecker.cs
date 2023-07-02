using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 参数必须是公有类型，才能序列化，或者标记为[JsonProperty(Name = "my_private_field")]
/// </summary>
[Serializable]
public abstract class ConditionChecker
{
    UnityAction callback = null;

    //设置添加满足的回调函数
    void SetCallback(UnityAction callback)
    {
        this.callback = callback;
    }

    /// <summary>
    /// 检测条件，满足后调用callback
    /// </summary>
    /// <param name="target"></param>
    /// <param name="callback"></param>
    public void MonitorCondition(GameObject target, UnityAction callback)
    {
        SetCallback(callback);

        if (StartListen(target))
        {

        }
        else
        {
            MonoMgr.GetInstance().StartCoroutine(StartEvaluate(target));   //会在切换场景时结束
        }
    }


    IEnumerator StartEvaluate(GameObject target)
    {
        while (true)
        {
            if (EvaluateCondition(target))
            {
                callback.Invoke();
                break;
            }
            UpdateProgress(target);
            yield return null;
        }
    }

    /// <summary>
    /// 监测条件开始时调用，用于给事件中心添加监听,如果实现了，则返回true,并且不再每一帧更新检测
    /// </summary>
    abstract protected bool StartListen(GameObject target);

    /// <summary>
    /// 具体检测方法
    /// </summary>
    /// <param name="targe"></param>
    /// <returns></returns>
    abstract public bool EvaluateCondition(GameObject target);

    /// <summary>
    /// 每一帧的推进
    /// </summary>
    /// <param name="target"></param>
    abstract protected void UpdateProgress(GameObject target);
}
