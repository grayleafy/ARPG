using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 应用根运动，结束时设置为不使用根运动，持续时间设置为-1则表示没有时间限制
    /// </summary>
    [Serializable]
    public class ApplyRootMotion : ActionNode
    {
        public float duration = -1;

        Animator animator;
        float leftTime;

        public override void EnterAction()
        {
            base.EnterAction();
            animator = GetValue<GameObject>("gameObject").GetComponent<Animator>();
            animator.applyRootMotion = true;

            leftTime = duration;
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            if (leftTime == -1)
            {
                return BehaviorTreeResult.Running;
            }

            leftTime -= Time.deltaTime;
            if (leftTime <= 0)
            {
                return BehaviorTreeResult.Success;
            }

            return BehaviorTreeResult.Running;
        }

        public override void ExitAction()
        {
            base.ExitAction();
            animator.applyRootMotion = false;
        }
    }
}
