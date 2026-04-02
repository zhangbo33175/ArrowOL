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
    public class GameLibRSetupLuaTableValueWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLib.RSetupLuaTableValue);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 17, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "RScreenToWorldPoint", _m_RScreenToWorldPoint_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetTimeDeltaTime", _m_GetTimeDeltaTime_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRealtimeSinceStartup", _m_GetRealtimeSinceStartup_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsPointerOverUIObject", _m_IsPointerOverUIObject_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetDateTime1970AddSeconds", _m_GetDateTime1970AddSeconds_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetLocalTimeInfo", _m_GetLocalTimeInfo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetLocalTimeStamp", _m_GetLocalTimeStamp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUniversalTimeStamp", _m_GetUniversalTimeStamp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetPeriodicNotificationEndTimestamp", _m_GetPeriodicNotificationEndTimestamp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetPeriodicNotificationLocalDayDate", _m_GetPeriodicNotificationLocalDayDate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetLocalTimeStampByParams", _m_GetLocalTimeStampByParams_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUTC0TimeStamp", _m_GetUTC0TimeStamp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUTC0TimeInfo", _m_GetUTC0TimeInfo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUTC0TimeInfoByUTC0TimeStamp", _m_GetUTC0TimeInfoByUTC0TimeStamp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUTC0TimeStampByParams", _m_GetUTC0TimeStampByParams_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetTimeStringByTimeStamp", _m_GetTimeStringByTimeStamp_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "GameLib.RSetupLuaTableValue does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RScreenToWorldPoint_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    UnityEngine.Vector3 _screenPosition;translator.Get(L, 2, out _screenPosition);
                    XLua.LuaTable _worldPosition = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                    GameLib.RSetupLuaTableValue.RScreenToWorldPoint( _camera, _screenPosition, _worldPosition );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTimeDeltaTime_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        float gen_ret = GameLib.RSetupLuaTableValue.GetTimeDeltaTime(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRealtimeSinceStartup_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        float gen_ret = GameLib.RSetupLuaTableValue.GetRealtimeSinceStartup(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsPointerOverUIObject_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        bool gen_ret = GameLib.RSetupLuaTableValue.IsPointerOverUIObject(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDateTime1970AddSeconds_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _seconds = LuaAPI.xlua_tointeger(L, 1);
                    
                        GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_ret = GameLib.RSetupLuaTableValue.GetDateTime1970AddSeconds( _seconds );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLocalTimeInfo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    bool _isFromServer = LuaAPI.lua_toboolean(L, 1);
                    double _utc0SecondsFromServer = LuaAPI.lua_tonumber(L, 2);
                    
                        GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_ret = GameLib.RSetupLuaTableValue.GetLocalTimeInfo( _isFromServer, _utc0SecondsFromServer );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLocalTimeStamp_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool _isFromServer = LuaAPI.lua_toboolean(L, 1);
                    double _utc0SecondsFromServer = LuaAPI.lua_tonumber(L, 2);
                    
                        double gen_ret = GameLib.RSetupLuaTableValue.GetLocalTimeStamp( _isFromServer, _utc0SecondsFromServer );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUniversalTimeStamp_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    double _localTimeStamp = LuaAPI.lua_tonumber(L, 1);
                    
                        double gen_ret = GameLib.RSetupLuaTableValue.GetUniversalTimeStamp( _localTimeStamp );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPeriodicNotificationEndTimestamp_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    double _localTime = LuaAPI.lua_tonumber(L, 1);
                    
                        double gen_ret = GameLib.RSetupLuaTableValue.GetPeriodicNotificationEndTimestamp( _localTime );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPeriodicNotificationLocalDayDate_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    double _localTime = LuaAPI.lua_tonumber(L, 1);
                    int _delayDays = LuaAPI.xlua_tointeger(L, 2);
                    int _hours = LuaAPI.xlua_tointeger(L, 3);
                    int _minutes = LuaAPI.xlua_tointeger(L, 4);
                    
                        double gen_ret = GameLib.RSetupLuaTableValue.GetPeriodicNotificationLocalDayDate( _localTime, _delayDays, _hours, _minutes );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLocalTimeStampByParams_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _year = LuaAPI.xlua_tointeger(L, 1);
                    int _month = LuaAPI.xlua_tointeger(L, 2);
                    int _day = LuaAPI.xlua_tointeger(L, 3);
                    int _hour = LuaAPI.xlua_tointeger(L, 4);
                    int _minute = LuaAPI.xlua_tointeger(L, 5);
                    int _second = LuaAPI.xlua_tointeger(L, 6);
                    
                        double gen_ret = GameLib.RSetupLuaTableValue.GetLocalTimeStampByParams( _year, _month, _day, _hour, _minute, _second );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUTC0TimeStamp_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        double gen_ret = GameLib.RSetupLuaTableValue.GetUTC0TimeStamp(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUTC0TimeInfo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    bool _isFromServer = LuaAPI.lua_toboolean(L, 1);
                    double _utc0SecondsFromServer = LuaAPI.lua_tonumber(L, 2);
                    
                        GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_ret = GameLib.RSetupLuaTableValue.GetUTC0TimeInfo( _isFromServer, _utc0SecondsFromServer );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUTC0TimeInfoByUTC0TimeStamp_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    double _utc0TimeStamp = LuaAPI.lua_tonumber(L, 1);
                    
                        GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS gen_ret = GameLib.RSetupLuaTableValue.GetUTC0TimeInfoByUTC0TimeStamp( _utc0TimeStamp );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUTC0TimeStampByParams_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _year = LuaAPI.xlua_tointeger(L, 1);
                    int _month = LuaAPI.xlua_tointeger(L, 2);
                    int _day = LuaAPI.xlua_tointeger(L, 3);
                    int _hour = LuaAPI.xlua_tointeger(L, 4);
                    int _minute = LuaAPI.xlua_tointeger(L, 5);
                    int _second = LuaAPI.xlua_tointeger(L, 6);
                    
                        double gen_ret = GameLib.RSetupLuaTableValue.GetUTC0TimeStampByParams( _year, _month, _day, _hour, _minute, _second );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTimeStringByTimeStamp_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _format = LuaAPI.lua_tostring(L, 1);
                    double _timeStamp = LuaAPI.lua_tonumber(L, 2);
                    
                        string gen_ret = GameLib.RSetupLuaTableValue.GetTimeStringByTimeStamp( _format, _timeStamp );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
