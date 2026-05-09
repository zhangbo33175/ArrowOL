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
            ProcedureState[] procedures = new ProcedureState[m_ProcedureTypeNames.Length];
            for (int i = 0; i < m_ProcedureTypeNames.Length; i++)
            {
                // 反射获取流程类型
                Type procedureType = Type.GetType(m_ProcedureTypeNames[i]);
                if (procedureType == null)
                {
                    Log.Error("无法找到 procedure 类型 '{0}'.", m_ProcedureTypeNames[i]);
                    return;
                }

                // 反射创建流程实例
                procedures[i] = (ProcedureState)Activator.CreateInstance(procedureType);
                if (procedures[i] == null)
                {
                    Log.Error("无法创建 procedure 实例 '{0}'.", m_ProcedureTypeNames[i]);
                    return;
                }

                // 匹配并记录入口流程
                if (m_EntryProcedureTypeName == m_ProcedureTypeNames[i])
                {
                    m_EntryProcedure = procedures[i];
                }
            }

            // 入口流程不能为空
            if (m_EntryProcedure == null)
            {
                Log.Error("入口 procedure 无效。");
                return;
            }

            // 初始化流程状态机
            m_ProcedureStateMachine = new ProcedureStateMachine(this, procedures);

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

        /// <summary>
        /// 初始化所有流程的 Lua 脚本绑定
        /// 白名单内的流程会自动关联对应 Lua 逻辑
        /// </summary>
        public void InitLuaBindings()
        {
            var procedures = m_ProcedureStateMachine.GetAllStates();
            for (int i = 0; i < m_ProcedureTypeNames.Length; i++)
            {
                // 截取类型名称作为脚本名
                string[] words = m_ProcedureTypeNames[i].Split('.');
                string luaScriptName = words[words.Length - 1];

                // 白名单内流程执行 Lua 绑定
                if (LuaScriptWhiteNameList.Contains(luaScriptName))
                {
                    ((ProcedureState)procedures[i]).InitLuaBindings(luaScriptName);
                }
            }
        }

        /// <summary>
        /// 启动指定类型的流程
        /// </summary>
        /// <param name="procedureType">流程类型</param>
        private void StartProcedure(Type procedureType)
        {
            if (m_ProcedureStateMachine == null)
            {
                throw new GameException("必须先初始化 ProcedureStateMachine。");
            }
            m_ProcedureStateMachine.Start(procedureType);
        }

        /// <summary>
        /// 判断是否包含某个流程
        /// </summary>
        public bool HasProcedure(Type procedureType)
        {
            if (m_ProcedureStateMachine == null)
            {
                throw new GameException("必须先初始化 ProcedureStateMachine。");
            }
            return m_ProcedureStateMachine.HasState(procedureType);
        }

        /// <summary>
        /// 获取指定类型的流程实例
        /// </summary>
        public ProcedureState GetProcedure(Type procedureType)
        {
            if (m_ProcedureStateMachine == null)
            {
                throw new GameException("必须先初始化 ProcedureStateMachine。");
            }
            return (ProcedureState)m_ProcedureStateMachine.GetState(procedureType);
        }

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
    }
}