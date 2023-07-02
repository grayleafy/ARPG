using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//public abstract class GameAction
//{
//    bool finished = false;
//    UnityEvent callback = new();

//    protected abstract void Init(string[] args);

//    public static GameAction Create(string gameActionDef)
//    {
//        string[] parts = gameActionDef.Split(":");
//        string actionName = parts[0];
//        string[] args = parts[1].Split(" ");
//        Type type = Type.GetType(actionName + "GameAction");
//        GameAction gameAction = Activator.CreateInstance(type) as GameAction;
//        gameAction.Init(args);
//        return gameAction;
//    }

//    public void SetCallback(UnityAction action)
//    {
//        callback.AddListener(action);
//    }

//    protected abstract void Start(object[] args);

//    /// <summary>
//    /// 每一帧更新
//    /// </summary>
//    /// <param name="args"></param>
//    protected abstract void Update(object[] args);

//    protected abstract bool CheckFinish(object[] args);

//    public IEnumerator Run(object[] args)
//    {
//        Start(args);


//        while (!(finished = CheckFinish(args)))
//        {
//            Update(args);
//            yield return null;
//        }
//        callback.Invoke();
//        yield return null;
//    }
//}
