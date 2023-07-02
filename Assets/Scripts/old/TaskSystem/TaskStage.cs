using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class TaskStage
{
    Effect[] startActions;
    Condition[] compeleteConditions;
    List<int> preparationStage;
    List<int> subsequentStage;

    /// <summary>
    /// 根据字符串定义创建TaskStage, 样例:  {{effect1: arg1 arg2}{effect2}}
    /// {{condition1: arg1}}
    /// {{pre1} {pre2}}
    /// {{sub1} {sub2}}
    /// </summary>
    /// <param name="taskStageDef"></param>
    /// <returns></returns>
    public static TaskStage Create(string taskStageDef)
    {
        TaskStage taskStage = new TaskStage();
        string[] parts = taskStageDef.Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
        string[] effectsDefs = parts[0].Split(new[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);

        taskStage.startActions = new Effect[effectsDefs.Length];

        return taskStage;
    }
}
