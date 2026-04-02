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
    public class HonorRuntimeGameConstantsPersistIAPItemKeyWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.GameConstants.Persist.IAP.ItemKey);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 20, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "OrderingStates", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.OrderingStates);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NonConsumeDotDatas", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.NonConsumeDotDatas);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SubscriptionDotDatas", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.SubscriptionDotDatas);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SubscriptionOrderTableIDs", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.SubscriptionOrderTableIDs);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SubscriptionInBuyID", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.SubscriptionInBuyID);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedOrderIDs", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.FinishedOrderIDs);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedFailedOrderIDs", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.FinishedFailedOrderIDs);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedOrderFirstTime", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.FinishedOrderFirstTime);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedOrderLatestTime", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.FinishedOrderLatestTime);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedOrderLatestTimeInterval", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.FinishedOrderLatestTimeInterval);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedOrderTotalCount", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.FinishedOrderTotalCount);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedOrderTotalMoney", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.FinishedOrderTotalMoney);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FinishedUserLayerInfo", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.FinishedUserLayerInfo);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ThirdOrderingStates", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.ThirdOrderingStates);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ThirdFinishOrderIDs", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.ThirdFinishOrderIDs);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ThirdFailedOrderIDs", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.ThirdFailedOrderIDs);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AmazonOrderingStates", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.AmazonOrderingStates);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AmazonFinishOrderIDs", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.AmazonFinishOrderIDs);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AmazonFailedOrderIDs", Honor.Runtime.GameConstants.Persist.IAP.ItemKey.AmazonFailedOrderIDs);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.GameConstants.Persist.IAP.ItemKey does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        
        
        
        
        
		
		
		
		
    }
}
