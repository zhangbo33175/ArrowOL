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
    public class HonorRuntimeLuaProtocolRunTimeWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.LuaProtocolRunTime);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 11, 3, 2);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Initialize", _m_Initialize_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Release", _m_Release_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Request", _m_Request_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReceiveSuccess", _m_ReceiveSuccess_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReceiveFail", _m_ReceiveFail_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BindPushMethod", _m_BindPushMethod_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RemovePushMethod", _m_RemovePushMethod_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CallPushMethod", _m_CallPushMethod_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "NotifyLua", _m_NotifyLua_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SyncCall", _m_SyncCall_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Dispatcher", _g_get_Dispatcher);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "LuaNotifyHandle", _g_get_LuaNotifyHandle);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "LuaSyncHandle", _g_get_LuaSyncHandle);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "LuaNotifyHandle", _s_set_LuaNotifyHandle);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "LuaSyncHandle", _s_set_LuaSyncHandle);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.LuaProtocolRunTime does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Initialize_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Honor.Runtime.LuaProtocolDispatcher _dispatcher = (Honor.Runtime.LuaProtocolDispatcher)translator.GetObject(L, 1, typeof(Honor.Runtime.LuaProtocolDispatcher));
                    
                    Honor.Runtime.LuaProtocolRunTime.Initialize( _dispatcher );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Release_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Honor.Runtime.LuaProtocolRunTime.Release(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Request_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    object _content = translator.GetObject(L, 2, typeof(object));
                    Honor.Runtime.LuaProtocolSendFun _requestFun = translator.GetDelegate<Honor.Runtime.LuaProtocolSendFun>(L, 3);
                    
                    Honor.Runtime.LuaProtocolRunTime.Request( _cmd, _content, _requestFun );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReceiveSuccess_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    object _content = translator.GetObject(L, 2, typeof(object));
                    
                    Honor.Runtime.LuaProtocolRunTime.ReceiveSuccess( _cmd, _content );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReceiveFail_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    object _content = translator.GetObject(L, 2, typeof(object));
                    
                    Honor.Runtime.LuaProtocolRunTime.ReceiveFail( _cmd, _content );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BindPushMethod_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    Honor.Runtime.LuaProtocolPushFun _bind = translator.GetDelegate<Honor.Runtime.LuaProtocolPushFun>(L, 2);
                    
                    Honor.Runtime.LuaProtocolRunTime.BindPushMethod( _cmd, _bind );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemovePushMethod_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    
                    Honor.Runtime.LuaProtocolRunTime.RemovePushMethod( _cmd );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CallPushMethod_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    object _content = translator.GetObject(L, 2, typeof(object));
                    
                    Honor.Runtime.LuaProtocolRunTime.CallPushMethod( _cmd, _content );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NotifyLua_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    object _content = translator.GetObject(L, 2, typeof(object));
                    int _state = LuaAPI.xlua_tointeger(L, 3);
                    
                    Honor.Runtime.LuaProtocolRunTime.NotifyLua( _cmd, _content, _state );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SyncCall_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    object _content = translator.GetObject(L, 2, typeof(object));
                    
                        object gen_ret = Honor.Runtime.LuaProtocolRunTime.SyncCall( _cmd, _content );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Dispatcher(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Honor.Runtime.LuaProtocolRunTime.Dispatcher);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaNotifyHandle(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Honor.Runtime.LuaProtocolRunTime.LuaNotifyHandle);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaSyncHandle(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Honor.Runtime.LuaProtocolRunTime.LuaSyncHandle);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaNotifyHandle(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Honor.Runtime.LuaProtocolRunTime.LuaNotifyHandle = translator.GetDelegate<Honor.Runtime.LuaProtocolSendFun>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaSyncHandle(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Honor.Runtime.LuaProtocolRunTime.LuaSyncHandle = translator.GetDelegate<Honor.Runtime.LuaProtocolSyncFun>(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
