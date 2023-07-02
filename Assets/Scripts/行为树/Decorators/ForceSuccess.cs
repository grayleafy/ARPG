using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 使子节点只会返回成功
    /// </summary>
    [Serializable]
    public class ForceSuccess : Decorator
    {
        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();

            StartChildAction(child);
            var result = child.DoAction();

            if (result == BehaviorTreeResult.Running)
            {
                return BehaviorTreeResult.Running;
            }

            else
            {
                return BehaviorTreeResult.Success;
            }
        }
    }
}