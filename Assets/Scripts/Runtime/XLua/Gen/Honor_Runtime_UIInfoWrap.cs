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
    public class HonorRuntimeUIInfoWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.UIInfo);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 13, 14);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Equals", _m_Equals);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "UIType", _g_get_UIType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ABPath", _g_get_ABPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetName", _g_get_AssetName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsAppend", _g_get_IsAppend);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsModal", _g_get_IsModal);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ZOrder", _g_get_ZOrder);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Priority", _g_get_Priority);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CloseOnEscapeKeyUp", _g_get_CloseOnEscapeKeyUp);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BlockingMaskValue", _g_get_BlockingMaskValue);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BlockingObjects", _g_get_BlockingObjects);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MultiTypeTextCompsCoexist", _g_get_MultiTypeTextCompsCoexist);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaParams", _g_get_LuaParams);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OverCallback", _g_get_OverCallback);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "UIType", _s_set_UIType);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ABPath", _s_set_ABPath);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetName", _s_set_AssetName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsAppend", _s_set_IsAppend);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsModal", _s_set_IsModal);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ZOrder", _s_set_ZOrder);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Priority", _s_set_Priority);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CloseOnEscapeKeyUp", _s_set_CloseOnEscapeKeyUp);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BlockingMaskValue", _s_set_BlockingMaskValue);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BlockingMask", _s_set_BlockingMask);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BlockingObjects", _s_set_BlockingObjects);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MultiTypeTextCompsCoexist", _s_set_MultiTypeTextCompsCoexist);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LuaParams", _s_set_LuaParams);
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
					
					Honor.Runtime.UIInfo gen_ret = new Honor.Runtime.UIInfo();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIInfo constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Equals(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    
                        bool gen_ret = gen_to_be_invoked.Equals( _uiInfo );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UIType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                translator.PushHonorRuntimeUIType(L, gen_to_be_invoked.UIType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ABPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.ABPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.AssetName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsAppend(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsAppend);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsModal(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsModal);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ZOrder(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ZOrder);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Priority(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Priority);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CloseOnEscapeKeyUp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CloseOnEscapeKeyUp);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BlockingMaskValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BlockingMaskValue);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BlockingObjects(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BlockingObjects);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MultiTypeTextCompsCoexist(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.MultiTypeTextCompsCoexist);
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
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaParams);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OverCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OverCallback);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UIType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                Honor.Runtime.UIType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.UIType = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ABPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ABPath = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AssetName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsAppend(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsAppend = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsModal(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsModal = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ZOrder(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ZOrder = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Priority(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Priority = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CloseOnEscapeKeyUp(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CloseOnEscapeKeyUp = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BlockingMaskValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                UnityEngine.LayerMask gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.BlockingMaskValue = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BlockingMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BlockingMask = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BlockingObjects(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                UnityEngine.UI.GraphicRaycaster.BlockingObjects gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.BlockingObjects = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MultiTypeTextCompsCoexist(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MultiTypeTextCompsCoexist = LuaAPI.lua_toboolean(L, 2);
            
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
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LuaParams = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
            
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
			
                Honor.Runtime.UIInfo gen_to_be_invoked = (Honor.Runtime.UIInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OverCallback = translator.GetDelegate<Honor.Runtime.UILoadOverCallback>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
