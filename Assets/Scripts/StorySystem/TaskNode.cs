using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TaskNode
{
    public string name;
    public int id;
    public List<int> predecessors = new();
    public List<int> successors = new();
    [SerializeReference, SubclassSelector] public ConditionChecker entryCondition;
    [SerializeReference, SubclassSelector] public ConditionChecker completeCondition;
    [SerializeReference, SubclassSelector] public GameAction entryAction;
    [SerializeReference, SubclassSelector] public GameAction completeAction;

    //判断是否还有前置节点
    public void TryTrigger(Task task, int triggerNodeIndex)
    {
        predecessors.Remove(triggerNodeIndex);
        if (predecessors.Count == 0)
        {
            Trigger(task);
        }
    }

    void Trigger(Task task)
    {
        task.notTriggerNodes.Remove(this);
        task.triggeredNotStartedNodes.Add(this);
        if (entryCondition != null)
        {
            entryCondition.MonitorCondition(null, () => Start(task));
        }
        else
        {
            Start(task);
        }
    }

    void Start(Task task)
    {
        task.triggeredNotStartedNodes.Remove(this);
        task.startedNodes.Add(this);
        if (entryAction != null)
        {
            entryAction.StartExecute();
        }

        MonitorCompleteCondition(task);
    }

    public void MonitorCompleteCondition(Task task)
    {
        if (completeCondition != null)
        {
            completeCondition.MonitorCondition(null, () => Complete(task));
        }
        else
        {
            Complete(task);
        }
    }

    void Complete(Task task)
    {
        task.startedNodes.Remove(this);
        if (completeAction != null) completeAction.StartExecute();

        //触发后继节点
        for (int i = 0; i < successors.Count; i++)
        {
            TaskNode successor = task.FindNotTriggerNode(successors[i]);
            if (successor != null)
            {
                successor.TryTrigger(task, id);
            }
        }
    }
}
