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
    public class GameLibUtilWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLib.Util);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 6, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GameViewSize", _m_GameViewSize_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsRegexMatch", _m_IsRegexMatch_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReplaceSymbolInName", _m_ReplaceSymbolInName_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetMultiTouchEnabled", _m_SetMultiTouchEnabled_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMultiTouchEnabled", _m_GetMultiTouchEnabled_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "GameLib.Util does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GameViewSize_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        UnityEngine.Vector2 gen_ret = GameLib.Util.GameViewSize(  );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsRegexMatch_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _src = LuaAPI.lua_tostring(L, 1);
                    string _regex = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = GameLib.Util.IsRegexMatch( _src, _regex );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReplaceSymbolInName_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _origin = LuaAPI.lua_tostring(L, 1);
                    
                        string gen_ret = GameLib.Util.ReplaceSymbolInName( _origin );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetMultiTouchEnabled_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool _isEnable = LuaAPI.lua_toboolean(L, 1);
                    
                    GameLib.Util.SetMultiTouchEnabled( _isEnable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMultiTouchEnabled_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        bool gen_ret = GameLib.Util.GetMultiTouchEnabled(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
