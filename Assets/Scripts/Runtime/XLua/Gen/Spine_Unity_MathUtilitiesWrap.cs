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
    public class SpineUnityMathUtilitiesWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Spine.Unity.MathUtilities);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "InverseLerp", _m_InverseLerp_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Spine.Unity.MathUtilities does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InverseLerp_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    float _a = (float)LuaAPI.lua_tonumber(L, 1);
                    float _b = (float)LuaAPI.lua_tonumber(L, 2);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        float gen_ret = Spine.Unity.MathUtilities.InverseLerp( _a, _b, _value );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector2>(L, 1)&& translator.Assignable<UnityEngine.Vector2>(L, 2)&& translator.Assignable<UnityEngine.Vector2>(L, 3)) 
                {
                    UnityEngine.Vector2 _a;translator.Get(L, 1, out _a);
                    UnityEngine.Vector2 _b;translator.Get(L, 2, out _b);
                    UnityEngine.Vector2 _value;translator.Get(L, 3, out _value);
                    
                        UnityEngine.Vector2 gen_ret = Spine.Unity.MathUtilities.InverseLerp( _a, _b, _value );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<UnityEngine.Vector3>(L, 3)) 
                {
                    UnityEngine.Vector3 _a;translator.Get(L, 1, out _a);
                    UnityEngine.Vector3 _b;translator.Get(L, 2, out _b);
                    UnityEngine.Vector3 _value;translator.Get(L, 3, out _value);
                    
                        UnityEngine.Vector3 gen_ret = Spine.Unity.MathUtilities.InverseLerp( _a, _b, _value );
                        translator.PushUnityEngineVector3(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Vector4>(L, 1)&& translator.Assignable<UnityEngine.Vector4>(L, 2)&& translator.Assignable<UnityEngine.Vector4>(L, 3)) 
                {
                    UnityEngine.Vector4 _a;translator.Get(L, 1, out _a);
                    UnityEngine.Vector4 _b;translator.Get(L, 2, out _b);
                    UnityEngine.Vector4 _value;translator.Get(L, 3, out _value);
                    
                        UnityEngine.Vector4 gen_ret = Spine.Unity.MathUtilities.InverseLerp( _a, _b, _value );
                        translator.PushUnityEngineVector4(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.MathUtilities.InverseLerp!");
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
