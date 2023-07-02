using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayClipEffect : Effect
{
    int clipID;


    protected override void EffectStart(GameObject releaser, GameObject[] targets, object[] parameters)
    {
        base.EffectStart(releaser, targets, parameters);
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].GetComponent<Animator>().CrossFade(clipID, 0.25f);
        }

        finished = true;
    }
    protected override void EffectUpdate(GameObject releaser, GameObject[] targets, object[] parameters)
    {

    }

    public override void Init(string[] parameters)
    {
        clipID = Animator.StringToHash(parameters[0]);
    }
}
