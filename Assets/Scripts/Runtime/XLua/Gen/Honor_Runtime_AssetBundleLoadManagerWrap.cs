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
    public class HonorRuntimeAssetBundleLoadManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.AssetBundleLoadManager);
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
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ReadyAssetBundleList", _g_get_ReadyAssetBundleList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadingAssetBundleList", _g_get_LoadingAssetBundleList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadedAssetBundleList", _g_get_LoadedAssetBundleList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnloadAssetBundleList", _g_get_UnloadAssetBundleList);
            
			
			
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
					
					Honor.Runtime.AssetBundleLoadManager gen_ret = new Honor.Runtime.AssetBundleLoadManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetBundleLoadManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetBundleLoadManager gen_to_be_invoked = (Honor.Runtime.AssetBundleLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.AssetBundleLoadManager gen_to_be_invoked = (Honor.Runtime.AssetBundleLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.AssetBundleLoadManager gen_to_be_invoked = (Honor.Runtime.AssetBundleLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.AssetBundleLoadManager gen_to_be_invoked = (Honor.Runtime.AssetBundleLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.AssetBundleLoadManager gen_to_be_invoked = (Honor.Runtime.AssetBundleLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.AssetBundleLoadManager gen_to_be_invoked = (Honor.Runtime.AssetBundleLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.AssetBundleLoadManager gen_to_be_invoked = (Honor.Runtime.AssetBundleLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.AssetBundleLoadManager gen_to_be_invoked = (Honor.Runtime.AssetBundleLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.AssetBundleLoadManager gen_to_be_invoked = (Honor.Runtime.AssetBundleLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
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
			
                Honor.Runtime.AssetBundleLoadManager gen_to_be_invoked = (Honor.Runtime.AssetBundleLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.DependsDataList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ReadyAssetBundleList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleLoadManager gen_to_be_invoked = (Honor.Runtime.AssetBundleLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ReadyAssetBundleList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadingAssetBundleList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleLoadManager gen_to_be_invoked = (Honor.Runtime.AssetBundleLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LoadingAssetBundleList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadedAssetBundleList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleLoadManager gen_to_be_invoked = (Honor.Runtime.AssetBundleLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LoadedAssetBundleList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnloadAssetBundleList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleLoadManager gen_to_be_invoked = (Honor.Runtime.AssetBundleLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UnloadAssetBundleList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
