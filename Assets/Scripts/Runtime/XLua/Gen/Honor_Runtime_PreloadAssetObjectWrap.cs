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
    public class HonorRuntimePreloadAssetObjectWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.PreloadAssetObject);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 7, 7);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "TypeName", _g_get_TypeName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetBundlePath", _g_get_AssetBundlePath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetName", _g_get_AssetName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetPath", _g_get_AssetPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsScene", _g_get_IsScene);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsWeak", _g_get_IsWeak);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetLoadOverCallback", _g_get_AssetLoadOverCallback);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "TypeName", _s_set_TypeName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetBundlePath", _s_set_AssetBundlePath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetName", _s_set_AssetName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetPath", _s_set_AssetPath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsScene", _s_set_IsScene);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsWeak", _s_set_IsWeak);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetLoadOverCallback", _s_set_AssetLoadOverCallback);
            
			
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
					
					Honor.Runtime.PreloadAssetObject gen_ret = new Honor.Runtime.PreloadAssetObject();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.PreloadAssetObject constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TypeName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PreloadAssetObject gen_to_be_invoked = (Honor.Runtime.PreloadAssetObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.TypeName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetBundlePath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PreloadAssetObject gen_to_be_invoked = (Honor.Runtime.PreloadAssetObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.AssetBundlePath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PreloadAssetObject gen_to_be_invoked = (Honor.Runtime.PreloadAssetObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.AssetName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PreloadAssetObject gen_to_be_invoked = (Honor.Runtime.PreloadAssetObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.AssetPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsScene(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PreloadAssetObject gen_to_be_invoked = (Honor.Runtime.PreloadAssetObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsScene);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsWeak(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PreloadAssetObject gen_to_be_invoked = (Honor.Runtime.PreloadAssetObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsWeak);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetLoadOverCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PreloadAssetObject gen_to_be_invoked = (Honor.Runtime.PreloadAssetObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AssetLoadOverCallback);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TypeName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PreloadAssetObject gen_to_be_invoked = (Honor.Runtime.PreloadAssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.TypeName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AssetBundlePath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PreloadAssetObject gen_to_be_invoked = (Honor.Runtime.PreloadAssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetBundlePath = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AssetName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PreloadAssetObject gen_to_be_invoked = (Honor.Runtime.PreloadAssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AssetPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PreloadAssetObject gen_to_be_invoked = (Honor.Runtime.PreloadAssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetPath = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsScene(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PreloadAssetObject gen_to_be_invoked = (Honor.Runtime.PreloadAssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsScene = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsWeak(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PreloadAssetObject gen_to_be_invoked = (Honor.Runtime.PreloadAssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsWeak = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AssetLoadOverCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PreloadAssetObject gen_to_be_invoked = (Honor.Runtime.PreloadAssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetLoadOverCallback = translator.GetDelegate<Honor.Runtime.AssetLoadOverCallback>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
