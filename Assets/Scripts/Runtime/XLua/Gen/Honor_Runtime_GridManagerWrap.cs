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
    public class HonorRuntimeGridManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.GridManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 10, 15, 15);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CellIsOccupied", _m_CellIsOccupied);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OccupyCell", _m_OccupyCell);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FreeCell", _m_FreeCell);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetNextPosition", _m_SetNextPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLastPosition", _m_SetLastPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PositionToPosIndex", _m_PositionToPosIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PosIndexToPosition", _m_PosIndexToPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Setup", _m_Setup);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetOccupiedCells", _m_SetOccupiedCells);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BuildOverlay", _m_BuildOverlay);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "GridOrigin", _g_get_GridOrigin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GridUnitSize", _g_get_GridUnitSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DrawDebugGrid", _g_get_DrawDebugGrid);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DebugDrawMode", _g_get_DebugDrawMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DebugGridSize", _g_get_DebugGridSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CellBorderColor", _g_get_CellBorderColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InnerColor", _g_get_InnerColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OccupiedGridCells", _g_get_OccupiedGridCells);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LastPositions", _g_get_LastPositions);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "NextPositions", _g_get_NextPositions);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RegionCols", _g_get_RegionCols);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RegionRows", _g_get_RegionRows);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnlockedCount", _g_get_UnlockedCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GridCols", _g_get_GridCols);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GridRows", _g_get_GridRows);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "GridOrigin", _s_set_GridOrigin);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GridUnitSize", _s_set_GridUnitSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DrawDebugGrid", _s_set_DrawDebugGrid);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DebugDrawMode", _s_set_DebugDrawMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DebugGridSize", _s_set_DebugGridSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CellBorderColor", _s_set_CellBorderColor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "InnerColor", _s_set_InnerColor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OccupiedGridCells", _s_set_OccupiedGridCells);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LastPositions", _s_set_LastPositions);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "NextPositions", _s_set_NextPositions);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RegionCols", _s_set_RegionCols);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RegionRows", _s_set_RegionRows);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UnlockedCount", _s_set_UnlockedCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GridCols", _s_set_GridCols);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GridRows", _s_set_GridRows);
            
			
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
					
					Honor.Runtime.GridManager gen_ret = new Honor.Runtime.GridManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.GridManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CellIsOccupied(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _cellCoordinates;translator.Get(L, 2, out _cellCoordinates);
                    
                        bool gen_ret = gen_to_be_invoked.CellIsOccupied( _cellCoordinates );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OccupyCell(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _cellCoordinates;translator.Get(L, 2, out _cellCoordinates);
                    
                    gen_to_be_invoked.OccupyCell( _cellCoordinates );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FreeCell(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _cellCoordinates;translator.Get(L, 2, out _cellCoordinates);
                    
                    gen_to_be_invoked.FreeCell( _cellCoordinates );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetNextPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _trackedObject = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    UnityEngine.Vector3Int _posIndex;translator.Get(L, 3, out _posIndex);
                    
                    gen_to_be_invoked.SetNextPosition( _trackedObject, _posIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLastPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _trackedObject = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    UnityEngine.Vector3Int _posIndex;translator.Get(L, 3, out _posIndex);
                    
                    gen_to_be_invoked.SetLastPosition( _trackedObject, _posIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PositionToPosIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Vector3Int gen_ret = gen_to_be_invoked.PositionToPosIndex( _position );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PosIndexToPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _posIndex;translator.Get(L, 2, out _posIndex);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.PosIndexToPosition( _posIndex );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Setup(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _gridSize = LuaAPI.xlua_tointeger(L, 2);
                    int _gridCols = LuaAPI.xlua_tointeger(L, 3);
                    int _gridRows = LuaAPI.xlua_tointeger(L, 4);
                    int _regionCols = LuaAPI.xlua_tointeger(L, 5);
                    int _regionRows = LuaAPI.xlua_tointeger(L, 6);
                    float _scale = (float)LuaAPI.lua_tonumber(L, 7);
                    int _unlockedCount = LuaAPI.xlua_tointeger(L, 8);
                    UnityEngine.Transform _zoneRoot = (UnityEngine.Transform)translator.GetObject(L, 9, typeof(UnityEngine.Transform));
                    
                    gen_to_be_invoked.Setup( _gridSize, _gridCols, _gridRows, _regionCols, _regionRows, _scale, _unlockedCount, _zoneRoot );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetOccupiedCells(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int[] _occupied = (int[])translator.GetObject(L, 2, typeof(int[]));
                    
                    gen_to_be_invoked.SetOccupiedCells( _occupied );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BuildOverlay(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.BuildOverlay(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GridOrigin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.GridOrigin);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GridUnitSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.GridUnitSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DrawDebugGrid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.DrawDebugGrid);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DebugDrawMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                translator.PushHonorRuntimeGameDefinitionsDimensionMode(L, gen_to_be_invoked.DebugDrawMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DebugGridSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.DebugGridSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CellBorderColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.CellBorderColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InnerColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.InnerColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OccupiedGridCells(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OccupiedGridCells);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LastPositions(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LastPositions);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_NextPositions(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.NextPositions);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RegionCols(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.RegionCols);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RegionRows(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.RegionRows);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnlockedCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.UnlockedCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GridCols(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.GridCols);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GridRows(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.GridRows);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GridOrigin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GridOrigin = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GridUnitSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GridUnitSize = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DrawDebugGrid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DrawDebugGrid = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DebugDrawMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                Honor.Runtime.GameDefinitions.DimensionMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.DebugDrawMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DebugGridSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DebugGridSize = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CellBorderColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.CellBorderColor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_InnerColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.InnerColor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OccupiedGridCells(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OccupiedGridCells = (System.Collections.Generic.List<UnityEngine.Vector3>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Vector3>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LastPositions(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LastPositions = (System.Collections.Generic.Dictionary<UnityEngine.GameObject, UnityEngine.Vector3Int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<UnityEngine.GameObject, UnityEngine.Vector3Int>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_NextPositions(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.NextPositions = (System.Collections.Generic.Dictionary<UnityEngine.GameObject, UnityEngine.Vector3Int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<UnityEngine.GameObject, UnityEngine.Vector3Int>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RegionCols(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RegionCols = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RegionRows(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RegionRows = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UnlockedCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UnlockedCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GridCols(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GridCols = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GridRows(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GridManager gen_to_be_invoked = (Honor.Runtime.GridManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GridRows = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
