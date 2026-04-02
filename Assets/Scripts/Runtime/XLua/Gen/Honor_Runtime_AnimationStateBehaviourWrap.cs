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
    public class HonorRuntimeAnimationStateBehaviourWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.AnimationStateBehaviour);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnStateEnter", _m_OnStateEnter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnStateUpdate", _m_OnStateUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnStateExit", _m_OnStateExit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnStateMove", _m_OnStateMove);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnStateIK", _m_OnStateIK);
			
			
			
			
			
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
					
					Honor.Runtime.AnimationStateBehaviour gen_ret = new Honor.Runtime.AnimationStateBehaviour();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AnimationStateBehaviour constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnStateEnter(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AnimationStateBehaviour gen_to_be_invoked = (Honor.Runtime.AnimationStateBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 2, typeof(UnityEngine.Animator));
                    UnityEngine.AnimatorStateInfo _stateInfo;translator.Get(L, 3, out _stateInfo);
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.OnStateEnter( _animator, _stateInfo, _layerIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnStateUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AnimationStateBehaviour gen_to_be_invoked = (Honor.Runtime.AnimationStateBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 2, typeof(UnityEngine.Animator));
                    UnityEngine.AnimatorStateInfo _stateInfo;translator.Get(L, 3, out _stateInfo);
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.OnStateUpdate( _animator, _stateInfo, _layerIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnStateExit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AnimationStateBehaviour gen_to_be_invoked = (Honor.Runtime.AnimationStateBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 2, typeof(UnityEngine.Animator));
                    UnityEngine.AnimatorStateInfo _stateInfo;translator.Get(L, 3, out _stateInfo);
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.OnStateExit( _animator, _stateInfo, _layerIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnStateMove(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AnimationStateBehaviour gen_to_be_invoked = (Honor.Runtime.AnimationStateBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 2, typeof(UnityEngine.Animator));
                    UnityEngine.AnimatorStateInfo _stateInfo;translator.Get(L, 3, out _stateInfo);
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.OnStateMove( _animator, _stateInfo, _layerIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnStateIK(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AnimationStateBehaviour gen_to_be_invoked = (Honor.Runtime.AnimationStateBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 2, typeof(UnityEngine.Animator));
                    UnityEngine.AnimatorStateInfo _stateInfo;translator.Get(L, 3, out _stateInfo);
                    int _layerIndex = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.OnStateIK( _animator, _stateInfo, _layerIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
