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
    public class GameLibGridMapManagerGridUtilsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLib.GridMapManager.GridUtils);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 4, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "WorldToGrid", _m_WorldToGrid_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GridToWorld", _m_GridToWorld_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsCellOccupied", _m_IsCellOccupied_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					GameLib.GridMapManager.GridUtils gen_ret = new GameLib.GridMapManager.GridUtils();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameLib.GridMapManager.GridUtils constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WorldToGrid_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 _worldPos;translator.Get(L, 1, out _worldPos);
                    int _cellWidth = LuaAPI.xlua_tointeger(L, 2);
                    int _cellHeight = LuaAPI.xlua_tointeger(L, 3);
                    
                        UnityEngine.Vector2Int gen_ret = GameLib.GridMapManager.GridUtils.WorldToGrid( _worldPos, _cellWidth, _cellHeight );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GridToWorld_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector2Int _gridPos;translator.Get(L, 1, out _gridPos);
                    int _cellWidth = LuaAPI.xlua_tointeger(L, 2);
                    int _cellHeight = LuaAPI.xlua_tointeger(L, 3);
                    
                        UnityEngine.Vector3 gen_ret = GameLib.GridMapManager.GridUtils.GridToWorld( _gridPos, _cellWidth, _cellHeight );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsCellOccupied_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector2Int _gridPos;translator.Get(L, 1, out _gridPos);
                    int _gridWidth = LuaAPI.xlua_tointeger(L, 2);
                    int _gridHeight = LuaAPI.xlua_tointeger(L, 3);
                    GameLib.GridMapManager.GridTile[,] _grid = (GameLib.GridMapManager.GridTile[,])translator.GetObject(L, 4, typeof(GameLib.GridMapManager.GridTile[,]));
                    
                        bool gen_ret = GameLib.GridMapManager.GridUtils.IsCellOccupied( _gridPos, _gridWidth, _gridHeight, _grid );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
