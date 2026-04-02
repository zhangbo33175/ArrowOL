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
    public class HonorRuntimeProcedureComponentWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.ProcedureComponent);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 23, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitLuaBindings", _m_InitLuaBindings);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasProcedure", _m_HasProcedure);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetProcedure", _m_GetProcedure);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RecordRuntimeProcedureInfos", _m_RecordRuntimeProcedureInfos);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "UITransitionABPath", _g_get_UITransitionABPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UITransitionAssetName", _g_get_UITransitionAssetName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UISplashABPath", _g_get_UISplashABPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UISplashAssetName", _g_get_UISplashAssetName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SplashProcedureDuration", _g_get_SplashProcedureDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseUIPreload", _g_get_UseUIPreload);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UIPreloadABPath", _g_get_UIPreloadABPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UIPreloadAssetName", _g_get_UIPreloadAssetName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RuntimeProcedureRecordInfos", _g_get_RuntimeProcedureRecordInfos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedure", _g_get_CurrentProcedure);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedureTime", _g_get_CurrentProcedureTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedureTransitionEnterFlag", _g_get_CurrentProcedureTransitionEnterFlag);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedureTransitionEnterDuration", _g_get_CurrentProcedureTransitionEnterDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedureTransitionEnterBlockRaycast", _g_get_CurrentProcedureTransitionEnterBlockRaycast);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedureTransitionExitFlag", _g_get_CurrentProcedureTransitionExitFlag);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedureTransitionExitDuration", _g_get_CurrentProcedureTransitionExitDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedureTransitionExitBlockRaycast", _g_get_CurrentProcedureTransitionExitBlockRaycast);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedureTransitionEnterFlagFromProcedureHotfix", _g_get_CurrentProcedureTransitionEnterFlagFromProcedureHotfix);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedureTransitionEnterDurationFromProcedureHotfix", _g_get_CurrentProcedureTransitionEnterDurationFromProcedureHotfix);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedureTransitionEnterBlockRaycastFromProcedureHotfix", _g_get_CurrentProcedureTransitionEnterBlockRaycastFromProcedureHotfix);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedureTransitionExitFlagFromProcedureHotfix", _g_get_CurrentProcedureTransitionExitFlagFromProcedureHotfix);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedureTransitionExitDurationFromProcedureHotfix", _g_get_CurrentProcedureTransitionExitDurationFromProcedureHotfix);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurrentProcedureTransitionExitBlockRaycastFromProcedureHotfix", _g_get_CurrentProcedureTransitionExitBlockRaycastFromProcedureHotfix);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 1);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "LuaScriptWhiteNameList", _g_get_LuaScriptWhiteNameList);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "LuaScriptWhiteNameList", _s_set_LuaScriptWhiteNameList);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Honor.Runtime.ProcedureComponent gen_ret = new Honor.Runtime.ProcedureComponent();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.ProcedureComponent constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitLuaBindings(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitLuaBindings(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasProcedure(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Type _procedureType = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        bool gen_ret = gen_to_be_invoked.HasProcedure( _procedureType );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetProcedure(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Type _procedureType = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        Honor.Runtime.ProcedureState gen_ret = gen_to_be_invoked.GetProcedure( _procedureType );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RecordRuntimeProcedureInfos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _procedureTypeName = LuaAPI.lua_tostring(L, 2);
                    float _time = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.RecordRuntimeProcedureInfos( _procedureTypeName, _time );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UITransitionABPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.UITransitionABPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UITransitionAssetName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.UITransitionAssetName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UISplashABPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.UISplashABPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UISplashAssetName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.UISplashAssetName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SplashProcedureDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SplashProcedureDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseUIPreload(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseUIPreload);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UIPreloadABPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.UIPreloadABPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UIPreloadAssetName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.UIPreloadAssetName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RuntimeProcedureRecordInfos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.RuntimeProcedureRecordInfos);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedure(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.CurrentProcedure);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedureTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CurrentProcedureTime);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedureTransitionEnterFlag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CurrentProcedureTransitionEnterFlag);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedureTransitionEnterDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CurrentProcedureTransitionEnterDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedureTransitionEnterBlockRaycast(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CurrentProcedureTransitionEnterBlockRaycast);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedureTransitionExitFlag(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CurrentProcedureTransitionExitFlag);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedureTransitionExitDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CurrentProcedureTransitionExitDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedureTransitionExitBlockRaycast(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CurrentProcedureTransitionExitBlockRaycast);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedureTransitionEnterFlagFromProcedureHotfix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CurrentProcedureTransitionEnterFlagFromProcedureHotfix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedureTransitionEnterDurationFromProcedureHotfix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CurrentProcedureTransitionEnterDurationFromProcedureHotfix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedureTransitionEnterBlockRaycastFromProcedureHotfix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CurrentProcedureTransitionEnterBlockRaycastFromProcedureHotfix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedureTransitionExitFlagFromProcedureHotfix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CurrentProcedureTransitionExitFlagFromProcedureHotfix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedureTransitionExitDurationFromProcedureHotfix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CurrentProcedureTransitionExitDurationFromProcedureHotfix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurrentProcedureTransitionExitBlockRaycastFromProcedureHotfix(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ProcedureComponent gen_to_be_invoked = (Honor.Runtime.ProcedureComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CurrentProcedureTransitionExitBlockRaycastFromProcedureHotfix);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaScriptWhiteNameList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Honor.Runtime.ProcedureComponent.LuaScriptWhiteNameList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaScriptWhiteNameList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Honor.Runtime.ProcedureComponent.LuaScriptWhiteNameList = (System.Collections.Generic.List<string>)translator.GetObject(L, 1, typeof(System.Collections.Generic.List<string>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
