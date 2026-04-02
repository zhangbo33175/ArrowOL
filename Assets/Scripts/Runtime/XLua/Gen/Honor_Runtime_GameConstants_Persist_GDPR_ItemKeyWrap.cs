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
    public class HonorRuntimeGameConstantsPersistGDPRItemKeyWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.GameConstants.Persist.GDPR.ItemKey);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 6, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GDPROver", Honor.Runtime.GameConstants.Persist.GDPR.ItemKey.GDPROver);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GDPRNeedFlagFromNet", Honor.Runtime.GameConstants.Persist.GDPR.ItemKey.GDPRNeedFlagFromNet);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "HasUserConsent", Honor.Runtime.GameConstants.Persist.GDPR.ItemKey.HasUserConsent);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IsSell", Honor.Runtime.GameConstants.Persist.GDPR.ItemKey.IsSell);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IsAgeReachStandard", Honor.Runtime.GameConstants.Persist.GDPR.ItemKey.IsAgeReachStandard);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.GameConstants.Persist.GDPR.ItemKey does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        
        
        
        
        
		
		
		
		
    }
}
