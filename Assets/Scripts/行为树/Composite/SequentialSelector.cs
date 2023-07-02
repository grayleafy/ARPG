using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 顺序选择节点，从上次结束的子节点开始，一帧内顺序执行子节点，某一个子节点成功，则停止，返回成功
    /// </summary>
    [Serializable]
    public class SequentialSelector : Composite
    {
        public float resetTime = 1;

        int index;
        float lastTime;
        bool isLoop = false; //索引是否已经回归到零


        public override void Init(BehaviorTreeNode parent)
        {
            base.Init(parent);
            index = -1;
        }

        public override void EnterAction()
        {
            base.EnterAction();
            isLoop = false;
            float currentTime = Time.realtimeSinceStartup;
            if (currentTime - lastTime > resetTime)
            {
                index = 0;
            }
            else
            {
                index = (index + 1) % children.Count;
            }
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();

            //如果没有子节点则失败
            if (children == null || children.Count == 0)
            {
                return BehaviorTreeResult.Fail;
            }

            while (isLoop == false || index < children.Count)
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

                    if (index >= children.Count)
                    {
                        index = 0;
                        isLoop = true;
                    }
                }
            }

            return BehaviorTreeResult.Fail;
        }


        public override void ExitAction()
        {
            base.ExitAction();
            lastTime = Time.realtimeSinceStartup;
        }
    }
}