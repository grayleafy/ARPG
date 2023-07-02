using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 随机选择节点
    /// </summary>
    [Serializable]
    public class RandomSelector : Composite
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
                var random = new System.Random();
                currentIndex = random.Next(children.Count);
                StartChildAction(children[currentIndex]);
            }

            return children[currentIndex].DoAction();


        }
    }
}
