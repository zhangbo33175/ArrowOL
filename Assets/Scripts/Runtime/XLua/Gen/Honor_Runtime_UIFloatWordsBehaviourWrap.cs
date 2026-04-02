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
    public class HonorRuntimeUIFloatWordsBehaviourWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.UIFloatWordsBehaviour);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 5, 3);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "WordsText", _g_get_WordsText);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WordsTextTMP", _g_get_WordsTextTMP);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BottomMaskLayer", _g_get_BottomMaskLayer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TopMaskLayer", _g_get_TopMaskLayer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Duration", _g_get_Duration);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Duration", _s_set_Duration);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BlockUITouches", _s_set_BlockUITouches);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OverCallback", _s_set_OverCallback);
            
			
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
					
					Honor.Runtime.UIFloatWordsBehaviour gen_ret = new Honor.Runtime.UIFloatWordsBehaviour();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIFloatWordsBehaviour constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WordsText(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFloatWordsBehaviour gen_to_be_invoked = (Honor.Runtime.UIFloatWordsBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.WordsText);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WordsTextTMP(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFloatWordsBehaviour gen_to_be_invoked = (Honor.Runtime.UIFloatWordsBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.WordsTextTMP);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BottomMaskLayer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFloatWordsBehaviour gen_to_be_invoked = (Honor.Runtime.UIFloatWordsBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BottomMaskLayer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TopMaskLayer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFloatWordsBehaviour gen_to_be_invoked = (Honor.Runtime.UIFloatWordsBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TopMaskLayer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Duration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFloatWordsBehaviour gen_to_be_invoked = (Honor.Runtime.UIFloatWordsBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.Duration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Duration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFloatWordsBehaviour gen_to_be_invoked = (Honor.Runtime.UIFloatWordsBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Duration = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BlockUITouches(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFloatWordsBehaviour gen_to_be_invoked = (Honor.Runtime.UIFloatWordsBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BlockUITouches = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OverCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFloatWordsBehaviour gen_to_be_invoked = (Honor.Runtime.UIFloatWordsBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OverCallback = translator.GetDelegate<System.Action>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
