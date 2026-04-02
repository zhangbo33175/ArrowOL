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
    public class HonorRuntimeSceneManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.SceneManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 11, 4, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PreLoadSceneAsync", _m_PreLoadSceneAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadSceneAsync", _m_LoadSceneAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadSceneSync", _m_LoadSceneSync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadScene", _m_UnloadScene);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SceneIsLoaded", _m_SceneIsLoaded);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLoadedSceneAssetNames", _m_GetLoadedSceneAssetNames);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SceneIsLoading", _m_SceneIsLoading);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLoadingSceneAssetNames", _m_GetLoadingSceneAssetNames);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SceneIsUnloading", _m_SceneIsUnloading);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUnloadingSceneAssetNames", _m_GetUnloadingSceneAssetNames);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasScene", _m_HasScene);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "PreLoadSceneAssetNames", _g_get_PreLoadSceneAssetNames);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadingSceneAssetNames", _g_get_LoadingSceneAssetNames);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadedSceneAssetNames", _g_get_LoadedSceneAssetNames);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnloadingSceneAssetNames", _g_get_UnloadingSceneAssetNames);
            
			
			
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
					
					Honor.Runtime.SceneManager gen_ret = new Honor.Runtime.SceneManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PreLoadSceneAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.PreLoadSceneAsync( _abPath, _assetName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSceneAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
            
            
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneManager.LoadSceneAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSceneSync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    
                        UnityEngine.SceneManagement.Scene gen_ret = gen_to_be_invoked.LoadSceneSync( _abPath, _assetName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadScene(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
            
            
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneManager.UnloadScene!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SceneIsLoaded(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.SceneIsLoaded( _abPath, _assetName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLoadedSceneAssetNames(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                        System.Collections.Generic.List<System.Collections.Generic.List<string>> gen_ret = gen_to_be_invoked.GetLoadedSceneAssetNames(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<System.Collections.Generic.List<System.Collections.Generic.List<string>>>(L, 2)) 
                {
                    System.Collections.Generic.List<System.Collections.Generic.List<string>> _results = (System.Collections.Generic.List<System.Collections.Generic.List<string>>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<System.Collections.Generic.List<string>>));
                    
                    gen_to_be_invoked.GetLoadedSceneAssetNames( _results );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneManager.GetLoadedSceneAssetNames!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SceneIsLoading(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.SceneIsLoading( _abPath, _assetName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLoadingSceneAssetNames(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                        System.Collections.Generic.List<System.Collections.Generic.List<string>> gen_ret = gen_to_be_invoked.GetLoadingSceneAssetNames(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<System.Collections.Generic.List<System.Collections.Generic.List<string>>>(L, 2)) 
                {
                    System.Collections.Generic.List<System.Collections.Generic.List<string>> _results = (System.Collections.Generic.List<System.Collections.Generic.List<string>>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<System.Collections.Generic.List<string>>));
                    
                    gen_to_be_invoked.GetLoadingSceneAssetNames( _results );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneManager.GetLoadingSceneAssetNames!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SceneIsUnloading(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.SceneIsUnloading( _abPath, _assetName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnloadingSceneAssetNames(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                        System.Collections.Generic.List<System.Collections.Generic.List<string>> gen_ret = gen_to_be_invoked.GetUnloadingSceneAssetNames(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<System.Collections.Generic.List<System.Collections.Generic.List<string>>>(L, 2)) 
                {
                    System.Collections.Generic.List<System.Collections.Generic.List<string>> _results = (System.Collections.Generic.List<System.Collections.Generic.List<string>>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<System.Collections.Generic.List<string>>));
                    
                    gen_to_be_invoked.GetUnloadingSceneAssetNames( _results );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneManager.GetUnloadingSceneAssetNames!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasScene(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.HasScene( _abPath, _assetName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PreLoadSceneAssetNames(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PreLoadSceneAssetNames);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadingSceneAssetNames(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LoadingSceneAssetNames);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadedSceneAssetNames(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LoadedSceneAssetNames);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnloadingSceneAssetNames(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SceneManager gen_to_be_invoked = (Honor.Runtime.SceneManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UnloadingSceneAssetNames);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
