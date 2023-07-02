using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 设置最近的敌对单位作为黑板的TargetGameObject变量
    /// </summary>
    [Serializable]
    public class AutoSetClosetAsTargetGameObject : ActionNode
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
            if (target != null)
            {
                SetValue<GameObject>("targetGameObject", target.gameObject);
            }
            else
            {
                return BehaviorTreeResult.Fail;
            }



            return BehaviorTreeResult.Success;
        }

        Transform AutoFindTarget(GameObject character)
        {
            Transform target = null;
            if (controller.info.tag == CharacterTag.Player)
            {
                target = InputMgr.GetInstance().GetAimPoint();

            }
            else if (controller.info.tag == CharacterTag.Enemy)
            {
                List<GameObject> targetObjects = new();
                targetObjects.AddRange(GameObject.FindGameObjectsWithTag(CharacterTag.Player.ToString()).ToList());
                targetObjects.AddRange(GameObject.FindGameObjectsWithTag(CharacterTag.Ally.ToString()).ToList());
                var targetGameObject = targetObjects.FindMin<GameObject, float>((x) => (x.transform.position - gameObject.transform.position).magnitude);
                if (targetGameObject != null)
                    target = targetGameObject.transform;
            }
            else if (controller.info.tag == CharacterTag.Ally)
            {
                List<GameObject> targetObjects = new();
                targetObjects.AddRange(GameObject.FindGameObjectsWithTag(CharacterTag.Enemy.ToString()).ToList());
                var targetGameObject = targetObjects.FindMin<GameObject, float>((x) => (x.transform.position - gameObject.transform.position).magnitude);
                if (targetGameObject != null)
                    target = targetGameObject.transform;
            }


            return target;
        }
    }
}
