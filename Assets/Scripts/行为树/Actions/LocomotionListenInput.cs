using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    [Serializable]
    public class LocomotionListenInput : ActionNode
    {
        public override void EnterAction()
        {
            base.EnterAction();
            GameObject gameObject = GetValue<GameObject>("gameObject");
            if (gameObject == null)
            {
                Debug.LogError("未设置实例对象");
            }

            LocomotionController locomotionController = gameObject.GetComponent<LocomotionController>();
            if (locomotionController == null)
            {
                Debug.LogError("未找到locomotionController");
            }

            locomotionController.StartListenInput();
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            return BehaviorTreeResult.Running;
        }

        public override void ExitAction()
        {
            base.ExitAction();
            GameObject gameObject = GetValue<GameObject>("gameObject");
            if (gameObject == null)
            {
                Debug.LogError("未设置实例对象");
            }

            LocomotionController locomotionController = gameObject.GetComponent<LocomotionController>();
            if (locomotionController == null)
            {
                Debug.LogError("未找到locomotionController");
            }

            locomotionController.StopListemInput();
        }
    }
}
