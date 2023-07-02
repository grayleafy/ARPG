using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BT
{
    [Serializable]
    public class Decorator : BehaviorTreeNode
    {
        [SerializeReference, SubclassSelector] public BehaviorTreeNode child;

        bool childRunning;



        /// <summary>
        /// 使子节点开始执行并调用enterAction
        /// </summary>
        /// <param name="child"></param>
        protected void StartChildAction(BehaviorTreeNode child)
        {
            if (childRunning) return;
            childRunning = true;
            child.EnterAction();
        }

        /// <summary>
        /// 使子节点停止执行并调用ExitAction
        /// </summary>
        /// <param name="child"></param>
        protected void StopChildAction(BehaviorTreeNode child)
        {
            if (!childRunning) return;
            childRunning = false;
            child.ExitAction();
        }

        public override void Init(BehaviorTreeNode parent)
        {
            base.Init(parent);


            if (child != null)
            {
                child.Init(this);
            }
        }

        public override void EnterAction()
        {
            base.EnterAction();
            childRunning = false;
            //if (child != null)
            //{
            //    child.EnterAction();
            //}
        }

        public override void ExitAction()
        {
            base.ExitAction();
            if (child != null)
            {
                StopChildAction(child);
            }
        }

        public override void DoBeforeAction()
        {
            base.DoBeforeAction();
            child.DoBeforeAction();
        }

        public override void DoAfterAction()
        {
            base.DoAfterAction();
            child.DoAfterAction();
        }
    }
}
