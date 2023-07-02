using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 生命值首次低于某个值
    /// </summary>
    [Serializable]
    public class HealthFirstLower : Condition
    {
        public float threshold = 0.5f;

        GameObject gameObject;
        NPCController npcController;
        bool isLower = false;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);
            gameObject = GetValue<GameObject>("gameObject");
            npcController = gameObject.GetComponent<NPCController>();

            if ((npcController.info.health / npcController.info.maxHealth) >= threshold)
            {
                isLower = false;
            }
            else
            {
                isLower = true;
            }
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();

            if ((npcController.info.health / npcController.info.maxHealth) < threshold)
            {
                if (isLower == false)
                {
                    isLower = true;
                    return BehaviorTreeResult.Success;
                }
            }

            return BehaviorTreeResult.Fail;
        }
    }
}
