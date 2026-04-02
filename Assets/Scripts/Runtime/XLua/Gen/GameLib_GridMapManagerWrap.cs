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
    public class GameLibGridMapManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLib.GridMapManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 14, 14);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaceObject", _m_PlaceObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveObject", _m_RemoveObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SaveTilemap", _m_SaveTilemap);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "gridWidth", _g_get_gridWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gridHeight", _g_get_gridHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_GridSize", _g_get_m_GridSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_GridWidth", _g_get_m_GridWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_GridHeight", _g_get_m_GridHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_CellWidth", _g_get_m_CellWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_CellHeight", _g_get_m_CellHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_NormalColor", _g_get_m_NormalColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_WalkableColor", _g_get_m_WalkableColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_BlockedColor", _g_get_m_BlockedColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_GridData", _g_get_m_GridData);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_GridTile", _g_get_m_GridTile);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_ShowGrid", _g_get_m_ShowGrid);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_Grid", _g_get_m_Grid);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "gridWidth", _s_set_gridWidth);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "gridHeight", _s_set_gridHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_GridSize", _s_set_m_GridSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_GridWidth", _s_set_m_GridWidth);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_GridHeight", _s_set_m_GridHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_CellWidth", _s_set_m_CellWidth);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_CellHeight", _s_set_m_CellHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_NormalColor", _s_set_m_NormalColor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_WalkableColor", _s_set_m_WalkableColor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_BlockedColor", _s_set_m_BlockedColor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_GridData", _s_set_m_GridData);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_GridTile", _s_set_m_GridTile);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_ShowGrid", _s_set_m_ShowGrid);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_Grid", _s_set_m_Grid);
            
			
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
					
					GameLib.GridMapManager gen_ret = new GameLib.GridMapManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameLib.GridMapManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaceObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _obj = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    UnityEngine.Vector2Int _gridPos;translator.Get(L, 3, out _gridPos);
                    GameLib.GridMapManager.GridType _objectType;translator.Get(L, 4, out _objectType);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 5, typeof(UnityEngine.Transform));
                    
                        bool gen_ret = gen_to_be_invoked.PlaceObject( _obj, _gridPos, _objectType, _parent );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2Int _gridPos;translator.Get(L, 2, out _gridPos);
                    
                    gen_to_be_invoked.RemoveObject( _gridPos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SaveTilemap(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Grid _grid = (UnityEngine.Grid)translator.GetObject(L, 2, typeof(UnityEngine.Grid));
                    string _path = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.SaveTilemap( _grid, _path );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gridWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.gridWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gridHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.gridHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_GridSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.m_GridSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_GridWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.m_GridWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_GridHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.m_GridHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_CellWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_CellWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_CellHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_CellHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_NormalColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.m_NormalColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_WalkableColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.m_WalkableColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_BlockedColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.m_BlockedColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_GridData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_GridData);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_GridTile(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_GridTile);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_ShowGrid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.m_ShowGrid);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_Grid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_Grid);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gridWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.gridWidth = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gridHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.gridHeight = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_GridSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_GridSize = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_GridWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_GridWidth = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_GridHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_GridHeight = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_CellWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_CellWidth = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_CellHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_CellHeight = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_NormalColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_NormalColor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_WalkableColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_WalkableColor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_BlockedColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_BlockedColor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_GridData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_GridData = (bool[,])translator.GetObject(L, 2, typeof(bool[,]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_GridTile(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_GridTile = (GameLib.GridMapManager.GridTile[,])translator.GetObject(L, 2, typeof(GameLib.GridMapManager.GridTile[,]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_ShowGrid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_ShowGrid = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_Grid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.GridMapManager gen_to_be_invoked = (GameLib.GridMapManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_Grid = (UnityEngine.Grid)translator.GetObject(L, 2, typeof(UnityEngine.Grid));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
