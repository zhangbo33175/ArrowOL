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
    public class UnityEngineTimelineAnimationTrackWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Timeline.AnimationTrack);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 15, 12);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateClip", _m_CreateClip);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateInfiniteClip", _m_CreateInfiniteClip);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateRecordableClip", _m_CreateRecordableClip);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GatherProperties", _m_GatherProperties);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "position", _g_get_position);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "rotation", _g_get_rotation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "eulerAngles", _g_get_eulerAngles);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "trackOffset", _g_get_trackOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "matchTargetFields", _g_get_matchTargetFields);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "infiniteClip", _g_get_infiniteClip);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "avatarMask", _g_get_avatarMask);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "applyAvatarMask", _g_get_applyAvatarMask);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "outputs", _g_get_outputs);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "inClipMode", _g_get_inClipMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "infiniteClipOffsetPosition", _g_get_infiniteClipOffsetPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "infiniteClipOffsetRotation", _g_get_infiniteClipOffsetRotation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "infiniteClipOffsetEulerAngles", _g_get_infiniteClipOffsetEulerAngles);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "infiniteClipPreExtrapolation", _g_get_infiniteClipPreExtrapolation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "infiniteClipPostExtrapolation", _g_get_infiniteClipPostExtrapolation);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "position", _s_set_position);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "rotation", _s_set_rotation);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "eulerAngles", _s_set_eulerAngles);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "trackOffset", _s_set_trackOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "matchTargetFields", _s_set_matchTargetFields);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "avatarMask", _s_set_avatarMask);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "applyAvatarMask", _s_set_applyAvatarMask);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "infiniteClipOffsetPosition", _s_set_infiniteClipOffsetPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "infiniteClipOffsetRotation", _s_set_infiniteClipOffsetRotation);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "infiniteClipOffsetEulerAngles", _s_set_infiniteClipOffsetEulerAngles);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "infiniteClipPreExtrapolation", _s_set_infiniteClipPreExtrapolation);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "infiniteClipPostExtrapolation", _s_set_infiniteClipPostExtrapolation);
            
			
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
					
					UnityEngine.Timeline.AnimationTrack gen_ret = new UnityEngine.Timeline.AnimationTrack();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Timeline.AnimationTrack constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateClip(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AnimationClip _clip = (UnityEngine.AnimationClip)translator.GetObject(L, 2, typeof(UnityEngine.AnimationClip));
                    
                        UnityEngine.Timeline.TimelineClip gen_ret = gen_to_be_invoked.CreateClip( _clip );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateInfiniteClip(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _infiniteClipName = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.CreateInfiniteClip( _infiniteClipName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateRecordableClip(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _animClipName = LuaAPI.lua_tostring(L, 2);
                    
                        UnityEngine.Timeline.TimelineClip gen_ret = gen_to_be_invoked.CreateRecordableClip( _animClipName );
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
            
            
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _g_get_position(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.position);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_rotation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineQuaternion(L, gen_to_be_invoked.rotation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_eulerAngles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.eulerAngles);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_trackOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineTrackOffset(L, gen_to_be_invoked.trackOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_matchTargetFields(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineMatchTargetFields(L, gen_to_be_invoked.matchTargetFields);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_infiniteClip(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.infiniteClip);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_avatarMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.avatarMask);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_applyAvatarMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.applyAvatarMask);
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
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.outputs);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_inClipMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.inClipMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_infiniteClipOffsetPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.infiniteClipOffsetPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_infiniteClipOffsetRotation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineQuaternion(L, gen_to_be_invoked.infiniteClipOffsetRotation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_infiniteClipOffsetEulerAngles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.infiniteClipOffsetEulerAngles);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_infiniteClipPreExtrapolation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineTimelineClipClipExtrapolation(L, gen_to_be_invoked.infiniteClipPreExtrapolation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_infiniteClipPostExtrapolation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineTimelineClipClipExtrapolation(L, gen_to_be_invoked.infiniteClipPostExtrapolation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_position(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.position = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_rotation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                UnityEngine.Quaternion gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.rotation = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_eulerAngles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.eulerAngles = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_trackOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                UnityEngine.Timeline.TrackOffset gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.trackOffset = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_matchTargetFields(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                UnityEngine.Timeline.MatchTargetFields gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.matchTargetFields = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_avatarMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.avatarMask = (UnityEngine.AvatarMask)translator.GetObject(L, 2, typeof(UnityEngine.AvatarMask));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_applyAvatarMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.applyAvatarMask = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_infiniteClipOffsetPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.infiniteClipOffsetPosition = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_infiniteClipOffsetRotation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                UnityEngine.Quaternion gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.infiniteClipOffsetRotation = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_infiniteClipOffsetEulerAngles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.infiniteClipOffsetEulerAngles = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_infiniteClipPreExtrapolation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                UnityEngine.Timeline.TimelineClip.ClipExtrapolation gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.infiniteClipPreExtrapolation = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_infiniteClipPostExtrapolation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationTrack gen_to_be_invoked = (UnityEngine.Timeline.AnimationTrack)translator.FastGetCSObj(L, 1);
                UnityEngine.Timeline.TimelineClip.ClipExtrapolation gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.infiniteClipPostExtrapolation = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
