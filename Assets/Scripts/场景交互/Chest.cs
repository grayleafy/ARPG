using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<ItemInfo> chest = new();

    public void OpenChest()
    {
        var play = GameObject.FindWithTag("Player");
        var npcController = play.GetComponent<NPCController>();
        for (int i = 0; i < chest.Count; i++)
        {
            npcController.ObtainItem(chest[i].id);
        }

        string toolTip = "获得了";
        for (int i = 0; i < chest.Count; i++)
        {
            toolTip += chest[i].GetItem().name;
            if (i == chest.Count - 1)
            {
                toolTip += "。";
            }
            else
            {
                toolTip += "、";
            }
        }

        UIManager.GetInstance().ShowPanel<ToolTipPanel>("ToolTipPanel", E_UI_Layer.Mid, (panel) =>
        {
            panel.SetToolTip(toolTip);
        });
    }
}
