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
    public class GameLibMapGridCheckUtilWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLib.MapGridCheckUtil);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 1, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "ValidateEntity", _m_ValidateEntity_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "MAP_GRID_VALIDATE_ENABLED", _g_get_MAP_GRID_VALIDATE_ENABLED);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "MAP_GRID_VALIDATE_ENABLED", _s_set_MAP_GRID_VALIDATE_ENABLED);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "GameLib.MapGridCheckUtil does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ValidateEntity_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _tag = LuaAPI.lua_tostring(L, 1);
                    float _editorX = (float)LuaAPI.lua_tonumber(L, 2);
                    float _editorY = (float)LuaAPI.lua_tonumber(L, 3);
                    float _saveGridX = (float)LuaAPI.lua_tonumber(L, 4);
                    float _saveGridY = (float)LuaAPI.lua_tonumber(L, 5);
                    float _worldX = (float)LuaAPI.lua_tonumber(L, 6);
                    float _worldY = (float)LuaAPI.lua_tonumber(L, 7);
                    int _gridSize = LuaAPI.xlua_tointeger(L, 8);
                    
                    GameLib.MapGridCheckUtil.ValidateEntity( _tag, _editorX, _editorY, _saveGridX, _saveGridY, _worldX, _worldY, _gridSize );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MAP_GRID_VALIDATE_ENABLED(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, GameLib.MapGridCheckUtil.MAP_GRID_VALIDATE_ENABLED);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MAP_GRID_VALIDATE_ENABLED(RealStatePtr L)
        {
		    try {
                
			    GameLib.MapGridCheckUtil.MAP_GRID_VALIDATE_ENABLED = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
