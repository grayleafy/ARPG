using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 执行武器附带的行为树
    /// </summary>
    [Serializable]
    public class WeaponBehavior : ActionNode
    {
        public WeaponHoldType weapon;
        public WeaponBehaviorType weaponBehaviorType;
        public enum WeaponBehaviorType
        {
            Attack,
            Aim,
            Shoot,
        }


        [SerializeReference, SubclassSelector] BehaviorTreeNode weaponBehavior = null;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);

            GameObject gameObject = GetValue<GameObject>("gameObject");
            var npcInfo = gameObject.GetComponent<NPCController>().info;


            //未装备对应武器
            if (weapon == WeaponHoldType.MainWeapon && npcInfo.mainWeaponID <= 0)
            {
                return;
            }
            if (weapon == WeaponHoldType.SecondaryWeapon && npcInfo.secondaryWeaponID <= 0)
            {
                return;
            }

            Weapon weaponInfo = null;
            if (weapon == WeaponHoldType.MainWeapon)
            {
                weaponInfo = DataManager.GetInstance().GetData().weapons.Find(x => x.id == npcInfo.mainWeaponID);
            }
            else if (weapon == WeaponHoldType.SecondaryWeapon)
            {
                weaponInfo = DataManager.GetInstance().GetData().weapons.Find(x => x.id == npcInfo.secondaryWeaponID);
            }

            if (weaponBehaviorType == WeaponBehaviorType.Attack)
            {
                weaponBehavior = weaponInfo.attackBehavior.DeepClone();
            }
            else if (weaponBehaviorType == WeaponBehaviorType.Aim)
            {
                weaponBehavior = weaponInfo.aimBehavior.DeepClone();
            }
            else if (weaponBehaviorType == WeaponBehaviorType.Shoot)
            {
                weaponBehavior = weaponInfo.shootBehavior.DeepClone();
            }
            weaponBehavior?.Init(this);
        }

        public override void EnterAction()
        {
            base.EnterAction();

            weaponBehavior?.EnterAction();
        }


        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            if (weaponBehavior != null)
            {
                return weaponBehavior.DoAction();
            }
            return BehaviorTreeResult.Fail;
        }

        public override void ExitAction()
        {
            base.ExitAction();
            weaponBehavior?.ExitAction();
        }
    }
}