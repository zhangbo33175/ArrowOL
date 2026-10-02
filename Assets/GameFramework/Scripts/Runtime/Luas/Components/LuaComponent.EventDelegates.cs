/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaComponent.EventDelegates.cs
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

#if BEST_HTTP_ENABLE

        /// <summary>
        /// Lua 层 WebSocket 建立成功回调全局派发委托
        /// </summary>
        private LuaWebSocketOpenCSEventDelegate m_LuaWebSocketOpenCSEventDelegate;

        /// <summary>
        /// Lua 层 WebSocket 建立成功回调全局派发委托（只读属性）
        /// </summary>
        public LuaWebSocketOpenCSEventDelegate LuaWebSocketOpenCSEventDelegate
        {
            get { return m_LuaWebSocketOpenCSEventDelegate; }
        }

        /// <summary>
        /// Lua 层 WebSocket 文本信息接收回调全局派发委托
        /// </summary>
        private LuaWebSocketMessageReceivedCSEventDelegate m_LuaWebSocketMessageReceivedCSEventDelegate;

        /// <summary>
        /// Lua 层 WebSocket 文本信息接收回调全局派发委托（只读属性）
        /// </summary>
        public LuaWebSocketMessageReceivedCSEventDelegate LuaWebSocketMessageReceivedCSEventDelegate
        {
            get { return m_LuaWebSocketMessageReceivedCSEventDelegate; }
        }

        /// <summary>
        /// Lua 层 WebSocket 字节流信息接收回调全局派发委托
        /// </summary>
        private LuaWebSocketBinaryReceivedCSEventDelegate m_LuaWebSocketBinaryReceivedCSEventDelegate;

        /// <summary>
        /// Lua 层 WebSocket 字节流信息接收回调全局派发委托（只读属性）
        /// </summary>
        public LuaWebSocketBinaryReceivedCSEventDelegate LuaWebSocketBinaryReceivedCSEventDelegate
        {
            get { return m_LuaWebSocketBinaryReceivedCSEventDelegate; }
        }

        /// <summary>
        /// Lua 层 WebSocket 关闭回调全局派发委托
        /// </summary>
        private LuaWebSocketClosedCSEventDelegate m_LuaWebSocketClosedCSEventDelegate;

        /// <summary>
        /// Lua 层 WebSocket 关闭回调全局派发委托（只读属性）
        /// </summary>
        public LuaWebSocketClosedCSEventDelegate LuaWebSocketClosedCSEventDelegate
        {
            get { return m_LuaWebSocketClosedCSEventDelegate; }
        }

        /// <summary>
        /// Lua 层 WebSocket 错误回调全局派发委托
        /// </summary>
        private LuaWebSocketErrorCSEventDelegate m_LuaWebSocketErrorCSEventDelegate;

        /// <summary>
        /// Lua 层 WebSocket 错误回调全局派发委托（只读属性）
        /// </summary>
        public LuaWebSocketErrorCSEventDelegate LuaWebSocketErrorCSEventDelegate
        {
            get { return m_LuaWebSocketErrorCSEventDelegate; }
        }

#endif

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
            if (!TryBindGlobalDelegate("CreateLuaClassFromCS", "LuaCreateLuaClassFromCSEventDelegate 绑定失败", out m_LuaCreateLuaClassFromCSEventDelegate)) return;

            // 本地化表关联
            if (!TryBindGlobalDelegate("Relate_Localization_Table_Data", "LuaRelateLocalizationTableDataFromCSEventDelegate 无效。", out m_LuaRelateLocalizationTableDataFromCSEventDelegate)) return;

            // 创建流程 Lua 类
            if (!TryBindGlobalDelegate("CreatePocedureLuaClassFromCS", "LuaCreateProcedureLuaClassFromCSEventDelegate 绑定失败", out m_LuaCreateProcedureLuaClassFromCSEventDelegate)) return;

            // 应用暂停
            if (!TryBindGlobalDelegate("ApplicationPauseCallback", "LuaApplicationPauseFromCSEventDelegate 无效。", out m_LuaApplicationPauseFromCSEventDelegate)) return;

            // 应用退出
            if (!TryBindGlobalDelegate("ApplicationQuitCallback", "LuaApplicationQuitFromCSEventDelegate 绑定失败", out m_LuaApplicationQuitFromCSEventDelegate)) return;

            // 按键抬起
            if (!TryBindGlobalDelegate("KeyboardsUpCallback", "LuaKeysUpFromCSEventDelegate 绑定失败", out m_LuaKeysUpFromCSEventDelegate)) return;

            // 获取Lua层接收C#事件回调全局派发
            if (!TryBindGlobalDelegate("ReceiveCsEventCallback", "m_LuaReceiveEventCSEventDelegate 无效。", out m_LuaReceiveEventCSEventDelegate)) return;

            // c#获取lua层的ResDefInfo事件全局派发
            if (!TryBindGlobalDelegate("GetResDefInfoCallback", "m_LuaGetResDefInfoEventDelegate 无效。", out m_LuaGetResDefInfoEventDelegate)) return;

#if BEST_HTTP_ENABLE
            // 获取Lua层WebSocket建立成功回调全局派发
            if (!TryBindGlobalDelegate("framework_websocket_open_callback", "LuaWebSocketOpenCSEventDelegate 无效。", out m_LuaWebSocketOpenCSEventDelegate)) return;

            // 获取Lua层WebSocket文本信息接收回调全局派发
            if (!TryBindGlobalDelegate("framework_websocket_message_received_callback", "LuaWebSocketMessageReceivedCSEventDelegate 无效。", out m_LuaWebSocketMessageReceivedCSEventDelegate)) return;

            // 获取Lua层WebSocket字节流信息接收回调全局派发
            if (!TryBindGlobalDelegate("framework_websocket_binary_received_callback", "LuaWebSocketBinaryReceivedCSEventDelegate 无效。", out m_LuaWebSocketBinaryReceivedCSEventDelegate)) return;

            // 获取Lua层WebSocket关闭回调全局派发
            if (!TryBindGlobalDelegate("framework_websocket_closed_callback", "LuaWebSocketClosedCSEventDelegate 无效。", out m_LuaWebSocketClosedCSEventDelegate)) return;

            // 获取Lua层WebSocket错误回调全局派发
            if (!TryBindGlobalDelegate("framework_websocket_error_callback", "LuaWebSocketErrorCSEventDelegate 无效。", out m_LuaWebSocketErrorCSEventDelegate)) return;
#endif
        }

        /// <summary>
        /// 从Lua虚拟机获取全局委托，获取失败时打印致命错误并返回false
        /// </summary>
        /// <typeparam name="T">委托类型</typeparam>
        /// <param name="luaName">Lua全局函数名</param>
        /// <param name="fatalMsg">失败时的致命错误日志</param>
        /// <param name="del">输出绑定到的委托字段</param>
        /// <returns>是否绑定成功</returns>
        private bool TryBindGlobalDelegate<T>(string luaName, string fatalMsg, out T del)
        {
            del = GetGlobalValue<T>(luaName);
            if (del == null)
            {
                Log.Fatal(fatalMsg);
                return false;
            }
            return true;
        }

        #endregion
    }
}