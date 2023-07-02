using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Effect
{
    public bool finished = false;
    UnityEvent callback = new();

    public void SetCallback(UnityAction action)
    {
        callback.AddListener(action);
    }

    public abstract void Init(string[] parameters);

    protected virtual void EffectStart(GameObject releaser, GameObject[] targets, object[] parameters)
    {
        finished = false;
    }

    protected abstract void EffectUpdate(GameObject releaser, GameObject[] targets, object[] parameters);

    public IEnumerator Run(GameObject releaser, GameObject[] targets, object[] parameters)
    {
        EffectStart(releaser, targets, parameters);

        while (!finished)
        {
            EffectUpdate(releaser, targets, parameters);
            yield return null;
        }
    }

    public static Effect Create(string effectDef)
    {
        string[] split = effectDef.Split(":");
        string name = split[0];
        string[] parameters = null;
        if (split.Length > 1)
        {
            parameters = split[1].Split(" ");
        }

        Type type = Type.GetType(name + "Effect");
        Effect effect = Activator.CreateInstance(type) as Effect;
        effect.Init(parameters);
        return effect;
    }


}
