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
    public class HonorRuntimeSceneComponentWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.SceneComponent);
			Utils.BeginObjectRegister(type, L, translator, 0, 18, 8, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DestroyAllSceneGOs", _m_DestroyAllSceneGOs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PreLoadSceneAsync", _m_PreLoadSceneAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadSceneAsync", _m_LoadSceneAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadSceneSync", _m_LoadSceneSync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadScene", _m_UnloadScene);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLoadedSceneAssetNames", _m_GetLoadedSceneAssetNames);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLoadingSceneAssetNames", _m_GetLoadingSceneAssetNames);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUnloadingSceneAssetNames", _m_GetUnloadingSceneAssetNames);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasScene", _m_HasScene);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddSceneCamera", _m_AddSceneCamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveSceneCamera", _m_RemoveSceneCamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveSceneCameraByIndex", _m_RemoveSceneCameraByIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSceneCameraIndex", _m_GetSceneCameraIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSceneCamera", _m_GetSceneCamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetSceneCameraEnable", _m_SetSceneCameraEnable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitSceneCamera", _m_InitSceneCamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitSceneCameraWithAnimation", _m_InitSceneCameraWithAnimation);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaySceneCameraAnimationToTarget", _m_PlaySceneCameraAnimationToTarget);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "SceneCameras", _g_get_SceneCameras);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SceneRootGO", _g_get_SceneRootGO);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SceneManager", _g_get_SceneManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TouchComponent", _g_get_TouchComponent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PreLoadSceneAssetNames", _g_get_PreLoadSceneAssetNames);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadingSceneAssetNames", _g_get_LoadingSceneAssetNames);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadedSceneAssetNames", _g_get_LoadedSceneAssetNames);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnloadingSceneAssetNames", _g_get_UnloadingSceneAssetNames);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "SceneCameras", _s_set_SceneCameras);
            
			
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
					
					Honor.Runtime.SceneComponent gen_ret = new Honor.Runtime.SceneComponent();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneComponent constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DestroyAllSceneGOs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneComponent.DestroyAllSceneGOs!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PreLoadSceneAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneComponent.LoadSceneAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSceneSync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneComponent.UnloadScene!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLoadedSceneAssetNames(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneComponent.GetLoadedSceneAssetNames!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLoadingSceneAssetNames(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneComponent.GetLoadingSceneAssetNames!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnloadingSceneAssetNames(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneComponent.GetUnloadingSceneAssetNames!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasScene(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_AddSceneCamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                        int gen_ret = gen_to_be_invoked.AddSceneCamera( _camera );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveSceneCamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                        bool gen_ret = gen_to_be_invoked.RemoveSceneCamera( _camera );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveSceneCameraByIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.RemoveSceneCameraByIndex( _index );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSceneCameraIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                        int gen_ret = gen_to_be_invoked.GetSceneCameraIndex( _camera );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSceneCamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.Camera gen_ret = gen_to_be_invoked.GetSceneCamera( _index );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSceneCameraEnable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    bool _enabled = LuaAPI.lua_toboolean(L, 2);
                    int _index = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.SetSceneCameraEnable( _enabled, _index );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _enabled = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetSceneCameraEnable( _enabled );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneComponent.SetSceneCameraEnable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitSceneCamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _sceneCameraIndex = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Vector3 _originalPosition;translator.Get(L, 3, out _originalPosition);
                    UnityEngine.Quaternion _originalRotation;translator.Get(L, 4, out _originalRotation);
                    float _sizeOrField = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.InitSceneCamera( _sceneCameraIndex, _originalPosition, _originalRotation, _sizeOrField );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitSceneCameraWithAnimation(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 11&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.Vector3>(L, 6)&& translator.Assignable<UnityEngine.Quaternion>(L, 7)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 8)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 9)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 10)&& translator.Assignable<System.Action>(L, 11)) 
                {
                    int _sceneCameraIndex = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Vector3 _originalPosition;translator.Get(L, 3, out _originalPosition);
                    UnityEngine.Quaternion _originalRotation;translator.Get(L, 4, out _originalRotation);
                    float _originalSizeOrField = (float)LuaAPI.lua_tonumber(L, 5);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 6, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 7, out _targetRotation);
                    float _targetSizeOrField = (float)LuaAPI.lua_tonumber(L, 8);
                    float _targetDuration = (float)LuaAPI.lua_tonumber(L, 9);
                    bool _canInterruptByGestures = LuaAPI.lua_toboolean(L, 10);
                    System.Action _overCallback = translator.GetDelegate<System.Action>(L, 11);
                    
                    gen_to_be_invoked.InitSceneCameraWithAnimation( _sceneCameraIndex, _originalPosition, _originalRotation, _originalSizeOrField, _targetPosition, _targetRotation, _targetSizeOrField, _targetDuration, _canInterruptByGestures, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 10&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.Vector3>(L, 6)&& translator.Assignable<UnityEngine.Quaternion>(L, 7)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 8)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 9)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 10)) 
                {
                    int _sceneCameraIndex = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Vector3 _originalPosition;translator.Get(L, 3, out _originalPosition);
                    UnityEngine.Quaternion _originalRotation;translator.Get(L, 4, out _originalRotation);
                    float _originalSizeOrField = (float)LuaAPI.lua_tonumber(L, 5);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 6, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 7, out _targetRotation);
                    float _targetSizeOrField = (float)LuaAPI.lua_tonumber(L, 8);
                    float _targetDuration = (float)LuaAPI.lua_tonumber(L, 9);
                    bool _canInterruptByGestures = LuaAPI.lua_toboolean(L, 10);
                    
                    gen_to_be_invoked.InitSceneCameraWithAnimation( _sceneCameraIndex, _originalPosition, _originalRotation, _originalSizeOrField, _targetPosition, _targetRotation, _targetSizeOrField, _targetDuration, _canInterruptByGestures );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 9&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.Vector3>(L, 6)&& translator.Assignable<UnityEngine.Quaternion>(L, 7)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 8)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 9)) 
                {
                    int _sceneCameraIndex = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Vector3 _originalPosition;translator.Get(L, 3, out _originalPosition);
                    UnityEngine.Quaternion _originalRotation;translator.Get(L, 4, out _originalRotation);
                    float _originalSizeOrField = (float)LuaAPI.lua_tonumber(L, 5);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 6, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 7, out _targetRotation);
                    float _targetSizeOrField = (float)LuaAPI.lua_tonumber(L, 8);
                    float _targetDuration = (float)LuaAPI.lua_tonumber(L, 9);
                    
                    gen_to_be_invoked.InitSceneCameraWithAnimation( _sceneCameraIndex, _originalPosition, _originalRotation, _originalSizeOrField, _targetPosition, _targetRotation, _targetSizeOrField, _targetDuration );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 8&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.Vector3>(L, 6)&& translator.Assignable<UnityEngine.Quaternion>(L, 7)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 8)) 
                {
                    int _sceneCameraIndex = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Vector3 _originalPosition;translator.Get(L, 3, out _originalPosition);
                    UnityEngine.Quaternion _originalRotation;translator.Get(L, 4, out _originalRotation);
                    float _originalSizeOrField = (float)LuaAPI.lua_tonumber(L, 5);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 6, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 7, out _targetRotation);
                    float _targetSizeOrField = (float)LuaAPI.lua_tonumber(L, 8);
                    
                    gen_to_be_invoked.InitSceneCameraWithAnimation( _sceneCameraIndex, _originalPosition, _originalRotation, _originalSizeOrField, _targetPosition, _targetRotation, _targetSizeOrField );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.Vector3>(L, 6)&& translator.Assignable<UnityEngine.Quaternion>(L, 7)) 
                {
                    int _sceneCameraIndex = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Vector3 _originalPosition;translator.Get(L, 3, out _originalPosition);
                    UnityEngine.Quaternion _originalRotation;translator.Get(L, 4, out _originalRotation);
                    float _originalSizeOrField = (float)LuaAPI.lua_tonumber(L, 5);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 6, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 7, out _targetRotation);
                    
                    gen_to_be_invoked.InitSceneCameraWithAnimation( _sceneCameraIndex, _originalPosition, _originalRotation, _originalSizeOrField, _targetPosition, _targetRotation );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& translator.Assignable<UnityEngine.Vector3>(L, 6)) 
                {
                    int _sceneCameraIndex = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Vector3 _originalPosition;translator.Get(L, 3, out _originalPosition);
                    UnityEngine.Quaternion _originalRotation;translator.Get(L, 4, out _originalRotation);
                    float _originalSizeOrField = (float)LuaAPI.lua_tonumber(L, 5);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 6, out _targetPosition);
                    
                    gen_to_be_invoked.InitSceneCameraWithAnimation( _sceneCameraIndex, _originalPosition, _originalRotation, _originalSizeOrField, _targetPosition );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    int _sceneCameraIndex = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Vector3 _originalPosition;translator.Get(L, 3, out _originalPosition);
                    UnityEngine.Quaternion _originalRotation;translator.Get(L, 4, out _originalRotation);
                    float _originalSizeOrField = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.InitSceneCameraWithAnimation( _sceneCameraIndex, _originalPosition, _originalRotation, _originalSizeOrField );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneComponent.InitSceneCameraWithAnimation!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaySceneCameraAnimationToTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 8&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& translator.Assignable<UnityEngine.Quaternion>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)&& translator.Assignable<System.Action>(L, 8)) 
                {
                    int _sceneCameraIndex = LuaAPI.xlua_tointeger(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 4, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 5, out _targetRotation);
                    float _targetSizeOrField = (float)LuaAPI.lua_tonumber(L, 6);
                    bool _canInterruptByGestures = LuaAPI.lua_toboolean(L, 7);
                    System.Action _overCallback = translator.GetDelegate<System.Action>(L, 8);
                    
                    gen_to_be_invoked.PlaySceneCameraAnimationToTarget( _sceneCameraIndex, _duration, _targetPosition, _targetRotation, _targetSizeOrField, _canInterruptByGestures, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& translator.Assignable<UnityEngine.Quaternion>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)) 
                {
                    int _sceneCameraIndex = LuaAPI.xlua_tointeger(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 4, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 5, out _targetRotation);
                    float _targetSizeOrField = (float)LuaAPI.lua_tonumber(L, 6);
                    bool _canInterruptByGestures = LuaAPI.lua_toboolean(L, 7);
                    
                    gen_to_be_invoked.PlaySceneCameraAnimationToTarget( _sceneCameraIndex, _duration, _targetPosition, _targetRotation, _targetSizeOrField, _canInterruptByGestures );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)&& translator.Assignable<UnityEngine.Quaternion>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    int _sceneCameraIndex = LuaAPI.xlua_tointeger(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 4, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 5, out _targetRotation);
                    float _targetSizeOrField = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    gen_to_be_invoked.PlaySceneCameraAnimationToTarget( _sceneCameraIndex, _duration, _targetPosition, _targetRotation, _targetSizeOrField );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneComponent.PlaySceneCameraAnimationToTarget!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SceneCameras(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SceneCameras);
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
			
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SceneRootGO);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SceneManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SceneManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TouchComponent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TouchComponent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PreLoadSceneAssetNames(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UnloadingSceneAssetNames);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SceneCameras(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SceneComponent gen_to_be_invoked = (Honor.Runtime.SceneComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SceneCameras = (System.Collections.Generic.List<UnityEngine.Camera>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Camera>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
