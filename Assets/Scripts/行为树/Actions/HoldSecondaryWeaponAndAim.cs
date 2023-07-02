using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 拿起副武器并瞄准，拿起动作执行完成后返回成功
    /// </summary>
    [Serializable]
    public class HoldSecondaryWeaponAndAim : ActionNode
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
            bool success = controller.Aim();
            if (success)
            {
                return BehaviorTreeResult.Success;
            }

            return BehaviorTreeResult.Running;
        }


    }
}