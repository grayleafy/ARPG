using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 释放魔法预制体
    /// </summary>
    [Serializable]
    public class CastMagicOnTargetGameObject : ActionNode
    {
        public string prefabName;

        GameObject gameObject;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);
            gameObject = GetValue<GameObject>("gameObject");
        }

        public override BehaviorTreeResult DoAction()
        {

            base.DoAction();
            GameObject targetGameObject = GetValue<GameObject>("targetGameObject");
            if (targetGameObject == null)
            {
                return BehaviorTreeResult.Fail;
            }

            PoolMgr.GetInstance().GetObj(prefabName, (o) =>
            {
                o.transform.position = targetGameObject.transform.position;
                o.GetComponent<CircleHit>().characterTag = gameObject.GetComponent<NPCController>().info.tag;
            });
            return BehaviorTreeResult.Success;
        }
    }
}
