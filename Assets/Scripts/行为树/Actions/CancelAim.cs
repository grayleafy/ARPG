using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 取消瞄准，变为持枪状态
    /// </summary>
    [Serializable]
    public class CancelAim : ActionNode
    {
        NPCController controller;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);
            controller = GetValue<GameObject>("gameObject").GetComponent<NPCController>();
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            controller.CancelAim();
            return BehaviorTreeResult.Success;
        }
    }
}