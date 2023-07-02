using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 检测targetGameObjects是否在半径范围内
    /// </summary>
    [Serializable]
    public class TargetRangeChecker : Condition
    {
        public float radius = 7;


        GameObject gameObject;
        GameObject target;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);
            gameObject = GetValue<GameObject>("gameObject");

        }

        public override void EnterAction()
        {
            base.EnterAction();
            target = GetValue<GameObject>("targetGameObject");
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();

            float distance = (target.transform.position - gameObject.transform.position).magnitude;    //当场景中原来的目标被销毁时会出错

            if (distance < radius)
            {
                return BehaviorTreeResult.Success;
            }

            return BehaviorTreeResult.Fail;
        }

    }
}
