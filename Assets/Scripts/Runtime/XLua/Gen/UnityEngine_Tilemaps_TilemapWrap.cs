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
    public class UnityEngineTilemapsTilemapWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Tilemaps.Tilemap);
			Utils.BeginObjectRegister(type, L, translator, 0, 48, 10, 7);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCellCenterLocal", _m_GetCellCenterLocal);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCellCenterWorld", _m_GetCellCenterWorld);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTile", _m_GetTile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTilesBlock", _m_GetTilesBlock);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTilesBlockNonAlloc", _m_GetTilesBlockNonAlloc);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTilesRangeCount", _m_GetTilesRangeCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTilesRangeNonAlloc", _m_GetTilesRangeNonAlloc);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTile", _m_SetTile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTiles", _m_SetTiles);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTilesBlock", _m_SetTilesBlock);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasTile", _m_HasTile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RefreshTile", _m_RefreshTile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RefreshAllTiles", _m_RefreshAllTiles);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SwapTile", _m_SwapTile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ContainsTile", _m_ContainsTile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUsedTilesCount", _m_GetUsedTilesCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUsedSpritesCount", _m_GetUsedSpritesCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUsedTilesNonAlloc", _m_GetUsedTilesNonAlloc);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUsedSpritesNonAlloc", _m_GetUsedSpritesNonAlloc);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSprite", _m_GetSprite);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTransformMatrix", _m_GetTransformMatrix);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTransformMatrix", _m_SetTransformMatrix);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetColor", _m_GetColor);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetColor", _m_SetColor);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTileFlags", _m_GetTileFlags);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTileFlags", _m_SetTileFlags);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddTileFlags", _m_AddTileFlags);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveTileFlags", _m_RemoveTileFlags);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetInstantiatedObject", _m_GetInstantiatedObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetObjectToInstantiate", _m_GetObjectToInstantiate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetColliderType", _m_SetColliderType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetColliderType", _m_GetColliderType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAnimationFrameCount", _m_GetAnimationFrameCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAnimationFrame", _m_GetAnimationFrame);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAnimationFrame", _m_SetAnimationFrame);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAnimationTime", _m_GetAnimationTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAnimationTime", _m_SetAnimationTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTileAnimationFlags", _m_GetTileAnimationFlags);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTileAnimationFlags", _m_SetTileAnimationFlags);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddTileAnimationFlags", _m_AddTileAnimationFlags);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveTileAnimationFlags", _m_RemoveTileAnimationFlags);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FloodFill", _m_FloodFill);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BoxFill", _m_BoxFill);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InsertCells", _m_InsertCells);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DeleteCells", _m_DeleteCells);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearAllTiles", _m_ClearAllTiles);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResizeBounds", _m_ResizeBounds);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CompressBounds", _m_CompressBounds);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "layoutGrid", _g_get_layoutGrid);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellBounds", _g_get_cellBounds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "localBounds", _g_get_localBounds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "animationFrameRate", _g_get_animationFrameRate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "color", _g_get_color);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "origin", _g_get_origin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "size", _g_get_size);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "tileAnchor", _g_get_tileAnchor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "orientation", _g_get_orientation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "orientationMatrix", _g_get_orientationMatrix);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "animationFrameRate", _s_set_animationFrameRate);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "color", _s_set_color);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "origin", _s_set_origin);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "size", _s_set_size);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "tileAnchor", _s_set_tileAnchor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "orientation", _s_set_orientation);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "orientationMatrix", _s_set_orientationMatrix);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			
			Utils.RegisterFunc(L, Utils.CLS_IDX, "tilemapPositionsChanged", _e_tilemapPositionsChanged);
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UnityEngine.Tilemaps.Tilemap gen_ret = new UnityEngine.Tilemaps.Tilemap();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Tilemaps.Tilemap constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCellCenterLocal(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.GetCellCenterLocal( _position );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCellCenterWorld(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Vector3 gen_ret = gen_to_be_invoked.GetCellCenterWorld( _position );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Tilemaps.TileBase gen_ret = gen_to_be_invoked.GetTile( _position );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTilesBlock(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.BoundsInt _bounds;translator.Get(L, 2, out _bounds);
                    
                        UnityEngine.Tilemaps.TileBase[] gen_ret = gen_to_be_invoked.GetTilesBlock( _bounds );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTilesBlockNonAlloc(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.BoundsInt _bounds;translator.Get(L, 2, out _bounds);
                    UnityEngine.Tilemaps.TileBase[] _tiles = (UnityEngine.Tilemaps.TileBase[])translator.GetObject(L, 3, typeof(UnityEngine.Tilemaps.TileBase[]));
                    
                        int gen_ret = gen_to_be_invoked.GetTilesBlockNonAlloc( _bounds, _tiles );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTilesRangeCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _startPosition;translator.Get(L, 2, out _startPosition);
                    UnityEngine.Vector3Int _endPosition;translator.Get(L, 3, out _endPosition);
                    
                        int gen_ret = gen_to_be_invoked.GetTilesRangeCount( _startPosition, _endPosition );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTilesRangeNonAlloc(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _startPosition;translator.Get(L, 2, out _startPosition);
                    UnityEngine.Vector3Int _endPosition;translator.Get(L, 3, out _endPosition);
                    UnityEngine.Vector3Int[] _positions = (UnityEngine.Vector3Int[])translator.GetObject(L, 4, typeof(UnityEngine.Vector3Int[]));
                    UnityEngine.Tilemaps.TileBase[] _tiles = (UnityEngine.Tilemaps.TileBase[])translator.GetObject(L, 5, typeof(UnityEngine.Tilemaps.TileBase[]));
                    
                        int gen_ret = gen_to_be_invoked.GetTilesRangeNonAlloc( _startPosition, _endPosition, _positions, _tiles );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Tilemaps.TileChangeData>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Tilemaps.TileChangeData _tileChangeData;translator.Get(L, 2, out _tileChangeData);
                    bool _ignoreLockFlags = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.SetTile( _tileChangeData, _ignoreLockFlags );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3Int>(L, 2)&& translator.Assignable<UnityEngine.Tilemaps.TileBase>(L, 3)) 
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.TileBase _tile = (UnityEngine.Tilemaps.TileBase)translator.GetObject(L, 3, typeof(UnityEngine.Tilemaps.TileBase));
                    
                    gen_to_be_invoked.SetTile( _position, _tile );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Tilemaps.Tilemap.SetTile!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTiles(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Tilemaps.TileChangeData[]>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Tilemaps.TileChangeData[] _tileChangeDataArray = (UnityEngine.Tilemaps.TileChangeData[])translator.GetObject(L, 2, typeof(UnityEngine.Tilemaps.TileChangeData[]));
                    bool _ignoreLockFlags = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.SetTiles( _tileChangeDataArray, _ignoreLockFlags );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3Int[]>(L, 2)&& translator.Assignable<UnityEngine.Tilemaps.TileBase[]>(L, 3)) 
                {
                    UnityEngine.Vector3Int[] _positionArray = (UnityEngine.Vector3Int[])translator.GetObject(L, 2, typeof(UnityEngine.Vector3Int[]));
                    UnityEngine.Tilemaps.TileBase[] _tileArray = (UnityEngine.Tilemaps.TileBase[])translator.GetObject(L, 3, typeof(UnityEngine.Tilemaps.TileBase[]));
                    
                    gen_to_be_invoked.SetTiles( _positionArray, _tileArray );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Tilemaps.Tilemap.SetTiles!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTilesBlock(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.BoundsInt _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.TileBase[] _tileArray = (UnityEngine.Tilemaps.TileBase[])translator.GetObject(L, 3, typeof(UnityEngine.Tilemaps.TileBase[]));
                    
                    gen_to_be_invoked.SetTilesBlock( _position, _tileArray );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasTile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        bool gen_ret = gen_to_be_invoked.HasTile( _position );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshTile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                    gen_to_be_invoked.RefreshTile( _position );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshAllTiles(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.RefreshAllTiles(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SwapTile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Tilemaps.TileBase _changeTile = (UnityEngine.Tilemaps.TileBase)translator.GetObject(L, 2, typeof(UnityEngine.Tilemaps.TileBase));
                    UnityEngine.Tilemaps.TileBase _newTile = (UnityEngine.Tilemaps.TileBase)translator.GetObject(L, 3, typeof(UnityEngine.Tilemaps.TileBase));
                    
                    gen_to_be_invoked.SwapTile( _changeTile, _newTile );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ContainsTile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Tilemaps.TileBase _tileAsset = (UnityEngine.Tilemaps.TileBase)translator.GetObject(L, 2, typeof(UnityEngine.Tilemaps.TileBase));
                    
                        bool gen_ret = gen_to_be_invoked.ContainsTile( _tileAsset );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUsedTilesCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        int gen_ret = gen_to_be_invoked.GetUsedTilesCount(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUsedSpritesCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        int gen_ret = gen_to_be_invoked.GetUsedSpritesCount(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUsedTilesNonAlloc(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Tilemaps.TileBase[] _usedTiles = (UnityEngine.Tilemaps.TileBase[])translator.GetObject(L, 2, typeof(UnityEngine.Tilemaps.TileBase[]));
                    
                        int gen_ret = gen_to_be_invoked.GetUsedTilesNonAlloc( _usedTiles );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUsedSpritesNonAlloc(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Sprite[] _usedSprites = (UnityEngine.Sprite[])translator.GetObject(L, 2, typeof(UnityEngine.Sprite[]));
                    
                        int gen_ret = gen_to_be_invoked.GetUsedSpritesNonAlloc( _usedSprites );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSprite(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Sprite gen_ret = gen_to_be_invoked.GetSprite( _position );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTransformMatrix(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Matrix4x4 gen_ret = gen_to_be_invoked.GetTransformMatrix( _position );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTransformMatrix(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Matrix4x4 _transform;translator.Get(L, 3, out _transform);
                    
                    gen_to_be_invoked.SetTransformMatrix( _position, _transform );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetColor(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Color gen_ret = gen_to_be_invoked.GetColor( _position );
                        translator.PushUnityEngineColor(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetColor(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Color _color;translator.Get(L, 3, out _color);
                    
                    gen_to_be_invoked.SetColor( _position, _color );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTileFlags(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Tilemaps.TileFlags gen_ret = gen_to_be_invoked.GetTileFlags( _position );
                        translator.PushUnityEngineTilemapsTileFlags(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTileFlags(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.TileFlags _flags;translator.Get(L, 3, out _flags);
                    
                    gen_to_be_invoked.SetTileFlags( _position, _flags );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddTileFlags(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.TileFlags _flags;translator.Get(L, 3, out _flags);
                    
                    gen_to_be_invoked.AddTileFlags( _position, _flags );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveTileFlags(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.TileFlags _flags;translator.Get(L, 3, out _flags);
                    
                    gen_to_be_invoked.RemoveTileFlags( _position, _flags );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInstantiatedObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.GetInstantiatedObject( _position );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetObjectToInstantiate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.GetObjectToInstantiate( _position );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetColliderType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.Tile.ColliderType _colliderType;translator.Get(L, 3, out _colliderType);
                    
                    gen_to_be_invoked.SetColliderType( _position, _colliderType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetColliderType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Tilemaps.Tile.ColliderType gen_ret = gen_to_be_invoked.GetColliderType( _position );
                        translator.PushUnityEngineTilemapsTileColliderType(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAnimationFrameCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        int gen_ret = gen_to_be_invoked.GetAnimationFrameCount( _position );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAnimationFrame(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        int gen_ret = gen_to_be_invoked.GetAnimationFrame( _position );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAnimationFrame(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    int _frame = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.SetAnimationFrame( _position, _frame );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAnimationTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        float gen_ret = gen_to_be_invoked.GetAnimationTime( _position );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAnimationTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetAnimationTime( _position, _time );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTileAnimationFlags(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    
                        UnityEngine.Tilemaps.TileAnimationFlags gen_ret = gen_to_be_invoked.GetTileAnimationFlags( _position );
                        translator.PushUnityEngineTilemapsTileAnimationFlags(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTileAnimationFlags(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.TileAnimationFlags _flags;translator.Get(L, 3, out _flags);
                    
                    gen_to_be_invoked.SetTileAnimationFlags( _position, _flags );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddTileAnimationFlags(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.TileAnimationFlags _flags;translator.Get(L, 3, out _flags);
                    
                    gen_to_be_invoked.AddTileAnimationFlags( _position, _flags );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveTileAnimationFlags(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.TileAnimationFlags _flags;translator.Get(L, 3, out _flags);
                    
                    gen_to_be_invoked.RemoveTileAnimationFlags( _position, _flags );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FloodFill(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.TileBase _tile = (UnityEngine.Tilemaps.TileBase)translator.GetObject(L, 3, typeof(UnityEngine.Tilemaps.TileBase));
                    
                    gen_to_be_invoked.FloodFill( _position, _tile );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BoxFill(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.TileBase _tile = (UnityEngine.Tilemaps.TileBase)translator.GetObject(L, 3, typeof(UnityEngine.Tilemaps.TileBase));
                    int _startX = LuaAPI.xlua_tointeger(L, 4);
                    int _startY = LuaAPI.xlua_tointeger(L, 5);
                    int _endX = LuaAPI.xlua_tointeger(L, 6);
                    int _endY = LuaAPI.xlua_tointeger(L, 7);
                    
                    gen_to_be_invoked.BoxFill( _position, _tile, _startX, _startY, _endX, _endY );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InsertCells(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3Int>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    int _numColumns = LuaAPI.xlua_tointeger(L, 3);
                    int _numRows = LuaAPI.xlua_tointeger(L, 4);
                    int _numLayers = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.InsertCells( _position, _numColumns, _numRows, _numLayers );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3Int>(L, 2)&& translator.Assignable<UnityEngine.Vector3Int>(L, 3)) 
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Vector3Int _insertCells;translator.Get(L, 3, out _insertCells);
                    
                    gen_to_be_invoked.InsertCells( _position, _insertCells );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Tilemaps.Tilemap.InsertCells!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteCells(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3Int>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    int _numColumns = LuaAPI.xlua_tointeger(L, 3);
                    int _numRows = LuaAPI.xlua_tointeger(L, 4);
                    int _numLayers = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.DeleteCells( _position, _numColumns, _numRows, _numLayers );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3Int>(L, 2)&& translator.Assignable<UnityEngine.Vector3Int>(L, 3)) 
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Vector3Int _deleteCells;translator.Get(L, 3, out _deleteCells);
                    
                    gen_to_be_invoked.DeleteCells( _position, _deleteCells );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Tilemaps.Tilemap.DeleteCells!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearAllTiles(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearAllTiles(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResizeBounds(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResizeBounds(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CompressBounds(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CompressBounds(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_layoutGrid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.layoutGrid);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cellBounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.cellBounds);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_localBounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineBounds(L, gen_to_be_invoked.localBounds);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_animationFrameRate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.animationFrameRate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_color(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.color);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_origin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.origin);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_size(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.size);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_tileAnchor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.tileAnchor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_orientation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTilemapsTilemapOrientation(L, gen_to_be_invoked.orientation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_orientationMatrix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.orientationMatrix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_animationFrameRate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.animationFrameRate = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_color(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.color = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_origin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3Int gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.origin = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_size(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3Int gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.size = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_tileAnchor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.tileAnchor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_orientation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                UnityEngine.Tilemaps.Tilemap.Orientation gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.orientation = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_orientationMatrix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.Tilemap gen_to_be_invoked = (UnityEngine.Tilemaps.Tilemap)translator.FastGetCSObj(L, 1);
                UnityEngine.Matrix4x4 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.orientationMatrix = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_tilemapPositionsChanged(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
                System.Action<UnityEngine.Tilemaps.Tilemap, Unity.Collections.NativeArray<UnityEngine.Vector3Int>> gen_delegate = translator.GetDelegate<System.Action<UnityEngine.Tilemaps.Tilemap, Unity.Collections.NativeArray<UnityEngine.Vector3Int>>>(L, 2);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#2 need System.Action<UnityEngine.Tilemaps.Tilemap, Unity.Collections.NativeArray<UnityEngine.Vector3Int>>!");
                }
                
				
				if (gen_param_count == 2 && LuaAPI.xlua_is_eq_str(L, 1, "+")) {
					UnityEngine.Tilemaps.Tilemap.tilemapPositionsChanged += gen_delegate;
					return 0;
				} 
				
				
				if (gen_param_count == 2 && LuaAPI.xlua_is_eq_str(L, 1, "-")) {
					UnityEngine.Tilemaps.Tilemap.tilemapPositionsChanged -= gen_delegate;
					return 0;
				} 
				
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Tilemaps.Tilemap.tilemapPositionsChanged!");
        }
        
    }
}
