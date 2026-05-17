/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  StateMachine.cs
 * author:    云毅
 * created:   2026
 * descrip:   有限状态机抽象基类，管理状态生命周期、切换与轮询
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    //=========================================================================
    // 有限状态机抽象基类
    //=========================================================================
    /// <summary>
    /// 有限状态机抽象基类
    /// 负责管理一组状态的生命周期、切换、轮询与运行时数据
    /// </summary>
    /// <typeparam name="T">状态机持有者类型</typeparam>
    public abstract class StateMachine<T> where T : class
    {
        #region 私有字段
        /// <summary>
        /// 状态机名称
        /// </summary>
        protected string m_Name;

        /// <summary>
        /// 状态机持有者（拥有该状态机的对象）
        /// </summary>
        protected T m_Owner;

        /// <summary>
        /// 状态集合（类型 -> 状态实例）
        /// </summary>
        protected readonly Dictionary<Type, State<T>> m_States;

        /// <summary>
        /// 上一个状态
        /// </summary>
        protected State<T> m_LastState;

        /// <summary>
        /// 上一个状态持续时间
        /// </summary>
        protected float m_LastStateTime;

        /// <summary>
        /// 当前状态
        /// </summary>
        protected State<T> m_CurrentState;

        /// <summary>
        /// 当前状态已持续时间
        /// </summary>
        protected float m_CurrentStateTime;

        /// <summary>
        /// 是否已销毁
        /// </summary>
        protected bool m_IsDestroyed;
        #endregion

        #region 构造函数
        /// <summary>
        /// 创建状态机
        /// </summary>
        /// <param name="owner">持有者</param>
        /// <param name="states">状态机包含的所有状态</param>
        /// <exception cref="GameException">参数无效时抛出异常</exception>
        public StateMachine(T owner, params State<T>[] states)
        {
            if (owner == null)
            {
                throw new GameException("状态机创建失败：Owner 不能为空。");
            }

            if (states == null || states.Length < 1)
            {
                throw new GameException("状态机创建失败：必须至少包含一个状态。");
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
                    throw new GameException("状态机创建失败：状态列表中包含空状态。");
                }

                Type stateType = state.GetType();
                if (m_States.ContainsKey(stateType))
                {
                    throw new GameException($"状态机创建失败：状态 {stateType} 已重复添加。");
                }

                m_States.Add(stateType, state);
                state.OnInit(this);
            }
        }
        #endregion

        #region 公共属性
        /// <summary>
        /// 状态机名称
        /// </summary>
        public string Name
        {
            get => m_Name;
            protected set => m_Name = value ?? string.Empty;
        }

        /// <summary>
        /// 状态机持有者
        /// </summary>
        public T Owner => m_Owner;

        /// <summary>
        /// 持有者类型
        /// </summary>
        public Type OwnerType => typeof(T);

        /// <summary>
        /// 状态数量
        /// </summary>
        public int StateCount => m_States.Count;

        /// <summary>
        /// 是否正在运行（当前状态不为空）
        /// </summary>
        public bool IsRunning => m_CurrentState != null;

        /// <summary>
        /// 是否已销毁
        /// </summary>
        public bool IsDestroyed => m_IsDestroyed;

        /// <summary>
        /// 上一个状态
        /// </summary>
        public State<T> LastState => m_LastState;

        /// <summary>
        /// 上一个状态名称
        /// </summary>
        public string LastStateName => m_LastState?.GetType().FullName;

        /// <summary>
        /// 上一个状态持续时间
        /// </summary>
        public float LastStateTime => m_LastStateTime;

        /// <summary>
        /// 当前状态
        /// </summary>
        public State<T> CurrentState => m_CurrentState;

        /// <summary>
        /// 当前状态名称
        /// </summary>
        public string CurrentStateName => m_CurrentState?.GetType().FullName;

        /// <summary>
        /// 当前状态已持续时间
        /// </summary>
        public float CurrentStateTime => m_CurrentStateTime;
        #endregion

        #region 状态机控制
        /// <summary>
        /// 清空并销毁状态机
        /// </summary>
        public void Clear()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.OnLeave(this, true);
            }

            foreach (var state in m_States.Values)
            {
                state.OnDestroy(this);
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
        /// 启动状态机
        /// </summary>
        /// <param name="stateType">初始状态类型</param>
        /// <exception cref="GameException">状态机已运行或状态无效时抛出异常</exception>
        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                throw new GameException("状态机已在运行中，无法重复启动。");
            }

            if (stateType == null)
            {
                throw new GameException("启动失败：状态类型不能为空。");
            }

            if (!typeof(State<T>).IsAssignableFrom(stateType))
            {
                throw new GameException($"启动失败：类型 {stateType.FullName} 不是有效状态。");
            }

            State<T> state = GetState(stateType);
            if (state == null)
            {
                throw new GameException($"启动失败：状态 {stateType.FullName} 不存在于状态机中。");
            }

            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        /// <summary>
        /// 关闭并销毁状态机
        /// </summary>
        public virtual void Shutdown()
        {
            Clear();
        }
        #endregion

        #region 状态操作
        /// <summary>
        /// 是否包含指定状态
        /// </summary>
        /// <param name="stateType">状态类型</param>
        /// <returns>是否包含</returns>
        public bool HasState(Type stateType)
        {
            if (stateType == null)
            {
                throw new GameException("状态类型不能为空。");
            }

            if (!typeof(State<T>).IsAssignableFrom(stateType))
            {
                throw new GameException($"类型 {stateType.FullName} 不是有效状态。");
            }

            return m_States.ContainsKey(stateType);
        }

        /// <summary>
        /// 获取指定状态
        /// </summary>
        /// <param name="stateType">状态类型</param>
        /// <returns>状态实例</returns>
        public State<T> GetState(Type stateType)
        {
            if (stateType == null)
            {
                throw new GameException("状态类型不能为空。");
            }

            if (!typeof(State<T>).IsAssignableFrom(stateType))
            {
                throw new GameException($"类型 {stateType.FullName} 不是有效状态。");
            }

            m_States.TryGetValue(stateType, out State<T> state);
            return state;
        }

        /// <summary>
        /// 获取所有状态
        /// </summary>
        /// <returns>状态数组</returns>
        public State<T>[] GetAllStates()
        {
            int index = 0;
            State<T>[] results = new State<T>[m_States.Count];
            foreach (var pair in m_States)
            {
                results[index++] = pair.Value;
            }
            return results;
        }

        /// <summary>
        /// 获取所有状态（不分配数组，更高效）
        /// </summary>
        /// <param name="results">接收结果的列表</param>
        public void GetAllStates(List<State<T>> results)
        {
            if (results == null)
            {
                throw new GameException("传入的结果列表不能为空。");
            }

            results.Clear();
            foreach (var pair in m_States)
            {
                results.Add(pair.Value);
            }
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="stateType">目标状态类型</param>
        /// <exception cref="GameException">状态机未运行或状态无效时抛出异常</exception>
        public void ChangeState(Type stateType)
        {
            if (m_CurrentState == null)
            {
                throw new GameException("切换失败：当前状态无效，状态机未运行。");
            }

            State<T> state = GetState(stateType);
            if (state == null)
            {
                throw new GameException($"切换失败：状态 {stateType.FullName} 不存在。");
            }

            m_CurrentState.OnLeave(this, false);
            m_LastState = m_CurrentState;
            m_LastStateTime = m_CurrentStateTime;
            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }
        #endregion

        #region 生命周期更新
        /// <summary>
        /// 每帧轮询状态机
        /// </summary>
        public virtual void Update()
        {
            if (m_CurrentState == null)
                return;

            m_CurrentStateTime += Time.deltaTime;
            m_CurrentState.OnUpdate(this);
        }
        #endregion
    }
}