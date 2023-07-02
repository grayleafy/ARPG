using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : BasePanel
{
    [SerializeField] RectTransform taskTitleArea;
    [SerializeField] RectTransform taskNodeArea;



    public override void ShowMe()
    {
        base.ShowMe();
        InputMgr.GetInstance().AddListener(InputEvent.EscDown, () => UIManager.GetInstance().HidePanel("TaskPanel"), true);
        ShowTaskList(TaskManager.GetInstance().tasksOnGoing);

        GetControl<Image>("TaskContent").gameObject.SetActive(false);
        if (TaskManager.GetInstance().trackedTask != null && TaskManager.GetInstance().trackedTask.state == TaskState.Started)
        {
            ShowTask(TaskManager.GetInstance().trackedTask);
        }
    }

    public override void HideMe()
    {
        base.HideMe();
        InputMgr.GetInstance().AddListener(InputEvent.EscDown, () => UIManager.GetInstance().ShowPanel<EscPanel>("EscPanel"), true);
    }

    //左边显示任务列表
    public void ShowTaskList(List<Task> tasks)
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            GameObject taskTitleButton = ResMgr.GetInstance().Load<GameObject>("UI/TaskTitleButtonPanel", taskTitleArea);
            taskTitleButton.GetComponent<TaskTitleButtonPanel>().SetTask(tasks[i]);
        }

        //刷新自动布局
        taskTitleArea.gameObject.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
        taskTitleArea.gameObject.GetComponent<VerticalLayoutGroup>().SetLayoutVertical();
        LayoutRebuilder.MarkLayoutForRebuild(taskTitleArea);
        LayoutRebuilder.ForceRebuildLayoutImmediate(taskTitleArea);
    }

    //显示详细任务信息
    public void ShowTask(Task task)
    {
        GetControl<Image>("TaskContent").gameObject.SetActive(true);
        GetControl<TextMeshProUGUI>("TaskDescription").text = task.description;
        GetControl<Button>("TrackTask").onClick.RemoveAllListeners();
        GetControl<Button>("TrackTask").onClick.AddListener(() => TaskManager.GetInstance().trackedTask = task);

        for (int i = 0; i < taskNodeArea.childCount; i++)
        {
            Destroy(taskNodeArea.GetChild(i).gameObject);
        }

        for (int i = 0; i < task.startedNodes.Count; i++)
        {
            GameObject taskNodeButtonPanel = ResMgr.GetInstance().Load<GameObject>("UI/TaskNodeButtonPanel", taskNodeArea);
            taskNodeButtonPanel.GetComponent<TaskNodeButtonPanel>().SetTaskNode(task.startedNodes[i]);
        }
    }
}
