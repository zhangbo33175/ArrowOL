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
    public class HonorRuntimeUILauncherLoadingViewWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.UILauncherLoadingView);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 4, 5);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLoadingMode", _m_SetLoadingMode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RefreshViews", _m_RefreshViews);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetVisible", _m_SetVisible);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnStartButtonClicked", _m_OnStartButtonClicked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnRetryButtonClicked", _m_OnRetryButtonClicked);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnCloseButtonClicked", _m_OnCloseButtonClicked);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "EnterTransitionEnabled", _g_get_EnterTransitionEnabled);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EnterTransitionDuration", _g_get_EnterTransitionDuration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ExitTransitionEnabled", _g_get_ExitTransitionEnabled);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ExitTransitionDuration", _g_get_ExitTransitionDuration);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "EnterTransitionEnabled", _s_set_EnterTransitionEnabled);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EnterTransitionDuration", _s_set_EnterTransitionDuration);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ExitTransitionEnabled", _s_set_ExitTransitionEnabled);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ExitTransitionDuration", _s_set_ExitTransitionDuration);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DescContent", _s_set_DescContent);
            
			
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
					
					Honor.Runtime.UILauncherLoadingView gen_ret = new Honor.Runtime.UILauncherLoadingView();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UILauncherLoadingView constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLoadingMode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.UILauncherLoadingView.LoadingMode _loadingMode;translator.Get(L, 2, out _loadingMode);
                    
                    gen_to_be_invoked.SetLoadingMode( _loadingMode );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RefreshViews(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 8&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<Honor.Runtime.GameDefinitions.DownloadStep>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 8)) 
                {
                    string _descContent = LuaAPI.lua_tostring(L, 2);
                    float _progress = (float)LuaAPI.lua_tonumber(L, 3);
                    Honor.Runtime.GameDefinitions.DownloadStep _downloadStep;translator.Get(L, 4, out _downloadStep);
                    int _curFileNum = LuaAPI.xlua_tointeger(L, 5);
                    int _totalFileNum = LuaAPI.xlua_tointeger(L, 6);
                    float _curBytes = (float)LuaAPI.lua_tonumber(L, 7);
                    float _totalBytes = (float)LuaAPI.lua_tonumber(L, 8);
                    
                    gen_to_be_invoked.RefreshViews( _descContent, _progress, _downloadStep, _curFileNum, _totalFileNum, _curBytes, _totalBytes );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<Honor.Runtime.GameDefinitions.DownloadStep>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    string _descContent = LuaAPI.lua_tostring(L, 2);
                    float _progress = (float)LuaAPI.lua_tonumber(L, 3);
                    Honor.Runtime.GameDefinitions.DownloadStep _downloadStep;translator.Get(L, 4, out _downloadStep);
                    int _curFileNum = LuaAPI.xlua_tointeger(L, 5);
                    int _totalFileNum = LuaAPI.xlua_tointeger(L, 6);
                    float _curBytes = (float)LuaAPI.lua_tonumber(L, 7);
                    
                    gen_to_be_invoked.RefreshViews( _descContent, _progress, _downloadStep, _curFileNum, _totalFileNum, _curBytes );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<Honor.Runtime.GameDefinitions.DownloadStep>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    string _descContent = LuaAPI.lua_tostring(L, 2);
                    float _progress = (float)LuaAPI.lua_tonumber(L, 3);
                    Honor.Runtime.GameDefinitions.DownloadStep _downloadStep;translator.Get(L, 4, out _downloadStep);
                    int _curFileNum = LuaAPI.xlua_tointeger(L, 5);
                    int _totalFileNum = LuaAPI.xlua_tointeger(L, 6);
                    
                    gen_to_be_invoked.RefreshViews( _descContent, _progress, _downloadStep, _curFileNum, _totalFileNum );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<Honor.Runtime.GameDefinitions.DownloadStep>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    string _descContent = LuaAPI.lua_tostring(L, 2);
                    float _progress = (float)LuaAPI.lua_tonumber(L, 3);
                    Honor.Runtime.GameDefinitions.DownloadStep _downloadStep;translator.Get(L, 4, out _downloadStep);
                    int _curFileNum = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.RefreshViews( _descContent, _progress, _downloadStep, _curFileNum );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<Honor.Runtime.GameDefinitions.DownloadStep>(L, 4)) 
                {
                    string _descContent = LuaAPI.lua_tostring(L, 2);
                    float _progress = (float)LuaAPI.lua_tonumber(L, 3);
                    Honor.Runtime.GameDefinitions.DownloadStep _downloadStep;translator.Get(L, 4, out _downloadStep);
                    
                    gen_to_be_invoked.RefreshViews( _descContent, _progress, _downloadStep );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    string _descContent = LuaAPI.lua_tostring(L, 2);
                    float _progress = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.RefreshViews( _descContent, _progress );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UILauncherLoadingView.RefreshViews!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetVisible(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _visible = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.SetVisible( _visible );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnStartButtonClicked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnStartButtonClicked(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnRetryButtonClicked(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnRetryButtonClicked(  );
                    
                    
                    
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
            
            
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnCloseButtonClicked(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EnterTransitionEnabled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.EnterTransitionEnabled);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EnterTransitionDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.EnterTransitionDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ExitTransitionEnabled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.ExitTransitionEnabled);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ExitTransitionDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.ExitTransitionDuration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EnterTransitionEnabled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EnterTransitionEnabled = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EnterTransitionDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EnterTransitionDuration = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ExitTransitionEnabled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ExitTransitionEnabled = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ExitTransitionDuration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ExitTransitionDuration = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DescContent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UILauncherLoadingView gen_to_be_invoked = (Honor.Runtime.UILauncherLoadingView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DescContent = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
