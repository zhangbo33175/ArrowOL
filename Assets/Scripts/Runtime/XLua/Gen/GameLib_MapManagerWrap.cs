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
    public class GameLibMapManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLib.MapManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 3, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WorldToGrid", _m_WorldToGrid);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GridToWorld", _m_GridToWorld);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsCellOccupied", _m_IsCellOccupied);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaceObject", _m_PlaceObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveObject", _m_RemoveObject);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "size", _g_get_size);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellWidth", _g_get_cellWidth);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellHeight", _g_get_cellHeight);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "size", _s_set_size);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellWidth", _s_set_cellWidth);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellHeight", _s_set_cellHeight);
            
			
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
					
					GameLib.MapManager gen_ret = new GameLib.MapManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameLib.MapManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WorldToGrid(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameLib.MapManager gen_to_be_invoked = (GameLib.MapManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _worldPos;translator.Get(L, 2, out _worldPos);
                    
                        UnityEngine.Vector2Int gen_ret = gen_to_be_invoked.WorldToGrid( _worldPos );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GridToWorld(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameLib.MapManager gen_to_be_invoked = (GameLib.MapManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2Int _gridPos;translator.Get(L, 2, out _gridPos);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.GridToWorld( _gridPos );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsCellOccupied(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameLib.MapManager gen_to_be_invoked = (GameLib.MapManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2Int _gridPos;translator.Get(L, 2, out _gridPos);
                    
                        bool gen_ret = gen_to_be_invoked.IsCellOccupied( _gridPos );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaceObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GameLib.MapManager gen_to_be_invoked = (GameLib.MapManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _obj = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    UnityEngine.Vector2Int _gridPos;translator.Get(L, 3, out _gridPos);
                    
                        bool gen_ret = gen_to_be_invoked.PlaceObject( _obj, _gridPos );
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
            
            
                GameLib.MapManager gen_to_be_invoked = (GameLib.MapManager)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _g_get_size(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.MapManager gen_to_be_invoked = (GameLib.MapManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.size);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cellWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.MapManager gen_to_be_invoked = (GameLib.MapManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.cellWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cellHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.MapManager gen_to_be_invoked = (GameLib.MapManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.cellHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_size(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.MapManager gen_to_be_invoked = (GameLib.MapManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.size = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cellWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.MapManager gen_to_be_invoked = (GameLib.MapManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.cellWidth = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cellHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.MapManager gen_to_be_invoked = (GameLib.MapManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.cellHeight = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
