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
    public class UnityEngineTimelineActivationTrackWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Timeline.ActivationTrack);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 1, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateTrackMixer", _m_CreateTrackMixer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GatherProperties", _m_GatherProperties);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "postPlaybackState", _g_get_postPlaybackState);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "postPlaybackState", _s_set_postPlaybackState);
            
			
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
					
					UnityEngine.Timeline.ActivationTrack gen_ret = new UnityEngine.Timeline.ActivationTrack();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Timeline.ActivationTrack constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateTrackMixer(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.ActivationTrack gen_to_be_invoked = (UnityEngine.Timeline.ActivationTrack)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Playables.PlayableGraph _graph;translator.Get(L, 2, out _graph);
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    int _inputCount = LuaAPI.xlua_tointeger(L, 4);
                    
                        UnityEngine.Playables.Playable gen_ret = gen_to_be_invoked.CreateTrackMixer( _graph, _go, _inputCount );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GatherProperties(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.ActivationTrack gen_to_be_invoked = (UnityEngine.Timeline.ActivationTrack)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Playables.PlayableDirector _director = (UnityEngine.Playables.PlayableDirector)translator.GetObject(L, 2, typeof(UnityEngine.Playables.PlayableDirector));
                    UnityEngine.Timeline.IPropertyCollector _driver = (UnityEngine.Timeline.IPropertyCollector)translator.GetObject(L, 3, typeof(UnityEngine.Timeline.IPropertyCollector));
                    
                    gen_to_be_invoked.GatherProperties( _director, _driver );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_postPlaybackState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ActivationTrack gen_to_be_invoked = (UnityEngine.Timeline.ActivationTrack)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineActivationTrackPostPlaybackState(L, gen_to_be_invoked.postPlaybackState);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_postPlaybackState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ActivationTrack gen_to_be_invoked = (UnityEngine.Timeline.ActivationTrack)translator.FastGetCSObj(L, 1);
                UnityEngine.Timeline.ActivationTrack.PostPlaybackState gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.postPlaybackState = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
