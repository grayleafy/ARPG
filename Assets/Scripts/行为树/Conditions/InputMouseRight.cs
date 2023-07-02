using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 鼠标右键是否按下
    /// </summary>
    [Serializable]
    public class InputMouseRight : Condition
    {
        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            if (InputMgr.GetInstance().mouseRight)
            {
                return BehaviorTreeResult.Success;
            }

            return BehaviorTreeResult.Fail;
        }
    }
}
