using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskNodeButtonPanel : BasePanel
{
    public void SetTaskNode(TaskNode taskNode)
    {
        GetControl<TextMeshProUGUI>("TaskNodeDescription").text = taskNode.name;
    }
}
