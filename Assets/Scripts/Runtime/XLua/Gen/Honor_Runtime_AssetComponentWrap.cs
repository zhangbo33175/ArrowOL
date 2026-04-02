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
    public class HonorRuntimeAssetComponentWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.AssetComponent);
			Utils.BeginObjectRegister(type, L, translator, 0, 14, 16, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadManifest", _m_LoadManifest);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadPrefabSync", _m_LoadPrefabSync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadPrefabAsync", _m_LoadPrefabAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InstantiateGO", _m_InstantiateGO);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAssetSync", _m_LoadAssetSync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAssetAsync", _m_LoadAssetAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PreLoadAssetAsync", _m_PreLoadAssetAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadAsset", _m_UnloadAsset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ForceUnloadUnusedAssets", _m_ForceUnloadUnusedAssets);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadSceneSync", _m_LoadSceneSync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadSceneAsync", _m_LoadSceneAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PreLoadSceneAsync", _m_PreLoadSceneAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadScene", _m_UnloadScene);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsAssetExist", _m_IsAssetExist);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "LauncherComponent", _g_get_LauncherComponent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetLoadManager", _g_get_AssetLoadManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PrefabLoadManager", _g_get_PrefabLoadManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DependsDataList", _g_get_DependsDataList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EditorResourceMode", _g_get_EditorResourceMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadedPrefabList", _g_get_LoadedPrefabList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadingAssetList", _g_get_LoadingAssetList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadedAssetList", _g_get_LoadedAssetList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnloadAssetList", _g_get_UnloadAssetList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PreloadedAssetList", _g_get_PreloadedAssetList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ReadyABList", _g_get_ReadyABList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadingABList", _g_get_LoadingABList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadedABList", _g_get_LoadedABList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnloadABList", _g_get_UnloadABList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Scenes", _g_get_Scenes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "StrictCheck", _g_get_StrictCheck);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "UnloadAssetDelayFrameNum", _s_set_UnloadAssetDelayFrameNum);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LoadedMaxNumToCleanMemery", _s_set_LoadedMaxNumToCleanMemery);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "StrictCheck", _s_set_StrictCheck);
            
			
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
					
					Honor.Runtime.AssetComponent gen_ret = new Honor.Runtime.AssetComponent();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetComponent constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadManifest(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LoadManifest(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadPrefabSync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Transform>(L, 4)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TTABLE)) 
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 4, typeof(UnityEngine.Transform));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 5, typeof(XLua.LuaTable));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.LoadPrefabSync( _abPath, _assetName, _parent, _luaParams );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Transform>(L, 4)) 
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 4, typeof(UnityEngine.Transform));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.LoadPrefabSync( _abPath, _assetName, _parent );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetComponent.LoadPrefabSync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadPrefabAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Transform>(L, 4)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TTABLE)&& translator.Assignable<Honor.Runtime.PrefabLoadOverCallback>(L, 6)) 
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 4, typeof(UnityEngine.Transform));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 5, typeof(XLua.LuaTable));
                    Honor.Runtime.PrefabLoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.PrefabLoadOverCallback>(L, 6);
                    
                    gen_to_be_invoked.LoadPrefabAsync( _abPath, _assetName, _parent, _luaParams, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Transform>(L, 4)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TTABLE)) 
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 4, typeof(UnityEngine.Transform));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 5, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.LoadPrefabAsync( _abPath, _assetName, _parent, _luaParams );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Transform>(L, 4)) 
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 4, typeof(UnityEngine.Transform));
                    
                    gen_to_be_invoked.LoadPrefabAsync( _abPath, _assetName, _parent );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetComponent.LoadPrefabAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InstantiateGO(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Transform>(L, 2)&& translator.Assignable<UnityEngine.GameObject>(L, 3)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TTABLE)) 
                {
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    UnityEngine.GameObject _childTemplateGO = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.InstantiateGO( _parent, _childTemplateGO, _luaParams );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Transform>(L, 2)&& translator.Assignable<UnityEngine.GameObject>(L, 3)) 
                {
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    UnityEngine.GameObject _childTemplateGO = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.InstantiateGO( _parent, _childTemplateGO );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetComponent.InstantiateGO!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAssetSync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    
                        UnityEngine.Object gen_ret = gen_to_be_invoked.LoadAssetSync( _typeName, _abPath, _assetName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAssetAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    Honor.Runtime.AssetLoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetLoadOverCallback>(L, 5);
                    
                    gen_to_be_invoked.LoadAssetAsync( _typeName, _abPath, _assetName, _overCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PreLoadAssetAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& translator.Assignable<Honor.Runtime.AssetLoadOverCallback>(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    Honor.Runtime.AssetLoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetLoadOverCallback>(L, 5);
                    bool _isWeak = LuaAPI.lua_toboolean(L, 6);
                    
                    gen_to_be_invoked.PreLoadAssetAsync( _typeName, _abPath, _assetName, _overCallback, _isWeak );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& translator.Assignable<Honor.Runtime.AssetLoadOverCallback>(L, 5)) 
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    Honor.Runtime.AssetLoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetLoadOverCallback>(L, 5);
                    
                    gen_to_be_invoked.PreLoadAssetAsync( _typeName, _abPath, _assetName, _overCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetComponent.PreLoadAssetAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Object>(L, 2)&& translator.Assignable<Honor.Runtime.AssetUnloadOverCallback>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Object _asset = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
                    Honor.Runtime.AssetUnloadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetUnloadOverCallback>(L, 3);
                    bool _rightNow = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.UnloadAsset( _asset, _overCallback, _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Object>(L, 2)&& translator.Assignable<Honor.Runtime.AssetUnloadOverCallback>(L, 3)) 
                {
                    UnityEngine.Object _asset = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
                    Honor.Runtime.AssetUnloadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetUnloadOverCallback>(L, 3);
                    
                    gen_to_be_invoked.UnloadAsset( _asset, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Object>(L, 2)) 
                {
                    UnityEngine.Object _asset = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
                    
                    gen_to_be_invoked.UnloadAsset( _asset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetComponent.UnloadAsset!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ForceUnloadUnusedAssets(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<System.Action>(L, 2)) 
                {
                    System.Action _overCallback = translator.GetDelegate<System.Action>(L, 2);
                    
                    gen_to_be_invoked.ForceUnloadUnusedAssets( _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.ForceUnloadUnusedAssets(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetComponent.ForceUnloadUnusedAssets!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSceneSync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _sceneName = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.LoadSceneSync( _abPath, _sceneName );
                    
                    
                    
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
            
            
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _sceneName = LuaAPI.lua_tostring(L, 3);
                    Honor.Runtime.AssetLoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetLoadOverCallback>(L, 4);
                    
                    gen_to_be_invoked.LoadSceneAsync( _abPath, _sceneName, _overCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PreLoadSceneAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<Honor.Runtime.AssetLoadOverCallback>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _sceneName = LuaAPI.lua_tostring(L, 3);
                    Honor.Runtime.AssetLoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetLoadOverCallback>(L, 4);
                    bool _isWeak = LuaAPI.lua_toboolean(L, 5);
                    
                    gen_to_be_invoked.PreLoadSceneAsync( _abPath, _sceneName, _overCallback, _isWeak );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<Honor.Runtime.AssetLoadOverCallback>(L, 4)) 
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _sceneName = LuaAPI.lua_tostring(L, 3);
                    Honor.Runtime.AssetLoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetLoadOverCallback>(L, 4);
                    
                    gen_to_be_invoked.PreLoadSceneAsync( _abPath, _sceneName, _overCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetComponent.PreLoadSceneAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadScene(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<Honor.Runtime.AssetUnloadOverCallback>(L, 3)) 
                {
                    string _sceneName = LuaAPI.lua_tostring(L, 2);
                    Honor.Runtime.AssetUnloadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetUnloadOverCallback>(L, 3);
                    
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetComponent.UnloadScene!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsAssetExist(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    
                        bool gen_ret = gen_to_be_invoked.IsAssetExist( _typeName, _abPath, _assetName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LauncherComponent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LauncherComponent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetLoadManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AssetLoadManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PrefabLoadManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PrefabLoadManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DependsDataList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.DependsDataList);
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
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.EditorResourceMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadedPrefabList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LoadedPrefabList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadingAssetList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LoadingAssetList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadedAssetList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LoadedAssetList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnloadAssetList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UnloadAssetList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PreloadedAssetList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PreloadedAssetList);
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
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UnloadABList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Scenes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Scenes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StrictCheck(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.StrictCheck);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UnloadAssetDelayFrameNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UnloadAssetDelayFrameNum = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LoadedMaxNumToCleanMemery(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LoadedMaxNumToCleanMemery = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_StrictCheck(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetComponent gen_to_be_invoked = (Honor.Runtime.AssetComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.StrictCheck = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
