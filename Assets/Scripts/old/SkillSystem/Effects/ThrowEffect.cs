using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowEffect : Effect
{
    GameObject throwsPrefab;
    GameObject throws;

    protected override void EffectStart(GameObject releaser, GameObject[] targets, object[] parameters)
    {
        base.EffectStart(releaser, targets, parameters);
        Vector3 pos = (Vector3)parameters[0];
        Vector3 dir = pos - releaser.transform.position;

        throws = GameObject.Instantiate(throwsPrefab);
        throws.transform.position = releaser.transform.position + Vector3.up * 2f;
        throws.GetComponent<Rigidbody>().velocity = dir * 1.5f + Vector3.up;
    }

    protected override void EffectUpdate(GameObject releaser, GameObject[] targets, object[] parameters)
    {

    }

    public override void Init(string[] parameters)
    {
        throwsPrefab = Resources.Load<GameObject>("Prefabs/" + parameters[0]);
    }
}
