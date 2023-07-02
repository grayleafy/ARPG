using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 播放动画
    /// </summary>
    [Serializable]
    public class PlayAnimation : ActionNode
    {
        public string animationName;
        public float earlyTerminationTime;
        public float transitionDuration = 0.25f;

        float clipTime;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);
        }

        public override void EnterAction()
        {
            base.EnterAction();
            var animator = GetValue<GameObject>("gameObject").GetComponent<Animator>();
            animator.CrossFade(animationName, transitionDuration);
            clipTime = AnimationManager.GetInstance().GetClipLength(animator.runtimeAnimatorController, animationName);
            clipTime -= earlyTerminationTime;
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            clipTime -= Time.deltaTime;

            if (clipTime <= 0)
            {
                return BehaviorTreeResult.Success;
            }

            return BehaviorTreeResult.Running;
        }
    }
}
