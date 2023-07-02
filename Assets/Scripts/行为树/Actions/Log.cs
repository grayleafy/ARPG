using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    [Serializable]
    public class Log : ActionNode
    {
        public string logMessage;

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            Debug.Log(logMessage);
            return BehaviorTreeResult.Success;
        }
    }
}
