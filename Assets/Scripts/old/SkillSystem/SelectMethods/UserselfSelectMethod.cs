using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserselfSelectMethod : SelectMethod
{


    public override void GetTarget(GameObject releaser, out GameObject[] targets, out object[] parameters)
    {
        targets = new GameObject[1];
        targets[0] = releaser;
        parameters = null;
    }

    public override void Init(string[] parameters)
    {

    }

    public override void SelectUpdate(GameObject releaser)
    {
        finished = true;
    }
}
