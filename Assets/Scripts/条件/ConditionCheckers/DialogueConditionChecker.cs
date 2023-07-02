using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueConditionChecker : ConditionChecker
{
    public int dialogueID;

    bool isFinished;

    public override bool EvaluateCondition(GameObject target)
    {
        return isFinished;
    }

    protected override bool StartListen(GameObject target)
    {
        EventCenter.GetInstance().AddEventListener<int>("对话完成", ListenDialogue);
        return false;
    }

    protected override void UpdateProgress(GameObject target)
    {

    }

    void ListenDialogue(int id)
    {
        if (id == dialogueID)
        {
            isFinished = true;
            EventCenter.GetInstance().RemoveEventListener<int>("对话完成", ListenDialogue);
        }
    }
}
