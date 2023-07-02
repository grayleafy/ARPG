using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 并行选择节点，当某一个子节点成功结束时，全部结束
    /// </summary>
    [Serializable]
    public class ParallelSelector : Composite
    {
        public override void EnterAction()
        {
            base.EnterAction();
            for (int i = 0; i < children.Count; i++)
            {
                StartChildAction(children[i]);
            }
        }
        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();

            for (int i = 0; i < runningChildren.Count; i++)
            {
                var result = runningChildren[i].DoAction();

                if (result == BehaviorTreeResult.Running)
                {

                }
                else if (result == BehaviorTreeResult.Success)
                {
                    return BehaviorTreeResult.Success;
                }
                else if (result == BehaviorTreeResult.Fail)
                {
                    StopChildAction(runningChildren[i]);
                    i--;
                }
            }

            if (runningChildren.Count == 0)
            {
                return BehaviorTreeResult.Fail;
            }

            return BehaviorTreeResult.Running;
        }

        public override void ExitAction()
        {
            base.ExitAction();
        }
    }

}