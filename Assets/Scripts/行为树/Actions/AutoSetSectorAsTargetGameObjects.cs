using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 设置扇形区域的敌人为黑板的targetGameObjects变量
    /// </summary>
    [Serializable]
    public class AutoSetSectorAsTargetGameObjects : ActionNode
    {
        [Header("半径")]
        [SerializeField] public float radius;
        [Header("角度范围")]
        [SerializeField] public float angleRange_L;
        [SerializeField] public float angleRange_R;
        [Header("高度相差范围")]
        [SerializeField] public float heightRange_L;
        [SerializeField] public float heightRange_R;



        GameObject gameObject;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);
            gameObject = GetValue<GameObject>("gameObject");
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();

            GameObject[] enermys = null;
            if (gameObject.tag == "Player" || gameObject.tag == "Ally")
            {
                enermys = GameObject.FindGameObjectsWithTag("Enemy");
            }
            else if (gameObject.tag == "Enemy")
            {
                List<GameObject> list = new();
                list.AddRange(GameObject.FindGameObjectsWithTag("Player").ToList());
                list.AddRange(GameObject.FindGameObjectsWithTag("Ally").ToList());
                enermys = list.ToArray();
            }
            if (enermys == null || enermys.Length == 0)
            {
                return BehaviorTreeResult.Fail;
            }

            List<GameObject> targetGameObjects = new();
            for (int i = 0; i < enermys.Length; i++)
            {
                if (IsInArea(enermys[i]))
                {
                    //EventCenter.GetInstance().EventTrigger<(GameObject, GameObject, float, float)>("伤害判定", (gameObject, enermys[i], damageValue, toughnessPenalty));
                    targetGameObjects.Add(enermys[i]);
                }
            }

            SetValue<List<GameObject>>("targetGameObjects", targetGameObjects);
            return BehaviorTreeResult.Success;
        }


        bool IsInArea(GameObject enemy)
        {
            Vector3 enemyPos = enemy.transform.position;
            enemyPos = gameObject.transform.InverseTransformPoint(enemyPos);

            //半径限制
            if (enemyPos.x * enemyPos.x + enemyPos.z * enemyPos.z > radius * radius)
            {
                return false;
            }

            //高度范围
            if (enemyPos.y < angleRange_L || enemyPos.y > angleRange_R)
            {
                return false;
            }

            //角度范围
            float angle = MathF.Atan2(enemyPos.x, enemyPos.z) * 180 / Mathf.PI;
            if (angle < angleRange_L || angle > angleRange_R)
            {
                return false;
            }

            return true;
        }
    }
}