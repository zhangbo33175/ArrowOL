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
    public class HonorRuntimeLocalizationManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.LocalizationManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 13, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadDefaultLanguages", _m_LoadDefaultLanguages);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasDefaultLanguage", _m_HasDefaultLanguage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveAllDefaultLanguages", _m_RemoveAllDefaultLanguages);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadDefaultDatas", _m_LoadDefaultDatas);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasDefaultData", _m_HasDefaultData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddDefaultData", _m_AddDefaultData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveDefaultData", _m_RemoveDefaultData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveAllDefaultDatas", _m_RemoveAllDefaultDatas);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetDefaultData", _m_GetDefaultData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadFontDatas", _m_LoadFontDatas);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddFontData", _m_AddFontData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveAllFontDatas", _m_RemoveAllFontDatas);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetFontDatas", _m_GetFontDatas);
			
			
			
			
			
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
					
					Honor.Runtime.LocalizationManager gen_ret = new Honor.Runtime.LocalizationManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.LocalizationManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadDefaultLanguages(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LocalizationManager gen_to_be_invoked = (Honor.Runtime.LocalizationManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LoadDefaultLanguages(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasDefaultLanguage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LocalizationManager gen_to_be_invoked = (Honor.Runtime.LocalizationManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameDefinitions.Language _language;translator.Get(L, 2, out _language);
                    
                        bool gen_ret = gen_to_be_invoked.HasDefaultLanguage( _language );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveAllDefaultLanguages(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LocalizationManager gen_to_be_invoked = (Honor.Runtime.LocalizationManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.RemoveAllDefaultLanguages(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadDefaultDatas(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LocalizationManager gen_to_be_invoked = (Honor.Runtime.LocalizationManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameDefinitions.Language _language;translator.Get(L, 2, out _language);
                    string _abPath = LuaAPI.lua_tostring(L, 3);
                    string _assetName = LuaAPI.lua_tostring(L, 4);
                    
                    gen_to_be_invoked.LoadDefaultDatas( _language, _abPath, _assetName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasDefaultData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LocalizationManager gen_to_be_invoked = (Honor.Runtime.LocalizationManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameDefinitions.Language _language;translator.Get(L, 2, out _language);
                    string _keyName = LuaAPI.lua_tostring(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.HasDefaultData( _language, _keyName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddDefaultData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LocalizationManager gen_to_be_invoked = (Honor.Runtime.LocalizationManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameDefinitions.Language _language;translator.Get(L, 2, out _language);
                    string _keyName = LuaAPI.lua_tostring(L, 3);
                    string _content = LuaAPI.lua_tostring(L, 4);
                    
                    gen_to_be_invoked.AddDefaultData( _language, _keyName, _content );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveDefaultData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LocalizationManager gen_to_be_invoked = (Honor.Runtime.LocalizationManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameDefinitions.Language _language;translator.Get(L, 2, out _language);
                    string _keyName = LuaAPI.lua_tostring(L, 3);
                    
                        bool gen_ret = gen_to_be_invoked.RemoveDefaultData( _language, _keyName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveAllDefaultDatas(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LocalizationManager gen_to_be_invoked = (Honor.Runtime.LocalizationManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1) 
                {
                    
                    gen_to_be_invoked.RemoveAllDefaultDatas(  );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.GameDefinitions.Language>(L, 2)) 
                {
                    Honor.Runtime.GameDefinitions.Language _language;translator.Get(L, 2, out _language);
                    
                    gen_to_be_invoked.RemoveAllDefaultDatas( _language );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.LocalizationManager.RemoveAllDefaultDatas!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDefaultData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LocalizationManager gen_to_be_invoked = (Honor.Runtime.LocalizationManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameDefinitions.Language _language;translator.Get(L, 2, out _language);
                    string _keyName = LuaAPI.lua_tostring(L, 3);
                    
                        string gen_ret = gen_to_be_invoked.GetDefaultData( _language, _keyName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadFontDatas(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LocalizationManager gen_to_be_invoked = (Honor.Runtime.LocalizationManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _abPath = LuaAPI.lua_tostring(L, 2);
                    string _assetName = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.LoadFontDatas( _abPath, _assetName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddFontData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LocalizationManager gen_to_be_invoked = (Honor.Runtime.LocalizationManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameDefinitions.Language _language;translator.Get(L, 2, out _language);
                    Honor.Runtime.LocalizationFontData _fontData = (Honor.Runtime.LocalizationFontData)translator.GetObject(L, 3, typeof(Honor.Runtime.LocalizationFontData));
                    
                    gen_to_be_invoked.AddFontData( _language, _fontData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveAllFontDatas(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LocalizationManager gen_to_be_invoked = (Honor.Runtime.LocalizationManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.RemoveAllFontDatas(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFontDatas(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LocalizationManager gen_to_be_invoked = (Honor.Runtime.LocalizationManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.GameDefinitions.Language _language;translator.Get(L, 2, out _language);
                    System.Collections.Generic.List<Honor.Runtime.LocalizationFontData> _fontDatas;
                    
                    gen_to_be_invoked.GetFontDatas( _language, out _fontDatas );
                    translator.Push(L, _fontDatas);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
