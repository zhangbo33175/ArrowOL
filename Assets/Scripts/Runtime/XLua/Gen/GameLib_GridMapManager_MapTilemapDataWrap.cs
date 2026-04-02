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
    public class GameLibGridMapManagerMapTilemapDataWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLib.GridMapManager.MapTilemapData);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 1, 1);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "layers", _g_get_layers);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "layers", _s_set_layers);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					GameLib.GridMapManager.MapTilemapData gen_ret = new GameLib.GridMapManager.MapTilemapData();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameLib.GridMapManager.MapTilemapData constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_layers(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.MapTilemapData gen_to_be_invoked = (GameLib.GridMapManager.MapTilemapData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.layers);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_layers(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.MapTilemapData gen_to_be_invoked = (GameLib.GridMapManager.MapTilemapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.layers = (System.Collections.Generic.List<GameLib.GridMapManager.LayerData>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<GameLib.GridMapManager.LayerData>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
