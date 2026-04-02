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
    public class HonorRuntimeAorItemPosMgrWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.AorItemPosMgr);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 2, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetItemMaxCount", _m_SetItemMaxCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetItemSize", _m_SetItemSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetItemPos", _m_GetItemPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetItemIndexAndPosAtGivenPos", _m_GetItemIndexAndPosAtGivenPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "mTotalSize", _g_get_mTotalSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mItemDefaultSize", _g_get_mItemDefaultSize);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "mTotalSize", _s_set_mTotalSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mItemDefaultSize", _s_set_mItemDefaultSize);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "mItemMaxCountPerGroup", Honor.Runtime.AorItemPosMgr.mItemMaxCountPerGroup);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 2 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2))
				{
					float _itemDefaultSize = (float)LuaAPI.lua_tonumber(L, 2);
					
					Honor.Runtime.AorItemPosMgr gen_ret = new Honor.Runtime.AorItemPosMgr(_itemDefaultSize);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.AorItemPosMgr constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetItemMaxCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AorItemPosMgr gen_to_be_invoked = (Honor.Runtime.AorItemPosMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _maxCount = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.SetItemMaxCount( _maxCount );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetItemSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AorItemPosMgr gen_to_be_invoked = (Honor.Runtime.AorItemPosMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _itemIndex = LuaAPI.xlua_tointeger(L, 2);
                    float _size = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    gen_to_be_invoked.SetItemSize( _itemIndex, _size );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetItemPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AorItemPosMgr gen_to_be_invoked = (Honor.Runtime.AorItemPosMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _itemIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                        float gen_ret = gen_to_be_invoked.GetItemPos( _itemIndex );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetItemIndexAndPosAtGivenPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AorItemPosMgr gen_to_be_invoked = (Honor.Runtime.AorItemPosMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _pos = (float)LuaAPI.lua_tonumber(L, 2);
                    int _index = LuaAPI.xlua_tointeger(L, 3);
                    float _itemPos = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        bool gen_ret = gen_to_be_invoked.GetItemIndexAndPosAtGivenPos( _pos, ref _index, ref _itemPos );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    LuaAPI.xlua_pushinteger(L, _index);
                        
                    LuaAPI.lua_pushnumber(L, _itemPos);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.AorItemPosMgr gen_to_be_invoked = (Honor.Runtime.AorItemPosMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool _updateAll = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.Update( _updateAll );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mTotalSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AorItemPosMgr gen_to_be_invoked = (Honor.Runtime.AorItemPosMgr)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mTotalSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mItemDefaultSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AorItemPosMgr gen_to_be_invoked = (Honor.Runtime.AorItemPosMgr)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mItemDefaultSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mTotalSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AorItemPosMgr gen_to_be_invoked = (Honor.Runtime.AorItemPosMgr)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mTotalSize = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mItemDefaultSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.AorItemPosMgr gen_to_be_invoked = (Honor.Runtime.AorItemPosMgr)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mItemDefaultSize = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
