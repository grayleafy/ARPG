using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 执行武器定制化效果
    /// </summary>
    [Serializable]
    public class WeaponCustomEffect : ActionNode
    {
        public WeaponHoldType weapon;
        public WeaponHoldController.WeaponEffectType weaponEffectType;


        WeaponHoldController controller;

        public override void EnterAction()
        {
            base.EnterAction();
            var gameObject = GetValue<GameObject>("gameObject");
            controller = gameObject.GetComponent<WeaponHoldController>();
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            controller.CustomWeaponEffect(weapon, weaponEffectType);
            return BehaviorTreeResult.Success;
        }
    }
}
