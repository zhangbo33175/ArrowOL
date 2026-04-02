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
    public class HonorRuntimeLauncherComponentWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.LauncherComponent);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 22, 18);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnApplicationPause", _m_OnApplicationPause);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnApplicationQuit", _m_OnApplicationQuit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PauseGame", _m_PauseGame);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResumeGame", _m_ResumeGame);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetNormalGameSpeed", _m_ResetNormalGameSpeed);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Shutdown", _m_Shutdown);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "GameSpeedCache", _g_get_GameSpeedCache);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EditorResourceMode", _g_get_EditorResourceMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EditorLanguage", _g_get_EditorLanguage);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DevelopMode", _g_get_DevelopMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuacMode", _g_get_LuacMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaHotReloadMode", _g_get_LuaHotReloadMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FrameRate", _g_get_FrameRate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GameSpeed", _g_get_GameSpeed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsGamePaused", _g_get_IsGamePaused);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsNormalGameSpeed", _g_get_IsNormalGameSpeed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RunInBackground", _g_get_RunInBackground);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "NeverSleep", _g_get_NeverSleep);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsAmazonStore", _g_get_IsAmazonStore);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CustomDevicePerformance", _g_get_CustomDevicePerformance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseDevicePerformance", _g_get_UseDevicePerformance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EditorPerformance", _g_get_EditorPerformance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "iOSPerformance", _g_get_iOSPerformance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AndroidPerformance", _g_get_AndroidPerformance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsLocalServer", _g_get_IsLocalServer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsRealTimeDebuggerHotfixForEditor", _g_get_IsRealTimeDebuggerHotfixForEditor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_IsLocalServer", _g_get_m_IsLocalServer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_IsRealTimeDebuggerHotfixForEditor", _g_get_m_IsRealTimeDebuggerHotfixForEditor);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "EditorResourceMode", _s_set_EditorResourceMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EditorLanguage", _s_set_EditorLanguage);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DevelopMode", _s_set_DevelopMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LuacMode", _s_set_LuacMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LuaHotReloadMode", _s_set_LuaHotReloadMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FrameRate", _s_set_FrameRate);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GameSpeed", _s_set_GameSpeed);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RunInBackground", _s_set_RunInBackground);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "NeverSleep", _s_set_NeverSleep);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CustomDevicePerformance", _s_set_CustomDevicePerformance);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseDevicePerformance", _s_set_UseDevicePerformance);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EditorPerformance", _s_set_EditorPerformance);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "iOSPerformance", _s_set_iOSPerformance);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AndroidPerformance", _s_set_AndroidPerformance);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsLocalServer", _s_set_IsLocalServer);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsRealTimeDebuggerHotfixForEditor", _s_set_IsRealTimeDebuggerHotfixForEditor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_IsLocalServer", _s_set_m_IsLocalServer);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_IsRealTimeDebuggerHotfixForEditor", _s_set_m_IsRealTimeDebuggerHotfixForEditor);
            
			
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
					
					Honor.Runtime.LauncherComponent gen_ret = new Honor.Runtime.LauncherComponent();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.LauncherComponent constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnApplicationPause(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _pause = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.OnApplicationPause( _pause );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnApplicationQuit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnApplicationQuit(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PauseGame(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.PauseGame(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResumeGame(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResumeGame(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetNormalGameSpeed(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetNormalGameSpeed(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Shutdown(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Shutdown(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GameSpeedCache(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.GameSpeedCache);
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
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                translator.PushHonorRuntimeGameDefinitionsLanguage(L, gen_to_be_invoked.EditorLanguage);
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
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.DevelopMode);
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
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.LuaHotReloadMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FrameRate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.FrameRate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GameSpeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.GameSpeed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsGamePaused(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsGamePaused);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsNormalGameSpeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsNormalGameSpeed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RunInBackground(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.RunInBackground);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_NeverSleep(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.NeverSleep);
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
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsAmazonStore);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CustomDevicePerformance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CustomDevicePerformance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseDevicePerformance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseDevicePerformance);
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
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AndroidPerformance);
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
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsLocalServer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsRealTimeDebuggerHotfixForEditor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsRealTimeDebuggerHotfixForEditor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_IsLocalServer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.m_IsLocalServer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_IsRealTimeDebuggerHotfixForEditor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.m_IsRealTimeDebuggerHotfixForEditor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EditorResourceMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EditorResourceMode = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EditorLanguage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                Honor.Runtime.GameDefinitions.Language gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.EditorLanguage = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DevelopMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DevelopMode = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuacMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LuacMode = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaHotReloadMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LuaHotReloadMode = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FrameRate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FrameRate = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GameSpeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GameSpeed = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RunInBackground(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RunInBackground = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_NeverSleep(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.NeverSleep = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CustomDevicePerformance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CustomDevicePerformance = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseDevicePerformance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseDevicePerformance = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EditorPerformance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EditorPerformance = (Honor.Runtime.LauncherComponent.DevicePerformanceData)translator.GetObject(L, 2, typeof(Honor.Runtime.LauncherComponent.DevicePerformanceData));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_iOSPerformance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.iOSPerformance = (Honor.Runtime.LauncherComponent.DevicePerformanceData)translator.GetObject(L, 2, typeof(Honor.Runtime.LauncherComponent.DevicePerformanceData));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AndroidPerformance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AndroidPerformance = (Honor.Runtime.LauncherComponent.DevicePerformanceData)translator.GetObject(L, 2, typeof(Honor.Runtime.LauncherComponent.DevicePerformanceData));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsLocalServer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsLocalServer = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsRealTimeDebuggerHotfixForEditor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsRealTimeDebuggerHotfixForEditor = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_IsLocalServer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_IsLocalServer = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_IsRealTimeDebuggerHotfixForEditor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent gen_to_be_invoked = (Honor.Runtime.LauncherComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_IsRealTimeDebuggerHotfixForEditor = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
