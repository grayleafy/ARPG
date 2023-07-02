using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OwnItemConditionChecker : ConditionChecker
{
    public int itemID;

    CharacterBag bag;

    public override bool EvaluateCondition(GameObject target)
    {

        ItemInfo itemInfo = null;

        itemInfo = bag.weapons.Find((x) => x.id == itemID);
        if (itemInfo != null) { return true; }
        itemInfo = bag.usableItems.Find((x) => x.id == itemID);
        if (itemInfo != null) { return true; }

        return false;
    }

    protected override bool StartListen(GameObject target)
    {
        bag = DataManager.GetInstance().GetData().NPCInfos.Find((x) => x.tag == CharacterTag.Player).bag;
        return false;
    }

    protected override void UpdateProgress(GameObject target)
    {

    }


}
