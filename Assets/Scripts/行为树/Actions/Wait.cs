using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 等待一段时间
    /// </summary>
    [Serializable]
    public class Wait : ActionNode
    {
        public float waitTime;

        float leftTime;

        public override void EnterAction()
        {
            base.EnterAction();
            leftTime = waitTime;
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            leftTime -= Time.deltaTime;

            if (leftTime <= 0)
            {
                return BehaviorTreeResult.Success;
            }

            return BehaviorTreeResult.Running;
        }
    }
}