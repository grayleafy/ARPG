using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DelayCondionChecker : ConditionChecker
{
    [JsonProperty] public float delayTime = 5f;



    public override bool EvaluateCondition(GameObject target)
    {

        return delayTime <= 0;
    }

    protected override bool StartListen(GameObject target)
    {
        return false;
    }

    protected override void UpdateProgress(GameObject target)
    {
        delayTime -= Time.deltaTime;
    }
}
