using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCPlayAnimationGameAction : GameAction
{
    public int npcID;
    public string animationName;

    float leftTime;

    protected override bool CheckActionComplete()
    {
        return leftTime <= 0;
    }

    protected override void ExecutePerFrame()
    {
        leftTime -= Time.deltaTime;
    }

    protected override void Finish()
    {

    }

    protected override void Init()
    {
        GameObject npc = DataManager.GetInstance().GetNPC(npcID);
        Animator animator = npc.GetComponent<Animator>();
        animator.CrossFade(animationName, 0.25f);

        leftTime = AnimationManager.GetInstance().GetClipLength(animator.runtimeAnimatorController, animationName);
    }
}
