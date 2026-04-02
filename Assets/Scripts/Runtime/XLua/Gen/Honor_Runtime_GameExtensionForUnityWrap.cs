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
    public class HonorRuntimeGameExtensionForUnityWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.GameExtensionForUnity);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 10, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "AddAnimatorParameterIfExists", _m_AddAnimatorParameterIfExists_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateAnimatorBool", _m_UpdateAnimatorBool_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateAnimatorInteger", _m_UpdateAnimatorInteger_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateAnimatorFloat", _m_UpdateAnimatorFloat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateAnimatorTrigger", _m_UpdateAnimatorTrigger_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateAnimatorBoolIfExists", _m_UpdateAnimatorBoolIfExists_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateAnimatorTriggerIfExists", _m_UpdateAnimatorTriggerIfExists_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateAnimatorFloatIfExists", _m_UpdateAnimatorFloatIfExists_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateAnimatorIntegerIfExists", _m_UpdateAnimatorIntegerIfExists_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.GameExtensionForUnity does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddAnimatorParameterIfExists_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.AnimatorControllerParameterType>(L, 3)&& translator.Assignable<System.Collections.Generic.HashSet<string>>(L, 4)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.AnimatorControllerParameterType _type;translator.Get(L, 3, out _type);
                    System.Collections.Generic.HashSet<string> _parameterList = (System.Collections.Generic.HashSet<string>)translator.GetObject(L, 4, typeof(System.Collections.Generic.HashSet<string>));
                    
                    Honor.Runtime.GameExtensionForUnity.AddAnimatorParameterIfExists( _animator, _parameterName, _type, _parameterList );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.AnimatorControllerParameterType>(L, 3)&& translator.Assignable<System.Collections.Generic.HashSet<int>>(L, 4)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    int _parameter;
                    UnityEngine.AnimatorControllerParameterType _type;translator.Get(L, 3, out _type);
                    System.Collections.Generic.HashSet<int> _parameterList = (System.Collections.Generic.HashSet<int>)translator.GetObject(L, 4, typeof(System.Collections.Generic.HashSet<int>));
                    
                    Honor.Runtime.GameExtensionForUnity.AddAnimatorParameterIfExists( _animator, _parameterName, out _parameter, _type, _parameterList );
                    LuaAPI.xlua_pushinteger(L, _parameter);
                        
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.GameExtensionForUnity.AddAnimatorParameterIfExists!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateAnimatorBool_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    bool _value = LuaAPI.lua_toboolean(L, 3);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorBool( _animator, _parameterName, _value );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Animator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.HashSet<int>>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    int _parameter = LuaAPI.xlua_tointeger(L, 2);
                    bool _value = LuaAPI.lua_toboolean(L, 3);
                    System.Collections.Generic.HashSet<int> _parameterList = (System.Collections.Generic.HashSet<int>)translator.GetObject(L, 4, typeof(System.Collections.Generic.HashSet<int>));
                    bool _performSanityCheck = LuaAPI.lua_toboolean(L, 5);
                    
                        bool gen_ret = Honor.Runtime.GameExtensionForUnity.UpdateAnimatorBool( _animator, _parameter, _value, _parameterList, _performSanityCheck );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Animator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.HashSet<int>>(L, 4)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    int _parameter = LuaAPI.xlua_tointeger(L, 2);
                    bool _value = LuaAPI.lua_toboolean(L, 3);
                    System.Collections.Generic.HashSet<int> _parameterList = (System.Collections.Generic.HashSet<int>)translator.GetObject(L, 4, typeof(System.Collections.Generic.HashSet<int>));
                    
                        bool gen_ret = Honor.Runtime.GameExtensionForUnity.UpdateAnimatorBool( _animator, _parameter, _value, _parameterList );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.HashSet<string>>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    bool _value = LuaAPI.lua_toboolean(L, 3);
                    System.Collections.Generic.HashSet<string> _parameterList = (System.Collections.Generic.HashSet<string>)translator.GetObject(L, 4, typeof(System.Collections.Generic.HashSet<string>));
                    bool _performSanityCheck = LuaAPI.lua_toboolean(L, 5);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorBool( _animator, _parameterName, _value, _parameterList, _performSanityCheck );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.HashSet<string>>(L, 4)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    bool _value = LuaAPI.lua_toboolean(L, 3);
                    System.Collections.Generic.HashSet<string> _parameterList = (System.Collections.Generic.HashSet<string>)translator.GetObject(L, 4, typeof(System.Collections.Generic.HashSet<string>));
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorBool( _animator, _parameterName, _value, _parameterList );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.GameExtensionForUnity.UpdateAnimatorBool!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateAnimatorInteger_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    int _value = LuaAPI.xlua_tointeger(L, 3);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorInteger( _animator, _parameterName, _value );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Animator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.HashSet<int>>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    int _parameter = LuaAPI.xlua_tointeger(L, 2);
                    int _value = LuaAPI.xlua_tointeger(L, 3);
                    System.Collections.Generic.HashSet<int> _parameterList = (System.Collections.Generic.HashSet<int>)translator.GetObject(L, 4, typeof(System.Collections.Generic.HashSet<int>));
                    bool _performSanityCheck = LuaAPI.lua_toboolean(L, 5);
                    
                        bool gen_ret = Honor.Runtime.GameExtensionForUnity.UpdateAnimatorInteger( _animator, _parameter, _value, _parameterList, _performSanityCheck );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Animator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.HashSet<int>>(L, 4)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    int _parameter = LuaAPI.xlua_tointeger(L, 2);
                    int _value = LuaAPI.xlua_tointeger(L, 3);
                    System.Collections.Generic.HashSet<int> _parameterList = (System.Collections.Generic.HashSet<int>)translator.GetObject(L, 4, typeof(System.Collections.Generic.HashSet<int>));
                    
                        bool gen_ret = Honor.Runtime.GameExtensionForUnity.UpdateAnimatorInteger( _animator, _parameter, _value, _parameterList );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.HashSet<string>>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    int _value = LuaAPI.xlua_tointeger(L, 3);
                    System.Collections.Generic.HashSet<string> _parameterList = (System.Collections.Generic.HashSet<string>)translator.GetObject(L, 4, typeof(System.Collections.Generic.HashSet<string>));
                    bool _performSanityCheck = LuaAPI.lua_toboolean(L, 5);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorInteger( _animator, _parameterName, _value, _parameterList, _performSanityCheck );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.HashSet<string>>(L, 4)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    int _value = LuaAPI.xlua_tointeger(L, 3);
                    System.Collections.Generic.HashSet<string> _parameterList = (System.Collections.Generic.HashSet<string>)translator.GetObject(L, 4, typeof(System.Collections.Generic.HashSet<string>));
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorInteger( _animator, _parameterName, _value, _parameterList );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.GameExtensionForUnity.UpdateAnimatorInteger!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateAnimatorFloat_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _performSanityCheck = LuaAPI.lua_toboolean(L, 4);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorFloat( _animator, _parameterName, _value, _performSanityCheck );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorFloat( _animator, _parameterName, _value );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Animator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.HashSet<int>>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    int _parameter = LuaAPI.xlua_tointeger(L, 2);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Collections.Generic.HashSet<int> _parameterList = (System.Collections.Generic.HashSet<int>)translator.GetObject(L, 4, typeof(System.Collections.Generic.HashSet<int>));
                    bool _performSanityCheck = LuaAPI.lua_toboolean(L, 5);
                    
                        bool gen_ret = Honor.Runtime.GameExtensionForUnity.UpdateAnimatorFloat( _animator, _parameter, _value, _parameterList, _performSanityCheck );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Animator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.HashSet<int>>(L, 4)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    int _parameter = LuaAPI.xlua_tointeger(L, 2);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Collections.Generic.HashSet<int> _parameterList = (System.Collections.Generic.HashSet<int>)translator.GetObject(L, 4, typeof(System.Collections.Generic.HashSet<int>));
                    
                        bool gen_ret = Honor.Runtime.GameExtensionForUnity.UpdateAnimatorFloat( _animator, _parameter, _value, _parameterList );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.HashSet<string>>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Collections.Generic.HashSet<string> _parameterList = (System.Collections.Generic.HashSet<string>)translator.GetObject(L, 4, typeof(System.Collections.Generic.HashSet<string>));
                    bool _performSanityCheck = LuaAPI.lua_toboolean(L, 5);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorFloat( _animator, _parameterName, _value, _parameterList, _performSanityCheck );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Collections.Generic.HashSet<string>>(L, 4)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Collections.Generic.HashSet<string> _parameterList = (System.Collections.Generic.HashSet<string>)translator.GetObject(L, 4, typeof(System.Collections.Generic.HashSet<string>));
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorFloat( _animator, _parameterName, _value, _parameterList );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.GameExtensionForUnity.UpdateAnimatorFloat!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateAnimatorTrigger_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Animator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Collections.Generic.HashSet<int>>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    int _parameter = LuaAPI.xlua_tointeger(L, 2);
                    System.Collections.Generic.HashSet<int> _parameterList = (System.Collections.Generic.HashSet<int>)translator.GetObject(L, 3, typeof(System.Collections.Generic.HashSet<int>));
                    bool _performSanityCheck = LuaAPI.lua_toboolean(L, 4);
                    
                        bool gen_ret = Honor.Runtime.GameExtensionForUnity.UpdateAnimatorTrigger( _animator, _parameter, _parameterList, _performSanityCheck );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Animator>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Collections.Generic.HashSet<int>>(L, 3)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    int _parameter = LuaAPI.xlua_tointeger(L, 2);
                    System.Collections.Generic.HashSet<int> _parameterList = (System.Collections.Generic.HashSet<int>)translator.GetObject(L, 3, typeof(System.Collections.Generic.HashSet<int>));
                    
                        bool gen_ret = Honor.Runtime.GameExtensionForUnity.UpdateAnimatorTrigger( _animator, _parameter, _parameterList );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Collections.Generic.HashSet<string>>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    System.Collections.Generic.HashSet<string> _parameterList = (System.Collections.Generic.HashSet<string>)translator.GetObject(L, 3, typeof(System.Collections.Generic.HashSet<string>));
                    bool _performSanityCheck = LuaAPI.lua_toboolean(L, 4);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorTrigger( _animator, _parameterName, _parameterList, _performSanityCheck );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Collections.Generic.HashSet<string>>(L, 3)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    System.Collections.Generic.HashSet<string> _parameterList = (System.Collections.Generic.HashSet<string>)translator.GetObject(L, 3, typeof(System.Collections.Generic.HashSet<string>));
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorTrigger( _animator, _parameterName, _parameterList );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.GameExtensionForUnity.UpdateAnimatorTrigger!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateAnimatorBoolIfExists_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    bool _value = LuaAPI.lua_toboolean(L, 3);
                    bool _performSanityCheck = LuaAPI.lua_toboolean(L, 4);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorBoolIfExists( _animator, _parameterName, _value, _performSanityCheck );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    bool _value = LuaAPI.lua_toboolean(L, 3);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorBoolIfExists( _animator, _parameterName, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.GameExtensionForUnity.UpdateAnimatorBoolIfExists!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateAnimatorTriggerIfExists_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    bool _performSanityCheck = LuaAPI.lua_toboolean(L, 3);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorTriggerIfExists( _animator, _parameterName, _performSanityCheck );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorTriggerIfExists( _animator, _parameterName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.GameExtensionForUnity.UpdateAnimatorTriggerIfExists!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateAnimatorFloatIfExists_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _performSanityCheck = LuaAPI.lua_toboolean(L, 4);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorFloatIfExists( _animator, _parameterName, _value, _performSanityCheck );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    float _value = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorFloatIfExists( _animator, _parameterName, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.GameExtensionForUnity.UpdateAnimatorFloatIfExists!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateAnimatorIntegerIfExists_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    int _value = LuaAPI.xlua_tointeger(L, 3);
                    bool _performSanityCheck = LuaAPI.lua_toboolean(L, 4);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorIntegerIfExists( _animator, _parameterName, _value, _performSanityCheck );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Animator>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 1, typeof(UnityEngine.Animator));
                    string _parameterName = LuaAPI.lua_tostring(L, 2);
                    int _value = LuaAPI.xlua_tointeger(L, 3);
                    
                    Honor.Runtime.GameExtensionForUnity.UpdateAnimatorIntegerIfExists( _animator, _parameterName, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.GameExtensionForUnity.UpdateAnimatorIntegerIfExists!");
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
