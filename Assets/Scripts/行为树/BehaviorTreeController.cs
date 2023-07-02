using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public abstract class VariableInfo
{
    public string key;

    public abstract object GetValue();
    public abstract Type GetVariableType();
}

[Serializable]
public class GameObjectVariable : VariableInfo
{
    public GameObject value;

    public override object GetValue()
    {
        return value;
    }

    public override Type GetVariableType()
    {
        return typeof(GameObject);
    }
}


public class BehaviorTreeController : MonoBehaviour
{
    [SerializeReference, SubclassSelector] public BT.Entry entry;
    [SerializeReference, SubclassSelector] List<VariableInfo> blackBoardConfig = new();

    private void Awake()
    {
        SetBlackBoard();
    }

    private void OnEnable()
    {
        entry.StartExecute();
    }

    private void OnDisable()
    {
        entry.StopExecute();
    }

    //设置黑板参数
    void SetBlackBoard()
    {
        entry.SetValue<GameObject>("gameObject", gameObject);
        foreach (var variableInfo in blackBoardConfig)
        {
            if (variableInfo.GetVariableType() == typeof(GameObject))
            {
                entry.SetValue(variableInfo.key, (GameObject)variableInfo.GetValue());
            }

        }
    }
}
