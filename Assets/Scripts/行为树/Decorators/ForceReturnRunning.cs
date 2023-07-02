using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 强制返回Running
    /// </summary>
    [Serializable]
    public class ForceReturnRunning : Decorator
    {
        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            StartChildAction(child);
            child.DoAction();
            return BehaviorTreeResult.Running;
        }
    }

}