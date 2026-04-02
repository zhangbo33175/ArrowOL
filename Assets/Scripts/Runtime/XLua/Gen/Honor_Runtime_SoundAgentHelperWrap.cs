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
    public class HonorRuntimeSoundAgentHelperWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.SoundAgentHelper);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 13, 11);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Play", _m_Play);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Stop", _m_Stop);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Pause", _m_Pause);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Resume", _m_Resume);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Reset", _m_Reset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetSoundAsset", _m_SetSoundAsset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetWorldPosition", _m_SetWorldPosition);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsPlaying", _g_get_IsPlaying);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Length", _g_get_Length);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Time", _g_get_Time);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Mute", _g_get_Mute);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Loop", _g_get_Loop);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Priority", _g_get_Priority);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Volume", _g_get_Volume);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Pitch", _g_get_Pitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PanStereo", _g_get_PanStereo);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SpatialBlend", _g_get_SpatialBlend);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaxDistance", _g_get_MaxDistance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DopplerLevel", _g_get_DopplerLevel);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AudioMixerGroup", _g_get_AudioMixerGroup);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Time", _s_set_Time);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Mute", _s_set_Mute);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Loop", _s_set_Loop);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Priority", _s_set_Priority);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Volume", _s_set_Volume);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Pitch", _s_set_Pitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PanStereo", _s_set_PanStereo);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SpatialBlend", _s_set_SpatialBlend);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MaxDistance", _s_set_MaxDistance);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DopplerLevel", _s_set_DopplerLevel);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AudioMixerGroup", _s_set_AudioMixerGroup);
            
			
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
					
					Honor.Runtime.SoundAgentHelper gen_ret = new Honor.Runtime.SoundAgentHelper();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SoundAgentHelper constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Play(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _fadeInSeconds = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.Play( _fadeInSeconds );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Stop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _fadeOutSeconds = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.Stop( _fadeOutSeconds );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Pause(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _fadeOutSeconds = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.Pause( _fadeOutSeconds );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Resume(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _fadeInSeconds = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.Resume( _fadeInSeconds );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Reset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Reset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSoundAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    object _soundAsset = translator.GetObject(L, 2, typeof(object));
                    
                        bool gen_ret = gen_to_be_invoked.SetSoundAsset( _soundAsset );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetWorldPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _worldPosition;translator.Get(L, 2, out _worldPosition);
                    
                    gen_to_be_invoked.SetWorldPosition( _worldPosition );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsPlaying(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsPlaying);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Length(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.Length);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Time(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.Time);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Mute(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.Mute);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Loop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.Loop);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Priority(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Priority);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Volume(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.Volume);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Pitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.Pitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PanStereo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.PanStereo);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SpatialBlend(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.SpatialBlend);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaxDistance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.MaxDistance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DopplerLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.DopplerLevel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AudioMixerGroup(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AudioMixerGroup);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Time(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Time = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Mute(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Mute = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Loop(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Loop = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Priority(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Priority = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Volume(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Volume = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Pitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Pitch = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PanStereo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PanStereo = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SpatialBlend(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SpatialBlend = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MaxDistance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MaxDistance = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DopplerLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DopplerLevel = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AudioMixerGroup(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundAgentHelper gen_to_be_invoked = (Honor.Runtime.SoundAgentHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AudioMixerGroup = (UnityEngine.Audio.AudioMixerGroup)translator.GetObject(L, 2, typeof(UnityEngine.Audio.AudioMixerGroup));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
