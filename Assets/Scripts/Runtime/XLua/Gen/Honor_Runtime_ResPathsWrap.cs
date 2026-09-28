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
    public class HonorRuntimeResPathsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.ResPaths);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 7, 1, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetResourcePath", _m_GetResourcePath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRoot", _m_GetRoot_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetFullPathWithSuffix", _m_GetFullPathWithSuffix_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetPersistentDataPath", _m_GetPersistentDataPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PathFormat", _m_PathFormat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsFileExist", _m_IsFileExist_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "LoadAB", _g_get_LoadAB);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "LoadAB", _s_set_LoadAB);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.ResPaths does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetResourcePath_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _resName = LuaAPI.lua_tostring(L, 1);
                    Honor.Runtime.ResPaths.PathsEnum _type;translator.Get(L, 2, out _type);
                    
                        string gen_ret = Honor.Runtime.ResPaths.GetResourcePath( _resName, _type );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRoot_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Honor.Runtime.ResPaths.PathsEnum _type;translator.Get(L, 1, out _type);
                    
                        string gen_ret = Honor.Runtime.ResPaths.GetRoot( _type );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFullPathWithSuffix_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _assetPath = LuaAPI.lua_tostring(L, 1);
                    
                        string gen_ret = Honor.Runtime.ResPaths.GetFullPathWithSuffix( _assetPath );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPersistentDataPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    
                        string gen_ret = Honor.Runtime.ResPaths.GetPersistentDataPath( _name );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PathFormat_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 1);
                    
                        string gen_ret = Honor.Runtime.ResPaths.PathFormat( _path );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsFileExist_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _fullPath = LuaAPI.lua_tostring(L, 1);
                    
                        bool gen_ret = Honor.Runtime.ResPaths.IsFileExist( _fullPath );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadAB(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Honor.Runtime.ResPaths.LoadAB);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LoadAB(RealStatePtr L)
        {
		    try {
                
			    Honor.Runtime.ResPaths.LoadAB = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
