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
    public class UnityEngineTilemapsTilemapCollider2DWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Tilemaps.TilemapCollider2D);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 4, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ProcessTilemapChanges", _m_ProcessTilemapChanges);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "useDelaunayMesh", _g_get_useDelaunayMesh);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "maximumTileChangeCount", _g_get_maximumTileChangeCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "extrusionFactor", _g_get_extrusionFactor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "hasTilemapChanges", _g_get_hasTilemapChanges);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "useDelaunayMesh", _s_set_useDelaunayMesh);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "maximumTileChangeCount", _s_set_maximumTileChangeCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "extrusionFactor", _s_set_extrusionFactor);
            
			
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
					
					UnityEngine.Tilemaps.TilemapCollider2D gen_ret = new UnityEngine.Tilemaps.TilemapCollider2D();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Tilemaps.TilemapCollider2D constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ProcessTilemapChanges(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Tilemaps.TilemapCollider2D gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapCollider2D)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ProcessTilemapChanges(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_useDelaunayMesh(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapCollider2D gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapCollider2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.useDelaunayMesh);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maximumTileChangeCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapCollider2D gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapCollider2D)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushuint(L, gen_to_be_invoked.maximumTileChangeCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_extrusionFactor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapCollider2D gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapCollider2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.extrusionFactor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_hasTilemapChanges(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapCollider2D gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapCollider2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.hasTilemapChanges);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_useDelaunayMesh(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapCollider2D gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapCollider2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.useDelaunayMesh = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_maximumTileChangeCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapCollider2D gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapCollider2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.maximumTileChangeCount = LuaAPI.xlua_touint(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_extrusionFactor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapCollider2D gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapCollider2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.extrusionFactor = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
