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
    public class HonorRuntimeAssetBundleObjectWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.AssetBundleObject);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 8, 6);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "FormatPath", _g_get_FormatPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RefCount", _g_get_RefCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DependLoadingCount", _g_get_DependLoadingCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Origin", _g_get_Origin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Request", _g_get_Request);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetBundles", _g_get_AssetBundles);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Depends", _g_get_Depends);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetBundleLoadOverCallbacksList", _g_get_AssetBundleLoadOverCallbacksList);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "FormatPath", _s_set_FormatPath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RefCount", _s_set_RefCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DependLoadingCount", _s_set_DependLoadingCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Origin", _s_set_Origin);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Request", _s_set_Request);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetBundles", _s_set_AssetBundles);
            
			
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
					
					Honor.Runtime.AssetBundleObject gen_ret = new Honor.Runtime.AssetBundleObject();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetBundleObject constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FormatPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleObject gen_to_be_invoked = (Honor.Runtime.AssetBundleObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.FormatPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RefCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleObject gen_to_be_invoked = (Honor.Runtime.AssetBundleObject)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.RefCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DependLoadingCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleObject gen_to_be_invoked = (Honor.Runtime.AssetBundleObject)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.DependLoadingCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Origin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleObject gen_to_be_invoked = (Honor.Runtime.AssetBundleObject)translator.FastGetCSObj(L, 1);
                translator.PushHonorRuntimeOriginType(L, gen_to_be_invoked.Origin);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Request(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleObject gen_to_be_invoked = (Honor.Runtime.AssetBundleObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Request);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetBundles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleObject gen_to_be_invoked = (Honor.Runtime.AssetBundleObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AssetBundles);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Depends(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleObject gen_to_be_invoked = (Honor.Runtime.AssetBundleObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Depends);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetBundleLoadOverCallbacksList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleObject gen_to_be_invoked = (Honor.Runtime.AssetBundleObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AssetBundleLoadOverCallbacksList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FormatPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleObject gen_to_be_invoked = (Honor.Runtime.AssetBundleObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FormatPath = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RefCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleObject gen_to_be_invoked = (Honor.Runtime.AssetBundleObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RefCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DependLoadingCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleObject gen_to_be_invoked = (Honor.Runtime.AssetBundleObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DependLoadingCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Origin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleObject gen_to_be_invoked = (Honor.Runtime.AssetBundleObject)translator.FastGetCSObj(L, 1);
                Honor.Runtime.OriginType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.Origin = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Request(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleObject gen_to_be_invoked = (Honor.Runtime.AssetBundleObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Request = (UnityEngine.AssetBundleCreateRequest)translator.GetObject(L, 2, typeof(UnityEngine.AssetBundleCreateRequest));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AssetBundles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetBundleObject gen_to_be_invoked = (Honor.Runtime.AssetBundleObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetBundles = (UnityEngine.AssetBundle)translator.GetObject(L, 2, typeof(UnityEngine.AssetBundle));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
