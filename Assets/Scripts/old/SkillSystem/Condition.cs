using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class Condition
{
    public bool satisfied = false;
    UnityEvent callback = new UnityEvent();

    /// <summary>
    /// 设置条件为真时的回调函数
    /// </summary>
    /// <param name="action"></param>
    public void SetCallback(UnityAction action)
    {
        callback.AddListener(action);
    }


    protected abstract void Init(string[] parameters);

    public bool ForceCheck()
    {
        satisfied = CustomForceCheck();
        return satisfied;
    }
    protected abstract bool CustomForceCheck();



    public void Run()
    {
        satisfied = ForceCheck();
        if (satisfied) callback.Invoke();
        StartListen();
    }

    protected abstract void StartListen();

    public static Condition Create(string conditionDef)
    {
        string[] split = conditionDef.Split(":");
        string name = split[0];
        string[] parameters = null;
        if (split.Length > 1)
        {
            parameters = split[1].Split(" ");
        }

        Type type = Type.GetType(name + "Condition");
        Condition condition = Activator.CreateInstance(type) as Condition;
        condition.Init(parameters);
        return condition;
    }
}
