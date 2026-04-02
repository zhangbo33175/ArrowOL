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
using UnityEngine.Timeline;

namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class UnityEngineTimelineTrackAssetWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Timeline.TrackAsset);
			Utils.BeginObjectRegister(type, L, translator, 0, 16, 16, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetClips", _m_GetClips);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetChildTracks", _m_GetChildTracks);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateCurves", _m_CreateCurves);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateTrackMixer", _m_CreateTrackMixer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreatePlayable", _m_CreatePlayable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateDefaultClip", _m_CreateDefaultClip);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DeleteClip", _m_DeleteClip);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateMarker", _m_CreateMarker);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DeleteMarker", _m_DeleteMarker);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMarkers", _m_GetMarkers);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMarkerCount", _m_GetMarkerCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMarker", _m_GetMarker);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GatherProperties", _m_GatherProperties);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CanCreateTrackMixer", _m_CanCreateTrackMixer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetGroup", _m_GetGroup);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetGroup", _m_SetGroup);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "start", _g_get_start);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "end", _g_get_end);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "duration", _g_get_duration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "muted", _g_get_muted);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mutedInHierarchy", _g_get_mutedInHierarchy);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "timelineAsset", _g_get_timelineAsset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "parent", _g_get_parent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isEmpty", _g_get_isEmpty);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "hasClips", _g_get_hasClips);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "hasCurves", _g_get_hasCurves);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isSubTrack", _g_get_isSubTrack);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "outputs", _g_get_outputs);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "curves", _g_get_curves);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "locked", _g_get_locked);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "lockedInHierarchy", _g_get_lockedInHierarchy);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "supportsNotifications", _g_get_supportsNotifications);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "muted", _s_set_muted);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "locked", _s_set_locked);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "UnityEngine.Timeline.TrackAsset does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetClips(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.Collections.Generic.IEnumerable<UnityEngine.Timeline.TimelineClip> gen_ret = gen_to_be_invoked.GetClips(  );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetChildTracks(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.Collections.Generic.IEnumerable<UnityEngine.Timeline.TrackAsset> gen_ret = gen_to_be_invoked.GetChildTracks(  );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateCurves(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _curvesClipName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.CreateCurves( _curvesClipName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateTrackMixer(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_CreatePlayable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Playables.PlayableGraph _graph;translator.Get(L, 2, out _graph);
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    
                        UnityEngine.Playables.Playable gen_ret = gen_to_be_invoked.CreatePlayable( _graph, _go );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateDefaultClip(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        UnityEngine.Timeline.TimelineClip gen_ret = gen_to_be_invoked.CreateDefaultClip(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteClip(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Timeline.TimelineClip _clip = (UnityEngine.Timeline.TimelineClip)translator.GetObject(L, 2, typeof(UnityEngine.Timeline.TimelineClip));
                    
                        bool gen_ret = gen_to_be_invoked.DeleteClip( _clip );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateMarker(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Type _type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    double _time = LuaAPI.lua_tonumber(L, 3);
                    
                        UnityEngine.Timeline.IMarker gen_ret = gen_to_be_invoked.CreateMarker( _type, _time );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteMarker(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Timeline.IMarker _marker = (UnityEngine.Timeline.IMarker)translator.GetObject(L, 2, typeof(UnityEngine.Timeline.IMarker));
                    
                        bool gen_ret = gen_to_be_invoked.DeleteMarker( _marker );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMarkers(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.Collections.Generic.IEnumerable<UnityEngine.Timeline.IMarker> gen_ret = gen_to_be_invoked.GetMarkers(  );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMarkerCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        int gen_ret = gen_to_be_invoked.GetMarkerCount(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMarker(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _idx = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.Timeline.IMarker gen_ret = gen_to_be_invoked.GetMarker( _idx );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
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
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_CanCreateTrackMixer(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.CanCreateTrackMixer(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGroup(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        UnityEngine.Timeline.GroupTrack gen_ret = gen_to_be_invoked.GetGroup(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetGroup(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Timeline.GroupTrack _group = (UnityEngine.Timeline.GroupTrack)translator.GetObject(L, 2, typeof(UnityEngine.Timeline.GroupTrack));
                    
                    gen_to_be_invoked.SetGroup( _group );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_start(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.start);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_end(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.end);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_duration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.duration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_muted(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.muted);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mutedInHierarchy(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.mutedInHierarchy);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_timelineAsset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.timelineAsset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_parent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.parent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isEmpty(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isEmpty);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_hasClips(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.hasClips);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_hasCurves(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.hasCurves);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isSubTrack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isSubTrack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_outputs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.outputs);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_curves(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.curves);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_locked(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.locked);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_lockedInHierarchy(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.lockedInHierarchy);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_supportsNotifications(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.supportsNotifications);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_muted(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.muted = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_locked(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TrackAsset gen_to_be_invoked = (UnityEngine.Timeline.TrackAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.locked = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
