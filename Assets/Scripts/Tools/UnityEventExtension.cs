using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 好像有点问题
/// </summary>
public static class UnityEventExtension
{
    /// <summary>
    /// 添加唯一事件，不同实例的同一方法可同时存在
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="unityEvent"></param>
    /// <param name="fun"></param>
    public static void AddUniqueListener<T>(this UnityEvent<T> unityEvent, UnityAction<T> fun)
    {
        if (!unityEvent.HasListener<T>(fun))
        {
            unityEvent.AddListener(fun);

        }
    }


    /// <summary>
    /// 有问题
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="unityEvent"></param>
    /// <param name="fun"></param>
    /// <returns></returns>
    public static bool HasListener<T>(this UnityEvent<T> unityEvent, UnityAction<T> fun)
    {
        for (int i = 0; i < unityEvent.GetPersistentEventCount(); i++)
        {
            if (unityEvent.GetPersistentTarget(i) == (object)fun.Target && unityEvent.GetPersistentMethodName(i).Equals(fun.Method.Name))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 添加唯一事件，不同实例的同一方法可同时存在
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="unityEvent"></param>
    /// <param name="fun"></param>
    public static void AddUniqueListener(this UnityEvent unityEvent, UnityAction fun)
    {
        if (!unityEvent.HasListener(fun))
        {
            unityEvent.AddListener(fun);
            Debug.Log(unityEvent.ToString() + "     count = " + unityEvent.GetPersistentEventCount());
        }
    }


    /// <summary>
    /// 有问题
    /// </summary>
    /// <param name="unityEvent"></param>
    /// <param name="fun"></param>
    /// <returns></returns>
    public static bool HasListener(this UnityEvent unityEvent, UnityAction fun)
    {
        for (int i = 0; i < unityEvent.GetPersistentEventCount(); i++)
        {
            if (unityEvent.GetPersistentTarget(i) == (object)fun.Target && unityEvent.GetPersistentMethodName(i).Equals(fun.Method.Name))
            {
                return true;
            }
        }
        return false;
    }
}
