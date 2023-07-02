using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 并行行为，同时执行，都执行完后结束
/// </summary>
[Serializable]
public class ParallelGameAction : GameAction
{
    [SerializeReference, SubclassSelector] public List<GameAction> parallelActions = new();

    bool allCompleted;


    protected override void Init()
    {
        allCompleted = false;
        AsyncGroup asyncGroup = new AsyncGroup(() => allCompleted = true, parallelActions.Count);
        for (int i = 0; i < parallelActions.Count; i++)
        {
            parallelActions[i].SetCallback(asyncGroup.OnNodeComplete);
            parallelActions[i].StartExecute();
        }
    }

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
}
