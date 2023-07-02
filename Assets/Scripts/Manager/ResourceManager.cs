using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    [SerializeField] Dictionary<string, GameObject> map = new();

    public GameObject Instantiate(string url)
    {
        if (!map.ContainsKey(url))
        {
            GameObject o = Resources.Load<GameObject>(url);
            map.Add(url, o);
        }


        return Instantiate(map[url]);
    }
}
