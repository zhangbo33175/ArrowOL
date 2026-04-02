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
    public class HonorRuntimeUIGDPRBehaviourWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.UIGDPRBehaviour);
			Utils.BeginObjectRegister(type, L, translator, 0, 13, 6, 6);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnCOPPAToggleChanged", _m_OnCOPPAToggleChanged);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnGDPRLinkButtonClicked", _m_OnGDPRLinkButtonClicked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnCCPALinkButtonClicked", _m_OnCCPALinkButtonClicked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnCOPPALinkButtonClicked", _m_OnCOPPALinkButtonClicked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnStartGameButtonClicked", _m_OnStartGameButtonClicked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnMoreButtonClicked", _m_OnMoreButtonClicked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetMainBgAnimationVisible", _m_SetMainBgAnimationVisible);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnMoreBackButtonClicked", _m_OnMoreBackButtonClicked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetMoreBgAnimationVisible", _m_SetMoreBgAnimationVisible);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnResetGDPRLinkButtonClicked", _m_OnResetGDPRLinkButtonClicked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnResetCCPALinkButtonClicked", _m_OnResetCCPALinkButtonClicked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnSaveButtonClicked", _m_OnSaveButtonClicked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnCloseButtonClicked", _m_OnCloseButtonClicked);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnOverButtonClickedCallback", _g_get_OnOverButtonClickedCallback);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnGDPRLinkButtonClickedCallback", _g_get_OnGDPRLinkButtonClickedCallback);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnCCPALinkButtonClickedCallback", _g_get_OnCCPALinkButtonClickedCallback);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnCOPPALinkButtonClickedCallback", _g_get_OnCOPPALinkButtonClickedCallback);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OnBackButtonClickedCallback", _g_get_OnBackButtonClickedCallback);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InGame", _g_get_InGame);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnOverButtonClickedCallback", _s_set_OnOverButtonClickedCallback);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnGDPRLinkButtonClickedCallback", _s_set_OnGDPRLinkButtonClickedCallback);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnCCPALinkButtonClickedCallback", _s_set_OnCCPALinkButtonClickedCallback);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnCOPPALinkButtonClickedCallback", _s_set_OnCOPPALinkButtonClickedCallback);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OnBackButtonClickedCallback", _s_set_OnBackButtonClickedCallback);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "InGame", _s_set_InGame);
            
			
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
					
					Honor.Runtime.UIGDPRBehaviour gen_ret = new Honor.Runtime.UIGDPRBehaviour();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIGDPRBehaviour constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnCOPPAToggleChanged(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnCOPPAToggleChanged(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnGDPRLinkButtonClicked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnGDPRLinkButtonClicked(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnCCPALinkButtonClicked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnCCPALinkButtonClicked(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnCOPPALinkButtonClicked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnCOPPALinkButtonClicked(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnStartGameButtonClicked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnStartGameButtonClicked(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnMoreButtonClicked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnMoreButtonClicked(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetMainBgAnimationVisible(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _visible = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetMainBgAnimationVisible( _visible );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnMoreBackButtonClicked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnMoreBackButtonClicked(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetMoreBgAnimationVisible(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _visible = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetMoreBgAnimationVisible( _visible );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnResetGDPRLinkButtonClicked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnResetGDPRLinkButtonClicked(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnResetCCPALinkButtonClicked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnResetCCPALinkButtonClicked(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnSaveButtonClicked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnSaveButtonClicked(  );
                    
                    
                    
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
            
            
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnCloseButtonClicked(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnOverButtonClickedCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnOverButtonClickedCallback);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnGDPRLinkButtonClickedCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnGDPRLinkButtonClickedCallback);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnCCPALinkButtonClickedCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnCCPALinkButtonClickedCallback);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnCOPPALinkButtonClickedCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnCOPPALinkButtonClickedCallback);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OnBackButtonClickedCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OnBackButtonClickedCallback);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InGame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.InGame);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnOverButtonClickedCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnOverButtonClickedCallback = translator.GetDelegate<System.Action>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnGDPRLinkButtonClickedCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnGDPRLinkButtonClickedCallback = translator.GetDelegate<System.Action>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnCCPALinkButtonClickedCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnCCPALinkButtonClickedCallback = translator.GetDelegate<System.Action>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnCOPPALinkButtonClickedCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnCOPPALinkButtonClickedCallback = translator.GetDelegate<System.Action>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OnBackButtonClickedCallback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OnBackButtonClickedCallback = translator.GetDelegate<System.Action>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_InGame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIGDPRBehaviour gen_to_be_invoked = (Honor.Runtime.UIGDPRBehaviour)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.InGame = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
