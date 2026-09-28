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
    public class HonorRuntimeDefLayerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.DefLayer);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 29, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Default", Honor.Runtime.DefLayer.Default);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TransparentFX", Honor.Runtime.DefLayer.TransparentFX);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IgnoreRaycast", Honor.Runtime.DefLayer.IgnoreRaycast);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Water", Honor.Runtime.DefLayer.Water);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UGUI", Honor.Runtime.DefLayer.UGUI);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NGUI", Honor.Runtime.DefLayer.NGUI);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Temp", Honor.Runtime.DefLayer.Temp);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Scene", Honor.Runtime.DefLayer.Scene);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Light", Honor.Runtime.DefLayer.Light);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Shadow", Honor.Runtime.DefLayer.Shadow);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MapObstacle", Honor.Runtime.DefLayer.MapObstacle);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ShadowCaster", Honor.Runtime.DefLayer.ShadowCaster);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MapNode", Honor.Runtime.DefLayer.MapNode);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MapGround", Honor.Runtime.DefLayer.MapGround);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PostEffect", Honor.Runtime.DefLayer.PostEffect);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Distort", Honor.Runtime.DefLayer.Distort);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Hide", Honor.Runtime.DefLayer.Hide);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "StoryUI", Honor.Runtime.DefLayer.StoryUI);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RenderTex", Honor.Runtime.DefLayer.RenderTex);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Role", Honor.Runtime.DefLayer.Role);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BackGround", Honor.Runtime.DefLayer.BackGround);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UI3D", Honor.Runtime.DefLayer.UI3D);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TOPUI", Honor.Runtime.DefLayer.TOPUI);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FocusUI", Honor.Runtime.DefLayer.FocusUI);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CharacterFace", Honor.Runtime.DefLayer.CharacterFace);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CharacterBody", Honor.Runtime.DefLayer.CharacterBody);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CharacterLeg", Honor.Runtime.DefLayer.CharacterLeg);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BigMapEvent", Honor.Runtime.DefLayer.BigMapEvent);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.DefLayer does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        
        
        
        
        
		
		
		
		
    }
}
