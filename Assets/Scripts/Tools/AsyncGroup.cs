using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




/// <summary>
///异步组，当一个操作需要在多个异步操作全部完成后调用，可以使用如下写法
///AsyncGroup asyncGroup = new AsyncGroup(callback, size);
///for (int i = 0; i 《 size; i++){
///    int temp = i;
///    funAsync(args, () =》asyncGroup.OnNodeComplete());
///}
/// </summary>
public class AsyncGroup
{
    UnityAction callback;
    int leftNode;

    public AsyncGroup(UnityAction callback, int size)
    {
        this.callback = callback;
        leftNode = size;
        if (size == 0) { callback.Invoke(); };
    }

    public void OnNodeComplete()
    {
        leftNode--;
        if (CheckIsAllFinished())
        {
            callback.Invoke();
        }
    }

    bool CheckIsAllFinished()
    {
        return leftNode <= 0;
    }
}
