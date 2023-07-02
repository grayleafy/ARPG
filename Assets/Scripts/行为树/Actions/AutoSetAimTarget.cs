using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 自动设置瞄准目标，如果是玩家，则设置为鼠标。否则设置为离自己最近的敌对单位
    /// </summary>
    [Serializable]
    public class AutoSetAimTarget : ActionNode
    {

        GameObject gameObject;
        NPCController controller;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);
            gameObject = GetValue<GameObject>("gameObject");
            controller = gameObject.GetComponent<NPCController>();
        }


        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            var target = AutoFindTarget(gameObject);
            SetValue<Transform>("AimTargetTransform", target);
            controller.SetAimTarget(target);
            return BehaviorTreeResult.Success;
        }

        Transform AutoFindTarget(GameObject character)
        {
            Transform target = null;
            if (character.tag == CharacterTag.Player.ToString())
            {
                target = InputMgr.GetInstance().GetAimPoint();

            }
            else if (character.tag == CharacterTag.Enemy.ToString())
            {
                List<GameObject> targetObjects = new();
                targetObjects.AddRange(GameObject.FindGameObjectsWithTag(CharacterTag.Player.ToString()).ToList());
                targetObjects.AddRange(GameObject.FindGameObjectsWithTag(CharacterTag.Ally.ToString()).ToList());
                target = targetObjects.FindMin<GameObject, float>((x) => (x.transform.position - gameObject.transform.position).magnitude).transform;
            }
            else if (character.tag == CharacterTag.Ally.ToString())
            {
                List<GameObject> targetObjects = new();
                targetObjects.AddRange(GameObject.FindGameObjectsWithTag(CharacterTag.Enemy.ToString()).ToList());
                target = targetObjects.FindMin<GameObject, float>((x) => (x.transform.position - gameObject.transform.position).magnitude).transform;
            }


            return target;
        }
    }
}
