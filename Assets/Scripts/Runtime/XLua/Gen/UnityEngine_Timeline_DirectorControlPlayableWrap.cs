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
    public class UnityEngineTimelineDirectorControlPlayableWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Timeline.DirectorControlPlayable);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 1, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPlayableDestroy", _m_OnPlayableDestroy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PrepareFrame", _m_PrepareFrame);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBehaviourPlay", _m_OnBehaviourPlay);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBehaviourPause", _m_OnBehaviourPause);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ProcessFrame", _m_ProcessFrame);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "director", _g_get_director);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "director", _s_set_director);
            
			
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
					
					UnityEngine.Timeline.DirectorControlPlayable gen_ret = new UnityEngine.Timeline.DirectorControlPlayable();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Timeline.DirectorControlPlayable constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Create_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Playables.PlayableGraph _graph;translator.Get(L, 1, out _graph);
                    UnityEngine.Playables.PlayableDirector _director = (UnityEngine.Playables.PlayableDirector)translator.GetObject(L, 2, typeof(UnityEngine.Playables.PlayableDirector));
                    
                        UnityEngine.Playables.ScriptPlayable<UnityEngine.Timeline.DirectorControlPlayable> gen_ret = UnityEngine.Timeline.DirectorControlPlayable.Create( _graph, _director );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnPlayableDestroy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.DirectorControlPlayable gen_to_be_invoked = (UnityEngine.Timeline.DirectorControlPlayable)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Playables.Playable _playable;translator.Get(L, 2, out _playable);
                    
                    gen_to_be_invoked.OnPlayableDestroy( _playable );
                    
                    
                    
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
            
            
                UnityEngine.Timeline.DirectorControlPlayable gen_to_be_invoked = (UnityEngine.Timeline.DirectorControlPlayable)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_OnBehaviourPlay(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.DirectorControlPlayable gen_to_be_invoked = (UnityEngine.Timeline.DirectorControlPlayable)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Playables.Playable _playable;translator.Get(L, 2, out _playable);
                    UnityEngine.Playables.FrameData _info;translator.Get(L, 3, out _info);
                    
                    gen_to_be_invoked.OnBehaviourPlay( _playable, _info );
                    
                    
                    
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
            
            
                UnityEngine.Timeline.DirectorControlPlayable gen_to_be_invoked = (UnityEngine.Timeline.DirectorControlPlayable)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_ProcessFrame(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.DirectorControlPlayable gen_to_be_invoked = (UnityEngine.Timeline.DirectorControlPlayable)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Playables.Playable _playable;translator.Get(L, 2, out _playable);
                    UnityEngine.Playables.FrameData _info;translator.Get(L, 3, out _info);
                    object _playerData = translator.GetObject(L, 4, typeof(object));
                    
                    gen_to_be_invoked.ProcessFrame( _playable, _info, _playerData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_director(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.DirectorControlPlayable gen_to_be_invoked = (UnityEngine.Timeline.DirectorControlPlayable)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.director);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_director(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.DirectorControlPlayable gen_to_be_invoked = (UnityEngine.Timeline.DirectorControlPlayable)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.director = (UnityEngine.Playables.PlayableDirector)translator.GetObject(L, 2, typeof(UnityEngine.Playables.PlayableDirector));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
