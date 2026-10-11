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
    public class HonorRuntimeProcedurePreloadWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.ProcedurePreload);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 1, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnInit", _m_OnInit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEnter", _m_OnEnter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnUpdate", _m_OnUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnLeave", _m_OnLeave);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsLaunch", _m_IsLaunch);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddStep", _m_AddStep);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "StartLoading", _g_get_StartLoading);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "StartLoading", _s_set_StartLoading);
            
			
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
					
					Honor.Runtime.ProcedurePreload gen_ret = new Honor.Runtime.ProcedurePreload();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.ProcedurePreload constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnInit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedurePreload gen_to_be_invoked = (Honor.Runtime.ProcedurePreload)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_OnEnter(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedurePreload gen_to_be_invoked = (Honor.Runtime.ProcedurePreload)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.ProcedurePreload gen_to_be_invoked = (Honor.Runtime.ProcedurePreload)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.ProcedurePreload gen_to_be_invoked = (Honor.Runtime.ProcedurePreload)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_IsLaunch(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedurePreload gen_to_be_invoked = (Honor.Runtime.ProcedurePreload)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.IsLaunch(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddStep(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedurePreload gen_to_be_invoked = (Honor.Runtime.ProcedurePreload)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<System.Action>(L, 2)) 
                {
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 2);
                    
                    gen_to_be_invoked.AddStep( _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action>(L, 3)) 
                {
                    string _description = LuaAPI.lua_tostring(L, 2);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 3);
                    
                    gen_to_be_invoked.AddStep( _description, _callback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Func<bool>>(L, 3)) 
                {
                    string _description = LuaAPI.lua_tostring(L, 2);
                    System.Func<bool> _callback = translator.GetDelegate<System.Func<bool>>(L, 3);
                    
                    gen_to_be_invoked.AddStep( _description, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.ProcedurePreload.AddStep!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StartLoading(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedurePreload gen_to_be_invoked = (Honor.Runtime.ProcedurePreload)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.StartLoading);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_StartLoading(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedurePreload gen_to_be_invoked = (Honor.Runtime.ProcedurePreload)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.StartLoading = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
