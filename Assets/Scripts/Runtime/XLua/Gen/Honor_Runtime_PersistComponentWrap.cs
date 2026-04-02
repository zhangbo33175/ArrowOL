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
    public class HonorRuntimePersistComponentWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.PersistComponent);
			Utils.BeginObjectRegister(type, L, translator, 0, 17, 3, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Save", _m_Save);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAllItemNames", _m_GetAllItemNames);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasItem", _m_HasItem);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveItem", _m_RemoveItem);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveAllItems", _m_RemoveAllItems);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetBool", _m_GetBool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetBool", _m_SetBool);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetInt", _m_GetInt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetInt", _m_SetInt);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetFloat", _m_GetFloat);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetFloat", _m_SetFloat);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetString", _m_GetString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetString", _m_SetString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Count", _m_Count);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SaveByClassifyName", _m_SaveByClassifyName);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveAllItemsByClassifyName", _m_RemoveAllItemsByClassifyName);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SavePlayerPrefsDataAfterFrameEnd", _m_SavePlayerPrefsDataAfterFrameEnd);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "FileFragmentForWebGLManager", _g_get_FileFragmentForWebGLManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FileFragmentManager", _g_get_FileFragmentManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PlayerPrefsManager", _g_get_PlayerPrefsManager);
            
			
			
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
					
					Honor.Runtime.PersistComponent gen_ret = new Honor.Runtime.PersistComponent();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.PersistComponent constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Save(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    
                    gen_to_be_invoked.Save( _wayType );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.Save( _wayType, _classifyName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.PersistComponent.Save!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAllItemNames(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    
                        string[] gen_ret = gen_to_be_invoked.GetAllItemNames( _wayType, _classifyName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Collections.Generic.List<string>>(L, 4)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    System.Collections.Generic.List<string> _results = (System.Collections.Generic.List<string>)translator.GetObject(L, 4, typeof(System.Collections.Generic.List<string>));
                    
                    gen_to_be_invoked.GetAllItemNames( _wayType, _classifyName, _results );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.PersistComponent.GetAllItemNames!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasItem(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    
                        bool gen_ret = gen_to_be_invoked.HasItem( _wayType, _classifyName, _itemName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveItem(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    
                        bool gen_ret = gen_to_be_invoked.RemoveItem( _wayType, _classifyName, _itemName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveAllItems(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    
                    gen_to_be_invoked.RemoveAllItems( _wayType );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.RemoveAllItems( _wayType, _classifyName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.PersistComponent.RemoveAllItems!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    
                        bool gen_ret = gen_to_be_invoked.GetBool( _wayType, _classifyName, _itemName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    bool _defaultValue = LuaAPI.lua_toboolean(L, 5);
                    
                        bool gen_ret = gen_to_be_invoked.GetBool( _wayType, _classifyName, _itemName, _defaultValue );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.PersistComponent.GetBool!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetBool(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    bool _value = LuaAPI.lua_toboolean(L, 5);
                    
                    gen_to_be_invoked.SetBool( _wayType, _classifyName, _itemName, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    
                        int gen_ret = gen_to_be_invoked.GetInt( _wayType, _classifyName, _itemName );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    int _defaultValue = LuaAPI.xlua_tointeger(L, 5);
                    
                        int gen_ret = gen_to_be_invoked.GetInt( _wayType, _classifyName, _itemName, _defaultValue );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.PersistComponent.GetInt!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetInt(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    int _value = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.SetInt( _wayType, _classifyName, _itemName, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFloat(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    
                        float gen_ret = gen_to_be_invoked.GetFloat( _wayType, _classifyName, _itemName );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    float _defaultValue = (float)LuaAPI.lua_tonumber(L, 5);
                    
                        float gen_ret = gen_to_be_invoked.GetFloat( _wayType, _classifyName, _itemName, _defaultValue );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.PersistComponent.GetFloat!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetFloat(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    float _value = (float)LuaAPI.lua_tonumber(L, 5);
                    
                    gen_to_be_invoked.SetFloat( _wayType, _classifyName, _itemName, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    
                        string gen_ret = gen_to_be_invoked.GetString( _wayType, _classifyName, _itemName );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<Honor.Runtime.PersistWayType>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 4) || LuaAPI.lua_type(L, 4) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 5) || LuaAPI.lua_type(L, 5) == LuaTypes.LUA_TSTRING)) 
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    string _defaultValue = LuaAPI.lua_tostring(L, 5);
                    
                        string gen_ret = gen_to_be_invoked.GetString( _wayType, _classifyName, _itemName, _defaultValue );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.PersistComponent.GetString!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    string _itemName = LuaAPI.lua_tostring(L, 4);
                    string _value = LuaAPI.lua_tostring(L, 5);
                    
                    gen_to_be_invoked.SetString( _wayType, _classifyName, _itemName, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Count(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    
                        int gen_ret = gen_to_be_invoked.Count( _wayType, _classifyName );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SaveByClassifyName(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.SaveByClassifyName( _wayType, _classifyName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveAllItemsByClassifyName(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.PersistWayType _wayType;translator.Get(L, 2, out _wayType);
                    string _classifyName = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.RemoveAllItemsByClassifyName( _wayType, _classifyName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SavePlayerPrefsDataAfterFrameEnd(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.SavePlayerPrefsDataAfterFrameEnd(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FileFragmentForWebGLManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.FileFragmentForWebGLManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FileFragmentManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.FileFragmentManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PlayerPrefsManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PersistComponent gen_to_be_invoked = (Honor.Runtime.PersistComponent)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PlayerPrefsManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
