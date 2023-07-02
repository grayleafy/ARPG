using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ResponseChoice
{
    [SerializeField] public string content;
    [SerializeField] public List<int> jumpIDs;
}

[Serializable]
public class DialogueSegment
{
    [SerializeField] public string speaker;
    [SerializeField] public int id;
    [SerializeField, SerializeReference, SubclassSelector] public ConditionChecker entryCondition = null;
    [SerializeField] public string content;
    [SerializeField] public List<int> defaultJumpIDs = new();
    [SerializeField] public List<ResponseChoice> responseChoices = new();
    [SerializeReference, SubclassSelector] public GameAction enterAction;
    [SerializeReference, SubclassSelector] public GameAction exitAction;


    public void Start()
    {
        if (enterAction != null) { enterAction.StartExecute(); }
    }

    public void End()
    {
        if (exitAction != null)
        {
            exitAction.StartExecute();
        }

        //事件中心
        EventCenter.GetInstance().EventTrigger<int>("对话完成", id);
    }
}
