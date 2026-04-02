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
    public class HonorRuntimePlaySoundInfoWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.PlaySoundInfo);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 4, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "SerialID", _g_get_SerialID);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SoundGroup", _g_get_SoundGroup);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PlaySoundParams", _g_get_PlaySoundParams);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PlaySoundInfoShell", _g_get_PlaySoundInfoShell);
            
			
			
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
					
					Honor.Runtime.PlaySoundInfo gen_ret = new Honor.Runtime.PlaySoundInfo();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.PlaySoundInfo constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Create_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _serialID = LuaAPI.xlua_tointeger(L, 1);
                    Honor.Runtime.SoundGroup _soundGroup = (Honor.Runtime.SoundGroup)translator.GetObject(L, 2, typeof(Honor.Runtime.SoundGroup));
                    Honor.Runtime.PlaySoundParams _playSoundParams = (Honor.Runtime.PlaySoundParams)translator.GetObject(L, 3, typeof(Honor.Runtime.PlaySoundParams));
                    Honor.Runtime.PlaySoundInfoShell _playSoundInfoShell = (Honor.Runtime.PlaySoundInfoShell)translator.GetObject(L, 4, typeof(Honor.Runtime.PlaySoundInfoShell));
                    
                        Honor.Runtime.PlaySoundInfo gen_ret = Honor.Runtime.PlaySoundInfo.Create( _serialID, _soundGroup, _playSoundParams, _playSoundInfoShell );
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
            
            
                Honor.Runtime.PlaySoundInfo gen_to_be_invoked = (Honor.Runtime.PlaySoundInfo)translator.FastGetCSObj(L, 1);
            
            
                
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
			
                Honor.Runtime.PlaySoundInfo gen_to_be_invoked = (Honor.Runtime.PlaySoundInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SerialID);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SoundGroup(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PlaySoundInfo gen_to_be_invoked = (Honor.Runtime.PlaySoundInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SoundGroup);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PlaySoundParams(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PlaySoundInfo gen_to_be_invoked = (Honor.Runtime.PlaySoundInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PlaySoundParams);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PlaySoundInfoShell(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.PlaySoundInfo gen_to_be_invoked = (Honor.Runtime.PlaySoundInfo)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.PlaySoundInfoShell);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
