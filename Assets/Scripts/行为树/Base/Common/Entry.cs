using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 行为树入口节点
    /// </summary>
    [Serializable]
    public class Entry : Decorator
    {
        Coroutine runningCoroutine = null;

        public void StartExecute()
        {
            if (!isInitialized) Init(null);
            runningCoroutine = MonoMgr.GetInstance().StartCoroutine(ReallyExecute());
            var dic = blackboard;

        }

        public void StopExecute()
        {
            MonoMgr.GetInstance().StopCoroutine(runningCoroutine);
            ExitAction();
        }

        IEnumerator ReallyExecute()
        {
            EnterAction();
            while (true)
            {
                DoBeforeAction();
                var result = DoAction();
                DoAfterAction();
                if (result == BehaviorTreeResult.Running)
                {
                    yield return null;

                    continue;
                }
                break;
            }
            ExitAction();
        }



        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            if (child == null)
            {
                Debug.LogError("入口没有子节点");
            }
            StartChildAction(child);
            return child.DoAction();
        }
    }
}
