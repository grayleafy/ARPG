using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BT
{
    /// <summary>
    /// 获得金钱
    /// </summary>
    [Serializable]
    public class GainMoney : ActionNode
    {
        public int money;


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
            controller.info.money += money;
            return BehaviorTreeResult.Success;
        }
    }
}
