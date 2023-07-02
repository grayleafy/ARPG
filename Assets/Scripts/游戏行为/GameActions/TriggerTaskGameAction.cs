using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TriggerTaskGameAction : GameAction
{
    public int taskID;


    protected override bool CheckActionComplete()
    {
        return true;
    }

    protected override void ExecutePerFrame()
    {
        TaskManager.GetInstance().TriggerTask(taskID);
    }

    protected override void Finish()
    {

    }

    protected override void Init()
    {

    }
}
