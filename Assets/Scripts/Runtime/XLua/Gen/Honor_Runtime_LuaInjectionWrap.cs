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
    public class HonorRuntimeLuaInjectionWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.LuaInjection);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 14, 14);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetExtendsCount", _m_GetExtendsCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetExtendsInfo", _m_GetExtendsInfo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetExtendsCountAtElementIndex", _m_GetExtendsCountAtElementIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetExtendsInfoAtElementIndex", _m_GetExtendsInfoAtElementIndex);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Comment", _g_get_Comment);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InjectionTypeName", _g_get_InjectionTypeName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Name", _g_get_Name);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsArray", _g_get_IsArray);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Obj", _g_get_Obj);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Variant", _g_get_Variant);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InfoEx", _g_get_InfoEx);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ExtendsEnabled", _g_get_ExtendsEnabled);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Extends", _g_get_Extends);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ElementsObjs", _g_get_ElementsObjs);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ElementsVariants", _g_get_ElementsVariants);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ElementsInfoExs", _g_get_ElementsInfoExs);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ElementsExtendsEnableds", _g_get_ElementsExtendsEnableds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ElementsExtends", _g_get_ElementsExtends);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Comment", _s_set_Comment);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "InjectionTypeName", _s_set_InjectionTypeName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Name", _s_set_Name);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsArray", _s_set_IsArray);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Obj", _s_set_Obj);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Variant", _s_set_Variant);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "InfoEx", _s_set_InfoEx);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ExtendsEnabled", _s_set_ExtendsEnabled);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Extends", _s_set_Extends);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ElementsObjs", _s_set_ElementsObjs);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ElementsVariants", _s_set_ElementsVariants);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ElementsInfoExs", _s_set_ElementsInfoExs);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ElementsExtendsEnableds", _s_set_ElementsExtendsEnableds);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ElementsExtends", _s_set_ElementsExtends);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 4, 4);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "LuaInjectionType", _g_get_LuaInjectionType);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "DisplayInjectionTypeString", _g_get_DisplayInjectionTypeString);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "InjectionTypeDisplayToRealMapping", _g_get_InjectionTypeDisplayToRealMapping);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "InjectionTypeRealToDisplayMapping", _g_get_InjectionTypeRealToDisplayMapping);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "LuaInjectionType", _s_set_LuaInjectionType);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "DisplayInjectionTypeString", _s_set_DisplayInjectionTypeString);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "InjectionTypeDisplayToRealMapping", _s_set_InjectionTypeDisplayToRealMapping);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "InjectionTypeRealToDisplayMapping", _s_set_InjectionTypeRealToDisplayMapping);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Honor.Runtime.LuaInjection gen_ret = new Honor.Runtime.LuaInjection();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.LuaInjection constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetExtendsCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        int gen_ret = gen_to_be_invoked.GetExtendsCount(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetExtendsInfo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        string gen_ret = gen_to_be_invoked.GetExtendsInfo( _index );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetExtendsCountAtElementIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _elementIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        int gen_ret = gen_to_be_invoked.GetExtendsCountAtElementIndex( _elementIndex );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetExtendsInfoAtElementIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _elementIndex = LuaAPI.xlua_tointeger(L, 2);
                    int _index = LuaAPI.xlua_tointeger(L, 3);
                    
                        string gen_ret = gen_to_be_invoked.GetExtendsInfoAtElementIndex( _elementIndex, _index );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaInjectionType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Honor.Runtime.LuaInjection.LuaInjectionType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DisplayInjectionTypeString(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Honor.Runtime.LuaInjection.DisplayInjectionTypeString);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InjectionTypeDisplayToRealMapping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Honor.Runtime.LuaInjection.InjectionTypeDisplayToRealMapping);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InjectionTypeRealToDisplayMapping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Honor.Runtime.LuaInjection.InjectionTypeRealToDisplayMapping);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Comment(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Comment);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InjectionTypeName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                translator.PushHonorRuntimeLuaInjectionInjectionType(L, gen_to_be_invoked.InjectionTypeName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Name(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Name);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsArray(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsArray);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Obj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Obj);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Variant(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Variant);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InfoEx(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.InfoEx);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ExtendsEnabled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.ExtendsEnabled);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Extends(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.Extends);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ElementsObjs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ElementsObjs);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ElementsVariants(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ElementsVariants);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ElementsInfoExs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ElementsInfoExs);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ElementsExtendsEnableds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ElementsExtendsEnableds);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ElementsExtends(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ElementsExtends);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LuaInjectionType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Honor.Runtime.LuaInjection.LuaInjectionType = (string[])translator.GetObject(L, 1, typeof(string[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DisplayInjectionTypeString(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Honor.Runtime.LuaInjection.DisplayInjectionTypeString = (string[])translator.GetObject(L, 1, typeof(string[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_InjectionTypeDisplayToRealMapping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Honor.Runtime.LuaInjection.InjectionTypeDisplayToRealMapping = (System.Collections.Generic.Dictionary<int, int>)translator.GetObject(L, 1, typeof(System.Collections.Generic.Dictionary<int, int>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_InjectionTypeRealToDisplayMapping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Honor.Runtime.LuaInjection.InjectionTypeRealToDisplayMapping = (System.Collections.Generic.Dictionary<int, int>)translator.GetObject(L, 1, typeof(System.Collections.Generic.Dictionary<int, int>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Comment(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Comment = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_InjectionTypeName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                Honor.Runtime.LuaInjection.InjectionType gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.InjectionTypeName = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Name(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Name = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsArray(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsArray = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Obj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Obj = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Variant(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Variant = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_InfoEx(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.InfoEx = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ExtendsEnabled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ExtendsEnabled = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Extends(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Extends = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ElementsObjs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ElementsObjs = (System.Collections.Generic.List<UnityEngine.Object>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Object>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ElementsVariants(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ElementsVariants = (System.Collections.Generic.List<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<string>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ElementsInfoExs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ElementsInfoExs = (System.Collections.Generic.List<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<string>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ElementsExtendsEnableds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ElementsExtendsEnableds = (System.Collections.Generic.List<bool>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<bool>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ElementsExtends(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.LuaInjection gen_to_be_invoked = (Honor.Runtime.LuaInjection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ElementsExtends = (System.Collections.Generic.List<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<string>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
