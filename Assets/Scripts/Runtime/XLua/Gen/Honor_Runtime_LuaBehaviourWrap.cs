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
    public class HonorRuntimeLuaBehaviourWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.LuaBehaviour);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 34, 12);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Proc", _m_Proc);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AwakeAppended", _m_AwakeAppended);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEnableAppended", _m_OnEnableAppended);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAllDirectChildren", _m_GetAllDirectChildren);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CallLuaClose", _m_CallLuaClose);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "BindValues", _g_get_BindValues);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PrefabType", _g_get_PrefabType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaScriptCommonName", _g_get_LuaScriptCommonName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaScriptNames", _g_get_LuaScriptNames);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaSuperScriptNames", _g_get_LuaSuperScriptNames);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseProc", _g_get_UseProc);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaskLayer", _g_get_MaskLayer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BottomCloseLayer", _g_get_BottomCloseLayer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseCollider2DLifeCycles", _g_get_UseCollider2DLifeCycles);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseCollider3DLifeCycles", _g_get_UseCollider3DLifeCycles);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseTrigger2DLifeCycles", _g_get_UseTrigger2DLifeCycles);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseTrigger3DLifeCycles", _g_get_UseTrigger3DLifeCycles);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OpenAnimation", _g_get_OpenAnimation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CloseAnimation", _g_get_CloseAnimation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ShowRaycastTargetsGizmos", _g_get_ShowRaycastTargetsGizmos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Injections", _g_get_Injections);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaParams", _g_get_LuaParams);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "lua", _g_get_lua);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "luaClass", _g_get_luaClass);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "luaClassView", _g_get_luaClassView);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "luaClassViewModel", _g_get_luaClassViewModel);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ValidLuaClass", _g_get_ValidLuaClass);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OwnLuaEnvs", _g_get_OwnLuaEnvs);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OwnLuaClasses", _g_get_OwnLuaClasses);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaAwakes", _g_get_LuaAwakes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaOnEnables", _g_get_LuaOnEnables);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaStarts", _g_get_LuaStarts);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaProcs", _g_get_LuaProcs);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaOnDisables", _g_get_LuaOnDisables);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaOnDestroys", _g_get_LuaOnDestroys);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Collider2DLifeCyclesBehaviour", _g_get_Collider2DLifeCyclesBehaviour);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Collider3DLifeCyclesBehaviour", _g_get_Collider3DLifeCyclesBehaviour);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Trigger2DLifeCyclesBehaviour", _g_get_Trigger2DLifeCyclesBehaviour);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Trigger3DLifeCyclesBehaviour", _g_get_Trigger3DLifeCyclesBehaviour);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseProc", _s_set_UseProc);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OpenAnimation", _s_set_OpenAnimation);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CloseAnimation", _s_set_CloseAnimation);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LuaParams", _s_set_LuaParams);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OwnLuaEnvs", _s_set_OwnLuaEnvs);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OwnLuaClasses", _s_set_OwnLuaClasses);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LuaAwakes", _s_set_LuaAwakes);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LuaOnEnables", _s_set_LuaOnEnables);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LuaStarts", _s_set_LuaStarts);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LuaProcs", _s_set_LuaProcs);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LuaOnDisables", _s_set_LuaOnDisables);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LuaOnDestroys", _s_set_LuaOnDestroys);
            
			
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
					
					Honor.Runtime.LuaBehaviour gen_ret = new Honor.Runtime.LuaBehaviour();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.LuaBehaviour constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Proc(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Proc(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AwakeAppended(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.AwakeAppended(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnEnableAppended(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _isAuto = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.OnEnableAppended( _isAuto );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.OnEnableAppended(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.LuaBehaviour.OnEnableAppended!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAllDirectChildren(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.Collections.Generic.List<Honor.Runtime.LuaBehaviour> gen_ret = gen_to_be_invoked.GetAllDirectChildren(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CallLuaClose(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CallLuaClose(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BindValues(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BindValues);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PrefabType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.PushHonorRuntimePrefabType(L, gen_to_be_invoked.PrefabType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaScriptCommonName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.LuaScriptCommonName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaScriptNames(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaScriptNames);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaSuperScriptNames(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaSuperScriptNames);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseProc(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseProc);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaskLayer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.MaskLayer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BottomCloseLayer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.BottomCloseLayer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseCollider2DLifeCycles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseCollider2DLifeCycles);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseCollider3DLifeCycles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseCollider3DLifeCycles);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseTrigger2DLifeCycles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseTrigger2DLifeCycles);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseTrigger3DLifeCycles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseTrigger3DLifeCycles);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OpenAnimation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.OpenAnimation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CloseAnimation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CloseAnimation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ShowRaycastTargetsGizmos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.ShowRaycastTargetsGizmos);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Injections(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Injections);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaParams(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaParams);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_lua(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.lua);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_luaClass(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.luaClass);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_luaClassView(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.luaClassView);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_luaClassViewModel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.luaClassViewModel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ValidLuaClass(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ValidLuaClass);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OwnLuaEnvs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OwnLuaEnvs);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OwnLuaClasses(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OwnLuaClasses);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaAwakes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaAwakes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaOnEnables(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaOnEnables);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaStarts(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaStarts);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaProcs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaProcs);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaOnDisables(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaOnDisables);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaOnDestroys(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaOnDestroys);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Collider2DLifeCyclesBehaviour(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Collider2DLifeCyclesBehaviour);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Collider3DLifeCyclesBehaviour(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Collider3DLifeCyclesBehaviour);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Trigger2DLifeCyclesBehaviour(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Trigger2DLifeCyclesBehaviour);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Trigger3DLifeCyclesBehaviour(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Trigger3DLifeCyclesBehaviour);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseProc(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseProc = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OpenAnimation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OpenAnimation = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CloseAnimation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CloseAnimation = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaParams(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LuaParams = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OwnLuaEnvs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OwnLuaEnvs = (XLua.LuaTable[])translator.GetObject(L, 2, typeof(XLua.LuaTable[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OwnLuaClasses(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OwnLuaClasses = (XLua.LuaTable[])translator.GetObject(L, 2, typeof(XLua.LuaTable[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaAwakes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LuaAwakes = (System.Action[])translator.GetObject(L, 2, typeof(System.Action[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaOnEnables(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LuaOnEnables = (System.Action[])translator.GetObject(L, 2, typeof(System.Action[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaStarts(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LuaStarts = (System.Action[])translator.GetObject(L, 2, typeof(System.Action[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaProcs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LuaProcs = (System.Action[])translator.GetObject(L, 2, typeof(System.Action[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaOnDisables(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LuaOnDisables = (System.Action[])translator.GetObject(L, 2, typeof(System.Action[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaOnDestroys(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaBehaviour gen_to_be_invoked = (Honor.Runtime.LuaBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LuaOnDestroys = (System.Action[])translator.GetObject(L, 2, typeof(System.Action[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
