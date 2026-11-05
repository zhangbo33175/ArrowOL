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
    public class HonorRuntimeHonorConstantsPersistIAPItemKeyWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.HonorConstants.Persist.IAP.ItemKey);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 20, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "OrderingStates", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.OrderingStates);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NonConsumeDotDatas", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.NonConsumeDotDatas);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SubscriptionDotDatas", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.SubscriptionDotDatas);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SubscriptionOrderTableIDs", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.SubscriptionOrderTableIDs);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SubscriptionInBuyID", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.SubscriptionInBuyID);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedOrderIDs", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.FinishedOrderIDs);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedFailedOrderIDs", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.FinishedFailedOrderIDs);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedOrderFirstTime", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.FinishedOrderFirstTime);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedOrderLatestTime", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.FinishedOrderLatestTime);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedOrderLatestTimeInterval", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.FinishedOrderLatestTimeInterval);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedOrderTotalCount", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.FinishedOrderTotalCount);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedOrderTotalMoney", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.FinishedOrderTotalMoney);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedUserLayerInfo", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.FinishedUserLayerInfo);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ThirdOrderingStates", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.ThirdOrderingStates);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ThirdFinishOrderIDs", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.ThirdFinishOrderIDs);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ThirdFailedOrderIDs", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.ThirdFailedOrderIDs);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AmazonOrderingStates", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.AmazonOrderingStates);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AmazonFinishOrderIDs", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.AmazonFinishOrderIDs);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AmazonFailedOrderIDs", Honor.Runtime.HonorConstants.Persist.IAP.ItemKey.AmazonFailedOrderIDs);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.HonorConstants.Persist.IAP.ItemKey does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        
        
        
        
        
		
		
		
		
    }
}
