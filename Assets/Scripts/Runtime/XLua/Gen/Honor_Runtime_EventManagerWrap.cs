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
    public class HonorRuntimeEventManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.EventManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 8, 2, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Shutdown", _m_Shutdown);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Check", _m_Check);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Subscribe", _m_Subscribe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Unsubscribe", _m_Unsubscribe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Fire", _m_Fire);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FireNow", _m_FireNow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SubscribedEventCount", _m_SubscribedEventCount);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "SubscribedEventTypeCount", _g_get_SubscribedEventTypeCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EventsForFireCount", _g_get_EventsForFireCount);
            
			
			
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
					
					Honor.Runtime.EventManager gen_ret = new Honor.Runtime.EventManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.EventManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.EventManager gen_to_be_invoked = (Honor.Runtime.EventManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Shutdown(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.EventManager gen_to_be_invoked = (Honor.Runtime.EventManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Shutdown(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Check(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.EventManager gen_to_be_invoked = (Honor.Runtime.EventManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameEventCmd _cmd;translator.Get(L, 2, out _cmd);
                    object _userData = translator.GetObject(L, 3, typeof(object));
                    Honor.Runtime.HonorEventHandler<Honor.Runtime.EventParams> _handler = translator.GetDelegate<Honor.Runtime.HonorEventHandler<Honor.Runtime.EventParams>>(L, 4);
                    
                        bool gen_ret = gen_to_be_invoked.Check( _cmd, _userData, _handler );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Subscribe(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.EventManager gen_to_be_invoked = (Honor.Runtime.EventManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameEventCmd _cmd;translator.Get(L, 2, out _cmd);
                    object _userData = translator.GetObject(L, 3, typeof(object));
                    Honor.Runtime.HonorEventHandler<Honor.Runtime.EventParams> _handler = translator.GetDelegate<Honor.Runtime.HonorEventHandler<Honor.Runtime.EventParams>>(L, 4);
                    
                    gen_to_be_invoked.Subscribe( _cmd, _userData, _handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unsubscribe(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.EventManager gen_to_be_invoked = (Honor.Runtime.EventManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameEventCmd _cmd;translator.Get(L, 2, out _cmd);
                    object _userData = translator.GetObject(L, 3, typeof(object));
                    Honor.Runtime.HonorEventHandler<Honor.Runtime.EventParams> _handler = translator.GetDelegate<Honor.Runtime.HonorEventHandler<Honor.Runtime.EventParams>>(L, 4);
                    
                    gen_to_be_invoked.Unsubscribe( _cmd, _userData, _handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Fire(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.EventManager gen_to_be_invoked = (Honor.Runtime.EventManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    Honor.Runtime.EventParams _e = (Honor.Runtime.EventParams)translator.GetObject(L, 3, typeof(Honor.Runtime.EventParams));
                    
                    gen_to_be_invoked.Fire( _sender, _e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FireNow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.EventManager gen_to_be_invoked = (Honor.Runtime.EventManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    Honor.Runtime.EventParams _e = (Honor.Runtime.EventParams)translator.GetObject(L, 3, typeof(Honor.Runtime.EventParams));
                    
                    gen_to_be_invoked.FireNow( _sender, _e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SubscribedEventCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.EventManager gen_to_be_invoked = (Honor.Runtime.EventManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameEventCmd _cmd;translator.Get(L, 2, out _cmd);
                    
                        int gen_ret = gen_to_be_invoked.SubscribedEventCount( _cmd );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SubscribedEventTypeCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.EventManager gen_to_be_invoked = (Honor.Runtime.EventManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SubscribedEventTypeCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EventsForFireCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.EventManager gen_to_be_invoked = (Honor.Runtime.EventManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.EventsForFireCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
