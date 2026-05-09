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
    public class MapCamDataWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(MapCamData);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 3, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NormalAdjustPosY", _m_NormalAdjustPosY);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NormalAdjustLeftPosX", _m_NormalAdjustLeftPosX);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NormalAdjustRightPosX", _m_NormalAdjustRightPosX);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ExitPlayAdjustPosX", _m_ExitPlayAdjustPosX);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayAdjustPosY", _m_PlayAdjustPosY);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "size", _g_get_size);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "posX", _g_get_posX);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "posY", _g_get_posY);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "size", _s_set_size);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "posX", _s_set_posX);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "posY", _s_set_posY);
            
			
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
					
					MapCamData gen_ret = new MapCamData();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to MapCamData constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NormalAdjustPosY(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapCamData gen_to_be_invoked = (MapCamData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _designHeight = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.NormalAdjustPosY( _designHeight );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NormalAdjustLeftPosX(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapCamData gen_to_be_invoked = (MapCamData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _viewSize;translator.Get(L, 2, out _viewSize);
                    float _mapLeftWorldPosX = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.NormalAdjustLeftPosX( _viewSize, _mapLeftWorldPosX );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NormalAdjustRightPosX(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapCamData gen_to_be_invoked = (MapCamData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _viewSize;translator.Get(L, 2, out _viewSize);
                    float _mapRightWorldPosX = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.NormalAdjustRightPosX( _viewSize, _mapRightWorldPosX );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ExitPlayAdjustPosX(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapCamData gen_to_be_invoked = (MapCamData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _viewSize;translator.Get(L, 2, out _viewSize);
                    float _playPosX = (float)LuaAPI.lua_tonumber(L, 3);
                    float _mapRightWorldPosX = (float)LuaAPI.lua_tonumber(L, 4);
                    RMapPlayHudPosType _mapPlayHudPosType;translator.Get(L, 5, out _mapPlayHudPosType);
                    
                    gen_to_be_invoked.ExitPlayAdjustPosX( _viewSize, _playPosX, _mapRightWorldPosX, _mapPlayHudPosType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayAdjustPosY(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                MapCamData gen_to_be_invoked = (MapCamData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    MapCamData _normalCamData = (MapCamData)translator.GetObject(L, 2, typeof(MapCamData));
                    float _distance = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.PlayAdjustPosY( _normalCamData, _distance );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_size(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapCamData gen_to_be_invoked = (MapCamData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.size);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_posX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapCamData gen_to_be_invoked = (MapCamData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.posX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_posY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapCamData gen_to_be_invoked = (MapCamData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.posY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_size(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapCamData gen_to_be_invoked = (MapCamData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.size = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_posX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapCamData gen_to_be_invoked = (MapCamData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.posX = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_posY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                MapCamData gen_to_be_invoked = (MapCamData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.posY = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
