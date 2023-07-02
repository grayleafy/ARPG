using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 串行行为，依次执行，上一个行为结束后再执行下一个
/// </summary>
[Serializable]
public class SequentialGameAction : GameAction
{
    [SerializeReference, SubclassSelector] public List<GameAction> sequentialActions = new();


    bool allCompleted;


    protected override bool CheckActionComplete()
    {
        return allCompleted;
    }

    protected override void ExecutePerFrame()
    {

    }

    protected override void Finish()
    {

    }

    protected override void Init()
    {
        allCompleted = false;
        for (int i = 0; i < sequentialActions.Count - 1; i++)
        {
            int temp = i + 1;
            sequentialActions[i].SetCallback(() => sequentialActions[temp].StartExecute());
        }
        if (sequentialActions.Count > 0)
        {
            sequentialActions[sequentialActions.Count - 1].SetCallback(() => allCompleted = true);
            sequentialActions[0].StartExecute();
        }
        else
        {
            allCompleted = true;
        }

    }

    //强制打断
    public override void ForceInterrupt()
    {
        base.ForceInterrupt();
        for (int i = 0; i < sequentialActions.Count; i++)
        {
            sequentialActions[i].ForceInterrupt();
        }
    }
}
