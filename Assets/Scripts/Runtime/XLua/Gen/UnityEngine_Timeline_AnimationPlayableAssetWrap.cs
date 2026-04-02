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
    public class UnityEngineTimelineAnimationPlayableAssetWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Timeline.AnimationPlayableAsset);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 12, 9);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreatePlayable", _m_CreatePlayable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetOffsets", _m_ResetOffsets);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GatherProperties", _m_GatherProperties);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "position", _g_get_position);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "rotation", _g_get_rotation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "eulerAngles", _g_get_eulerAngles);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "useTrackMatchFields", _g_get_useTrackMatchFields);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "matchTargetFields", _g_get_matchTargetFields);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "removeStartOffset", _g_get_removeStartOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "applyFootIK", _g_get_applyFootIK);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "loop", _g_get_loop);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "clip", _g_get_clip);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "duration", _g_get_duration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "outputs", _g_get_outputs);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "clipCaps", _g_get_clipCaps);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "position", _s_set_position);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "rotation", _s_set_rotation);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "eulerAngles", _s_set_eulerAngles);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "useTrackMatchFields", _s_set_useTrackMatchFields);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "matchTargetFields", _s_set_matchTargetFields);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "removeStartOffset", _s_set_removeStartOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "applyFootIK", _s_set_applyFootIK);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "loop", _s_set_loop);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "clip", _s_set_clip);
            
			
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
					
					UnityEngine.Timeline.AnimationPlayableAsset gen_ret = new UnityEngine.Timeline.AnimationPlayableAsset();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Timeline.AnimationPlayableAsset constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreatePlayable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_ResetOffsets(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetOffsets(  );
                    
                    
                    
                    return 0;
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
            
            
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
            
            
                
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
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
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
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
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
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.eulerAngles);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_useTrackMatchFields(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.useTrackMatchFields);
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
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineMatchTargetFields(L, gen_to_be_invoked.matchTargetFields);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_removeStartOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.removeStartOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_applyFootIK(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.applyFootIK);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_loop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineAnimationPlayableAssetLoopMode(L, gen_to_be_invoked.loop);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_clip(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.clip);
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
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.duration);
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
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.outputs);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_clipCaps(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineClipCaps(L, gen_to_be_invoked.clipCaps);
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
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
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
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
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
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.eulerAngles = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_useTrackMatchFields(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.useTrackMatchFields = LuaAPI.lua_toboolean(L, 2);
            
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
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                UnityEngine.Timeline.MatchTargetFields gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.matchTargetFields = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_removeStartOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.removeStartOffset = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_applyFootIK(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.applyFootIK = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_loop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                UnityEngine.Timeline.AnimationPlayableAsset.LoopMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.loop = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_clip(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.AnimationPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.AnimationPlayableAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.clip = (UnityEngine.AnimationClip)translator.GetObject(L, 2, typeof(UnityEngine.AnimationClip));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
