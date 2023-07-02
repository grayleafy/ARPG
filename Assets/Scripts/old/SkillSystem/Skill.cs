using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skill
{
    [SerializeField] public int id;
    [SerializeField] public GameObject releaser;
    [SerializeField] public Condition[] conditions;
    [SerializeField] public SelectMethod selectMethod;
    [SerializeField] public Effect[] effects;

    //技能定义的格式为  condition2: parameter1 parameter2&condition1: parameter1 parameter2|mySelectMethod:parameter1 parameter2|effect1:p1 p2&effect2:p1 p2
    public void InitByDefineString(string skillDefine)
    {
        skillDefine = skillDefine.Replace("\n", "");
        skillDefine = skillDefine.Replace("\r", "");
        string[] split = skillDefine.Split('|');
        string[] conditionsDefs = split[0].Split("&");
        string selectMethodDef = split[1];
        string[] effectsDefs = split[2].Split("&");

        conditions = new Condition[conditionsDefs.Length];
        for (int i = 0; i < conditionsDefs.Length; i++)
        {
            conditions[i] = Condition.Create(conditionsDefs[i]);
        }
        selectMethod = SelectMethod.Create(selectMethodDef);
        effects = new Effect[effectsDefs.Length];
        for (int i = 0; i < effectsDefs.Length; i++)
        {
            effects[i] = Effect.Create(effectsDefs[i]);
        }
    }


}
