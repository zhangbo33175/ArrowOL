using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class ProcedureComponent : GameComponent
    {
        /// <summary>
        /// 所有流程的完整类型名称集合（用于反射实例化）
        /// </summary>
        [SerializeField]
        private string[] m_ProcedureTypeNames = null;

        /// <summary>
        /// 对应流程：切换时是否需要【进入式】过渡动画
        /// </summary>
        [SerializeField]
        private bool[] m_ProcedureTransitionEnterFlags = null;

        /// <summary>
        /// 对应流程：【进入式】过渡动画持续时间
        /// </summary>
        [SerializeField]
        private float[] m_ProcedureTransitionEnterDurations = null;

        /// <summary>
        /// 对应流程：【进入式】过渡时是否屏蔽触摸
        /// </summary>
        [SerializeField]
        private bool[] m_ProcedureTransitionEnterBlockRaycasts = null;

        /// <summary>
        /// 对应流程：切换时是否需要【退出式】过渡动画
        /// </summary>
        [SerializeField]
        private bool[] m_ProcedureTransitionExitFlags = null;

        /// <summary>
        /// 对应流程：【退出式】过渡动画持续时间
        /// </summary>
        [SerializeField]
        private float[] m_ProcedureTransitionExitDurations = null;

        /// <summary>
        /// 对应流程：【退出式】过渡时是否屏蔽触摸
        /// </summary>
        [SerializeField]
        private bool[] m_ProcedureTransitionExitBlockRaycasts = null;

        /// <summary>
        /// 特殊流程配置：从 Hotfix 流程进入 Preload 流程时，是否启用进入过渡
        /// </summary>
        [SerializeField]
        private bool m_ProcedureTransitionEnterFlagFromProcedureHotfix;

        /// <summary>
        /// 特殊流程配置：从 Hotfix 进入 Preload 的进入过渡时长
        /// </summary>
        [SerializeField]
        private float m_ProcedureTransitionEnterDurationFromProcedureHotfix;

        /// <summary>
        /// 特殊流程配置：从 Hotfix 进入 Preload 时是否屏蔽触摸
        /// </summary>
        [SerializeField]
        private bool m_ProcedureTransitionEnterBlockRaycastFromProcedureHotfix;

        /// <summary>
        /// 特殊流程配置：从 Preload 退回到 Hotfix 时，是否启用退出过渡
        /// </summary>
        [SerializeField]
        private bool m_ProcedureTransitionExitFlagFromProcedureHotfix;

        /// <summary>
        /// 特殊流程配置：从 Preload 退回到 Hotfix 的退出过渡时长
        /// </summary>
        [SerializeField]
        private float m_ProcedureTransitionExitDurationFromProcedureHotfix;

        /// <summary>
        /// 特殊流程配置：从 Preload 退回到 Hotfix 时是否屏蔽触摸
        /// </summary>
        [SerializeField]
        private bool m_ProcedureTransitionExitBlockRaycastFromProcedureHotfix;

        /// <summary>
        /// 游戏启动入口流程类型名称
        /// </summary>
        [SerializeField]
        private string m_EntryProcedureTypeName = null;

        /// <summary>
        /// 流程切换过渡界面 AB 包路径
        /// </summary>
        [SerializeField]
        private string m_UITransitionABPath;
        public string UITransitionABPath
        {
            get { return m_UITransitionABPath; }
        }

        /// <summary>
        /// 流程切换过渡界面资源名称
        /// </summary>
        [SerializeField]
        private string m_UITransitionAssetName;
        public string UITransitionAssetName
        {
            get { return m_UITransitionAssetName; }
        }

        /// <summary>
        /// 闪屏界面 AB 包路径
        /// </summary>
        [SerializeField]
        private string m_UISplashABPath;
        public string UISplashABPath
        {
            get { return m_UISplashABPath; }
        }

        /// <summary>
        /// 闪屏界面资源名称
        /// </summary>
        [SerializeField]
        private string m_UISplashAssetName;
        public string UISplashAssetName
        {
            get { return m_UISplashAssetName; }
        }

        /// <summary>
        /// 闪屏流程默认显示时长（秒）
        /// </summary>
        [SerializeField]
        private int m_SplashProcedureDuration = 5;
        public int SplashProcedureDuration
        {
            get { return m_SplashProcedureDuration; }
        }

        /// <summary>
        /// 是否启用预加载界面
        /// </summary>
        [SerializeField]
        private bool m_UseUIPreload;
        public bool UseUIPreload
        {
            get { return m_UseUIPreload; }
        }

        /// <summary>
        /// 预加载界面 AB 包路径
        /// </summary>
        [SerializeField]
        private string m_UIPreloadABPath;
        public string UIPreloadABPath
        {
            get { return m_UIPreloadABPath; }
        }

        /// <summary>
        /// 预加载界面资源名称
        /// </summary>
        [SerializeField]
        private string m_UIPreloadAssetName;
        public string UIPreloadAssetName
        {
            get { return m_UIPreloadAssetName; }
        }

        /// <summary>
        /// Lua 脚本白名单（仅白名单内流程可绑定 Lua 逻辑）
        /// </summary>
        public static List<string> LuaScriptWhiteNameList = new List<string>
        {
            "ProcedurePreload",
            "ProcedurePlaying",
        };

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
        /// 当前流程是否需要【进入式】过渡
        /// </summary>
        public bool CurrentProcedureTransitionEnterFlag
        {
            get
            {
                for (int index = 0; index < m_ProcedureTypeNames.Length; index++)
                {
                    Type procedureType = Type.GetType(m_ProcedureTypeNames[index]);
                    if (m_ProcedureStateMachine.CurrentState.GetType() == procedureType)
                        return m_ProcedureTransitionEnterFlags[index];
                }
                return false;
            }
        }

        /// <summary>
        /// 当前流程【进入式】过渡时长
        /// </summary>
        public float CurrentProcedureTransitionEnterDuration
        {
            get
            {
                for (int index = 0; index < m_ProcedureTypeNames.Length; index++)
                {
                    Type procedureType = Type.GetType(m_ProcedureTypeNames[index]);
                    if (m_ProcedureStateMachine.CurrentState.GetType() == procedureType)
                        return m_ProcedureTransitionEnterDurations[index];
                }
                return 0f;
            }
        }

        /// <summary>
        /// 当前流程【进入式】过渡是否屏蔽触摸
        /// </summary>
        public bool CurrentProcedureTransitionEnterBlockRaycast
        {
            get
            {
                for (int index = 0; index < m_ProcedureTypeNames.Length; index++)
                {
                    Type procedureType = Type.GetType(m_ProcedureTypeNames[index]);
                    if (m_ProcedureStateMachine.CurrentState.GetType() == procedureType)
                        return m_ProcedureTransitionEnterBlockRaycasts[index];
                }
                return false;
            }
        }

        /// <summary>
        /// 当前流程是否需要【退出式】过渡
        /// </summary>
        public bool CurrentProcedureTransitionExitFlag
        {
            get
            {
                for (int index = 0; index < m_ProcedureTypeNames.Length; index++)
                {
                    Type procedureType = Type.GetType(m_ProcedureTypeNames[index]);
                    if (m_ProcedureStateMachine.CurrentState.GetType() == procedureType)
                        return m_ProcedureTransitionExitFlags[index];
                }
                return false;
            }
        }

        /// <summary>
        /// 当前流程【退出式】过渡时长
        /// </summary>
        public float CurrentProcedureTransitionExitDuration
        {
            get
            {
                for (int index = 0; index < m_ProcedureTypeNames.Length; index++)
                {
                    Type procedureType = Type.GetType(m_ProcedureTypeNames[index]);
                    if (m_ProcedureStateMachine.CurrentState.GetType() == procedureType)
                        return m_ProcedureTransitionExitDurations[index];
                }
                return 0f;
            }
        }

        /// <summary>
        /// 当前流程【退出式】过渡是否屏蔽触摸
        /// </summary>
        public bool CurrentProcedureTransitionExitBlockRaycast
        {
            get
            {
                for (int index = 0; index < m_ProcedureTypeNames.Length; index++)
                {
                    Type procedureType = Type.GetType(m_ProcedureTypeNames[index]);
                    if (m_ProcedureStateMachine.CurrentState.GetType() == procedureType)
                        return m_ProcedureTransitionExitBlockRaycasts[index];
                }
                return false;
            }
        }

        /// <summary>
        /// 特殊流程：从 Hotfix → Preload 是否启用进入过渡
        /// </summary>
        public bool CurrentProcedureTransitionEnterFlagFromProcedureHotfix
        {
            get { return m_ProcedureTransitionEnterFlagFromProcedureHotfix; }
        }

        /// <summary>
        /// 特殊流程：从 Hotfix → Preload 进入过渡时长
        /// </summary>
        public float CurrentProcedureTransitionEnterDurationFromProcedureHotfix
        {
            get { return m_ProcedureTransitionEnterDurationFromProcedureHotfix; }
        }

        /// <summary>
        /// 特殊流程：从 Hotfix → Preload 进入过渡是否屏蔽触摸
        /// </summary>
        public bool CurrentProcedureTransitionEnterBlockRaycastFromProcedureHotfix
        {
            get { return m_ProcedureTransitionEnterBlockRaycastFromProcedureHotfix; }
        }

        /// <summary>
        /// 特殊流程：从 Preload → Hotfix 是否启用退出过渡
        /// </summary>
        public bool CurrentProcedureTransitionExitFlagFromProcedureHotfix
        {
            get { return m_ProcedureTransitionExitFlagFromProcedureHotfix; }
        }

        /// <summary>
        /// 特殊流程：从 Preload → Hotfix 退出过渡时长
        /// </summary>
        public float CurrentProcedureTransitionExitDurationFromProcedureHotfix
        {
            get { return m_ProcedureTransitionExitDurationFromProcedureHotfix; }
        }

        /// <summary>
        /// 特殊流程：从 Preload → Hotfix 退出过渡是否屏蔽触摸
        /// </summary>
        public bool CurrentProcedureTransitionExitBlockRaycastFromProcedureHotfix
        {
            get { return m_ProcedureTransitionExitBlockRaycastFromProcedureHotfix; }
        }
    }
}