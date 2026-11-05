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
    public class HonorRuntimeHonorPlaySoundInfoWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.HonorPlaySoundInfo);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 4, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "SerialID", _g_get_SerialID);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "HonorSoundCategory", _g_get_HonorSoundCategory);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "HonorPlaySoundParams", _g_get_HonorPlaySoundParams);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "HonorPlaySoundHandle", _g_get_HonorPlaySoundHandle);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Create", _m_Create_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Honor.Runtime.HonorPlaySoundInfo gen_ret = new Honor.Runtime.HonorPlaySoundInfo();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.HonorPlaySoundInfo constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Create_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _serialID = LuaAPI.xlua_tointeger(L, 1);
                    Honor.Runtime.HonorSoundCategory _HonorSoundCategory = (Honor.Runtime.HonorSoundCategory)translator.GetObject(L, 2, typeof(Honor.Runtime.HonorSoundCategory));
                    Honor.Runtime.HonorPlaySoundParams _HonorPlaySoundParams = (Honor.Runtime.HonorPlaySoundParams)translator.GetObject(L, 3, typeof(Honor.Runtime.HonorPlaySoundParams));
                    Honor.Runtime.HonorPlaySoundHandle _HonorPlaySoundHandle = (Honor.Runtime.HonorPlaySoundHandle)translator.GetObject(L, 4, typeof(Honor.Runtime.HonorPlaySoundHandle));
                    
                        Honor.Runtime.HonorPlaySoundInfo gen_ret = Honor.Runtime.HonorPlaySoundInfo.Create( _serialID, _HonorSoundCategory, _HonorPlaySoundParams, _HonorPlaySoundHandle );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.HonorPlaySoundInfo gen_to_be_invoked = (Honor.Runtime.HonorPlaySoundInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SerialID(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorPlaySoundInfo gen_to_be_invoked = (Honor.Runtime.HonorPlaySoundInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SerialID);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HonorSoundCategory(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorPlaySoundInfo gen_to_be_invoked = (Honor.Runtime.HonorPlaySoundInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.HonorSoundCategory);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HonorPlaySoundParams(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorPlaySoundInfo gen_to_be_invoked = (Honor.Runtime.HonorPlaySoundInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.HonorPlaySoundParams);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HonorPlaySoundHandle(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.HonorPlaySoundInfo gen_to_be_invoked = (Honor.Runtime.HonorPlaySoundInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.HonorPlaySoundHandle);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
