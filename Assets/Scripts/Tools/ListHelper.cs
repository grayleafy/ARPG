using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListHelpe
{
    /// <summary>
    /// 找到列表中最小的元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="list"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T FindMin<T, TValue>(this List<T> list, Func<T, TValue> selector) where TValue : IComparable<TValue>
    {
        if (list == null || list.Count == 0)
        {
            return default;
        }

        T minValue = list[0];
        TValue minVal = selector(minValue);

        for (int i = 1; i < list.Count; i++)
        {
            T item = list[i];
            TValue val = selector(item);

            if (val.CompareTo(minVal) < 0)
            {
                minValue = item;
                minVal = val;
            }
        }

        return minValue;
    }

    /// <summary>
    /// 找到列表中最小的元素
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="list"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T FindMin<T>(this List<T> list) where T : IComparable<T>
    {
        if (list == null || list.Count == 0)
        {
            throw new InvalidOperationException("List is empty or null.");
        }

        T minValue = list[0];


        for (int i = 1; i < list.Count; i++)
        {
            T item = list[i];


            if (item.CompareTo(minValue) < 0)
            {
                minValue = item;
            }
        }

        return minValue;
    }
}
