using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 拿出主武器
    /// </summary>
    [Serializable]
    public class HoldMainWeapon : ActionNode
    {
        NPCController controller;



        public override void EnterAction()
        {
            base.EnterAction();
            var gameObject = GetValue<GameObject>("gameObject");
            controller = gameObject.GetComponent<NPCController>();
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            bool success = controller.HoldMainWeapon();
            if (success)
            {
                return BehaviorTreeResult.Success;
            }
            return BehaviorTreeResult.Running;
        }
    }
}
