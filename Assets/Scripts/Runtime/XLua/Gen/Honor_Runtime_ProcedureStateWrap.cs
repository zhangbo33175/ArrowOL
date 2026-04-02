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
    public class HonorRuntimeProcedureStateWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.ProcedureState);
			Utils.BeginObjectRegister(type, L, translator, 0, 12, 6, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitLuaBindings", _m_InitLuaBindings);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnInit", _m_OnInit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDestroy", _m_OnDestroy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEnter", _m_OnEnter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnUpdate", _m_OnUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnLeave", _m_OnLeave);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeState", _m_ChangeState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnProcedureTransitionEnterOverEventCallback", _m_OnProcedureTransitionEnterOverEventCallback);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnProcedureTransitionExitOverEventCallback", _m_OnProcedureTransitionExitOverEventCallback);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PrepareToNextProcedure", _m_PrepareToNextProcedure);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowProcedureTransitionEnter", _m_ShowProcedureTransitionEnter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowProcedureTransitionExit", _m_ShowProcedureTransitionExit);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Name", _g_get_Name);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PrepareArgsFromChanging", _g_get_PrepareArgsFromChanging);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ArgsFromChanging", _g_get_ArgsFromChanging);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EnterOver", _g_get_EnterOver);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RemoveAllContentsOnProcedureTransition", _g_get_RemoveAllContentsOnProcedureTransition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "lua", _g_get_lua);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "PrepareArgsFromChanging", _s_set_PrepareArgsFromChanging);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ArgsFromChanging", _s_set_ArgsFromChanging);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EnterOver", _s_set_EnterOver);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RemoveAllContentsOnProcedureTransition", _s_set_RemoveAllContentsOnProcedureTransition);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 1);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "IsReset", _g_get_IsReset);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "IsReset", _s_set_IsReset);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.ProcedureState does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitLuaBindings(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _luaScriptName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.InitLuaBindings( _luaScriptName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnInit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent> _ownerMachine = (Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>)translator.GetObject(L, 2, typeof(Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>));
                    
                    gen_to_be_invoked.OnInit( _ownerMachine );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDestroy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent> _ownerMachine = (Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>)translator.GetObject(L, 2, typeof(Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>));
                    
                    gen_to_be_invoked.OnDestroy( _ownerMachine );
                    
                    
                    
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
            
            
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent> _ownerMachine = (Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>)translator.GetObject(L, 2, typeof(Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>));
                    
                    gen_to_be_invoked.OnEnter( _ownerMachine );
                    
                    
                    
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
            
            
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent> _ownerMachine = (Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>)translator.GetObject(L, 2, typeof(Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>));
                    
                    gen_to_be_invoked.OnUpdate( _ownerMachine );
                    
                    
                    
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
            
            
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent> _ownerMachine = (Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>)translator.GetObject(L, 2, typeof(Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>));
                    bool _isShutdown = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.OnLeave( _ownerMachine, _isShutdown );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>>(L, 2)&& translator.Assignable<System.Type>(L, 3)) 
                {
                    Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent> _ownerMachine = (Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>)translator.GetObject(L, 2, typeof(Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>));
                    System.Type _stateType = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    
                    gen_to_be_invoked.ChangeState( _ownerMachine, _stateType );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent> _ownerMachine = (Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>)translator.GetObject(L, 2, typeof(Honor.Runtime.StateMachine<Honor.Runtime.ProcedureComponent>));
                    string _stateName = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.ChangeState( _ownerMachine, _stateName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.ProcedureState.ChangeState!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnProcedureTransitionEnterOverEventCallback(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    object _userData = translator.GetObject(L, 3, typeof(object));
                    Honor.Runtime.EventParams _e = (Honor.Runtime.EventParams)translator.GetObject(L, 4, typeof(Honor.Runtime.EventParams));
                    
                    gen_to_be_invoked.OnProcedureTransitionEnterOverEventCallback( _sender, _userData, _e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnProcedureTransitionExitOverEventCallback(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    object _userData = translator.GetObject(L, 3, typeof(object));
                    Honor.Runtime.EventParams _e = (Honor.Runtime.EventParams)translator.GetObject(L, 4, typeof(Honor.Runtime.EventParams));
                    
                    gen_to_be_invoked.OnProcedureTransitionExitOverEventCallback( _sender, _userData, _e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PrepareToNextProcedure(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<System.Type>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    System.Type _stateType = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    XLua.LuaTable _argsFromChanging = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.PrepareToNextProcedure( _stateType, _argsFromChanging );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<System.Type>(L, 2)) 
                {
                    System.Type _stateType = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                    gen_to_be_invoked.PrepareToNextProcedure( _stateType );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    XLua.LuaTable _argsFromChanging = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.PrepareToNextProcedure( _stateName, _argsFromChanging );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _stateName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.PrepareToNextProcedure( _stateName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.ProcedureState.PrepareToNextProcedure!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowProcedureTransitionEnter(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _forceOver = LuaAPI.lua_toboolean(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _blockRaycast = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.ShowProcedureTransitionEnter( _forceOver, _duration, _blockRaycast );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowProcedureTransitionExit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _forceOver = LuaAPI.lua_toboolean(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _blockRaycast = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.ShowProcedureTransitionExit( _forceOver, _duration, _blockRaycast );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Name(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Name);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PrepareArgsFromChanging(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PrepareArgsFromChanging);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ArgsFromChanging(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ArgsFromChanging);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EnterOver(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.EnterOver);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RemoveAllContentsOnProcedureTransition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.RemoveAllContentsOnProcedureTransition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_lua(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.lua);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsReset(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Honor.Runtime.ProcedureState.IsReset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PrepareArgsFromChanging(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PrepareArgsFromChanging = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ArgsFromChanging(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ArgsFromChanging = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EnterOver(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EnterOver = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RemoveAllContentsOnProcedureTransition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureState gen_to_be_invoked = (Honor.Runtime.ProcedureState)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RemoveAllContentsOnProcedureTransition = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsReset(RealStatePtr L)
        {
		    try {
                
			    Honor.Runtime.ProcedureState.IsReset = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
