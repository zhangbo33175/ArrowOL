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
    public class HonorRuntimeSceneCameraActorWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.SceneCameraActor);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 1, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitWithAnimation", _m_InitWithAnimation);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayAnimationToTarget", _m_PlayAnimationToTarget);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MoveTo", _m_MoveTo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RotateTo", _m_RotateTo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ScaleTo", _m_ScaleTo);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Camera", _g_get_Camera);
            
			
			
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
					
					Honor.Runtime.SceneCameraActor gen_ret = new Honor.Runtime.SceneCameraActor();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneCameraActor constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneCameraActor gen_to_be_invoked = (Honor.Runtime.SceneCameraActor)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _originalPosition;translator.Get(L, 2, out _originalPosition);
                    UnityEngine.Quaternion _originalRotation;translator.Get(L, 3, out _originalRotation);
                    float _originalSizeOrField = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    gen_to_be_invoked.Init( _originalPosition, _originalRotation, _originalSizeOrField );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitWithAnimation(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneCameraActor gen_to_be_invoked = (Honor.Runtime.SceneCameraActor)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 10&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Quaternion>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.Vector3>(L, 5)&& translator.Assignable<UnityEngine.Quaternion>(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 8)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 9)&& translator.Assignable<System.Action>(L, 10)) 
                {
                    UnityEngine.Vector3 _originalPosition;translator.Get(L, 2, out _originalPosition);
                    UnityEngine.Quaternion _originalRotation;translator.Get(L, 3, out _originalRotation);
                    float _originalSizeOrField = (float)LuaAPI.lua_tonumber(L, 4);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 5, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 6, out _targetRotation);
                    float _targetSizeOrField = (float)LuaAPI.lua_tonumber(L, 7);
                    float _targetDuration = (float)LuaAPI.lua_tonumber(L, 8);
                    bool _canInterruptByGestures = LuaAPI.lua_toboolean(L, 9);
                    System.Action _overCallback = translator.GetDelegate<System.Action>(L, 10);
                    
                    gen_to_be_invoked.InitWithAnimation( _originalPosition, _originalRotation, _originalSizeOrField, _targetPosition, _targetRotation, _targetSizeOrField, _targetDuration, _canInterruptByGestures, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 9&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Quaternion>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.Vector3>(L, 5)&& translator.Assignable<UnityEngine.Quaternion>(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 8)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 9)) 
                {
                    UnityEngine.Vector3 _originalPosition;translator.Get(L, 2, out _originalPosition);
                    UnityEngine.Quaternion _originalRotation;translator.Get(L, 3, out _originalRotation);
                    float _originalSizeOrField = (float)LuaAPI.lua_tonumber(L, 4);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 5, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 6, out _targetRotation);
                    float _targetSizeOrField = (float)LuaAPI.lua_tonumber(L, 7);
                    float _targetDuration = (float)LuaAPI.lua_tonumber(L, 8);
                    bool _canInterruptByGestures = LuaAPI.lua_toboolean(L, 9);
                    
                    gen_to_be_invoked.InitWithAnimation( _originalPosition, _originalRotation, _originalSizeOrField, _targetPosition, _targetRotation, _targetSizeOrField, _targetDuration, _canInterruptByGestures );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 8&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Quaternion>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.Vector3>(L, 5)&& translator.Assignable<UnityEngine.Quaternion>(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 8)) 
                {
                    UnityEngine.Vector3 _originalPosition;translator.Get(L, 2, out _originalPosition);
                    UnityEngine.Quaternion _originalRotation;translator.Get(L, 3, out _originalRotation);
                    float _originalSizeOrField = (float)LuaAPI.lua_tonumber(L, 4);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 5, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 6, out _targetRotation);
                    float _targetSizeOrField = (float)LuaAPI.lua_tonumber(L, 7);
                    float _targetDuration = (float)LuaAPI.lua_tonumber(L, 8);
                    
                    gen_to_be_invoked.InitWithAnimation( _originalPosition, _originalRotation, _originalSizeOrField, _targetPosition, _targetRotation, _targetSizeOrField, _targetDuration );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Quaternion>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<UnityEngine.Vector3>(L, 5)&& translator.Assignable<UnityEngine.Quaternion>(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    UnityEngine.Vector3 _originalPosition;translator.Get(L, 2, out _originalPosition);
                    UnityEngine.Quaternion _originalRotation;translator.Get(L, 3, out _originalRotation);
                    float _originalSizeOrField = (float)LuaAPI.lua_tonumber(L, 4);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 5, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 6, out _targetRotation);
                    float _targetSizeOrField = (float)LuaAPI.lua_tonumber(L, 7);
                    
                    gen_to_be_invoked.InitWithAnimation( _originalPosition, _originalRotation, _originalSizeOrField, _targetPosition, _targetRotation, _targetSizeOrField );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneCameraActor.InitWithAnimation!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayAnimationToTarget(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneCameraActor gen_to_be_invoked = (Honor.Runtime.SceneCameraActor)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)&& translator.Assignable<System.Action>(L, 7)) 
                {
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 3, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 4, out _targetRotation);
                    float _targetSizeOrField = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _canInterruptByGestures = LuaAPI.lua_toboolean(L, 6);
                    System.Action _overCallback = translator.GetDelegate<System.Action>(L, 7);
                    
                    gen_to_be_invoked.PlayAnimationToTarget( _duration, _targetPosition, _targetRotation, _targetSizeOrField, _canInterruptByGestures, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 3, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 4, out _targetRotation);
                    float _targetSizeOrField = (float)LuaAPI.lua_tonumber(L, 5);
                    bool _canInterruptByGestures = LuaAPI.lua_toboolean(L, 6);
                    
                    gen_to_be_invoked.PlayAnimationToTarget( _duration, _targetPosition, _targetRotation, _targetSizeOrField, _canInterruptByGestures );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& translator.Assignable<UnityEngine.Quaternion>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 3, out _targetPosition);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 4, out _targetRotation);
                    float _targetSizeOrField = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.PlayAnimationToTarget( _duration, _targetPosition, _targetRotation, _targetSizeOrField );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneCameraActor.PlayAnimationToTarget!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneCameraActor gen_to_be_invoked = (Honor.Runtime.SceneCameraActor)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 3, out _targetPosition);
                    bool _canInterruptByGestures = LuaAPI.lua_toboolean(L, 4);
                    System.Action _overCallback = translator.GetDelegate<System.Action>(L, 5);
                    
                    gen_to_be_invoked.MoveTo( _duration, _targetPosition, _canInterruptByGestures, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 3, out _targetPosition);
                    bool _canInterruptByGestures = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.MoveTo( _duration, _targetPosition, _canInterruptByGestures );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)) 
                {
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _targetPosition;translator.Get(L, 3, out _targetPosition);
                    
                    gen_to_be_invoked.MoveTo( _duration, _targetPosition );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneCameraActor.MoveTo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RotateTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneCameraActor gen_to_be_invoked = (Honor.Runtime.SceneCameraActor)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Quaternion>(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 3, out _targetRotation);
                    System.Action _overCallback = translator.GetDelegate<System.Action>(L, 4);
                    
                    gen_to_be_invoked.RotateTo( _duration, _targetRotation, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Quaternion>(L, 3)) 
                {
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Quaternion _targetRotation;translator.Get(L, 3, out _targetRotation);
                    
                    gen_to_be_invoked.RotateTo( _duration, _targetRotation );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneCameraActor.RotateTo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScaleTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SceneCameraActor gen_to_be_invoked = (Honor.Runtime.SceneCameraActor)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    float _targetOrthographicSizeOrField = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _canInterruptByGestures = LuaAPI.lua_toboolean(L, 4);
                    System.Action _overCallback = translator.GetDelegate<System.Action>(L, 5);
                    
                    gen_to_be_invoked.ScaleTo( _duration, _targetOrthographicSizeOrField, _canInterruptByGestures, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    float _targetOrthographicSizeOrField = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _canInterruptByGestures = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.ScaleTo( _duration, _targetOrthographicSizeOrField, _canInterruptByGestures );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    float _targetOrthographicSizeOrField = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.ScaleTo( _duration, _targetOrthographicSizeOrField );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SceneCameraActor.ScaleTo!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Camera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SceneCameraActor gen_to_be_invoked = (Honor.Runtime.SceneCameraActor)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Camera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
