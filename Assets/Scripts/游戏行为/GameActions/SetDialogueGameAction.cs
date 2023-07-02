using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SetDialogueGameAction : GameAction
{
    public int npcID;
    public List<int> dialogueEntrys;

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
        var npcInfo = DataManager.GetInstance().GetData().NPCInfos.Find((x) => x.id == npcID);
        npcInfo.dialogueEntrys = dialogueEntrys;
    }
}
