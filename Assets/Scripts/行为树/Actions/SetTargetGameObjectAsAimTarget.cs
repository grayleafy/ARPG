using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 设置黑板中的targetGmaeObject为瞄准目标
    /// </summary>
    [Serializable]
    public class SetTargetGameObjectAsAimTarget : ActionNode
    {
        GameObject gameObject;
        NPCController controller;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);
            gameObject = GetValue<GameObject>("gameObject");
            controller = gameObject.GetComponent<NPCController>();
        }


        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            var target = GetValue<GameObject>("targetGameObject").transform;
            controller.SetAimTarget(target);
            return BehaviorTreeResult.Success;
        }


    }
}
