using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    [Serializable]
    public abstract class Composite : BehaviorTreeNode
    {
        [SerializeReference, SubclassSelector] public List<BehaviorTreeNode> children = null;

        protected List<BehaviorTreeNode> runningChildren = new();

        //初始化黑板
        public override void Init(BehaviorTreeNode parent)
        {
            base.Init(parent);



            if (children != null)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].Init(this);
                }
            }
        }

        /// <summary>
        /// 使子节点开始执行并调用enterAction
        /// </summary>
        /// <param name="child"></param>
        protected void StartChildAction(BehaviorTreeNode child)
        {
            if (runningChildren.Contains(child)) return;
            runningChildren.Add(child);
            child.EnterAction();
        }

        /// <summary>
        /// 使子节点停止执行并调用ExitAction
        /// </summary>
        /// <param name="child"></param>
        protected void StopChildAction(BehaviorTreeNode child)
        {
            if (!runningChildren.Contains(child)) return;
            runningChildren.Remove(child);
            child.ExitAction();
        }

        /// <summary>
        /// 结束当前正在运行的所有子节点
        /// </summary>
        public override void ExitAction()
        {
            base.ExitAction();
            for (int i = 0; i < runningChildren.Count; i++)
            {
                runningChildren[i].ExitAction();
            }
            runningChildren.Clear();
        }

        public override void DoBeforeAction()
        {
            base.DoBeforeAction();
            foreach (var child in children)
            {
                child.DoBeforeAction();
            }
        }

        public override void DoAfterAction()
        {
            base.DoAfterAction();
            foreach (var child in children)
            {
                child.DoAfterAction();
            }
        }
    }
}