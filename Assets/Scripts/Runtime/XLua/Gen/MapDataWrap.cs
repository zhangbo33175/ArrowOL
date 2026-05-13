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
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 18, 18);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NormalCamSize", _m_NormalCamSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayHudRightViewWorldPosX", _m_PlayHudRightViewWorldPosX);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayHudLeftViewWorldPosX", _m_PlayHudLeftViewWorldPosX);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_ChapterId", _g_get_m_ChapterId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_LevelId", _g_get_m_LevelId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_MapCamPosType", _g_get_m_MapCamPosType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_MapPlayHudPosType", _g_get_m_MapPlayHudPosType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mapBounds", _g_get_mapBounds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "parallaxTransform", _g_get_parallaxTransform);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_MapType", _g_get_m_MapType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_IsPad", _g_get_m_IsPad);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_NormalCamData", _g_get_m_NormalCamData);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_PlayCamData", _g_get_m_PlayCamData);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_ExitPlayCamData", _g_get_m_ExitPlayCamData);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_MainHudMaxHeight", _g_get_m_MainHudMaxHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_RMapChapterTypeData", _g_get_m_RMapChapterTypeData);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_CenterOfViewBounds", _g_get_m_CenterOfViewBounds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_CenterOfViewLeftOffset", _g_get_m_CenterOfViewLeftOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_CenterOfViewRightOffset", _g_get_m_CenterOfViewRightOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_PlayHudMaxHeight", _g_get_m_PlayHudMaxHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_PlayHudMaxHeightPad", _g_get_m_PlayHudMaxHeightPad);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_ChapterId", _s_set_m_ChapterId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_LevelId", _s_set_m_LevelId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_MapCamPosType", _s_set_m_MapCamPosType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_MapPlayHudPosType", _s_set_m_MapPlayHudPosType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mapBounds", _s_set_mapBounds);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "parallaxTransform", _s_set_parallaxTransform);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_MapType", _s_set_m_MapType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_IsPad", _s_set_m_IsPad);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_NormalCamData", _s_set_m_NormalCamData);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_PlayCamData", _s_set_m_PlayCamData);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_ExitPlayCamData", _s_set_m_ExitPlayCamData);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_MainHudMaxHeight", _s_set_m_MainHudMaxHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_RMapChapterTypeData", _s_set_m_RMapChapterTypeData);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_CenterOfViewBounds", _s_set_m_CenterOfViewBounds);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_CenterOfViewLeftOffset", _s_set_m_CenterOfViewLeftOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_CenterOfViewRightOffset", _s_set_m_CenterOfViewRightOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_PlayHudMaxHeight", _s_set_m_PlayHudMaxHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_PlayHudMaxHeightPad", _s_set_m_PlayHudMaxHeightPad);
            
			
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
        static int _g_get_m_MapCamPosType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                translator.PushRMapCamPosType(L, gen_to_be_invoked.m_MapCamPosType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_MapPlayHudPosType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                translator.PushRMapPlayHudPosType(L, gen_to_be_invoked.m_MapPlayHudPosType);
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
        static int _g_get_parallaxTransform(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.parallaxTransform);
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
        static int _g_get_m_CenterOfViewBounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.m_CenterOfViewBounds);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_CenterOfViewLeftOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_CenterOfViewLeftOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_CenterOfViewRightOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_CenterOfViewRightOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_PlayHudMaxHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_PlayHudMaxHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_PlayHudMaxHeightPad(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.m_PlayHudMaxHeightPad);
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
        static int _s_set_m_MapCamPosType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                RMapCamPosType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_MapCamPosType = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_MapPlayHudPosType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                RMapPlayHudPosType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.m_MapPlayHudPosType = gen_value;
            
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
        static int _s_set_parallaxTransform(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.parallaxTransform = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
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
        static int _s_set_m_CenterOfViewBounds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_CenterOfViewBounds = (GameLib.MapInBounds)translator.GetObject(L, 2, typeof(GameLib.MapInBounds));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_CenterOfViewLeftOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_CenterOfViewLeftOffset = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_CenterOfViewRightOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_CenterOfViewRightOffset = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_PlayHudMaxHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_PlayHudMaxHeight = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_PlayHudMaxHeightPad(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapData gen_to_be_invoked = (MapData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.m_PlayHudMaxHeightPad = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
