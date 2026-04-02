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
    public class HonorRuntimeAorTextLocalizingWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.AorTextLocalizing);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 5, 3);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "LocalizingKeyName", _g_get_LocalizingKeyName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LocalizingFontMark", _g_get_LocalizingFontMark);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LocalizingValue", _g_get_LocalizingValue);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Language", _g_get_Language);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OpenPosOffset", _g_get_OpenPosOffset);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "LocalizingKeyName", _s_set_LocalizingKeyName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Language", _s_set_Language);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OpenPosOffset", _s_set_OpenPosOffset);
            
			
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
					
					Honor.Runtime.AorTextLocalizing gen_ret = new Honor.Runtime.AorTextLocalizing();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AorTextLocalizing constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LocalizingKeyName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AorTextLocalizing gen_to_be_invoked = (Honor.Runtime.AorTextLocalizing)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.LocalizingKeyName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LocalizingFontMark(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AorTextLocalizing gen_to_be_invoked = (Honor.Runtime.AorTextLocalizing)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.LocalizingFontMark);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LocalizingValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AorTextLocalizing gen_to_be_invoked = (Honor.Runtime.AorTextLocalizing)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.LocalizingValue);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Language(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AorTextLocalizing gen_to_be_invoked = (Honor.Runtime.AorTextLocalizing)translator.FastGetCSObj(L, 1);
                translator.PushHonorRuntimeGameDefinitionsLanguage(L, gen_to_be_invoked.Language);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OpenPosOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AorTextLocalizing gen_to_be_invoked = (Honor.Runtime.AorTextLocalizing)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.OpenPosOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LocalizingKeyName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AorTextLocalizing gen_to_be_invoked = (Honor.Runtime.AorTextLocalizing)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LocalizingKeyName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Language(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AorTextLocalizing gen_to_be_invoked = (Honor.Runtime.AorTextLocalizing)translator.FastGetCSObj(L, 1);
                Honor.Runtime.GameDefinitions.Language gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.Language = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OpenPosOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AorTextLocalizing gen_to_be_invoked = (Honor.Runtime.AorTextLocalizing)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OpenPosOffset = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
