using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverHealthEffect : Effect
{
    int value;
    protected override void EffectStart(GameObject user, GameObject[] targets, object[] parameters)
    {
        base.EffectStart(user, targets, parameters);

        finished = true;
    }

    protected override void EffectUpdate(GameObject releaser, GameObject[] targets, object[] parameters)
    {

    }


    public override void Init(string[] parameters)
    {
        value = int.Parse(parameters[0]);
    }
}
