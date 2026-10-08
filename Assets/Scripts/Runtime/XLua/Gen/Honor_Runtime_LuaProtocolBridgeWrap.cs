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
    public class HonorRuntimeLuaProtocolBridgeWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.LuaProtocolBridge);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 7, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "RegisterHandler", _m_RegisterHandler_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnregisterHandler", _m_UnregisterHandler_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ClearHandlers", _m_ClearHandlers_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SendFuncMessage", _m_SendFuncMessage_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReceiveFuncMessage", _m_ReceiveFuncMessage_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BuildData", _m_BuildData_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.LuaProtocolBridge does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterHandler_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    Honor.Runtime.LuaProtocolPushFun _handler = translator.GetDelegate<Honor.Runtime.LuaProtocolPushFun>(L, 2);
                    
                    Honor.Runtime.LuaProtocolBridge.RegisterHandler( _cmd, _handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnregisterHandler_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    
                    Honor.Runtime.LuaProtocolBridge.UnregisterHandler( _cmd );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearHandlers_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Honor.Runtime.LuaProtocolBridge.ClearHandlers(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendFuncMessage_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    object _content = translator.GetObject(L, 2, typeof(object));
                    
                        bool gen_ret = Honor.Runtime.LuaProtocolBridge.SendFuncMessage( _cmd, _content );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReceiveFuncMessage_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    object _content = translator.GetObject(L, 2, typeof(object));
                    
                        bool gen_ret = Honor.Runtime.LuaProtocolBridge.ReceiveFuncMessage( _cmd, _content );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BuildData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    
                        Honor.Runtime.LuaProtocolDataBase gen_ret = Honor.Runtime.LuaProtocolBridge.BuildData( _cmd );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
