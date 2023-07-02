using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCTalkGameAction : GameAction
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
        NPCController npcController = DataManager.GetInstance().GetNPC(npcID).GetComponent<NPCController>();
        npcController.DoTalk();
    }
}
