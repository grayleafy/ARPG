using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 转换器，使得技能系统可以调用普通的gameAction
/// </summary>
[Serializable]
public class UniversalConverterGameActionWithReleaser : GameActionWithReleaser
{
    [SerializeReference, SubclassSelector] public GameAction action;
    protected override bool CheckActionComplete()
    {
        return action.IsCompleted();
    }

    protected override void ExecutePerFrame()
    {

    }

    protected override void Finish()
    {

    }

    protected override void Init()
    {
        action.StartExecute();
    }
}
