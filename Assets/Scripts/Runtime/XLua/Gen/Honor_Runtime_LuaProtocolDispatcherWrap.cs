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
    public class HonorRuntimeLuaProtocolDispatcherWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.LuaProtocolDispatcher);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Request", _m_Request);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReceiveSuccess", _m_ReceiveSuccess);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReceiveFail", _m_ReceiveFail);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BindPushMethod", _m_BindPushMethod);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemovePushMethod", _m_RemovePushMethod);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CallPushMethod", _m_CallPushMethod);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearRequests", _m_ClearRequests);
			
			
			
			
			
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
					
					Honor.Runtime.LuaProtocolDispatcher gen_ret = new Honor.Runtime.LuaProtocolDispatcher();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.LuaProtocolDispatcher constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Request(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaProtocolDispatcher gen_to_be_invoked = (Honor.Runtime.LuaProtocolDispatcher)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 2);
                    object _content = translator.GetObject(L, 3, typeof(object));
                    Honor.Runtime.LuaProtocolSendFun _requestFun = translator.GetDelegate<Honor.Runtime.LuaProtocolSendFun>(L, 4);
                    
                    gen_to_be_invoked.Request( _cmd, _content, _requestFun );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReceiveSuccess(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaProtocolDispatcher gen_to_be_invoked = (Honor.Runtime.LuaProtocolDispatcher)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 2);
                    object _content = translator.GetObject(L, 3, typeof(object));
                    
                    gen_to_be_invoked.ReceiveSuccess( _cmd, _content );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReceiveFail(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaProtocolDispatcher gen_to_be_invoked = (Honor.Runtime.LuaProtocolDispatcher)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 2);
                    object _content = translator.GetObject(L, 3, typeof(object));
                    
                    gen_to_be_invoked.ReceiveFail( _cmd, _content );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BindPushMethod(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaProtocolDispatcher gen_to_be_invoked = (Honor.Runtime.LuaProtocolDispatcher)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 2);
                    Honor.Runtime.LuaProtocolPushFun _bind = translator.GetDelegate<Honor.Runtime.LuaProtocolPushFun>(L, 3);
                    
                    gen_to_be_invoked.BindPushMethod( _cmd, _bind );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemovePushMethod(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaProtocolDispatcher gen_to_be_invoked = (Honor.Runtime.LuaProtocolDispatcher)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.RemovePushMethod( _cmd );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CallPushMethod(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaProtocolDispatcher gen_to_be_invoked = (Honor.Runtime.LuaProtocolDispatcher)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 2);
                    object _content = translator.GetObject(L, 3, typeof(object));
                    
                    gen_to_be_invoked.CallPushMethod( _cmd, _content );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearRequests(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaProtocolDispatcher gen_to_be_invoked = (Honor.Runtime.LuaProtocolDispatcher)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearRequests(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
