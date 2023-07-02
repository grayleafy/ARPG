using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 使子节点的结果反转
    /// </summary>
    [Serializable]
    public class Reverser : Decorator
    {
        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            StartChildAction(child);
            var result = child.DoAction();

            if (result == BehaviorTreeResult.Success)
            {
                return BehaviorTreeResult.Fail;
            }
            else if (result == BehaviorTreeResult.Fail)
            {
                return BehaviorTreeResult.Success;
            }

            return BehaviorTreeResult.Running;
        }
    }
}
