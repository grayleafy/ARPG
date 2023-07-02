using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class GameActionWithReleaser : GameAction
{
    protected GameObject releaser;

    public void SetReleaser(GameObject releaser)
    {
        this.releaser = releaser;
    }
}
