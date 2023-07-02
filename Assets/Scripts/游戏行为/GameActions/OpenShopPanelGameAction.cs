using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OpenShopPanelGameAction : GameAction
{
    public int npcID;

    protected override bool CheckActionComplete()
    {
        return true;
    }

    protected override void ExecutePerFrame()
    {

    }

    protected override void Finish()
    {

    }

    protected override void Init()
    {
        NPCController npc = DataManager.GetInstance().NPCs[npcID].GetComponent<NPCController>();
        UIManager.GetInstance().ShowPanel<ShopPanel>("ShopPanel", E_UI_Layer.Mid, (panel) =>
        {
            panel.SetBag(npc.info.bag);
        });
    }
}
