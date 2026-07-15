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
    public class MapDataWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(MapData);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 16, 16);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NormalCamSize", _m_NormalCamSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMapViewportSize", _m_GetMapViewportSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CalcAutoMinCamSize", _m_CalcAutoMinCamSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayHudRightViewWorldPosX", _m_PlayHudRightViewWorldPosX);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayHudRightViewWorldPosX_Viewport", _m_PlayHudRightViewWorldPosX_Viewport);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayHudLeftViewWorldPosX", _m_PlayHudLeftViewWorldPosX);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayHudLeftViewWorldPosX_Viewport", _m_PlayHudLeftViewWorldPosX_Viewport);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_ChapterId", _g_get_m_ChapterId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_LevelId", _g_get_m_LevelId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mapBounds", _g_get_mapBounds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_MapType", _g_get_m_MapType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_IsPad", _g_get_m_IsPad);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_NormalCamData", _g_get_m_NormalCamData);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_PlayCamData", _g_get_m_PlayCamData);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_ExitPlayCamData", _g_get_m_ExitPlayCamData);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_MainHudMaxHeight", _g_get_m_MainHudMaxHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_RMapChapterTypeData", _g_get_m_RMapChapterTypeData);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_MinCamSize", _g_get_m_MinCamSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_MaxCamSize", _g_get_m_MaxCamSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_ScrollSensitivity", _g_get_m_ScrollSensitivity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_TopHudPixelHeight", _g_get_m_TopHudPixelHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_BottomHudPixelHeight", _g_get_m_BottomHudPixelHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_MinScaleLockCenter", _g_get_m_MinScaleLockCenter);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_ChapterId", _s_set_m_ChapterId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_LevelId", _s_set_m_LevelId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mapBounds", _s_set_mapBounds);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_MapType", _s_set_m_MapType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_IsPad", _s_set_m_IsPad);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_NormalCamData", _s_set_m_NormalCamData);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_PlayCamData", _s_set_m_PlayCamData);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_ExitPlayCamData", _s_set_m_ExitPlayCamData);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_MainHudMaxHeight", _s_set_m_MainHudMaxHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_RMapChapterTypeData", _s_set_m_RMapChapterTypeData);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_MinCamSize", _s_set_m_MinCamSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_MaxCamSize", _s_set_m_MaxCamSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_ScrollSensitivity", _s_set_m_ScrollSensitivity);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_TopHudPixelHeight", _s_set_m_TopHudPixelHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_BottomHudPixelHeight", _s_set_m_BottomHudPixelHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_MinScaleLockCenter", _s_set_m_MinScaleLockCenter);
            
			
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
					
					MapData gen_ret = new MapData();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to MapData constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NormalCamSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _viewSize;translator.Get(L, 2, out _viewSize);
                    
                        float gen_ret = gen_to_be_invoked.NormalCamSize( _viewSize );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMapViewportSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        UnityEngine.Vector2 gen_ret = gen_to_be_invoked.GetMapViewportSize(  );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CalcAutoMinCamSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        float gen_ret = gen_to_be_invoked.CalcAutoMinCamSize(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayHudRightViewWorldPosX(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        float gen_ret = gen_to_be_invoked.PlayHudRightViewWorldPosX(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayHudRightViewWorldPosX_Viewport(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        float gen_ret = gen_to_be_invoked.PlayHudRightViewWorldPosX_Viewport(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayHudLeftViewWorldPosX(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        float gen_ret = gen_to_be_invoked.PlayHudLeftViewWorldPosX(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayHudLeftViewWorldPosX_Viewport(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        float gen_ret = gen_to_be_invoked.PlayHudLeftViewWorldPosX_Viewport(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_ChapterId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.m_ChapterId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_LevelId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.m_LevelId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mapBounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.mapBounds);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_MapType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                translator.PushRMapType(L, gen_to_be_invoked.m_MapType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_IsPad(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.m_IsPad);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_NormalCamData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_NormalCamData);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_PlayCamData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_PlayCamData);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_ExitPlayCamData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_ExitPlayCamData);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_MainHudMaxHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_MainHudMaxHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_RMapChapterTypeData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_RMapChapterTypeData);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_MinCamSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_MinCamSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_MaxCamSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_MaxCamSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_ScrollSensitivity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_ScrollSensitivity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_TopHudPixelHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_TopHudPixelHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_BottomHudPixelHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_BottomHudPixelHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_MinScaleLockCenter(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.m_MinScaleLockCenter);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_ChapterId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_ChapterId = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_LevelId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_LevelId = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mapBounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mapBounds = (GameLib.MapMotionLayer)translator.GetObject(L, 2, typeof(GameLib.MapMotionLayer));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_MapType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                RMapType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_MapType = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_IsPad(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_IsPad = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_NormalCamData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_NormalCamData = (MapCamData)translator.GetObject(L, 2, typeof(MapCamData));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_PlayCamData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_PlayCamData = (MapCamData)translator.GetObject(L, 2, typeof(MapCamData));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_ExitPlayCamData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_ExitPlayCamData = (MapCamData)translator.GetObject(L, 2, typeof(MapCamData));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_MainHudMaxHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_MainHudMaxHeight = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_RMapChapterTypeData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_RMapChapterTypeData = (RMapChapterTypeData)translator.GetObject(L, 2, typeof(RMapChapterTypeData));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_MinCamSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_MinCamSize = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_MaxCamSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_MaxCamSize = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_ScrollSensitivity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_ScrollSensitivity = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_TopHudPixelHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_TopHudPixelHeight = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_BottomHudPixelHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_BottomHudPixelHeight = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_MinScaleLockCenter(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_MinScaleLockCenter = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
