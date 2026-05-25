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
    public class HonorRuntimeGesturesUIWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.GesturesUI);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 22, 13);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnUIElementTouchCover", _m_OnUIElementTouchCover);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnUIElementTouchEnd", _m_OnUIElementTouchEnd);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "UITouchCoverCallbacks", _g_get_UITouchCoverCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UITouchEndCallbacks", _g_get_UITouchEndCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectedObjCallbacks", _g_get_SelectedObjCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UpdateSelectedObjCallbacks", _g_get_UpdateSelectedObjCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnselectedObjCallbacks", _g_get_UnselectedObjCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectedObjDragBeginCallbacks", _g_get_SelectedObjDragBeginCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectedObjDragCallbacks", _g_get_SelectedObjDragCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectedObjDragEndCallbacks", _g_get_SelectedObjDragEndCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UICameraIndex", _g_get_UICameraIndex);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UICamera", _g_get_UICamera);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EnableSwitch", _g_get_EnableSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UISwitch", _g_get_UISwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectSwitch", _g_get_SelectSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DragSwitch", _g_get_DragSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectHoldMode", _g_get_SelectHoldMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectReboundInSelectHoldMode", _g_get_SelectReboundInSelectHoldMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectReboundAfterDragEndInSelectHoldMode", _g_get_SelectReboundAfterDragEndInSelectHoldMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PressTimeOfSelectingObj", _g_get_PressTimeOfSelectingObj);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectingTypes", _g_get_SelectingTypes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FindSelectingTypesOnSelf", _g_get_FindSelectingTypesOnSelf);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FindSelectingTypesOnParent", _g_get_FindSelectingTypesOnParent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FindSelectingTypesOnChildren", _g_get_FindSelectingTypesOnChildren);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "UICameraIndex", _s_set_UICameraIndex);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EnableSwitch", _s_set_EnableSwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UISwitch", _s_set_UISwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SelectSwitch", _s_set_SelectSwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DragSwitch", _s_set_DragSwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SelectHoldMode", _s_set_SelectHoldMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SelectReboundInSelectHoldMode", _s_set_SelectReboundInSelectHoldMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SelectReboundAfterDragEndInSelectHoldMode", _s_set_SelectReboundAfterDragEndInSelectHoldMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PressTimeOfSelectingObj", _s_set_PressTimeOfSelectingObj);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SelectingTypes", _s_set_SelectingTypes);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FindSelectingTypesOnSelf", _s_set_FindSelectingTypesOnSelf);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FindSelectingTypesOnParent", _s_set_FindSelectingTypesOnParent);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FindSelectingTypesOnChildren", _s_set_FindSelectingTypesOnChildren);
            
			
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
					
					Honor.Runtime.GesturesUI gen_ret = new Honor.Runtime.GesturesUI();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.GesturesUI constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnUIElementTouchCover(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    HedgehogTeam.EasyTouch.Gesture _gesture = (HedgehogTeam.EasyTouch.Gesture)translator.GetObject(L, 2, typeof(HedgehogTeam.EasyTouch.Gesture));
                    
                    gen_to_be_invoked.OnUIElementTouchCover( _gesture );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnUIElementTouchEnd(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    HedgehogTeam.EasyTouch.Gesture _gesture = (HedgehogTeam.EasyTouch.Gesture)translator.GetObject(L, 2, typeof(HedgehogTeam.EasyTouch.Gesture));
                    
                    gen_to_be_invoked.OnUIElementTouchEnd( _gesture );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UITouchCoverCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UITouchCoverCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UITouchEndCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UITouchEndCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectedObjCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SelectedObjCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UpdateSelectedObjCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UpdateSelectedObjCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnselectedObjCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UnselectedObjCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectedObjDragBeginCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SelectedObjDragBeginCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectedObjDragCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SelectedObjDragCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectedObjDragEndCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SelectedObjDragEndCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UICameraIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.UICameraIndex);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UICamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UICamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EnableSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.EnableSwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UISwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UISwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.SelectSwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DragSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.DragSwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectHoldMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.SelectHoldMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectReboundInSelectHoldMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.SelectReboundInSelectHoldMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectReboundAfterDragEndInSelectHoldMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.SelectReboundAfterDragEndInSelectHoldMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PressTimeOfSelectingObj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.PressTimeOfSelectingObj);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectingTypes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SelectingTypes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FindSelectingTypesOnSelf(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.FindSelectingTypesOnSelf);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FindSelectingTypesOnParent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.FindSelectingTypesOnParent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FindSelectingTypesOnChildren(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.FindSelectingTypesOnChildren);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UICameraIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UICameraIndex = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EnableSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EnableSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UISwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UISwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SelectSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SelectSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DragSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DragSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SelectHoldMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SelectHoldMode = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SelectReboundInSelectHoldMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SelectReboundInSelectHoldMode = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SelectReboundAfterDragEndInSelectHoldMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SelectReboundAfterDragEndInSelectHoldMode = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PressTimeOfSelectingObj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PressTimeOfSelectingObj = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SelectingTypes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SelectingTypes = (System.Collections.Generic.List<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<string>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FindSelectingTypesOnSelf(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FindSelectingTypesOnSelf = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FindSelectingTypesOnParent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FindSelectingTypesOnParent = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FindSelectingTypesOnChildren(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.GesturesUI gen_to_be_invoked = (Honor.Runtime.GesturesUI)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FindSelectingTypesOnChildren = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
