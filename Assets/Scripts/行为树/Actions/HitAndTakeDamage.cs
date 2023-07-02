using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    /// <summary>
    /// 伤害判定和顿帧
    /// </summary>
    [Serializable]
    public class HitAndTakeDamage : ActionNode
    {
        public float damageValue;
        public float toughnessPenalty;
        public float impactTime = 0.05f;

        GameObject gameObject;
        List<GameObject> targetGameObjects;
        float endTime;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);
            gameObject = GetValue<GameObject>("gameObject");
        }

        public override void EnterAction()
        {
            base.EnterAction();
            targetGameObjects = GetValue<List<GameObject>>("targetGameObjects");

            bool hit = false;

            for (int i = 0; i < targetGameObjects.Count; i++)
            {
                EventCenter.GetInstance().EventTrigger<(GameObject, GameObject, float, float)>("伤害判定", (gameObject, targetGameObjects[i], damageValue, toughnessPenalty));
                if (targetGameObjects[i].tag == "Enemy" || targetGameObjects[i].tag == "Player" || targetGameObjects[i].tag == "Ally")
                {
                    hit = true;
                }
            }

            endTime = Time.realtimeSinceStartup;

            if (hit)
            {
                Time.timeScale = 0.1f;
                endTime += impactTime;
            }


        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();


            float currentTime = Time.realtimeSinceStartup;
            if (currentTime >= endTime)
            {
                Time.timeScale = 1;
                return BehaviorTreeResult.Success;
            }

            return BehaviorTreeResult.Running;
        }



    }
}
