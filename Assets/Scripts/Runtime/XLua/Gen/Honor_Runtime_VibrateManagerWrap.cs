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
    public class HonorRuntimeVibrateManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.VibrateManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 9, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Play", _m_Play);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayCustom", _m_PlayCustom);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayCustomGroup", _m_PlayCustomGroup);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayEmphasis", _m_PlayEmphasis);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayEmphasisGroup", _m_PlayEmphasisGroup);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopAll", _m_StopAll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetEnable", _m_SetEnable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetEnable", _m_GetEnable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsSupported", _m_IsSupported);
			
			
			
			
			
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
					
					Honor.Runtime.VibrateManager gen_ret = new Honor.Runtime.VibrateManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.VibrateManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Play(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.VibrateManager gen_to_be_invoked = (Honor.Runtime.VibrateManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.Play(  );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.VibrateType>(L, 2)) 
                {
                    Honor.Runtime.VibrateType _type;translator.Get(L, 2, out _type);
                    
                    gen_to_be_invoked.Play( _type );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.VibrateManager.Play!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayCustom(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.VibrateManager gen_to_be_invoked = (Honor.Runtime.VibrateManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _intensity = (float)LuaAPI.lua_tonumber(L, 2);
                    float _sharpness = (float)LuaAPI.lua_tonumber(L, 3);
                    float _preDuration = (float)LuaAPI.lua_tonumber(L, 4);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 5);
                    System.Action _overCallback = translator.GetDelegate<System.Action>(L, 6);
                    
                    gen_to_be_invoked.PlayCustom( _intensity, _sharpness, _preDuration, _duration, _overCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayCustomGroup(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.VibrateManager gen_to_be_invoked = (Honor.Runtime.VibrateManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.PlayCustomGroup( _luaTable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayEmphasis(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.VibrateManager gen_to_be_invoked = (Honor.Runtime.VibrateManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _amplitude = (float)LuaAPI.lua_tonumber(L, 2);
                    float _frequency = (float)LuaAPI.lua_tonumber(L, 3);
                    float _preDuration = (float)LuaAPI.lua_tonumber(L, 4);
                    float _interval = (float)LuaAPI.lua_tonumber(L, 5);
                    System.Action _overCallback = translator.GetDelegate<System.Action>(L, 6);
                    
                    gen_to_be_invoked.PlayEmphasis( _amplitude, _frequency, _preDuration, _interval, _overCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayEmphasisGroup(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.VibrateManager gen_to_be_invoked = (Honor.Runtime.VibrateManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.PlayEmphasisGroup( _luaTable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopAll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.VibrateManager gen_to_be_invoked = (Honor.Runtime.VibrateManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StopAll(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetEnable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.VibrateManager gen_to_be_invoked = (Honor.Runtime.VibrateManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _enable = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetEnable( _enable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetEnable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.VibrateManager gen_to_be_invoked = (Honor.Runtime.VibrateManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.GetEnable(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsSupported(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.VibrateManager gen_to_be_invoked = (Honor.Runtime.VibrateManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.IsSupported(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
