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
    public class HonorRuntimeAssetObjectWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.AssetObject);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 15, 15);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "TypeName", _g_get_TypeName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetBundlePath", _g_get_AssetBundlePath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetName", _g_get_AssetName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetPath", _g_get_AssetPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Origin", _g_get_Origin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsScene", _g_get_IsScene);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LockCallbackCount", _g_get_LockCallbackCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetLoadOverCallbackList", _g_get_AssetLoadOverCallbackList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetUnloadOverCallbackList", _g_get_AssetUnloadOverCallbackList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InstanceID", _g_get_InstanceID);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Request", _g_get_Request);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Asset", _g_get_Asset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsWeak", _g_get_IsWeak);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RefCount", _g_get_RefCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnloadTickNum", _g_get_UnloadTickNum);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "TypeName", _s_set_TypeName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetBundlePath", _s_set_AssetBundlePath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetName", _s_set_AssetName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetPath", _s_set_AssetPath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Origin", _s_set_Origin);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsScene", _s_set_IsScene);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LockCallbackCount", _s_set_LockCallbackCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetLoadOverCallbackList", _s_set_AssetLoadOverCallbackList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetUnloadOverCallbackList", _s_set_AssetUnloadOverCallbackList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "InstanceID", _s_set_InstanceID);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Request", _s_set_Request);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Asset", _s_set_Asset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsWeak", _s_set_IsWeak);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RefCount", _s_set_RefCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UnloadTickNum", _s_set_UnloadTickNum);
            
			
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
					
					Honor.Runtime.AssetObject gen_ret = new Honor.Runtime.AssetObject();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AssetObject constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TypeName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.AssetPath);
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                translator.PushHonorRuntimeOriginType(L, gen_to_be_invoked.Origin);
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsScene);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LockCallbackCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.LockCallbackCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetLoadOverCallbackList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AssetLoadOverCallbackList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetUnloadOverCallbackList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AssetUnloadOverCallbackList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InstanceID(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.InstanceID);
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Request);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Asset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Asset);
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsWeak);
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.RefCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnloadTickNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.UnloadTickNum);
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetPath = LuaAPI.lua_tostring(L, 2);
            
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                Honor.Runtime.OriginType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.Origin = gen_value;
            
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsScene = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LockCallbackCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LockCallbackCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AssetLoadOverCallbackList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetLoadOverCallbackList = (System.Collections.Generic.List<Honor.Runtime.AssetLoadOverCallback>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<Honor.Runtime.AssetLoadOverCallback>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AssetUnloadOverCallbackList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetUnloadOverCallbackList = (System.Collections.Generic.List<Honor.Runtime.AssetUnloadOverCallback>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<Honor.Runtime.AssetUnloadOverCallback>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_InstanceID(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.InstanceID = LuaAPI.xlua_tointeger(L, 2);
            
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Request = (UnityEngine.AsyncOperation)translator.GetObject(L, 2, typeof(UnityEngine.AsyncOperation));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Asset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Asset = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
            
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsWeak = LuaAPI.lua_toboolean(L, 2);
            
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
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RefCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UnloadTickNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AssetObject gen_to_be_invoked = (Honor.Runtime.AssetObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UnloadTickNum = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
