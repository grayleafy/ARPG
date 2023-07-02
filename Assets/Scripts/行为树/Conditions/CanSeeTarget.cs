using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 可以看到目标
    /// </summary>
    [Serializable]
    public class CanSeeTarget : Condition
    {
        public float eyeAngle = 60;
        public float eyeDistance = 10;
        public Vector3 rayOffset = new Vector3(0, 1f, 0);

        GameObject targetGameObject;
        GameObject gameObject;

        public override void EnterAction()
        {
            base.EnterAction();
            gameObject = GetValue<GameObject>("gameObject");
            targetGameObject = GetValue<GameObject>("targetGameObject");
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            if (isInEyeArea(targetGameObject))
            {
                return BehaviorTreeResult.Success;
            }

            return BehaviorTreeResult.Fail;
        }

        bool isInEyeArea(GameObject target)
        {
            //首先是扇形内
            Vector3 dir = target.transform.position - gameObject.transform.position;
            Vector3 forward = gameObject.transform.forward;
            float cos = Vector3.Dot(forward, dir.normalized);
            float angle = Mathf.Acos(cos) / Mathf.PI * 180;
            if (Mathf.Abs(angle) > eyeAngle)
            {
                return false;
            }

            //范围约束
            if (dir.magnitude > eyeDistance)
            {
                return false;
            }

            //然后判断有没有障碍物
            if (Physics.Raycast(gameObject.transform.position + rayOffset, dir, out RaycastHit hitInfo, dir.magnitude))
            {
                if (hitInfo.collider.gameObject != target)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
