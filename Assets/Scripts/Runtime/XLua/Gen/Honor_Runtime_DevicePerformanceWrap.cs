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
    public class HonorRuntimeDevicePerformanceWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.DevicePerformance);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 6, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetDevicePerformanceLevel", _m_GetDevicePerformanceLevel_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ModifyQualityLevelsBasedOnPerformanceLevel", _m_ModifyQualityLevelsBasedOnPerformanceLevel_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ModifyQualitySettingsBasedOnPerformanceLevel", _m_ModifyQualitySettingsBasedOnPerformanceLevel_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetQualitySettings", _m_SetQualitySettings_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetDevicePerformanceLevelColor", _m_GetDevicePerformanceLevelColor_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.DevicePerformance does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDevicePerformanceLevel_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        Honor.Runtime.DevicePerformanceLevel gen_ret = Honor.Runtime.DevicePerformance.GetDevicePerformanceLevel(  );
                        translator.PushHonorRuntimeDevicePerformanceLevel(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ModifyQualityLevelsBasedOnPerformanceLevel_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _lowQuality = LuaAPI.xlua_tointeger(L, 1);
                    int _midQuality = LuaAPI.xlua_tointeger(L, 2);
                    int _highQuality = LuaAPI.xlua_tointeger(L, 3);
                    
                    Honor.Runtime.DevicePerformance.ModifyQualityLevelsBasedOnPerformanceLevel( _lowQuality, _midQuality, _highQuality );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ModifyQualitySettingsBasedOnPerformanceLevel_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Honor.Runtime.DevicePerformance.ModifyQualitySettingsBasedOnPerformanceLevel(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetQualitySettings_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Honor.Runtime.QualityLevel _qualityLevel;translator.Get(L, 1, out _qualityLevel);
                    
                    Honor.Runtime.DevicePerformance.SetQualitySettings( _qualityLevel );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDevicePerformanceLevelColor_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Honor.Runtime.DevicePerformanceLevel _level;translator.Get(L, 1, out _level);
                    
                        UnityEngine.Color gen_ret = Honor.Runtime.DevicePerformance.GetDevicePerformanceLevelColor( _level );
                        translator.PushUnityEngineColor(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
