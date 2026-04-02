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
    public class UnityEngineTilemapsTileBaseWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Tilemaps.TileBase);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RefreshTile", _m_RefreshTile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTileData", _m_GetTileData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTileAnimationData", _m_GetTileAnimationData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StartUp", _m_StartUp);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "UnityEngine.Tilemaps.TileBase does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshTile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.TileBase gen_to_be_invoked = (UnityEngine.Tilemaps.TileBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.ITilemap _tilemap = (UnityEngine.Tilemaps.ITilemap)translator.GetObject(L, 3, typeof(UnityEngine.Tilemaps.ITilemap));
                    
                    gen_to_be_invoked.RefreshTile( _position, _tilemap );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTileData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.TileBase gen_to_be_invoked = (UnityEngine.Tilemaps.TileBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.ITilemap _tilemap = (UnityEngine.Tilemaps.ITilemap)translator.GetObject(L, 3, typeof(UnityEngine.Tilemaps.ITilemap));
                    UnityEngine.Tilemaps.TileData _tileData;translator.Get(L, 4, out _tileData);
                    
                    gen_to_be_invoked.GetTileData( _position, _tilemap, ref _tileData );
                    translator.Push(L, _tileData);
                        translator.Update(L, 4, _tileData);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTileAnimationData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.TileBase gen_to_be_invoked = (UnityEngine.Tilemaps.TileBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.ITilemap _tilemap = (UnityEngine.Tilemaps.ITilemap)translator.GetObject(L, 3, typeof(UnityEngine.Tilemaps.ITilemap));
                    UnityEngine.Tilemaps.TileAnimationData _tileAnimationData;translator.Get(L, 4, out _tileAnimationData);
                    
                        bool gen_ret = gen_to_be_invoked.GetTileAnimationData( _position, _tilemap, ref _tileAnimationData );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _tileAnimationData);
                        translator.Update(L, 4, _tileAnimationData);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartUp(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.TileBase gen_to_be_invoked = (UnityEngine.Tilemaps.TileBase)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _position;translator.Get(L, 2, out _position);
                    UnityEngine.Tilemaps.ITilemap _tilemap = (UnityEngine.Tilemaps.ITilemap)translator.GetObject(L, 3, typeof(UnityEngine.Tilemaps.ITilemap));
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 4, typeof(UnityEngine.GameObject));
                    
                        bool gen_ret = gen_to_be_invoked.StartUp( _position, _tilemap, _go );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
