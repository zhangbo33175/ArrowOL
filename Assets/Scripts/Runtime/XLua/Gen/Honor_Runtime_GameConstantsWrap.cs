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
    public class HonorRuntimeGameConstantsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.GameConstants);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 16, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MinGameVersion", Honor.Runtime.GameConstants.MinGameVersion);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "HonorVersion", Honor.Runtime.GameConstants.HonorVersion);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LaunchUIZOrder", Honor.Runtime.GameConstants.LaunchUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SplashUIZOrder", Honor.Runtime.GameConstants.SplashUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "HotfixUIZOrder", Honor.Runtime.GameConstants.HotfixUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "HotfixErrorUIZOrder", Honor.Runtime.GameConstants.HotfixErrorUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WebGLUIZOrder", Honor.Runtime.GameConstants.WebGLUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PreloadUIZOrder", Honor.Runtime.GameConstants.PreloadUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AppDownloadUIZOrder", Honor.Runtime.GameConstants.AppDownloadUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WaitingUIZOrder", Honor.Runtime.GameConstants.WaitingUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ProcedureTransitionUIZOrder", Honor.Runtime.GameConstants.ProcedureTransitionUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GDPRUIZOrder", Honor.Runtime.GameConstants.GDPRUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AppReviewUIZOrder", Honor.Runtime.GameConstants.AppReviewUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AppFeedbackUIZOrder", Honor.Runtime.GameConstants.AppFeedbackUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FloatWordsUIZOrder", Honor.Runtime.GameConstants.FloatWordsUIZOrder);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.GameConstants does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        
        
        
        
        
		
		
		
		
    }
}
