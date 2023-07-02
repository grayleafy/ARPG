using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SequentialGameActionWithReleaser : GameActionWithReleaser
{
    [SerializeReference, SubclassSelector] public List<GameActionWithReleaser> actions;

    SequentialGameAction sequentialGameAction = new();

    protected override bool CheckActionComplete()
    {
        return sequentialGameAction.IsCompleted();
    }

    protected override void ExecutePerFrame()
    {

    }

    protected override void Finish()
    {

    }

    protected override void Init()
    {
        sequentialGameAction = new();

        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].SetReleaser(releaser);
            sequentialGameAction.sequentialActions.Add(actions[i]);
        }

        sequentialGameAction.StartExecute();
    }

    public override void ForceInterrupt()
    {
        base.ForceInterrupt();
        sequentialGameAction.ForceInterrupt();
    }
}
