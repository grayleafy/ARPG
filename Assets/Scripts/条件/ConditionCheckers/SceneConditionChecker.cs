using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SceneConditionChecker : ConditionChecker
{
    public string sceneName;

    public override bool EvaluateCondition(GameObject target)
    {
        return SceneManager.GetActiveScene().name == sceneName;
    }

    protected override bool StartListen(GameObject target)
    {
        return false;
    }

    protected override void UpdateProgress(GameObject target)
    {

    }
}
