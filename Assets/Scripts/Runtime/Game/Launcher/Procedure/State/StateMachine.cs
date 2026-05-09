using System;
using System.Collections.Generic;
using Honor.Runtime;
using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 通用有限状态机基类 (FSM)
    /// 管理一组状态的切换、更新、生命周期
    /// </summary>
    /// <typeparam name="T">状态机持有者（拥有者）的类型</typeparam>
    public abstract class StateMachine<T> where T : class
    {
        /// <summary>
        /// 状态机名称（用于标识、调试）
        /// </summary>
        protected string m_Name;

        /// <summary>
        /// 状态机持有者（实体对象，如角色、怪物、UI）
        /// </summary>
        protected T m_Owner;

        /// <summary>
        /// 状态集合（Type -> State 映射）
        /// </summary>
        protected readonly Dictionary<Type, State<T>> m_States;

        /// <summary>
        /// 上一个状态
        /// </summary>
        protected State<T> m_LastState;

        /// <summary>
        /// 上一个状态持续了多久
        /// </summary>
        protected float m_LastStateTime;

        /// <summary>
        /// 当前状态
        /// </summary>
        protected State<T> m_CurrentState;

        /// <summary>
        /// 当前状态持续了多久
        /// </summary>
        protected float m_CurrentStateTime;

        /// <summary>
        /// 是否已销毁
        /// </summary>
        protected bool m_IsDestroyed;

        /// <summary>
        /// 构造函数：初始化状态机
        /// </summary>
        /// <param name="owner">持有者</param>
        /// <param name="states">所有状态实例</param>
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

            // 注册所有状态并初始化
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
        /// 状态机名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            protected set { m_Name = value ?? string.Empty; }
        }

        /// <summary>
        /// 状态机持有者
        /// </summary>
        public T Owner
        {
            get { return m_Owner; }
        }

        /// <summary>
        /// 持有者类型
        /// </summary>
        public Type OwnerType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// 状态数量
        /// </summary>
        public int StateCount
        {
            get { return m_States.Count; }
        }

        /// <summary>
        /// 是否正在运行（当前状态不为空）
        /// </summary>
        public bool IsRunning
        {
            get { return m_CurrentState != null; }
        }

        /// <summary>
        /// 是否已销毁
        /// </summary>
        public bool IsDestroyed
        {
            get { return m_IsDestroyed; }
        }

        /// <summary>
        /// 上一个状态
        /// </summary>
        public State<T> LastState
        {
            get { return m_LastState; }
        }

        /// <summary>
        /// 上一个状态名称
        /// </summary>
        public string LastStateName
        {
            get { return m_LastState != null ? m_LastState.GetType().FullName : null; }
        }

        /// <summary>
        /// 上一个状态持续时间
        /// </summary>
        public float LastStateTime
        {
            get { return m_LastStateTime; }
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public State<T> CurrentState
        {
            get { return m_CurrentState; }
        }

        /// <summary>
        /// 当前状态名称
        /// </summary>
        public string CurrentStateName
        {
            get { return m_CurrentState != null ? m_CurrentState.GetType().FullName : null; }
        }

        /// <summary>
        /// 当前状态已持续时间
        /// </summary>
        public float CurrentStateTime
        {
            get { return m_CurrentStateTime; }
        }

        /// <summary>
        /// 清空状态机（退出当前状态、销毁所有状态）
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
        /// 启动状态机（进入初始状态）
        /// </summary>
        /// <param name="stateType">初始状态类型</param>
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
        /// 是否包含某个状态
        /// </summary>
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
        /// 获取指定状态
        /// </summary>
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
        /// 获取所有状态（数组）
        /// </summary>
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
        /// 获取所有状态（List）
        /// </summary>
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
        /// 切换状态
        /// </summary>
        /// <param name="stateType">目标状态类型</param>
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

            // 退出当前状态
            m_CurrentState.OnLeave(this, false);

            // 记录上一个状态
            m_LastState = m_CurrentState;
            m_LastStateTime = m_CurrentStateTime;

            // 进入新状态
            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        /// <summary>
        /// 每帧更新
        /// 驱动当前状态的逻辑
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
        /// 关闭状态机（子类可重写）
        /// </summary>
        public virtual void Shutdown()
        {
        }
    }
}