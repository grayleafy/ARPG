using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 保存硬直，持续时间设置为-1即没有时间限制
    /// </summary>
    [Serializable]
    public class KeepStagger : ActionNode
    {
        public float duration = -1;

        NPCController controller;
        float leftTime;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);
            controller = GetValue<GameObject>("gameObject").GetComponent<NPCController>();
        }

        public override void EnterAction()
        {
            base.EnterAction();
            controller.SetStagger(1000);
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
            controller.SetStagger(0, true);
        }
    }
}
