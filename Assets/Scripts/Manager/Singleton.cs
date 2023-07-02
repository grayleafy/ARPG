using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// awake注意要重载
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static private T instance;
    static public T Instance { get { return instance; } }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
            if (instance.transform.parent == null) DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        instance = null;
    }
}
