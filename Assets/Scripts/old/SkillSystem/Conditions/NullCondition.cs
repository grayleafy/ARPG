using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullCondition : Condition
{
    protected override void Init(string[] parameters)
    {
        //throw new System.NotImplementedException();
    }

    protected override bool CustomForceCheck()
    {
        return true;
    }

    protected override void StartListen()
    {

    }
}
