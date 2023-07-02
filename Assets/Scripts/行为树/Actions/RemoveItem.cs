using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 移除一个道具
    /// </summary>
    [Serializable]
    public class RemoveItem : ActionNode
    {
        public int ItemID;

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            var gameObject = GetValue<GameObject>("gameObject");
            if (gameObject == null) { return BehaviorTreeResult.Fail; }
            var info = gameObject.GetComponent<NPCController>();
            if (info == null)
            {
                return BehaviorTreeResult.Fail;
            }

            var bag = info.info.bag;

            ItemInfo itemInfo = bag.usableItems.Find((x) => x.id == ItemID);
            if (itemInfo != null)
            {
                bag.usableItems.Remove(itemInfo);
                return BehaviorTreeResult.Success;
            }
            else
            {
                itemInfo = bag.weapons.Find((x) => x.id == ItemID);
                if (itemInfo != null)
                {
                    bag.weapons.Remove(itemInfo);
                    return BehaviorTreeResult.Success;
                }
            }

            return BehaviorTreeResult.Fail;
        }
    }
}
