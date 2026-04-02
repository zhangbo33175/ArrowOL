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
    public class HonorRuntimeUIWebGLWebViewWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.UIWebGLWebView);
			Utils.BeginObjectRegister(type, L, translator, 0, 9, 2, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnWebGLVuplexViewInitialized", _m_OnWebGLVuplexViewInitialized);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadUrl", _m_LoadUrl);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadHtml", _m_LoadHtml);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateUniWebView", _m_CreateUniWebView);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PostMessage", _m_PostMessage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ExecuteJavaScript", _m_ExecuteJavaScript);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Reload", _m_Reload);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnLoadProgressChanged", _e_OnLoadProgressChanged);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnMessageEmitted", _e_OnMessageEmitted);
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "WebGLVuplexViewInitialized", _g_get_WebGLVuplexViewInitialized);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InitialUrl", _g_get_InitialUrl);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "InitialUrl", _s_set_InitialUrl);
            
			
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
					
					Honor.Runtime.UIWebGLWebView gen_ret = new Honor.Runtime.UIWebGLWebView();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIWebGLWebView constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnWebGLVuplexViewInitialized(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIWebGLWebView gen_to_be_invoked = (Honor.Runtime.UIWebGLWebView)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<object>(L, 2)&& translator.Assignable<object>(L, 3)&& translator.Assignable<Honor.Runtime.EventParams>(L, 4)) 
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    object _userData = translator.GetObject(L, 3, typeof(object));
                    Honor.Runtime.EventParams _e = (Honor.Runtime.EventParams)translator.GetObject(L, 4, typeof(Honor.Runtime.EventParams));
                    
                    gen_to_be_invoked.OnWebGLVuplexViewInitialized( _sender, _userData, _e );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<object>(L, 2)&& translator.Assignable<object>(L, 3)) 
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    object _userData = translator.GetObject(L, 3, typeof(object));
                    
                    gen_to_be_invoked.OnWebGLVuplexViewInitialized( _sender, _userData );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<object>(L, 2)) 
                {
                    object _sender = translator.GetObject(L, 2, typeof(object));
                    
                    gen_to_be_invoked.OnWebGLVuplexViewInitialized( _sender );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.OnWebGLVuplexViewInitialized(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIWebGLWebView.OnWebGLVuplexViewInitialized!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadUrl(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIWebGLWebView gen_to_be_invoked = (Honor.Runtime.UIWebGLWebView)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.LoadUrl( _url );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.LoadUrl(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIWebGLWebView.LoadUrl!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadHtml(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIWebGLWebView gen_to_be_invoked = (Honor.Runtime.UIWebGLWebView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _context = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.LoadHtml( _context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateUniWebView(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIWebGLWebView gen_to_be_invoked = (Honor.Runtime.UIWebGLWebView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CreateUniWebView(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PostMessage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIWebGLWebView gen_to_be_invoked = (Honor.Runtime.UIWebGLWebView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _context = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.PostMessage( _context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ExecuteJavaScript(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIWebGLWebView gen_to_be_invoked = (Honor.Runtime.UIWebGLWebView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _javaScript = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.ExecuteJavaScript( _javaScript );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Reload(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.UIWebGLWebView gen_to_be_invoked = (Honor.Runtime.UIWebGLWebView)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Reload(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WebGLVuplexViewInitialized(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIWebGLWebView gen_to_be_invoked = (Honor.Runtime.UIWebGLWebView)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.WebGLVuplexViewInitialized);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InitialUrl(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIWebGLWebView gen_to_be_invoked = (Honor.Runtime.UIWebGLWebView)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.InitialUrl);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_InitialUrl(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.UIWebGLWebView gen_to_be_invoked = (Honor.Runtime.UIWebGLWebView)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.InitialUrl = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnLoadProgressChanged(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Honor.Runtime.UIWebGLWebView gen_to_be_invoked = (Honor.Runtime.UIWebGLWebView)translator.FastGetCSObj(L, 1);
                System.EventHandler<Honor.Runtime.LoadEventArgs<string>> gen_delegate = translator.GetDelegate<System.EventHandler<Honor.Runtime.LoadEventArgs<string>>>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need System.EventHandler<Honor.Runtime.LoadEventArgs<string>>!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnLoadProgressChanged += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnLoadProgressChanged -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIWebGLWebView.OnLoadProgressChanged!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnMessageEmitted(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Honor.Runtime.UIWebGLWebView gen_to_be_invoked = (Honor.Runtime.UIWebGLWebView)translator.FastGetCSObj(L, 1);
                System.EventHandler<Honor.Runtime.EventArgs<string>> gen_delegate = translator.GetDelegate<System.EventHandler<Honor.Runtime.EventArgs<string>>>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need System.EventHandler<Honor.Runtime.EventArgs<string>>!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnMessageEmitted += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnMessageEmitted -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.UIWebGLWebView.OnMessageEmitted!");
            return 0;
        }
        
		
		
    }
}
