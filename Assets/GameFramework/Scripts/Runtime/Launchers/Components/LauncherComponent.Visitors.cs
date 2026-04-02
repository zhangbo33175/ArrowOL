using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class LauncherComponent : GameComponent
    {
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
        /// 游戏缓存速率（用于取消暂停时游戏速率的恢复）
        /// </summary>
        private float m_GameSpeedCache = 1f;
        public float GameSpeedCache
        {
            get
            {
                return m_GameSpeedCache;
            }
        }
        
        /// <summary>
        /// 获取或设置是否使用编辑器资源模式（仅编辑器内有效）。
        /// </summary>
        public bool EditorResourceMode
        {
            get
            {
                return m_EditorResourceMode;
            }
            set
            {
                m_EditorResourceMode = value;
            }
        }

        /// <summary>
        /// 获取或设置编辑器启动语言（仅编辑器内有效）。
        /// </summary>
        public GameDefinitions.Language EditorLanguage
        {
            get
            {
                return m_EditorLanguage;
            }
            set
            {
                m_EditorLanguage = value;
            }
        }

        /// <summary>
        /// 获取或设置开发模式。
        /// </summary>
        public bool DevelopMode
        {
            get
            {
                return m_DevelopMode;
            }
            set
            {
                m_DevelopMode = value;
            }
        }

        /// <summary>
        /// 获取或设置Luac模式。
        /// </summary>
        public bool LuacMode
        {
            get
            {
                return m_LuacMode;
            }
            set
            {
                m_LuacMode = value;
            }
        }

        /// <summary>
        /// 获取或设置Lua热重载模式。
        /// </summary>
        public bool LuaHotReloadMode
        {
            get
            {
                return m_LuaHotReloadMode;
            }
            set
            {
                m_LuaHotReloadMode = value;
            }
        }

        /// <summary>
        /// 获取或设置游戏帧率
        /// </summary>
        public int FrameRate
        {
            get
            {
                return m_FrameRate;
            }
            set
            {
                Application.targetFrameRate = m_FrameRate = value;
            }
        }
        
        /// <summary>
        /// 获取或设置游戏速度
        /// </summary>
        public float GameSpeed
        {
            get
            {
                return m_GameSpeed;
            }
            set
            {
                Time.timeScale = m_GameSpeed = value >= 0f ? value : 0f;
            }
        }

        /// <summary>
        /// 获取游戏是否暂停
        /// </summary>
        public bool IsGamePaused
        {
            get
            {
                return m_GameSpeed <= 0f;
            }
        }

        /// <summary>
        /// 获取是否正常游戏速度
        /// </summary>
        public bool IsNormalGameSpeed
        {
            get
            {
                return m_GameSpeed == 1f;
            }
        }

        /// <summary>
        /// 获取或设置是否允许后台运行
        /// </summary>
        public bool RunInBackground
        {
            get
            {
                return m_RunInBackground;
            }
            set
            {
                Application.runInBackground = m_RunInBackground = value;
            }
        }

        /// <summary>
        /// 获取或设置是否禁止休眠
        /// </summary>
        public bool NeverSleep
        {
            get
            {
                return m_NeverSleep;
            }
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
        [SerializeField]
        private bool m_CustomDevicePerformance = false;
        public bool CustomDevicePerformance
        { 
            set 
            { 
                m_CustomDevicePerformance = value; 
            }
            get 
            { 
                return m_CustomDevicePerformance; 
            }
        }

        /// <summary>
        /// 开启设备分级/评级
        /// </summary>
        [SerializeField]
        private bool m_UseDevicePerformance = false;
        public bool UseDevicePerformance
        {
            set
            {
                m_UseDevicePerformance = value;
            }
            get
            {
                return m_UseDevicePerformance;
            }
        }

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

        [SerializeField]
        private GUISkin debugSkin = null;



    }

}


