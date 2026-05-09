using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua 核心组件
    /// 负责 C# 与 Lua 全局事件绑定、生命周期派发、跨语言通信
    /// </summary>
    public sealed partial class LuaComponent : GameComponent
    {
        /// <summary>
        /// Lua 创建类回调（C# -> Lua）
        /// </summary>
        private LuaCreateLuaClassFromCSEventDelegate m_LuaCreateLuaClassFromCSEventDelegate;

        public LuaCreateLuaClassFromCSEventDelegate LuaCreateLuaClassFromCSEventDelegate
        {
            get { return m_LuaCreateLuaClassFromCSEventDelegate; }
        }

        /// <summary>
        /// 本地化语言表关联回调
        /// </summary>
        private LuaRelateLocalizationTableDataFromCSEventDelegate m_LuaRelateLocalizationTableDataFromCSEventDelegate;

        public LuaRelateLocalizationTableDataFromCSEventDelegate LuaRelateLocalizationTableDataFromCSEventDelegate
        {
            get { return m_LuaRelateLocalizationTableDataFromCSEventDelegate; }
        }

        /// <summary>
        /// Lua 创建流程类回调
        /// </summary>
        private LuaCreateProcedureLuaClassFromCSEventDelegate m_LuaCreateProcedureLuaClassFromCSEventDelegate;

        public LuaCreateProcedureLuaClassFromCSEventDelegate LuaCreateProcedureLuaClassFromCSEventDelegate
        {
            get { return m_LuaCreateProcedureLuaClassFromCSEventDelegate; }
        }

        /// <summary>
        /// 应用暂停回调
        /// </summary>
        private LuaApplicationPauseFromCSEventDelegate m_LuaApplicationPauseFromCSEventDelegate;

        public LuaApplicationPauseFromCSEventDelegate LuaApplicationPauseFromCSEventDelegate
        {
            get { return m_LuaApplicationPauseFromCSEventDelegate; }
        }

        /// <summary>
        /// 应用退出回调
        /// </summary>
        private LuaApplicationQuitFromCSEventDelegate m_LuaApplicationQuitFromCSEventDelegate;

        public LuaApplicationQuitFromCSEventDelegate LuaApplicationQuitFromCSEventDelegate
        {
            get { return m_LuaApplicationQuitFromCSEventDelegate; }
        }

        /// <summary>
        /// 键盘按键抬起回调
        /// </summary>
  
        private LuaKeysUpFromCSEventDelegate m_LuaKeysUpFromCSEventDelegate;
   
        public LuaKeysUpFromCSEventDelegate LuaKeysUpFromCSEventDelegate
        {
            get { return m_LuaKeysUpFromCSEventDelegate; }
        }


        /// <summary>
        /// Apple 登录回调
        /// </summary>
        private LuaSignInWithAppleCSEventDelegate m_LuaSignInWithAppleCSEventDelegate;

        public LuaSignInWithAppleCSEventDelegate LuaSignInWithAppleCSEventDelegate
        {
            get { return m_LuaSignInWithAppleCSEventDelegate; }
        }

        /// Lua层Apple登陆状态查询全局事件派发
        /// </summary>
        private LuaSignInWithAppleStateCSEventDelegate m_LuaSignInWithAppleStateCSEventDelegate;

        public LuaSignInWithAppleStateCSEventDelegate LuaSignInWithAppleStateCSEventDelegate
        {
            get { return m_LuaSignInWithAppleStateCSEventDelegate; }
        }

        /// <summary>
        /// Google 登录回调
        /// </summary>
        private LuaSignInWithAppleCSEventDelegate m_LuaSignInWithGoogleCSEventDelegate;

        public LuaSignInWithAppleCSEventDelegate LuaSignInWithGoogleCSEventDelegate
        {
            get { return m_LuaSignInWithGoogleCSEventDelegate; }
        }

        /// <summary>
        /// Google 登出回调
        /// </summary>
        private LuaSignInWithAppleCSEventDelegate m_LuaSignOutWithGoogleCSEventDelegate;

        public LuaSignInWithAppleCSEventDelegate LuaSignOutWithGoogleCSEventDelegate
        {
            get { return m_LuaSignOutWithGoogleCSEventDelegate; }
        }

        /// <summary>
        /// Google 当前账号回调
        /// </summary>
        private LuaSignInWithAppleCSEventDelegate m_LuaSignInGoogleAccountCSEventDelegate;

        public LuaSignInWithAppleCSEventDelegate LuaSignInGoogleAccountCSEventDelegate
        {
            get { return m_LuaSignInGoogleAccountCSEventDelegate; }
        }

        /// Lua层接收C#事件回调全局派发
        /// </summary>
        private LuaReceiveEventCSEventDelegate m_LuaReceiveEventCSEventDelegate;
        public LuaReceiveEventCSEventDelegate LuaReceiveEventCSEventDelegate
        {
            get { return m_LuaReceiveEventCSEventDelegate; }
        }

        /// <summary>
        /// 获取资源定义信息
        /// </summary>
        private LuaGetResDefInfoEventDelegate m_LuaGetResDefInfoEventDelegate;

        public LuaGetResDefInfoEventDelegate LuaGetResDefInfoEventDelegate
        {
            get { return m_LuaGetResDefInfoEventDelegate; }
        }

        /// Lua层Localizing-UI本地化全局事件派发
        /// </summary>
        private LuaLocalizingCSEventDelegate m_LuaLocalizingCSEventDelegate;

        public LuaLocalizingCSEventDelegate LuaLocalizingCSEventDelegate
        {
            get { return m_LuaLocalizingCSEventDelegate; }
        }
        
        /// <summary>
        /// 初始化 C# <-> Lua 全局事件绑定
        /// </summary>
        public void InitLuaBindings()
        {
            // 获取Lua层的面向对象Class创建回调
            m_LuaCreateLuaClassFromCSEventDelegate =GetGlobalValue<LuaCreateLuaClassFromCSEventDelegate>("CreateLuaClassFromCS");
            if (m_LuaCreateLuaClassFromCSEventDelegate == null)
            {
                Log.Fatal("LuaCreateLuaClassFromCSEventDelegate 绑定失败");
                return;
            }

            // 本地化表关联
            m_LuaRelateLocalizationTableDataFromCSEventDelegate =GetGlobalValue<LuaRelateLocalizationTableDataFromCSEventDelegate>("Relate_Localization_Table_Data");
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
            m_LuaApplicationPauseFromCSEventDelegate =GetGlobalValue<LuaApplicationPauseFromCSEventDelegate>("ApplicationPauseCallback");
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
            m_LuaReceiveEventCSEventDelegate =GetGlobalValue<LuaReceiveEventCSEventDelegate>("ReceiveCsEventCallback");
            if (m_LuaReceiveEventCSEventDelegate == null)
            {
                Log.Fatal("m_LuaReceiveEventCSEventDelegate 无效。");
                return;
            }


            // c#获取lua层的ResDefInfo事件全局派发
            m_LuaGetResDefInfoEventDelegate =GetGlobalValue<LuaGetResDefInfoEventDelegate>("GetResDefInfoCallback");
            if (m_LuaGetResDefInfoEventDelegate == null)
            {
                Log.Fatal("m_LuaGetResDefInfoEventDelegate 无效。");
                return;
            }
        }
    }
}