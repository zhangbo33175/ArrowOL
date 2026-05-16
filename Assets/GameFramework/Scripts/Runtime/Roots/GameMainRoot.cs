/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  GameMainRoot.cs
 * author:  云毅
 * created:
 * descrip:   游戏全局根管理器 - 唯一核心入口，统一管理所有核心系统组件
 ***************************************************************/

using System.Globalization;
using System.Threading;
using GameLib;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏全局根管理器（游戏唯一核心入口）
    /// 功能：统一挂载、获取、管理所有核心系统组件
    /// 所有系统都通过 GameMainRoot.XXX 访问
    /// </summary>
    public partial class GameMainRoot : MonoBehaviour
    {
        //=========================================================================

        #region 静态系统组件（全局访问点）

        //=========================================================================

        /// <summary>
        /// 启动器组件（游戏初始化、运行模式）
        /// </summary>
        public static LauncherComponent Launcher { get; private set; }

        /// <summary>
        /// 资源管理组件（AB包、资源加载/卸载）
        /// </summary>
        public static AssetComponent Asset { get; private set; }

        /// <summary>
        /// Lua热更组件（Lua环境、脚本、绑定）
        /// </summary>
        public static LuaComponent Lua { get; private set; }

        /// <summary>
        /// 网络组件（socket、http、消息收发）
        /// </summary>
        public static NetworkComponent Network { get; private set; }

        /// <summary>
        /// 全局事件组件（发送/监听全局事件）
        /// </summary>
        public static EventComponent Event { get; private set; }

        /// <summary>
        /// 配置管理组件（游戏配置读取）
        /// </summary>
        public static ConfigComponent Config { get; private set; }

        /// <summary>
        /// 数据表组件（Excel/Json表格读取）
        /// </summary>
        public static TableComponent Table { get; private set; }

        /// <summary>
        /// 持久化存储组件（存档、PlayerPrefs、文件）
        /// </summary>
        public static PersistComponent Persist { get; private set; }

        /// <summary>
        /// 多语言本地化组件
        /// </summary>
        public static LocalizationComponent Localization { get; private set; }

        /// <summary>
        /// UI管理组件（界面显示/隐藏/层级）
        /// </summary>
        public static UIComponent UI { get; private set; }

        /// <summary>
        /// 流程状态机组件（游戏流程切换、启动/热更/预加载/游戏）
        /// </summary>
        public static ProcedureComponent Procedure { get; private set; }

        /// <summary>
        /// 触摸输入组件（点击、手势、输入屏蔽）
        /// </summary>
        public static TouchComponent Touch { get; private set; }

        /// <summary>
        /// 场景管理组件（场景加载/卸载/对象管理）
        /// </summary>
        public static SceneComponent Scene { get; private set; }

        /// <summary>
        /// 音频组件（背景音乐、音效）
        /// </summary>
        public static SoundComponent Sound { get; private set; }

        /// <summary>
        /// 游戏核心玩法组件
        /// </summary>
        public static PlayingComponent Playing { get; private set; }

        /// <summary>
        /// 震动反馈组件
        /// </summary>
        public static VibrateComponent Vibrate { get; private set; }

        /// <summary>
        /// 游戏核心单例（第三方/通用管理器）
        /// </summary>
        public static GameManager gameManager { get; private set; }

        #endregion

        //=========================================================================

        #region 生命周期

        //=========================================================================

        /// <summary>
        /// Awake：初始化文化信息（防止数字、格式解析异常）
        /// </summary>
        private void Awake()
        {
            // 设置全局文化信息为标准不变格式，避免多语言环境导致字符串/数字解析错误
            SetCultureInfo(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Start：获取并绑定所有核心系统组件
        /// </summary>
        private void Start()
        {
            gameManager = GameManager.Instance;

            // 从全局组件组中获取所有系统实例
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

        #endregion

        //=========================================================================

        #region 文化信息初始化

        //=========================================================================

        /// <summary>
        /// 设置全局文化信息
        /// 解决不同国家语言环境下，日期、数字、字符串格式不统一导致的BUG
        /// </summary>
        private void SetCultureInfo(CultureInfo cultureInfo)
        {
            try
            {
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
            catch (GameException e)
            {
                Log.Error(e);
            }
        }

        #endregion
    }
}