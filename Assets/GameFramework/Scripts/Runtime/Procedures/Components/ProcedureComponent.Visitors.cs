/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ProcedureComponent.Visitors.cs
 * author:    云毅
 * created:
 * descrip:   流程状态机组件 - 序列化配置、属性、过渡动画参数
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 流程状态机组件 - 序列化配置、属性与过渡动画参数分部类
    /// </summary>
    public sealed partial class ProcedureComponent : GameComponent
    {
        //=========================================================================
        #region 序列化配置（Inspector 可见）
        //=========================================================================

        /// <summary>
        /// 所有流程的完整类型名称集合（用于反射实例化）
        /// </summary>
        [SerializeField]
        private string[] m_ProcedureTypeNames = null;

        /// <summary>
        /// 游戏启动入口流程类型名称
        /// </summary>
        [SerializeField]
        private string m_EntryProcedureTypeName = null;

        /// <summary>
        /// 闪屏界面 AB 包路径
        /// </summary>
        [SerializeField]
        private string m_UISplashABPath;

        /// <summary>
        /// 闪屏界面资源名称
        /// </summary>
        [SerializeField]
        private string m_UISplashAssetName;

        /// <summary>
        /// 闪屏流程默认显示时长（秒）
        /// </summary>
        [SerializeField]
        private int m_SplashProcedureDuration = 5;

        /// <summary>
        /// 是否启用预加载界面
        /// </summary>
        [SerializeField]
        private bool m_UseUIPreload;

        /// <summary>
        /// 预加载界面 AB 包路径
        /// </summary>
        [SerializeField]
        private string m_UIPreloadABPath;

        /// <summary>
        /// 预加载界面资源名称
        /// </summary>
        [SerializeField]
        private string m_UIPreloadAssetName;

        #endregion

        //=========================================================================
        #region 公共属性（外部访问）
        //=========================================================================

        /// <summary>
        /// 流程切换过渡界面 AB 包路径
        /// </summary>
        /// <summary>
        /// 闪屏界面 AB 包路径
        /// </summary>
        public string UISplashABPath
        {
            get { return m_UISplashABPath; }
        }

        /// <summary>
        /// 闪屏界面资源名称
        /// </summary>
        public string UISplashAssetName
        {
            get { return m_UISplashAssetName; }
        }

        /// <summary>
        /// 闪屏流程默认显示时长（秒）
        /// </summary>
        public int SplashProcedureDuration
        {
            get { return m_SplashProcedureDuration; }
        }

        /// <summary>
        /// 是否启用预加载界面
        /// </summary>
        public bool UseUIPreload
        {
            get { return m_UseUIPreload; }
        }

        /// <summary>
        /// 预加载界面 AB 包路径
        /// </summary>
        public string UIPreloadABPath
        {
            get { return m_UIPreloadABPath; }
        }

        /// <summary>
        /// 预加载界面资源名称
        /// </summary>
        public string UIPreloadAssetName
        {
            get { return m_UIPreloadAssetName; }
        }

        #endregion

        //=========================================================================
        #region 静态配置
        //=========================================================================

        /// <summary>
        /// Lua 脚本白名单（仅白名单内流程可绑定 Lua 逻辑）
        /// </summary>
        public static List<string> LuaScriptWhiteNameList = new List<string>
        {
            "ProcedurePreload",
            "ProcedurePlaying",
        };

        #endregion

        //=========================================================================
        #region 运行时私有变量
        //=========================================================================

        /// <summary>
        /// 流程状态机（核心驱动）
        /// </summary>
        private ProcedureStateMachine m_ProcedureStateMachine;

        /// <summary>
        /// 入口流程实例
        /// </summary>
        private ProcedureState m_EntryProcedure = null;

        /// <summary>
        /// 运行时流程切换记录（用于日志/打点/调试）
        /// </summary>
        private List<string> m_RuntimeProcedureRecordInfos = null;

        #endregion

        //=========================================================================
        #region 运行时只读属性
        //=========================================================================

        /// <summary>
        /// 运行时流程切换记录（用于日志/打点/调试）
        /// </summary>
        public List<string> RuntimeProcedureRecordInfos
        {
            get { return m_RuntimeProcedureRecordInfos; }
        }

        /// <summary>
        /// 当前运行中的流程
        /// </summary>
        public ProcedureState CurrentProcedure
        {
            get
            {
                if (m_ProcedureStateMachine == null)
                    throw new GameException("必须先初始化 procedure 状态机。");
                return (ProcedureState)m_ProcedureStateMachine.CurrentState;
            }
        }

        /// <summary>
        /// 当前运行中的流程（状态机未初始化时返回 null，供编辑器或安全访问使用）
        /// </summary>
        public ProcedureState CurrentProcedureOrNull
        {
            get { return m_ProcedureStateMachine == null ? null : (ProcedureState)m_ProcedureStateMachine.CurrentState; }
        }

        /// <summary>
        /// 当前流程已运行时长
        /// </summary>
        public float CurrentProcedureTime
        {
            get
            {
                if (m_ProcedureStateMachine == null)
                    throw new GameException("必须先初始化 procedure 状态机。");
                return m_ProcedureStateMachine.CurrentStateTime;
            }
        }

        /// <summary>
        /// 在类型名配置数组中定位当前运行流程对应的下标
        /// </summary>
        /// <returns>命中返回下标，未命中返回 -1</returns>
        private int FindCurrentProcedureIndex()
        {
            Type currentType = m_ProcedureStateMachine.CurrentState.GetType();
            for (int index = 0; index < m_ProcedureTypeNames.Length; index++)
            {
                if (Type.GetType(m_ProcedureTypeNames[index]) == currentType)
                {
                    return index;
                }
            }

            return -1;
        }
        #endregion
    }
}