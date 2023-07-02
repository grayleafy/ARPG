using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BT
{
    /// <summary>
    /// 根据游戏对象的tag进行选择单一节点执行
    /// </summary>
    [Serializable]
    public class TagSelector : Composite
    {
        public List<string> childrenTag;

        GameObject gameObject = null;
        NPCController npcController = null;

        public override void Init(BehaviorTreeNode parent)
        {
            base.Init(parent);
            if (childrenTag.Count != children.Count)
            {
                Debug.LogError("标签数量和子节点数量不对应");
            }



        }

        public override void EnterAction()
        {
            base.EnterAction();
            gameObject = GetValue<GameObject>("gameObject");
            if (gameObject == null)
            {
                Debug.LogError("未设置角色对象");
            }
            npcController = gameObject.GetComponent<NPCController>();
        }

        public override BehaviorTreeResult DoAction()
        {
            base.DoAction();
            for (int i = 0; i < childrenTag.Count; i++)
            {
                if (childrenTag[i] == npcController.info.tag.ToString()) //tag在awake后更新
                {
                    if (runningChildren.Count == 0 || !runningChildren.Contains(children[i]))
                    {
                        if (runningChildren.Count > 0) StopChildAction(runningChildren[0]);
                        StartChildAction(children[i]);
                    }
                    return children[i].DoAction();
                }
            }

            return BehaviorTreeResult.Fail;
        }
    }
}
