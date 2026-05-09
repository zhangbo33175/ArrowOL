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
    public class HonorRuntimeTimerCounterWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.TimerCounter);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 5, 5);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLeftTime", _m_GetLeftTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetCallBack", _m_SetCallBack);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Del", _g_get_Del);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DelayTime", _g_get_DelayTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DeltaTime", _g_get_DeltaTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Owner", _g_get_Owner);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DelObj", _g_get_DelObj);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Del", _s_set_Del);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DelayTime", _s_set_DelayTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DeltaTime", _s_set_DeltaTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Owner", _s_set_Owner);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DelObj", _s_set_DelObj);
            
			
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
					
					Honor.Runtime.TimerCounter gen_ret = new Honor.Runtime.TimerCounter();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.TimerCounter constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLeftTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.TimerCounter gen_to_be_invoked = (Honor.Runtime.TimerCounter)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        float gen_ret = gen_to_be_invoked.GetLeftTime(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetCallBack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.TimerCounter gen_to_be_invoked = (Honor.Runtime.TimerCounter)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<string> _func = translator.GetDelegate<System.Action<string>>(L, 2);
                    
                    gen_to_be_invoked.SetCallBack( _func );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Del(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.TimerCounter gen_to_be_invoked = (Honor.Runtime.TimerCounter)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Del);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DelayTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.TimerCounter gen_to_be_invoked = (Honor.Runtime.TimerCounter)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.DelayTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DeltaTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.TimerCounter gen_to_be_invoked = (Honor.Runtime.TimerCounter)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.DeltaTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Owner(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.TimerCounter gen_to_be_invoked = (Honor.Runtime.TimerCounter)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Owner);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DelObj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.TimerCounter gen_to_be_invoked = (Honor.Runtime.TimerCounter)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.DelObj);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Del(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.TimerCounter gen_to_be_invoked = (Honor.Runtime.TimerCounter)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Del = translator.GetDelegate<System.Action<string>>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DelayTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.TimerCounter gen_to_be_invoked = (Honor.Runtime.TimerCounter)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DelayTime = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DeltaTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.TimerCounter gen_to_be_invoked = (Honor.Runtime.TimerCounter)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DeltaTime = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Owner(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.TimerCounter gen_to_be_invoked = (Honor.Runtime.TimerCounter)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Owner = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DelObj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.TimerCounter gen_to_be_invoked = (Honor.Runtime.TimerCounter)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DelObj = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
