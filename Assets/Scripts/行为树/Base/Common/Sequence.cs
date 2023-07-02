using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 顺序执行子节点，子节点全部成功则成功，当某一子节点失败则停止
    /// </summary>
    [Serializable]
    public class Sequence : Composite
    {
        int currentIndex;

        public override void EnterAction()
        {
            base.EnterAction();
            currentIndex = -1;
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            if (children == null || children.Count == 0)
            {
                return BehaviorTreeResult.Fail;
            }

            if (currentIndex < 0)
            {
                currentIndex = 0;
                StartChildAction(children[currentIndex]);
            }

            var result = children[currentIndex].DoAction();

            switch (result)
            {
                case BehaviorTreeResult.Running:
                    return BehaviorTreeResult.Running;
                case BehaviorTreeResult.Fail:
                    StopChildAction(children[currentIndex]);
                    return BehaviorTreeResult.Fail;
                case BehaviorTreeResult.Success:
                    StopChildAction(children[currentIndex]);
                    if (currentIndex == children.Count - 1)
                    {
                        return BehaviorTreeResult.Success;
                    }
                    else
                    {
                        currentIndex++;
                        StartChildAction(children[currentIndex]);
                        return BehaviorTreeResult.Running;
                    }
            }

            return BehaviorTreeResult.Running;
        }



    }
}