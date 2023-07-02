using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 角色控制行为树入口
    /// </summary>
    [Serializable]
    public class CharacterEntry : Entry
    {
        public override void Init(BehaviorTreeNode parent)
        {
            base.Init(parent);
            SetValue<bool>("interruptSkill", false);
        }

        public override void DoAfterAction()
        {
            base.DoAfterAction();
            if (GetValue<bool>("interruptSkill"))
            {
                SetValue<bool>("interruptSkill", false);
            }
        }


        public void InterruptSkill()
        {
            SetValue<bool>("interruptSkill", true);
        }
    }
}
