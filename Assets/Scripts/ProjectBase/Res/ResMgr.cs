using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载模块
/// 1.异步加载
/// 2.委托和 lambda表达式
/// 3.协程
/// 4.泛型
/// </summary>
public class ResMgr : BaseManager<ResMgr>
{
    //同步加载资源
    public T Load<T>(string name, Transform parent = null) where T : Object
    {
        T res = Resources.Load<T>(name);
        //如果对象是一个GameObject类型的 我把他实例化后 再返回出去 外部 直接使用即可
        if (res is GameObject)
        {
            if (parent == null)
            {
                return GameObject.Instantiate(res);
            }
            else
            {
                return GameObject.Instantiate(res, parent);
            }
        }
        else//TextAsset AudioClip
            return res;
    }


    //异步加载资源
    public void LoadAsync<T>(string name, UnityAction<T> callback, Transform parent = null) where T : Object
    {
        //开启异步加载的协程
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadAsync(name, parent, callback));
    }

    //真正的协同程序函数  用于 开启异步加载对应的资源
    private IEnumerator ReallyLoadAsync<T>(string name, Transform parent, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;

        if (r.asset is GameObject)
        {
            var o = GameObject.Instantiate(r.asset, parent) as T;
            if (callback != null) callback(o);
        }
        else
        {
            if (callback != null) callback(r.asset as T);
        }

    }


}
