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
    public class HonorRuntimeListViewInitParamWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.ListViewInitParam);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 8, 8);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "mDistanceForRecycle0", _g_get_mDistanceForRecycle0);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mDistanceForNew0", _g_get_mDistanceForNew0);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mDistanceForRecycle1", _g_get_mDistanceForRecycle1);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mDistanceForNew1", _g_get_mDistanceForNew1);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mSmoothDumpRate", _g_get_mSmoothDumpRate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mSnapFinishThreshold", _g_get_mSnapFinishThreshold);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mSnapVecThreshold", _g_get_mSnapVecThreshold);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mItemDefaultWithPaddingSize", _g_get_mItemDefaultWithPaddingSize);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "mDistanceForRecycle0", _s_set_mDistanceForRecycle0);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mDistanceForNew0", _s_set_mDistanceForNew0);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mDistanceForRecycle1", _s_set_mDistanceForRecycle1);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mDistanceForNew1", _s_set_mDistanceForNew1);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mSmoothDumpRate", _s_set_mSmoothDumpRate);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mSnapFinishThreshold", _s_set_mSnapFinishThreshold);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mSnapVecThreshold", _s_set_mSnapVecThreshold);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mItemDefaultWithPaddingSize", _s_set_mItemDefaultWithPaddingSize);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "CopyDefaultInitParam", _m_CopyDefaultInitParam_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Honor.Runtime.ListViewInitParam gen_ret = new Honor.Runtime.ListViewInitParam();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.ListViewInitParam constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CopyDefaultInitParam_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        Honor.Runtime.ListViewInitParam gen_ret = Honor.Runtime.ListViewInitParam.CopyDefaultInitParam(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mDistanceForRecycle0(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mDistanceForRecycle0);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mDistanceForNew0(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mDistanceForNew0);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mDistanceForRecycle1(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mDistanceForRecycle1);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mDistanceForNew1(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mDistanceForNew1);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mSmoothDumpRate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mSmoothDumpRate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mSnapFinishThreshold(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mSnapFinishThreshold);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mSnapVecThreshold(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mSnapVecThreshold);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mItemDefaultWithPaddingSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.mItemDefaultWithPaddingSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mDistanceForRecycle0(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mDistanceForRecycle0 = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mDistanceForNew0(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mDistanceForNew0 = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mDistanceForRecycle1(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mDistanceForRecycle1 = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mDistanceForNew1(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mDistanceForNew1 = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mSmoothDumpRate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mSmoothDumpRate = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mSnapFinishThreshold(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mSnapFinishThreshold = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mSnapVecThreshold(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mSnapVecThreshold = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mItemDefaultWithPaddingSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.ListViewInitParam gen_to_be_invoked = (Honor.Runtime.ListViewInitParam)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.mItemDefaultWithPaddingSize = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
