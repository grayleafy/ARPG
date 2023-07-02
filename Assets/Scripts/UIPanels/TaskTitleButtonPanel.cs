using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskTitleButtonPanel : BasePanel
{
    Task task;

    public void SetTask(Task task)
    {
        this.task = task;

        GetControl<TextMeshProUGUI>("Title").text = task.name;
        GetComponent<Button>().onClick.AddListener(() => UIManager.GetInstance().GetPanel<TaskPanel>("TaskPanel").ShowTask(task));
    }
}
