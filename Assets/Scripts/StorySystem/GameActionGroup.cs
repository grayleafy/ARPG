using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 游戏行为组，当所有行为都结束后，调用回调事件，参数必须是公有类型，才能序列化，或者标记为[JsonProperty(Name = "my_private_field")]
/// </summary>
//[Serializable]
//public class GameActionGroup
//{
//    UnityEvent callback = new();
//    bool isCompleted = false;
//    [SerializeReference, SubclassSelector, JsonProperty] List<GameAction> actions = new List<GameAction>();

//    public void AddCallback(UnityAction fun)
//    {
//        callback.AddListener(fun);
//    }

//    public void StartExecute()
//    {
//        isCompleted = false;
//        AsyncGroup asyncGroup = new AsyncGroup(() =>
//        {
//            isCompleted = true;
//            callback.Invoke();
//        }, actions.Count);

//        for (int i = 0; i < actions.Count; i++)
//        {
//            actions[i].AddCallback(asyncGroup.OnNodeComplete);
//            actions[i].StartExecute();
//        }
//    }

//    public bool IsCompleted()
//    {
//        return isCompleted;
//    }
//}
