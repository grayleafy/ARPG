using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 持续自动追踪目标
    /// </summary>
    [Serializable]
    public class FollwTargetGameObject : ActionNode
    {
        GameObject gameObject;
        NPCController controller;
        GameObject target = null;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);
            gameObject = GetValue<GameObject>("gameObject");
            controller = gameObject.GetComponent<NPCController>();

        }

        //public override void EnterAction()
        //{
        //    base.EnterAction();

        //    if (controller.info.tag == CharacterTag.Player || controller.info.tag == CharacterTag.Ally)
        //    {
        //        List<GameObject> list = GameObject.FindGameObjectsWithTag(CharacterTag.Enemy.ToString()).ToList();
        //        target = list.FindMin<GameObject, float>((x) => (x.transform.position - gameObject.transform.position).magnitude);
        //    }
        //    else if (controller.info.tag == CharacterTag.Enemy)
        //    {
        //        List<GameObject> list = GameObject.FindGameObjectsWithTag(CharacterTag.Player.ToString()).ToList();
        //        list.AddRange(GameObject.FindGameObjectsWithTag(CharacterTag.Ally.ToString()).ToList());
        //        target = list.FindMin<GameObject, float>((x) => (x.transform.position - gameObject.transform.position).magnitude);
        //    }
        //}

        public override void EnterAction()
        {
            base.EnterAction();
            target = GetValue<GameObject>("targetGameObject");
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            controller.Follow(target.transform.position);
            return BehaviorTreeResult.Running;
        }

        public override void ExitAction()
        {
            base.ExitAction();
            controller.Follow(gameObject.transform.position);
            controller.StopFollow();
        }
    }
}
