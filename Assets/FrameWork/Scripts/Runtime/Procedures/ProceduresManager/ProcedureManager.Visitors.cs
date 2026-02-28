using System;
using System.Collections.Generic;
using GameLib;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class ProcedureManager 
    {
         /// <summary>
        /// 流程类型名称集合
        /// </summary>
        [SerializeField]
        private string[] m_ProcedureTypeNames = null;

        /// <summary>
        /// 流程在切换时是否需要“进入式”切换过渡
        /// </summary>
        [SerializeField]
        private bool[] m_ProcedureTransitionEnterFlags = null;

        /// <summary>
        /// 流程在“进入式”切换时持续时间
        /// </summary>
        [SerializeField]
        private float[] m_ProcedureTransitionEnterDurations = null;

        /// <summary>
        /// 流程在“进入式”切换时屏蔽触摸
        /// </summary>
        [SerializeField]
        private bool[] m_ProcedureTransitionEnterBlockRaycasts = null;

        /// <summary>
        /// 流程在切换时是否需要“退出式”切换过渡
        /// </summary>
        [SerializeField]
        private bool[] m_ProcedureTransitionExitFlags = null;

        /// <summary>
        /// 流程在“退出式”切换时持续时间
        /// </summary>
        [SerializeField]
        private float[] m_ProcedureTransitionExitDurations = null;

        /// <summary>
        /// 流程在“退出式”切换时屏蔽触摸
        /// </summary>
        [SerializeField]
        private bool[] m_ProcedureTransitionExitBlockRaycasts = null;

        /// <summary>
        /// 流程在切换时是否需要“进入式”切换过渡（针对于ProcedurePreload，前次流程为ProcedureHotfix时有效）
        /// </summary>
        [SerializeField]
        private bool m_ProcedureTransitionEnterFlagFromProcedureHotfix;

        /// <summary>
        /// 流程在“进入式”切换时持续时间（针对于ProcedurePreload，前次流程为ProcedureHotfix时有效）
        /// </summary>
        [SerializeField]
        private float m_ProcedureTransitionEnterDurationFromProcedureHotfix;

        /// <summary>
        /// 流程在“进入式”切换时屏蔽触摸（针对于ProcedurePreload，前次流程为ProcedureHotfix时有效）
        /// </summary>
        [SerializeField]
        private bool m_ProcedureTransitionEnterBlockRaycastFromProcedureHotfix;

        /// <summary>
        /// 流程在切换时是否需要“退出式”切换过渡（针对于ProcedurePreload，前次流程为ProcedureHotfix时有效）
        /// </summary>
        [SerializeField]
        private bool m_ProcedureTransitionExitFlagFromProcedureHotfix;

        /// <summary>
        /// 流程在“退出式”切换时持续时间（针对于ProcedurePreload，前次流程为ProcedureHotfix时有效）
        /// </summary>
        [SerializeField]
        private float m_ProcedureTransitionExitDurationFromProcedureHotfix;

        /// <summary>
        /// 流程在“退出式”切换时屏蔽触摸（针对于ProcedurePreload，前次流程为ProcedureHotfix时有效）
        /// </summary>
        [SerializeField]
        private bool m_ProcedureTransitionExitBlockRaycastFromProcedureHotfix;

        /// <summary>
        /// 启动入口流程名称
        /// </summary>
        [SerializeField]
        private string m_EntryProcedureTypeName = null;

        /// <summary>
        /// 切换过渡界面AB路径
        /// </summary>
        [SerializeField]
        private string m_UITransitionABPath;
        public string UITransitionABPath
        {
            get
            {
                return m_UITransitionABPath;
            }
        }

        /// <summary>
        /// 切换过渡界面Asset资源名称
        /// </summary>
        [SerializeField]
        private string m_UITransitionAssetName;
        public string UITransitionAssetName
        {
            get
            {
                return m_UITransitionAssetName;
            }
        }

        /// <summary>
        /// 闪屏界面AB路径
        /// </summary>
        [SerializeField]
        private string m_UISplashABPath;
        public string UISplashABPath
        {
            get
            {
                return m_UISplashABPath;
            }
        }

        /// <summary>
        /// 闪屏界面Asset资源名称
        /// </summary>
        [SerializeField]
        private string m_UISplashAssetName;
        public string UISplashAssetName
        {
            get
            {
                return m_UISplashAssetName;
            }
        }

        /// <summary>
        /// 闪屏流程的持续时间（单位s）
        /// </summary>
        [SerializeField]
        private int m_SplashProcedureDuration = 5;
        public int SplashProcedureDuration
        {
            get
            {
                return m_SplashProcedureDuration;
            }
        }

        /// <summary>
        /// 使用预加载界面
        /// </summary>
        [SerializeField]
        private bool m_UseUIPreload;
        public bool UseUIPreload
        {
            get
            {
                return m_UseUIPreload;
            }
        }

        /// <summary>
        /// 预加载界面AB路径
        /// </summary>
        [SerializeField]
        private string m_UIPreloadABPath;
        public string UIPreloadABPath
        {
            get
            {
                return m_UIPreloadABPath;
            }
        }

        /// <summary>
        /// 预加载界面Asset资源名称
        /// </summary>
        [SerializeField]
        private string m_UIPreloadAssetName;
        public string UIPreloadAssetName
        {
            get
            {
                return m_UIPreloadAssetName;
            }
        }

        /// <summary>
        /// Lua脚本白名单
        /// 只有白名单上的脚本才允许生成lua脚本
        /// </summary>
        public static List<string> LuaScriptWhiteNameList = new List<string>
        {
            "ProcedurePreload",
            "ProcedurePlaying",
        };



        /// <summary>
        /// 运行过程中流程记录信息
        /// </summary>
        private List<string> m_RuntimeProcedureRecordInfos = null;
        public List<string> RuntimeProcedureRecordInfos
        {
            get
            {
                return m_RuntimeProcedureRecordInfos;
            }
        }

        /// <summary>
        /// 获取当前流程。
        /// </summary>
        public ProcedureState CurrentProcedure
        {
            get
            {
                if (GameConfig.instance.m_ProcedureStateMachine == null)
                {
                    throw new GameException("必须先初始化 procedure 状态机。");
                }

                return (ProcedureState)GameConfig.instance.m_ProcedureStateMachine.CurrentState;
            }
        }

        /// <summary>
        /// 获取当前流程持续时间。
        /// </summary>
        public float CurrentProcedureTime
        {
            get
            {
                if (GameConfig.instance.m_ProcedureStateMachine == null)
                {
                    throw new GameException("必须先初始化 procedure 状态机。");
                }

                return GameConfig.instance.m_ProcedureStateMachine.CurrentStateTime;
            }
        }

        /// <summary>
        /// 获取当前流程在切换时是否需要“进入式”切换过渡
        /// </summary>
        public bool CurrentProcedureTransitionEnterFlag
        {
            get
            {
                for (int index = 0; index < m_ProcedureTypeNames.Length; index++)
                {
                    Type procedureType = Type.GetType(m_ProcedureTypeNames[index]);
                    if (GameConfig.instance.m_ProcedureStateMachine.CurrentState.GetType() == procedureType)
                    {
                        return m_ProcedureTransitionEnterFlags[index];
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 获取当前流程在“进入式”切换时的持续时间
        /// </summary>
        public float CurrentProcedureTransitionEnterDuration
        {
            get
            {
                for (int index = 0; index < m_ProcedureTypeNames.Length; index++)
                {
                    Type procedureType = Type.GetType(m_ProcedureTypeNames[index]);
                    if (GameConfig.instance.m_ProcedureStateMachine.CurrentState.GetType() == procedureType)
                    {
                        return m_ProcedureTransitionEnterDurations[index];
                    }
                }
                return 0f;
            }
        }

        /// <summary>
        /// 获取当前流程在“进入式”切换时屏蔽触摸
        /// </summary>
        public bool CurrentProcedureTransitionEnterBlockRaycast
        {
            get
            {
                for (int index = 0; index < m_ProcedureTypeNames.Length; index++)
                {
                    Type procedureType = Type.GetType(m_ProcedureTypeNames[index]);
                    if (GameConfig.instance.m_ProcedureStateMachine.CurrentState.GetType() == procedureType)
                    {
                        return m_ProcedureTransitionEnterBlockRaycasts[index];
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 获取当前流程在切换时是否需要“退出式”切换过渡
        /// </summary>
        public bool CurrentProcedureTransitionExitFlag
        {
            get
            {
                for (int index = 0; index < m_ProcedureTypeNames.Length; index++)
                {
                    Type procedureType = Type.GetType(m_ProcedureTypeNames[index]);
                    if (GameConfig.instance.m_ProcedureStateMachine.CurrentState.GetType() == procedureType)
                    {
                        return m_ProcedureTransitionExitFlags[index];
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 获取当前流程在“退出式”切换时的持续时间
        /// </summary>
        public float CurrentProcedureTransitionExitDuration
        {
            get
            {
                for (int index = 0; index < m_ProcedureTypeNames.Length; index++)
                {
                    Type procedureType = Type.GetType(m_ProcedureTypeNames[index]);
                    if (GameConfig.instance.m_ProcedureStateMachine.CurrentState.GetType() == procedureType)
                    {
                        return m_ProcedureTransitionExitDurations[index];
                    }
                }
                return 0f;
            }
        }

        /// <summary>
        /// 获取当前流程在“退出式”切换时屏蔽触摸
        /// </summary>
        public bool CurrentProcedureTransitionExitBlockRaycast
        {
            get
            {
                for (int index = 0; index < m_ProcedureTypeNames.Length; index++)
                {
                    Type procedureType = Type.GetType(m_ProcedureTypeNames[index]);
                    if (GameConfig.instance.m_ProcedureStateMachine.CurrentState.GetType() == procedureType)
                    {
                        return m_ProcedureTransitionExitBlockRaycasts[index];
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 获取当前流程在切换时是否需要“进入式”切换过渡（针对于ProcedurePreload，前次流程为ProcedureHotfix时有效）
        /// </summary>
        public bool CurrentProcedureTransitionEnterFlagFromProcedureHotfix
        {
            get
            {
                return m_ProcedureTransitionEnterFlagFromProcedureHotfix;
            }
        }

        /// <summary>
        /// 获取当前流程“进入式”切换时持续时间（针对于ProcedurePreload，前次流程为ProcedureHotfix时有效）
        /// </summary>
        public float CurrentProcedureTransitionEnterDurationFromProcedureHotfix
        {
            get
            {
                return m_ProcedureTransitionEnterDurationFromProcedureHotfix;
            }
        }

        /// <summary>
        /// 获取当前流程“进入式”切换时屏蔽触摸（针对于ProcedurePreload，前次流程为ProcedureHotfix时有效）
        /// </summary>
        public bool CurrentProcedureTransitionEnterBlockRaycastFromProcedureHotfix
        {
            get
            {
                return m_ProcedureTransitionEnterBlockRaycastFromProcedureHotfix;
            }
        }

        /// <summary>
        /// 获取当前流程在切换时是否需要“退出式”切换过渡（针对于ProcedurePreload，前次流程为ProcedureHotfix时有效）
        /// </summary>
        public bool CurrentProcedureTransitionExitFlagFromProcedureHotfix
        {
            get
            {
                return m_ProcedureTransitionExitFlagFromProcedureHotfix;
            }
        }

        /// <summary>
        /// 获取当前流程“退出式”切换时持续时间（针对于ProcedurePreload，前次流程为ProcedureHotfix时有效）
        /// </summary>
        public float CurrentProcedureTransitionExitDurationFromProcedureHotfix
        {
            get
            {
                return m_ProcedureTransitionExitDurationFromProcedureHotfix;
            }
        }

        /// <summary>
        /// 获取当前流程“退出式”切换时屏蔽触摸（针对于ProcedurePreload，前次流程为ProcedureHotfix时有效）
        /// </summary>
        public bool CurrentProcedureTransitionExitBlockRaycastFromProcedureHotfix
        {
            get
            {
                return m_ProcedureTransitionExitBlockRaycastFromProcedureHotfix;
            }
        }
    }
}