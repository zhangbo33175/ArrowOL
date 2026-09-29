/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ProcedureComponent.cs
 * author:    云毅
 * created:
 * descrip:   游戏流程状态机核心组件 - 流程管理、状态切换、Lua绑定、日志记录
 ***************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 流程状态机组件（游戏核心流程管理器）
    /// 负责：流程实例化、状态切换、生命周期驱动、Lua 绑定、流程记录
    /// 游戏启动、登录、主界面、战斗等所有场景流程均由此管理
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class ProcedureComponent : GameComponent
    {
        //=========================================================================
        #region 生命周期
        //=========================================================================

        /// <summary>
        /// 组件初始化
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// 启动时：根据配置自动实例化所有流程 → 找到入口流程 → 启动状态机
        /// </summary>
        private void Start()
        {
            // 初始化流程运行时记录列表
            m_RuntimeProcedureRecordInfos = new List<string>();

            // 根据配置的流程类型名称数组，批量实例化流程类
            ProcedureState[] stateInstances = new ProcedureState[m_ProcedureTypeNames.Length];
            for (int cursor = 0; cursor < m_ProcedureTypeNames.Length; cursor++)
            {
                // 反射获取流程类型
                Type resolvedType = Type.GetType(m_ProcedureTypeNames[cursor]);
                if (resolvedType == null)
                {
                    Log.Error("无法找到 procedure 类型 '{0}'.", m_ProcedureTypeNames[cursor]);
                    return;
                }

                // 反射创建流程实例
                stateInstances[cursor] = (ProcedureState)Activator.CreateInstance(resolvedType);
                if (stateInstances[cursor] == null)
                {
                    Log.Error("无法创建 procedure 实例 '{0}'.", m_ProcedureTypeNames[cursor]);
                    return;
                }

                // 匹配并记录入口流程
                if (m_EntryProcedureTypeName == m_ProcedureTypeNames[cursor])
                {
                    m_EntryProcedure = stateInstances[cursor];
                }
            }

            // 入口流程不能为空
            if (m_EntryProcedure == null)
            {
                Log.Error("入口 procedure 无效。");
                return;
            }

            // 初始化流程状态机
            m_ProcedureStateMachine = new ProcedureStateMachine(this, stateInstances);

            // 启动入口流程
            StartProcedure(m_EntryProcedure.GetType());
        }

        /// <summary>
        /// 每帧驱动流程状态机更新
        /// </summary>
        private void Update()
        {
            if (m_ProcedureStateMachine != null)
            {
                m_ProcedureStateMachine.Update();
            }
        }

        /// <summary>
        /// 组件销毁（不直接清理状态机，避免循环销毁）
        /// </summary>
        private void OnDestroy()
        {
            // 禁止在OnDestroy中间接调用destroy接口
            //if (m_ProcedureStateMachine != null)
            //{
            //    m_ProcedureStateMachine.Clear();
            //}
        }

        #endregion

        //=========================================================================
        #region Lua 绑定
        //=========================================================================

        /// <summary>
        /// 初始化所有流程的 Lua 脚本绑定
        /// 白名单内的流程会自动关联对应 Lua 逻辑
        /// </summary>
        public void InitLuaBindings()
        {
            var procedures = m_ProcedureStateMachine.GetAllStates();
            for (int cursor = 0; cursor < m_ProcedureTypeNames.Length; cursor++)
            {
                // 截取类型名称作为脚本名
                string[] words = m_ProcedureTypeNames[cursor].Split('.');
                string luaScriptName = words[words.Length - 1];

                // 白名单内流程执行 Lua 绑定
                if (LuaScriptWhiteNameList.Contains(luaScriptName))
                {
                    ((ProcedureState)procedures[cursor]).InitLuaBindings(luaScriptName);
                }
            }
        }

        #endregion

        //=========================================================================
        #region 流程控制
        //=========================================================================

        /// <summary>
        /// 校验状态机已初始化，否则抛出异常
        /// </summary>
        private void EnsureStateMachineReady()
        {
            if (m_ProcedureStateMachine == null)
            {
                throw new GameException("必须先初始化 ProcedureStateMachine。");
            }
        }

        /// <summary>
        /// 启动指定类型的流程
        /// </summary>
        /// <param name="procedureType">流程类型</param>
        private void StartProcedure(Type procedureType)
        {
            EnsureStateMachineReady();
            m_ProcedureStateMachine.Start(procedureType);
        }

        /// <summary>
        /// 判断是否包含某个流程
        /// </summary>
        public bool HasProcedure(Type procedureType)
        {
            EnsureStateMachineReady();
            return m_ProcedureStateMachine.HasState(procedureType);
        }

        /// <summary>
        /// 获取指定类型的流程实例
        /// </summary>
        public ProcedureState GetProcedure(Type procedureType)
        {
            EnsureStateMachineReady();
            return (ProcedureState)m_ProcedureStateMachine.GetState(procedureType);
        }

        #endregion

        //=========================================================================
        #region 运行时日志记录
        //=========================================================================

        /// <summary>
        /// 记录流程运行时信息（用于日志、打点、调试）
        /// </summary>
        /// <param name="procedureTypeName">流程名</param>
        /// <param name="time">耗时</param>
        public void RecordRuntimeProcedureInfos(string procedureTypeName, float time)
        {
            if (Application.isPlaying)
            {
                m_RuntimeProcedureRecordInfos.Add(AorTxt.Format("{0}\t（{1:N2}秒）", procedureTypeName, time));
            }
        }

        #endregion
    }
}