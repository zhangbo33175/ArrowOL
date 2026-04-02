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
    public class HonorRuntimePrefabObjectWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.PrefabObject);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 10, 10);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetBundlePath", _g_get_AssetBundlePath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetName", _g_get_AssetName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetPath", _g_get_AssetPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LockCallbackCount", _g_get_LockCallbackCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PrefabLoadOverCallbackList", _g_get_PrefabLoadOverCallbackList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PrefabLoadLuaTableParamList", _g_get_PrefabLoadLuaTableParamList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PrefabInstancingGOParentList", _g_get_PrefabInstancingGOParentList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Asset", _g_get_Asset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RefCount", _g_get_RefCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GOInstanceIDs", _g_get_GOInstanceIDs);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetBundlePath", _s_set_AssetBundlePath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetName", _s_set_AssetName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetPath", _s_set_AssetPath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LockCallbackCount", _s_set_LockCallbackCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PrefabLoadOverCallbackList", _s_set_PrefabLoadOverCallbackList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PrefabLoadLuaTableParamList", _s_set_PrefabLoadLuaTableParamList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PrefabInstancingGOParentList", _s_set_PrefabInstancingGOParentList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Asset", _s_set_Asset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RefCount", _s_set_RefCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GOInstanceIDs", _s_set_GOInstanceIDs);
            
			
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
					
					Honor.Runtime.PrefabObject gen_ret = new Honor.Runtime.PrefabObject();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.PrefabObject constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetBundlePath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.AssetPath);
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
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.LockCallbackCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PrefabLoadOverCallbackList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PrefabLoadOverCallbackList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PrefabLoadLuaTableParamList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PrefabLoadLuaTableParamList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PrefabInstancingGOParentList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PrefabInstancingGOParentList);
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
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Asset);
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
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.RefCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GOInstanceIDs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.GOInstanceIDs);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AssetBundlePath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetPath = LuaAPI.lua_tostring(L, 2);
            
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
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LockCallbackCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PrefabLoadOverCallbackList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PrefabLoadOverCallbackList = (System.Collections.Generic.List<Honor.Runtime.PrefabLoadOverCallback>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<Honor.Runtime.PrefabLoadOverCallback>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PrefabLoadLuaTableParamList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PrefabLoadLuaTableParamList = (System.Collections.Generic.List<XLua.LuaTable>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<XLua.LuaTable>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PrefabInstancingGOParentList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PrefabInstancingGOParentList = (System.Collections.Generic.List<UnityEngine.Transform>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Transform>));
            
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
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Asset = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
            
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
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RefCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GOInstanceIDs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PrefabObject gen_to_be_invoked = (Honor.Runtime.PrefabObject)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GOInstanceIDs = (System.Collections.Generic.HashSet<int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.HashSet<int>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
