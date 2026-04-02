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
    public class HonorRuntimeSoundGroupWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.SoundGroup);
			Utils.BeginObjectRegister(type, L, translator, 0, 8, 6, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddSoundAgent", _m_AddSoundAgent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaySound", _m_PlaySound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopSound", _m_StopSound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PauseSound", _m_PauseSound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResumeSound", _m_ResumeSound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopAllLoadedSounds", _m_StopAllLoadedSounds);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PauseAllLoadedSounds", _m_PauseAllLoadedSounds);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResumeAllLoadedSounds", _m_ResumeAllLoadedSounds);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Name", _g_get_Name);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SoundGroupHelper", _g_get_SoundGroupHelper);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SoundAgentCount", _g_get_SoundAgentCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AvoidBeingReplacedBySamePriority", _g_get_AvoidBeingReplacedBySamePriority);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Mute", _g_get_Mute);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Volume", _g_get_Volume);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "AvoidBeingReplacedBySamePriority", _s_set_AvoidBeingReplacedBySamePriority);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Mute", _s_set_Mute);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Volume", _s_set_Volume);
            
			
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
				if(LuaAPI.lua_gettop(L) == 3 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && translator.Assignable<Honor.Runtime.SoundGroupHelper>(L, 3))
				{
					string _name = LuaAPI.lua_tostring(L, 2);
					Honor.Runtime.SoundGroupHelper _soundGroupHelper = (Honor.Runtime.SoundGroupHelper)translator.GetObject(L, 3, typeof(Honor.Runtime.SoundGroupHelper));
					
					Honor.Runtime.SoundGroup gen_ret = new Honor.Runtime.SoundGroup(_name, _soundGroupHelper);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SoundGroup constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddSoundAgent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.SoundManager _soundManager = (Honor.Runtime.SoundManager)translator.GetObject(L, 2, typeof(Honor.Runtime.SoundManager));
                    Honor.Runtime.SoundAgentHelper _soundAgentHelper = (Honor.Runtime.SoundAgentHelper)translator.GetObject(L, 3, typeof(Honor.Runtime.SoundAgentHelper));
                    
                    gen_to_be_invoked.AddSoundAgent( _soundManager, _soundAgentHelper );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaySound(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _serialID = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Object _soundAsset = (UnityEngine.Object)translator.GetObject(L, 3, typeof(UnityEngine.Object));
                    Honor.Runtime.PlaySoundParams _playSoundParams = (Honor.Runtime.PlaySoundParams)translator.GetObject(L, 4, typeof(Honor.Runtime.PlaySoundParams));
                    System.Nullable<Honor.Runtime.PlaySoundErrorCode> _errorCode;
                    
                        Honor.Runtime.SoundAgent gen_ret = gen_to_be_invoked.PlaySound( _serialID, _soundAsset, _playSoundParams, out _errorCode );
                        translator.Push(L, gen_ret);
                    translator.PushAny(L, _errorCode);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopSound(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _serialID = LuaAPI.xlua_tointeger(L, 2);
                    float _fadeOutSeconds = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.StopSound( _serialID, _fadeOutSeconds );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PauseSound(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _serialID = LuaAPI.xlua_tointeger(L, 2);
                    float _fadeOutSeconds = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.PauseSound( _serialID, _fadeOutSeconds );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResumeSound(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _serialID = LuaAPI.xlua_tointeger(L, 2);
                    float _fadeInSeconds = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.ResumeSound( _serialID, _fadeInSeconds );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopAllLoadedSounds(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.StopAllLoadedSounds(  );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    float _fadeOutSeconds = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.StopAllLoadedSounds( _fadeOutSeconds );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SoundGroup.StopAllLoadedSounds!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PauseAllLoadedSounds(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _fadeOutSeconds = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.PauseAllLoadedSounds( _fadeOutSeconds );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResumeAllLoadedSounds(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _fadeInSeconds = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.ResumeAllLoadedSounds( _fadeInSeconds );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Name(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Name);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SoundGroupHelper(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SoundGroupHelper);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SoundAgentCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SoundAgentCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AvoidBeingReplacedBySamePriority(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.AvoidBeingReplacedBySamePriority);
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
			
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.Mute);
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
			
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.Volume);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AvoidBeingReplacedBySamePriority(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AvoidBeingReplacedBySamePriority = LuaAPI.lua_toboolean(L, 2);
            
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
			
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Mute = LuaAPI.lua_toboolean(L, 2);
            
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
			
                Honor.Runtime.SoundGroup gen_to_be_invoked = (Honor.Runtime.SoundGroup)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Volume = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
