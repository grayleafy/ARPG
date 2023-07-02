using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pair<T, U>
{
    public T key;
    public U value;
}

public static class PairHelper
{
    public static Dictionary<T, U> PairsToDictionary<T, U>(Pair<T, U>[] pairs)
    {
        Dictionary<T, U> dic = new Dictionary<T, U>();
        foreach (Pair<T, U> pair in pairs)
        {
            dic[pair.key] = pair.value;
        }
        return dic;
    }
}
