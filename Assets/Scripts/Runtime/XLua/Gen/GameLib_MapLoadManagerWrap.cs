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
    public class GameLibMapLoadManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLib.MapLoadManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 7, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "SetupZoneItem", _m_SetupZoneItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateZoneRoot", _m_CreateZoneRoot_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetupMapBounds", _m_SetupMapBounds_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RefreshMapBoundsAndCamera", _m_RefreshMapBoundsAndCamera_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnlockZone", _m_UnlockZone_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LockZone", _m_LockZone_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "GameLib.MapLoadManager does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetupZoneItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _zoneGo = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    UnityEngine.Transform _zoneRoot = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    int _sectionId = LuaAPI.xlua_tointeger(L, 3);
                    int _cols = LuaAPI.xlua_tointeger(L, 4);
                    int _rows = LuaAPI.xlua_tointeger(L, 5);
                    float _scale = (float)LuaAPI.lua_tonumber(L, 6);
                    UnityEngine.Sprite _bgSprite = (UnityEngine.Sprite)translator.GetObject(L, 7, typeof(UnityEngine.Sprite));
                    bool _unlocked = LuaAPI.lua_toboolean(L, 8);
                    
                    GameLib.MapLoadManager.SetupZoneItem( _zoneGo, _zoneRoot, _sectionId, _cols, _rows, _scale, _bgSprite, _unlocked );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateZoneRoot_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    
                        UnityEngine.Transform gen_ret = GameLib.MapLoadManager.CreateZoneRoot( _parent );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetupMapBounds_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _gameMapRoot = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    float _rectW = (float)LuaAPI.lua_tonumber(L, 2);
                    float _rectH = (float)LuaAPI.lua_tonumber(L, 3);
                    float _centerX = (float)LuaAPI.lua_tonumber(L, 4);
                    float _centerY = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    GameLib.MapLoadManager.SetupMapBounds( _gameMapRoot, _rectW, _rectH, _centerX, _centerY );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshMapBoundsAndCamera_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<UnityEngine.Camera>(L, 3)) 
                {
                    UnityEngine.GameObject _gameMainMap = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    float _zoneSize = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 3, typeof(UnityEngine.Camera));
                    
                    GameLib.MapLoadManager.RefreshMapBoundsAndCamera( _gameMainMap, _zoneSize, _camera );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.GameObject _gameMainMap = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    float _zoneSize = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    GameLib.MapLoadManager.RefreshMapBoundsAndCamera( _gameMainMap, _zoneSize );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1&& translator.Assignable<UnityEngine.GameObject>(L, 1)) 
                {
                    UnityEngine.GameObject _gameMainMap = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    
                    GameLib.MapLoadManager.RefreshMapBoundsAndCamera( _gameMainMap );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameLib.MapLoadManager.RefreshMapBoundsAndCamera!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnlockZone_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Camera>(L, 4)) 
                {
                    UnityEngine.GameObject _gameMainMap = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    int _sectionId = LuaAPI.xlua_tointeger(L, 2);
                    float _zoneSize = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 4, typeof(UnityEngine.Camera));
                    
                    GameLib.MapLoadManager.UnlockZone( _gameMainMap, _sectionId, _zoneSize, _camera );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.GameObject _gameMainMap = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    int _sectionId = LuaAPI.xlua_tointeger(L, 2);
                    float _zoneSize = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    GameLib.MapLoadManager.UnlockZone( _gameMainMap, _sectionId, _zoneSize );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.GameObject _gameMainMap = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    int _sectionId = LuaAPI.xlua_tointeger(L, 2);
                    
                    GameLib.MapLoadManager.UnlockZone( _gameMainMap, _sectionId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameLib.MapLoadManager.UnlockZone!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LockZone_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<UnityEngine.Camera>(L, 4)) 
                {
                    UnityEngine.GameObject _gameMainMap = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    int _sectionId = LuaAPI.xlua_tointeger(L, 2);
                    float _zoneSize = (float)LuaAPI.lua_tonumber(L, 3);
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 4, typeof(UnityEngine.Camera));
                    
                    GameLib.MapLoadManager.LockZone( _gameMainMap, _sectionId, _zoneSize, _camera );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.GameObject _gameMainMap = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    int _sectionId = LuaAPI.xlua_tointeger(L, 2);
                    float _zoneSize = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    GameLib.MapLoadManager.LockZone( _gameMainMap, _sectionId, _zoneSize );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.GameObject _gameMainMap = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    int _sectionId = LuaAPI.xlua_tointeger(L, 2);
                    
                    GameLib.MapLoadManager.LockZone( _gameMainMap, _sectionId );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GameLib.MapLoadManager.LockZone!");
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
