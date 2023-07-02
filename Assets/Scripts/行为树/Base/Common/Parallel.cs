using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 并行节点，全部执行完才结束，全部成功则成功
    /// </summary>
    [Serializable]
    public class Parallel : Composite
    {
        BehaviorTreeResult finalResult;

        public override void EnterAction()
        {
            base.EnterAction();
            for (int i = 0; i < children.Count; i++)
            {
                StartChildAction(children[i]);
            }

            finalResult = BehaviorTreeResult.Success;
        }


        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();

            bool isRunning = false;

            for (int i = 0; i < runningChildren.Count; i++)
            {
                var result = runningChildren[i].DoAction();
                if (result == BehaviorTreeResult.Fail)
                {
                    finalResult = BehaviorTreeResult.Fail;
                    StopChildAction(runningChildren[i]);
                    i--;
                }
                else if (result == BehaviorTreeResult.Success)
                {
                    StopChildAction(runningChildren[i]);
                    i--;
                }
                else if (result == BehaviorTreeResult.Running)
                {
                    isRunning = true;
                }
            }

            if (isRunning)
            {
                return BehaviorTreeResult.Running;
            }
            else
            {
                return finalResult;
            }
        }
    }
}