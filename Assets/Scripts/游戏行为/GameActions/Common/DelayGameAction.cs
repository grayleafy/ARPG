using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DelayGameAction : GameAction
{
    public float delayTime;

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
        leftTime = delayTime;
    }
}
