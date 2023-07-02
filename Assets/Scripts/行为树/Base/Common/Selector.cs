using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 选择节点，一帧内顺序执行子节点，某一个子节点成功，则停止，返回成功
    /// </summary>
    [Serializable]
    public class Selector : Composite
    {
        int index;

        public override void EnterAction()
        {
            base.EnterAction();
            index = 0;
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();

            //如果没有子节点则失败
            if (children == null || children.Count == 0)
            {
                return BehaviorTreeResult.Fail;
            }

            while (index < children.Count)
            {
                StartChildAction(children[index]);
                var result = children[index].DoAction();
                if (result == BehaviorTreeResult.Running)
                {
                    return BehaviorTreeResult.Running;
                }
                else if (result == BehaviorTreeResult.Success)
                {
                    return BehaviorTreeResult.Success;
                }
                else if (result == BehaviorTreeResult.Fail)
                {
                    StopChildAction(children[index]);
                    index++;
                }
            }

            return BehaviorTreeResult.Fail;

            //if (index == -1)
            //{
            //    index = 0;
            //    StartChildAction(children[index]);
            //}

            //var result = children[index].DoAction();

            //if (result == BehaviorTreeResult.Running)
            //{
            //    return BehaviorTreeResult.Running;
            //}
            //else if (result == BehaviorTreeResult.Success)
            //{
            //    return BehaviorTreeResult.Success;
            //}
            //else if (result == BehaviorTreeResult.Fail)
            //{
            //    StopChildAction(children[index]);
            //    index++;
            //    if (index >= children.Count)
            //    {
            //        return BehaviorTreeResult.Fail;
            //    }
            //    else
            //    {
            //        StartChildAction(children[index]);
            //        return BehaviorTreeResult.Running;
            //    }
            //}

            //return BehaviorTreeResult.Fail;
        }
    }
}