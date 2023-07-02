using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public enum BehaviorTreeResult
    {
        None,
        Fail,
        Success,
        Running,
    }



    /// <summary>
    /// 行为树节点
    /// </summary>
    [Serializable]
    public abstract class BehaviorTreeNode
    {
        //public string name;

        //运行时设置的参数
        [HideInInspector] public Dictionary<string, object> blackboard = null;
        protected bool isInitialized = false;

        //调试用
        //public bool NodeIsRunning = false;

        public virtual void Init(BehaviorTreeNode parent = null)
        {
            isInitialized = true;
            if (parent != null)
            {
                blackboard = parent.blackboard;
            }
            else if (blackboard == null)
            {
                blackboard = new Dictionary<string, object>();
            }
        }

        /// <summary>
        /// 进入行为时执行
        /// </summary>
        public virtual void EnterAction()
        {
            //NodeisRunning = true;
            //Debug.Log("action enter: " + this.GetType().Name);
        }

        /// <summary>
        /// 每一帧行为执行前调用，行为树的所有节点都会执行
        /// </summary>
        public virtual void DoBeforeAction() { }

        public virtual BehaviorTreeResult DoAction()
        {
            return BehaviorTreeResult.None;
        }

        /// <summary>
        /// 每一帧行为执行后调用，行为树的所有节点都会执行
        /// </summary>
        public virtual void DoAfterAction() { }

        /// <summary>
        /// 退出行为时执行状态的恢复,强制打断时会调用
        /// </summary>
        public virtual void ExitAction()
        {
            //NodeisRunning = false;
            //Debug.Log("action exit: " + this.GetType().Name);
        }



        public void SetValue<T>(string key, T value)
        {
            if (blackboard == null)
            {
                blackboard = new Dictionary<string, object>();
            }
            blackboard[key] = value;
        }

        public T GetValue<T>(string key)
        {
            if (blackboard == null)
            {
                blackboard = new Dictionary<string, object>();
            }
            if (blackboard.ContainsKey(key))
            {
                return (T)blackboard[key];
            }
            return default;
        }


    }
}
