using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 鼠标左键按下则执行子节点
    /// </summary>
    [Serializable]
    public class InputMouseLeft : Condition
    {
        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            if (InputMgr.GetInstance().mouseLeft)
            {
                return BehaviorTreeResult.Success;
            }

            return BehaviorTreeResult.Fail;
        }
    }
}
