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
    public class HonorRuntimeLauncherComponentDevicePerformanceDataWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.LauncherComponent.DevicePerformanceData);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 5, 5);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "ProcessorCount", _g_get_ProcessorCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GraphicsMemorySizeHighBase", _g_get_GraphicsMemorySizeHighBase);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GraphicsMemorySizeMidBase", _g_get_GraphicsMemorySizeMidBase);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SystemMemorySizeHighBase", _g_get_SystemMemorySizeHighBase);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SystemMemorySizeMidBase", _g_get_SystemMemorySizeMidBase);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "ProcessorCount", _s_set_ProcessorCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GraphicsMemorySizeHighBase", _s_set_GraphicsMemorySizeHighBase);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GraphicsMemorySizeMidBase", _s_set_GraphicsMemorySizeMidBase);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SystemMemorySizeHighBase", _s_set_SystemMemorySizeHighBase);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SystemMemorySizeMidBase", _s_set_SystemMemorySizeMidBase);
            
			
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
				if(LuaAPI.lua_gettop(L) == 6 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6))
				{
					int _processorCount = LuaAPI.xlua_tointeger(L, 2);
					int _graphicsMemorySizeHighBase = LuaAPI.xlua_tointeger(L, 3);
					int _graphicsMemorySizeMidBase = LuaAPI.xlua_tointeger(L, 4);
					int _systemMemorySizeHighBase = LuaAPI.xlua_tointeger(L, 5);
					int _systemMemorySizeMidBase = LuaAPI.xlua_tointeger(L, 6);
					
					Honor.Runtime.LauncherComponent.DevicePerformanceData gen_ret = new Honor.Runtime.LauncherComponent.DevicePerformanceData(_processorCount, _graphicsMemorySizeHighBase, _graphicsMemorySizeMidBase, _systemMemorySizeHighBase, _systemMemorySizeMidBase);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.LauncherComponent.DevicePerformanceData constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ProcessorCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent.DevicePerformanceData gen_to_be_invoked = (Honor.Runtime.LauncherComponent.DevicePerformanceData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ProcessorCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GraphicsMemorySizeHighBase(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent.DevicePerformanceData gen_to_be_invoked = (Honor.Runtime.LauncherComponent.DevicePerformanceData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.GraphicsMemorySizeHighBase);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GraphicsMemorySizeMidBase(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent.DevicePerformanceData gen_to_be_invoked = (Honor.Runtime.LauncherComponent.DevicePerformanceData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.GraphicsMemorySizeMidBase);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SystemMemorySizeHighBase(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent.DevicePerformanceData gen_to_be_invoked = (Honor.Runtime.LauncherComponent.DevicePerformanceData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SystemMemorySizeHighBase);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SystemMemorySizeMidBase(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent.DevicePerformanceData gen_to_be_invoked = (Honor.Runtime.LauncherComponent.DevicePerformanceData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SystemMemorySizeMidBase);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ProcessorCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent.DevicePerformanceData gen_to_be_invoked = (Honor.Runtime.LauncherComponent.DevicePerformanceData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ProcessorCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GraphicsMemorySizeHighBase(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent.DevicePerformanceData gen_to_be_invoked = (Honor.Runtime.LauncherComponent.DevicePerformanceData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GraphicsMemorySizeHighBase = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GraphicsMemorySizeMidBase(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent.DevicePerformanceData gen_to_be_invoked = (Honor.Runtime.LauncherComponent.DevicePerformanceData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GraphicsMemorySizeMidBase = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SystemMemorySizeHighBase(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent.DevicePerformanceData gen_to_be_invoked = (Honor.Runtime.LauncherComponent.DevicePerformanceData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SystemMemorySizeHighBase = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SystemMemorySizeMidBase(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LauncherComponent.DevicePerformanceData gen_to_be_invoked = (Honor.Runtime.LauncherComponent.DevicePerformanceData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SystemMemorySizeMidBase = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
