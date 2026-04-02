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
    public class UnityEngineTimelineTimelinePlayableWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Timeline.TimelinePlayable);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Compile", _m_Compile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PrepareFrame", _m_PrepareFrame);
			
			
			
			
			
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
					
					UnityEngine.Timeline.TimelinePlayable gen_ret = new UnityEngine.Timeline.TimelinePlayable();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Timeline.TimelinePlayable constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Create_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Playables.PlayableGraph _graph;translator.Get(L, 1, out _graph);
                    System.Collections.Generic.IEnumerable<UnityEngine.Timeline.TrackAsset> _tracks = (System.Collections.Generic.IEnumerable<UnityEngine.Timeline.TrackAsset>)translator.GetObject(L, 2, typeof(System.Collections.Generic.IEnumerable<UnityEngine.Timeline.TrackAsset>));
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    bool _autoRebalance = LuaAPI.lua_toboolean(L, 4);
                    bool _createOutputs = LuaAPI.lua_toboolean(L, 5);
                    
                        UnityEngine.Playables.ScriptPlayable<UnityEngine.Timeline.TimelinePlayable> gen_ret = UnityEngine.Timeline.TimelinePlayable.Create( _graph, _tracks, _go, _autoRebalance, _createOutputs );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Compile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimelinePlayable gen_to_be_invoked = (UnityEngine.Timeline.TimelinePlayable)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Playables.PlayableGraph _graph;translator.Get(L, 2, out _graph);
                    UnityEngine.Playables.Playable _timelinePlayable;translator.Get(L, 3, out _timelinePlayable);
                    System.Collections.Generic.IEnumerable<UnityEngine.Timeline.TrackAsset> _tracks = (System.Collections.Generic.IEnumerable<UnityEngine.Timeline.TrackAsset>)translator.GetObject(L, 4, typeof(System.Collections.Generic.IEnumerable<UnityEngine.Timeline.TrackAsset>));
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 5, typeof(UnityEngine.GameObject));
                    bool _autoRebalance = LuaAPI.lua_toboolean(L, 6);
                    bool _createOutputs = LuaAPI.lua_toboolean(L, 7);
                    
                    gen_to_be_invoked.Compile( _graph, _timelinePlayable, _tracks, _go, _autoRebalance, _createOutputs );
                    
                    
                    
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
            
            
                UnityEngine.Timeline.TimelinePlayable gen_to_be_invoked = (UnityEngine.Timeline.TimelinePlayable)translator.FastGetCSObj(L, 1);
            
            
                
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
        
        
        
        
        
        
		
		
		
		
    }
}
