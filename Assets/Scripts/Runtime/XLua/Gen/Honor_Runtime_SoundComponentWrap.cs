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
    public class HonorRuntimeSoundComponentWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.SoundComponent);
			Utils.BeginObjectRegister(type, L, translator, 0, 16, 2, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasSoundGroup", _m_HasSoundGroup);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSoundGroup", _m_GetSoundGroup);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAllSoundGroups", _m_GetAllSoundGroups);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddSoundGroup", _m_AddSoundGroup);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAllLoadingSoundSerialIDs", _m_GetAllLoadingSoundSerialIDs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsLoadingSound", _m_IsLoadingSound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaySound", _m_PlaySound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopSound", _m_StopSound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopAllLoadedSounds", _m_StopAllLoadedSounds);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopAllLoadingSounds", _m_StopAllLoadingSounds);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PauseSound", _m_PauseSound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResumeSound", _m_ResumeSound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PauseGourpSound", _m_PauseGourpSound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResumeGroupSound", _m_ResumeGroupSound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopGroupSound", _m_StopGroupSound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAllSoundVolume", _m_SetAllSoundVolume);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "AudioMixer", _g_get_AudioMixer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SoundGroupCount", _g_get_SoundGroupCount);
            
			
			
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
					
					Honor.Runtime.SoundComponent gen_ret = new Honor.Runtime.SoundComponent();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SoundComponent constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasSoundGroup(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _soundGroupName = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.HasSoundGroup( _soundGroupName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSoundGroup(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _soundGroupName = LuaAPI.lua_tostring(L, 2);
                    
                        Honor.Runtime.SoundGroup gen_ret = gen_to_be_invoked.GetSoundGroup( _soundGroupName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAllSoundGroups(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                        Honor.Runtime.SoundGroup[] gen_ret = gen_to_be_invoked.GetAllSoundGroups(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<System.Collections.Generic.List<Honor.Runtime.SoundGroup>>(L, 2)) 
                {
                    System.Collections.Generic.List<Honor.Runtime.SoundGroup> _results = (System.Collections.Generic.List<Honor.Runtime.SoundGroup>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<Honor.Runtime.SoundGroup>));
                    
                    gen_to_be_invoked.GetAllSoundGroups( _results );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SoundComponent.GetAllSoundGroups!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddSoundGroup(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _soundGroupName = LuaAPI.lua_tostring(L, 2);
                    int _soundAgentCount = LuaAPI.xlua_tointeger(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.AddSoundGroup( _soundGroupName, _soundAgentCount );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    string _soundGroupName = LuaAPI.lua_tostring(L, 2);
                    bool _soundGroupAvoidBeingReplacedBySamePriority = LuaAPI.lua_toboolean(L, 3);
                    bool _soundGroupMute = LuaAPI.lua_toboolean(L, 4);
                    float _soundGroupVolume = (float)LuaAPI.lua_tonumber(L, 5);
                    int _soundAgentCount = LuaAPI.xlua_tointeger(L, 6);
                    
                        bool gen_ret = gen_to_be_invoked.AddSoundGroup( _soundGroupName, _soundGroupAvoidBeingReplacedBySamePriority, _soundGroupMute, _soundGroupVolume, _soundAgentCount );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SoundComponent.AddSoundGroup!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAllLoadingSoundSerialIDs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                        int[] gen_ret = gen_to_be_invoked.GetAllLoadingSoundSerialIDs(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<System.Collections.Generic.List<int>>(L, 2)) 
                {
                    System.Collections.Generic.List<int> _results = (System.Collections.Generic.List<int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<int>));
                    
                    gen_to_be_invoked.GetAllLoadingSoundSerialIDs( _results );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SoundComponent.GetAllLoadingSoundSerialIDs!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsLoadingSound(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _serialID = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.IsLoadingSound( _serialID );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
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
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    
                        int gen_ret = gen_to_be_invoked.PlaySound( _luaTable );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& translator.Assignable<Honor.Runtime.PlaySoundParams>(L, 5)&& translator.Assignable<UnityEngine.Vector3>(L, 6)) 
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    string _soundGroupName = LuaAPI.lua_tostring(L, 4);
                    Honor.Runtime.PlaySoundParams _playSoundParams = (Honor.Runtime.PlaySoundParams)translator.GetObject(L, 5, typeof(Honor.Runtime.PlaySoundParams));
                    UnityEngine.Vector3 _worldPosition;translator.Get(L, 6, out _worldPosition);
                    
                        int gen_ret = gen_to_be_invoked.PlaySound( _abPath, _assetName, _soundGroupName, _playSoundParams, _worldPosition );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& translator.Assignable<Honor.Runtime.PlaySoundParams>(L, 5)) 
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    string _soundGroupName = LuaAPI.lua_tostring(L, 4);
                    Honor.Runtime.PlaySoundParams _playSoundParams = (Honor.Runtime.PlaySoundParams)translator.GetObject(L, 5, typeof(Honor.Runtime.PlaySoundParams));
                    
                        int gen_ret = gen_to_be_invoked.PlaySound( _abPath, _assetName, _soundGroupName, _playSoundParams );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    string _soundGroupName = LuaAPI.lua_tostring(L, 4);
                    
                        int gen_ret = gen_to_be_invoked.PlaySound( _abPath, _assetName, _soundGroupName );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SoundComponent.PlaySound!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopSound(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _serialID = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.StopSound( _serialID );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SoundComponent.StopSound!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopAllLoadedSounds(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SoundComponent.StopAllLoadedSounds!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopAllLoadingSounds(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StopAllLoadingSounds(  );
                    
                    
                    
                    return 0;
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
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _serialID = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.PauseSound( _serialID );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _serialID = LuaAPI.xlua_tointeger(L, 2);
                    float _fadeOutSeconds = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.PauseSound( _serialID, _fadeOutSeconds );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SoundComponent.PauseSound!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResumeSound(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _serialID = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.ResumeSound( _serialID );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.SoundComponent.ResumeSound!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PauseGourpSound(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _groupName = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.PauseGourpSound( _groupName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResumeGroupSound(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _groupName = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.ResumeGroupSound( _groupName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopGroupSound(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _groupName = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.StopGroupSound( _groupName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAllSoundVolume(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _newVolume = (float)LuaAPI.lua_tonumber(L, 2);
                    string _groupName = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.SetAllSoundVolume( _newVolume, _groupName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AudioMixer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AudioMixer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SoundGroupCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.SoundComponent gen_to_be_invoked = (Honor.Runtime.SoundComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SoundGroupCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
