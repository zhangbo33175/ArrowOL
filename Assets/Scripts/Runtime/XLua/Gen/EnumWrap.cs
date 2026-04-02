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
    
    public class UnityEngineAnimatorUpdateModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.AnimatorUpdateMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.AnimatorUpdateMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.AnimatorUpdateMode), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Normal", UnityEngine.AnimatorUpdateMode.Normal);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AnimatePhysics", UnityEngine.AnimatorUpdateMode.AnimatePhysics);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnscaledTime", UnityEngine.AnimatorUpdateMode.UnscaledTime);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.AnimatorUpdateMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineAnimatorUpdateMode(L, (UnityEngine.AnimatorUpdateMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Normal"))
                {
                    translator.PushUnityEngineAnimatorUpdateMode(L, UnityEngine.AnimatorUpdateMode.Normal);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "AnimatePhysics"))
                {
                    translator.PushUnityEngineAnimatorUpdateMode(L, UnityEngine.AnimatorUpdateMode.AnimatePhysics);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "UnscaledTime"))
                {
                    translator.PushUnityEngineAnimatorUpdateMode(L, UnityEngine.AnimatorUpdateMode.UnscaledTime);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.AnimatorUpdateMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.AnimatorUpdateMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTilemapsTileColliderTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Tilemaps.Tile.ColliderType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Tilemaps.Tile.ColliderType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Tilemaps.Tile.ColliderType), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityEngine.Tilemaps.Tile.ColliderType.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Sprite", UnityEngine.Tilemaps.Tile.ColliderType.Sprite);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Grid", UnityEngine.Tilemaps.Tile.ColliderType.Grid);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Tilemaps.Tile.ColliderType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTilemapsTileColliderType(L, (UnityEngine.Tilemaps.Tile.ColliderType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushUnityEngineTilemapsTileColliderType(L, UnityEngine.Tilemaps.Tile.ColliderType.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Sprite"))
                {
                    translator.PushUnityEngineTilemapsTileColliderType(L, UnityEngine.Tilemaps.Tile.ColliderType.Sprite);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Grid"))
                {
                    translator.PushUnityEngineTilemapsTileColliderType(L, UnityEngine.Tilemaps.Tile.ColliderType.Grid);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Tilemaps.Tile.ColliderType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Tilemaps.Tile.ColliderType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTilemapsTilemapOrientationWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Tilemaps.Tilemap.Orientation), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Tilemaps.Tilemap.Orientation), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Tilemaps.Tilemap.Orientation), L, null, 8, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "XY", UnityEngine.Tilemaps.Tilemap.Orientation.XY);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "XZ", UnityEngine.Tilemaps.Tilemap.Orientation.XZ);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "YX", UnityEngine.Tilemaps.Tilemap.Orientation.YX);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "YZ", UnityEngine.Tilemaps.Tilemap.Orientation.YZ);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ZX", UnityEngine.Tilemaps.Tilemap.Orientation.ZX);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ZY", UnityEngine.Tilemaps.Tilemap.Orientation.ZY);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Custom", UnityEngine.Tilemaps.Tilemap.Orientation.Custom);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Tilemaps.Tilemap.Orientation), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTilemapsTilemapOrientation(L, (UnityEngine.Tilemaps.Tilemap.Orientation)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "XY"))
                {
                    translator.PushUnityEngineTilemapsTilemapOrientation(L, UnityEngine.Tilemaps.Tilemap.Orientation.XY);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "XZ"))
                {
                    translator.PushUnityEngineTilemapsTilemapOrientation(L, UnityEngine.Tilemaps.Tilemap.Orientation.XZ);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "YX"))
                {
                    translator.PushUnityEngineTilemapsTilemapOrientation(L, UnityEngine.Tilemaps.Tilemap.Orientation.YX);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "YZ"))
                {
                    translator.PushUnityEngineTilemapsTilemapOrientation(L, UnityEngine.Tilemaps.Tilemap.Orientation.YZ);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "ZX"))
                {
                    translator.PushUnityEngineTilemapsTilemapOrientation(L, UnityEngine.Tilemaps.Tilemap.Orientation.ZX);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "ZY"))
                {
                    translator.PushUnityEngineTilemapsTilemapOrientation(L, UnityEngine.Tilemaps.Tilemap.Orientation.ZY);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Custom"))
                {
                    translator.PushUnityEngineTilemapsTilemapOrientation(L, UnityEngine.Tilemaps.Tilemap.Orientation.Custom);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Tilemaps.Tilemap.Orientation!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Tilemaps.Tilemap.Orientation! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTilemapsTileFlagsWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Tilemaps.TileFlags), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Tilemaps.TileFlags), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Tilemaps.TileFlags), L, null, 7, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityEngine.Tilemaps.TileFlags.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LockColor", UnityEngine.Tilemaps.TileFlags.LockColor);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LockTransform", UnityEngine.Tilemaps.TileFlags.LockTransform);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "InstantiateGameObjectRuntimeOnly", UnityEngine.Tilemaps.TileFlags.InstantiateGameObjectRuntimeOnly);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "KeepGameObjectRuntimeOnly", UnityEngine.Tilemaps.TileFlags.KeepGameObjectRuntimeOnly);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LockAll", UnityEngine.Tilemaps.TileFlags.LockAll);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Tilemaps.TileFlags), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTilemapsTileFlags(L, (UnityEngine.Tilemaps.TileFlags)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushUnityEngineTilemapsTileFlags(L, UnityEngine.Tilemaps.TileFlags.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LockColor"))
                {
                    translator.PushUnityEngineTilemapsTileFlags(L, UnityEngine.Tilemaps.TileFlags.LockColor);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LockTransform"))
                {
                    translator.PushUnityEngineTilemapsTileFlags(L, UnityEngine.Tilemaps.TileFlags.LockTransform);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "InstantiateGameObjectRuntimeOnly"))
                {
                    translator.PushUnityEngineTilemapsTileFlags(L, UnityEngine.Tilemaps.TileFlags.InstantiateGameObjectRuntimeOnly);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "KeepGameObjectRuntimeOnly"))
                {
                    translator.PushUnityEngineTilemapsTileFlags(L, UnityEngine.Tilemaps.TileFlags.KeepGameObjectRuntimeOnly);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LockAll"))
                {
                    translator.PushUnityEngineTilemapsTileFlags(L, UnityEngine.Tilemaps.TileFlags.LockAll);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Tilemaps.TileFlags!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Tilemaps.TileFlags! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTilemapsTileAnimationFlagsWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Tilemaps.TileAnimationFlags), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Tilemaps.TileAnimationFlags), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Tilemaps.TileAnimationFlags), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityEngine.Tilemaps.TileAnimationFlags.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LoopOnce", UnityEngine.Tilemaps.TileAnimationFlags.LoopOnce);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PauseAnimation", UnityEngine.Tilemaps.TileAnimationFlags.PauseAnimation);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UpdatePhysics", UnityEngine.Tilemaps.TileAnimationFlags.UpdatePhysics);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Tilemaps.TileAnimationFlags), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTilemapsTileAnimationFlags(L, (UnityEngine.Tilemaps.TileAnimationFlags)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushUnityEngineTilemapsTileAnimationFlags(L, UnityEngine.Tilemaps.TileAnimationFlags.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LoopOnce"))
                {
                    translator.PushUnityEngineTilemapsTileAnimationFlags(L, UnityEngine.Tilemaps.TileAnimationFlags.LoopOnce);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PauseAnimation"))
                {
                    translator.PushUnityEngineTilemapsTileAnimationFlags(L, UnityEngine.Tilemaps.TileAnimationFlags.PauseAnimation);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "UpdatePhysics"))
                {
                    translator.PushUnityEngineTilemapsTileAnimationFlags(L, UnityEngine.Tilemaps.TileAnimationFlags.UpdatePhysics);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Tilemaps.TileAnimationFlags!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Tilemaps.TileAnimationFlags! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTilemapsTilemapRendererSortOrderWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Tilemaps.TilemapRenderer.SortOrder), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Tilemaps.TilemapRenderer.SortOrder), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Tilemaps.TilemapRenderer.SortOrder), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BottomLeft", UnityEngine.Tilemaps.TilemapRenderer.SortOrder.BottomLeft);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BottomRight", UnityEngine.Tilemaps.TilemapRenderer.SortOrder.BottomRight);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TopLeft", UnityEngine.Tilemaps.TilemapRenderer.SortOrder.TopLeft);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TopRight", UnityEngine.Tilemaps.TilemapRenderer.SortOrder.TopRight);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Tilemaps.TilemapRenderer.SortOrder), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTilemapsTilemapRendererSortOrder(L, (UnityEngine.Tilemaps.TilemapRenderer.SortOrder)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "BottomLeft"))
                {
                    translator.PushUnityEngineTilemapsTilemapRendererSortOrder(L, UnityEngine.Tilemaps.TilemapRenderer.SortOrder.BottomLeft);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "BottomRight"))
                {
                    translator.PushUnityEngineTilemapsTilemapRendererSortOrder(L, UnityEngine.Tilemaps.TilemapRenderer.SortOrder.BottomRight);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TopLeft"))
                {
                    translator.PushUnityEngineTilemapsTilemapRendererSortOrder(L, UnityEngine.Tilemaps.TilemapRenderer.SortOrder.TopLeft);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TopRight"))
                {
                    translator.PushUnityEngineTilemapsTilemapRendererSortOrder(L, UnityEngine.Tilemaps.TilemapRenderer.SortOrder.TopRight);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Tilemaps.TilemapRenderer.SortOrder!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Tilemaps.TilemapRenderer.SortOrder! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTilemapsTilemapRendererModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Tilemaps.TilemapRenderer.Mode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Tilemaps.TilemapRenderer.Mode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Tilemaps.TilemapRenderer.Mode), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Chunk", UnityEngine.Tilemaps.TilemapRenderer.Mode.Chunk);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Individual", UnityEngine.Tilemaps.TilemapRenderer.Mode.Individual);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Tilemaps.TilemapRenderer.Mode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTilemapsTilemapRendererMode(L, (UnityEngine.Tilemaps.TilemapRenderer.Mode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Chunk"))
                {
                    translator.PushUnityEngineTilemapsTilemapRendererMode(L, UnityEngine.Tilemaps.TilemapRenderer.Mode.Chunk);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Individual"))
                {
                    translator.PushUnityEngineTilemapsTilemapRendererMode(L, UnityEngine.Tilemaps.TilemapRenderer.Mode.Individual);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Tilemaps.TilemapRenderer.Mode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Tilemaps.TilemapRenderer.Mode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTilemapsTilemapRendererDetectChunkCullingBoundsWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Auto", UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds.Auto);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Manual", UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds.Manual);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTilemapsTilemapRendererDetectChunkCullingBounds(L, (UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Auto"))
                {
                    translator.PushUnityEngineTilemapsTilemapRendererDetectChunkCullingBounds(L, UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds.Auto);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Manual"))
                {
                    translator.PushUnityEngineTilemapsTilemapRendererDetectChunkCullingBounds(L, UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds.Manual);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeOriginTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.OriginType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.OriginType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.OriginType), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", Honor.Runtime.OriginType.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Editor", Honor.Runtime.OriginType.Editor);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Persistent", Honor.Runtime.OriginType.Persistent);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Streaming", Honor.Runtime.OriginType.Streaming);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.OriginType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeOriginType(L, (Honor.Runtime.OriginType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushHonorRuntimeOriginType(L, Honor.Runtime.OriginType.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Editor"))
                {
                    translator.PushHonorRuntimeOriginType(L, Honor.Runtime.OriginType.Editor);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Persistent"))
                {
                    translator.PushHonorRuntimeOriginType(L, Honor.Runtime.OriginType.Persistent);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Streaming"))
                {
                    translator.PushHonorRuntimeOriginType(L, Honor.Runtime.OriginType.Streaming);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.OriginType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.OriginType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimePrefabDetailTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.PrefabDetailType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.PrefabDetailType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.PrefabDetailType), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", Honor.Runtime.PrefabDetailType.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UI", Honor.Runtime.PrefabDetailType.UI);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameObject", Honor.Runtime.PrefabDetailType.GameObject);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.PrefabDetailType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimePrefabDetailType(L, (Honor.Runtime.PrefabDetailType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushHonorRuntimePrefabDetailType(L, Honor.Runtime.PrefabDetailType.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "UI"))
                {
                    translator.PushHonorRuntimePrefabDetailType(L, Honor.Runtime.PrefabDetailType.UI);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "GameObject"))
                {
                    translator.PushHonorRuntimePrefabDetailType(L, Honor.Runtime.PrefabDetailType.GameObject);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.PrefabDetailType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.PrefabDetailType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimePrefabTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.PrefabType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.PrefabType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.PrefabType), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UI", Honor.Runtime.PrefabType.UI);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Entity", Honor.Runtime.PrefabType.Entity);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.PrefabType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimePrefabType(L, (Honor.Runtime.PrefabType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "UI"))
                {
                    translator.PushHonorRuntimePrefabType(L, Honor.Runtime.PrefabType.UI);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Entity"))
                {
                    translator.PushHonorRuntimePrefabType(L, Honor.Runtime.PrefabType.Entity);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.PrefabType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.PrefabType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeGameEventCmdWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.GameEventCmd), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.GameEventCmd), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.GameEventCmd), L, null, 25, 0, 0);

            Utils.RegisterEnumType(L, typeof(Honor.Runtime.GameEventCmd));

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.GameEventCmd), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeGameEventCmd(L, (Honor.Runtime.GameEventCmd)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

                try
				{
                    translator.TranslateToEnumToTop(L, typeof(Honor.Runtime.GameEventCmd), 1);
				}
				catch (System.Exception e)
				{
					return LuaAPI.luaL_error(L, "cast to " + typeof(Honor.Runtime.GameEventCmd) + " exception:" + e);
				}

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.GameEventCmd! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeSnapStatusWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.SnapStatus), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.SnapStatus), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.SnapStatus), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NoTargetSet", Honor.Runtime.SnapStatus.NoTargetSet);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TargetHasSet", Honor.Runtime.SnapStatus.TargetHasSet);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SnapMoving", Honor.Runtime.SnapStatus.SnapMoving);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SnapMoveFinish", Honor.Runtime.SnapStatus.SnapMoveFinish);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.SnapStatus), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeSnapStatus(L, (Honor.Runtime.SnapStatus)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "NoTargetSet"))
                {
                    translator.PushHonorRuntimeSnapStatus(L, Honor.Runtime.SnapStatus.NoTargetSet);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TargetHasSet"))
                {
                    translator.PushHonorRuntimeSnapStatus(L, Honor.Runtime.SnapStatus.TargetHasSet);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "SnapMoving"))
                {
                    translator.PushHonorRuntimeSnapStatus(L, Honor.Runtime.SnapStatus.SnapMoving);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "SnapMoveFinish"))
                {
                    translator.PushHonorRuntimeSnapStatus(L, Honor.Runtime.SnapStatus.SnapMoveFinish);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.SnapStatus!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.SnapStatus! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeItemCornerEnumWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.ItemCornerEnum), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.ItemCornerEnum), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.ItemCornerEnum), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftBottom", Honor.Runtime.ItemCornerEnum.LeftBottom);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftTop", Honor.Runtime.ItemCornerEnum.LeftTop);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightTop", Honor.Runtime.ItemCornerEnum.RightTop);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightBottom", Honor.Runtime.ItemCornerEnum.RightBottom);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.ItemCornerEnum), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeItemCornerEnum(L, (Honor.Runtime.ItemCornerEnum)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "LeftBottom"))
                {
                    translator.PushHonorRuntimeItemCornerEnum(L, Honor.Runtime.ItemCornerEnum.LeftBottom);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeftTop"))
                {
                    translator.PushHonorRuntimeItemCornerEnum(L, Honor.Runtime.ItemCornerEnum.LeftTop);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightTop"))
                {
                    translator.PushHonorRuntimeItemCornerEnum(L, Honor.Runtime.ItemCornerEnum.RightTop);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightBottom"))
                {
                    translator.PushHonorRuntimeItemCornerEnum(L, Honor.Runtime.ItemCornerEnum.RightBottom);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.ItemCornerEnum!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.ItemCornerEnum! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeListItemArrangeTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.ListItemArrangeType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.ListItemArrangeType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.ListItemArrangeType), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TopToBottom", Honor.Runtime.ListItemArrangeType.TopToBottom);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BottomToTop", Honor.Runtime.ListItemArrangeType.BottomToTop);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftToRight", Honor.Runtime.ListItemArrangeType.LeftToRight);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightToLeft", Honor.Runtime.ListItemArrangeType.RightToLeft);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.ListItemArrangeType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeListItemArrangeType(L, (Honor.Runtime.ListItemArrangeType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "TopToBottom"))
                {
                    translator.PushHonorRuntimeListItemArrangeType(L, Honor.Runtime.ListItemArrangeType.TopToBottom);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "BottomToTop"))
                {
                    translator.PushHonorRuntimeListItemArrangeType(L, Honor.Runtime.ListItemArrangeType.BottomToTop);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeftToRight"))
                {
                    translator.PushHonorRuntimeListItemArrangeType(L, Honor.Runtime.ListItemArrangeType.LeftToRight);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightToLeft"))
                {
                    translator.PushHonorRuntimeListItemArrangeType(L, Honor.Runtime.ListItemArrangeType.RightToLeft);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.ListItemArrangeType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.ListItemArrangeType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeGridItemArrangeTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.GridItemArrangeType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.GridItemArrangeType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.GridItemArrangeType), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TopLeftToBottomRight", Honor.Runtime.GridItemArrangeType.TopLeftToBottomRight);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BottomLeftToTopRight", Honor.Runtime.GridItemArrangeType.BottomLeftToTopRight);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TopRightToBottomLeft", Honor.Runtime.GridItemArrangeType.TopRightToBottomLeft);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BottomRightToTopLeft", Honor.Runtime.GridItemArrangeType.BottomRightToTopLeft);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.GridItemArrangeType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeGridItemArrangeType(L, (Honor.Runtime.GridItemArrangeType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "TopLeftToBottomRight"))
                {
                    translator.PushHonorRuntimeGridItemArrangeType(L, Honor.Runtime.GridItemArrangeType.TopLeftToBottomRight);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "BottomLeftToTopRight"))
                {
                    translator.PushHonorRuntimeGridItemArrangeType(L, Honor.Runtime.GridItemArrangeType.BottomLeftToTopRight);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TopRightToBottomLeft"))
                {
                    translator.PushHonorRuntimeGridItemArrangeType(L, Honor.Runtime.GridItemArrangeType.TopRightToBottomLeft);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "BottomRightToTopLeft"))
                {
                    translator.PushHonorRuntimeGridItemArrangeType(L, Honor.Runtime.GridItemArrangeType.BottomRightToTopLeft);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.GridItemArrangeType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.GridItemArrangeType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeGridFixedTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.GridFixedType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.GridFixedType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.GridFixedType), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ColumnCountFixed", Honor.Runtime.GridFixedType.ColumnCountFixed);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RowCountFixed", Honor.Runtime.GridFixedType.RowCountFixed);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.GridFixedType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeGridFixedType(L, (Honor.Runtime.GridFixedType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "ColumnCountFixed"))
                {
                    translator.PushHonorRuntimeGridFixedType(L, Honor.Runtime.GridFixedType.ColumnCountFixed);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RowCountFixed"))
                {
                    translator.PushHonorRuntimeGridFixedType(L, Honor.Runtime.GridFixedType.RowCountFixed);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.GridFixedType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.GridFixedType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimePatternTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.PatternType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.PatternType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.PatternType), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", Honor.Runtime.PatternType.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MVVM", Honor.Runtime.PatternType.MVVM);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.PatternType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimePatternType(L, (Honor.Runtime.PatternType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushHonorRuntimePatternType(L, Honor.Runtime.PatternType.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "MVVM"))
                {
                    translator.PushHonorRuntimePatternType(L, Honor.Runtime.PatternType.MVVM);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.PatternType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.PatternType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeNonePatternTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.NonePatternType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.NonePatternType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.NonePatternType), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Default", Honor.Runtime.NonePatternType.Default);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TotalNum", Honor.Runtime.NonePatternType.TotalNum);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.NonePatternType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeNonePatternType(L, (Honor.Runtime.NonePatternType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Default"))
                {
                    translator.PushHonorRuntimeNonePatternType(L, Honor.Runtime.NonePatternType.Default);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TotalNum"))
                {
                    translator.PushHonorRuntimeNonePatternType(L, Honor.Runtime.NonePatternType.TotalNum);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.NonePatternType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.NonePatternType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeMVVMPatternTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.MVVMPatternType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.MVVMPatternType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.MVVMPatternType), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "View", Honor.Runtime.MVVMPatternType.View);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ViewModel", Honor.Runtime.MVVMPatternType.ViewModel);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TotalNum", Honor.Runtime.MVVMPatternType.TotalNum);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.MVVMPatternType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeMVVMPatternType(L, (Honor.Runtime.MVVMPatternType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "View"))
                {
                    translator.PushHonorRuntimeMVVMPatternType(L, Honor.Runtime.MVVMPatternType.View);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "ViewModel"))
                {
                    translator.PushHonorRuntimeMVVMPatternType(L, Honor.Runtime.MVVMPatternType.ViewModel);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TotalNum"))
                {
                    translator.PushHonorRuntimeMVVMPatternType(L, Honor.Runtime.MVVMPatternType.TotalNum);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.MVVMPatternType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.MVVMPatternType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimePersistWayTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.PersistWayType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.PersistWayType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.PersistWayType), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FileFragment", Honor.Runtime.PersistWayType.FileFragment);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PlayerPrefs", Honor.Runtime.PersistWayType.PlayerPrefs);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.PersistWayType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimePersistWayType(L, (Honor.Runtime.PersistWayType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "FileFragment"))
                {
                    translator.PushHonorRuntimePersistWayType(L, Honor.Runtime.PersistWayType.FileFragment);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PlayerPrefs"))
                {
                    translator.PushHonorRuntimePersistWayType(L, Honor.Runtime.PersistWayType.PlayerPrefs);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.PersistWayType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.PersistWayType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimePlaySoundErrorCodeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.PlaySoundErrorCode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.PlaySoundErrorCode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.PlaySoundErrorCode), L, null, 7, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Unknown", Honor.Runtime.PlaySoundErrorCode.Unknown);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SoundGroupNotExist", Honor.Runtime.PlaySoundErrorCode.SoundGroupNotExist);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SoundGroupHasNotEnoughAgent", Honor.Runtime.PlaySoundErrorCode.SoundGroupHasNotEnoughAgent);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LoadAssetFailure", Honor.Runtime.PlaySoundErrorCode.LoadAssetFailure);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IgnoredDueToLowPriority", Honor.Runtime.PlaySoundErrorCode.IgnoredDueToLowPriority);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SetSoundAssetFailure", Honor.Runtime.PlaySoundErrorCode.SetSoundAssetFailure);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.PlaySoundErrorCode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimePlaySoundErrorCode(L, (Honor.Runtime.PlaySoundErrorCode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Unknown"))
                {
                    translator.PushHonorRuntimePlaySoundErrorCode(L, Honor.Runtime.PlaySoundErrorCode.Unknown);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "SoundGroupNotExist"))
                {
                    translator.PushHonorRuntimePlaySoundErrorCode(L, Honor.Runtime.PlaySoundErrorCode.SoundGroupNotExist);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "SoundGroupHasNotEnoughAgent"))
                {
                    translator.PushHonorRuntimePlaySoundErrorCode(L, Honor.Runtime.PlaySoundErrorCode.SoundGroupHasNotEnoughAgent);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LoadAssetFailure"))
                {
                    translator.PushHonorRuntimePlaySoundErrorCode(L, Honor.Runtime.PlaySoundErrorCode.LoadAssetFailure);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "IgnoredDueToLowPriority"))
                {
                    translator.PushHonorRuntimePlaySoundErrorCode(L, Honor.Runtime.PlaySoundErrorCode.IgnoredDueToLowPriority);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "SetSoundAssetFailure"))
                {
                    translator.PushHonorRuntimePlaySoundErrorCode(L, Honor.Runtime.PlaySoundErrorCode.SetSoundAssetFailure);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.PlaySoundErrorCode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.PlaySoundErrorCode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeDevicePerformanceLevelWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.DevicePerformanceLevel), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.DevicePerformanceLevel), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.DevicePerformanceLevel), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Low", Honor.Runtime.DevicePerformanceLevel.Low);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Mid", Honor.Runtime.DevicePerformanceLevel.Mid);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "High", Honor.Runtime.DevicePerformanceLevel.High);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.DevicePerformanceLevel), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeDevicePerformanceLevel(L, (Honor.Runtime.DevicePerformanceLevel)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Low"))
                {
                    translator.PushHonorRuntimeDevicePerformanceLevel(L, Honor.Runtime.DevicePerformanceLevel.Low);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Mid"))
                {
                    translator.PushHonorRuntimeDevicePerformanceLevel(L, Honor.Runtime.DevicePerformanceLevel.Mid);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "High"))
                {
                    translator.PushHonorRuntimeDevicePerformanceLevel(L, Honor.Runtime.DevicePerformanceLevel.High);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.DevicePerformanceLevel!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.DevicePerformanceLevel! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeQualityLevelWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.QualityLevel), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.QualityLevel), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.QualityLevel), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Low", Honor.Runtime.QualityLevel.Low);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Mid", Honor.Runtime.QualityLevel.Mid);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "High", Honor.Runtime.QualityLevel.High);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.QualityLevel), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeQualityLevel(L, (Honor.Runtime.QualityLevel)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Low"))
                {
                    translator.PushHonorRuntimeQualityLevel(L, Honor.Runtime.QualityLevel.Low);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Mid"))
                {
                    translator.PushHonorRuntimeQualityLevel(L, Honor.Runtime.QualityLevel.Mid);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "High"))
                {
                    translator.PushHonorRuntimeQualityLevel(L, Honor.Runtime.QualityLevel.High);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.QualityLevel!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.QualityLevel! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeUITypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.UIType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.UIType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.UIType), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", Honor.Runtime.UIType.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Screen", Honor.Runtime.UIType.Screen);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Scene", Honor.Runtime.UIType.Scene);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.UIType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeUIType(L, (Honor.Runtime.UIType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushHonorRuntimeUIType(L, Honor.Runtime.UIType.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Screen"))
                {
                    translator.PushHonorRuntimeUIType(L, Honor.Runtime.UIType.Screen);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Scene"))
                {
                    translator.PushHonorRuntimeUIType(L, Honor.Runtime.UIType.Scene);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.UIType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.UIType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeVibrateTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.VibrateType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.VibrateType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.VibrateType), L, null, 11, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", Honor.Runtime.VibrateType.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Selection", Honor.Runtime.VibrateType.Selection);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Success", Honor.Runtime.VibrateType.Success);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Warning", Honor.Runtime.VibrateType.Warning);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Failure", Honor.Runtime.VibrateType.Failure);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LightImpact", Honor.Runtime.VibrateType.LightImpact);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MediumImpact", Honor.Runtime.VibrateType.MediumImpact);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "HeavyImpact", Honor.Runtime.VibrateType.HeavyImpact);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RigidImpact", Honor.Runtime.VibrateType.RigidImpact);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SoftImpact", Honor.Runtime.VibrateType.SoftImpact);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.VibrateType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeVibrateType(L, (Honor.Runtime.VibrateType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushHonorRuntimeVibrateType(L, Honor.Runtime.VibrateType.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Selection"))
                {
                    translator.PushHonorRuntimeVibrateType(L, Honor.Runtime.VibrateType.Selection);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Success"))
                {
                    translator.PushHonorRuntimeVibrateType(L, Honor.Runtime.VibrateType.Success);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Warning"))
                {
                    translator.PushHonorRuntimeVibrateType(L, Honor.Runtime.VibrateType.Warning);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Failure"))
                {
                    translator.PushHonorRuntimeVibrateType(L, Honor.Runtime.VibrateType.Failure);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LightImpact"))
                {
                    translator.PushHonorRuntimeVibrateType(L, Honor.Runtime.VibrateType.LightImpact);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "MediumImpact"))
                {
                    translator.PushHonorRuntimeVibrateType(L, Honor.Runtime.VibrateType.MediumImpact);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "HeavyImpact"))
                {
                    translator.PushHonorRuntimeVibrateType(L, Honor.Runtime.VibrateType.HeavyImpact);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RigidImpact"))
                {
                    translator.PushHonorRuntimeVibrateType(L, Honor.Runtime.VibrateType.RigidImpact);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "SoftImpact"))
                {
                    translator.PushHonorRuntimeVibrateType(L, Honor.Runtime.VibrateType.SoftImpact);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.VibrateType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.VibrateType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeGameDefinitionsDimensionModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.GameDefinitions.DimensionMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.GameDefinitions.DimensionMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.GameDefinitions.DimensionMode), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Two", Honor.Runtime.GameDefinitions.DimensionMode.Two);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Three", Honor.Runtime.GameDefinitions.DimensionMode.Three);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.GameDefinitions.DimensionMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeGameDefinitionsDimensionMode(L, (Honor.Runtime.GameDefinitions.DimensionMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Two"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDimensionMode(L, Honor.Runtime.GameDefinitions.DimensionMode.Two);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Three"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDimensionMode(L, Honor.Runtime.GameDefinitions.DimensionMode.Three);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.GameDefinitions.DimensionMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.GameDefinitions.DimensionMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeGameDefinitionsAssetTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.GameDefinitions.AssetType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.GameDefinitions.AssetType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.GameDefinitions.AssetType), L, null, 20, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GameObject", Honor.Runtime.GameDefinitions.AssetType.GameObject);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Texture2D", Honor.Runtime.GameDefinitions.AssetType.Texture2D);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Texture", Honor.Runtime.GameDefinitions.AssetType.Texture);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Sprite", Honor.Runtime.GameDefinitions.AssetType.Sprite);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ScriptableObject", Honor.Runtime.GameDefinitions.AssetType.ScriptableObject);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TileBase", Honor.Runtime.GameDefinitions.AssetType.TileBase);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Font", Honor.Runtime.GameDefinitions.AssetType.Font);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FontTMP", Honor.Runtime.GameDefinitions.AssetType.FontTMP);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Shader", Honor.Runtime.GameDefinitions.AssetType.Shader);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Material", Honor.Runtime.GameDefinitions.AssetType.Material);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TextAsset", Honor.Runtime.GameDefinitions.AssetType.TextAsset);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "JsonAsset", Honor.Runtime.GameDefinitions.AssetType.JsonAsset);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BinaryAsset", Honor.Runtime.GameDefinitions.AssetType.BinaryAsset);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LuaAsset", Honor.Runtime.GameDefinitions.AssetType.LuaAsset);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LuacAsset", Honor.Runtime.GameDefinitions.AssetType.LuacAsset);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Scene", Honor.Runtime.GameDefinitions.AssetType.Scene);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Sound", Honor.Runtime.GameDefinitions.AssetType.Sound);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Model", Honor.Runtime.GameDefinitions.AssetType.Model);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AnimatorController", Honor.Runtime.GameDefinitions.AssetType.AnimatorController);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.GameDefinitions.AssetType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeGameDefinitionsAssetType(L, (Honor.Runtime.GameDefinitions.AssetType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "GameObject"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.GameObject);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Texture2D"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.Texture2D);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Texture"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.Texture);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Sprite"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.Sprite);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "ScriptableObject"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.ScriptableObject);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TileBase"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.TileBase);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Font"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.Font);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "FontTMP"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.FontTMP);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Shader"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.Shader);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Material"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.Material);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TextAsset"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.TextAsset);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "JsonAsset"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.JsonAsset);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "BinaryAsset"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.BinaryAsset);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LuaAsset"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.LuaAsset);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LuacAsset"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.LuacAsset);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Scene"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.Scene);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Sound"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.Sound);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Model"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.Model);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "AnimatorController"))
                {
                    translator.PushHonorRuntimeGameDefinitionsAssetType(L, Honor.Runtime.GameDefinitions.AssetType.AnimatorController);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.GameDefinitions.AssetType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.GameDefinitions.AssetType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeGameDefinitionsPathTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.GameDefinitions.PathType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.GameDefinitions.PathType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.GameDefinitions.PathType), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Persist", Honor.Runtime.GameDefinitions.PathType.Persist);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Streaming", Honor.Runtime.GameDefinitions.PathType.Streaming);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Server", Honor.Runtime.GameDefinitions.PathType.Server);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ServerGray", Honor.Runtime.GameDefinitions.PathType.ServerGray);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.GameDefinitions.PathType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeGameDefinitionsPathType(L, (Honor.Runtime.GameDefinitions.PathType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Persist"))
                {
                    translator.PushHonorRuntimeGameDefinitionsPathType(L, Honor.Runtime.GameDefinitions.PathType.Persist);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Streaming"))
                {
                    translator.PushHonorRuntimeGameDefinitionsPathType(L, Honor.Runtime.GameDefinitions.PathType.Streaming);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Server"))
                {
                    translator.PushHonorRuntimeGameDefinitionsPathType(L, Honor.Runtime.GameDefinitions.PathType.Server);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "ServerGray"))
                {
                    translator.PushHonorRuntimeGameDefinitionsPathType(L, Honor.Runtime.GameDefinitions.PathType.ServerGray);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.GameDefinitions.PathType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.GameDefinitions.PathType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeGameDefinitionsDownloadStepWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.GameDefinitions.DownloadStep), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.GameDefinitions.DownloadStep), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.GameDefinitions.DownloadStep), L, null, 8, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", Honor.Runtime.GameDefinitions.DownloadStep.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Idle", Honor.Runtime.GameDefinitions.DownloadStep.Idle);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Processing", Honor.Runtime.GameDefinitions.DownloadStep.Processing);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AllOver", Honor.Runtime.GameDefinitions.DownloadStep.AllOver);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Skip", Honor.Runtime.GameDefinitions.DownloadStep.Skip);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Error", Honor.Runtime.GameDefinitions.DownloadStep.Error);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CheckError", Honor.Runtime.GameDefinitions.DownloadStep.CheckError);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.GameDefinitions.DownloadStep), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeGameDefinitionsDownloadStep(L, (Honor.Runtime.GameDefinitions.DownloadStep)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDownloadStep(L, Honor.Runtime.GameDefinitions.DownloadStep.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Idle"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDownloadStep(L, Honor.Runtime.GameDefinitions.DownloadStep.Idle);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Processing"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDownloadStep(L, Honor.Runtime.GameDefinitions.DownloadStep.Processing);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "AllOver"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDownloadStep(L, Honor.Runtime.GameDefinitions.DownloadStep.AllOver);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Skip"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDownloadStep(L, Honor.Runtime.GameDefinitions.DownloadStep.Skip);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Error"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDownloadStep(L, Honor.Runtime.GameDefinitions.DownloadStep.Error);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "CheckError"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDownloadStep(L, Honor.Runtime.GameDefinitions.DownloadStep.CheckError);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.GameDefinitions.DownloadStep!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.GameDefinitions.DownloadStep! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeGameDefinitionsDebugModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.GameDefinitions.DebugMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.GameDefinitions.DebugMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.GameDefinitions.DebugMode), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", Honor.Runtime.GameDefinitions.DebugMode.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IDEFirst", Honor.Runtime.GameDefinitions.DebugMode.IDEFirst);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnityFirst", Honor.Runtime.GameDefinitions.DebugMode.UnityFirst);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.GameDefinitions.DebugMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeGameDefinitionsDebugMode(L, (Honor.Runtime.GameDefinitions.DebugMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDebugMode(L, Honor.Runtime.GameDefinitions.DebugMode.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "IDEFirst"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDebugMode(L, Honor.Runtime.GameDefinitions.DebugMode.IDEFirst);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "UnityFirst"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDebugMode(L, Honor.Runtime.GameDefinitions.DebugMode.UnityFirst);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.GameDefinitions.DebugMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.GameDefinitions.DebugMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeGameDefinitionsLanguageWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.GameDefinitions.Language), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.GameDefinitions.Language), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.GameDefinitions.Language), L, null, 52, 0, 0);

            Utils.RegisterEnumType(L, typeof(Honor.Runtime.GameDefinitions.Language));

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.GameDefinitions.Language), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeGameDefinitionsLanguage(L, (Honor.Runtime.GameDefinitions.Language)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

                try
				{
                    translator.TranslateToEnumToTop(L, typeof(Honor.Runtime.GameDefinitions.Language), 1);
				}
				catch (System.Exception e)
				{
					return LuaAPI.luaL_error(L, "cast to " + typeof(Honor.Runtime.GameDefinitions.Language) + " exception:" + e);
				}

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.GameDefinitions.Language! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeGameDefinitionsDebugWindowModelWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.GameDefinitions.DebugWindowModel), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.GameDefinitions.DebugWindowModel), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.GameDefinitions.DebugWindowModel), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MiniWindow", Honor.Runtime.GameDefinitions.DebugWindowModel.MiniWindow);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FPSWindow", Honor.Runtime.GameDefinitions.DebugWindowModel.FPSWindow);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FullWindow", Honor.Runtime.GameDefinitions.DebugWindowModel.FullWindow);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PopWindow", Honor.Runtime.GameDefinitions.DebugWindowModel.PopWindow);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.GameDefinitions.DebugWindowModel), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeGameDefinitionsDebugWindowModel(L, (Honor.Runtime.GameDefinitions.DebugWindowModel)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "MiniWindow"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDebugWindowModel(L, Honor.Runtime.GameDefinitions.DebugWindowModel.MiniWindow);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "FPSWindow"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDebugWindowModel(L, Honor.Runtime.GameDefinitions.DebugWindowModel.FPSWindow);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "FullWindow"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDebugWindowModel(L, Honor.Runtime.GameDefinitions.DebugWindowModel.FullWindow);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PopWindow"))
                {
                    translator.PushHonorRuntimeGameDefinitionsDebugWindowModel(L, Honor.Runtime.GameDefinitions.DebugWindowModel.PopWindow);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.GameDefinitions.DebugWindowModel!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.GameDefinitions.DebugWindowModel! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeAorTextEffectOutlineHorizontalAligmentTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Left", Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType.Left);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Center", Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType.Center);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Right", Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType.Right);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeAorTextEffectOutlineHorizontalAligmentType(L, (Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Left"))
                {
                    translator.PushHonorRuntimeAorTextEffectOutlineHorizontalAligmentType(L, Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType.Left);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Center"))
                {
                    translator.PushHonorRuntimeAorTextEffectOutlineHorizontalAligmentType(L, Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType.Center);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Right"))
                {
                    translator.PushHonorRuntimeAorTextEffectOutlineHorizontalAligmentType(L, Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType.Right);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeAorTextEffectSpacingHorizontalAligmentTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Left", Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType.Left);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Center", Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType.Center);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Right", Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType.Right);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeAorTextEffectSpacingHorizontalAligmentType(L, (Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Left"))
                {
                    translator.PushHonorRuntimeAorTextEffectSpacingHorizontalAligmentType(L, Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType.Left);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Center"))
                {
                    translator.PushHonorRuntimeAorTextEffectSpacingHorizontalAligmentType(L, Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType.Center);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Right"))
                {
                    translator.PushHonorRuntimeAorTextEffectSpacingHorizontalAligmentType(L, Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType.Right);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeLuaBindValueBindValueTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.LuaBindValue.BindValueType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.LuaBindValue.BindValueType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.LuaBindValue.BindValueType), L, null, 9, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Int32", Honor.Runtime.LuaBindValue.BindValueType.Int32);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Float", Honor.Runtime.LuaBindValue.BindValueType.Float);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "String", Honor.Runtime.LuaBindValue.BindValueType.String);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Boolean", Honor.Runtime.LuaBindValue.BindValueType.Boolean);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Table", Honor.Runtime.LuaBindValue.BindValueType.Table);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Array", Honor.Runtime.LuaBindValue.BindValueType.Array);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Any", Honor.Runtime.LuaBindValue.BindValueType.Any);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Trigger", Honor.Runtime.LuaBindValue.BindValueType.Trigger);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.LuaBindValue.BindValueType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeLuaBindValueBindValueType(L, (Honor.Runtime.LuaBindValue.BindValueType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Int32"))
                {
                    translator.PushHonorRuntimeLuaBindValueBindValueType(L, Honor.Runtime.LuaBindValue.BindValueType.Int32);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Float"))
                {
                    translator.PushHonorRuntimeLuaBindValueBindValueType(L, Honor.Runtime.LuaBindValue.BindValueType.Float);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "String"))
                {
                    translator.PushHonorRuntimeLuaBindValueBindValueType(L, Honor.Runtime.LuaBindValue.BindValueType.String);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Boolean"))
                {
                    translator.PushHonorRuntimeLuaBindValueBindValueType(L, Honor.Runtime.LuaBindValue.BindValueType.Boolean);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Table"))
                {
                    translator.PushHonorRuntimeLuaBindValueBindValueType(L, Honor.Runtime.LuaBindValue.BindValueType.Table);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Array"))
                {
                    translator.PushHonorRuntimeLuaBindValueBindValueType(L, Honor.Runtime.LuaBindValue.BindValueType.Array);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Any"))
                {
                    translator.PushHonorRuntimeLuaBindValueBindValueType(L, Honor.Runtime.LuaBindValue.BindValueType.Any);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Trigger"))
                {
                    translator.PushHonorRuntimeLuaBindValueBindValueType(L, Honor.Runtime.LuaBindValue.BindValueType.Trigger);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.LuaBindValue.BindValueType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.LuaBindValue.BindValueType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeLuaInjectionInjectionTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.LuaInjection.InjectionType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.LuaInjection.InjectionType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.LuaInjection.InjectionType), L, null, 31, 0, 0);

            Utils.RegisterEnumType(L, typeof(Honor.Runtime.LuaInjection.InjectionType));

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.LuaInjection.InjectionType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeLuaInjectionInjectionType(L, (Honor.Runtime.LuaInjection.InjectionType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

                try
				{
                    translator.TranslateToEnumToTop(L, typeof(Honor.Runtime.LuaInjection.InjectionType), 1);
				}
				catch (System.Exception e)
				{
					return LuaAPI.luaL_error(L, "cast to " + typeof(Honor.Runtime.LuaInjection.InjectionType) + " exception:" + e);
				}

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.LuaInjection.InjectionType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeUIFaderInitStateWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.UIFader.InitState), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.UIFader.InitState), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.UIFader.InitState), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", Honor.Runtime.UIFader.InitState.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Active", Honor.Runtime.UIFader.InitState.Active);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Inactive", Honor.Runtime.UIFader.InitState.Inactive);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.UIFader.InitState), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeUIFaderInitState(L, (Honor.Runtime.UIFader.InitState)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushHonorRuntimeUIFaderInitState(L, Honor.Runtime.UIFader.InitState.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Active"))
                {
                    translator.PushHonorRuntimeUIFaderInitState(L, Honor.Runtime.UIFader.InitState.Active);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Inactive"))
                {
                    translator.PushHonorRuntimeUIFaderInitState(L, Honor.Runtime.UIFader.InitState.Inactive);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.UIFader.InitState!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.UIFader.InitState! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeLogLogLevelWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.Log.LogLevel), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.Log.LogLevel), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.Log.LogLevel), L, null, 6, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Debug", Honor.Runtime.Log.LogLevel.Debug);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Info", Honor.Runtime.Log.LogLevel.Info);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Warning", Honor.Runtime.Log.LogLevel.Warning);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Error", Honor.Runtime.Log.LogLevel.Error);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Fatal", Honor.Runtime.Log.LogLevel.Fatal);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.Log.LogLevel), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeLogLogLevel(L, (Honor.Runtime.Log.LogLevel)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Debug"))
                {
                    translator.PushHonorRuntimeLogLogLevel(L, Honor.Runtime.Log.LogLevel.Debug);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Info"))
                {
                    translator.PushHonorRuntimeLogLogLevel(L, Honor.Runtime.Log.LogLevel.Info);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Warning"))
                {
                    translator.PushHonorRuntimeLogLogLevel(L, Honor.Runtime.Log.LogLevel.Warning);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Error"))
                {
                    translator.PushHonorRuntimeLogLogLevel(L, Honor.Runtime.Log.LogLevel.Error);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Fatal"))
                {
                    translator.PushHonorRuntimeLogLogLevel(L, Honor.Runtime.Log.LogLevel.Fatal);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.Log.LogLevel!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.Log.LogLevel! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class HonorRuntimeUILauncherLoadingViewLoadingModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Honor.Runtime.UILauncherLoadingView.LoadingMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Honor.Runtime.UILauncherLoadingView.LoadingMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Honor.Runtime.UILauncherLoadingView.LoadingMode), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "HotfixLauncher", Honor.Runtime.UILauncherLoadingView.LoadingMode.HotfixLauncher);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "HotfixIncreaser", Honor.Runtime.UILauncherLoadingView.LoadingMode.HotfixIncreaser);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Preload", Honor.Runtime.UILauncherLoadingView.LoadingMode.Preload);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Honor.Runtime.UILauncherLoadingView.LoadingMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushHonorRuntimeUILauncherLoadingViewLoadingMode(L, (Honor.Runtime.UILauncherLoadingView.LoadingMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "HotfixLauncher"))
                {
                    translator.PushHonorRuntimeUILauncherLoadingViewLoadingMode(L, Honor.Runtime.UILauncherLoadingView.LoadingMode.HotfixLauncher);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "HotfixIncreaser"))
                {
                    translator.PushHonorRuntimeUILauncherLoadingViewLoadingMode(L, Honor.Runtime.UILauncherLoadingView.LoadingMode.HotfixIncreaser);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Preload"))
                {
                    translator.PushHonorRuntimeUILauncherLoadingViewLoadingMode(L, Honor.Runtime.UILauncherLoadingView.LoadingMode.Preload);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Honor.Runtime.UILauncherLoadingView.LoadingMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Honor.Runtime.UILauncherLoadingView.LoadingMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTimelineMatchTargetFieldsWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Timeline.MatchTargetFields), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Timeline.MatchTargetFields), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Timeline.MatchTargetFields), L, null, 7, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PositionX", UnityEngine.Timeline.MatchTargetFields.PositionX);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PositionY", UnityEngine.Timeline.MatchTargetFields.PositionY);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PositionZ", UnityEngine.Timeline.MatchTargetFields.PositionZ);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RotationX", UnityEngine.Timeline.MatchTargetFields.RotationX);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RotationY", UnityEngine.Timeline.MatchTargetFields.RotationY);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RotationZ", UnityEngine.Timeline.MatchTargetFields.RotationZ);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Timeline.MatchTargetFields), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTimelineMatchTargetFields(L, (UnityEngine.Timeline.MatchTargetFields)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "PositionX"))
                {
                    translator.PushUnityEngineTimelineMatchTargetFields(L, UnityEngine.Timeline.MatchTargetFields.PositionX);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PositionY"))
                {
                    translator.PushUnityEngineTimelineMatchTargetFields(L, UnityEngine.Timeline.MatchTargetFields.PositionY);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PositionZ"))
                {
                    translator.PushUnityEngineTimelineMatchTargetFields(L, UnityEngine.Timeline.MatchTargetFields.PositionZ);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RotationX"))
                {
                    translator.PushUnityEngineTimelineMatchTargetFields(L, UnityEngine.Timeline.MatchTargetFields.RotationX);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RotationY"))
                {
                    translator.PushUnityEngineTimelineMatchTargetFields(L, UnityEngine.Timeline.MatchTargetFields.RotationY);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RotationZ"))
                {
                    translator.PushUnityEngineTimelineMatchTargetFields(L, UnityEngine.Timeline.MatchTargetFields.RotationZ);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Timeline.MatchTargetFields!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Timeline.MatchTargetFields! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTimelineTrackOffsetWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Timeline.TrackOffset), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Timeline.TrackOffset), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Timeline.TrackOffset), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ApplyTransformOffsets", UnityEngine.Timeline.TrackOffset.ApplyTransformOffsets);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ApplySceneOffsets", UnityEngine.Timeline.TrackOffset.ApplySceneOffsets);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Auto", UnityEngine.Timeline.TrackOffset.Auto);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Timeline.TrackOffset), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTimelineTrackOffset(L, (UnityEngine.Timeline.TrackOffset)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "ApplyTransformOffsets"))
                {
                    translator.PushUnityEngineTimelineTrackOffset(L, UnityEngine.Timeline.TrackOffset.ApplyTransformOffsets);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "ApplySceneOffsets"))
                {
                    translator.PushUnityEngineTimelineTrackOffset(L, UnityEngine.Timeline.TrackOffset.ApplySceneOffsets);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Auto"))
                {
                    translator.PushUnityEngineTimelineTrackOffset(L, UnityEngine.Timeline.TrackOffset.Auto);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Timeline.TrackOffset!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Timeline.TrackOffset! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTimelineClipCapsWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Timeline.ClipCaps), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Timeline.ClipCaps), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Timeline.ClipCaps), L, null, 9, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityEngine.Timeline.ClipCaps.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Looping", UnityEngine.Timeline.ClipCaps.Looping);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Extrapolation", UnityEngine.Timeline.ClipCaps.Extrapolation);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ClipIn", UnityEngine.Timeline.ClipCaps.ClipIn);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SpeedMultiplier", UnityEngine.Timeline.ClipCaps.SpeedMultiplier);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Blending", UnityEngine.Timeline.ClipCaps.Blending);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AutoScale", UnityEngine.Timeline.ClipCaps.AutoScale);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "All", UnityEngine.Timeline.ClipCaps.All);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Timeline.ClipCaps), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTimelineClipCaps(L, (UnityEngine.Timeline.ClipCaps)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushUnityEngineTimelineClipCaps(L, UnityEngine.Timeline.ClipCaps.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Looping"))
                {
                    translator.PushUnityEngineTimelineClipCaps(L, UnityEngine.Timeline.ClipCaps.Looping);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Extrapolation"))
                {
                    translator.PushUnityEngineTimelineClipCaps(L, UnityEngine.Timeline.ClipCaps.Extrapolation);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "ClipIn"))
                {
                    translator.PushUnityEngineTimelineClipCaps(L, UnityEngine.Timeline.ClipCaps.ClipIn);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "SpeedMultiplier"))
                {
                    translator.PushUnityEngineTimelineClipCaps(L, UnityEngine.Timeline.ClipCaps.SpeedMultiplier);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Blending"))
                {
                    translator.PushUnityEngineTimelineClipCaps(L, UnityEngine.Timeline.ClipCaps.Blending);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "AutoScale"))
                {
                    translator.PushUnityEngineTimelineClipCaps(L, UnityEngine.Timeline.ClipCaps.AutoScale);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "All"))
                {
                    translator.PushUnityEngineTimelineClipCaps(L, UnityEngine.Timeline.ClipCaps.All);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Timeline.ClipCaps!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Timeline.ClipCaps! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTimelineNotificationFlagsWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Timeline.NotificationFlags), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Timeline.NotificationFlags), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Timeline.NotificationFlags), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TriggerInEditMode", UnityEngine.Timeline.NotificationFlags.TriggerInEditMode);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Retroactive", UnityEngine.Timeline.NotificationFlags.Retroactive);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TriggerOnce", UnityEngine.Timeline.NotificationFlags.TriggerOnce);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Timeline.NotificationFlags), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTimelineNotificationFlags(L, (UnityEngine.Timeline.NotificationFlags)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "TriggerInEditMode"))
                {
                    translator.PushUnityEngineTimelineNotificationFlags(L, UnityEngine.Timeline.NotificationFlags.TriggerInEditMode);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Retroactive"))
                {
                    translator.PushUnityEngineTimelineNotificationFlags(L, UnityEngine.Timeline.NotificationFlags.Retroactive);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TriggerOnce"))
                {
                    translator.PushUnityEngineTimelineNotificationFlags(L, UnityEngine.Timeline.NotificationFlags.TriggerOnce);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Timeline.NotificationFlags!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Timeline.NotificationFlags! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTimelineTrackBindingFlagsWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Timeline.TrackBindingFlags), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Timeline.TrackBindingFlags), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Timeline.TrackBindingFlags), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityEngine.Timeline.TrackBindingFlags.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AllowCreateComponent", UnityEngine.Timeline.TrackBindingFlags.AllowCreateComponent);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "All", UnityEngine.Timeline.TrackBindingFlags.All);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Timeline.TrackBindingFlags), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTimelineTrackBindingFlags(L, (UnityEngine.Timeline.TrackBindingFlags)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushUnityEngineTimelineTrackBindingFlags(L, UnityEngine.Timeline.TrackBindingFlags.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "AllowCreateComponent"))
                {
                    translator.PushUnityEngineTimelineTrackBindingFlags(L, UnityEngine.Timeline.TrackBindingFlags.AllowCreateComponent);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "All"))
                {
                    translator.PushUnityEngineTimelineTrackBindingFlags(L, UnityEngine.Timeline.TrackBindingFlags.All);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Timeline.TrackBindingFlags!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Timeline.TrackBindingFlags! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTimelineStandardFrameRatesWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Timeline.StandardFrameRates), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Timeline.StandardFrameRates), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Timeline.StandardFrameRates), L, null, 9, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Fps24", UnityEngine.Timeline.StandardFrameRates.Fps24);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Fps23_97", UnityEngine.Timeline.StandardFrameRates.Fps23_97);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Fps25", UnityEngine.Timeline.StandardFrameRates.Fps25);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Fps30", UnityEngine.Timeline.StandardFrameRates.Fps30);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Fps29_97", UnityEngine.Timeline.StandardFrameRates.Fps29_97);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Fps50", UnityEngine.Timeline.StandardFrameRates.Fps50);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Fps60", UnityEngine.Timeline.StandardFrameRates.Fps60);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Fps59_94", UnityEngine.Timeline.StandardFrameRates.Fps59_94);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Timeline.StandardFrameRates), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTimelineStandardFrameRates(L, (UnityEngine.Timeline.StandardFrameRates)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Fps24"))
                {
                    translator.PushUnityEngineTimelineStandardFrameRates(L, UnityEngine.Timeline.StandardFrameRates.Fps24);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Fps23_97"))
                {
                    translator.PushUnityEngineTimelineStandardFrameRates(L, UnityEngine.Timeline.StandardFrameRates.Fps23_97);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Fps25"))
                {
                    translator.PushUnityEngineTimelineStandardFrameRates(L, UnityEngine.Timeline.StandardFrameRates.Fps25);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Fps30"))
                {
                    translator.PushUnityEngineTimelineStandardFrameRates(L, UnityEngine.Timeline.StandardFrameRates.Fps30);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Fps29_97"))
                {
                    translator.PushUnityEngineTimelineStandardFrameRates(L, UnityEngine.Timeline.StandardFrameRates.Fps29_97);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Fps50"))
                {
                    translator.PushUnityEngineTimelineStandardFrameRates(L, UnityEngine.Timeline.StandardFrameRates.Fps50);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Fps60"))
                {
                    translator.PushUnityEngineTimelineStandardFrameRates(L, UnityEngine.Timeline.StandardFrameRates.Fps60);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Fps59_94"))
                {
                    translator.PushUnityEngineTimelineStandardFrameRates(L, UnityEngine.Timeline.StandardFrameRates.Fps59_94);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Timeline.StandardFrameRates!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Timeline.StandardFrameRates! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTimelineActivationTrackPostPlaybackStateWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Timeline.ActivationTrack.PostPlaybackState), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Timeline.ActivationTrack.PostPlaybackState), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Timeline.ActivationTrack.PostPlaybackState), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Active", UnityEngine.Timeline.ActivationTrack.PostPlaybackState.Active);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Inactive", UnityEngine.Timeline.ActivationTrack.PostPlaybackState.Inactive);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Revert", UnityEngine.Timeline.ActivationTrack.PostPlaybackState.Revert);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeaveAsIs", UnityEngine.Timeline.ActivationTrack.PostPlaybackState.LeaveAsIs);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Timeline.ActivationTrack.PostPlaybackState), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTimelineActivationTrackPostPlaybackState(L, (UnityEngine.Timeline.ActivationTrack.PostPlaybackState)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Active"))
                {
                    translator.PushUnityEngineTimelineActivationTrackPostPlaybackState(L, UnityEngine.Timeline.ActivationTrack.PostPlaybackState.Active);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Inactive"))
                {
                    translator.PushUnityEngineTimelineActivationTrackPostPlaybackState(L, UnityEngine.Timeline.ActivationTrack.PostPlaybackState.Inactive);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Revert"))
                {
                    translator.PushUnityEngineTimelineActivationTrackPostPlaybackState(L, UnityEngine.Timeline.ActivationTrack.PostPlaybackState.Revert);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeaveAsIs"))
                {
                    translator.PushUnityEngineTimelineActivationTrackPostPlaybackState(L, UnityEngine.Timeline.ActivationTrack.PostPlaybackState.LeaveAsIs);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Timeline.ActivationTrack.PostPlaybackState!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Timeline.ActivationTrack.PostPlaybackState! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTimelineAnimationPlayableAssetLoopModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Timeline.AnimationPlayableAsset.LoopMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Timeline.AnimationPlayableAsset.LoopMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Timeline.AnimationPlayableAsset.LoopMode), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UseSourceAsset", UnityEngine.Timeline.AnimationPlayableAsset.LoopMode.UseSourceAsset);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "On", UnityEngine.Timeline.AnimationPlayableAsset.LoopMode.On);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Off", UnityEngine.Timeline.AnimationPlayableAsset.LoopMode.Off);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Timeline.AnimationPlayableAsset.LoopMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTimelineAnimationPlayableAssetLoopMode(L, (UnityEngine.Timeline.AnimationPlayableAsset.LoopMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "UseSourceAsset"))
                {
                    translator.PushUnityEngineTimelineAnimationPlayableAssetLoopMode(L, UnityEngine.Timeline.AnimationPlayableAsset.LoopMode.UseSourceAsset);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "On"))
                {
                    translator.PushUnityEngineTimelineAnimationPlayableAssetLoopMode(L, UnityEngine.Timeline.AnimationPlayableAsset.LoopMode.On);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Off"))
                {
                    translator.PushUnityEngineTimelineAnimationPlayableAssetLoopMode(L, UnityEngine.Timeline.AnimationPlayableAsset.LoopMode.Off);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Timeline.AnimationPlayableAsset.LoopMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Timeline.AnimationPlayableAsset.LoopMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTimelineTimelineClipClipExtrapolationWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Timeline.TimelineClip.ClipExtrapolation), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Timeline.TimelineClip.ClipExtrapolation), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Timeline.TimelineClip.ClipExtrapolation), L, null, 6, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", UnityEngine.Timeline.TimelineClip.ClipExtrapolation.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Hold", UnityEngine.Timeline.TimelineClip.ClipExtrapolation.Hold);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Loop", UnityEngine.Timeline.TimelineClip.ClipExtrapolation.Loop);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PingPong", UnityEngine.Timeline.TimelineClip.ClipExtrapolation.PingPong);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Continue", UnityEngine.Timeline.TimelineClip.ClipExtrapolation.Continue);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Timeline.TimelineClip.ClipExtrapolation), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTimelineTimelineClipClipExtrapolation(L, (UnityEngine.Timeline.TimelineClip.ClipExtrapolation)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushUnityEngineTimelineTimelineClipClipExtrapolation(L, UnityEngine.Timeline.TimelineClip.ClipExtrapolation.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Hold"))
                {
                    translator.PushUnityEngineTimelineTimelineClipClipExtrapolation(L, UnityEngine.Timeline.TimelineClip.ClipExtrapolation.Hold);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Loop"))
                {
                    translator.PushUnityEngineTimelineTimelineClipClipExtrapolation(L, UnityEngine.Timeline.TimelineClip.ClipExtrapolation.Loop);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PingPong"))
                {
                    translator.PushUnityEngineTimelineTimelineClipClipExtrapolation(L, UnityEngine.Timeline.TimelineClip.ClipExtrapolation.PingPong);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Continue"))
                {
                    translator.PushUnityEngineTimelineTimelineClipClipExtrapolation(L, UnityEngine.Timeline.TimelineClip.ClipExtrapolation.Continue);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Timeline.TimelineClip.ClipExtrapolation!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Timeline.TimelineClip.ClipExtrapolation! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTimelineTimelineClipBlendCurveModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Timeline.TimelineClip.BlendCurveMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Timeline.TimelineClip.BlendCurveMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Timeline.TimelineClip.BlendCurveMode), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Auto", UnityEngine.Timeline.TimelineClip.BlendCurveMode.Auto);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Manual", UnityEngine.Timeline.TimelineClip.BlendCurveMode.Manual);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Timeline.TimelineClip.BlendCurveMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTimelineTimelineClipBlendCurveMode(L, (UnityEngine.Timeline.TimelineClip.BlendCurveMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Auto"))
                {
                    translator.PushUnityEngineTimelineTimelineClipBlendCurveMode(L, UnityEngine.Timeline.TimelineClip.BlendCurveMode.Auto);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Manual"))
                {
                    translator.PushUnityEngineTimelineTimelineClipBlendCurveMode(L, UnityEngine.Timeline.TimelineClip.BlendCurveMode.Manual);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Timeline.TimelineClip.BlendCurveMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Timeline.TimelineClip.BlendCurveMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTimelineTimelineAssetDurationModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Timeline.TimelineAsset.DurationMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Timeline.TimelineAsset.DurationMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Timeline.TimelineAsset.DurationMode), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BasedOnClips", UnityEngine.Timeline.TimelineAsset.DurationMode.BasedOnClips);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FixedLength", UnityEngine.Timeline.TimelineAsset.DurationMode.FixedLength);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Timeline.TimelineAsset.DurationMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTimelineTimelineAssetDurationMode(L, (UnityEngine.Timeline.TimelineAsset.DurationMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "BasedOnClips"))
                {
                    translator.PushUnityEngineTimelineTimelineAssetDurationMode(L, UnityEngine.Timeline.TimelineAsset.DurationMode.BasedOnClips);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "FixedLength"))
                {
                    translator.PushUnityEngineTimelineTimelineAssetDurationMode(L, UnityEngine.Timeline.TimelineAsset.DurationMode.FixedLength);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Timeline.TimelineAsset.DurationMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Timeline.TimelineAsset.DurationMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class UnityEngineTimelineActivationControlPlayablePostPlaybackStateWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Active", UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState.Active);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Inactive", UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState.Inactive);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Revert", UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState.Revert);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineTimelineActivationControlPlayablePostPlaybackState(L, (UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Active"))
                {
                    translator.PushUnityEngineTimelineActivationControlPlayablePostPlaybackState(L, UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState.Active);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Inactive"))
                {
                    translator.PushUnityEngineTimelineActivationControlPlayablePostPlaybackState(L, UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState.Inactive);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Revert"))
                {
                    translator.PushUnityEngineTimelineActivationControlPlayablePostPlaybackState(L, UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState.Revert);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class SpineUnityUpdateModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Spine.Unity.UpdateMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Spine.Unity.UpdateMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Spine.Unity.UpdateMode), L, null, 6, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Nothing", Spine.Unity.UpdateMode.Nothing);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "OnlyAnimationStatus", Spine.Unity.UpdateMode.OnlyAnimationStatus);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "OnlyEventTimelines", Spine.Unity.UpdateMode.OnlyEventTimelines);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EverythingExceptMesh", Spine.Unity.UpdateMode.EverythingExceptMesh);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FullUpdate", Spine.Unity.UpdateMode.FullUpdate);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Spine.Unity.UpdateMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushSpineUnityUpdateMode(L, (Spine.Unity.UpdateMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Nothing"))
                {
                    translator.PushSpineUnityUpdateMode(L, Spine.Unity.UpdateMode.Nothing);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "OnlyAnimationStatus"))
                {
                    translator.PushSpineUnityUpdateMode(L, Spine.Unity.UpdateMode.OnlyAnimationStatus);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "OnlyEventTimelines"))
                {
                    translator.PushSpineUnityUpdateMode(L, Spine.Unity.UpdateMode.OnlyEventTimelines);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "EverythingExceptMesh"))
                {
                    translator.PushSpineUnityUpdateMode(L, Spine.Unity.UpdateMode.EverythingExceptMesh);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "FullUpdate"))
                {
                    translator.PushSpineUnityUpdateMode(L, Spine.Unity.UpdateMode.FullUpdate);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Spine.Unity.UpdateMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Spine.Unity.UpdateMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class SpineUnityUpdateTimingWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Spine.Unity.UpdateTiming), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Spine.Unity.UpdateTiming), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Spine.Unity.UpdateTiming), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ManualUpdate", Spine.Unity.UpdateTiming.ManualUpdate);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "InUpdate", Spine.Unity.UpdateTiming.InUpdate);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "InFixedUpdate", Spine.Unity.UpdateTiming.InFixedUpdate);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Spine.Unity.UpdateTiming), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushSpineUnityUpdateTiming(L, (Spine.Unity.UpdateTiming)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "ManualUpdate"))
                {
                    translator.PushSpineUnityUpdateTiming(L, Spine.Unity.UpdateTiming.ManualUpdate);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "InUpdate"))
                {
                    translator.PushSpineUnityUpdateTiming(L, Spine.Unity.UpdateTiming.InUpdate);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "InFixedUpdate"))
                {
                    translator.PushSpineUnityUpdateTiming(L, Spine.Unity.UpdateTiming.InFixedUpdate);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Spine.Unity.UpdateTiming!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Spine.Unity.UpdateTiming! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class SpineUnitySettingsTriStateWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Spine.Unity.SettingsTriState), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Spine.Unity.SettingsTriState), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Spine.Unity.SettingsTriState), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Disable", Spine.Unity.SettingsTriState.Disable);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Enable", Spine.Unity.SettingsTriState.Enable);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UseGlobalSetting", Spine.Unity.SettingsTriState.UseGlobalSetting);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Spine.Unity.SettingsTriState), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushSpineUnitySettingsTriState(L, (Spine.Unity.SettingsTriState)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Disable"))
                {
                    translator.PushSpineUnitySettingsTriState(L, Spine.Unity.SettingsTriState.Disable);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Enable"))
                {
                    translator.PushSpineUnitySettingsTriState(L, Spine.Unity.SettingsTriState.Enable);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "UseGlobalSetting"))
                {
                    translator.PushSpineUnitySettingsTriState(L, Spine.Unity.SettingsTriState.UseGlobalSetting);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Spine.Unity.SettingsTriState!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Spine.Unity.SettingsTriState! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class SpineUnityBoneFollowerAxisOrientationWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Spine.Unity.BoneFollower.AxisOrientation), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Spine.Unity.BoneFollower.AxisOrientation), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Spine.Unity.BoneFollower.AxisOrientation), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "XAxis", Spine.Unity.BoneFollower.AxisOrientation.XAxis);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "YAxis", Spine.Unity.BoneFollower.AxisOrientation.YAxis);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Spine.Unity.BoneFollower.AxisOrientation), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushSpineUnityBoneFollowerAxisOrientation(L, (Spine.Unity.BoneFollower.AxisOrientation)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "XAxis"))
                {
                    translator.PushSpineUnityBoneFollowerAxisOrientation(L, Spine.Unity.BoneFollower.AxisOrientation.XAxis);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "YAxis"))
                {
                    translator.PushSpineUnityBoneFollowerAxisOrientation(L, Spine.Unity.BoneFollower.AxisOrientation.YAxis);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Spine.Unity.BoneFollower.AxisOrientation!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Spine.Unity.BoneFollower.AxisOrientation! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class SpineUnitySkeletonGraphicLayoutModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Spine.Unity.SkeletonGraphic.LayoutMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Spine.Unity.SkeletonGraphic.LayoutMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Spine.Unity.SkeletonGraphic.LayoutMode), L, null, 6, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", Spine.Unity.SkeletonGraphic.LayoutMode.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WidthControlsHeight", Spine.Unity.SkeletonGraphic.LayoutMode.WidthControlsHeight);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "HeightControlsWidth", Spine.Unity.SkeletonGraphic.LayoutMode.HeightControlsWidth);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FitInParent", Spine.Unity.SkeletonGraphic.LayoutMode.FitInParent);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EnvelopeParent", Spine.Unity.SkeletonGraphic.LayoutMode.EnvelopeParent);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Spine.Unity.SkeletonGraphic.LayoutMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushSpineUnitySkeletonGraphicLayoutMode(L, (Spine.Unity.SkeletonGraphic.LayoutMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushSpineUnitySkeletonGraphicLayoutMode(L, Spine.Unity.SkeletonGraphic.LayoutMode.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "WidthControlsHeight"))
                {
                    translator.PushSpineUnitySkeletonGraphicLayoutMode(L, Spine.Unity.SkeletonGraphic.LayoutMode.WidthControlsHeight);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "HeightControlsWidth"))
                {
                    translator.PushSpineUnitySkeletonGraphicLayoutMode(L, Spine.Unity.SkeletonGraphic.LayoutMode.HeightControlsWidth);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "FitInParent"))
                {
                    translator.PushSpineUnitySkeletonGraphicLayoutMode(L, Spine.Unity.SkeletonGraphic.LayoutMode.FitInParent);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "EnvelopeParent"))
                {
                    translator.PushSpineUnitySkeletonGraphicLayoutMode(L, Spine.Unity.SkeletonGraphic.LayoutMode.EnvelopeParent);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Spine.Unity.SkeletonGraphic.LayoutMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Spine.Unity.SkeletonGraphic.LayoutMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class SpineUnitySkeletonUtilityBoneModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Spine.Unity.SkeletonUtilityBone.Mode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Spine.Unity.SkeletonUtilityBone.Mode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Spine.Unity.SkeletonUtilityBone.Mode), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Follow", Spine.Unity.SkeletonUtilityBone.Mode.Follow);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Override", Spine.Unity.SkeletonUtilityBone.Mode.Override);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Spine.Unity.SkeletonUtilityBone.Mode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushSpineUnitySkeletonUtilityBoneMode(L, (Spine.Unity.SkeletonUtilityBone.Mode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Follow"))
                {
                    translator.PushSpineUnitySkeletonUtilityBoneMode(L, Spine.Unity.SkeletonUtilityBone.Mode.Follow);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Override"))
                {
                    translator.PushSpineUnitySkeletonUtilityBoneMode(L, Spine.Unity.SkeletonUtilityBone.Mode.Override);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Spine.Unity.SkeletonUtilityBone.Mode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Spine.Unity.SkeletonUtilityBone.Mode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class SpineUnitySkeletonUtilityBoneUpdatePhaseWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Spine.Unity.SkeletonUtilityBone.UpdatePhase), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Spine.Unity.SkeletonUtilityBone.UpdatePhase), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Spine.Unity.SkeletonUtilityBone.UpdatePhase), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Local", Spine.Unity.SkeletonUtilityBone.UpdatePhase.Local);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "World", Spine.Unity.SkeletonUtilityBone.UpdatePhase.World);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Complete", Spine.Unity.SkeletonUtilityBone.UpdatePhase.Complete);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Spine.Unity.SkeletonUtilityBone.UpdatePhase), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushSpineUnitySkeletonUtilityBoneUpdatePhase(L, (Spine.Unity.SkeletonUtilityBone.UpdatePhase)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Local"))
                {
                    translator.PushSpineUnitySkeletonUtilityBoneUpdatePhase(L, Spine.Unity.SkeletonUtilityBone.UpdatePhase.Local);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "World"))
                {
                    translator.PushSpineUnitySkeletonUtilityBoneUpdatePhase(L, Spine.Unity.SkeletonUtilityBone.UpdatePhase.World);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Complete"))
                {
                    translator.PushSpineUnitySkeletonUtilityBoneUpdatePhase(L, Spine.Unity.SkeletonUtilityBone.UpdatePhase.Complete);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Spine.Unity.SkeletonUtilityBone.UpdatePhase!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Spine.Unity.SkeletonUtilityBone.UpdatePhase! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class SpineUnitySkeletonMecanimMecanimTranslatorMixModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AlwaysMix", Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode.AlwaysMix);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MixNext", Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode.MixNext);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Hard", Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode.Hard);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushSpineUnitySkeletonMecanimMecanimTranslatorMixMode(L, (Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "AlwaysMix"))
                {
                    translator.PushSpineUnitySkeletonMecanimMecanimTranslatorMixMode(L, Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode.AlwaysMix);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "MixNext"))
                {
                    translator.PushSpineUnitySkeletonMecanimMecanimTranslatorMixMode(L, Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode.MixNext);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Hard"))
                {
                    translator.PushSpineUnitySkeletonMecanimMecanimTranslatorMixMode(L, Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode.Hard);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningAutoPlayWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.AutoPlay), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.AutoPlay), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.AutoPlay), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", DG.Tweening.AutoPlay.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AutoPlaySequences", DG.Tweening.AutoPlay.AutoPlaySequences);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AutoPlayTweeners", DG.Tweening.AutoPlay.AutoPlayTweeners);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "All", DG.Tweening.AutoPlay.All);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.AutoPlay), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningAutoPlay(L, (DG.Tweening.AutoPlay)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushDGTweeningAutoPlay(L, DG.Tweening.AutoPlay.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "AutoPlaySequences"))
                {
                    translator.PushDGTweeningAutoPlay(L, DG.Tweening.AutoPlay.AutoPlaySequences);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "AutoPlayTweeners"))
                {
                    translator.PushDGTweeningAutoPlay(L, DG.Tweening.AutoPlay.AutoPlayTweeners);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "All"))
                {
                    translator.PushDGTweeningAutoPlay(L, DG.Tweening.AutoPlay.All);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.AutoPlay!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.AutoPlay! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningAxisConstraintWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.AxisConstraint), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.AxisConstraint), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.AxisConstraint), L, null, 6, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", DG.Tweening.AxisConstraint.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "X", DG.Tweening.AxisConstraint.X);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Y", DG.Tweening.AxisConstraint.Y);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Z", DG.Tweening.AxisConstraint.Z);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "W", DG.Tweening.AxisConstraint.W);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.AxisConstraint), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningAxisConstraint(L, (DG.Tweening.AxisConstraint)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushDGTweeningAxisConstraint(L, DG.Tweening.AxisConstraint.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "X"))
                {
                    translator.PushDGTweeningAxisConstraint(L, DG.Tweening.AxisConstraint.X);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Y"))
                {
                    translator.PushDGTweeningAxisConstraint(L, DG.Tweening.AxisConstraint.Y);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Z"))
                {
                    translator.PushDGTweeningAxisConstraint(L, DG.Tweening.AxisConstraint.Z);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "W"))
                {
                    translator.PushDGTweeningAxisConstraint(L, DG.Tweening.AxisConstraint.W);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.AxisConstraint!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.AxisConstraint! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningEaseWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.Ease), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.Ease), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.Ease), L, null, 39, 0, 0);

            Utils.RegisterEnumType(L, typeof(DG.Tweening.Ease));

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.Ease), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningEase(L, (DG.Tweening.Ease)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

                try
				{
                    translator.TranslateToEnumToTop(L, typeof(DG.Tweening.Ease), 1);
				}
				catch (System.Exception e)
				{
					return LuaAPI.luaL_error(L, "cast to " + typeof(DG.Tweening.Ease) + " exception:" + e);
				}

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.Ease! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningPathModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.PathMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.PathMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.PathMode), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Ignore", DG.Tweening.PathMode.Ignore);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Full3D", DG.Tweening.PathMode.Full3D);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TopDown2D", DG.Tweening.PathMode.TopDown2D);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Sidescroller2D", DG.Tweening.PathMode.Sidescroller2D);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.PathMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningPathMode(L, (DG.Tweening.PathMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Ignore"))
                {
                    translator.PushDGTweeningPathMode(L, DG.Tweening.PathMode.Ignore);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Full3D"))
                {
                    translator.PushDGTweeningPathMode(L, DG.Tweening.PathMode.Full3D);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TopDown2D"))
                {
                    translator.PushDGTweeningPathMode(L, DG.Tweening.PathMode.TopDown2D);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Sidescroller2D"))
                {
                    translator.PushDGTweeningPathMode(L, DG.Tweening.PathMode.Sidescroller2D);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.PathMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.PathMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningPathTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.PathType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.PathType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.PathType), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Linear", DG.Tweening.PathType.Linear);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CatmullRom", DG.Tweening.PathType.CatmullRom);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.PathType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningPathType(L, (DG.Tweening.PathType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Linear"))
                {
                    translator.PushDGTweeningPathType(L, DG.Tweening.PathType.Linear);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "CatmullRom"))
                {
                    translator.PushDGTweeningPathType(L, DG.Tweening.PathType.CatmullRom);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.PathType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.PathType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningRotateModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.RotateMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.RotateMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.RotateMode), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Fast", DG.Tweening.RotateMode.Fast);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FastBeyond360", DG.Tweening.RotateMode.FastBeyond360);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "WorldAxisAdd", DG.Tweening.RotateMode.WorldAxisAdd);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LocalAxisAdd", DG.Tweening.RotateMode.LocalAxisAdd);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.RotateMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningRotateMode(L, (DG.Tweening.RotateMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Fast"))
                {
                    translator.PushDGTweeningRotateMode(L, DG.Tweening.RotateMode.Fast);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "FastBeyond360"))
                {
                    translator.PushDGTweeningRotateMode(L, DG.Tweening.RotateMode.FastBeyond360);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "WorldAxisAdd"))
                {
                    translator.PushDGTweeningRotateMode(L, DG.Tweening.RotateMode.WorldAxisAdd);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LocalAxisAdd"))
                {
                    translator.PushDGTweeningRotateMode(L, DG.Tweening.RotateMode.LocalAxisAdd);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.RotateMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.RotateMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningScrambleModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.ScrambleMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.ScrambleMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.ScrambleMode), L, null, 7, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", DG.Tweening.ScrambleMode.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "All", DG.Tweening.ScrambleMode.All);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Uppercase", DG.Tweening.ScrambleMode.Uppercase);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Lowercase", DG.Tweening.ScrambleMode.Lowercase);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Numerals", DG.Tweening.ScrambleMode.Numerals);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Custom", DG.Tweening.ScrambleMode.Custom);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.ScrambleMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningScrambleMode(L, (DG.Tweening.ScrambleMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushDGTweeningScrambleMode(L, DG.Tweening.ScrambleMode.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "All"))
                {
                    translator.PushDGTweeningScrambleMode(L, DG.Tweening.ScrambleMode.All);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Uppercase"))
                {
                    translator.PushDGTweeningScrambleMode(L, DG.Tweening.ScrambleMode.Uppercase);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Lowercase"))
                {
                    translator.PushDGTweeningScrambleMode(L, DG.Tweening.ScrambleMode.Lowercase);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Numerals"))
                {
                    translator.PushDGTweeningScrambleMode(L, DG.Tweening.ScrambleMode.Numerals);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Custom"))
                {
                    translator.PushDGTweeningScrambleMode(L, DG.Tweening.ScrambleMode.Custom);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.ScrambleMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.ScrambleMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningLoopTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.LoopType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.LoopType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.LoopType), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Restart", DG.Tweening.LoopType.Restart);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Yoyo", DG.Tweening.LoopType.Yoyo);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Incremental", DG.Tweening.LoopType.Incremental);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.LoopType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningLoopType(L, (DG.Tweening.LoopType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Restart"))
                {
                    translator.PushDGTweeningLoopType(L, DG.Tweening.LoopType.Restart);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Yoyo"))
                {
                    translator.PushDGTweeningLoopType(L, DG.Tweening.LoopType.Yoyo);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Incremental"))
                {
                    translator.PushDGTweeningLoopType(L, DG.Tweening.LoopType.Incremental);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.LoopType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.LoopType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningLogBehaviourWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.LogBehaviour), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.LogBehaviour), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.LogBehaviour), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Default", DG.Tweening.LogBehaviour.Default);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Verbose", DG.Tweening.LogBehaviour.Verbose);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ErrorsOnly", DG.Tweening.LogBehaviour.ErrorsOnly);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.LogBehaviour), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningLogBehaviour(L, (DG.Tweening.LogBehaviour)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Default"))
                {
                    translator.PushDGTweeningLogBehaviour(L, DG.Tweening.LogBehaviour.Default);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Verbose"))
                {
                    translator.PushDGTweeningLogBehaviour(L, DG.Tweening.LogBehaviour.Verbose);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "ErrorsOnly"))
                {
                    translator.PushDGTweeningLogBehaviour(L, DG.Tweening.LogBehaviour.ErrorsOnly);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.LogBehaviour!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.LogBehaviour! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningTweenTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.TweenType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.TweenType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.TweenType), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Tweener", DG.Tweening.TweenType.Tweener);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Sequence", DG.Tweening.TweenType.Sequence);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Callback", DG.Tweening.TweenType.Callback);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.TweenType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningTweenType(L, (DG.Tweening.TweenType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Tweener"))
                {
                    translator.PushDGTweeningTweenType(L, DG.Tweening.TweenType.Tweener);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Sequence"))
                {
                    translator.PushDGTweeningTweenType(L, DG.Tweening.TweenType.Sequence);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Callback"))
                {
                    translator.PushDGTweeningTweenType(L, DG.Tweening.TweenType.Callback);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.TweenType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.TweenType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningUpdateTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.UpdateType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.UpdateType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.UpdateType), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Normal", DG.Tweening.UpdateType.Normal);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Late", DG.Tweening.UpdateType.Late);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Fixed", DG.Tweening.UpdateType.Fixed);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.UpdateType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningUpdateType(L, (DG.Tweening.UpdateType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Normal"))
                {
                    translator.PushDGTweeningUpdateType(L, DG.Tweening.UpdateType.Normal);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Late"))
                {
                    translator.PushDGTweeningUpdateType(L, DG.Tweening.UpdateType.Late);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Fixed"))
                {
                    translator.PushDGTweeningUpdateType(L, DG.Tweening.UpdateType.Fixed);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.UpdateType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.UpdateType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningHandlesDrawModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.HandlesDrawMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.HandlesDrawMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.HandlesDrawMode), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Orthographic", DG.Tweening.HandlesDrawMode.Orthographic);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Perspective", DG.Tweening.HandlesDrawMode.Perspective);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.HandlesDrawMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningHandlesDrawMode(L, (DG.Tweening.HandlesDrawMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Orthographic"))
                {
                    translator.PushDGTweeningHandlesDrawMode(L, DG.Tweening.HandlesDrawMode.Orthographic);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Perspective"))
                {
                    translator.PushDGTweeningHandlesDrawMode(L, DG.Tweening.HandlesDrawMode.Perspective);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.HandlesDrawMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.HandlesDrawMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningHandlesTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.HandlesType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.HandlesType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.HandlesType), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Free", DG.Tweening.HandlesType.Free);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Full", DG.Tweening.HandlesType.Full);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.HandlesType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningHandlesType(L, (DG.Tweening.HandlesType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Free"))
                {
                    translator.PushDGTweeningHandlesType(L, DG.Tweening.HandlesType.Free);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Full"))
                {
                    translator.PushDGTweeningHandlesType(L, DG.Tweening.HandlesType.Full);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.HandlesType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.HandlesType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningDOTweenInspectorModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.DOTweenInspectorMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.DOTweenInspectorMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.DOTweenInspectorMode), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Default", DG.Tweening.DOTweenInspectorMode.Default);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "InfoAndWaypointsOnly", DG.Tweening.DOTweenInspectorMode.InfoAndWaypointsOnly);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Developer", DG.Tweening.DOTweenInspectorMode.Developer);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "OnlyPath", DG.Tweening.DOTweenInspectorMode.OnlyPath);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.DOTweenInspectorMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningDOTweenInspectorMode(L, (DG.Tweening.DOTweenInspectorMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Default"))
                {
                    translator.PushDGTweeningDOTweenInspectorMode(L, DG.Tweening.DOTweenInspectorMode.Default);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "InfoAndWaypointsOnly"))
                {
                    translator.PushDGTweeningDOTweenInspectorMode(L, DG.Tweening.DOTweenInspectorMode.InfoAndWaypointsOnly);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Developer"))
                {
                    translator.PushDGTweeningDOTweenInspectorMode(L, DG.Tweening.DOTweenInspectorMode.Developer);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "OnlyPath"))
                {
                    translator.PushDGTweeningDOTweenInspectorMode(L, DG.Tweening.DOTweenInspectorMode.OnlyPath);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.DOTweenInspectorMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.DOTweenInspectorMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningSpiralModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.SpiralMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.SpiralMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.SpiralMode), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Expand", DG.Tweening.SpiralMode.Expand);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ExpandThenContract", DG.Tweening.SpiralMode.ExpandThenContract);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.SpiralMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningSpiralMode(L, (DG.Tweening.SpiralMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Expand"))
                {
                    translator.PushDGTweeningSpiralMode(L, DG.Tweening.SpiralMode.Expand);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "ExpandThenContract"))
                {
                    translator.PushDGTweeningSpiralMode(L, DG.Tweening.SpiralMode.ExpandThenContract);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.SpiralMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.SpiralMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class GameLibEGameModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(GameLib.EGameMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(GameLib.EGameMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(GameLib.EGameMode), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ENone", GameLib.EGameMode.ENone);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EDevelopMode", GameLib.EGameMode.EDevelopMode);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EPublishMode", GameLib.EGameMode.EPublishMode);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(GameLib.EGameMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushGameLibEGameMode(L, (GameLib.EGameMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "ENone"))
                {
                    translator.PushGameLibEGameMode(L, GameLib.EGameMode.ENone);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "EDevelopMode"))
                {
                    translator.PushGameLibEGameMode(L, GameLib.EGameMode.EDevelopMode);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "EPublishMode"))
                {
                    translator.PushGameLibEGameMode(L, GameLib.EGameMode.EPublishMode);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for GameLib.EGameMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for GameLib.EGameMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class GameLibGridMapManagerGridTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(GameLib.GridMapManager.GridType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(GameLib.GridMapManager.GridType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(GameLib.GridMapManager.GridType), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Empty", GameLib.GridMapManager.GridType.Empty);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Obstacle", GameLib.GridMapManager.GridType.Obstacle);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NPC", GameLib.GridMapManager.GridType.NPC);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Props", GameLib.GridMapManager.GridType.Props);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(GameLib.GridMapManager.GridType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushGameLibGridMapManagerGridType(L, (GameLib.GridMapManager.GridType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Empty"))
                {
                    translator.PushGameLibGridMapManagerGridType(L, GameLib.GridMapManager.GridType.Empty);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Obstacle"))
                {
                    translator.PushGameLibGridMapManagerGridType(L, GameLib.GridMapManager.GridType.Obstacle);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "NPC"))
                {
                    translator.PushGameLibGridMapManagerGridType(L, GameLib.GridMapManager.GridType.NPC);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Props"))
                {
                    translator.PushGameLibGridMapManagerGridType(L, GameLib.GridMapManager.GridType.Props);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for GameLib.GridMapManager.GridType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for GameLib.GridMapManager.GridType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class GameLibGridMapManagerLayerLevelWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(GameLib.GridMapManager.LayerLevel), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(GameLib.GridMapManager.LayerLevel), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(GameLib.GridMapManager.LayerLevel), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ground", GameLib.GridMapManager.LayerLevel.ground);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "building", GameLib.GridMapManager.LayerLevel.building);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "decoration", GameLib.GridMapManager.LayerLevel.decoration);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(GameLib.GridMapManager.LayerLevel), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushGameLibGridMapManagerLayerLevel(L, (GameLib.GridMapManager.LayerLevel)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "ground"))
                {
                    translator.PushGameLibGridMapManagerLayerLevel(L, GameLib.GridMapManager.LayerLevel.ground);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "building"))
                {
                    translator.PushGameLibGridMapManagerLayerLevel(L, GameLib.GridMapManager.LayerLevel.building);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "decoration"))
                {
                    translator.PushGameLibGridMapManagerLayerLevel(L, GameLib.GridMapManager.LayerLevel.decoration);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for GameLib.GridMapManager.LayerLevel!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for GameLib.GridMapManager.LayerLevel! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
}