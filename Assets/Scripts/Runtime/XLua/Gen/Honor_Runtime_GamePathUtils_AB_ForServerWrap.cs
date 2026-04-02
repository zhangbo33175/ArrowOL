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
    public class HonorRuntimeGamePathUtilsABForServerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.GamePathUtils.AB.ForServer);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 5, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetPlatformFolderFullPath", _m_GetPlatformFolderFullPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRootDirectoryFullPath", _m_GetRootDirectoryFullPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetVersionFileFullPath", _m_GetVersionFileFullPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetFileFullPath", _m_GetFileFullPath_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.GamePathUtils.AB.ForServer does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPlatformFolderFullPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _platformName = LuaAPI.lua_tostring(L, 1);
                    
                        string gen_ret = Honor.Runtime.GamePathUtils.AB.ForServer.GetPlatformFolderFullPath( _platformName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRootDirectoryFullPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _platformName = LuaAPI.lua_tostring(L, 1);
                    string _appVersion = LuaAPI.lua_tostring(L, 2);
                    int _resMinor = LuaAPI.xlua_tointeger(L, 3);
                    
                        string gen_ret = Honor.Runtime.GamePathUtils.AB.ForServer.GetRootDirectoryFullPath( _platformName, _appVersion, _resMinor );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetVersionFileFullPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _platformName = LuaAPI.lua_tostring(L, 1);
                    string _version = LuaAPI.lua_tostring(L, 2);
                    
                        string gen_ret = Honor.Runtime.GamePathUtils.AB.ForServer.GetVersionFileFullPath( _platformName, _version );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _platformName = LuaAPI.lua_tostring(L, 1);
                    string _appVersion = LuaAPI.lua_tostring(L, 2);
                    int _resMinor = LuaAPI.xlua_tointeger(L, 3);
                    
                        string gen_ret = Honor.Runtime.GamePathUtils.AB.ForServer.GetVersionFileFullPath( _platformName, _appVersion, _resMinor );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.GamePathUtils.AB.ForServer.GetVersionFileFullPath!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFileFullPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _platformName = LuaAPI.lua_tostring(L, 1);
                    string _appVersion = LuaAPI.lua_tostring(L, 2);
                    int _resMinor = LuaAPI.xlua_tointeger(L, 3);
                    string _formatPath = LuaAPI.lua_tostring(L, 4);
                    
                        string gen_ret = Honor.Runtime.GamePathUtils.AB.ForServer.GetFileFullPath( _platformName, _appVersion, _resMinor, _formatPath );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
