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
    public class UnityEngineTimelineParticleControlPlayableWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Timeline.ParticleControlPlayable);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 1, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Initialize", _m_Initialize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPlayableDestroy", _m_OnPlayableDestroy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PrepareFrame", _m_PrepareFrame);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBehaviourPlay", _m_OnBehaviourPlay);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBehaviourPause", _m_OnBehaviourPause);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "particleSystem", _g_get_particleSystem);
            
			
			
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
					
					UnityEngine.Timeline.ParticleControlPlayable gen_ret = new UnityEngine.Timeline.ParticleControlPlayable();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Timeline.ParticleControlPlayable constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Create_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Playables.PlayableGraph _graph;translator.Get(L, 1, out _graph);
                    UnityEngine.ParticleSystem _component = (UnityEngine.ParticleSystem)translator.GetObject(L, 2, typeof(UnityEngine.ParticleSystem));
                    uint _randomSeed = LuaAPI.xlua_touint(L, 3);
                    
                        UnityEngine.Playables.ScriptPlayable<UnityEngine.Timeline.ParticleControlPlayable> gen_ret = UnityEngine.Timeline.ParticleControlPlayable.Create( _graph, _component, _randomSeed );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Initialize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.ParticleControlPlayable gen_to_be_invoked = (UnityEngine.Timeline.ParticleControlPlayable)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.ParticleSystem _ps = (UnityEngine.ParticleSystem)translator.GetObject(L, 2, typeof(UnityEngine.ParticleSystem));
                    uint _randomSeed = LuaAPI.xlua_touint(L, 3);
                    
                    gen_to_be_invoked.Initialize( _ps, _randomSeed );
                    
                    
                    
                    return 0;
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
            
            
                UnityEngine.Timeline.ParticleControlPlayable gen_to_be_invoked = (UnityEngine.Timeline.ParticleControlPlayable)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                UnityEngine.Timeline.ParticleControlPlayable gen_to_be_invoked = (UnityEngine.Timeline.ParticleControlPlayable)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Playables.Playable _playable;translator.Get(L, 2, out _playable);
                    UnityEngine.Playables.FrameData _data;translator.Get(L, 3, out _data);
                    
                    gen_to_be_invoked.PrepareFrame( _playable, _data );
                    
                    
                    
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
            
            
                UnityEngine.Timeline.ParticleControlPlayable gen_to_be_invoked = (UnityEngine.Timeline.ParticleControlPlayable)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                UnityEngine.Timeline.ParticleControlPlayable gen_to_be_invoked = (UnityEngine.Timeline.ParticleControlPlayable)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _g_get_particleSystem(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ParticleControlPlayable gen_to_be_invoked = (UnityEngine.Timeline.ParticleControlPlayable)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.particleSystem);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
