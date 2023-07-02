
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[Serializable]
public enum FSMStateType
{
    NormalControl,
    Idle,
}

/// <summary>
/// 类的命名必须是stateID + State
/// </summary>
[Serializable]
public abstract class FSMState
{
    [HideInInspector] public string name;
    [HideInInspector] public FSMStateType type;
    public Dictionary<FSMTrigger, FSMState> map = new();
    public bool isFinished = false;



    public FSMState()
    {
        InitType();
    }

    public abstract void InitType();

    public abstract void Init(FSM fsm);

    /// <summary>
    /// 状态执行完的判断，如果状态可随时中断，则直接设置isFinished = true;
    /// </summary>
    /// <returns></returns>
    public abstract bool CheckIsFinished(FSM fsm);

    /// <summary>
    /// 动画结束，通常用于CheckIsFinished();
    /// </summary>
    /// <param name="fsm"></param>
    /// <returns></returns>
    public bool IsAnimatorFinished(FSM fsm)
    {
        return !fsm.animator.GetBool(fsm.interactingID);
    }

    //状态持续内每一帧的更新
    public virtual void ActionState(FSM fsm) { isFinished = CheckIsFinished(fsm); }
    //物理相关更新
    public virtual void FixedActionState(FSM fsm) { isFinished = CheckIsFinished(fsm); }
    //进入状态
    public virtual void EnterState(FSM fsm)
    {
        isFinished = false;
    }
    //离开状态
    public virtual void ExitState(FSM fsm) { }

    //判断条件,返回下一个状态
    public FSMState Reason(FSM fsm)
    {
        if (!isFinished) { return null; }
        FSMState nextState = null;
        foreach (var trigger in map.Keys)
        {
            if (trigger.HandleTrigger(fsm))
            {
                nextState = map[trigger];
                break;
            }
        }
        return nextState;
    }

    /// <summary>
    /// 添加映射关系,必须在状态机创建状态之后再调用
    /// </summary>
    /// <param name="triggerID"></param>
    /// <param name="stateID"></param>
    /// <param name="fsm"></param>
    public void AddMap(FSMTriggerType triggerID, FSMStateType stateID, FSM fsm)
    {
        FSMTrigger trigger = FSMTrigger.CreateTrigger(triggerID);
        if (map.ContainsKey(trigger)) { return; }

        FSMState state = fsm.states.Find(t => t.type == stateID);
        Assert.IsTrue(state != null);

        map.Add(trigger, state);
    }

    //根据ID创建state
    public static FSMState CreateFSMState(FSMStateType stateID)
    {
        Type type = Type.GetType(stateID + "State");
        return Activator.CreateInstance(type) as FSMState;
    }
}


