/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaComponent.Events.cs
 * author:    云毅
 * created:   2026
 * descrip:   Lua 全局事件绑定模块，负责 C# 与 Lua 之间的跨语言通信、生命周期事件派发、全局委托绑定
 ***************************************************************/

using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua 核心组件 - 事件绑定部分
    /// 负责 C# 与 Lua 全局事件绑定、生命周期派发、跨语言通信
    /// </summary>
    public sealed partial class LuaComponent : GameComponent
    {
        //=========================================================================
        #region 全局回调委托定义
        //=========================================================================

        /// <summary>
        /// C# 主动创建 Lua 类的回调委托
        /// </summary>
        private LuaCreateLuaClassFromCSEventDelegate m_LuaCreateLuaClassFromCSEventDelegate;

        /// <summary>
        /// C# 主动创建 Lua 类的回调委托（只读属性）
        /// </summary>
        public LuaCreateLuaClassFromCSEventDelegate LuaCreateLuaClassFromCSEventDelegate
        {
            get { return m_LuaCreateLuaClassFromCSEventDelegate; }
        }

        /// <summary>
        /// 本地化语言表数据关联委托
        /// </summary>
        private LuaRelateLocalizationTableDataFromCSEventDelegate m_LuaRelateLocalizationTableDataFromCSEventDelegate;

        /// <summary>
        /// 本地化语言表数据关联委托（只读属性）
        /// </summary>
        public LuaRelateLocalizationTableDataFromCSEventDelegate LuaRelateLocalizationTableDataFromCSEventDelegate
        {
            get { return m_LuaRelateLocalizationTableDataFromCSEventDelegate; }
        }

        /// <summary>
        /// C# 创建流程层 Lua 类委托
        /// </summary>
        private LuaCreateProcedureLuaClassFromCSEventDelegate m_LuaCreateProcedureLuaClassFromCSEventDelegate;

        /// <summary>
        /// C# 创建流程层 Lua 类委托（只读属性）
        /// </summary>
        public LuaCreateProcedureLuaClassFromCSEventDelegate LuaCreateProcedureLuaClassFromCSEventDelegate
        {
            get { return m_LuaCreateProcedureLuaClassFromCSEventDelegate; }
        }

        /// <summary>
        /// 应用暂停事件委托
        /// </summary>
        private LuaApplicationPauseFromCSEventDelegate m_LuaApplicationPauseFromCSEventDelegate;

        /// <summary>
        /// 应用暂停事件委托（只读属性）
        /// </summary>
        public LuaApplicationPauseFromCSEventDelegate LuaApplicationPauseFromCSEventDelegate
        {
            get { return m_LuaApplicationPauseFromCSEventDelegate; }
        }

        /// <summary>
        /// 应用退出事件委托
        /// </summary>
        private LuaApplicationQuitFromCSEventDelegate m_LuaApplicationQuitFromCSEventDelegate;

        /// <summary>
        /// 应用退出事件委托（只读属性）
        /// </summary>
        public LuaApplicationQuitFromCSEventDelegate LuaApplicationQuitFromCSEventDelegate
        {
            get { return m_LuaApplicationQuitFromCSEventDelegate; }
        }

        /// <summary>
        /// 键盘按键抬起事件委托
        /// </summary>
        private LuaKeysUpFromCSEventDelegate m_LuaKeysUpFromCSEventDelegate;

        /// <summary>
        /// 键盘按键抬起事件委托（只读属性）
        /// </summary>
        public LuaKeysUpFromCSEventDelegate LuaKeysUpFromCSEventDelegate
        {
            get { return m_LuaKeysUpFromCSEventDelegate; }
        }

        /// <summary>
        /// Apple 登录回调委托
        /// </summary>
        private LuaSignInWithAppleCSEventDelegate m_LuaSignInWithAppleCSEventDelegate;

        /// <summary>
        /// Apple 登录回调委托（只读属性）
        /// </summary>
        public LuaSignInWithAppleCSEventDelegate LuaSignInWithAppleCSEventDelegate
        {
            get { return m_LuaSignInWithAppleCSEventDelegate; }
        }

        /// <summary>
        /// Apple 登录状态查询委托
        /// </summary>
        private LuaSignInWithAppleStateCSEventDelegate m_LuaSignInWithAppleStateCSEventDelegate;

        /// <summary>
        /// Apple 登录状态查询委托（只读属性）
        /// </summary>
        public LuaSignInWithAppleStateCSEventDelegate LuaSignInWithAppleStateCSEventDelegate
        {
            get { return m_LuaSignInWithAppleStateCSEventDelegate; }
        }

        /// <summary>
        /// Google 登录委托
        /// </summary>
        private LuaSignInWithAppleCSEventDelegate m_LuaSignInWithGoogleCSEventDelegate;

        /// <summary>
        /// Google 登录委托（只读属性）
        /// </summary>
        public LuaSignInWithAppleCSEventDelegate LuaSignInWithGoogleCSEventDelegate
        {
            get { return m_LuaSignInWithGoogleCSEventDelegate; }
        }

        /// <summary>
        /// Google 登出委托
        /// </summary>
        private LuaSignInWithAppleCSEventDelegate m_LuaSignOutWithGoogleCSEventDelegate;

        /// <summary>
        /// Google 登出委托（只读属性）
        /// </summary>
        public LuaSignInWithAppleCSEventDelegate LuaSignOutWithGoogleCSEventDelegate
        {
            get { return m_LuaSignOutWithGoogleCSEventDelegate; }
        }

        /// <summary>
        /// Google 当前登录账号查询委托
        /// </summary>
        private LuaSignInWithAppleCSEventDelegate m_LuaSignInGoogleAccountCSEventDelegate;

        /// <summary>
        /// Google 当前登录账号查询委托（只读属性）
        /// </summary>
        public LuaSignInWithAppleCSEventDelegate LuaSignInGoogleAccountCSEventDelegate
        {
            get { return m_LuaSignInGoogleAccountCSEventDelegate; }
        }

        /// <summary>
        /// Lua 接收 C# 事件的全局派发委托
        /// </summary>
        private LuaReceiveEventCSEventDelegate m_LuaReceiveEventCSEventDelegate;

        /// <summary>
        /// Lua 接收 C# 事件的全局派发委托（只读属性）
        /// </summary>
        public LuaReceiveEventCSEventDelegate LuaReceiveEventCSEventDelegate
        {
            get { return m_LuaReceiveEventCSEventDelegate; }
        }

        /// <summary>
        /// 获取资源定义信息委托
        /// </summary>
        private LuaGetResDefInfoEventDelegate m_LuaGetResDefInfoEventDelegate;

        /// <summary>
        /// 获取资源定义信息委托（只读属性）
        /// </summary>
        public LuaGetResDefInfoEventDelegate LuaGetResDefInfoEventDelegate
        {
            get { return m_LuaGetResDefInfoEventDelegate; }
        }

        /// <summary>
        /// UI 本地化全局事件派发委托
        /// </summary>
        private LuaLocalizingCSEventDelegate m_LuaLocalizingCSEventDelegate;

        /// <summary>
        /// UI 本地化全局事件派发委托（只读属性）
        /// </summary>
        public LuaLocalizingCSEventDelegate LuaLocalizingCSEventDelegate
        {
            get { return m_LuaLocalizingCSEventDelegate; }
        }

        #endregion

        //=========================================================================
        #region 初始化绑定方法
        //=========================================================================

        /// <summary>
        /// 初始化 C# <-> Lua 全局事件绑定
        /// 从 Lua 虚拟机中获取所有全局委托并缓存，用于跨语言调用
        /// </summary>
        public void InitLuaBindings()
        {
            // 获取Lua层的面向对象Class创建回调
            m_LuaCreateLuaClassFromCSEventDelegate = GetGlobalValue<LuaCreateLuaClassFromCSEventDelegate>("CreateLuaClassFromCS");
            if (m_LuaCreateLuaClassFromCSEventDelegate == null)
            {
                Log.Fatal("LuaCreateLuaClassFromCSEventDelegate 绑定失败");
                return;
            }

            // 本地化表关联
            m_LuaRelateLocalizationTableDataFromCSEventDelegate = GetGlobalValue<LuaRelateLocalizationTableDataFromCSEventDelegate>("Relate_Localization_Table_Data");
            if (m_LuaRelateLocalizationTableDataFromCSEventDelegate == null)
            {
                Log.Fatal("LuaRelateLocalizationTableDataFromCSEventDelegate 无效。");
                return;
            }

            // 创建流程 Lua 类
            m_LuaCreateProcedureLuaClassFromCSEventDelegate = GetGlobalValue<LuaCreateProcedureLuaClassFromCSEventDelegate>("CreatePocedureLuaClassFromCS");
            if (m_LuaCreateProcedureLuaClassFromCSEventDelegate == null)
            {
                Log.Fatal("LuaCreateProcedureLuaClassFromCSEventDelegate 绑定失败");
                return;
            }

            // 应用暂停
            m_LuaApplicationPauseFromCSEventDelegate = GetGlobalValue<LuaApplicationPauseFromCSEventDelegate>("ApplicationPauseCallback");
            if (m_LuaApplicationPauseFromCSEventDelegate == null)
            {
                Log.Fatal("LuaApplicationPauseFromCSEventDelegate 无效。");
                return;
            }

            // 应用退出
            m_LuaApplicationQuitFromCSEventDelegate = GetGlobalValue<LuaApplicationQuitFromCSEventDelegate>("ApplicationQuitCallback");
            if (m_LuaApplicationQuitFromCSEventDelegate == null)
            {
                Log.Fatal("LuaApplicationQuitFromCSEventDelegate 绑定失败");
                return;
            }

            // 按键抬起
            m_LuaKeysUpFromCSEventDelegate = GetGlobalValue<LuaKeysUpFromCSEventDelegate>("KeyboardsUpCallback");
            if (m_LuaKeysUpFromCSEventDelegate == null)
            {
                Log.Fatal("LuaKeysUpFromCSEventDelegate 绑定失败");
                return;
            }

            // 获取Lua层接收C#事件回调全局派发
            m_LuaReceiveEventCSEventDelegate = GetGlobalValue<LuaReceiveEventCSEventDelegate>("ReceiveCsEventCallback");
            if (m_LuaReceiveEventCSEventDelegate == null)
            {
                Log.Fatal("m_LuaReceiveEventCSEventDelegate 无效。");
                return;
            }

            // c#获取lua层的ResDefInfo事件全局派发
            m_LuaGetResDefInfoEventDelegate = GetGlobalValue<LuaGetResDefInfoEventDelegate>("GetResDefInfoCallback");
            if (m_LuaGetResDefInfoEventDelegate == null)
            {
                Log.Fatal("m_LuaGetResDefInfoEventDelegate 无效。");
                return;
            }
        }

        #endregion
    }
}