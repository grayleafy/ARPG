using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaSelectMethod : SelectMethod
{
    string ballPrefabName = "Prefabs/Sphere";
    GameObject ballprefab;
    GameObject[] balls = new GameObject[5];
    RaycastHit hitInfo;

    public override void GetTarget(GameObject releaser, out GameObject[] targets, out object[] parameters)
    {
        targets = null;
        parameters = new object[1];
        parameters[0] = hitInfo.point;
    }

    public override void Init(string[] parameters)
    {

    }

    public override void SelectUpdate(GameObject releaser)
    {
        Vector2 mousePos = InputHandler.Instance.MousePos;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hitInfo))
        {
            Vector3 dir = hitInfo.point - releaser.transform.position;
            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].transform.position = releaser.transform.position + dir * ((float)i / (balls.Length - 1));
            }
        }

        if (InputHandler.Instance.mouseLeftButton)
        {
            for (int i = 0; i < balls.Length; i++)
            {
                GameObject.Destroy(balls[i]);
            }
            finished = true;
        }
    }

    public override void SelectStart(GameObject releaser)
    {
        base.SelectStart(releaser);
        ballprefab = Resources.Load<GameObject>(ballPrefabName);

        for (int i = 0; i < balls.Length; i++)
        {
            balls[i] = GameObject.Instantiate(ballprefab);
        }
    }


}
