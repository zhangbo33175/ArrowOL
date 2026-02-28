
using System.Collections.Generic;
using Honor.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameLib
{
    public partial class GameConfig
    {
        /// <summary>
        /// 运行帧率
        /// 游戏运行时每秒的最高帧数
        /// </summary>
        public int m_FrameRate = 60;

        /// <summary>
        /// 运行速率
        /// 游戏运行时的最快速率
        /// </summary>
        public float m_GameSpeed = 1f;

        /// <summary>
        /// 是否为本地服务器
        /// </summary>
        public bool m_IsLocalServer = true;

        /// <summary>
        /// 编辑器资源模式
        /// 是否以编辑器资源模式运行（仅编辑器内有效），在手机上时会自动校正归为false
        /// </summary>
        public bool m_EditorResourceMode;

        /// <summary>
        /// 
        /// </summary>
        [FormerlySerializedAs("M_StrictCheck")]
        public bool m_StrictCheck = false;

        /// <summary>
        /// 编辑器语言类型
        /// 编辑器运行模式下的语言类型
        /// </summary>
        [SerializeField] public GameDefinitions.Language m_EditorLanguage = GameDefinitions.Language.Unspecified;

        /// <summary>
        /// 开发模式
        /// 用于切换日常开发环境与生产环境的系统配置
        /// </summary>
        public bool m_DevelopMode = true;

        /// <summary>
        /// 是否启用屏幕方向变动检测
        /// </summary>
        public bool m_CheckOrientationState = false;

        /// <summary>
        /// Luac模式
        /// 是否以Luac模式运行
        /// </summary>
        public bool m_LuacMode = false;

        /// <summary>
        /// Lua热重载
        /// 是否启用Lua热重载
        /// </summary>
        public bool m_LuaHotReloadMode = false;

        /// <summary>
        /// 启用后台运行
        /// 游戏挂起时系统后台是否继续运行游戏
        /// </summary>
        public bool m_RunInBackground = true;

        /// <summary>
        /// Lua运行时分析模式
        /// 仅Editor中有效
        /// </summary>
        public bool m_LuaRuntimeProfilerMode;

        /// <summary>
        /// 启用屏幕常亮
        /// 游戏运行一段时间后屏幕是否常亮以避免设备进入锁定状态
        /// </summary>
        public bool m_NeverSleep = true;


        /// <summary>
        /// 流程状态机
        /// </summary>
        public ProcedureStateMachine m_ProcedureStateMachine;

        /// <summary>
        /// 启动入口流程
        /// </summary>
        public Honor.Runtime.ProcedureState m_EntryProcedure = null;
        /// <summary>
        /// 自定义设备分级/评级
        /// </summary>
        public bool m_CustomDevicePerformance = false;

        /// <summary>
        /// 开启设备分级/评级
        /// </summary>
        public bool m_UseDevicePerformance = false;
        
        
        /// <summary>
        /// 屏幕方向
        /// </summary>
        public ScreenOrientation m_ScreenOrientation = ScreenOrientation.Unknown;

        /// <summary>
        /// 语言类型
        /// </summary>
        public GameDefinitions.Language m_Language;

        /// <summary>
        /// 开启字体自动适配
        /// </summary>
        public bool m_AutoFontAdapt;

        /// <summary>
        /// 运行时前次语言类型
        /// 启动时默认强制为“简体中文”
        /// </summary>
        public GameDefinitions.Language m_RuntimeLastLanguage = GameDefinitions.Language.ChineseSimplified;
        
        /// <summary>
        /// 流程类型名称集合
        /// </summary>
        public string[] m_ProcedureTypeNames = null;

        /// <summary>
        /// 流程在切换时是否需要“进入式”切换过渡
        /// </summary>
        public bool[] m_ProcedureTransitionEnterFlags = null;
        /// <summary>
        /// 是否启用TextLocalizing脚本检测
        /// </summary>
        public bool m_CheckTextLocalizings;

        /// <summary>
        /// UI字体集合
        /// 类型：Font 或 TMP_FontAsset
        /// </summary>
        public List<Object> m_Fonts;

        /// <summary>
        /// 上一次UI字体集合
        /// 类型：Font 或 TMP_FontAsset
        /// </summary>
        public List<Object> m_LastFonts;


        /// <summary>
        /// 开启大版本与热更新检测
        /// </summary>
        public bool m_OpenVersionCheck;

        /// <summary>
        /// 启动入口流程名称
        /// </summary>
        public string m_EntryProcedureTypeName = null;

        /// <summary>
        /// 游戏缓存速率（用于取消暂停时游戏速率的恢复）
        /// </summary>
        public float m_GameSpeedCache = 1f;
        
        
        /// 获取当前流程。
        /// </summary>
        public ProcedureState CurrentProcedure
        {
            get
            {
                if (m_ProcedureStateMachine == null)
                {
                    throw new GameException("必须先初始化 procedure 状态机。");
                }

                return (ProcedureState)m_ProcedureStateMachine.CurrentState;
            }
        }
        
        /// <summary>
        /// 获取或设置是否禁止休眠
        /// </summary>
        public bool NeverSleep
        {
            get { return m_NeverSleep; }
            set
            {
                m_NeverSleep = value;
                Screen.sleepTimeout = value ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public float GameSpeedCache
        {
            get { return m_GameSpeedCache; }
        }
        
        /// <summary>
        /// 获取或设置游戏帧率
        /// </summary>
        public int FrameRate
        {
            get { return m_FrameRate; }
            set { Application.targetFrameRate = m_FrameRate = value; }
        }
        
        /// <summary>
        /// 获取或设置游戏速度
        /// </summary>
        public float GameSpeed
        {
            get { return m_GameSpeed; }
            set { Time.timeScale = m_GameSpeed = value >= 0f ? value : 0f; }
        }
    }
}