using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneObjectInteractedConditionChecker : ConditionChecker
{
    public int sceneObjectID;


    public override bool EvaluateCondition(GameObject target)
    {
        var data = DataManager.GetInstance().GetData();
        var sceneObjects = data.SceneObjects;
        var sceneObject = sceneObjects.Find((x)=>x.id == sceneObjectID);

        if (sceneObject != null)
        {
            if (sceneObject.interactable == false)
            {
                return true;
            }
        }

        return false;
    }

    protected override bool StartListen(GameObject target)
    {
        return false;
    }

    protected override void UpdateProgress(GameObject target)
    {
        
    }
}
