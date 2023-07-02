using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum TaskType
//{
//    Main,
//    Side,
//}

//[Serializable]
//public class Task
//{
//    [SerializeField] int id;
//    [SerializeField] TaskType type;
//    [SerializeField] string name;
//    [SerializeField] string description;
//    [SerializeField] bool display;

//    [SerializeField] string taskDef;

//    [SerializeField] TaskStage[] stages;
//    [SerializeField] List<int> currentStages = new();

//    public static Task Create(string taskDef)
//    {
//        Task task = new Task();
//        string[] parts = taskDef.Split(",");

//        task.id = int.Parse(parts[0]);
//        task.type = (TaskType)Enum.Parse(typeof(TaskType), parts[1]);
//        task.name = parts[2];
//        task.description = parts[3];
//        task.display = parts[4] == "是" ? true : false;
//        task.taskDef = parts[5];



//        return task;
//    }
//}
