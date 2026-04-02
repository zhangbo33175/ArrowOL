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
    public class UnityEngineTimelineTimelineClipWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Timeline.TimelineClip);
			Utils.BeginObjectRegister(type, L, translator, 0, 13, 35, 14);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetParentTrack", _m_GetParentTrack);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EvaluateMixOut", _m_EvaluateMixOut);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EvaluateMixIn", _m_EvaluateMixIn);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToLocalTime", _m_ToLocalTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToLocalTimeUnbound", _m_ToLocalTimeUnbound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsExtrapolatedTime", _m_IsExtrapolatedTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsPreExtrapolatedTime", _m_IsPreExtrapolatedTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsPostExtrapolatedTime", _m_IsPostExtrapolatedTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateCurves", _m_CreateCurves);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToString", _m_ToString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ConformEaseValues", _m_ConformEaseValues);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MoveToTrack", _m_MoveToTrack);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TryMoveToTrack", _m_TryMoveToTrack);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "hasPreExtrapolation", _g_get_hasPreExtrapolation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "hasPostExtrapolation", _g_get_hasPostExtrapolation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "timeScale", _g_get_timeScale);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "start", _g_get_start);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "duration", _g_get_duration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "end", _g_get_end);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "clipIn", _g_get_clipIn);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "displayName", _g_get_displayName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "clipAssetDuration", _g_get_clipAssetDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "curves", _g_get_curves);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "hasCurves", _g_get_hasCurves);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "asset", _g_get_asset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "easeInDuration", _g_get_easeInDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "easeOutDuration", _g_get_easeOutDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "easeOutTime", _g_get_easeOutTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "blendInDuration", _g_get_blendInDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "blendOutDuration", _g_get_blendOutDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "blendInCurveMode", _g_get_blendInCurveMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "blendOutCurveMode", _g_get_blendOutCurveMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "hasBlendIn", _g_get_hasBlendIn);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "hasBlendOut", _g_get_hasBlendOut);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mixInCurve", _g_get_mixInCurve);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mixInPercentage", _g_get_mixInPercentage);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mixInDuration", _g_get_mixInDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mixOutCurve", _g_get_mixOutCurve);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mixOutTime", _g_get_mixOutTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mixOutDuration", _g_get_mixOutDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mixOutPercentage", _g_get_mixOutPercentage);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "recordable", _g_get_recordable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "clipCaps", _g_get_clipCaps);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "animationClip", _g_get_animationClip);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "postExtrapolationMode", _g_get_postExtrapolationMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "preExtrapolationMode", _g_get_preExtrapolationMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "extrapolatedStart", _g_get_extrapolatedStart);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "extrapolatedDuration", _g_get_extrapolatedDuration);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "timeScale", _s_set_timeScale);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "start", _s_set_start);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "duration", _s_set_duration);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "clipIn", _s_set_clipIn);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "displayName", _s_set_displayName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "asset", _s_set_asset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "easeInDuration", _s_set_easeInDuration);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "easeOutDuration", _s_set_easeOutDuration);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "blendInDuration", _s_set_blendInDuration);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "blendOutDuration", _s_set_blendOutDuration);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "blendInCurveMode", _s_set_blendInCurveMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "blendOutCurveMode", _s_set_blendOutCurveMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mixInCurve", _s_set_mixInCurve);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mixOutCurve", _s_set_mixOutCurve);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 5, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "kDefaultClipCaps", UnityEngine.Timeline.TimelineClip.kDefaultClipCaps);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "kDefaultClipDurationInSeconds", UnityEngine.Timeline.TimelineClip.kDefaultClipDurationInSeconds);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "kTimeScaleMin", UnityEngine.Timeline.TimelineClip.kTimeScaleMin);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "kTimeScaleMax", UnityEngine.Timeline.TimelineClip.kTimeScaleMax);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "UnityEngine.Timeline.TimelineClip does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetParentTrack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        UnityEngine.Timeline.TrackAsset gen_ret = gen_to_be_invoked.GetParentTrack(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EvaluateMixOut(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    double _time = LuaAPI.lua_tonumber(L, 2);
                    
                        float gen_ret = gen_to_be_invoked.EvaluateMixOut( _time );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EvaluateMixIn(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    double _time = LuaAPI.lua_tonumber(L, 2);
                    
                        float gen_ret = gen_to_be_invoked.EvaluateMixIn( _time );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToLocalTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    double _time = LuaAPI.lua_tonumber(L, 2);
                    
                        double gen_ret = gen_to_be_invoked.ToLocalTime( _time );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToLocalTimeUnbound(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    double _time = LuaAPI.lua_tonumber(L, 2);
                    
                        double gen_ret = gen_to_be_invoked.ToLocalTimeUnbound( _time );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsExtrapolatedTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    double _sequenceTime = LuaAPI.lua_tonumber(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.IsExtrapolatedTime( _sequenceTime );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsPreExtrapolatedTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    double _sequenceTime = LuaAPI.lua_tonumber(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.IsPreExtrapolatedTime( _sequenceTime );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsPostExtrapolatedTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    double _sequenceTime = LuaAPI.lua_tonumber(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.IsPostExtrapolatedTime( _sequenceTime );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
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
            
            
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_ToString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        string gen_ret = gen_to_be_invoked.ToString(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ConformEaseValues(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ConformEaseValues(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveToTrack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Timeline.TrackAsset _destinationTrack = (UnityEngine.Timeline.TrackAsset)translator.GetObject(L, 2, typeof(UnityEngine.Timeline.TrackAsset));
                    
                    gen_to_be_invoked.MoveToTrack( _destinationTrack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TryMoveToTrack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Timeline.TrackAsset _destinationTrack = (UnityEngine.Timeline.TrackAsset)translator.GetObject(L, 2, typeof(UnityEngine.Timeline.TrackAsset));
                    
                        bool gen_ret = gen_to_be_invoked.TryMoveToTrack( _destinationTrack );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_hasPreExtrapolation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.hasPreExtrapolation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_hasPostExtrapolation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.hasPostExtrapolation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_timeScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.timeScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_start(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.start);
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
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.duration);
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
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.end);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_clipIn(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.clipIn);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_displayName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.displayName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_clipAssetDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.clipAssetDuration);
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
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.curves);
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
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.hasCurves);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_asset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.asset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_easeInDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.easeInDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_easeOutDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.easeOutDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_easeOutTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.easeOutTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_blendInDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.blendInDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_blendOutDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.blendOutDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_blendInCurveMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineTimelineClipBlendCurveMode(L, gen_to_be_invoked.blendInCurveMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_blendOutCurveMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineTimelineClipBlendCurveMode(L, gen_to_be_invoked.blendOutCurveMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_hasBlendIn(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.hasBlendIn);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_hasBlendOut(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.hasBlendOut);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mixInCurve(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.mixInCurve);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mixInPercentage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mixInPercentage);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mixInDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mixInDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mixOutCurve(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.mixOutCurve);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mixOutTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mixOutTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mixOutDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mixOutDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mixOutPercentage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mixOutPercentage);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_recordable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.recordable);
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
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineClipCaps(L, gen_to_be_invoked.clipCaps);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_animationClip(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.animationClip);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_postExtrapolationMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineTimelineClipClipExtrapolation(L, gen_to_be_invoked.postExtrapolationMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_preExtrapolationMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineTimelineClipClipExtrapolation(L, gen_to_be_invoked.preExtrapolationMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_extrapolatedStart(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.extrapolatedStart);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_extrapolatedDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.extrapolatedDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_timeScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.timeScale = LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_start(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.start = LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_duration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.duration = LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_clipIn(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.clipIn = LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_displayName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.displayName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_asset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.asset = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_easeInDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.easeInDuration = LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_easeOutDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.easeOutDuration = LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_blendInDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.blendInDuration = LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_blendOutDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.blendOutDuration = LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_blendInCurveMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                UnityEngine.Timeline.TimelineClip.BlendCurveMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.blendInCurveMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_blendOutCurveMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                UnityEngine.Timeline.TimelineClip.BlendCurveMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.blendOutCurveMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mixInCurve(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mixInCurve = (UnityEngine.AnimationCurve)translator.GetObject(L, 2, typeof(UnityEngine.AnimationCurve));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mixOutCurve(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.TimelineClip gen_to_be_invoked = (UnityEngine.Timeline.TimelineClip)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mixOutCurve = (UnityEngine.AnimationCurve)translator.GetObject(L, 2, typeof(UnityEngine.AnimationCurve));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
