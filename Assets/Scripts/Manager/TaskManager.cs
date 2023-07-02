using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : SingletonAutoMono<TaskManager>
{
    [SerializeField] public List<Task> Tasks { get { return DataManager.GetInstance().GetData().tasks; } }
    public List<Task> tasksOnGoing = new();
    [HideInInspector] public Task trackedTask = null;

    public void InitWhenLoadGame()
    {
        for (int i = 0; i < Tasks.Count; i++)
        {
            if (Tasks[i].state == TaskState.Started)
            {
                tasksOnGoing.Add(Tasks[i]);
            }
            Tasks[i].RestoreTaskStateAfterLoad();
        }
    }

    private void Awake()
    {
        //监听任务开始
        EventCenter.GetInstance().AddEventListener<Task>("任务开始", (task) => tasksOnGoing.Add(task));
        //监听任务完成
        EventCenter.GetInstance().AddEventListener<Task>("任务完成", (task) => tasksOnGoing.Remove(task));
    }

    public void TriggerTask(int id)
    {
        Task task = Tasks.Find((x) => x.id == id);
        task.TriggerTask();
    }
}
