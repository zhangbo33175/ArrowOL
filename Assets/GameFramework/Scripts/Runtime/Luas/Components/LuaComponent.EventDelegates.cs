namespace Honor.Runtime
{
    public sealed partial class LuaComponent : GameComponent
    {
        /// <summary>
        /// Lua创建LuaBahaviour到Lua层的面向对象Class全局事件派发
        /// </summary>
        private LuaCreateLuaClassFromCSEventDelegate m_LuaCreateLuaClassFromCSEventDelegate;

        public LuaCreateLuaClassFromCSEventDelegate LuaCreateLuaClassFromCSEventDelegate
        {
            get { return m_LuaCreateLuaClassFromCSEventDelegate; }
        }

        /// <summary>
        /// Lua层本地化语言表数据关联回调全局事件派发
        /// </summary>
        private LuaRelateLocalizationTableDataFromCSEventDelegate m_LuaRelateLocalizationTableDataFromCSEventDelegate;

        public LuaRelateLocalizationTableDataFromCSEventDelegate LuaRelateLocalizationTableDataFromCSEventDelegate
        {
            get { return m_LuaRelateLocalizationTableDataFromCSEventDelegate; }
        }

        /// <summary>
        /// Lua创建Lua层的Procedure面向对象Class全局事件派发
        /// </summary>
        private LuaCreateProcedureLuaClassFromCSEventDelegate m_LuaCreateProcedureLuaClassFromCSEventDelegate;

        public LuaCreateProcedureLuaClassFromCSEventDelegate LuaCreateProcedureLuaClassFromCSEventDelegate
        {
            get { return m_LuaCreateProcedureLuaClassFromCSEventDelegate; }
        }

        /// <summary>
        /// Lua层ApplicationPause回调全局事件派发
        /// </summary>
        private LuaApplicationPauseFromCSEventDelegate m_LuaApplicationPauseFromCSEventDelegate;

        public LuaApplicationPauseFromCSEventDelegate LuaApplicationPauseFromCSEventDelegate
        {
            get { return m_LuaApplicationPauseFromCSEventDelegate; }
        }

        /// <summary>
        /// Lua层ApplicationQuit回调全局事件派发
        /// </summary>
        private LuaApplicationQuitFromCSEventDelegate m_LuaApplicationQuitFromCSEventDelegate;

        public LuaApplicationQuitFromCSEventDelegate LuaApplicationQuitFromCSEventDelegate
        {
            get { return m_LuaApplicationQuitFromCSEventDelegate; }
        }

        /// <summary>
        /// Lua层原生按键弹起回调全局事件派发
        /// </summary>
        private LuaKeysUpFromCSEventDelegate m_LuaKeysUpFromCSEventDelegate;

        public LuaKeysUpFromCSEventDelegate LuaKeysUpFromCSEventDelegate
        {
            get { return m_LuaKeysUpFromCSEventDelegate; }
        }


        /// <summary>
        /// Lua层Apple登陆结果回调全局事件派发
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
        /// Lua层Google登陆结果回调全局事件派发
        /// </summary>
        private LuaSignInWithAppleCSEventDelegate m_LuaSignInWithGoogleCSEventDelegate;

        public LuaSignInWithAppleCSEventDelegate LuaSignInWithGoogleCSEventDelegate
        {
            get { return m_LuaSignInWithGoogleCSEventDelegate; }
        }

        /// <summary>
        /// Lua层Google登出结果回调全局事件派发
        /// </summary>
        private LuaSignInWithAppleCSEventDelegate m_LuaSignOutWithGoogleCSEventDelegate;

        public LuaSignInWithAppleCSEventDelegate LuaSignOutWithGoogleCSEventDelegate
        {
            get { return m_LuaSignOutWithGoogleCSEventDelegate; }
        }

        /// <summary>
        /// Lua层Google当前登录账号数据全局事件派发
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
        /// c#获取lua层的ResDefInfo 通过别名
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
        /// 初始化所有Lua全局事件派发接口
        /// </summary>
        public void InitLuaBindings()
        {
            // 获取Lua层的面向对象Class创建回调
            m_LuaCreateLuaClassFromCSEventDelegate =GetGlobalValue<LuaCreateLuaClassFromCSEventDelegate>("CreateLuaClassFromCS");
            if (m_LuaCreateLuaClassFromCSEventDelegate == null)
            {
                Log.Fatal("LuaCreateLuaClassFromCSEventDelegate 无效。");
                return;
            }

            // Lua层本地化语言表数据关联回调全局事件派发
            m_LuaRelateLocalizationTableDataFromCSEventDelegate =GetGlobalValue<LuaRelateLocalizationTableDataFromCSEventDelegate>("Relate_Localization_Table_Data");
            if (m_LuaRelateLocalizationTableDataFromCSEventDelegate == null)
            {
                Log.Fatal("LuaRelateLocalizationTableDataFromCSEventDelegate 无效。");
                return;
            }

            // 获取Lua层的Procedure面向对象Class创建回调
            m_LuaCreateProcedureLuaClassFromCSEventDelegate =GetGlobalValue<LuaCreateProcedureLuaClassFromCSEventDelegate>("CreatePocedureLuaClassFromCS");
            if (m_LuaCreateProcedureLuaClassFromCSEventDelegate == null)
            {
                Log.Fatal("LuaCreateProcedureLuaClassFromCSEventDelegate 无效。");
                return;
            }

            // 获取Lua层ApplicationPause回调
            m_LuaApplicationPauseFromCSEventDelegate =GetGlobalValue<LuaApplicationPauseFromCSEventDelegate>("ApplicationPauseCallback");
            if (m_LuaApplicationPauseFromCSEventDelegate == null)
            {
                Log.Fatal("LuaApplicationPauseFromCSEventDelegate 无效。");
                return;
            }

            // 获取Lua层ApplicationQuit回调
            m_LuaApplicationQuitFromCSEventDelegate =GetGlobalValue<LuaApplicationQuitFromCSEventDelegate>("ApplicationQuitCallback");
            if (m_LuaApplicationQuitFromCSEventDelegate == null)
            {
                Log.Fatal("LuaApplicationQuitFromCSEventDelegate 无效。");
                return;
            }

            // 获取Lua层原生按键抬起回调
            m_LuaKeysUpFromCSEventDelegate =GetGlobalValue<LuaKeysUpFromCSEventDelegate>("KeyboardsUpCallback");
            if (m_LuaKeysUpFromCSEventDelegate == null)
            {
                Log.Fatal("LuaKeysUpFromCSEventDelegate 无效。");
                return;
            }
            
            // // 获取Lua层Localizing-UI本地化全局事件派发
            // m_LuaLocalizingCSEventDelegate =GetGlobalValue<LuaLocalizingCSEventDelegate>("Framework_Ui_Localizing_Callback");
            // if (m_LuaLocalizingCSEventDelegate == null)
            // {
            //     Log.Fatal("m_LuaLocalizingCSEventDelegate 无效。");
            //     return;
            // }


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