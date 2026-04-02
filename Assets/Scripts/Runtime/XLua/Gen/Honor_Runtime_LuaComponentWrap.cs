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
    public class HonorRuntimeLuaComponentWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.LuaComponent);
			Utils.BeginObjectRegister(type, L, translator, 0, 8, 18, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitLuaConfigs", _m_InitLuaConfigs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitLuaEnv", _m_InitLuaEnv);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CustomLuaLoader", _m_CustomLuaLoader);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetLua", _m_ResetLua);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitLuaBindings", _m_InitLuaBindings);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CanABLoad", _m_CanABLoad);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitGameLuaBindings", _m_InitGameLuaBindings);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaCreateLuaClassFromCSEventDelegate", _g_get_LuaCreateLuaClassFromCSEventDelegate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaRelateLocalizationTableDataFromCSEventDelegate", _g_get_LuaRelateLocalizationTableDataFromCSEventDelegate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaCreateProcedureLuaClassFromCSEventDelegate", _g_get_LuaCreateProcedureLuaClassFromCSEventDelegate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaApplicationPauseFromCSEventDelegate", _g_get_LuaApplicationPauseFromCSEventDelegate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaApplicationQuitFromCSEventDelegate", _g_get_LuaApplicationQuitFromCSEventDelegate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaKeysUpFromCSEventDelegate", _g_get_LuaKeysUpFromCSEventDelegate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaSignInWithAppleCSEventDelegate", _g_get_LuaSignInWithAppleCSEventDelegate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaSignInWithAppleStateCSEventDelegate", _g_get_LuaSignInWithAppleStateCSEventDelegate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaSignInWithGoogleCSEventDelegate", _g_get_LuaSignInWithGoogleCSEventDelegate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaSignOutWithGoogleCSEventDelegate", _g_get_LuaSignOutWithGoogleCSEventDelegate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaSignInGoogleAccountCSEventDelegate", _g_get_LuaSignInGoogleAccountCSEventDelegate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaReceiveEventCSEventDelegate", _g_get_LuaReceiveEventCSEventDelegate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaGetResDefInfoEventDelegate", _g_get_LuaGetResDefInfoEventDelegate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaLocalizingCSEventDelegate", _g_get_LuaLocalizingCSEventDelegate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaRuntimeProfilerMode", _g_get_LuaRuntimeProfilerMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadedLuaScriptsNames", _g_get_LoadedLuaScriptsNames);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Env", _g_get_Env);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaSoundCSEventDelegate", _g_get_LuaSoundCSEventDelegate);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 1);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "s_LuacEncrytionKey", _g_get_s_LuacEncrytionKey);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "s_LuacEncrytionKey", _s_set_s_LuacEncrytionKey);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Honor.Runtime.LuaComponent gen_ret = new Honor.Runtime.LuaComponent();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.LuaComponent constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitLuaConfigs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitLuaConfigs(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitLuaEnv(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitLuaEnv(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CustomLuaLoader(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _fileName = LuaAPI.lua_tostring(L, 2);
                    
                        byte[] gen_ret = gen_to_be_invoked.CustomLuaLoader( ref _fileName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    LuaAPI.lua_pushstring(L, _fileName);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetLua(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetLua(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitLuaBindings(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitLuaBindings(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CanABLoad(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.CanABLoad( _abPath );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitGameLuaBindings(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitGameLuaBindings(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaCreateLuaClassFromCSEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaCreateLuaClassFromCSEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaRelateLocalizationTableDataFromCSEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaRelateLocalizationTableDataFromCSEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaCreateProcedureLuaClassFromCSEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaCreateProcedureLuaClassFromCSEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaApplicationPauseFromCSEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaApplicationPauseFromCSEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaApplicationQuitFromCSEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaApplicationQuitFromCSEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaKeysUpFromCSEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaKeysUpFromCSEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaSignInWithAppleCSEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaSignInWithAppleCSEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaSignInWithAppleStateCSEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaSignInWithAppleStateCSEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaSignInWithGoogleCSEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaSignInWithGoogleCSEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaSignOutWithGoogleCSEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaSignOutWithGoogleCSEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaSignInGoogleAccountCSEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaSignInGoogleAccountCSEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaReceiveEventCSEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaReceiveEventCSEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaGetResDefInfoEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaGetResDefInfoEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaLocalizingCSEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaLocalizingCSEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaRuntimeProfilerMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.LuaRuntimeProfilerMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadedLuaScriptsNames(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LoadedLuaScriptsNames);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Env(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Env);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaSoundCSEventDelegate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaComponent gen_to_be_invoked = (Honor.Runtime.LuaComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaSoundCSEventDelegate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_s_LuacEncrytionKey(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Honor.Runtime.LuaComponent.s_LuacEncrytionKey);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_s_LuacEncrytionKey(RealStatePtr L)
        {
		    try {
                
			    Honor.Runtime.LuaComponent.s_LuacEncrytionKey = LuaAPI.lua_tobytes(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
