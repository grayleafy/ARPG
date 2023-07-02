using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 打开提示面板
    /// </summary>
    [Serializable]
    public class OpenToolTipPanel : ActionNode
    {
        public string toolTip;

        public override void EnterAction()
        {
            base.EnterAction();
            UIManager.GetInstance().ShowPanel<ToolTipPanel>("ToolTipPanel", E_UI_Layer.Mid, (panel) =>
            {
                panel.SetToolTip(toolTip);
            });
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            return BehaviorTreeResult.Success;
        }
    }
}
