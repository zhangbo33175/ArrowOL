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
    public class HonorRuntimeHonorPathUtilsUIWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.HonorPathUtils.UI);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 5, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetExcelRootDirectoryFullPath", _m_GetExcelRootDirectoryFullPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetExcelFileFullPath", _m_GetExcelFileFullPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetLuaScriptRootDirectoryFullPath", _m_GetLuaScriptRootDirectoryFullPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetComponentLibRootDirectoryFullPath", _m_GetComponentLibRootDirectoryFullPath_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.HonorPathUtils.UI does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetExcelRootDirectoryFullPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        string gen_ret = Honor.Runtime.HonorPathUtils.UI.GetExcelRootDirectoryFullPath(  );
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
                    
                        string gen_ret = Honor.Runtime.HonorPathUtils.UI.GetExcelFileFullPath(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetLuaScriptRootDirectoryFullPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        string gen_ret = Honor.Runtime.HonorPathUtils.UI.GetLuaScriptRootDirectoryFullPath(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetComponentLibRootDirectoryFullPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        string gen_ret = Honor.Runtime.HonorPathUtils.UI.GetComponentLibRootDirectoryFullPath(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
