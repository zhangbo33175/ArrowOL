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
    public class HonorRuntimeUIManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.UIManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 36, 18, 9);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenUIAsyncByInfo", _m_OpenUIAsyncByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenUISyncByInfo", _m_OpenUISyncByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddUIAsyncByInfo", _m_AddUIAsyncByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddUISyncByInfo", _m_AddUISyncByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseUIByGO", _m_CloseUIByGO);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseUIByInfo", _m_CloseUIByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseUIsByInfo", _m_CloseUIsByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveUIByGO", _m_RemoveUIByGO);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUIByInfo", _m_GetUIByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUIsByInfo", _m_GetUIsByInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUIsByUIType", _m_GetUIsByUIType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseAllModalUIs", _m_CloseAllModalUIs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseAllUnModalUIs", _m_CloseAllUnModalUIs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseAllSceneUIs", _m_CloseAllSceneUIs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseAllUIs", _m_CloseAllUIs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddFlagToUnloadUIList", _m_AddFlagToUnloadUIList);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsUIExistInModalUIs", _m_IsUIExistInModalUIs);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetFont", _m_SetFont);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RefreshScreenMatchValue", _m_RefreshScreenMatchValue);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitBangsSize", _m_InitBangsSize);
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
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Fonts", _g_get_Fonts);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LastFonts", _g_get_LastFonts);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BlockModalUIsSwitch", _g_get_BlockModalUIsSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BlockAllUIsKeyUpSwitch", _g_get_BlockAllUIsKeyUpSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ConnectionWaitingUIConnection", _g_get_ConnectionWaitingUIConnection);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WaitingUIRefCount", _g_get_WaitingUIRefCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TransitionUI", _g_get_TransitionUI);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurModalUI", _g_get_CurModalUI);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ModalUIInfoList", _g_get_ModalUIInfoList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnModalUIList", _g_get_UnModalUIList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SceneUIList", _g_get_SceneUIList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SubUIList", _g_get_SubUIList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnloadUIList", _g_get_UnloadUIList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DestroyMaxNumPerFrame", _g_get_DestroyMaxNumPerFrame);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CheckTextLocalizings", _g_get_CheckTextLocalizings);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FloatWordsDuration", _g_get_FloatWordsDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UIBangsSize", _g_get_UIBangsSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ScreenOrientation", _g_get_ScreenOrientation);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Fonts", _s_set_Fonts);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LastFonts", _s_set_LastFonts);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BlockModalUIsSwitch", _s_set_BlockModalUIsSwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BlockAllUIsKeyUpSwitch", _s_set_BlockAllUIsKeyUpSwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WaitingUIRefCount", _s_set_WaitingUIRefCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DestroyMaxNumPerFrame", _s_set_DestroyMaxNumPerFrame);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FloatWordsDuration", _s_set_FloatWordsDuration);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UIBangsSize", _s_set_UIBangsSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ScreenOrientation", _s_set_ScreenOrientation);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 1);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "WebUICanvas", _g_get_WebUICanvas);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "WebUICanvas", _s_set_WebUICanvas);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 17 && translator.Assignable<Honor.Runtime.AssetComponent>(L, 2) && translator.Assignable<Honor.Runtime.LocalizationComponent>(L, 3) && translator.Assignable<Honor.Runtime.UIComponent>(L, 4) && translator.Assignable<System.Collections.Generic.List<UnityEngine.Camera>>(L, 5) && translator.Assignable<System.Collections.Generic.List<UnityEngine.Camera>>(L, 6) && translator.Assignable<UnityEngine.Canvas>(L, 7) && translator.Assignable<UnityEngine.Canvas>(L, 8) && translator.Assignable<UnityEngine.Vector2>(L, 9) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 11) && LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 12) && (LuaAPI.lua_isnil(L, 13) || LuaAPI.lua_type(L, 13) == LuaTypes.LUA_TSTRING) && (LuaAPI.lua_isnil(L, 14) || LuaAPI.lua_type(L, 14) == LuaTypes.LUA_TSTRING) && (LuaAPI.lua_isnil(L, 15) || LuaAPI.lua_type(L, 15) == LuaTypes.LUA_TSTRING) && (LuaAPI.lua_isnil(L, 16) || LuaAPI.lua_type(L, 16) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 17))
				{
					Honor.Runtime.AssetComponent _assetComponent = (Honor.Runtime.AssetComponent)translator.GetObject(L, 2, typeof(Honor.Runtime.AssetComponent));
					Honor.Runtime.LocalizationComponent _localizationComponent = (Honor.Runtime.LocalizationComponent)translator.GetObject(L, 3, typeof(Honor.Runtime.LocalizationComponent));
					Honor.Runtime.UIComponent _uiComponent = (Honor.Runtime.UIComponent)translator.GetObject(L, 4, typeof(Honor.Runtime.UIComponent));
					System.Collections.Generic.List<UnityEngine.Camera> _screenUICameras = (System.Collections.Generic.List<UnityEngine.Camera>)translator.GetObject(L, 5, typeof(System.Collections.Generic.List<UnityEngine.Camera>));
					System.Collections.Generic.List<UnityEngine.Camera> _sceneUICameras = (System.Collections.Generic.List<UnityEngine.Camera>)translator.GetObject(L, 6, typeof(System.Collections.Generic.List<UnityEngine.Camera>));
					UnityEngine.Canvas _screenUICanvas = (UnityEngine.Canvas)translator.GetObject(L, 7, typeof(UnityEngine.Canvas));
					UnityEngine.Canvas _sceneUICanvas = (UnityEngine.Canvas)translator.GetObject(L, 8, typeof(UnityEngine.Canvas));
					UnityEngine.Vector2 _screenDesignedResolution;translator.Get(L, 9, out _screenDesignedResolution);
					float _screenWidthHeightMatchValue = (float)LuaAPI.lua_tonumber(L, 10);
					int _destroyMaxNumPerFrame = LuaAPI.xlua_tointeger(L, 11);
					bool _checkTextLocalizings = LuaAPI.lua_toboolean(L, 12);
					string _waitingUIABPath = LuaAPI.lua_tostring(L, 13);
					string _waitingUIAssetName = LuaAPI.lua_tostring(L, 14);
					string _floatWordsUIABPath = LuaAPI.lua_tostring(L, 15);
					string _floatWordsUIAssetName = LuaAPI.lua_tostring(L, 16);
					float _floatWordsDuration = (float)LuaAPI.lua_tonumber(L, 17);
					
					Honor.Runtime.UIManager gen_ret = new Honor.Runtime.UIManager(_assetComponent, _localizationComponent, _uiComponent, _screenUICameras, _sceneUICameras, _screenUICanvas, _sceneUICanvas, _screenDesignedResolution, _screenWidthHeightMatchValue, _destroyMaxNumPerFrame, _checkTextLocalizings, _waitingUIABPath, _waitingUIAssetName, _floatWordsUIABPath, _floatWordsUIAssetName, _floatWordsDuration);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenUIAsyncByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_OpenUISyncByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_AddUIAsyncByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_AddUISyncByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _targetGO = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CloseUIByGO( _targetGO, _rightNow );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUIByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UIInfo _targetUIInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CloseUIByInfo( _targetUIInfo, _rightNow );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUIsByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UIInfo _targetUIInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CloseUIsByInfo( _targetUIInfo, _rightNow );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveUIByGO(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _uiGO = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.RemoveUIByGO( _uiGO, _rightNow );
                    
                    
                    
                    return 0;
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
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UIInfo _targetUIInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    
                        UnityEngine.GameObject gen_ret = gen_to_be_invoked.GetUIByInfo( _targetUIInfo );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUIsByInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.UIInfo>(L, 2)) 
                {
                    Honor.Runtime.UIInfo _targetUIInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    
                        UnityEngine.GameObject[] gen_ret = gen_to_be_invoked.GetUIsByInfo( _targetUIInfo );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.UIInfo>(L, 2)&& translator.Assignable<System.Collections.Generic.List<UnityEngine.GameObject>>(L, 3)) 
                {
                    Honor.Runtime.UIInfo _targetUIInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    System.Collections.Generic.List<UnityEngine.GameObject> _result = (System.Collections.Generic.List<UnityEngine.GameObject>)translator.GetObject(L, 3, typeof(System.Collections.Generic.List<UnityEngine.GameObject>));
                    
                    gen_to_be_invoked.GetUIsByInfo( _targetUIInfo, _result );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIManager.GetUIsByInfo!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUIsByUIType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.UIType>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    Honor.Runtime.UIType _targetUIType;translator.Get(L, 2, out _targetUIType);
                    bool _isAppend = LuaAPI.lua_toboolean(L, 3);
                    
                        UnityEngine.GameObject[] gen_ret = gen_to_be_invoked.GetUIsByUIType( _targetUIType, _isAppend );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<Honor.Runtime.UIType>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.List<UnityEngine.GameObject>>(L, 4)) 
                {
                    Honor.Runtime.UIType _targetUIType;translator.Get(L, 2, out _targetUIType);
                    bool _isAppend = LuaAPI.lua_toboolean(L, 3);
                    System.Collections.Generic.List<UnityEngine.GameObject> _result = (System.Collections.Generic.List<UnityEngine.GameObject>)translator.GetObject(L, 4, typeof(System.Collections.Generic.List<UnityEngine.GameObject>));
                    
                    gen_to_be_invoked.GetUIsByUIType( _targetUIType, _isAppend, _result );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIManager.GetUIsByUIType!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseAllModalUIs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _rightNow = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.CloseAllModalUIs( _rightNow );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseAllUnModalUIs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _rightNow = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.CloseAllUnModalUIs( _rightNow );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseAllSceneUIs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _rightNow = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.CloseAllSceneUIs( _rightNow );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseAllUIs(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UIType _uiType;translator.Get(L, 2, out _uiType);
                    bool _rightNow = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.CloseAllUIs( _uiType, _rightNow );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddFlagToUnloadUIList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UIFlagBehaviour _flagBehaviour = (Honor.Runtime.UIFlagBehaviour)translator.GetObject(L, 2, typeof(Honor.Runtime.UIFlagBehaviour));
                    
                    gen_to_be_invoked.AddFlagToUnloadUIList( _flagBehaviour );
                    
                    
                    
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
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UIInfo _uiInfo = (Honor.Runtime.UIInfo)translator.GetObject(L, 2, typeof(Honor.Runtime.UIInfo));
                    
                        bool gen_ret = gen_to_be_invoked.IsUIExistInModalUIs( _uiInfo );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetFont(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.List<Honor.Runtime.LocalizationFontData> _fontDatas = (System.Collections.Generic.List<Honor.Runtime.LocalizationFontData>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<Honor.Runtime.LocalizationFontData>));
                    UnityEngine.GameObject _ui = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.SetFont( _fontDatas, _ui );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshScreenMatchValue(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _matchValue = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.RefreshScreenMatchValue( _matchValue );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitBangsSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitBangsSize(  );
                    
                    
                    
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
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
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
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIManager.ShowFloatWords!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowSplash(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action _durationOverCallback = translator.GetDelegate<System.Action>(L, 2);
                    
                        Honor.Runtime.UILauncherView gen_ret = gen_to_be_invoked.ShowSplash( _durationOverCallback );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowAppDownload(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _showCloseButton = LuaAPI.lua_toboolean(L, 2);
                    
                        Honor.Runtime.UIAppDownloadBehaviour gen_ret = gen_to_be_invoked.ShowAppDownload( _showCloseButton );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowGDPR(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _inGame = LuaAPI.lua_toboolean(L, 2);
                    System.Action _overButtonClickedCallback = translator.GetDelegate<System.Action>(L, 3);
                    
                        Honor.Runtime.UIGDPRBehaviour gen_ret = gen_to_be_invoked.ShowGDPR( _inGame, _overButtonClickedCallback );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowLoading(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _starNum = LuaAPI.xlua_tointeger(L, 2);
                    string _locationDescForDot = LuaAPI.lua_tostring(L, 3);
                    
                        Honor.Runtime.UIAppFeedbackBehaviour gen_ret = gen_to_be_invoked.ShowAppFeedback( _starNum, _locationDescForDot );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WebUICanvas(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Honor.Runtime.UIManager.WebUICanvas);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Fonts(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Fonts);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LastFonts(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LastFonts);
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.BlockModalUIsSwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BlockAllUIsKeyUpSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.BlockAllUIsKeyUpSwitch);
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.WaitingUIRefCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TransitionUI(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TransitionUI);
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.CurModalUI);
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ModalUIInfoList);
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SubUIList);
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UnloadUIList);
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.DestroyMaxNumPerFrame);
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CheckTextLocalizings);
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.FloatWordsDuration);
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.UIBangsSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ScreenOrientation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ScreenOrientation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WebUICanvas(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Honor.Runtime.UIManager.WebUICanvas = (UnityEngine.Canvas)translator.GetObject(L, 1, typeof(UnityEngine.Canvas));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Fonts(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Fonts = (System.Collections.Generic.List<UnityEngine.Object>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Object>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LastFonts(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LastFonts = (System.Collections.Generic.List<UnityEngine.Object>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Object>));
            
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BlockModalUIsSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BlockAllUIsKeyUpSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BlockAllUIsKeyUpSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WaitingUIRefCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.WaitingUIRefCount = LuaAPI.xlua_tointeger(L, 2);
            
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
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
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FloatWordsDuration = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UIBangsSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.UIBangsSize = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ScreenOrientation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIManager gen_to_be_invoked = (Honor.Runtime.UIManager)translator.FastGetCSObj(L, 1);
                UnityEngine.ScreenOrientation gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.ScreenOrientation = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
