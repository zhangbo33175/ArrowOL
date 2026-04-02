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
    public class HonorRuntimeAssetLoadManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.AssetLoadManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 17, 6, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadSync", _m_LoadSync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAsync", _m_LoadAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PreLoadAsync", _m_PreLoadAsync);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Unload", _m_Unload);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ForceUnloadUnusedAssets", _m_ForceUnloadUnusedAssets);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsFileExist", _m_IsFileExist);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAssetSuffix", _m_GetAssetSuffix);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddAsset", _m_AddAsset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddAssetRef", _m_AddAssetRef);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveCallBack", _m_RemoveCallBack);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAssetAbsoluteFullPath", _m_GetAssetAbsoluteFullPath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAssetRelativeFullPath", _m_GetAssetRelativeFullPath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAssetPath", _m_GetAssetPath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLoadingAssetObjectFromList", _m_GetLoadingAssetObjectFromList);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLoadedAssetObjectFromList", _m_GetLoadedAssetObjectFromList);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUnLoadAssetObjectFromList", _m_GetUnLoadAssetObjectFromList);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadingList", _g_get_LoadingList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadedList", _g_get_LoadedList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnloadList", _g_get_UnloadList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PreloadedAsyncList", _g_get_PreloadedAsyncList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetBundleLoadManager", _g_get_AssetBundleLoadManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Scenes", _g_get_Scenes);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "UnloadAssetDelayFrameNum", _s_set_UnloadAssetDelayFrameNum);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LoadedMaxNumToCleanMemery", _s_set_LoadedMaxNumToCleanMemery);
            
			
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
				if(LuaAPI.lua_gettop(L) == 4 && LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4))
				{
					bool _editorResourceMode = LuaAPI.lua_toboolean(L, 2);
					int _unloadAssetDelayFrameNum = LuaAPI.xlua_tointeger(L, 3);
					int _loadedMaxNumToCleanMemery = LuaAPI.xlua_tointeger(L, 4);
					
					Honor.Runtime.AssetLoadManager gen_ret = new Honor.Runtime.AssetLoadManager(_editorResourceMode, _unloadAssetDelayFrameNum, _loadedMaxNumToCleanMemery);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetLoadManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    
                        UnityEngine.Object gen_ret = gen_to_be_invoked.LoadSync( _typeName, _abPath, _assetName );
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
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    Honor.Runtime.AssetLoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetLoadOverCallback>(L, 5);
                    
                    gen_to_be_invoked.LoadAsync( _typeName, _abPath, _assetName, _overCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PreLoadAsync(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& translator.Assignable<Honor.Runtime.AssetLoadOverCallback>(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    Honor.Runtime.AssetLoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetLoadOverCallback>(L, 5);
                    bool _isWeak = LuaAPI.lua_toboolean(L, 6);
                    
                    gen_to_be_invoked.PreLoadAsync( _typeName, _abPath, _assetName, _overCallback, _isWeak );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& translator.Assignable<Honor.Runtime.AssetLoadOverCallback>(L, 5)) 
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    Honor.Runtime.AssetLoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetLoadOverCallback>(L, 5);
                    
                    gen_to_be_invoked.PreLoadAsync( _typeName, _abPath, _assetName, _overCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetLoadManager.PreLoadAsync!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unload(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<object>(L, 2)&& translator.Assignable<Honor.Runtime.AssetUnloadOverCallback>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    object _oriAsset = translator.GetObject(L, 2, typeof(object));
                    Honor.Runtime.AssetUnloadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetUnloadOverCallback>(L, 3);
                    bool _rightNow = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.Unload( _oriAsset, _overCallback, _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<object>(L, 2)&& translator.Assignable<Honor.Runtime.AssetUnloadOverCallback>(L, 3)) 
                {
                    object _oriAsset = translator.GetObject(L, 2, typeof(object));
                    Honor.Runtime.AssetUnloadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetUnloadOverCallback>(L, 3);
                    
                    gen_to_be_invoked.Unload( _oriAsset, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<object>(L, 2)) 
                {
                    object _oriAsset = translator.GetObject(L, 2, typeof(object));
                    
                    gen_to_be_invoked.Unload( _oriAsset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetLoadManager.Unload!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ForceUnloadUnusedAssets(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<System.Action>(L, 2)) 
                {
                    System.Action _overcallback = translator.GetDelegate<System.Action>(L, 2);
                    
                    gen_to_be_invoked.ForceUnloadUnusedAssets( _overcallback );
                    
                    
                    
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetLoadManager.ForceUnloadUnusedAssets!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsFileExist(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    
                        bool gen_ret = gen_to_be_invoked.IsFileExist( _typeName, _abPath, _assetName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAssetSuffix(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    
                        string gen_ret = gen_to_be_invoked.GetAssetSuffix( _typeName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    UnityEngine.Object _asset = (UnityEngine.Object)translator.GetObject(L, 5, typeof(UnityEngine.Object));
                    
                    gen_to_be_invoked.AddAsset( _typeName, _abPath, _assetName, _asset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddAssetRef(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    
                    gen_to_be_invoked.AddAssetRef( _typeName, _abPath, _assetName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveCallBack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    Honor.Runtime.AssetLoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.AssetLoadOverCallback>(L, 5);
                    
                    gen_to_be_invoked.RemoveCallBack( _typeName, _abPath, _assetName, _overCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAssetAbsoluteFullPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    
                        string gen_ret = gen_to_be_invoked.GetAssetAbsoluteFullPath( _typeName, _abPath, _assetName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAssetRelativeFullPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    
                        string gen_ret = gen_to_be_invoked.GetAssetRelativeFullPath( _typeName, _abPath, _assetName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAssetPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    
                        string gen_ret = gen_to_be_invoked.GetAssetPath( _typeName, _abPath, _assetName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLoadingAssetObjectFromList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    
                        Honor.Runtime.AssetObject gen_ret = gen_to_be_invoked.GetLoadingAssetObjectFromList( _typeName, _abPath, _assetName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLoadedAssetObjectFromList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    
                        Honor.Runtime.AssetObject gen_ret = gen_to_be_invoked.GetLoadedAssetObjectFromList( _typeName, _abPath, _assetName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUnLoadAssetObjectFromList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 2);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    
                        Honor.Runtime.AssetObject gen_ret = gen_to_be_invoked.GetUnLoadAssetObjectFromList( _typeName, _abPath, _assetName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadingList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LoadingList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadedList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LoadedList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnloadList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UnloadList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PreloadedAsyncList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PreloadedAsyncList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetBundleLoadManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AssetBundleLoadManager);
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
			
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Scenes);
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
			
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.AssetLoadManager gen_to_be_invoked = (Honor.Runtime.AssetLoadManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LoadedMaxNumToCleanMemery = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
