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
    public class HonorRuntimeHonorCoreManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.HonorCoreManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 54, 19, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Dispose", _m_Dispose);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DriveUpdate", _m_DriveUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Fire", _m_Fire);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Subscribe", _m_Subscribe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Unsubscribe", _m_Unsubscribe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaySound", _m_PlaySound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopSound", _m_StopSound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAllSoundVolume", _m_SetAllSoundVolume);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadConfigs", _m_LoadConfigs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetConfigBool", _m_GetConfigBool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetConfigInt", _m_GetConfigInt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetConfigString", _m_GetConfigString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "VibratePlay", _m_VibratePlay);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "VibrateStopAll", _m_VibrateStopAll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddTimerCounter", _m_AddTimerCounter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveTimer", _m_RemoveTimer);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLanguage", _m_SetLanguage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetLocalizationText", _m_GetLocalizationText);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SavePersist", _m_SavePersist);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitLuaEnv", _m_InitLuaEnv);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetLua", _m_ResetLua);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FireNow", _m_FireNow);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetConfigFloat", _m_GetConfigFloat);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCurBuildPlatformName", _m_GetCurBuildPlatformName);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetDefaultData", _m_GetDefaultData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadDefaultLanguages", _m_LoadDefaultLanguages);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitCurLanguage", _m_InitCurLanguage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadDefaultDatas", _m_LoadDefaultDatas);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadFontDatas", _m_LoadFontDatas);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetFontData", _m_GetFontData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitLuaConfigs", _m_InitLuaConfigs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EnsureLuaScriptPathsBuilt", _m_EnsureLuaScriptPathsBuilt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PrefetchNextLuaModule", _m_PrefetchNextLuaModule);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BuildLuaPreloadExecutionPlan", _m_BuildLuaPreloadExecutionPlan);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ExecuteNextLuaPreloadModule", _m_ExecuteNextLuaPreloadModule);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ExecuteStartGameBody", _m_ExecuteStartGameBody);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitLuaBindings", _m_InitLuaBindings);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitGameLuaBindings", _m_InitGameLuaBindings);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopGroupSound", _m_StopGroupSound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetEnable", _m_SetEnable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasItem", _m_HasItem);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetString", _m_GetString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetString", _m_SetString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetInt", _m_GetInt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetInt", _m_SetInt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetBool", _m_GetBool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetBool", _m_SetBool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Save", _m_Save);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveAllItems", _m_RemoveAllItems);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Play", _m_Play);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsSupported", _m_IsSupported);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetEnable", _m_GetEnable);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaPrefetchModuleCount", _g_get_LuaPrefetchModuleCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Config", _g_get_Config);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Event", _g_get_Event);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Lua", _g_get_Lua);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Sound", _g_get_Sound);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Table", _g_get_Table);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Timer", _g_get_Timer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Vibrate", _g_get_Vibrate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Localization", _g_get_Localization);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Persist", _g_get_Persist);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Playing", _g_get_Playing);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsCoreReady", _g_get_IsCoreReady);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "HasTimer", _g_get_HasTimer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "HasLocalization", _g_get_HasLocalization);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "HasPersist", _g_get_HasPersist);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaEnv", _g_get_LuaEnv);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Language", _g_get_Language);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LanguageName", _g_get_LanguageName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AutoFontAdapt", _g_get_AutoFontAdapt);
            
			
			
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
					
					Honor.Runtime.HonorCoreManager gen_ret = new Honor.Runtime.HonorCoreManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Dispose(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Dispose(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DriveUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.DriveUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Fire(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<object>(L, 2)&& translator.Assignable<Honor.Runtime.GameEventCmd>(L, 3)&& translator.Assignable<System.Collections.Generic.Dictionary<string, object>>(L, 4)) 
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    Honor.Runtime.GameEventCmd _cmd;translator.Get(L, 3, out _cmd);
                    System.Collections.Generic.Dictionary<string, object> _objects = (System.Collections.Generic.Dictionary<string, object>)translator.GetObject(L, 4, typeof(System.Collections.Generic.Dictionary<string, object>));
                    
                    gen_to_be_invoked.Fire( _sender, _cmd, _objects );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<object>(L, 2)&& translator.Assignable<Honor.Runtime.GameEventCmd>(L, 3)) 
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    Honor.Runtime.GameEventCmd _cmd;translator.Get(L, 3, out _cmd);
                    
                    gen_to_be_invoked.Fire( _sender, _cmd );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager.Fire!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Subscribe(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameEventCmd _cmd;translator.Get(L, 2, out _cmd);
                    object _userData = translator.GetObject(L, 3, typeof(object));
                    Honor.Runtime.HonorEventHandler<Honor.Runtime.EventParams> _handler = translator.GetDelegate<Honor.Runtime.HonorEventHandler<Honor.Runtime.EventParams>>(L, 4);
                    
                    gen_to_be_invoked.Subscribe( _cmd, _userData, _handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unsubscribe(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameEventCmd _cmd;translator.Get(L, 2, out _cmd);
                    object _userData = translator.GetObject(L, 3, typeof(object));
                    Honor.Runtime.HonorEventHandler<Honor.Runtime.EventParams> _handler = translator.GetDelegate<Honor.Runtime.HonorEventHandler<Honor.Runtime.EventParams>>(L, 4);
                    
                    gen_to_be_invoked.Unsubscribe( _cmd, _userData, _handler );
                    
                    
                    
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
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager.PlaySound!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopSound(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager.StopSound!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAllSoundVolume(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_LoadConfigs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LoadConfigs(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetConfigBool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    string _configName = LuaAPI.lua_tostring(L, 2);
                    bool _onPlatform = LuaAPI.lua_toboolean(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.GetConfigBool( _configName, _onPlatform );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _configName = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.GetConfigBool( _configName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager.GetConfigBool!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetConfigInt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    string _configName = LuaAPI.lua_tostring(L, 2);
                    bool _onPlatform = LuaAPI.lua_toboolean(L, 3);
                    
                        int gen_ret = gen_to_be_invoked.GetConfigInt( _configName, _onPlatform );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _configName = LuaAPI.lua_tostring(L, 2);
                    
                        int gen_ret = gen_to_be_invoked.GetConfigInt( _configName );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager.GetConfigInt!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetConfigString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    string _configName = LuaAPI.lua_tostring(L, 2);
                    bool _onPlatform = LuaAPI.lua_toboolean(L, 3);
                    
                        string gen_ret = gen_to_be_invoked.GetConfigString( _configName, _onPlatform );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _configName = LuaAPI.lua_tostring(L, 2);
                    
                        string gen_ret = gen_to_be_invoked.GetConfigString( _configName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager.GetConfigString!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_VibratePlay(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.VibratePlay(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_VibrateStopAll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.VibrateStopAll(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddTimerCounter(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Action<string>>(L, 3)&& translator.Assignable<UnityEngine.GameObject>(L, 4)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)) 
                {
                    float _time = (float)LuaAPI.lua_tonumber(L, 2);
                    System.Action<string> _del = translator.GetDelegate<System.Action<string>>(L, 3);
                    UnityEngine.GameObject _obj = (UnityEngine.GameObject)translator.GetObject(L, 4, typeof(UnityEngine.GameObject));
                    string _owner = LuaAPI.lua_tostring(L, 5);
                    
                        Honor.Runtime.TimerCounter gen_ret = gen_to_be_invoked.AddTimerCounter( _time, _del, _obj, _owner );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Action<string>>(L, 3)&& translator.Assignable<UnityEngine.GameObject>(L, 4)) 
                {
                    float _time = (float)LuaAPI.lua_tonumber(L, 2);
                    System.Action<string> _del = translator.GetDelegate<System.Action<string>>(L, 3);
                    UnityEngine.GameObject _obj = (UnityEngine.GameObject)translator.GetObject(L, 4, typeof(UnityEngine.GameObject));
                    
                        Honor.Runtime.TimerCounter gen_ret = gen_to_be_invoked.AddTimerCounter( _time, _del, _obj );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager.AddTimerCounter!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveTimer(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _owner = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.RemoveTimer( _owner );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLanguage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.GameDefinitions.Language>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    Honor.Runtime.GameDefinitions.Language _language;translator.Get(L, 2, out _language);
                    bool _launchSet = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.SetLanguage( _language, _launchSet );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.GameDefinitions.Language>(L, 2)) 
                {
                    Honor.Runtime.GameDefinitions.Language _language;translator.Get(L, 2, out _language);
                    
                    gen_to_be_invoked.SetLanguage( _language );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager.SetLanguage!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLocalizationText(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _keyName = LuaAPI.lua_tostring(L, 2);
                    
                        string gen_ret = gen_to_be_invoked.GetLocalizationText( _keyName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SavePersist(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    
                    gen_to_be_invoked.SavePersist( _wayType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitLuaEnv(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitLuaEnv(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetLua(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetLua(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FireNow(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<object>(L, 2)&& translator.Assignable<Honor.Runtime.GameEventCmd>(L, 3)&& translator.Assignable<System.Collections.Generic.Dictionary<string, object>>(L, 4)) 
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    Honor.Runtime.GameEventCmd _cmd;translator.Get(L, 3, out _cmd);
                    System.Collections.Generic.Dictionary<string, object> _objects = (System.Collections.Generic.Dictionary<string, object>)translator.GetObject(L, 4, typeof(System.Collections.Generic.Dictionary<string, object>));
                    
                    gen_to_be_invoked.FireNow( _sender, _cmd, _objects );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<object>(L, 2)&& translator.Assignable<Honor.Runtime.GameEventCmd>(L, 3)) 
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    Honor.Runtime.GameEventCmd _cmd;translator.Get(L, 3, out _cmd);
                    
                    gen_to_be_invoked.FireNow( _sender, _cmd );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager.FireNow!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetConfigFloat(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    string _configName = LuaAPI.lua_tostring(L, 2);
                    bool _onPlatform = LuaAPI.lua_toboolean(L, 3);
                    
                        float gen_ret = gen_to_be_invoked.GetConfigFloat( _configName, _onPlatform );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _configName = LuaAPI.lua_tostring(L, 2);
                    
                        float gen_ret = gen_to_be_invoked.GetConfigFloat( _configName );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager.GetConfigFloat!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurBuildPlatformName(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        string gen_ret = gen_to_be_invoked.GetCurBuildPlatformName(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDefaultData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _keyName = LuaAPI.lua_tostring(L, 2);
                    
                        string gen_ret = gen_to_be_invoked.GetDefaultData( _keyName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadDefaultLanguages(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LoadDefaultLanguages(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitCurLanguage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitCurLanguage(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadDefaultDatas(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LoadDefaultDatas(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadFontDatas(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LoadFontDatas(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFontData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameDefinitions.Language _language;translator.Get(L, 2, out _language);
                    System.Collections.Generic.List<Honor.Runtime.LocalizationFontData> _fontDatas;
                    
                    gen_to_be_invoked.GetFontData( _language, out _fontDatas );
                    translator.Push(L, _fontDatas);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitLuaConfigs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitLuaConfigs(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EnsureLuaScriptPathsBuilt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.EnsureLuaScriptPathsBuilt(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PrefetchNextLuaModule(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.PrefetchNextLuaModule(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BuildLuaPreloadExecutionPlan(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.BuildLuaPreloadExecutionPlan(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ExecuteNextLuaPreloadModule(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.ExecuteNextLuaPreloadModule(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ExecuteStartGameBody(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.ExecuteStartGameBody(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitLuaBindings(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitLuaBindings(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitGameLuaBindings(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitGameLuaBindings(  );
                    
                    
                    
                    return 0;
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
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_SetEnable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _enable = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetEnable( _enable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasItem(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    
                        bool gen_ret = gen_to_be_invoked.HasItem( _wayType, _classifyName, _itemName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    
                        string gen_ret = gen_to_be_invoked.GetString( _wayType, _classifyName, _itemName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    string _defaultValue = LuaAPI.lua_tostring(L, 5);
                    
                        string gen_ret = gen_to_be_invoked.GetString( _wayType, _classifyName, _itemName, _defaultValue );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager.GetString!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    string _value = LuaAPI.lua_tostring(L, 5);
                    
                    gen_to_be_invoked.SetString( _wayType, _classifyName, _itemName, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    
                        int gen_ret = gen_to_be_invoked.GetInt( _wayType, _classifyName, _itemName );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetInt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    int _value = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.SetInt( _wayType, _classifyName, _itemName, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    
                        bool gen_ret = gen_to_be_invoked.GetBool( _wayType, _classifyName, _itemName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    bool _defaultValue = LuaAPI.lua_toboolean(L, 5);
                    
                        bool gen_ret = gen_to_be_invoked.GetBool( _wayType, _classifyName, _itemName, _defaultValue );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager.GetBool!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetBool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    bool _value = LuaAPI.lua_toboolean(L, 5);
                    
                    gen_to_be_invoked.SetBool( _wayType, _classifyName, _itemName, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Save(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    
                    gen_to_be_invoked.Save( _wayType );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.Save( _wayType, _classifyName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager.Save!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveAllItems(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    
                    gen_to_be_invoked.RemoveAllItems( _wayType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Play(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.Play(  );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.VibrateType>(L, 2)) 
                {
                    Honor.Runtime.VibrateType _type;translator.Get(L, 2, out _type);
                    
                    gen_to_be_invoked.Play( _type );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorCoreManager.Play!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsSupported(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.IsSupported(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetEnable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.GetEnable(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaPrefetchModuleCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.LuaPrefetchModuleCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Config(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Config);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Event(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Event);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Lua(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Lua);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Sound(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Sound);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Table(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Table);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Timer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Timer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Vibrate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Vibrate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Localization(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Localization);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Persist(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Persist);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Playing(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Playing);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsCoreReady(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsCoreReady);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HasTimer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.HasTimer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HasLocalization(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.HasLocalization);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HasPersist(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.HasPersist);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaEnv(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaEnv);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Language(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                translator.PushHonorRuntimeGameDefinitionsLanguage(L, gen_to_be_invoked.Language);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LanguageName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.LanguageName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AutoFontAdapt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorCoreManager gen_to_be_invoked = (Honor.Runtime.HonorCoreManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.AutoFontAdapt);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
