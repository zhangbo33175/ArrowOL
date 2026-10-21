#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class HonorRuntimeHonorUIManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.HonorUIManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 25, 28, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Dispose", _m_Dispose);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DriveUpdate", _m_DriveUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenUIAsyncByInfo", _m_OpenUIAsyncByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseUIByInfo", _m_CloseUIByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadSceneAsync", _m_LoadSceneAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadScene", _m_UnloadScene);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseAllUIs", _m_CloseAllUIs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DestroyAllSceneGOs", _m_DestroyAllSceneGOs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenUIAsyncByLuaTable", _m_OpenUIAsyncByLuaTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddUISyncByLuaTable", _m_AddUISyncByLuaTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseUIByLuaTable", _m_CloseUIByLuaTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseUIByGO", _m_CloseUIByGO);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddToUnloadUIList", _m_AddToUnloadUIList);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FadeOutLoadingViewThenClose", _m_FadeOutLoadingViewThenClose);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddUIMaskLayerRef", _m_AddUIMaskLayerRef);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SubUIMaskLayerRef", _m_SubUIMaskLayerRef);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartCoroutine", _m_StartCoroutine);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopCoroutine", _m_StopCoroutine);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopAllCoroutines", _m_StopAllCoroutines);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadFonts", _m_LoadFonts);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadFonts", _m_UnloadFonts);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RefreshFontsForUI", _m_RefreshFontsForUI);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenUISyncByLuaTable", _m_OpenUISyncByLuaTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddUIAsyncByLuaTable", _m_AddUIAsyncByLuaTable);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Launcher", _g_get_Launcher);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UI", _g_get_UI);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Procedure", _g_get_Procedure);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Scene", _g_get_Scene);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Debugger", _g_get_Debugger);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsLauncherReady", _g_get_IsLauncherReady);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsUIReady", _g_get_IsUIReady);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsProcedureReady", _g_get_IsProcedureReady);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsSceneReady", _g_get_IsSceneReady);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsDebuggerReady", _g_get_IsDebuggerReady);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsReady", _g_get_IsReady);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SceneCameras", _g_get_SceneCameras);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ScreenUICameras", _g_get_ScreenUICameras);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ScreenUICanvas", _g_get_ScreenUICanvas);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ScreenDesignedResolution", _g_get_ScreenDesignedResolution);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ButtonInteractDuration", _g_get_ButtonInteractDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SceneRootGO", _g_get_SceneRootGO);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EditorPerformance", _g_get_EditorPerformance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "iOSPerformance", _g_get_iOSPerformance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AndroidPerformance", _g_get_AndroidPerformance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsAmazonStore", _g_get_IsAmazonStore);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DevelopMode", _g_get_DevelopMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsLocalServer", _g_get_IsLocalServer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EditorResourceMode", _g_get_EditorResourceMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EditorLanguage", _g_get_EditorLanguage);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuacMode", _g_get_LuacMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaHotReloadMode", _g_get_LuaHotReloadMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedure", _g_get_CurrentProcedure);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Honor.Runtime.HonorUIManager gen_ret = new Honor.Runtime.HonorUIManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorUIManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Dispose(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Dispose(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DriveUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.DriveUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenUIAsyncByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    
                    gen_to_be_invoked.OpenUIAsyncByInfo( _uiInfo );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUIByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.UIInfo>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CloseUIByInfo( _uiInfo, _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.UIInfo>(L, 2)) 
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    
                    gen_to_be_invoked.CloseUIByInfo( _uiInfo );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorUIManager.CloseUIByInfo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSceneAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<Honor.Runtime.SceneLoadOverCallback>(L, 4)) 
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    Honor.Runtime.SceneLoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.SceneLoadOverCallback>(L, 4);
                    
                    gen_to_be_invoked.LoadSceneAsync( _abPath, _assetName, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.LoadSceneAsync( _abPath, _assetName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorUIManager.LoadSceneAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadScene(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<Honor.Runtime.SceneUnloadOverCallback>(L, 3)) 
                {
                    string _sceneName = LuaAPI.lua_tostring(L, 2);
                    Honor.Runtime.SceneUnloadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.SceneUnloadOverCallback>(L, 3);
                    
                    gen_to_be_invoked.UnloadScene( _sceneName, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _sceneName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.UnloadScene( _sceneName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorUIManager.UnloadScene!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseAllUIs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.UIType>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    Honor.Runtime.UIType _uiType;translator.Get(L, 2, out _uiType);
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CloseAllUIs( _uiType, _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.UIType>(L, 2)) 
                {
                    Honor.Runtime.UIType _uiType;translator.Get(L, 2, out _uiType);
                    
                    gen_to_be_invoked.CloseAllUIs( _uiType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorUIManager.CloseAllUIs!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DestroyAllSceneGOs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 2)) 
                {
                    UnityEngine.GameObject _root = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.DestroyAllSceneGOs( _root );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.DestroyAllSceneGOs(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorUIManager.DestroyAllSceneGOs!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenUIAsyncByLuaTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& translator.Assignable<Honor.Runtime.UILoadOverCallback>(L, 4)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    Honor.Runtime.UILoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.UILoadOverCallback>(L, 4);
                    
                    gen_to_be_invoked.OpenUIAsyncByLuaTable( _luaTable, _luaParams, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.OpenUIAsyncByLuaTable( _luaTable, _luaParams );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.OpenUIAsyncByLuaTable( _luaTable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorUIManager.OpenUIAsyncByLuaTable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddUISyncByLuaTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& translator.Assignable<UnityEngine.Transform>(L, 3)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.AddUISyncByLuaTable( _luaTable, _parent, _luaParams );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& translator.Assignable<UnityEngine.Transform>(L, 3)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.AddUISyncByLuaTable( _luaTable, _parent );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorUIManager.AddUISyncByLuaTable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUIByLuaTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CloseUIByLuaTable( _luaTable, _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.CloseUIByLuaTable( _luaTable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorUIManager.CloseUIByLuaTable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUIByGO(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.GameObject>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.GameObject _uiGO = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CloseUIByGO( _uiGO, _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 2)) 
                {
                    UnityEngine.GameObject _uiGO = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.CloseUIByGO( _uiGO );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorUIManager.CloseUIByGO!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddToUnloadUIList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.AddToUnloadUIList( _go );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FadeOutLoadingViewThenClose(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.FadeOutLoadingViewThenClose(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddUIMaskLayerRef(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.AddUIMaskLayerRef( _name );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SubUIMaskLayerRef(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.SubUIMaskLayerRef(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartCoroutine(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.IEnumerator _routine = (System.Collections.IEnumerator)translator.GetObject(L, 2, typeof(System.Collections.IEnumerator));
                    
                        UnityEngine.Coroutine gen_ret = gen_to_be_invoked.StartCoroutine( _routine );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopCoroutine(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Coroutine _routine = (UnityEngine.Coroutine)translator.GetObject(L, 2, typeof(UnityEngine.Coroutine));
                    
                    gen_to_be_invoked.StopCoroutine( _routine );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopAllCoroutines(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StopAllCoroutines(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadFonts(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LoadFonts(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadFonts(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _rightNow = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.UnloadFonts( _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.UnloadFonts(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorUIManager.UnloadFonts!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshFontsForUI(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 2)) 
                {
                    UnityEngine.GameObject _ui = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.RefreshFontsForUI( _ui );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.RefreshFontsForUI(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorUIManager.RefreshFontsForUI!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenUISyncByLuaTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.OpenUISyncByLuaTable( _luaTable, _luaParams );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.OpenUISyncByLuaTable( _luaTable );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorUIManager.OpenUISyncByLuaTable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddUIAsyncByLuaTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& translator.Assignable<UnityEngine.Transform>(L, 3)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TTABLE)&& translator.Assignable<Honor.Runtime.UILoadOverCallback>(L, 5)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    Honor.Runtime.UILoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.UILoadOverCallback>(L, 5);
                    
                    gen_to_be_invoked.AddUIAsyncByLuaTable( _luaTable, _parent, _luaParams, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& translator.Assignable<UnityEngine.Transform>(L, 3)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.AddUIAsyncByLuaTable( _luaTable, _parent, _luaParams );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& translator.Assignable<UnityEngine.Transform>(L, 3)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    
                    gen_to_be_invoked.AddUIAsyncByLuaTable( _luaTable, _parent );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorUIManager.AddUIAsyncByLuaTable!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Launcher(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Launcher);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UI(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UI);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Procedure(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Procedure);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Scene(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Scene);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Debugger(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Debugger);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsLauncherReady(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsLauncherReady);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsUIReady(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsUIReady);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsProcedureReady(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsProcedureReady);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsSceneReady(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsSceneReady);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsDebuggerReady(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsDebuggerReady);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsReady(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsReady);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SceneCameras(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SceneCameras);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ScreenUICameras(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ScreenUICameras);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ScreenUICanvas(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ScreenUICanvas);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ScreenDesignedResolution(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.ScreenDesignedResolution);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ButtonInteractDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.ButtonInteractDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SceneRootGO(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SceneRootGO);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EditorPerformance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.EditorPerformance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_iOSPerformance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.iOSPerformance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AndroidPerformance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AndroidPerformance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsAmazonStore(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsAmazonStore);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DevelopMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.DevelopMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsLocalServer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsLocalServer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EditorResourceMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.EditorResourceMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EditorLanguage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.PushHonorRuntimeGameDefinitionsLanguage(L, gen_to_be_invoked.EditorLanguage);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuacMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.LuacMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaHotReloadMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.LuaHotReloadMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedure(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorUIManager gen_to_be_invoked = (Honor.Runtime.HonorUIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.CurrentProcedure);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
