using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TaskType
{
    Main,
    Side,
}

public enum TaskState
{
    NotTriggered,
    TriggeredNotStarted,
    Started,
    Completed,
}

[Serializable]
public class Task
{
    [SerializeField] public string name;
    [SerializeField] public int id;
    [SerializeField] public string description;
    [SerializeField] public TaskState state = TaskState.NotTriggered;
    [SerializeReference, SubclassSelector] public ConditionChecker entryCondition;
    [SerializeField] public List<TaskNode> notTriggerNodes = new();  //任务节点结束后，由入口调用
    [SerializeField] public List<TaskNode> triggeredNotStartedNodes = new(); //需要恢复条件检测
    [SerializeField] public List<TaskNode> startedNodes = new(); //需要恢复结束条件的检测

    /// <summary>
    /// 从任务入口尝试开始任务，可设置是否持续监测条件是否满足
    /// </summary>
    /// <param name="monitorCondition"></param>
    public void TriggerTask(bool monitorCondition = true)
    {
        if (state == TaskState.NotTriggered)
        {
            if (monitorCondition)
            {
                state = TaskState.TriggeredNotStarted;
                if (entryCondition != null)
                {
                    entryCondition.MonitorCondition(null, StartTask);
                }
                else
                {
                    StartTask();
                }

            }
            else
            {
                if (entryCondition != null)
                {
                    if (entryCondition.EvaluateCondition(null))
                    {
                        StartTask();
                    }
                }
                else
                {
                    StartTask();
                }
            }
        }
    }

    void StartTask()
    {
        state = TaskState.Started;
        EventCenter.GetInstance().EventTrigger<Task>("任务开始", this);

        for (int i = 0; i < notTriggerNodes.Count; i++)
        {
            notTriggerNodes[i].TryTrigger(this, -1);
        }
    }

    public TaskNode FindNotTriggerNode(int id)
    {
        return notTriggerNodes.Find((x) => x.id == id);
    }

    /// <summary>
    /// 读档后恢复任务调用
    /// </summary>
    public void RestoreTaskStateAfterLoad()
    {
        if (state == TaskState.TriggeredNotStarted)
        {
            state = TaskState.TriggeredNotStarted;
            if (entryCondition != null)
            {
                entryCondition.MonitorCondition(null, StartTask);
            }
            else
            {
                StartTask();
            }
        }
        else if (state == TaskState.Started)
        {
            for (int i = 0; i < triggeredNotStartedNodes.Count; i++)
            {
                triggeredNotStartedNodes[i].TryTrigger(this, -1);
            }

            for (int i = 0; i < startedNodes.Count; i++)
            {
                startedNodes[i].MonitorCompleteCondition(this);
            }
        }
    }

    /// <summary>
    /// 任务完成
    /// </summary>
    public void CompleteTask()
    {
        state = TaskState.Completed;
        EventCenter.GetInstance().EventTrigger<Task>("任务完成", this);
    }
}
