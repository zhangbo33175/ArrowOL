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
    public class HonorRuntimeHonorTweenTypeWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.HonorTweenType);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 18, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ProcedureTransitionInTween", Honor.Runtime.HonorTweenType.ProcedureTransitionInTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ProcedureTransitionOutTween", Honor.Runtime.HonorTweenType.ProcedureTransitionOutTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SplashShowTween", Honor.Runtime.HonorTweenType.SplashShowTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LauncherLoadingViewTween", Honor.Runtime.HonorTweenType.LauncherLoadingViewTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CameraMasterTween", Honor.Runtime.HonorTweenType.CameraMasterTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CameraMoveTween", Honor.Runtime.HonorTweenType.CameraMoveTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CameraRotateTween", Honor.Runtime.HonorTweenType.CameraRotateTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CameraScaleTween", Honor.Runtime.HonorTweenType.CameraScaleTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CustomVibrateDelayTween", Honor.Runtime.HonorTweenType.CustomVibrateDelayTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CustomVibrateSustainTween", Honor.Runtime.HonorTweenType.CustomVibrateSustainTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EmphasisVibrateDelayTween", Honor.Runtime.HonorTweenType.EmphasisVibrateDelayTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EmphasisVibrateSustainTween", Honor.Runtime.HonorTweenType.EmphasisVibrateSustainTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxInterstitialAdTween", Honor.Runtime.HonorTweenType.MaxInterstitialAdTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxBannerAdTween", Honor.Runtime.HonorTweenType.MaxBannerAdTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxRewardedVideoAdTween", Honor.Runtime.HonorTweenType.MaxRewardedVideoAdTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UiFadeTween", Honor.Runtime.HonorTweenType.UiFadeTween);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DebuggerLogTextTween", Honor.Runtime.HonorTweenType.DebuggerLogTextTween);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.HonorTweenType does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        
        
        
        
        
		
		
		
		
    }
}
