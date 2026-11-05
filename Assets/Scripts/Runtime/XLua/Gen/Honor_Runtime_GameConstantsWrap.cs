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
    public class HonorRuntimeHonorConstantsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.HonorConstants);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 16, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MinGameVersion", Honor.Runtime.HonorConstants.MinGameVersion);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "HonorVersion", Honor.Runtime.HonorConstants.HonorVersion);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LaunchUIZOrder", Honor.Runtime.HonorConstants.LaunchUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SplashUIZOrder", Honor.Runtime.HonorConstants.SplashUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "HotfixUIZOrder", Honor.Runtime.HonorConstants.HotfixUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "HotfixErrorUIZOrder", Honor.Runtime.HonorConstants.HotfixErrorUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WebGLUIZOrder", Honor.Runtime.HonorConstants.WebGLUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PreloadUIZOrder", Honor.Runtime.HonorConstants.PreloadUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AppDownloadUIZOrder", Honor.Runtime.HonorConstants.AppDownloadUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WaitingUIZOrder", Honor.Runtime.HonorConstants.WaitingUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ProcedureTransitionUIZOrder", Honor.Runtime.HonorConstants.ProcedureTransitionUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GDPRUIZOrder", Honor.Runtime.HonorConstants.GDPRUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AppReviewUIZOrder", Honor.Runtime.HonorConstants.AppReviewUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AppFeedbackUIZOrder", Honor.Runtime.HonorConstants.AppFeedbackUIZOrder);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FloatWordsUIZOrder", Honor.Runtime.HonorConstants.FloatWordsUIZOrder);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.HonorConstants does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        
        
        
        
        
		
		
		
		
    }
}
