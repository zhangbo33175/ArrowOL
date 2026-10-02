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
    public class HonorRuntimeDebuggerComponentLogNodeWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.DebuggerComponent.LogNode);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 8, 7);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RefreshCount", _m_RefreshCount);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "LogFrameCount", _g_get_LogFrameCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LogTime", _g_get_LogTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LogType", _g_get_LogType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LogMessage", _g_get_LogMessage);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "StackTrack", _g_get_StackTrack);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LogOrderY", _g_get_LogOrderY);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LogRect", _g_get_LogRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LogIndex", _g_get_LogIndex);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "LogTime", _s_set_LogTime);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LogType", _s_set_LogType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LogMessage", _s_set_LogMessage);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "StackTrack", _s_set_StackTrack);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LogOrderY", _s_set_LogOrderY);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LogRect", _s_set_LogRect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LogIndex", _s_set_LogIndex);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 5, 4, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Create", _m_Create_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateThreadedNode", _m_CreateThreadedNode_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetLogStringColor", _m_GetLogStringColor_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Reset", _m_Reset_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "InfoCount", _g_get_InfoCount);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "WarningCount", _g_get_WarningCount);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ErrorCount", _g_get_ErrorCount);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "FatalCount", _g_get_FatalCount);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Honor.Runtime.DebuggerComponent.LogNode gen_ret = new Honor.Runtime.DebuggerComponent.LogNode();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.DebuggerComponent.LogNode constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Create_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Honor.Runtime.DebuggerComponent _debugComponent = (Honor.Runtime.DebuggerComponent)translator.GetObject(L, 1, typeof(Honor.Runtime.DebuggerComponent));
                    UnityEngine.LogType _logType;translator.Get(L, 2, out _logType);
                    string _logMessage = LuaAPI.lua_tostring(L, 3);
                    string _stackTrack = LuaAPI.lua_tostring(L, 4);
                    ulong _logIndex = LuaAPI.lua_touint64(L, 5);
                    
                        Honor.Runtime.DebuggerComponent.LogNode gen_ret = Honor.Runtime.DebuggerComponent.LogNode.Create( _debugComponent, _logType, _logMessage, _stackTrack, _logIndex );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateThreadedNode_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Honor.Runtime.DebuggerComponent _debugComponent = (Honor.Runtime.DebuggerComponent)translator.GetObject(L, 1, typeof(Honor.Runtime.DebuggerComponent));
                    UnityEngine.LogType _logType;translator.Get(L, 2, out _logType);
                    string _logMessage = LuaAPI.lua_tostring(L, 3);
                    string _stackTrack = LuaAPI.lua_tostring(L, 4);
                    int _frameCount = LuaAPI.xlua_tointeger(L, 5);
                    ulong _logIndex = LuaAPI.lua_touint64(L, 6);
                    
                        Honor.Runtime.DebuggerComponent.LogNode gen_ret = Honor.Runtime.DebuggerComponent.LogNode.CreateThreadedNode( _debugComponent, _logType, _logMessage, _stackTrack, _frameCount, _logIndex );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.RefreshCount(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLogStringColor_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.DebuggerComponent>(L, 1)&& translator.Assignable<UnityEngine.LogType>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    Honor.Runtime.DebuggerComponent _debugComponent = (Honor.Runtime.DebuggerComponent)translator.GetObject(L, 1, typeof(Honor.Runtime.DebuggerComponent));
                    UnityEngine.LogType _logType;translator.Get(L, 2, out _logType);
                    bool _idleModel = LuaAPI.lua_toboolean(L, 3);
                    
                        UnityEngine.Color32 gen_ret = Honor.Runtime.DebuggerComponent.LogNode.GetLogStringColor( _debugComponent, _logType, _idleModel );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.DebuggerComponent>(L, 1)&& translator.Assignable<UnityEngine.LogType>(L, 2)) 
                {
                    Honor.Runtime.DebuggerComponent _debugComponent = (Honor.Runtime.DebuggerComponent)translator.GetObject(L, 1, typeof(Honor.Runtime.DebuggerComponent));
                    UnityEngine.LogType _logType;translator.Get(L, 2, out _logType);
                    
                        UnityEngine.Color32 gen_ret = Honor.Runtime.DebuggerComponent.LogNode.GetLogStringColor( _debugComponent, _logType );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.DebuggerComponent.LogNode.GetLogStringColor!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Reset_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Honor.Runtime.DebuggerComponent.LogNode.Reset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LogFrameCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.LogFrameCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InfoCount(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, Honor.Runtime.DebuggerComponent.LogNode.InfoCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WarningCount(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, Honor.Runtime.DebuggerComponent.LogNode.WarningCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ErrorCount(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, Honor.Runtime.DebuggerComponent.LogNode.ErrorCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FatalCount(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, Honor.Runtime.DebuggerComponent.LogNode.FatalCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LogTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.LogTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LogType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LogType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LogMessage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.LogMessage);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StackTrack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.StackTrack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LogOrderY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.LogOrderY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LogRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LogRect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LogIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushuint64(L, gen_to_be_invoked.LogIndex);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LogTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LogTime = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LogType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                UnityEngine.LogType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.LogType = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LogMessage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LogMessage = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_StackTrack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.StackTrack = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LogOrderY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LogOrderY = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LogRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                UnityEngine.Rect gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.LogRect = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LogIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.LogNode gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.LogNode)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LogIndex = LuaAPI.lua_touint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
