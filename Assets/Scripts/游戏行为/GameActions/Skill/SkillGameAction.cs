using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillGameAction : GameActionWithReleaser
{
    [SerializeReference, SubclassSelector] public GameActionWithReleaser skill;


    protected override bool CheckActionComplete()
    {
        return skill.IsCompleted();
    }

    protected override void ExecutePerFrame()
    {

    }

    protected override void Finish()
    {

    }

    protected override void Init()
    {
        skill.SetReleaser(releaser);
        skill.StartExecute();
    }

    public override void ForceInterrupt()
    {
        base.ForceInterrupt();
        skill?.ForceInterrupt();
    }
}
