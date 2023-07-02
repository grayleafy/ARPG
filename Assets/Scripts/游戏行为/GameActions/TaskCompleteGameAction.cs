using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TaskCompleteGameAction : GameAction
{
    public int taskID;

    protected override bool CheckActionComplete()
    {
        return true;
    }

    protected override void ExecutePerFrame()
    {

    }

    protected override void Finish()
    {

    }

    protected override void Init()
    {
        var task = TaskManager.GetInstance().Tasks.Find((x) => x.id == taskID);
        if (task != null)
        {
            task.CompleteTask();
        }
    }
}
