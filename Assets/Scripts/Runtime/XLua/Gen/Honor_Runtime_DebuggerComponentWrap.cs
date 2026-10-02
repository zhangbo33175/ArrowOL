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
    public class HonorRuntimeDebuggerComponentWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.DebuggerComponent);
			Utils.BeginObjectRegister(type, L, translator, 0, 16, 34, 25);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetActiveWindow", _m_SetActiveWindow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RefreshBlockRect", _m_RefreshBlockRect);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RegisterDebuggerWindow", _m_RegisterDebuggerWindow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetDebuggerWindow", _m_GetDebuggerWindow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SelectDebuggerWindow", _m_SelectDebuggerWindow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetLayout", _m_ResetLayout);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRecentLogs", _m_GetRecentLogs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ControlInput", _m_ControlInput);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsDebugWinActive", _m_IsDebugWinActive);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBeginDrag", _m_OnBeginDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDrag", _m_OnDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDrop", _m_OnDrop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEndDrag", _m_OnEndDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnInitializePotentialDrag", _m_OnInitializePotentialDrag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPointerDown", _m_OnPointerDown);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPointerUp", _m_OnPointerUp);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "InfoColor", _g_get_InfoColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WarningColor", _g_get_WarningColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ErrorColor", _g_get_ErrorColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FatalColor", _g_get_FatalColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WinModel", _g_get_WinModel);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelLogString", _g_get_SelLogString);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelLogStackTrack", _g_get_SelLogStackTrack);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WindowInDrag", _g_get_WindowInDrag);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FullWindowInDrag", _g_get_FullWindowInDrag);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FullWindowDownPos", _g_get_FullWindowDownPos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "StayBorder", _g_get_StayBorder);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IdleState", _g_get_IdleState);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ActiveWindow", _g_get_ActiveWindow);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ShowFullWindow", _g_get_ShowFullWindow);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IconRect", _g_get_IconRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WindowRect", _g_get_WindowRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WindowScale", _g_get_WindowScale);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CustomButtonHeight", _g_get_CustomButtonHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CustomButtonFontSize", _g_get_CustomButtonFontSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CustomScrollBarValue", _g_get_CustomScrollBarValue);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentFPS", _g_get_CurrentFPS);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FullWinMaxWidth", _g_get_FullWinMaxWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FullWinUIBorder", _g_get_FullWinUIBorder);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FullWinMaxHeight", _g_get_FullWinMaxHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FullWinTitleHeight", _g_get_FullWinTitleHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LogScrollRect", _g_get_LogScrollRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LogScrollBarRect", _g_get_LogScrollBarRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LogScrollBarTop", _g_get_LogScrollBarTop);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LogScrollBarBottom", _g_get_LogScrollBarBottom);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LogScrollBarHeight", _g_get_LogScrollBarHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Portrail", _g_get_Portrail);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Inertia", _g_get_Inertia);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Velocity", _g_get_Velocity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LastScrollOffsetY", _g_get_LastScrollOffsetY);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "WinModel", _s_set_WinModel);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SelLogString", _s_set_SelLogString);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SelLogStackTrack", _s_set_SelLogStackTrack);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "StayBorder", _s_set_StayBorder);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IdleState", _s_set_IdleState);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ActiveWindow", _s_set_ActiveWindow);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ShowFullWindow", _s_set_ShowFullWindow);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IconRect", _s_set_IconRect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WindowRect", _s_set_WindowRect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WindowScale", _s_set_WindowScale);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CustomButtonHeight", _s_set_CustomButtonHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CustomButtonFontSize", _s_set_CustomButtonFontSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CustomScrollBarValue", _s_set_CustomScrollBarValue);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FullWinMaxWidth", _s_set_FullWinMaxWidth);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FullWinUIBorder", _s_set_FullWinUIBorder);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FullWinMaxHeight", _s_set_FullWinMaxHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LogScrollRect", _s_set_LogScrollRect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LogScrollBarRect", _s_set_LogScrollBarRect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LogScrollBarTop", _s_set_LogScrollBarTop);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LogScrollBarBottom", _s_set_LogScrollBarBottom);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LogScrollBarHeight", _s_set_LogScrollBarHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Portrail", _s_set_Portrail);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Inertia", _s_set_Inertia);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Velocity", _s_set_Velocity);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LastScrollOffsetY", _s_set_LastScrollOffsetY);
            
			
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
					
					Honor.Runtime.DebuggerComponent gen_ret = new Honor.Runtime.DebuggerComponent();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.DebuggerComponent constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetActiveWindow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.SetActiveWindow(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshBlockRect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Rect _newRect;translator.Get(L, 2, out _newRect);
                    
                    gen_to_be_invoked.RefreshBlockRect( _newRect );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterDebuggerWindow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.DebuggerComponent _debugComponent = (Honor.Runtime.DebuggerComponent)translator.GetObject(L, 2, typeof(Honor.Runtime.DebuggerComponent));
                    string _path = LuaAPI.lua_tostring(L, 3);
                    Honor.Runtime.IDebuggerWindow _debuggerWindow = (Honor.Runtime.IDebuggerWindow)translator.GetObject(L, 4, typeof(Honor.Runtime.IDebuggerWindow));
                    object[] _args = translator.GetParams<object>(L, 5);
                    
                    gen_to_be_invoked.RegisterDebuggerWindow( _debugComponent, _path, _debuggerWindow, _args );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDebuggerWindow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                        Honor.Runtime.IDebuggerWindow gen_ret = gen_to_be_invoked.GetDebuggerWindow( _path );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SelectDebuggerWindow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.SelectDebuggerWindow( _path );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetLayout(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetLayout(  );
                    
                    
                    
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
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.DebuggerComponent.GetRecentLogs!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ControlInput(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ControlInput(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsDebugWinActive(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.IsDebugWinActive(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnBeginDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnBeginDrag( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnDrag( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDrop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnDrop( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnEndDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnEndDrag( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnInitializePotentialDrag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnInitializePotentialDrag( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnPointerDown(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnPointerDown( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnPointerUp(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.EventSystems.PointerEventData _eventData = (UnityEngine.EventSystems.PointerEventData)translator.GetObject(L, 2, typeof(UnityEngine.EventSystems.PointerEventData));
                    
                    gen_to_be_invoked.OnPointerUp( _eventData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InfoColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.InfoColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WarningColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.WarningColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ErrorColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ErrorColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FatalColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.FatalColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WinModel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                translator.PushHonorRuntimeGameDefinitionsDebugWindowModel(L, gen_to_be_invoked.WinModel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelLogString(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.SelLogString);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelLogStackTrack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.SelLogStackTrack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WindowInDrag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.WindowInDrag);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FullWindowInDrag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.FullWindowInDrag);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FullWindowDownPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.FullWindowDownPos);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StayBorder(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.StayBorder);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IdleState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IdleState);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ActiveWindow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.ActiveWindow);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ShowFullWindow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.ShowFullWindow);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IconRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.IconRect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WindowRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.WindowRect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WindowScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.WindowScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CustomButtonHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.CustomButtonHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CustomButtonFontSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.CustomButtonFontSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CustomScrollBarValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.CustomScrollBarValue);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentFPS(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.CurrentFPS);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FullWinMaxWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.FullWinMaxWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FullWinUIBorder(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.FullWinUIBorder);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FullWinMaxHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.FullWinMaxHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FullWinTitleHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.FullWinTitleHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LogScrollRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LogScrollRect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LogScrollBarRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LogScrollBarRect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LogScrollBarTop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.LogScrollBarTop);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LogScrollBarBottom(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.LogScrollBarBottom);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LogScrollBarHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.LogScrollBarHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Portrail(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.Portrail);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Inertia(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.Inertia);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Velocity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.Velocity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LastScrollOffsetY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.LastScrollOffsetY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WinModel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                Honor.Runtime.GameDefinitions.DebugWindowModel gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.WinModel = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SelLogString(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SelLogString = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SelLogStackTrack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SelLogStackTrack = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_StayBorder(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.StayBorder = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IdleState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IdleState = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ActiveWindow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ActiveWindow = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ShowFullWindow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ShowFullWindow = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IconRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                UnityEngine.Rect gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.IconRect = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WindowRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                UnityEngine.Rect gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.WindowRect = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WindowScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.WindowScale = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CustomButtonHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CustomButtonHeight = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CustomButtonFontSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CustomButtonFontSize = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CustomScrollBarValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CustomScrollBarValue = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FullWinMaxWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FullWinMaxWidth = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FullWinUIBorder(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FullWinUIBorder = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FullWinMaxHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FullWinMaxHeight = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LogScrollRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                UnityEngine.Rect gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.LogScrollRect = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LogScrollBarRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                UnityEngine.Rect gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.LogScrollBarRect = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LogScrollBarTop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LogScrollBarTop = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LogScrollBarBottom(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LogScrollBarBottom = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LogScrollBarHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LogScrollBarHeight = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Portrail(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Portrail = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Inertia(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Inertia = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Velocity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.Velocity = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LastScrollOffsetY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.DebuggerComponent gen_to_be_invoked = (Honor.Runtime.DebuggerComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LastScrollOffsetY = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
