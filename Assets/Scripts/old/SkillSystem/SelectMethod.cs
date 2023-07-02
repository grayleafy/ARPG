using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectMethod
{
    public bool finished = false;


    public abstract void Init(string[] parameters);

    public virtual void SelectStart(GameObject releaser)
    {
        finished = false;

    }
    public abstract void SelectUpdate(GameObject releaser);

    public abstract void GetTarget(GameObject releaser, out GameObject[] targets, out object[] parameters);

    public IEnumerator Run(SkillDroper droper, Skill skill, GameObject releaser)
    {
        SelectStart(releaser);

        while (true)
        {
            SelectUpdate(releaser);
            if (finished)
            {
                GetTarget(releaser, out GameObject[] targets, out object[] parameters);
                for (int i = 0; i < skill.effects.Length; i++)
                {
                    //Debug.Log("start effect");
                    droper.StartCoroutine(skill.effects[i].Run(releaser, targets, parameters));
                }
                break;
            }

            yield return null;
        }
    }

    public static SelectMethod Create(string selectMethodDef)
    {
        string[] split = selectMethodDef.Split(":");
        string name = split[0];
        string[] parameters = null;
        if (split.Length > 1)
        {
            parameters = split[1].Split(" ");
        }

        Type type = Type.GetType(name + "SelectMethod");
        SelectMethod selectMethod = Activator.CreateInstance(type) as SelectMethod;
        selectMethod.Init(parameters);
        return selectMethod;
    }
}
