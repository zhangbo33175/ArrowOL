using System;
using System.Collections.Generic;
using Honor.Runtime;
using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 有限状态机。
    /// </summary>
    /// <typeparam name="T">有限状态机持有者类型。</typeparam>
    public abstract class StateMachine<T> where T : class
    {
        /// <summary>
        /// 有限状态机名称
        /// </summary>
        protected string m_Name;

        /// <summary>
        /// 有限状态机的持有者
        /// </summary>
        protected T m_Owner;

        /// <summary>
        /// 有限状态机中的状态集合
        /// </summary>
        protected readonly Dictionary<Type, State<T>> m_States;

        /// <summary>
        /// 有限状态机的前一次状态
        /// </summary>
        protected State<T> m_LastState;

        /// <summary>
        /// 前一次状态的持续时间
        /// </summary>
        protected float m_LastStateTime;

        /// <summary>
        /// 有限状态机的当前状态
        /// </summary>
        protected State<T> m_CurrentState;

        /// <summary>
        /// 当前状态的持续时间
        /// </summary>
        protected float m_CurrentStateTime;

        /// <summary>
        /// 有限状态机是否已经销毁
        /// </summary>
        protected bool m_IsDestroyed;

        /// <summary>
        /// 初始化有限状态机的新实例。
        /// </summary>
        public StateMachine(T owner, params State<T>[] states)
        {
            if (owner == null)
            {
                throw new Exception("状态机 owner 无效。");
            }

            if (states == null || states.Length < 1)
            {
                throw new Exception("状态机 states 无效。");
            }

            m_Name = string.Empty;
            m_Owner = owner;
            m_IsDestroyed = false;
            m_States = new Dictionary<Type, State<T>>();
            m_LastState = null;
            m_LastStateTime = 0f;
            m_CurrentState = null;
            m_CurrentStateTime = 0f;

            foreach (State<T> state in states)
            {
                if (state == null)
                {
                    throw new Exception("状态机 states 无效。");
                }

                Type stateType = state.GetType();
                if (m_States.ContainsKey(stateType))
                {
                    throw new Exception(AorTxt.Format("状态机 state '{1}' 已经存在。", stateType));
                }

                m_States.Add(stateType, state);
                state.OnInit(this);
            }
        }

        /// <summary>
        /// 获取有限状态机名称。
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            protected set { m_Name = value ?? string.Empty; }
        }

        /// <summary>
        /// 获取有限状态机持有者。
        /// </summary>
        public T Owner
        {
            get { return m_Owner; }
        }

        /// <summary>
        /// 获取有限状态机持有者类型。
        /// </summary>
        public Type OwnerType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// 获取有限状态机中状态的数量。
        /// </summary>
        public int StateCount
        {
            get { return m_States.Count; }
        }

        /// <summary>
        /// 获取有限状态机是否正在运行。
        /// </summary>
        public bool IsRunning
        {
            get { return m_CurrentState != null; }
        }

        /// <summary>
        /// 获取有限状态机是否被销毁。
        /// </summary>
        public bool IsDestroyed
        {
            get { return m_IsDestroyed; }
        }

        /// <summary>
        /// 获取前一次有限状态机状态。
        /// </summary>
        public State<T> LastState
        {
            get { return m_LastState; }
        }

        /// <summary>
        /// 获取前一次有限状态机状态名称。
        /// </summary>
        public string LastStateName
        {
            get { return m_LastState != null ? m_LastState.GetType().FullName : null; }
        }

        /// <summary>
        /// 获取前一次有限状态机状态持续时间。
        /// </summary>
        public float LastStateTime
        {
            get { return m_LastStateTime; }
        }

        /// <summary>
        /// 获取当前有限状态机状态。
        /// </summary>
        public State<T> CurrentState
        {
            get { return m_CurrentState; }
        }

        /// <summary>
        /// 获取当前有限状态机状态名称。
        /// </summary>
        public string CurrentStateName
        {
            get { return m_CurrentState != null ? m_CurrentState.GetType().FullName : null; }
        }

        /// <summary>
        /// 获取当前有限状态机状态持续时间。
        /// </summary>
        public float CurrentStateTime
        {
            get { return m_CurrentStateTime; }
        }

        /// <summary>
        /// 清理有限状态机。
        /// </summary>
        public void Clear()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.OnLeave(this, true);
            }

            foreach (KeyValuePair<Type, State<T>> state in m_States)
            {
                state.Value.OnDestroy(this);
            }

            Name = null;
            m_Owner = null;
            m_States.Clear();
            m_LastState = null;
            m_LastStateTime = 0f;
            m_CurrentState = null;
            m_CurrentStateTime = 0f;
            m_IsDestroyed = true;
        }

        /// <summary>
        /// 启动有限状态机。
        /// </summary>
        /// <param name="stateType">要开始的有限状态机状态类型。</param>
        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                throw new Exception("状态机正在执行中，不能再次开始。");
            }

            if (stateType == null)
            {
                throw new Exception("State type 无效。");
            }

            if (!typeof(State<T>).IsAssignableFrom(stateType))
            {
                throw new Exception(AorTxt.Format("State type '{0}' 无效。", stateType.FullName));
            }

            State<T> state = GetState(stateType);
            if (state == null)
            {
                throw new Exception(AorTxt.Format("状态机 '{0}' 不能开始一个不存在的 state '{1}'。", Name, stateType.FullName));
            }

            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        /// <summary>
        /// 是否存在有限状态机状态。
        /// </summary>
        /// <param name="stateType">要检查的有限状态机状态类型。</param>
        /// <returns>是否存在有限状态机状态。</returns>
        public bool HasState(Type stateType)
        {
            if (stateType == null)
            {
                throw new Exception("State type 无效。");
            }

            if (!typeof(State<T>).IsAssignableFrom(stateType))
            {
                throw new Exception(AorTxt.Format("State type '{0}' 无效。", stateType.FullName));
            }

            return m_States.ContainsKey(stateType);
        }

        /// <summary>
        /// 获取有限状态机状态。
        /// </summary>
        /// <param name="stateType">要获取的有限状态机状态类型。</param>
        /// <returns>要获取的有限状态机状态。</returns>
        public State<T> GetState(Type stateType)
        {
            if (stateType == null)
            {
                throw new Exception("State type 无效。");
            }

            if (!typeof(State<T>).IsAssignableFrom(stateType))
            {
                throw new Exception(AorTxt.Format("State type '{0}' 无效。", stateType.FullName));
            }

            State<T> state = null;
            if (m_States.TryGetValue(stateType, out state))
            {
                return state;
            }

            return null;
        }

        /// <summary>
        /// 获取有限状态机的所有状态。
        /// </summary>
        /// <returns>有限状态机的所有状态。</returns>
        public State<T>[] GetAllStates()
        {
            int index = 0;
            State<T>[] results = new State<T>[m_States.Count];
            foreach (KeyValuePair<Type, State<T>> state in m_States)
            {
                results[index++] = state.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取有限状态机的所有状态。
        /// </summary>
        /// <param name="results">有限状态机的所有状态。</param>
        public void GetAllStates(List<State<T>> results)
        {
            if (results == null)
            {
                throw new Exception("Results 无效。");
            }

            results.Clear();
            foreach (KeyValuePair<Type, State<T>> state in m_States)
            {
                results.Add(state.Value);
            }
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <param name="stateType">要切换到的有限状态机状态类型。</param>
        public void ChangeState(Type stateType)
        {
            if (m_CurrentState == null)
            {
                throw new Exception("Current state 无效。");
            }

            State<T> state = GetState(stateType);
            if (state == null)
            {
                throw new Exception(AorTxt.Format("状态机 '{0}' 不能切换到不存在的 state '{1}'。", Name, stateType.FullName));
            }

            m_CurrentState.OnLeave(this, false);
            m_LastState = m_CurrentState;
            m_LastStateTime = m_CurrentStateTime;
            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        /// <summary>
        /// 有限状态机轮询。
        /// </summary>
        public virtual void Update()
        {
            if (m_CurrentState == null)
            {
                return;
            }

            m_CurrentStateTime += Time.deltaTime;
            m_CurrentState.OnUpdate(this);
        }

        /// <summary>
        /// 关闭并清理有限状态机。
        /// </summary>
        public virtual void Shutdown()
        {
        }
    }
}