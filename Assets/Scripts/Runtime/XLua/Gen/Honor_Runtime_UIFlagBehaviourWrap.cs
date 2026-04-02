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
    public class HonorRuntimeUIFlagBehaviourWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.UIFlagBehaviour);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 4, 4);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaBehaviour", _g_get_LuaBehaviour);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PrefabInstanceGOBehaviour", _g_get_PrefabInstanceGOBehaviour);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UIInfo", _g_get_UIInfo);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FollowParentDestroy", _g_get_FollowParentDestroy);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "LuaBehaviour", _s_set_LuaBehaviour);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PrefabInstanceGOBehaviour", _s_set_PrefabInstanceGOBehaviour);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UIInfo", _s_set_UIInfo);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FollowParentDestroy", _s_set_FollowParentDestroy);
            
			
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
					
					Honor.Runtime.UIFlagBehaviour gen_ret = new Honor.Runtime.UIFlagBehaviour();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIFlagBehaviour constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaBehaviour(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFlagBehaviour gen_to_be_invoked = (Honor.Runtime.UIFlagBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaBehaviour);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PrefabInstanceGOBehaviour(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFlagBehaviour gen_to_be_invoked = (Honor.Runtime.UIFlagBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PrefabInstanceGOBehaviour);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UIInfo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFlagBehaviour gen_to_be_invoked = (Honor.Runtime.UIFlagBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UIInfo);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FollowParentDestroy(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFlagBehaviour gen_to_be_invoked = (Honor.Runtime.UIFlagBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.FollowParentDestroy);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaBehaviour(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFlagBehaviour gen_to_be_invoked = (Honor.Runtime.UIFlagBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LuaBehaviour = (Honor.Runtime.LuaBehaviour)translator.GetObject(L, 2, typeof(Honor.Runtime.LuaBehaviour));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PrefabInstanceGOBehaviour(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFlagBehaviour gen_to_be_invoked = (Honor.Runtime.UIFlagBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PrefabInstanceGOBehaviour = (Honor.Runtime.PrefabInstanceGOBehaviour)translator.GetObject(L, 2, typeof(Honor.Runtime.PrefabInstanceGOBehaviour));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UIInfo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFlagBehaviour gen_to_be_invoked = (Honor.Runtime.UIFlagBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UIInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FollowParentDestroy(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIFlagBehaviour gen_to_be_invoked = (Honor.Runtime.UIFlagBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FollowParentDestroy = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
