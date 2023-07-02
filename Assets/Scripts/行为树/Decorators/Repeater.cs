using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 重复执行子节点，直到失败或次数到达上限，次数设置为-1则无上限
    /// </summary>
    [Serializable]
    public class Repeater : Decorator
    {
        [Tooltip("循环次数，设置为-1则无上限")]
        public int loopCount = -1;

        int loopIndex;

        public override void EnterAction()
        {
            base.EnterAction();
            loopIndex = 0;
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();

            if (loopCount == -1 || loopIndex < loopCount)
            {
                StartChildAction(child);
                var result = child.DoAction();
                if (result == BehaviorTreeResult.Running)
                {
                    return BehaviorTreeResult.Running;
                }
                else if (result == BehaviorTreeResult.Fail)
                {
                    return BehaviorTreeResult.Fail;
                }
                else if (result == BehaviorTreeResult.Success)
                {
                    loopIndex++;
                    StopChildAction(child);
                    return BehaviorTreeResult.Running;
                }
            }

            return BehaviorTreeResult.Success;
        }
    }
}
