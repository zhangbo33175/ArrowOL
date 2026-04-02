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
    public class GameLibRSetupLuaTableValueRDateTimeWarpYMDHMSWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 6, 6);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Year", _g_get_Year);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Month", _g_get_Month);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Day", _g_get_Day);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Hour", _g_get_Hour);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Minute", _g_get_Minute);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Second", _g_get_Second);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Year", _s_set_Year);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Month", _s_set_Month);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Day", _s_set_Day);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Hour", _s_set_Hour);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Minute", _s_set_Minute);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Second", _s_set_Second);
            
			
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
					
					GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_ret = new GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Year(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_to_be_invoked = (GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Year);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Month(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_to_be_invoked = (GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Month);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Day(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_to_be_invoked = (GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Day);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Hour(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_to_be_invoked = (GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Hour);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Minute(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_to_be_invoked = (GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Minute);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Second(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_to_be_invoked = (GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Second);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Year(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_to_be_invoked = (GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Year = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Month(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_to_be_invoked = (GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Month = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Day(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_to_be_invoked = (GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Day = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Hour(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_to_be_invoked = (GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Hour = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Minute(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_to_be_invoked = (GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Minute = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Second(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_to_be_invoked = (GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Second = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
