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
    public class HonorRuntimeGamePathUtilsABWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.GamePathUtils.AB);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 4, 2, 2);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetExcelRootDirectoryFullPath", _m_GetExcelRootDirectoryFullPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetExcelFileFullPath", _m_GetExcelFileFullPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetVersionGrayFileName", _m_GetVersionGrayFileName_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "DirectoryPrefix", _g_get_DirectoryPrefix);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "VersionFileName", _g_get_VersionFileName);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "DirectoryPrefix", _s_set_DirectoryPrefix);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "VersionFileName", _s_set_VersionFileName);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.GamePathUtils.AB does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetExcelRootDirectoryFullPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        string gen_ret = Honor.Runtime.GamePathUtils.AB.GetExcelRootDirectoryFullPath(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetExcelFileFullPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        string gen_ret = Honor.Runtime.GamePathUtils.AB.GetExcelFileFullPath(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetVersionGrayFileName_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _appVersion = LuaAPI.lua_tostring(L, 1);
                    
                        string gen_ret = Honor.Runtime.GamePathUtils.AB.GetVersionGrayFileName( _appVersion );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DirectoryPrefix(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Honor.Runtime.GamePathUtils.AB.DirectoryPrefix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_VersionFileName(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Honor.Runtime.GamePathUtils.AB.VersionFileName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DirectoryPrefix(RealStatePtr L)
        {
		    try {
                
			    Honor.Runtime.GamePathUtils.AB.DirectoryPrefix = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_VersionFileName(RealStatePtr L)
        {
		    try {
                
			    Honor.Runtime.GamePathUtils.AB.VersionFileName = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
