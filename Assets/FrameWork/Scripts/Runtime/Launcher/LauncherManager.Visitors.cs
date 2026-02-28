using GameLib;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class LauncherManager
    {
        public float GameSpeedCache
        {
            get { return GameConfig.instance.m_GameSpeedCache; }
        }

        /// <summary>
        /// 获取或设置是否使用编辑器资源模式（仅编辑器内有效）。
        /// </summary>
        public bool EditorResourceMode
        {
            get { return GameConfig.instance.m_EditorResourceMode; }
            set { GameConfig.instance.m_EditorResourceMode = value; }
        }

        /// <summary>
        /// 获取或设置编辑器启动语言（仅编辑器内有效）。
        /// </summary>
        public GameDefinitions.Language EditorLanguage
        {
            get { return GameConfig.instance.m_EditorLanguage; }
            set { GameConfig.instance.m_EditorLanguage = value; }
        }

        /// <summary>
        /// 获取或设置开发模式。
        /// </summary>
        public bool DevelopMode
        {
            get { return GameConfig.instance.m_DevelopMode; }
            set { GameConfig.instance.m_DevelopMode = value; }
        }

        /// <summary>
        /// 获取或设置Luac模式。
        /// </summary>
        public bool LuacMode
        {
            get { return GameConfig.instance.m_LuacMode; }
            set { GameConfig.instance.m_LuacMode = value; }
        }

        /// <summary>
        /// 获取或设置Lua热重载模式。
        /// </summary>
        public bool LuaHotReloadMode
        {
            get { return GameConfig.instance.m_LuaHotReloadMode; }
            set { GameConfig.instance.m_LuaHotReloadMode = value; }
        }

        /// <summary>
        /// 获取或设置游戏帧率
        /// </summary>
        public int FrameRate
        {
            get { return GameConfig.instance.m_FrameRate; }
            set { Application.targetFrameRate = GameConfig.instance.m_FrameRate = value; }
        }

        /*
        /// <summary>
        /// 获取当前帧率
        /// </summary>
        public string CurFrameRate
        {
            get
            {
                return m_DebuggerComponent.CurrentFPS;
            }
        }
        */

        /// <summary>
        /// 获取或设置游戏速度
        /// </summary>
        public float GameSpeed
        {
            get { return GameConfig.instance.m_GameSpeed; }
            set { Time.timeScale = GameConfig.instance.m_GameSpeed = value >= 0f ? value : 0f; }
        }

        /// <summary>
        /// 获取游戏是否暂停
        /// </summary>
        public bool IsGamePaused
        {
            get { return GameConfig.instance.m_GameSpeed <= 0f; }
        }

        /// <summary>
        /// 获取是否正常游戏速度
        /// </summary>
        public bool IsNormalGameSpeed
        {
            get { return GameConfig.instance.m_GameSpeed == 1f; }
        }

        /// <summary>
        /// 获取或设置是否允许后台运行
        /// </summary>
        public bool RunInBackground
        {
            get { return GameConfig.instance.m_RunInBackground; }
            set { Application.runInBackground = GameConfig.instance.m_RunInBackground = value; }
        }

        /// <summary>
        /// 获取或设置是否禁止休眠
        /// </summary>
        public bool NeverSleep
        {
            get { return GameConfig.instance.m_NeverSleep; }
            set
            {
                GameConfig.instance.m_NeverSleep = value;
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


        public bool CustomDevicePerformance
        {
            set { GameConfig.instance.m_CustomDevicePerformance = value; }
            get { return GameConfig.instance.m_CustomDevicePerformance; }
        }


        public bool UseDevicePerformance
        {
            set { GameConfig.instance.m_UseDevicePerformance = value; }
            get { return GameConfig.instance.m_UseDevicePerformance; }
        }

        /*
        [SerializeField]
        private DevicePerformanceData m_EditorPerformance = new DevicePerformanceData(2, 4000, 2000, 8000, 4000);
        public DevicePerformanceData EditorPerformance
        {
            set
            {
                m_EditorPerformance = value;
            }
            get
            {
                return m_EditorPerformance;
            }
        }

        [SerializeField]
        private DevicePerformanceData m_iOSPerformance = new DevicePerformanceData(2, 4000, 2000, 8000, 4000);
        public DevicePerformanceData iOSPerformance
        {
            set
            {
                m_iOSPerformance = value;
            }
            get
            {
                return m_iOSPerformance;
            }
        }


        [SerializeField]
        private DevicePerformanceData m_AndroidPerformance = new DevicePerformanceData(4, 6000, 2000, 8000, 4000);
        public DevicePerformanceData AndroidPerformance
        {
            set
            {
                m_AndroidPerformance = value;
            }
            get
            {
                return m_AndroidPerformance;
            }
        }
        */

        
        /// <summary>
        /// 是否为本地服务器
        /// </summary>
        public bool IsLocalServer
        {
            get => GameConfig.instance.m_IsLocalServer;
            set => GameConfig.instance.m_IsLocalServer = value;
        }
        
        [SerializeField] private GUISkin debugSkin = null;
    }
}