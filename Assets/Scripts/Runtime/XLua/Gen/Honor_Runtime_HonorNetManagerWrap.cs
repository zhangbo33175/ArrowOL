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
    public class HonorRuntimeHonorNetManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.HonorNetManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 10, 2, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Dispose", _m_Dispose);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CheckNetworkActive", _m_CheckNetworkActive);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UrlEncode", _m_UrlEncode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTextFromUrl", _m_GetTextFromUrl);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HttpRequestOnGet", _m_HttpRequestOnGet);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HttpRequestOnPost", _m_HttpRequestOnPost);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HttpRequestOnPostWithFile", _m_HttpRequestOnPostWithFile);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateWebSocketConnection", _m_CreateWebSocketConnection);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetWebSocket", _m_GetWebSocket);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Network", _g_get_Network);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsNetworkReady", _g_get_IsNetworkReady);
            
			
			
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
					
					Honor.Runtime.HonorNetManager gen_ret = new Honor.Runtime.HonorNetManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorNetManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorNetManager gen_to_be_invoked = (Honor.Runtime.HonorNetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Dispose(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorNetManager gen_to_be_invoked = (Honor.Runtime.HonorNetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Dispose(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckNetworkActive(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorNetManager gen_to_be_invoked = (Honor.Runtime.HonorNetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool gen_ret = gen_to_be_invoked.CheckNetworkActive(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UrlEncode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorNetManager gen_to_be_invoked = (Honor.Runtime.HonorNetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _str = LuaAPI.lua_tostring(L, 2);
                    
                        string gen_ret = gen_to_be_invoked.UrlEncode( _str );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTextFromUrl(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorNetManager gen_to_be_invoked = (Honor.Runtime.HonorNetManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string>>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    System.Action<string> _finishCallback = translator.GetDelegate<System.Action<string>>(L, 3);
                    int _timeout = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.GetTextFromUrl( _url, _finishCallback, _timeout );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Action<string>>(L, 3)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    System.Action<string> _finishCallback = translator.GetDelegate<System.Action<string>>(L, 3);
                    
                    gen_to_be_invoked.GetTextFromUrl( _url, _finishCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.GetTextFromUrl( _url );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorNetManager.GetTextFromUrl!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HttpRequestOnGet(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorNetManager gen_to_be_invoked = (Honor.Runtime.HonorNetManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 7&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& (LuaAPI.lua_isnil(L, 7) || LuaAPI.lua_type(L, 7) == LuaTypes.LUA_TSTRING)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 3);
                    bool _keepAlive = LuaAPI.lua_toboolean(L, 4);
                    float _requestTimeout = (float)LuaAPI.lua_tonumber(L, 5);
                    float _connectTimeout = (float)LuaAPI.lua_tonumber(L, 6);
                    string _headerInfos = LuaAPI.lua_tostring(L, 7);
                    
                    gen_to_be_invoked.HttpRequestOnGet( _url, _finishedCallback, _keepAlive, _requestTimeout, _connectTimeout, _headerInfos );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 3);
                    bool _keepAlive = LuaAPI.lua_toboolean(L, 4);
                    float _requestTimeout = (float)LuaAPI.lua_tonumber(L, 5);
                    float _connectTimeout = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    gen_to_be_invoked.HttpRequestOnGet( _url, _finishedCallback, _keepAlive, _requestTimeout, _connectTimeout );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 3);
                    bool _keepAlive = LuaAPI.lua_toboolean(L, 4);
                    float _requestTimeout = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.HttpRequestOnGet( _url, _finishedCallback, _keepAlive, _requestTimeout );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 3);
                    bool _keepAlive = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.HttpRequestOnGet( _url, _finishedCallback, _keepAlive );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 3)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 3);
                    
                    gen_to_be_invoked.HttpRequestOnGet( _url, _finishedCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.HttpRequestOnGet( _url );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorNetManager.HttpRequestOnGet!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HttpRequestOnPost(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorNetManager gen_to_be_invoked = (Honor.Runtime.HonorNetManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 8&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)&& (LuaAPI.lua_isnil(L, 8) || LuaAPI.lua_type(L, 8) == LuaTypes.LUA_TSTRING)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _contentString = LuaAPI.lua_tostring(L, 3);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 4);
                    bool _keepAlive = LuaAPI.lua_toboolean(L, 5);
                    float _requestTimeout = (float)LuaAPI.lua_tonumber(L, 6);
                    float _connectTimeout = (float)LuaAPI.lua_tonumber(L, 7);
                    string _headerInfos = LuaAPI.lua_tostring(L, 8);
                    
                    gen_to_be_invoked.HttpRequestOnPost( _url, _contentString, _finishedCallback, _keepAlive, _requestTimeout, _connectTimeout, _headerInfos );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _contentString = LuaAPI.lua_tostring(L, 3);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 4);
                    bool _keepAlive = LuaAPI.lua_toboolean(L, 5);
                    float _requestTimeout = (float)LuaAPI.lua_tonumber(L, 6);
                    float _connectTimeout = (float)LuaAPI.lua_tonumber(L, 7);
                    
                    gen_to_be_invoked.HttpRequestOnPost( _url, _contentString, _finishedCallback, _keepAlive, _requestTimeout, _connectTimeout );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _contentString = LuaAPI.lua_tostring(L, 3);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 4);
                    bool _keepAlive = LuaAPI.lua_toboolean(L, 5);
                    float _requestTimeout = (float)LuaAPI.lua_tonumber(L, 6);
                    
                    gen_to_be_invoked.HttpRequestOnPost( _url, _contentString, _finishedCallback, _keepAlive, _requestTimeout );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _contentString = LuaAPI.lua_tostring(L, 3);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 4);
                    bool _keepAlive = LuaAPI.lua_toboolean(L, 5);
                    
                    gen_to_be_invoked.HttpRequestOnPost( _url, _contentString, _finishedCallback, _keepAlive );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 4)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _contentString = LuaAPI.lua_tostring(L, 3);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 4);
                    
                    gen_to_be_invoked.HttpRequestOnPost( _url, _contentString, _finishedCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _contentString = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.HttpRequestOnPost( _url, _contentString );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorNetManager.HttpRequestOnPost!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HttpRequestOnPostWithFile(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorNetManager gen_to_be_invoked = (Honor.Runtime.HonorNetManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 10&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 8)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 9)&& (LuaAPI.lua_isnil(L, 10) || LuaAPI.lua_type(L, 10) == LuaTypes.LUA_TSTRING)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _customJsonData = LuaAPI.lua_tostring(L, 3);
                    byte[] _fileBytes = LuaAPI.lua_tobytes(L, 4);
                    string _fileName = LuaAPI.lua_tostring(L, 5);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 6);
                    bool _keepAlive = LuaAPI.lua_toboolean(L, 7);
                    float _requestTimeout = (float)LuaAPI.lua_tonumber(L, 8);
                    float _connectTimeout = (float)LuaAPI.lua_tonumber(L, 9);
                    string _headerInfos = LuaAPI.lua_tostring(L, 10);
                    
                    gen_to_be_invoked.HttpRequestOnPostWithFile( _url, _customJsonData, _fileBytes, _fileName, _finishedCallback, _keepAlive, _requestTimeout, _connectTimeout, _headerInfos );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 9&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 8)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 9)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _customJsonData = LuaAPI.lua_tostring(L, 3);
                    byte[] _fileBytes = LuaAPI.lua_tobytes(L, 4);
                    string _fileName = LuaAPI.lua_tostring(L, 5);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 6);
                    bool _keepAlive = LuaAPI.lua_toboolean(L, 7);
                    float _requestTimeout = (float)LuaAPI.lua_tonumber(L, 8);
                    float _connectTimeout = (float)LuaAPI.lua_tonumber(L, 9);
                    
                    gen_to_be_invoked.HttpRequestOnPostWithFile( _url, _customJsonData, _fileBytes, _fileName, _finishedCallback, _keepAlive, _requestTimeout, _connectTimeout );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 8&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 8)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _customJsonData = LuaAPI.lua_tostring(L, 3);
                    byte[] _fileBytes = LuaAPI.lua_tobytes(L, 4);
                    string _fileName = LuaAPI.lua_tostring(L, 5);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 6);
                    bool _keepAlive = LuaAPI.lua_toboolean(L, 7);
                    float _requestTimeout = (float)LuaAPI.lua_tonumber(L, 8);
                    
                    gen_to_be_invoked.HttpRequestOnPostWithFile( _url, _customJsonData, _fileBytes, _fileName, _finishedCallback, _keepAlive, _requestTimeout );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _customJsonData = LuaAPI.lua_tostring(L, 3);
                    byte[] _fileBytes = LuaAPI.lua_tobytes(L, 4);
                    string _fileName = LuaAPI.lua_tostring(L, 5);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 6);
                    bool _keepAlive = LuaAPI.lua_toboolean(L, 7);
                    
                    gen_to_be_invoked.HttpRequestOnPostWithFile( _url, _customJsonData, _fileBytes, _fileName, _finishedCallback, _keepAlive );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)&& translator.Assignable<BestHTTP.OnRequestFinishedDelegate>(L, 6)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _customJsonData = LuaAPI.lua_tostring(L, 3);
                    byte[] _fileBytes = LuaAPI.lua_tobytes(L, 4);
                    string _fileName = LuaAPI.lua_tostring(L, 5);
                    BestHTTP.OnRequestFinishedDelegate _finishedCallback = translator.GetDelegate<BestHTTP.OnRequestFinishedDelegate>(L, 6);
                    
                    gen_to_be_invoked.HttpRequestOnPostWithFile( _url, _customJsonData, _fileBytes, _fileName, _finishedCallback );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)) 
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    string _customJsonData = LuaAPI.lua_tostring(L, 3);
                    byte[] _fileBytes = LuaAPI.lua_tobytes(L, 4);
                    string _fileName = LuaAPI.lua_tostring(L, 5);
                    
                    gen_to_be_invoked.HttpRequestOnPostWithFile( _url, _customJsonData, _fileBytes, _fileName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorNetManager.HttpRequestOnPostWithFile!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateWebSocketConnection(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorNetManager gen_to_be_invoked = (Honor.Runtime.HonorNetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _wsName = LuaAPI.lua_tostring(L, 2);
                    string _url = LuaAPI.lua_tostring(L, 3);
                    
                        BestHTTP.WebSocket.WebSocket gen_ret = gen_to_be_invoked.CreateWebSocketConnection( _wsName, _url );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetWebSocket(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorNetManager gen_to_be_invoked = (Honor.Runtime.HonorNetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _wsName = LuaAPI.lua_tostring(L, 2);
                    
                        BestHTTP.WebSocket.WebSocket gen_ret = gen_to_be_invoked.GetWebSocket( _wsName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Network(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorNetManager gen_to_be_invoked = (Honor.Runtime.HonorNetManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Network);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsNetworkReady(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorNetManager gen_to_be_invoked = (Honor.Runtime.HonorNetManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsNetworkReady);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
