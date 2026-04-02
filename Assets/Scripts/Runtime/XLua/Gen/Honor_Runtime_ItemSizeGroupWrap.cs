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
    public class HonorRuntimeItemSizeGroupWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.ItemSizeGroup);
			Utils.BeginObjectRegister(type, L, translator, 0, 8, 8, 7);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetItemStartPos", _m_GetItemStartPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetItemSize", _m_SetItemSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetItemCount", _m_SetItemCount);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RecalcGroupSize", _m_RecalcGroupSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetItemIndexByPos", _m_GetItemIndexByPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdateAllItemStartPos", _m_UpdateAllItemStartPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearOldData", _m_ClearOldData);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsDirty", _g_get_IsDirty);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mItemSizeArray", _g_get_mItemSizeArray);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mItemStartPosArray", _g_get_mItemStartPosArray);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mItemCount", _g_get_mItemCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mGroupSize", _g_get_mGroupSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mGroupStartPos", _g_get_mGroupStartPos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mGroupEndPos", _g_get_mGroupEndPos);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mGroupIndex", _g_get_mGroupIndex);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "mItemSizeArray", _s_set_mItemSizeArray);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mItemStartPosArray", _s_set_mItemStartPosArray);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mItemCount", _s_set_mItemCount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mGroupSize", _s_set_mGroupSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mGroupStartPos", _s_set_mGroupStartPos);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mGroupEndPos", _s_set_mGroupEndPos);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mGroupIndex", _s_set_mGroupIndex);
            
			
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
				if(LuaAPI.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3))
				{
					int _index = LuaAPI.xlua_tointeger(L, 2);
					float _itemDefaultSize = (float)LuaAPI.lua_tonumber(L, 3);
					
					Honor.Runtime.ItemSizeGroup gen_ret = new Honor.Runtime.ItemSizeGroup(_index, _itemDefaultSize);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.ItemSizeGroup constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetItemStartPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        float gen_ret = gen_to_be_invoked.GetItemStartPos( _index );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
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
            
            
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    float _size = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        float gen_ret = gen_to_be_invoked.SetItemSize( _index, _size );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetItemCount(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _count = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.SetItemCount( _count );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RecalcGroupSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.RecalcGroupSize(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetItemIndexByPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _pos = (float)LuaAPI.lua_tonumber(L, 2);
                    
                        int gen_ret = gen_to_be_invoked.GetItemIndexByPos( _pos );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateAllItemStartPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.UpdateAllItemStartPos(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearOldData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearOldData(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsDirty(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsDirty);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mItemSizeArray(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.mItemSizeArray);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mItemStartPosArray(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.mItemStartPosArray);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mItemCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.mItemCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mGroupSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mGroupSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mGroupStartPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mGroupStartPos);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mGroupEndPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mGroupEndPos);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mGroupIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.mGroupIndex);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mItemSizeArray(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mItemSizeArray = (float[])translator.GetObject(L, 2, typeof(float[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mItemStartPosArray(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mItemStartPosArray = (float[])translator.GetObject(L, 2, typeof(float[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mItemCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mItemCount = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mGroupSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mGroupSize = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mGroupStartPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mGroupStartPos = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mGroupEndPos(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mGroupEndPos = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mGroupIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ItemSizeGroup gen_to_be_invoked = (Honor.Runtime.ItemSizeGroup)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mGroupIndex = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
