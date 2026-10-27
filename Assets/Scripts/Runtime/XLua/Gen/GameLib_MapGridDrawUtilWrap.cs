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
    public class GameLibMapGridDrawUtilWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLib.MapGridDrawUtil);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "DefaultStyle", _m_DefaultStyle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ComputeGridLinePositions", _m_ComputeGridLinePositions_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "GameLib.MapGridDrawUtil does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DefaultStyle_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        GameLib.MapGridDrawUtil.GridStyle gen_ret = GameLib.MapGridDrawUtil.DefaultStyle(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ComputeGridLinePositions_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    float _mapW = (float)LuaAPI.lua_tonumber(L, 1);
                    float _mapH = (float)LuaAPI.lua_tonumber(L, 2);
                    int _gridCols = LuaAPI.xlua_tointeger(L, 3);
                    int _regionCols = LuaAPI.xlua_tointeger(L, 4);
                    int _regionRows = LuaAPI.xlua_tointeger(L, 5);
                    int _unlockedSection = LuaAPI.xlua_tointeger(L, 6);
                    float[] _verticalLines;
                    float[] _horizontalLines;
                    UnityEngine.Rect[] _regionRects;
                    
                    GameLib.MapGridDrawUtil.ComputeGridLinePositions( _mapW, _mapH, _gridCols, _regionCols, _regionRows, _unlockedSection, out _verticalLines, out _horizontalLines, out _regionRects );
                    translator.Push(L, _verticalLines);
                        
                    translator.Push(L, _horizontalLines);
                        
                    translator.Push(L, _regionRects);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
