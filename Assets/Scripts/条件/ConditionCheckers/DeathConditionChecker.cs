using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 检测角色死亡
/// </summary>
[Serializable]
public class DeathConditionChecker : ConditionChecker
{
    public int npcID;

    bool isFinished = false;

    public override bool EvaluateCondition(GameObject target)
    {
        return isFinished;
    }

    protected override bool StartListen(GameObject target)
    {
        isFinished = false;
        EventCenter.GetInstance().AddEventListener<GameObject>("角色死亡", OnDeath);

        return false;
    }

    protected override void UpdateProgress(GameObject target)
    {

    }

    void OnDeath(GameObject character)
    {
        int id = character.GetComponent<NPCController>().info.id;
        if (id == npcID)
        {
            isFinished = true;
            EventCenter.GetInstance().RemoveEventListener<GameObject>("角色死亡", OnDeath);
        }
    }
}
