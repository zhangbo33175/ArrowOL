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
    public class HonorRuntimeDebuggerComponentConsoleWindowWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.DebuggerComponent.ConsoleWindow);
			Utils.BeginObjectRegister(type, L, translator, 0, 27, 4, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Initialize", _m_Initialize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitLogShowStateFromSaveData", _m_InitLogShowStateFromSaveData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Shutdown", _m_Shutdown);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SaveLogFile", _m_SaveLogFile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEnter", _m_OnEnter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnLeave", _m_OnLeave);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnUpdate", _m_OnUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CalcLogHeight", _m_CalcLogHeight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdateLogShowState", _m_UpdateLogShowState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SaveLogShowSetting", _m_SaveLogShowSetting);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CanShowLogBySearchInfo", _m_CanShowLogBySearchInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ModefyShowLogsAndResetTotalHeight", _m_ModefyShowLogsAndResetTotalHeight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLogCountInfo", _m_GetLogCountInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartSearch", _m_StartSearch);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetSearch", _m_ResetSearch);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDrawSearch", _m_OnDrawSearch);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDrawClearLog", _m_OnDrawClearLog);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDrawUploadLog", _m_OnDrawUploadLog);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDrawLockScroll", _m_OnDrawLockScroll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDrawSpecifiedLog", _m_OnDrawSpecifiedLog);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDrawLogs", _m_OnDrawLogs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CalcScrollValue", _m_CalcScrollValue);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDraw", _m_OnDraw);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSelectLogString", _m_GetSelectLogString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnStartDown", _m_OnStartDown);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEndDown", _m_OnEndDown);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRecentLogs", _m_GetRecentLogs);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectedNode", _g_get_SelectedNode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LockScroll", _g_get_LockScroll);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaxLine", _g_get_MaxLine);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_InShowLogs", _g_get_m_InShowLogs);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "SelectedNode", _s_set_SelectedNode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LockScroll", _s_set_LockScroll);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MaxLine", _s_set_MaxLine);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_InShowLogs", _s_set_m_InShowLogs);
            
			
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
					
					Honor.Runtime.DebuggerComponent.ConsoleWindow gen_ret = new Honor.Runtime.DebuggerComponent.ConsoleWindow();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.DebuggerComponent.ConsoleWindow constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Initialize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.DebuggerComponent _rootDebug = (Honor.Runtime.DebuggerComponent)translator.GetObject(L, 2, typeof(Honor.Runtime.DebuggerComponent));
                    object[] _args = translator.GetParams<object>(L, 3);
                    
                    gen_to_be_invoked.Initialize( _rootDebug, _args );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitLogShowStateFromSaveData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitLogShowStateFromSaveData(  );
                    
                    
                    
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
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Shutdown(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SaveLogFile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.SaveLogFile(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnEnter(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnEnter(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnLeave(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnLeave(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _elapseSeconds = (float)LuaAPI.lua_tonumber(L, 2);
                    float _realElapseSeconds = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.OnUpdate( _elapseSeconds, _realElapseSeconds );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CalcLogHeight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _message = LuaAPI.lua_tostring(L, 2);
                    float _baseOnWidth = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        float gen_ret = gen_to_be_invoked.CalcLogHeight( _message, _baseOnWidth );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateLogShowState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.LogType _checkType;translator.Get(L, 2, out _checkType);
                    
                    gen_to_be_invoked.UpdateLogShowState( _checkType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SaveLogShowSetting(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _logType = LuaAPI.lua_tostring(L, 2);
                    bool _showState = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.SaveLogShowSetting( _logType, _showState );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CanShowLogBySearchInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.DebuggerComponent.LogNode _checkLog = (Honor.Runtime.DebuggerComponent.LogNode)translator.GetObject(L, 2, typeof(Honor.Runtime.DebuggerComponent.LogNode));
                    
                        bool gen_ret = gen_to_be_invoked.CanShowLogBySearchInfo( _checkLog );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ModefyShowLogsAndResetTotalHeight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ModefyShowLogsAndResetTotalHeight(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLogCountInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.LogType _curLogType;translator.Get(L, 2, out _curLogType);
                    
                        string gen_ret = gen_to_be_invoked.GetLogCountInfo( _curLogType );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartSearch(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StartSearch(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetSearch(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetSearch(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDrawSearch(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.DebuggerComponent _debugComponent = (Honor.Runtime.DebuggerComponent)translator.GetObject(L, 2, typeof(Honor.Runtime.DebuggerComponent));
                    float _offsetY = (float)LuaAPI.lua_tonumber(L, 3);
                    float _showWidth = (float)LuaAPI.lua_tonumber(L, 4);
                    float _Inv = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.OnDrawSearch( _debugComponent, _offsetY, _showWidth, _Inv );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDrawClearLog(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.DebuggerComponent _debugComponent = (Honor.Runtime.DebuggerComponent)translator.GetObject(L, 2, typeof(Honor.Runtime.DebuggerComponent));
                    float _offsetY = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.OnDrawClearLog( _debugComponent, _offsetY );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDrawUploadLog(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.DebuggerComponent _debugComponent = (Honor.Runtime.DebuggerComponent)translator.GetObject(L, 2, typeof(Honor.Runtime.DebuggerComponent));
                    float _offsetY = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.OnDrawUploadLog( _debugComponent, _offsetY );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDrawLockScroll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.DebuggerComponent _debugComponent = (Honor.Runtime.DebuggerComponent)translator.GetObject(L, 2, typeof(Honor.Runtime.DebuggerComponent));
                    float _offsetY = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.OnDrawLockScroll( _debugComponent, _offsetY );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDrawSpecifiedLog(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.DebuggerComponent _debugComponent = (Honor.Runtime.DebuggerComponent)translator.GetObject(L, 2, typeof(Honor.Runtime.DebuggerComponent));
                    float _offsetY = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.LogType _showType;translator.Get(L, 4, out _showType);
                    
                    gen_to_be_invoked.OnDrawSpecifiedLog( _debugComponent, _offsetY, _showType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDrawLogs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.DebuggerComponent _debugComponent = (Honor.Runtime.DebuggerComponent)translator.GetObject(L, 2, typeof(Honor.Runtime.DebuggerComponent));
                    float _offsetY = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.OnDrawLogs( _debugComponent, _offsetY );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CalcScrollValue(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CalcScrollValue(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDraw(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.DebuggerComponent _debugComponent = (Honor.Runtime.DebuggerComponent)translator.GetObject(L, 2, typeof(Honor.Runtime.DebuggerComponent));
                    float _ScrollOffsetY = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.OnDraw( _debugComponent, _ScrollOffsetY );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSelectLogString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        string gen_ret = gen_to_be_invoked.GetSelectLogString(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnStartDown(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _downPoint;translator.Get(L, 2, out _downPoint);
                    
                    gen_to_be_invoked.OnStartDown( _downPoint );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnEndDown(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _downPointY = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.OnEndDown( _downPointY );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRecentLogs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<System.Collections.Generic.List<Honor.Runtime.DebuggerComponent.LogNode>>(L, 2)) 
                {
                    System.Collections.Generic.List<Honor.Runtime.DebuggerComponent.LogNode> _results = (System.Collections.Generic.List<Honor.Runtime.DebuggerComponent.LogNode>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<Honor.Runtime.DebuggerComponent.LogNode>));
                    
                    gen_to_be_invoked.GetRecentLogs( _results );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<System.Collections.Generic.List<Honor.Runtime.DebuggerComponent.LogNode>>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    System.Collections.Generic.List<Honor.Runtime.DebuggerComponent.LogNode> _results = (System.Collections.Generic.List<Honor.Runtime.DebuggerComponent.LogNode>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<Honor.Runtime.DebuggerComponent.LogNode>));
                    int _count = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.GetRecentLogs( _results, _count );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.DebuggerComponent.ConsoleWindow.GetRecentLogs!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectedNode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SelectedNode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LockScroll(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.LockScroll);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaxLine(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MaxLine);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_InShowLogs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_InShowLogs);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SelectedNode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SelectedNode = (Honor.Runtime.DebuggerComponent.LogNode)translator.GetObject(L, 2, typeof(Honor.Runtime.DebuggerComponent.LogNode));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LockScroll(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LockScroll = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MaxLine(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MaxLine = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_InShowLogs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent.ConsoleWindow gen_to_be_invoked = (Honor.Runtime.DebuggerComponent.ConsoleWindow)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_InShowLogs = (System.Collections.Generic.List<int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<int>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
