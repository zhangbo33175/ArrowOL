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
    public class GameLibMapCamHelperWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLib.MapCamHelper);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 18, 3, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "InitMapCamera", _m_InitMapCamera_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LookMap", _m_LookMap_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LookMapPositionAndSize", _m_LookMapPositionAndSize_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetupMapPositionAndSize", _m_SetupMapPositionAndSize_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LookMapPosition", _m_LookMapPosition_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LookMapSizeAnim", _m_LookMapSizeAnim_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LookMapSizeAndPosYAnim", _m_LookMapSizeAndPosYAnim_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LookMapSizeAndPosXAnim", _m_LookMapSizeAndPosXAnim_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsEqualMapCamData", _m_IsEqualMapCamData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsInCam", _m_IsInCam_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsVisableInCamera", _m_IsVisableInCamera_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsBoundsVisableInCamera", _m_IsBoundsVisableInCamera_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OpenSceneRaycasterLayer", _m_OpenSceneRaycasterLayer_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CloseSceneRaycasterLayer", _m_CloseSceneRaycasterLayer_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetSceneWidthByCamSize", _m_GetSceneWidthByCamSize_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetCameraRenderLayer", _m_SetCameraRenderLayer_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CloseCameraRenderLayer", _m_CloseCameraRenderLayer_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "MapCamera", _g_get_MapCamera);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "CurCamSize", _g_get_CurCamSize);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "CurCamPos", _g_get_CurCamPos);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "GameLib.MapCamHelper does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitMapCamera_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    MapCamData _normalCamData = (MapCamData)translator.GetObject(L, 1, typeof(MapCamData));
                    
                    GameLib.MapCamHelper.InitMapCamera( _normalCamData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LookMap_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    MapCamData _mapCamData = (MapCamData)translator.GetObject(L, 1, typeof(MapCamData));
                    float _animTime = (float)LuaAPI.lua_tonumber(L, 2);
                    System.Action _finishCallback = translator.GetDelegate<System.Action>(L, 3);
                    
                    GameLib.MapCamHelper.LookMap( _mapCamData, _animTime, _finishCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LookMapPositionAndSize_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    float _posX = (float)LuaAPI.lua_tonumber(L, 1);
                    float _posY = (float)LuaAPI.lua_tonumber(L, 2);
                    float _size = (float)LuaAPI.lua_tonumber(L, 3);
                    float _animTime = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _finishCallback = translator.GetDelegate<System.Action>(L, 5);
                    
                    GameLib.MapCamHelper.LookMapPositionAndSize( _posX, _posY, _size, _animTime, _finishCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetupMapPositionAndSize_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float _posX = (float)LuaAPI.lua_tonumber(L, 1);
                    float _posY = (float)LuaAPI.lua_tonumber(L, 2);
                    float _size = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    GameLib.MapCamHelper.SetupMapPositionAndSize( _posX, _posY, _size );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LookMapPosition_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    float _posX = (float)LuaAPI.lua_tonumber(L, 1);
                    float _posY = (float)LuaAPI.lua_tonumber(L, 2);
                    float _animTime = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _finishCallback = translator.GetDelegate<System.Action>(L, 4);
                    
                    GameLib.MapCamHelper.LookMapPosition( _posX, _posY, _animTime, _finishCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LookMapSizeAnim_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    float _size = (float)LuaAPI.lua_tonumber(L, 1);
                    float _animTime = (float)LuaAPI.lua_tonumber(L, 2);
                    System.Action _finishCallback = translator.GetDelegate<System.Action>(L, 3);
                    
                    GameLib.MapCamHelper.LookMapSizeAnim( _size, _animTime, _finishCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LookMapSizeAndPosYAnim_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    float _posY = (float)LuaAPI.lua_tonumber(L, 1);
                    float _size = (float)LuaAPI.lua_tonumber(L, 2);
                    float _animTime = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _finishCallback = translator.GetDelegate<System.Action>(L, 4);
                    
                    GameLib.MapCamHelper.LookMapSizeAndPosYAnim( _posY, _size, _animTime, _finishCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LookMapSizeAndPosXAnim_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    float _posX = (float)LuaAPI.lua_tonumber(L, 1);
                    float _size = (float)LuaAPI.lua_tonumber(L, 2);
                    float _animTime = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _finishCallback = translator.GetDelegate<System.Action>(L, 4);
                    
                    GameLib.MapCamHelper.LookMapSizeAndPosXAnim( _posX, _size, _animTime, _finishCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsEqualMapCamData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    MapCamData _newMapCamData = (MapCamData)translator.GetObject(L, 1, typeof(MapCamData));
                    
                        bool gen_ret = GameLib.MapCamHelper.IsEqualMapCamData( _newMapCamData );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsInCam_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Bounds _bounds;translator.Get(L, 1, out _bounds);
                    
                        bool gen_ret = GameLib.MapCamHelper.IsInCam( _bounds );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsVisableInCamera_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 _pos;translator.Get(L, 1, out _pos);
                    
                        bool gen_ret = GameLib.MapCamHelper.IsVisableInCamera( _pos );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsBoundsVisableInCamera_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Bounds _bounds;translator.Get(L, 1, out _bounds);
                    
                        bool gen_ret = GameLib.MapCamHelper.IsBoundsVisableInCamera( _bounds );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenSceneRaycasterLayer_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _layerNumber = LuaAPI.xlua_tointeger(L, 1);
                    
                    GameLib.MapCamHelper.OpenSceneRaycasterLayer( _layerNumber );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseSceneRaycasterLayer_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _layerNumber = LuaAPI.xlua_tointeger(L, 1);
                    
                    GameLib.MapCamHelper.CloseSceneRaycasterLayer( _layerNumber );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSceneWidthByCamSize_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float _size = (float)LuaAPI.lua_tonumber(L, 1);
                    
                        float gen_ret = GameLib.MapCamHelper.GetSceneWidthByCamSize( _size );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetCameraRenderLayer_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _layerNumber = LuaAPI.xlua_tointeger(L, 1);
                    
                    GameLib.MapCamHelper.SetCameraRenderLayer( _layerNumber );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseCameraRenderLayer_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _layerNumber = LuaAPI.xlua_tointeger(L, 1);
                    
                    GameLib.MapCamHelper.CloseCameraRenderLayer( _layerNumber );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MapCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, GameLib.MapCamHelper.MapCamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurCamSize(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushnumber(L, GameLib.MapCamHelper.CurCamSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurCamPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, GameLib.MapCamHelper.CurCamPos);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
