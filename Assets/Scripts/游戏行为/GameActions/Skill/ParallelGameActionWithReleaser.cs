using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParallelGameActionWithReleaser : GameActionWithReleaser
{
    [SerializeReference, SubclassSelector] public List<GameActionWithReleaser> actions;

    ParallelGameAction parallelGameAction = new();

    protected override bool CheckActionComplete()
    {
        return parallelGameAction.IsCompleted();
    }

    protected override void ExecutePerFrame()
    {

    }

    protected override void Finish()
    {

    }

    protected override void Init()
    {
        parallelGameAction = new();

        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].SetReleaser(releaser);
            parallelGameAction.parallelActions.Add(actions[i]);
        }

        parallelGameAction.StartExecute();
    }

    public override void ForceInterrupt()
    {
        base.ForceInterrupt();
        parallelGameAction.ForceInterrupt();
    }

}
