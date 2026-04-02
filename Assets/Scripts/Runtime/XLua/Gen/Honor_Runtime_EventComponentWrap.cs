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
    public class HonorRuntimeEventComponentWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.EventComponent);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 3, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Check", _m_Check);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Subscribe", _m_Subscribe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Unsubscribe", _m_Unsubscribe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Fire", _m_Fire);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FireNow", _m_FireNow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SubscribedEventCount", _m_SubscribedEventCount);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "EventManager", _g_get_EventManager);
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
					
					Honor.Runtime.EventComponent gen_ret = new Honor.Runtime.EventComponent();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.EventComponent constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Check(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.EventComponent gen_to_be_invoked = (Honor.Runtime.EventComponent)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.EventComponent gen_to_be_invoked = (Honor.Runtime.EventComponent)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.EventComponent gen_to_be_invoked = (Honor.Runtime.EventComponent)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.EventComponent gen_to_be_invoked = (Honor.Runtime.EventComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<object>(L, 2)&& translator.Assignable<Honor.Runtime.GameEventCmd>(L, 3)&& translator.Assignable<System.Collections.Generic.Dictionary<string, object>>(L, 4)) 
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    Honor.Runtime.GameEventCmd _cmd;translator.Get(L, 3, out _cmd);
                    System.Collections.Generic.Dictionary<string, object> _objects = (System.Collections.Generic.Dictionary<string, object>)translator.GetObject(L, 4, typeof(System.Collections.Generic.Dictionary<string, object>));
                    
                    gen_to_be_invoked.Fire( _sender, _cmd, _objects );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<object>(L, 2)&& translator.Assignable<Honor.Runtime.GameEventCmd>(L, 3)) 
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    Honor.Runtime.GameEventCmd _cmd;translator.Get(L, 3, out _cmd);
                    
                    gen_to_be_invoked.Fire( _sender, _cmd );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.EventComponent.Fire!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FireNow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.EventComponent gen_to_be_invoked = (Honor.Runtime.EventComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<object>(L, 2)&& translator.Assignable<Honor.Runtime.GameEventCmd>(L, 3)&& translator.Assignable<System.Collections.Generic.Dictionary<string, object>>(L, 4)) 
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    Honor.Runtime.GameEventCmd _cmd;translator.Get(L, 3, out _cmd);
                    System.Collections.Generic.Dictionary<string, object> _objects = (System.Collections.Generic.Dictionary<string, object>)translator.GetObject(L, 4, typeof(System.Collections.Generic.Dictionary<string, object>));
                    
                    gen_to_be_invoked.FireNow( _sender, _cmd, _objects );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<object>(L, 2)&& translator.Assignable<Honor.Runtime.GameEventCmd>(L, 3)) 
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    Honor.Runtime.GameEventCmd _cmd;translator.Get(L, 3, out _cmd);
                    
                    gen_to_be_invoked.FireNow( _sender, _cmd );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.EventComponent.FireNow!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SubscribedEventCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.EventComponent gen_to_be_invoked = (Honor.Runtime.EventComponent)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _g_get_EventManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.EventComponent gen_to_be_invoked = (Honor.Runtime.EventComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.EventManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SubscribedEventTypeCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.EventComponent gen_to_be_invoked = (Honor.Runtime.EventComponent)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.EventComponent gen_to_be_invoked = (Honor.Runtime.EventComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.EventsForFireCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
