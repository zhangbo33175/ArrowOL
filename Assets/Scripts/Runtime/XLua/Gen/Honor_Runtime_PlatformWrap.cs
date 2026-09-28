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
    public class HonorRuntimePlatformWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.Platform);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 9, 0);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "IsEditor", _g_get_IsEditor);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "IsAndroid", _g_get_IsAndroid);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "IsIPhone", _g_get_IsIPhone);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "IsFlash", _g_get_IsFlash);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "IsWindows", _g_get_IsWindows);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "IsWindowsPlatform", _g_get_IsWindowsPlatform);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "BUILD_MODE", _g_get_BUILD_MODE);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "NOHONOR", _g_get_NOHONOR);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "_BANSHU", _g_get__BANSHU);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.Platform does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsEditor(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Honor.Runtime.Platform.IsEditor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsAndroid(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Honor.Runtime.Platform.IsAndroid);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsIPhone(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Honor.Runtime.Platform.IsIPhone);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsFlash(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Honor.Runtime.Platform.IsFlash);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsWindows(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Honor.Runtime.Platform.IsWindows);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsWindowsPlatform(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Honor.Runtime.Platform.IsWindowsPlatform);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BUILD_MODE(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Honor.Runtime.Platform.BUILD_MODE);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_NOHONOR(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Honor.Runtime.Platform.NOHONOR);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__BANSHU(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Honor.Runtime.Platform._BANSHU);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
