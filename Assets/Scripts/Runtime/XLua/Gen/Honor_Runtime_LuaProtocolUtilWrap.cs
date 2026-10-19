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
    public class HonorRuntimeLuaProtocolUtilWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.LuaProtocolUtil);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 5, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "IsValidCmd", _m_IsValidCmd_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BuildData", _m_BuildData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "BroadcastToLua", _m_BroadcastToLua_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SafeInvoke", _m_SafeInvoke_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.LuaProtocolUtil does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsValidCmd_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    
                        bool gen_ret = Honor.Runtime.LuaProtocolUtil.IsValidCmd( _cmd );
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
                    
                        Honor.Runtime.LuaProtocolDataBase gen_ret = Honor.Runtime.LuaProtocolUtil.BuildData( _cmd );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BroadcastToLua_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _cmd = LuaAPI.xlua_tointeger(L, 1);
                    object _content = translator.GetObject(L, 2, typeof(object));
                    
                    Honor.Runtime.LuaProtocolUtil.BroadcastToLua( _cmd, _content );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SafeInvoke_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.LuaProtocolPushFun>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<object>(L, 3)) 
                {
                    Honor.Runtime.LuaProtocolPushFun _handler = translator.GetDelegate<Honor.Runtime.LuaProtocolPushFun>(L, 1);
                    int _cmd = LuaAPI.xlua_tointeger(L, 2);
                    object _content = translator.GetObject(L, 3, typeof(object));
                    
                    Honor.Runtime.LuaProtocolUtil.SafeInvoke( _handler, _cmd, _content );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<Honor.Runtime.LuaProtocolSendFun>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<object>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    Honor.Runtime.LuaProtocolSendFun _handler = translator.GetDelegate<Honor.Runtime.LuaProtocolSendFun>(L, 1);
                    int _cmd = LuaAPI.xlua_tointeger(L, 2);
                    object _content = translator.GetObject(L, 3, typeof(object));
                    int _state = LuaAPI.xlua_tointeger(L, 4);
                    
                    Honor.Runtime.LuaProtocolUtil.SafeInvoke( _handler, _cmd, _content, _state );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.LuaProtocolUtil.SafeInvoke!");
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
