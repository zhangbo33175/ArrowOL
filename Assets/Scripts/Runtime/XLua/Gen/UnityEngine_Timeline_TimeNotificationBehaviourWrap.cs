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
    public class UnityEngineTimelineTimeNotificationBehaviourWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Timeline.TimeNotificationBehaviour);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 0, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddNotification", _m_AddNotification);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnGraphStart", _m_OnGraphStart);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBehaviourPause", _m_OnBehaviourPause);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PrepareFrame", _m_PrepareFrame);
			
			
			
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "timeSource", _s_set_timeSource);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Create", _m_Create_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UnityEngine.Timeline.TimeNotificationBehaviour gen_ret = new UnityEngine.Timeline.TimeNotificationBehaviour();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Timeline.TimeNotificationBehaviour constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Create_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Playables.PlayableGraph _graph;translator.Get(L, 1, out _graph);
                    double _duration = LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Playables.DirectorWrapMode _loopMode;translator.Get(L, 3, out _loopMode);
                    
                        UnityEngine.Playables.ScriptPlayable<UnityEngine.Timeline.TimeNotificationBehaviour> gen_ret = UnityEngine.Timeline.TimeNotificationBehaviour.Create( _graph, _duration, _loopMode );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddNotification(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimeNotificationBehaviour gen_to_be_invoked = (UnityEngine.Timeline.TimeNotificationBehaviour)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Playables.INotification>(L, 3)&& translator.Assignable<UnityEngine.Timeline.NotificationFlags>(L, 4)) 
                {
                    double _time = LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Playables.INotification _payload = (UnityEngine.Playables.INotification)translator.GetObject(L, 3, typeof(UnityEngine.Playables.INotification));
                    UnityEngine.Timeline.NotificationFlags _flags;translator.Get(L, 4, out _flags);
                    
                    gen_to_be_invoked.AddNotification( _time, _payload, _flags );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Playables.INotification>(L, 3)) 
                {
                    double _time = LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Playables.INotification _payload = (UnityEngine.Playables.INotification)translator.GetObject(L, 3, typeof(UnityEngine.Playables.INotification));
                    
                    gen_to_be_invoked.AddNotification( _time, _payload );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Timeline.TimeNotificationBehaviour.AddNotification!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnGraphStart(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimeNotificationBehaviour gen_to_be_invoked = (UnityEngine.Timeline.TimeNotificationBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Playables.Playable _playable;translator.Get(L, 2, out _playable);
                    
                    gen_to_be_invoked.OnGraphStart( _playable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnBehaviourPause(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimeNotificationBehaviour gen_to_be_invoked = (UnityEngine.Timeline.TimeNotificationBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Playables.Playable _playable;translator.Get(L, 2, out _playable);
                    UnityEngine.Playables.FrameData _info;translator.Get(L, 3, out _info);
                    
                    gen_to_be_invoked.OnBehaviourPause( _playable, _info );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PrepareFrame(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimeNotificationBehaviour gen_to_be_invoked = (UnityEngine.Timeline.TimeNotificationBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Playables.Playable _playable;translator.Get(L, 2, out _playable);
                    UnityEngine.Playables.FrameData _info;translator.Get(L, 3, out _info);
                    
                    gen_to_be_invoked.PrepareFrame( _playable, _info );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_timeSource(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimeNotificationBehaviour gen_to_be_invoked = (UnityEngine.Timeline.TimeNotificationBehaviour)translator.FastGetCSObj(L, 1);
                UnityEngine.Playables.Playable gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.timeSource = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
