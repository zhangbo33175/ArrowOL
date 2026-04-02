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
    public class UnityEngineTilemapsTilemapRendererWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Tilemaps.TilemapRenderer);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 8, 8);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "chunkSize", _g_get_chunkSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "chunkCullingBounds", _g_get_chunkCullingBounds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "maxChunkCount", _g_get_maxChunkCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "maxFrameAge", _g_get_maxFrameAge);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "sortOrder", _g_get_sortOrder);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mode", _g_get_mode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "detectChunkCullingBounds", _g_get_detectChunkCullingBounds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "maskInteraction", _g_get_maskInteraction);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "chunkSize", _s_set_chunkSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "chunkCullingBounds", _s_set_chunkCullingBounds);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "maxChunkCount", _s_set_maxChunkCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "maxFrameAge", _s_set_maxFrameAge);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "sortOrder", _s_set_sortOrder);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mode", _s_set_mode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "detectChunkCullingBounds", _s_set_detectChunkCullingBounds);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "maskInteraction", _s_set_maskInteraction);
            
			
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
					
					UnityEngine.Tilemaps.TilemapRenderer gen_ret = new UnityEngine.Tilemaps.TilemapRenderer();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Tilemaps.TilemapRenderer constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_chunkSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.chunkSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_chunkCullingBounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.chunkCullingBounds);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maxChunkCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.maxChunkCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maxFrameAge(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.maxFrameAge);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_sortOrder(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTilemapsTilemapRendererSortOrder(L, gen_to_be_invoked.sortOrder);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTilemapsTilemapRendererMode(L, gen_to_be_invoked.mode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_detectChunkCullingBounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTilemapsTilemapRendererDetectChunkCullingBounds(L, gen_to_be_invoked.detectChunkCullingBounds);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maskInteraction(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.maskInteraction);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_chunkSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3Int gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.chunkSize = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_chunkCullingBounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.chunkCullingBounds = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_maxChunkCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.maxChunkCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_maxFrameAge(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.maxFrameAge = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_sortOrder(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.Tilemaps.TilemapRenderer.SortOrder gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.sortOrder = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.Tilemaps.TilemapRenderer.Mode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.mode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_detectChunkCullingBounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.detectChunkCullingBounds = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_maskInteraction(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Tilemaps.TilemapRenderer gen_to_be_invoked = (UnityEngine.Tilemaps.TilemapRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.SpriteMaskInteraction gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.maskInteraction = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
