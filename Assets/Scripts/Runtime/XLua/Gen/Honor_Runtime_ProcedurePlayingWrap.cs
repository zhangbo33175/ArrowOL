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
    public class HonorRuntimeProcedurePlayingWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.ProcedurePlaying);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnInit", _m_OnInit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEnter", _m_OnEnter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnUpdate", _m_OnUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnLeave", _m_OnLeave);
			
			
			
			
			
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
					
					Honor.Runtime.ProcedurePlaying gen_ret = new Honor.Runtime.ProcedurePlaying();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.ProcedurePlaying constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnInit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedurePlaying gen_to_be_invoked = (Honor.Runtime.ProcedurePlaying)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.ProcedurePlaying gen_to_be_invoked = (Honor.Runtime.ProcedurePlaying)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.ProcedurePlaying gen_to_be_invoked = (Honor.Runtime.ProcedurePlaying)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.ProcedurePlaying gen_to_be_invoked = (Honor.Runtime.ProcedurePlaying)translator.FastGetCSObj(L, 1);
            
            
                
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
        
        
        
        
        
        
		
		
		
		
    }
}
