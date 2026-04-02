using System.Globalization;
using System.Threading;
using GameLib;
using UnityEngine;

namespace Honor.Runtime
{
    public partial class GameMainRoot : MonoBehaviour
    {
        /// <summary>
        /// Launcher组件
        /// </summary>
        public static LauncherComponent Launcher
        {
            get;
            private set;
        }

        /// <summary>
        /// Asset组件
        /// </summary>
        public static AssetComponent Asset
        {
            get;
            private set;
        }

        /// <summary>
        /// Lua组件
        /// </summary>
        public static LuaComponent Lua
        {
            get;
            private set;
        }

        /// <summary>
        /// Network组件
        /// </summary>
        public static NetworkComponent Network
        {
            get;
            private set;
        }

        /// <summary>
        /// Event组件
        /// </summary>
        public static EventComponent Event
        {
            get;
            private set;
        }

        /// <summary>
        /// Config组件
        /// </summary>
        public static ConfigComponent Config
        {
            get;
            private set;
        }

        /// <summary>
        /// Table组件
        /// </summary>
        public static TableComponent Table
        {
            get;
            private set;
        }

        /// <summary>
        /// Persist组件
        /// </summary>
        public static PersistComponent Persist
        {
            get;
            private set;
        }

        /// <summary>
        /// Localization组件
        /// </summary>
        public static LocalizationComponent Localization
        {
            get;
            private set;
        }

        /// <summary>
        /// UI组件
        /// </summary>
        public static UIComponent UI
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Procedure组件
        /// </summary>
        public static ProcedureComponent Procedure
        {
            get;
            private set;
        }

        /// <summary>
        /// Touch组件
        /// </summary>
        public static TouchComponent Touch
        {
            get;
            private set;
        }

        /// <summary>
        /// Playing组件
        /// </summary>
        public static PlayingComponent Playing
        {
            get;
            private set;
        }

        /// <summary>
        /// Scene组件
        /// </summary>
        public static SceneComponent Scene
        {
            get;
            private set;
        }

        /// <summary>
        /// Sound组件
        /// </summary>
        public static SoundComponent Sound
        {
            get;
            private set;
        }

        /// <summary>
        /// Vibrate组件
        /// </summary>
        public static VibrateComponent Vibrate
        {
            get;
            private set;
        }
        
        private void Awake()
        {
            // 设置全局文化信息参数（避免区域性国家语种字符串数字等信息非标准化转换等问题）
            SetCultureInfo(CultureInfo.InvariantCulture);
        }

        
        /// <summary>
        /// Sound组件
        /// </summary>
        public static GameManager gameManager
        {
            get;
            private set;
        }

        
        private void Start()
        {
            gameManager = GameManager.Instance;
            Launcher = GameComponentsGroup.GetComponent<LauncherComponent>();
            Asset = GameComponentsGroup.GetComponent<AssetComponent>();
            Lua = GameComponentsGroup.GetComponent<LuaComponent>();
            Network = GameComponentsGroup.GetComponent<NetworkComponent>();
            Event = GameComponentsGroup.GetComponent<EventComponent>();
            Config = GameComponentsGroup.GetComponent<ConfigComponent>();
            Table = GameComponentsGroup.GetComponent<TableComponent>();
            Persist = GameComponentsGroup.GetComponent<PersistComponent>();
            Localization = GameComponentsGroup.GetComponent<LocalizationComponent>();
            UI = GameComponentsGroup.GetComponent<UIComponent>();
            Procedure = GameComponentsGroup.GetComponent<ProcedureComponent>();
            Touch = GameComponentsGroup.GetComponent<TouchComponent>();
            Scene = GameComponentsGroup.GetComponent<SceneComponent>();
            Sound = GameComponentsGroup.GetComponent<SoundComponent>();
            Playing = GameComponentsGroup.GetComponent<PlayingComponent>();
            Vibrate = GameComponentsGroup.GetComponent<VibrateComponent>();
        }

        /// <summary>
        /// 设置全局文化信息参数
        /// </summary>
        /// <param name="cultureInfo">文化信息</param>
        private void SetCultureInfo(CultureInfo cultureInfo)
        {
            try
            {
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
            catch(GameException e)
            {
                Log.Error(e);
            }
        }

    }

}


