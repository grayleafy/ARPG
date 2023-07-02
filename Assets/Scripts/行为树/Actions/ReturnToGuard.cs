using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 返回起始点
    /// </summary>
    [Serializable]
    public class ReturnToGuard : ActionNode
    {
        NPCController controller;
        Vector3 originPos;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);
            controller = GetValue<GameObject>("gameObject").GetComponent<NPCController>();
            originPos = controller.transform.position;
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            controller.Follow(originPos);
            controller.Follow(originPos);
            return BehaviorTreeResult.Success;
        }

        public override void ExitAction()
        {
            base.ExitAction();
            //controller.StopFollow();
        }
    }
}
