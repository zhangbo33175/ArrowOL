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
    public class HonorRuntimeUIComponentWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.UIComponent);
			Utils.BeginObjectRegister(type, L, translator, 0, 55, 22, 10);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenUIAsyncByLuaTable", _m_OpenUIAsyncByLuaTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenUIAsyncByInfo", _m_OpenUIAsyncByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenUISyncByLuaTable", _m_OpenUISyncByLuaTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenUISyncByInfo", _m_OpenUISyncByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddUIAsyncByLuaTable", _m_AddUIAsyncByLuaTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddUIAsyncByInfo", _m_AddUIAsyncByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddUISyncByLuaTable", _m_AddUISyncByLuaTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddUISyncByInfo", _m_AddUISyncByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseUIByGO", _m_CloseUIByGO);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseUIByLuaTable", _m_CloseUIByLuaTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseUIByInfo", _m_CloseUIByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseUIsByLuaTable", _m_CloseUIsByLuaTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseUIsByInfo", _m_CloseUIsByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveUIByGO", _m_RemoveUIByGO);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUIByLuaTable", _m_GetUIByLuaTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUIByInfo", _m_GetUIByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUIsByLuaTable", _m_GetUIsByLuaTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUIsByInfo", _m_GetUIsByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUIsByUIType", _m_GetUIsByUIType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseAllModalUIs", _m_CloseAllModalUIs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseAllUnModalUIs", _m_CloseAllUnModalUIs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseAllUIs", _m_CloseAllUIs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddToUnloadUIList", _m_AddToUnloadUIList);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsUIExistInModalUIs", _m_IsUIExistInModalUIs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddSceneUICamera", _m_AddSceneUICamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveSceneUICamera", _m_RemoveSceneUICamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveSceneUICameraByIndex", _m_RemoveSceneUICameraByIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSceneUICameraIndex", _m_GetSceneUICameraIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSceneUICamera", _m_GetSceneUICamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetSceneUICameraEnable", _m_SetSceneUICameraEnable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddScreenUICamera", _m_AddScreenUICamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveScreenUICamera", _m_RemoveScreenUICamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveScreenUICameraByIndex", _m_RemoveScreenUICameraByIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetScreenUICameraIndex", _m_GetScreenUICameraIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetScreenUICamera", _m_GetScreenUICamera);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetScreenUICameraEnable", _m_SetScreenUICameraEnable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadFonts", _m_LoadFonts);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadFonts", _m_UnloadFonts);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RefreshFontsForUI", _m_RefreshFontsForUI);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RefreshScreenMatchValue", _m_RefreshScreenMatchValue);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddWaitingRef", _m_AddWaitingRef);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SubWaitingRef", _m_SubWaitingRef);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsWaitingVisible", _m_IsWaitingVisible);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetWaitingDescText", _m_SetWaitingDescText);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseWebView", _m_CloseWebView);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowProcedureTransitionEnter", _m_ShowProcedureTransitionEnter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowProcedureTransitionExit", _m_ShowProcedureTransitionExit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowFloatWords", _m_ShowFloatWords);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowSplash", _m_ShowSplash);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowAppDownload", _m_ShowAppDownload);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowGDPR", _m_ShowGDPR);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowLoading", _m_ShowLoading);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HideLoading", _m_HideLoading);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowAppReview", _m_ShowAppReview);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowAppFeedback", _m_ShowAppFeedback);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "ScreenDesignedResolution", _g_get_ScreenDesignedResolution);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ScreenWidthHeightMatchValue", _g_get_ScreenWidthHeightMatchValue);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DestroyMaxNumPerFrame", _g_get_DestroyMaxNumPerFrame);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FloatWordsDuration", _g_get_FloatWordsDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ButtonInteractDuration", _g_get_ButtonInteractDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ScreenUICameras", _g_get_ScreenUICameras);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SceneUICameras", _g_get_SceneUICameras);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ScreenUICanvas", _g_get_ScreenUICanvas);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SceneUICanvas", _g_get_SceneUICanvas);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CheckOrientationState", _g_get_CheckOrientationState);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CheckTextLocalizings", _g_get_CheckTextLocalizings);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UIBangsSize", _g_get_UIBangsSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BlockModalUIsSwitch", _g_get_BlockModalUIsSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ConnectionWaitingUIConnection", _g_get_ConnectionWaitingUIConnection);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WaitingUIRefCount", _g_get_WaitingUIRefCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ProcedureTransitionUI", _g_get_ProcedureTransitionUI);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnloadUIList", _g_get_UnloadUIList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurModalUI", _g_get_CurModalUI);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnModalUIList", _g_get_UnModalUIList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SceneUIList", _g_get_SceneUIList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SubUIList", _g_get_SubUIList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ModalUIInfoList", _g_get_ModalUIInfoList);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "ScreenWidthHeightMatchValue", _s_set_ScreenWidthHeightMatchValue);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DestroyMaxNumPerFrame", _s_set_DestroyMaxNumPerFrame);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FloatWordsDuration", _s_set_FloatWordsDuration);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ButtonInteractDuration", _s_set_ButtonInteractDuration);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ScreenUICameras", _s_set_ScreenUICameras);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ScreenUICanvas", _s_set_ScreenUICanvas);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SceneUICanvas", _s_set_SceneUICanvas);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CheckOrientationState", _s_set_CheckOrientationState);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CheckTextLocalizings", _s_set_CheckTextLocalizings);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BlockModalUIsSwitch", _s_set_BlockModalUIsSwitch);
            
			
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
					
					Honor.Runtime.UIComponent gen_ret = new Honor.Runtime.UIComponent();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenUIAsyncByLuaTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& translator.Assignable<Honor.Runtime.UILoadOverCallback>(L, 4)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    Honor.Runtime.UILoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.UILoadOverCallback>(L, 4);
                    
                    gen_to_be_invoked.OpenUIAsyncByLuaTable( _luaTable, _luaParams, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.OpenUIAsyncByLuaTable( _luaTable, _luaParams );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.OpenUIAsyncByLuaTable( _luaTable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.OpenUIAsyncByLuaTable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenUIAsyncByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    
                    gen_to_be_invoked.OpenUIAsyncByInfo( _uiInfo );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenUISyncByLuaTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.OpenUISyncByLuaTable( _luaTable, _luaParams );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.OpenUISyncByLuaTable( _luaTable );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.OpenUISyncByLuaTable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenUISyncByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.OpenUISyncByInfo( _uiInfo );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddUIAsyncByLuaTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& translator.Assignable<UnityEngine.Transform>(L, 3)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TTABLE)&& translator.Assignable<Honor.Runtime.UILoadOverCallback>(L, 5)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    Honor.Runtime.UILoadOverCallback _overCallback = translator.GetDelegate<Honor.Runtime.UILoadOverCallback>(L, 5);
                    
                    gen_to_be_invoked.AddUIAsyncByLuaTable( _luaTable, _parent, _luaParams, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& translator.Assignable<UnityEngine.Transform>(L, 3)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.AddUIAsyncByLuaTable( _luaTable, _parent, _luaParams );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& translator.Assignable<UnityEngine.Transform>(L, 3)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    
                    gen_to_be_invoked.AddUIAsyncByLuaTable( _luaTable, _parent );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.AddUIAsyncByLuaTable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddUIAsyncByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    
                    gen_to_be_invoked.AddUIAsyncByInfo( _uiInfo, _parent );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddUISyncByLuaTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& translator.Assignable<UnityEngine.Transform>(L, 3)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    XLua.LuaTable _luaParams = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.AddUISyncByLuaTable( _luaTable, _parent, _luaParams );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& translator.Assignable<UnityEngine.Transform>(L, 3)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.AddUISyncByLuaTable( _luaTable, _parent );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.AddUISyncByLuaTable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddUISyncByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 3, typeof(UnityEngine.Transform));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.AddUISyncByInfo( _uiInfo, _parent );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUIByGO(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.GameObject>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.GameObject _uiGO = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CloseUIByGO( _uiGO, _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 2)) 
                {
                    UnityEngine.GameObject _uiGO = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.CloseUIByGO( _uiGO );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.CloseUIByGO!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUIByLuaTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CloseUIByLuaTable( _luaTable, _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.CloseUIByLuaTable( _luaTable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.CloseUIByLuaTable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUIByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.UIInfo>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CloseUIByInfo( _uiInfo, _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.UIInfo>(L, 2)) 
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    
                    gen_to_be_invoked.CloseUIByInfo( _uiInfo );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.CloseUIByInfo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUIsByLuaTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CloseUIsByLuaTable( _luaTable, _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.CloseUIsByLuaTable( _luaTable );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.CloseUIsByLuaTable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUIsByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.UIInfo>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CloseUIsByInfo( _uiInfo, _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.UIInfo>(L, 2)) 
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    
                    gen_to_be_invoked.CloseUIsByInfo( _uiInfo );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.CloseUIsByInfo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveUIByGO(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.GameObject>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.GameObject _uiGO = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.RemoveUIByGO( _uiGO, _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 2)) 
                {
                    UnityEngine.GameObject _uiGO = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.RemoveUIByGO( _uiGO );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.RemoveUIByGO!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUIByLuaTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.GetUIByLuaTable( _luaTable );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUIByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.GetUIByInfo( _uiInfo );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUIsByLuaTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    
                        UnityEngine.GameObject[] gen_ret = gen_to_be_invoked.GetUIsByLuaTable( _luaTable );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& translator.Assignable<System.Collections.Generic.List<UnityEngine.GameObject>>(L, 3)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    System.Collections.Generic.List<UnityEngine.GameObject> _uis = (System.Collections.Generic.List<UnityEngine.GameObject>)translator.GetObject(L, 3, typeof(System.Collections.Generic.List<UnityEngine.GameObject>));
                    
                    gen_to_be_invoked.GetUIsByLuaTable( _luaTable, _uis );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.GetUIsByLuaTable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUIsByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.UIInfo>(L, 2)) 
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    
                        UnityEngine.GameObject[] gen_ret = gen_to_be_invoked.GetUIsByInfo( _uiInfo );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.UIInfo>(L, 2)&& translator.Assignable<System.Collections.Generic.List<UnityEngine.GameObject>>(L, 3)) 
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    System.Collections.Generic.List<UnityEngine.GameObject> _uis = (System.Collections.Generic.List<UnityEngine.GameObject>)translator.GetObject(L, 3, typeof(System.Collections.Generic.List<UnityEngine.GameObject>));
                    
                    gen_to_be_invoked.GetUIsByInfo( _uiInfo, _uis );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.GetUIsByInfo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUIsByUIType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.UIType>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    Honor.Runtime.UIType _uiType;translator.Get(L, 2, out _uiType);
                    bool _isAppend = LuaAPI.lua_toboolean(L, 3);
                    
                        UnityEngine.GameObject[] gen_ret = gen_to_be_invoked.GetUIsByUIType( _uiType, _isAppend );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<Honor.Runtime.UIType>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.List<UnityEngine.GameObject>>(L, 4)) 
                {
                    Honor.Runtime.UIType _uiType;translator.Get(L, 2, out _uiType);
                    bool _isAppend = LuaAPI.lua_toboolean(L, 3);
                    System.Collections.Generic.List<UnityEngine.GameObject> _uis = (System.Collections.Generic.List<UnityEngine.GameObject>)translator.GetObject(L, 4, typeof(System.Collections.Generic.List<UnityEngine.GameObject>));
                    
                    gen_to_be_invoked.GetUIsByUIType( _uiType, _isAppend, _uis );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.GetUIsByUIType!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseAllModalUIs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _rightNow = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.CloseAllModalUIs( _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.CloseAllModalUIs(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.CloseAllModalUIs!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseAllUnModalUIs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _rightNow = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.CloseAllUnModalUIs( _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.CloseAllUnModalUIs(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.CloseAllUnModalUIs!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseAllUIs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.UIType>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    Honor.Runtime.UIType _uiType;translator.Get(L, 2, out _uiType);
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CloseAllUIs( _uiType, _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.UIType>(L, 2)) 
                {
                    Honor.Runtime.UIType _uiType;translator.Get(L, 2, out _uiType);
                    
                    gen_to_be_invoked.CloseAllUIs( _uiType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.CloseAllUIs!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddToUnloadUIList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.AddToUnloadUIList( _go );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsUIExistInModalUIs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luaTable = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    
                        bool gen_ret = gen_to_be_invoked.IsUIExistInModalUIs( _luaTable );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.UIInfo>(L, 2)) 
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    
                        bool gen_ret = gen_to_be_invoked.IsUIExistInModalUIs( _uiInfo );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.IsUIExistInModalUIs!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddSceneUICamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                        int gen_ret = gen_to_be_invoked.AddSceneUICamera( _camera );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveSceneUICamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                        bool gen_ret = gen_to_be_invoked.RemoveSceneUICamera( _camera );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveSceneUICameraByIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.RemoveSceneUICameraByIndex( _index );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSceneUICameraIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                        int gen_ret = gen_to_be_invoked.GetSceneUICameraIndex( _camera );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSceneUICamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.Camera gen_ret = gen_to_be_invoked.GetSceneUICamera( _index );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSceneUICameraEnable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    bool _enabled = LuaAPI.lua_toboolean(L, 2);
                    int _index = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.SetSceneUICameraEnable( _enabled, _index );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _enabled = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetSceneUICameraEnable( _enabled );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.SetSceneUICameraEnable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddScreenUICamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                        int gen_ret = gen_to_be_invoked.AddScreenUICamera( _camera );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveScreenUICamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                        bool gen_ret = gen_to_be_invoked.RemoveScreenUICamera( _camera );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveScreenUICameraByIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.RemoveScreenUICameraByIndex( _index );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetScreenUICameraIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    
                        int gen_ret = gen_to_be_invoked.GetScreenUICameraIndex( _camera );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetScreenUICamera(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.Camera gen_ret = gen_to_be_invoked.GetScreenUICamera( _index );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetScreenUICameraEnable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    bool _enabled = LuaAPI.lua_toboolean(L, 2);
                    int _index = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.SetScreenUICameraEnable( _enabled, _index );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _enabled = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetScreenUICameraEnable( _enabled );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.SetScreenUICameraEnable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadFonts(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LoadFonts(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadFonts(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _rightNow = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.UnloadFonts( _rightNow );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.UnloadFonts(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.UnloadFonts!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshFontsForUI(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 2)) 
                {
                    UnityEngine.GameObject _ui = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.RefreshFontsForUI( _ui );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.RefreshFontsForUI(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.RefreshFontsForUI!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshScreenMatchValue(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.RefreshScreenMatchValue(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddWaitingRef(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.AddWaitingRef(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SubWaitingRef(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.SubWaitingRef(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsWaitingVisible(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.IsWaitingVisible(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetWaitingDescText(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _text = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.SetWaitingDescText( _text );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseWebView(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _closeView = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.CloseWebView( _closeView );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowProcedureTransitionEnter(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _forceOver = LuaAPI.lua_toboolean(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _blockRaycast = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.ShowProcedureTransitionEnter( _forceOver, _duration, _blockRaycast );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowProcedureTransitionExit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _forceOver = LuaAPI.lua_toboolean(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _blockRaycast = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.ShowProcedureTransitionExit( _forceOver, _duration, _blockRaycast );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowFloatWords(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    string _text = LuaAPI.lua_tostring(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _blockUITouches = LuaAPI.lua_toboolean(L, 4);
                    System.Action _overCallback = translator.GetDelegate<System.Action>(L, 5);
                    
                    gen_to_be_invoked.ShowFloatWords( _text, _duration, _blockUITouches, _overCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    string _text = LuaAPI.lua_tostring(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _blockUITouches = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.ShowFloatWords( _text, _duration, _blockUITouches );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _text = LuaAPI.lua_tostring(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.ShowFloatWords( _text, _duration );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _text = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.ShowFloatWords( _text );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.ShowFloatWords!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowSplash(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<System.Action>(L, 2)) 
                {
                    System.Action _durationOverCallback = translator.GetDelegate<System.Action>(L, 2);
                    
                        Honor.Runtime.UILauncherView gen_ret = gen_to_be_invoked.ShowSplash( _durationOverCallback );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        Honor.Runtime.UILauncherView gen_ret = gen_to_be_invoked.ShowSplash(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.ShowSplash!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowAppDownload(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _showCloseButton = LuaAPI.lua_toboolean(L, 2);
                    
                        Honor.Runtime.UIAppDownloadBehaviour gen_ret = gen_to_be_invoked.ShowAppDownload( _showCloseButton );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        Honor.Runtime.UIAppDownloadBehaviour gen_ret = gen_to_be_invoked.ShowAppDownload(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.ShowAppDownload!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowGDPR(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Action>(L, 3)) 
                {
                    bool _inGame = LuaAPI.lua_toboolean(L, 2);
                    System.Action _overButtonClickedCallback = translator.GetDelegate<System.Action>(L, 3);
                    
                        Honor.Runtime.UIGDPRBehaviour gen_ret = gen_to_be_invoked.ShowGDPR( _inGame, _overButtonClickedCallback );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _inGame = LuaAPI.lua_toboolean(L, 2);
                    
                        Honor.Runtime.UIGDPRBehaviour gen_ret = gen_to_be_invoked.ShowGDPR( _inGame );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        Honor.Runtime.UIGDPRBehaviour gen_ret = gen_to_be_invoked.ShowGDPR(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.ShowGDPR!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowLoading(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UILauncherLoadingView.LoadingMode _loadingMode;translator.Get(L, 2, out _loadingMode);
                    
                        Honor.Runtime.UILauncherLoadingView gen_ret = gen_to_be_invoked.ShowLoading( _loadingMode );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HideLoading(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UILauncherLoadingView _uiLauncher = (Honor.Runtime.UILauncherLoadingView)translator.GetObject(L, 2, typeof(Honor.Runtime.UILauncherLoadingView));
                    
                    gen_to_be_invoked.HideLoading( _uiLauncher );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowAppReview(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        Honor.Runtime.UIAppReviewBehaviour gen_ret = gen_to_be_invoked.ShowAppReview(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowAppFeedback(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    int _starNum = LuaAPI.xlua_tointeger(L, 2);
                    string _locationDescForDot = LuaAPI.lua_tostring(L, 3);
                    
                        Honor.Runtime.UIAppFeedbackBehaviour gen_ret = gen_to_be_invoked.ShowAppFeedback( _starNum, _locationDescForDot );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int _starNum = LuaAPI.xlua_tointeger(L, 2);
                    
                        Honor.Runtime.UIAppFeedbackBehaviour gen_ret = gen_to_be_invoked.ShowAppFeedback( _starNum );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        Honor.Runtime.UIAppFeedbackBehaviour gen_ret = gen_to_be_invoked.ShowAppFeedback(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIComponent.ShowAppFeedback!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ScreenDesignedResolution(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.ScreenDesignedResolution);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ScreenWidthHeightMatchValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.ScreenWidthHeightMatchValue);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DestroyMaxNumPerFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.DestroyMaxNumPerFrame);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FloatWordsDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.FloatWordsDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ButtonInteractDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.ButtonInteractDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ScreenUICameras(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ScreenUICameras);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SceneUICameras(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SceneUICameras);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ScreenUICanvas(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ScreenUICanvas);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SceneUICanvas(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SceneUICanvas);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CheckOrientationState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CheckOrientationState);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CheckTextLocalizings(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CheckTextLocalizings);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UIBangsSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.UIBangsSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BlockModalUIsSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.BlockModalUIsSwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ConnectionWaitingUIConnection(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ConnectionWaitingUIConnection);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WaitingUIRefCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.WaitingUIRefCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ProcedureTransitionUI(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ProcedureTransitionUI);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnloadUIList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UnloadUIList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurModalUI(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.CurModalUI);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnModalUIList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UnModalUIList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SceneUIList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SceneUIList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SubUIList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SubUIList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ModalUIInfoList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ModalUIInfoList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ScreenWidthHeightMatchValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ScreenWidthHeightMatchValue = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DestroyMaxNumPerFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DestroyMaxNumPerFrame = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FloatWordsDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FloatWordsDuration = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ButtonInteractDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ButtonInteractDuration = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ScreenUICameras(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ScreenUICameras = (System.Collections.Generic.List<UnityEngine.Camera>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Camera>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ScreenUICanvas(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ScreenUICanvas = (UnityEngine.Canvas)translator.GetObject(L, 2, typeof(UnityEngine.Canvas));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SceneUICanvas(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SceneUICanvas = (UnityEngine.Canvas)translator.GetObject(L, 2, typeof(UnityEngine.Canvas));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CheckOrientationState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CheckOrientationState = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CheckTextLocalizings(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CheckTextLocalizings = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BlockModalUIsSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIComponent gen_to_be_invoked = (Honor.Runtime.UIComponent)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BlockModalUIsSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
