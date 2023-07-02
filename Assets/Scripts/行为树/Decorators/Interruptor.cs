using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 可强制打断子节点
    /// </summary>
    [Serializable]
    public class Interruptor : Decorator
    {
        [Tooltip("如果该字符串的黑板变量为真，则打断")]
        public string conditionParamName;

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();

            if (GetValue<bool>(conditionParamName))
            {
                return BehaviorTreeResult.Success;
            }

            StartChildAction(child);
            return child.DoAction();
        }
    }
}