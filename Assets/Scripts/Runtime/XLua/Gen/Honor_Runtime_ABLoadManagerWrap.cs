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
    public class HonorRuntimeABLoadManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.ABLoadManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 9, 5, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadManifest", _m_LoadManifest);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadSync", _m_LoadSync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAsync", _m_LoadAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Unload", _m_Unload);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsABExist", _m_IsABExist);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsABExistInPersistentDataPath", _m_IsABExistInPersistentDataPath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetABFormatPath", _m_GetABFormatPath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetABRestoredPath", _m_GetABRestoredPath);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "DependsDataList", _g_get_DependsDataList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ReadyABList", _g_get_ReadyABList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadingABList", _g_get_LoadingABList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadedABList", _g_get_LoadedABList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnloadABList", _g_get_UnloadABList);
            
			
			
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
					
					Honor.Runtime.ABLoadManager gen_ret = new Honor.Runtime.ABLoadManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.ABLoadManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ABLoadManager gen_to_be_invoked = (Honor.Runtime.ABLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadManifest(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ABLoadManager gen_to_be_invoked = (Honor.Runtime.ABLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LoadManifest(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ABLoadManager gen_to_be_invoked = (Honor.Runtime.ABLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    
                        UnityEngine.AssetBundle gen_ret = gen_to_be_invoked.LoadSync( _abPath );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ABLoadManager gen_to_be_invoked = (Honor.Runtime.ABLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    Honor.Runtime.AssetBundleLoadOverCallBack _abLoadOverCallback = translator.GetDelegate<Honor.Runtime.AssetBundleLoadOverCallBack>(L, 3);
                    
                    gen_to_be_invoked.LoadAsync( _abPath, _abLoadOverCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unload(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ABLoadManager gen_to_be_invoked = (Honor.Runtime.ABLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.Unload( _abPath );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsABExist(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ABLoadManager gen_to_be_invoked = (Honor.Runtime.ABLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.IsABExist( _abPath );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsABExistInPersistentDataPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ABLoadManager gen_to_be_invoked = (Honor.Runtime.ABLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.IsABExistInPersistentDataPath( _abPath );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetABFormatPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ABLoadManager gen_to_be_invoked = (Honor.Runtime.ABLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    
                        string gen_ret = gen_to_be_invoked.GetABFormatPath( _abPath );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetABRestoredPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ABLoadManager gen_to_be_invoked = (Honor.Runtime.ABLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abFormatPath = LuaAPI.lua_tostring(L, 2);
                    
                        string gen_ret = gen_to_be_invoked.GetABRestoredPath( _abFormatPath );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DependsDataList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ABLoadManager gen_to_be_invoked = (Honor.Runtime.ABLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.DependsDataList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ReadyABList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ABLoadManager gen_to_be_invoked = (Honor.Runtime.ABLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ReadyABList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadingABList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ABLoadManager gen_to_be_invoked = (Honor.Runtime.ABLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LoadingABList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadedABList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ABLoadManager gen_to_be_invoked = (Honor.Runtime.ABLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LoadedABList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnloadABList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ABLoadManager gen_to_be_invoked = (Honor.Runtime.ABLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UnloadABList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
