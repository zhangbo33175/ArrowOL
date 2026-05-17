/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LauncherComponent.Fields.cs
 * author:    云毅
 *  created:   2026
 * descrip:   框架启动器 - 序列化字段与公共属性（partial）
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 框架启动器 - 字段与属性部分
    /// </summary>
    public sealed partial class LauncherComponent : GameComponent
    {
        //=========================================================================
        // 序列化字段（Inspector 配置）
        //=========================================================================
        #region Serialized Fields
        /// <summary>
        /// 编辑器资源模式
        /// 是否以编辑器资源模式运行（仅编辑器内有效），在手机上时会自动校正归为false
        /// </summary>
        [SerializeField]
        private bool m_EditorResourceMode = true;

        /// <summary>
        /// 编辑器语言类型
        /// 编辑器运行模式下的语言类型
        /// </summary>
        [SerializeField]
        private GameDefinitions.Language m_EditorLanguage = GameDefinitions.Language.Unspecified;

        /// <summary>
        /// 开发模式
        /// 用于切换日常开发环境与生产环境的系统配置
        /// </summary>
        [SerializeField]
        private bool m_DevelopMode = true;

        /// <summary>
        /// Luac模式
        /// 是否以Luac模式运行
        /// </summary>
        [SerializeField]
        private bool m_LuacMode = false;

        /// <summary>
        /// Lua热重载
        /// 是否启用Lua热重载
        /// </summary>
        [SerializeField]
        private bool m_LuaHotReloadMode = false;

        /// <summary>
        /// 运行帧率
        /// 游戏运行时每秒的最高帧数
        /// </summary>
        [SerializeField]
        private int m_FrameRate = 60;

        /// <summary>
        /// 运行速率
        /// 游戏运行时的最快速率
        /// </summary>
        [SerializeField]
        private float m_GameSpeed = 1f;

        /// <summary>
        /// 启用后台运行
        /// 游戏挂起时系统后台是否继续运行游戏
        /// </summary>
        [SerializeField]
        private bool m_RunInBackground = true;

        /// <summary>
        /// 启用屏幕常亮
        /// 游戏运行一段时间后屏幕是否常亮以避免设备进入锁定状态
        /// </summary>
        [SerializeField]
        private bool m_NeverSleep = true;

        /// <summary>
        /// 自定义设备分级/评级
        /// </summary>
        [SerializeField]
        private bool m_CustomDevicePerformance = false;

        /// <summary>
        /// 开启设备分级/评级
        /// </summary>
        [SerializeField]
        private bool m_UseDevicePerformance = false;

        /// <summary>
        /// 编辑器性能配置
        /// </summary>
        [SerializeField]
        private DevicePerformanceData m_EditorPerformance = new DevicePerformanceData(2, 4000, 2000, 8000, 4000);

        /// <summary>
        /// iOS 性能配置
        /// </summary>
        [SerializeField]
        private DevicePerformanceData m_iOSPerformance = new DevicePerformanceData(2, 4000, 2000, 8000, 4000);

        /// <summary>
        /// Android 性能配置
        /// </summary>
        [SerializeField]
        private DevicePerformanceData m_AndroidPerformance = new DevicePerformanceData(4, 6000, 2000, 8000, 4000);

        /// <summary>
        /// 调试界面皮肤
        /// </summary>
        [SerializeField]
        private GUISkin debugSkin = null;
        #endregion

        //=========================================================================
        // 私有字段
        //=========================================================================
        #region Private Fields
        /// <summary>
        /// 游戏缓存速率（用于取消暂停时游戏速率的恢复）
        /// </summary>
        private float m_GameSpeedCache = 1f;
        #endregion

        //=========================================================================
        // 公共属性
        //=========================================================================
        #region Public Properties
        /// <summary>
        /// 游戏缓存速度（只读）
        /// </summary>
        public float GameSpeedCache => m_GameSpeedCache;

        /// <summary>
        /// 获取或设置是否使用编辑器资源模式（仅编辑器内有效）
        /// </summary>
        public bool EditorResourceMode
        {
            get => m_EditorResourceMode;
            set => m_EditorResourceMode = value;
        }

        /// <summary>
        /// 获取或设置编辑器启动语言（仅编辑器内有效）
        /// </summary>
        public GameDefinitions.Language EditorLanguage
        {
            get => m_EditorLanguage;
            set => m_EditorLanguage = value;
        }

        /// <summary>
        /// 获取或设置开发模式
        /// </summary>
        public bool DevelopMode
        {
            get => m_DevelopMode;
            set => m_DevelopMode = value;
        }

        /// <summary>
        /// 获取或设置Luac模式
        /// </summary>
        public bool LuacMode
        {
            get => m_LuacMode;
            set => m_LuacMode = value;
        }

        /// <summary>
        /// 获取或设置Lua热重载模式
        /// </summary>
        public bool LuaHotReloadMode
        {
            get => m_LuaHotReloadMode;
            set => m_LuaHotReloadMode = value;
        }

        /// <summary>
        /// 获取或设置游戏帧率
        /// </summary>
        public int FrameRate
        {
            get => m_FrameRate;
            set
            {
                m_FrameRate = value;
                Application.targetFrameRate = m_FrameRate;
            }
        }

        /// <summary>
        /// 获取或设置游戏速度
        /// </summary>
        public float GameSpeed
        {
            get => m_GameSpeed;
            set
            {
                m_GameSpeed = value >= 0f ? value : 0f;
                Time.timeScale = m_GameSpeed;
            }
        }

        /// <summary>
        /// 获取游戏是否暂停
        /// </summary>
        public bool IsGamePaused => m_GameSpeed <= 0f;

        /// <summary>
        /// 获取是否正常游戏速度
        /// </summary>
        public bool IsNormalGameSpeed => m_GameSpeed == 1f;

        /// <summary>
        /// 获取或设置是否允许后台运行
        /// </summary>
        public bool RunInBackground
        {
            get => m_RunInBackground;
            set
            {
                m_RunInBackground = value;
                Application.runInBackground = m_RunInBackground;
            }
        }

        /// <summary>
        /// 获取或设置是否禁止休眠
        /// </summary>
        public bool NeverSleep
        {
            get => m_NeverSleep;
            set
            {
                m_NeverSleep = value;
                Screen.sleepTimeout = value ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
            }
        }

        /// <summary>
        /// 判断是否是亚马逊平台
        /// </summary>
        public bool IsAmazonStore
        {
            get
            {
#if AMAZON_ENABLE
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// 自定义设备分级/评级
        /// </summary>
        public bool CustomDevicePerformance
        {
            get => m_CustomDevicePerformance;
            set => m_CustomDevicePerformance = value;
        }

        /// <summary>
        /// 开启设备分级/评级
        /// </summary>
        public bool UseDevicePerformance
        {
            get => m_UseDevicePerformance;
            set => m_UseDevicePerformance = value;
        }

        /// <summary>
        /// 编辑器性能配置
        /// </summary>
        public DevicePerformanceData EditorPerformance
        {
            get => m_EditorPerformance;
            set => m_EditorPerformance = value;
        }

        /// <summary>
        /// iOS 性能配置
        /// </summary>
        public DevicePerformanceData iOSPerformance
        {
            get => m_iOSPerformance;
            set => m_iOSPerformance = value;
        }

        /// <summary>
        /// Android 性能配置
        /// </summary>
        public DevicePerformanceData AndroidPerformance
        {
            get => m_AndroidPerformance;
            set => m_AndroidPerformance = value;
        }
        #endregion
    }
}