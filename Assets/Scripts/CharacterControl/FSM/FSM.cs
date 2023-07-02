using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class FSMSettingInfo
{
    string name;
}


[Serializable]
public class TransitionInfo
{
    [HideInInspector] public string name;
    [HideInInspector] public FSMStateType state;
    public List<TriggerInfo> triggers = new();
}

[Serializable]
public class TriggerInfo
{
    public FSMTriggerType trigger;
    public FSMStateType targetState;
}

[Serializable]
public class FSM
{


    [Header("状态机设置")]
    [SerializeField] public string name;
    [SerializeField] FSMStateType defaultStateType;
    [SerializeField, SerializeReference] public List<FSMState> states = new();
    [SerializeField, SerializeReference] List<TransitionInfo> transitionConfigure = new();

    [Header("额外参数")]
    [SerializeReference, SubclassSelector] List<FSMSettingInfo> settings = new();


    [Header("运行时数据")]
    [SerializeField] public GameObject character;
    [SerializeField] public FSMStateType currentStateType;


    Dictionary<string, object> settingDit = new();   //额外参数
    FSMState lastState;
    [HideInInspector] public FSMState currentState;


    [HideInInspector] public int interactingID;
    [HideInInspector] public Animator animator;



    /// <summary>
    /// 加入状态，设置映射，当前状态初始化
    /// </summary>
    public void Init(HFSM hfsm)
    {
        character = hfsm.gameObject;
        animator = character.GetComponent<Animator>();
        interactingID = Animator.StringToHash("Interacting");

        //创建状态和转换条件
        InitState();
        InitTrigger();

        //当前状态初始化
        currentState = FindState(defaultStateType);
        currentState.EnterState(this);
    }

    void InitState()
    {
        for (int i = 0; i < states.Count; i++)
        {
            states[i].Init(this);
        }
    }

    void InitTrigger()
    {
        for (int i = 0; i < transitionConfigure.Count; i++)
        {
            for (int j = 0; j < transitionConfigure[i].triggers.Count; j++)
            {
                states[i].AddMap(transitionConfigure[i].triggers[j].trigger, transitionConfigure[i].triggers[j].targetState, this);
            }
        }
    }

    public void ReSet()
    {
        //当前状态初始化
        currentState = FindState(defaultStateType);
        currentState.EnterState(this);
    }

    /// <summary>
    /// states.Add(new OnGroundState()); state.Initialize()
    /// </summary>
    void SetStatesAndInitialize() { }

    /// <summary>
    /// base.FindState().AddMap(FSMTriggerID.JumpTab, FSMStateID.Jump, this);
    /// </summary>
    void SetTriggers() { }



    public void Update()
    {
        FSMState nextState = currentState.Reason(this);
        if (nextState != null) { ChangeActiveState(nextState); }

        currentState.ActionState(this);
    }

    public void FixedUpdate()
    {
        currentState.FixedActionState(this);
    }

    //改变状态
    public void ChangeActiveState(FSMState state)
    {
        //更新lastState
        lastState = currentState;
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }


    //根据ID找到状态
    public FSMState FindState(FSMStateType stateID)
    {
        return states.Find(t => t.type == stateID);
    }


    #region abondon

    #endregion
}
