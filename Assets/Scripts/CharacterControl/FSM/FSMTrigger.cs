using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FSMTriggerType
{

}

[Serializable]
public abstract class FSMTrigger
{
    /// <summary>
    /// 类的命名必须是triggerID + Trigger
    /// </summary>
    public FSMTriggerType type;

    public FSMTrigger()
    {
        InitType();
    }

    //必须初始化ID
    public abstract void InitType();

    //处理条件是否成立
    public abstract bool HandleTrigger(FSM fsm);

    public static FSMTrigger CreateTrigger(FSMTriggerType triggerType)
    {
        Type type = Type.GetType(triggerType + "Trigger");
        return Activator.CreateInstance(type) as FSMTrigger;
    }
}



