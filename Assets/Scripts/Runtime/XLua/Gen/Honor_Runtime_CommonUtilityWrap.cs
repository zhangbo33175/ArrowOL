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
    public class HonorRuntimeCommonUtilityWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.CommonUtility);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 10, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetItemNamesByGroup", _m_GetItemNamesByGroup_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AppendItemNamesByGroup", _m_AppendItemNamesByGroup_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CountItemsByGroup", _m_CountItemsByGroup_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "HasPlayerPrefsItem", _m_HasPlayerPrefsItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegisterItemIntoGroups", _m_RegisterItemIntoGroups_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RemoveItemFromGroups", _m_RemoveItemFromGroups_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadItemNameGroups", _m_LoadItemNameGroups_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SaveClassifyNameList", _m_SaveClassifyNameList_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SaveItemNameList", _m_SaveItemNameList_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.CommonUtility does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetItemNamesByGroup_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>> _groups = (System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>)translator.GetObject(L, 1, typeof(System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>));
                    string _classifyName = LuaAPI.lua_tostring(L, 2);
                    
                        string[] gen_ret = Honor.Runtime.CommonUtility.GetItemNamesByGroup( _groups, _classifyName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AppendItemNamesByGroup_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>> _groups = (System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>)translator.GetObject(L, 1, typeof(System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>));
                    string _classifyName = LuaAPI.lua_tostring(L, 2);
                    System.Collections.Generic.List<string> _results = (System.Collections.Generic.List<string>)translator.GetObject(L, 3, typeof(System.Collections.Generic.List<string>));
                    
                    Honor.Runtime.CommonUtility.AppendItemNamesByGroup( _groups, _classifyName, _results );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CountItemsByGroup_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>> _groups = (System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>)translator.GetObject(L, 1, typeof(System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>));
                    string _classifyName = LuaAPI.lua_tostring(L, 2);
                    
                        int gen_ret = Honor.Runtime.CommonUtility.CountItemsByGroup( _groups, _classifyName );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasPlayerPrefsItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _classifyName = LuaAPI.lua_tostring(L, 1);
                    string _itemName = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = Honor.Runtime.CommonUtility.HasPlayerPrefsItem( _classifyName, _itemName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterItemIntoGroups_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>> _groups = (System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>)translator.GetObject(L, 1, typeof(System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>));
                    string _classifyName = LuaAPI.lua_tostring(L, 2);
                    string _itemName = LuaAPI.lua_tostring(L, 3);
                    
                        bool gen_ret = Honor.Runtime.CommonUtility.RegisterItemIntoGroups( _groups, _classifyName, _itemName );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveItemFromGroups_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>> _groups = (System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>)translator.GetObject(L, 1, typeof(System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>));
                    string _classifyName = LuaAPI.lua_tostring(L, 2);
                    string _itemName = LuaAPI.lua_tostring(L, 3);
                    
                    Honor.Runtime.CommonUtility.RemoveItemFromGroups( _groups, _classifyName, _itemName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadItemNameGroups_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>> _groups = (System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>)translator.GetObject(L, 1, typeof(System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>));
                    string _classifyListKey = LuaAPI.lua_tostring(L, 2);
                    string _itemListKeyFormat = LuaAPI.lua_tostring(L, 3);
                    
                    Honor.Runtime.CommonUtility.LoadItemNameGroups( _groups, _classifyListKey, _itemListKeyFormat );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SaveClassifyNameList_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>> _groups = (System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>)translator.GetObject(L, 1, typeof(System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>));
                    string _classifyListKey = LuaAPI.lua_tostring(L, 2);
                    
                    Honor.Runtime.CommonUtility.SaveClassifyNameList( _groups, _classifyListKey );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SaveItemNameList_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>> _groups = (System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>)translator.GetObject(L, 1, typeof(System.Collections.Generic.SortedDictionary<string, System.Collections.Generic.List<string>>));
                    string _classifyNameForSetting = LuaAPI.lua_tostring(L, 2);
                    string _itemListKeyFormat = LuaAPI.lua_tostring(L, 3);
                    
                    Honor.Runtime.CommonUtility.SaveItemNameList( _groups, _classifyNameForSetting, _itemListKeyFormat );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
