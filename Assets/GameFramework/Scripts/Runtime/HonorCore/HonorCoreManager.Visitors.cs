/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorCoreManager.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   核心模块门面管理器 - 便捷状态与访问（partial）
 *            提供门面就绪状态、模块能力访问等便捷属性，提高调用可读性
 ***************************************************************/

using XLua;

namespace Honor.Runtime
{
    public sealed partial class HonorCoreManager
    {
        //=========================================================================

        #region 组件引用

        //=========================================================================

        /// <summary>配置管理组件（游戏配置读取）</summary>
        public ConfigComponent Config { get; private set; }

        /// <summary>全局事件组件（发送/监听全局事件）</summary>
        public EventComponent Event { get; private set; }

        /// <summary>Lua热更组件（Lua环境、脚本、绑定）</summary>
        public LuaComponent Lua { get; private set; }

        /// <summary>音频组件（背景音乐、音效）</summary>
        public SoundComponent Sound { get; private set; }

        /// <summary>数据表组件（Excel/Json表格读取）</summary>
        public TableComponent Table { get; private set; }

        /// <summary>计时器组件（驱动所有计时器计时）</summary>
        public TimerComponent Timer { get; private set; }

        /// <summary>震动反馈组件</summary>
        public VibrateComponent Vibrate { get; private set; }

        /// <summary>多语言本地化组件</summary>
        public LocalizationComponent Localization { get; private set; }

        /// <summary>持久化存储组件（存档、PlayerPrefs、文件）</summary>
        public PersistComponent Persist { get; private set; }

        /// <summary>游戏核心玩法组件</summary>
        public PlayingComponent Playing { get; private set; }

        #endregion

        //=========================================================================

        #region 便捷状态与访问

        //=========================================================================

        /// <summary>
        /// 核心基础组件是否全部就绪（配置/事件/Lua 均已完成初始化）
        /// <para>供调用方在启动流程中判断核心门面是否可安全使用</para>
        /// </summary>
        public bool IsCoreReady => Config != null && Event != null && Lua != null;

        /// <summary>
        /// 计时器服务是否就绪
        /// </summary>
        public bool HasTimer => Timer != null;

        /// <summary>
        /// 多语言服务是否就绪
        /// </summary>
        public bool HasLocalization => Localization != null;

        /// <summary>
        /// 持久化服务是否就绪
        /// </summary>
        public bool HasPersist => Persist != null;

        #endregion

        //=========================================================================

        #region 门面收敛 API（跨模块 + Lua）

        //=========================================================================

        /// <summary>
        /// 全局唯一 Lua 运行环境（转发自 LuaComponent）
        /// </summary>
        public LuaEnv LuaEnv => Lua != null ? Lua.Env : null;

        /// <summary>
        /// 当前语言类型（转发自 LocalizationComponent）
        /// </summary>
        public GameDefinitions.Language Language => Localization != null ? Localization.Language : default(GameDefinitions.Language);

        /// <summary>
        /// 当前语言名称字符串（转发自 LocalizationComponent）
        /// </summary>
        public string LanguageName => Localization != null ? Localization.LanguageName : null;

        /// <summary>
        /// 字体自动适配开关状态（转发自 LocalizationComponent）
        /// </summary>
        public bool AutoFontAdapt => Localization != null && Localization.AutoFontAdapt;

        #endregion
    }
}
