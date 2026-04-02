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
    public class HonorRuntimeGamePathUtilsEditorLuaCloudScriptWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.GamePathUtils.Editor.LuaCloudScript);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 2, 2);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ProjectSettingFileFullPath", _g_get_ProjectSettingFileFullPath);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "LibraryLocalLuaFileFullPath", _g_get_LibraryLocalLuaFileFullPath);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ProjectSettingFileFullPath", _s_set_ProjectSettingFileFullPath);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "LibraryLocalLuaFileFullPath", _s_set_LibraryLocalLuaFileFullPath);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.GamePathUtils.Editor.LuaCloudScript does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ProjectSettingFileFullPath(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Honor.Runtime.GamePathUtils.Editor.LuaCloudScript.ProjectSettingFileFullPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LibraryLocalLuaFileFullPath(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, Honor.Runtime.GamePathUtils.Editor.LuaCloudScript.LibraryLocalLuaFileFullPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ProjectSettingFileFullPath(RealStatePtr L)
        {
		    try {
                
			    Honor.Runtime.GamePathUtils.Editor.LuaCloudScript.ProjectSettingFileFullPath = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LibraryLocalLuaFileFullPath(RealStatePtr L)
        {
		    try {
                
			    Honor.Runtime.GamePathUtils.Editor.LuaCloudScript.LibraryLocalLuaFileFullPath = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
