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
    public class HonorRuntimeConverterWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.Converter);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 31, 2, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetCentimetersFromPixels", _m_GetCentimetersFromPixels_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetPixelsFromCentimeters", _m_GetPixelsFromCentimeters_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetInchesFromPixels", _m_GetInchesFromPixels_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetPixelsFromInches", _m_GetPixelsFromInches_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBytesByBoolean", _m_GetBytesByBoolean_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBoolean", _m_GetBoolean_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBytesByChar", _m_GetBytesByChar_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetChar", _m_GetChar_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBytesByShort", _m_GetBytesByShort_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetInt16", _m_GetInt16_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBytesByUShort", _m_GetBytesByUShort_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUInt16", _m_GetUInt16_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBytesByInt", _m_GetBytesByInt_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetInt32", _m_GetInt32_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBytesByUInt", _m_GetBytesByUInt_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUInt32", _m_GetUInt32_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBytesByLong", _m_GetBytesByLong_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetInt64", _m_GetInt64_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBytesByULong", _m_GetBytesByULong_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUInt64", _m_GetUInt64_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBytesByFloat", _m_GetBytesByFloat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetSingle", _m_GetSingle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBytesByDouble", _m_GetBytesByDouble_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetDouble", _m_GetDouble_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBytesByString", _m_GetBytesByString_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetString", _m_GetString_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ToNoBOMUTF8", _m_ToNoBOMUTF8_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetTextureBytes", _m_GetTextureBytes_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsBase64String", _m_IsBase64String_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsBase64Bytes", _m_IsBase64Bytes_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "IsLittleEndian", _g_get_IsLittleEndian);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ScreenDpi", _g_get_ScreenDpi);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ScreenDpi", _s_set_ScreenDpi);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.Converter does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCentimetersFromPixels_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float _pixels = (float)LuaAPI.lua_tonumber(L, 1);
                    
                        float gen_ret = Honor.Runtime.Converter.GetCentimetersFromPixels( _pixels );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPixelsFromCentimeters_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float _centimeters = (float)LuaAPI.lua_tonumber(L, 1);
                    
                        float gen_ret = Honor.Runtime.Converter.GetPixelsFromCentimeters( _centimeters );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInchesFromPixels_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float _pixels = (float)LuaAPI.lua_tonumber(L, 1);
                    
                        float gen_ret = Honor.Runtime.Converter.GetInchesFromPixels( _pixels );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPixelsFromInches_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float _inches = (float)LuaAPI.lua_tonumber(L, 1);
                    
                        float gen_ret = Honor.Runtime.Converter.GetPixelsFromInches( _inches );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBytesByBoolean_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)) 
                {
                    bool _value = LuaAPI.lua_toboolean(L, 1);
                    
                        byte[] gen_ret = Honor.Runtime.Converter.GetBytesByBoolean( _value );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    bool _value = LuaAPI.lua_toboolean(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    
                    Honor.Runtime.Converter.GetBytesByBoolean( _value, _buffer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    bool _value = LuaAPI.lua_toboolean(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    Honor.Runtime.Converter.GetBytesByBoolean( _value, _buffer, _startIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetBytesByBoolean!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBoolean_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    
                        bool gen_ret = Honor.Runtime.Converter.GetBoolean( _value );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool gen_ret = Honor.Runtime.Converter.GetBoolean( _value, _startIndex );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetBoolean!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBytesByChar_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    char _value = (char)LuaAPI.xlua_tointeger(L, 1);
                    
                        byte[] gen_ret = Honor.Runtime.Converter.GetBytesByChar( _value );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    char _value = (char)LuaAPI.xlua_tointeger(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    
                    Honor.Runtime.Converter.GetBytesByChar( _value, _buffer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    char _value = (char)LuaAPI.xlua_tointeger(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    Honor.Runtime.Converter.GetBytesByChar( _value, _buffer, _startIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetBytesByChar!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetChar_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    
                        char gen_ret = Honor.Runtime.Converter.GetChar( _value );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        char gen_ret = Honor.Runtime.Converter.GetChar( _value, _startIndex );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetChar!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBytesByShort_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    short _value = (short)LuaAPI.xlua_tointeger(L, 1);
                    
                        byte[] gen_ret = Honor.Runtime.Converter.GetBytesByShort( _value );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    short _value = (short)LuaAPI.xlua_tointeger(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    
                    Honor.Runtime.Converter.GetBytesByShort( _value, _buffer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    short _value = (short)LuaAPI.xlua_tointeger(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    Honor.Runtime.Converter.GetBytesByShort( _value, _buffer, _startIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetBytesByShort!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInt16_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    
                        short gen_ret = Honor.Runtime.Converter.GetInt16( _value );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        short gen_ret = Honor.Runtime.Converter.GetInt16( _value, _startIndex );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetInt16!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBytesByUShort_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    ushort _value = (ushort)LuaAPI.xlua_tointeger(L, 1);
                    
                        byte[] gen_ret = Honor.Runtime.Converter.GetBytesByUShort( _value );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    ushort _value = (ushort)LuaAPI.xlua_tointeger(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    
                    Honor.Runtime.Converter.GetBytesByUShort( _value, _buffer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    ushort _value = (ushort)LuaAPI.xlua_tointeger(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    Honor.Runtime.Converter.GetBytesByUShort( _value, _buffer, _startIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetBytesByUShort!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUInt16_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    
                        ushort gen_ret = Honor.Runtime.Converter.GetUInt16( _value );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        ushort gen_ret = Honor.Runtime.Converter.GetUInt16( _value, _startIndex );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetUInt16!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBytesByInt_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    int _value = LuaAPI.xlua_tointeger(L, 1);
                    
                        byte[] gen_ret = Honor.Runtime.Converter.GetBytesByInt( _value );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    int _value = LuaAPI.xlua_tointeger(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    
                    Honor.Runtime.Converter.GetBytesByInt( _value, _buffer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _value = LuaAPI.xlua_tointeger(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    Honor.Runtime.Converter.GetBytesByInt( _value, _buffer, _startIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetBytesByInt!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInt32_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    
                        int gen_ret = Honor.Runtime.Converter.GetInt32( _value );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        int gen_ret = Honor.Runtime.Converter.GetInt32( _value, _startIndex );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetInt32!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBytesByUInt_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    uint _value = LuaAPI.xlua_touint(L, 1);
                    
                        byte[] gen_ret = Honor.Runtime.Converter.GetBytesByUInt( _value );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    uint _value = LuaAPI.xlua_touint(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    
                    Honor.Runtime.Converter.GetBytesByUInt( _value, _buffer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    uint _value = LuaAPI.xlua_touint(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    Honor.Runtime.Converter.GetBytesByUInt( _value, _buffer, _startIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetBytesByUInt!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUInt32_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    
                        uint gen_ret = Honor.Runtime.Converter.GetUInt32( _value );
                        LuaAPI.xlua_pushuint(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        uint gen_ret = Honor.Runtime.Converter.GetUInt32( _value, _startIndex );
                        LuaAPI.xlua_pushuint(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetUInt32!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBytesByLong_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1) || LuaAPI.lua_isint64(L, 1))) 
                {
                    long _value = LuaAPI.lua_toint64(L, 1);
                    
                        byte[] gen_ret = Honor.Runtime.Converter.GetBytesByLong( _value );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1) || LuaAPI.lua_isint64(L, 1))&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    long _value = LuaAPI.lua_toint64(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    
                    Honor.Runtime.Converter.GetBytesByLong( _value, _buffer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1) || LuaAPI.lua_isint64(L, 1))&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    long _value = LuaAPI.lua_toint64(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    Honor.Runtime.Converter.GetBytesByLong( _value, _buffer, _startIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetBytesByLong!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInt64_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    
                        long gen_ret = Honor.Runtime.Converter.GetInt64( _value );
                        LuaAPI.lua_pushint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        long gen_ret = Honor.Runtime.Converter.GetInt64( _value, _startIndex );
                        LuaAPI.lua_pushint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetInt64!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBytesByULong_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1) || LuaAPI.lua_isuint64(L, 1))) 
                {
                    ulong _value = LuaAPI.lua_touint64(L, 1);
                    
                        byte[] gen_ret = Honor.Runtime.Converter.GetBytesByULong( _value );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1) || LuaAPI.lua_isuint64(L, 1))&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    ulong _value = LuaAPI.lua_touint64(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    
                    Honor.Runtime.Converter.GetBytesByULong( _value, _buffer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1) || LuaAPI.lua_isuint64(L, 1))&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    ulong _value = LuaAPI.lua_touint64(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    Honor.Runtime.Converter.GetBytesByULong( _value, _buffer, _startIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetBytesByULong!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUInt64_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    
                        ulong gen_ret = Honor.Runtime.Converter.GetUInt64( _value );
                        LuaAPI.lua_pushuint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        ulong gen_ret = Honor.Runtime.Converter.GetUInt64( _value, _startIndex );
                        LuaAPI.lua_pushuint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetUInt64!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBytesByFloat_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    float _value = (float)LuaAPI.lua_tonumber(L, 1);
                    
                        byte[] gen_ret = Honor.Runtime.Converter.GetBytesByFloat( _value );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    float _value = (float)LuaAPI.lua_tonumber(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    
                    Honor.Runtime.Converter.GetBytesByFloat( _value, _buffer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    float _value = (float)LuaAPI.lua_tonumber(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    Honor.Runtime.Converter.GetBytesByFloat( _value, _buffer, _startIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetBytesByFloat!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSingle_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    
                        float gen_ret = Honor.Runtime.Converter.GetSingle( _value );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        float gen_ret = Honor.Runtime.Converter.GetSingle( _value, _startIndex );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetSingle!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBytesByDouble_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    double _value = LuaAPI.lua_tonumber(L, 1);
                    
                        byte[] gen_ret = Honor.Runtime.Converter.GetBytesByDouble( _value );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    double _value = LuaAPI.lua_tonumber(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    
                    Honor.Runtime.Converter.GetBytesByDouble( _value, _buffer );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    double _value = LuaAPI.lua_tonumber(L, 1);
                    byte[] _buffer = LuaAPI.lua_tobytes(L, 2);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 3);
                    
                    Honor.Runtime.Converter.GetBytesByDouble( _value, _buffer, _startIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetBytesByDouble!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDouble_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    
                        double gen_ret = Honor.Runtime.Converter.GetDouble( _value );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        double gen_ret = Honor.Runtime.Converter.GetDouble( _value, _startIndex );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetDouble!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBytesByString_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _value = LuaAPI.lua_tostring(L, 1);
                    
                        byte[] gen_ret = Honor.Runtime.Converter.GetBytesByString( _value );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetString_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    
                        string gen_ret = Honor.Runtime.Converter.GetString( _value );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    byte[] _value = LuaAPI.lua_tobytes(L, 1);
                    int _startIndex = LuaAPI.xlua_tointeger(L, 2);
                    int _length = LuaAPI.xlua_tointeger(L, 3);
                    
                        string gen_ret = Honor.Runtime.Converter.GetString( _value, _startIndex, _length );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Converter.GetString!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToNoBOMUTF8_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _dirName = LuaAPI.lua_tostring(L, 1);
                    string _suffixInfos = LuaAPI.lua_tostring(L, 2);
                    
                    Honor.Runtime.Converter.ToNoBOMUTF8( _dirName, _suffixInfos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTextureBytes_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Texture2D _texture = (UnityEngine.Texture2D)translator.GetObject(L, 1, typeof(UnityEngine.Texture2D));
                    bool _isJpeg = LuaAPI.lua_toboolean(L, 2);
                    
                        byte[] gen_ret = Honor.Runtime.Converter.GetTextureBytes( _texture, _isJpeg );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsBase64String_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _content = LuaAPI.lua_tostring(L, 1);
                    
                        bool gen_ret = Honor.Runtime.Converter.IsBase64String( _content );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsBase64Bytes_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    byte[] _content = LuaAPI.lua_tobytes(L, 1);
                    
                        bool gen_ret = Honor.Runtime.Converter.IsBase64Bytes( _content );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsLittleEndian(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Honor.Runtime.Converter.IsLittleEndian);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ScreenDpi(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, Honor.Runtime.Converter.ScreenDpi);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ScreenDpi(RealStatePtr L)
        {
		    try {
                
			    Honor.Runtime.Converter.ScreenDpi = (float)LuaAPI.lua_tonumber(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
