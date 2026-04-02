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
    public class MapPathDrawerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(MapPathDrawer);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 3, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DrawPath", _m_DrawPath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearAllPaths", _m_ClearAllPaths);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetPathColor", _m_SetPathColor);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "pathPrefab", _g_get_pathPrefab);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pathColor", _g_get_pathColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pathWidth", _g_get_pathWidth);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "pathPrefab", _s_set_pathPrefab);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pathColor", _s_set_pathColor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pathWidth", _s_set_pathWidth);
            
			
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
					
					MapPathDrawer gen_ret = new MapPathDrawer();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to MapPathDrawer constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DrawPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapPathDrawer gen_to_be_invoked = (MapPathDrawer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _pathId = LuaAPI.xlua_tointeger(L, 2);
                    MapMarker _startMarker = (MapMarker)translator.GetObject(L, 3, typeof(MapMarker));
                    MapMarker _endMarker = (MapMarker)translator.GetObject(L, 4, typeof(MapMarker));
                    
                    gen_to_be_invoked.DrawPath( _pathId, _startMarker, _endMarker );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearAllPaths(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapPathDrawer gen_to_be_invoked = (MapPathDrawer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearAllPaths(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetPathColor(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapPathDrawer gen_to_be_invoked = (MapPathDrawer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _pathId = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Color _color;translator.Get(L, 3, out _color);
                    
                    gen_to_be_invoked.SetPathColor( _pathId, _color );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pathPrefab(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapPathDrawer gen_to_be_invoked = (MapPathDrawer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.pathPrefab);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pathColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapPathDrawer gen_to_be_invoked = (MapPathDrawer)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.pathColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pathWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapPathDrawer gen_to_be_invoked = (MapPathDrawer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.pathWidth);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pathPrefab(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapPathDrawer gen_to_be_invoked = (MapPathDrawer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.pathPrefab = (UnityEngine.LineRenderer)translator.GetObject(L, 2, typeof(UnityEngine.LineRenderer));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pathColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapPathDrawer gen_to_be_invoked = (MapPathDrawer)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.pathColor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pathWidth(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapPathDrawer gen_to_be_invoked = (MapPathDrawer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.pathWidth = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
