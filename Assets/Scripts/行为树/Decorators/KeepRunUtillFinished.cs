using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 保持执行直到执行结束
    /// </summary>
    [Serializable]
    public class KeepRunUtillFinished : Decorator
    {
        bool isExecuted;
        BehaviorTreeResult childResult;

        public override void DoBeforeAction()
        {
            base.DoBeforeAction();
            isExecuted = false;
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            isExecuted = true;
            StartChildAction(child);
            return childResult = child.DoAction();
        }

        public override void DoAfterAction()
        {
            base.DoAfterAction();
            if (!isExecuted && childResult == BehaviorTreeResult.Running)
            {
                DoAction();
            }
        }
    }
}
