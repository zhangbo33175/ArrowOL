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
    public class GameLibGridMapManagerGridTileWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLib.GridMapManager.GridTile);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 4, 3);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "GridPosition", _g_get_GridPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsOccupied", _g_get_IsOccupied);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Occupant", _g_get_Occupant);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ObjectType", _g_get_ObjectType);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsOccupied", _s_set_IsOccupied);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Occupant", _s_set_Occupant);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ObjectType", _s_set_ObjectType);
            
			
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
				if(LuaAPI.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3))
				{
					int _x = LuaAPI.xlua_tointeger(L, 2);
					int _y = LuaAPI.xlua_tointeger(L, 3);
					
					GameLib.GridMapManager.GridTile gen_ret = new GameLib.GridMapManager.GridTile(_x, _y);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameLib.GridMapManager.GridTile constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GridPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.GridTile gen_to_be_invoked = (GameLib.GridMapManager.GridTile)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.GridPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsOccupied(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.GridTile gen_to_be_invoked = (GameLib.GridMapManager.GridTile)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsOccupied);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Occupant(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.GridTile gen_to_be_invoked = (GameLib.GridMapManager.GridTile)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Occupant);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ObjectType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.GridTile gen_to_be_invoked = (GameLib.GridMapManager.GridTile)translator.FastGetCSObj(L, 1);
                translator.PushGameLibGridMapManagerGridType(L, gen_to_be_invoked.ObjectType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsOccupied(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.GridTile gen_to_be_invoked = (GameLib.GridMapManager.GridTile)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsOccupied = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Occupant(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.GridTile gen_to_be_invoked = (GameLib.GridMapManager.GridTile)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Occupant = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ObjectType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager.GridTile gen_to_be_invoked = (GameLib.GridMapManager.GridTile)translator.FastGetCSObj(L, 1);
                GameLib.GridMapManager.GridType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.ObjectType = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
