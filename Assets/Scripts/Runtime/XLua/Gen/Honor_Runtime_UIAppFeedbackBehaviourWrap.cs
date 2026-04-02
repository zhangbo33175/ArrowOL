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
    public class HonorRuntimeUIAppFeedbackBehaviourWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.UIAppFeedbackBehaviour);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 4, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnSubmitButtonClicked", _m_OnSubmitButtonClicked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnCloseButtonClicked", _m_OnCloseButtonClicked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEmailInputValueEndEdit", _m_OnEmailInputValueEndEdit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnIssueInputValueEndEdit", _m_OnIssueInputValueEndEdit);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "StarNum", _g_get_StarNum);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LocationDescForDot", _g_get_LocationDescForDot);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EmailContent", _g_get_EmailContent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IssueContent", _g_get_IssueContent);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "StarNum", _s_set_StarNum);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LocationDescForDot", _s_set_LocationDescForDot);
            
			
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
					
					Honor.Runtime.UIAppFeedbackBehaviour gen_ret = new Honor.Runtime.UIAppFeedbackBehaviour();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIAppFeedbackBehaviour constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnSubmitButtonClicked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIAppFeedbackBehaviour gen_to_be_invoked = (Honor.Runtime.UIAppFeedbackBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnSubmitButtonClicked(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnCloseButtonClicked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIAppFeedbackBehaviour gen_to_be_invoked = (Honor.Runtime.UIAppFeedbackBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnCloseButtonClicked(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnEmailInputValueEndEdit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIAppFeedbackBehaviour gen_to_be_invoked = (Honor.Runtime.UIAppFeedbackBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _valueEndEdit = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.OnEmailInputValueEndEdit( _valueEndEdit );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnIssueInputValueEndEdit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIAppFeedbackBehaviour gen_to_be_invoked = (Honor.Runtime.UIAppFeedbackBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _valueEndEdit = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.OnIssueInputValueEndEdit( _valueEndEdit );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StarNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIAppFeedbackBehaviour gen_to_be_invoked = (Honor.Runtime.UIAppFeedbackBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.StarNum);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LocationDescForDot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIAppFeedbackBehaviour gen_to_be_invoked = (Honor.Runtime.UIAppFeedbackBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.LocationDescForDot);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EmailContent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIAppFeedbackBehaviour gen_to_be_invoked = (Honor.Runtime.UIAppFeedbackBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.EmailContent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IssueContent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIAppFeedbackBehaviour gen_to_be_invoked = (Honor.Runtime.UIAppFeedbackBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.IssueContent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_StarNum(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIAppFeedbackBehaviour gen_to_be_invoked = (Honor.Runtime.UIAppFeedbackBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.StarNum = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LocationDescForDot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIAppFeedbackBehaviour gen_to_be_invoked = (Honor.Runtime.UIAppFeedbackBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LocationDescForDot = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
