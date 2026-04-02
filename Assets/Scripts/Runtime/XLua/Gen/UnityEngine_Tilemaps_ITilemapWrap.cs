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
    public class UnityEngineTilemapsITilemapWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Tilemaps.ITilemap);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 4, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSprite", _m_GetSprite);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetColor", _m_GetColor);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTransformMatrix", _m_GetTransformMatrix);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTileFlags", _m_GetTileFlags);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTile", _m_GetTile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RefreshTile", _m_RefreshTile);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "origin", _g_get_origin);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "size", _g_get_size);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "localBounds", _g_get_localBounds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellBounds", _g_get_cellBounds);
            
			
			
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
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<UnityEngine.Tilemaps.Tilemap>(L, 2))
				{
					UnityEngine.Tilemaps.Tilemap _tilemap = (UnityEngine.Tilemaps.Tilemap)translator.GetObject(L, 2, typeof(UnityEngine.Tilemaps.Tilemap));
					
					UnityEngine.Tilemaps.ITilemap gen_ret = new UnityEngine.Tilemaps.ITilemap(_tilemap);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Tilemaps.ITilemap constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSprite(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.ITilemap gen_to_be_invoked = (UnityEngine.Tilemaps.ITilemap)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_GetColor(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.ITilemap gen_to_be_invoked = (UnityEngine.Tilemaps.ITilemap)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_GetTransformMatrix(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.ITilemap gen_to_be_invoked = (UnityEngine.Tilemaps.ITilemap)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_GetTileFlags(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.ITilemap gen_to_be_invoked = (UnityEngine.Tilemaps.ITilemap)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_GetTile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.ITilemap gen_to_be_invoked = (UnityEngine.Tilemaps.ITilemap)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_RefreshTile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.ITilemap gen_to_be_invoked = (UnityEngine.Tilemaps.ITilemap)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _g_get_origin(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.ITilemap gen_to_be_invoked = (UnityEngine.Tilemaps.ITilemap)translator.FastGetCSObj(L, 1);
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
			
                UnityEngine.Tilemaps.ITilemap gen_to_be_invoked = (UnityEngine.Tilemaps.ITilemap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.size);
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
			
                UnityEngine.Tilemaps.ITilemap gen_to_be_invoked = (UnityEngine.Tilemaps.ITilemap)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineBounds(L, gen_to_be_invoked.localBounds);
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
			
                UnityEngine.Tilemaps.ITilemap gen_to_be_invoked = (UnityEngine.Tilemaps.ITilemap)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.cellBounds);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
