using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PrintGameAction : GameAction
{
    [SerializeField, JsonProperty] float lastTime = 5;
    float left;
    protected override bool CheckActionComplete()
    {
        return left <= 0;
    }

    protected override void ExecutePerFrame()
    {
        left -= Time.deltaTime;
        Debug.Log(left);
    }

    protected override void Finish()
    {

    }

    protected override void Init()
    {
        left = lastTime;
    }
}
