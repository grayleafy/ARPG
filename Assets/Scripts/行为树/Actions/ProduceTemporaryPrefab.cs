using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 生成一段时间后清除的预制体
    /// </summary>
    [Serializable]
    public class ProduceTemporaryPrefab : ActionNode
    {
        public string prefabName;
        public float lastingTime;
        public Vector3 positionOffset;
        public Quaternion rotationOffset;
        public Vector3 scale;

        GameObject gameObject;

        public override void Init(BehaviorTreeNode parent = null)
        {
            base.Init(parent);
            gameObject = GetValue<GameObject>("gameObject");
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();

            PoolMgr.GetInstance().GetObj(prefabName, (o) =>
            {
                o.transform.SetParent(gameObject.transform, false);
                o.transform.localPosition = positionOffset;
                o.transform.localRotation = rotationOffset;
                o.transform.localScale = scale;
                o.GetComponent<ParticleSystem>()?.Play();

                MonoMgr.GetInstance().StartCoroutine(PushObj(prefabName, o, lastingTime));
            });

            return BehaviorTreeResult.Success;
        }

        IEnumerator PushObj(string name, GameObject o, float t)
        {
            yield return new WaitForSeconds(t);
            PoolMgr.GetInstance().PushObj(name, o);
        }
    }
}
