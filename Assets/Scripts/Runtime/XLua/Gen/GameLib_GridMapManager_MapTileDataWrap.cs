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
    public class GameLibGridMapManagerMapTileDataWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLib.GridMapManager.MapTileData);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 5, 5);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "tileId", _g_get_tileId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gridPosition", _g_get_gridPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "tilePath", _g_get_tilePath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "prefab", _g_get_prefab);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LayerLevel", _g_get_LayerLevel);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "tileId", _s_set_tileId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "gridPosition", _s_set_gridPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "tilePath", _s_set_tilePath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "prefab", _s_set_prefab);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LayerLevel", _s_set_LayerLevel);
            
			
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
					
					GameLib.GridMapManager.MapTileData gen_ret = new GameLib.GridMapManager.MapTileData();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameLib.GridMapManager.MapTileData constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_tileId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.MapTileData gen_to_be_invoked = (GameLib.GridMapManager.MapTileData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.tileId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gridPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.MapTileData gen_to_be_invoked = (GameLib.GridMapManager.MapTileData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.gridPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_tilePath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.MapTileData gen_to_be_invoked = (GameLib.GridMapManager.MapTileData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.tilePath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_prefab(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.MapTileData gen_to_be_invoked = (GameLib.GridMapManager.MapTileData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.prefab);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LayerLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.MapTileData gen_to_be_invoked = (GameLib.GridMapManager.MapTileData)translator.FastGetCSObj(L, 1);
                translator.PushGameLibGridMapManagerLayerLevel(L, gen_to_be_invoked.LayerLevel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_tileId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.MapTileData gen_to_be_invoked = (GameLib.GridMapManager.MapTileData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.tileId = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gridPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.MapTileData gen_to_be_invoked = (GameLib.GridMapManager.MapTileData)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2Int gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.gridPosition = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_tilePath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.MapTileData gen_to_be_invoked = (GameLib.GridMapManager.MapTileData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.tilePath = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_prefab(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.MapTileData gen_to_be_invoked = (GameLib.GridMapManager.MapTileData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.prefab = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LayerLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.MapTileData gen_to_be_invoked = (GameLib.GridMapManager.MapTileData)translator.FastGetCSObj(L, 1);
                GameLib.GridMapManager.LayerLevel gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.LayerLevel = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
