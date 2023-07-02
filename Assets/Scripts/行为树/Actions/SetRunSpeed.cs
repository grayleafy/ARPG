using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BT
{
    /// <summary>
    /// 设置跑步速度
    /// </summary>
    [Serializable]
    public class SetRunSpeed : ActionNode
    {
        public float runSpeed;

        GameObject gameObject;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);
            gameObject = GetValue<GameObject>("gameObject");
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();

            gameObject.GetComponent<NavMeshAgent>().speed = runSpeed;
            return BehaviorTreeResult.Success;
        }
    }
}
