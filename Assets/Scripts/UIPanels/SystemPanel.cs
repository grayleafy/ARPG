using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SystemPanel : BasePanel
{
    NPCInfo playerInfo;

    protected override void Awake()
    {
        base.Awake();
        GetControl<Button>("TaskMenuButton").onClick.AddListener(() => UIManager.GetInstance().ShowPanel<TaskPanel>("TaskPanel"));
        GetControl<Button>("BagButton").onClick.AddListener(() => UIManager.GetInstance().ShowPanel<BagPanel>("BagPanel"));
    }

    public override void ShowMe()
    {
        base.ShowMe();
        GetComponent<RectTransform>().SetParent(UIManager.GetInstance().GetLayerFather(E_UI_Layer.Bot));

        playerInfo = DataManager.GetInstance().GetData().NPCInfos.Find((x) => x.tag == CharacterTag.Player);
    }

    private void Update()
    {
        GetControl<TextMeshProUGUI>("MoneyText").text = "金币：" + playerInfo.money.ToString();

        Task task = TaskManager.GetInstance().trackedTask;
        if (task != null && task.state == TaskState.Started)
        {
            GetControl<TextMeshProUGUI>("TaskTitle").text = "当前任务：" + task.name;
            string s = "";
            for (int i = 0; i < task.startedNodes.Count; i++)
            {
                s += task.startedNodes[i].name + "\n";
            }
            GetControl<TextMeshProUGUI>("TaskNodes").text = s;
        }
        else
        {
            GetControl<TextMeshProUGUI>("TaskTitle").text = "";
            GetControl<TextMeshProUGUI>("TaskNodes").text = "";
        }
    }
}
