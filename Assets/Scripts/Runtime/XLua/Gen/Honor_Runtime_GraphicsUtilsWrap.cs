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
    public class HonorRuntimeGraphicsUtilsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.GraphicsUtils);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 6, 1, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "ScreenRatio", _m_ScreenRatio_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateScreenInfoOnScreenChange", _m_UpdateScreenInfoOnScreenChange_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRoleRootTransform", _m_GetRoleRootTransform_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "designWidth", Honor.Runtime.GraphicsUtils.designWidth);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "designHeight", Honor.Runtime.GraphicsUtils.designHeight);
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "IsMainRun", _g_get_IsMainRun);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Honor.Runtime.GraphicsUtils does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScreenRatio_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Honor.Runtime.GraphicsUtils.EScreenType _type;translator.Get(L, 1, out _type);
                    
                        float gen_ret = Honor.Runtime.GraphicsUtils.ScreenRatio( _type );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateScreenInfoOnScreenChange_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    Honor.Runtime.GraphicsUtils.UpdateScreenInfoOnScreenChange(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRoleRootTransform_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& translator.Assignable<UnityEngine.Transform>(L, 1)) 
                {
                    UnityEngine.Transform _trans = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    
                        UnityEngine.Transform gen_ret = Honor.Runtime.GraphicsUtils.GetRoleRootTransform( _trans );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Transform>(L, 1)&& translator.Assignable<UnityEngine.LayerMask>(L, 2)) 
                {
                    UnityEngine.Transform _trans = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.LayerMask _layerMask;translator.Get(L, 2, out _layerMask);
                    
                        UnityEngine.Transform gen_ret = Honor.Runtime.GraphicsUtils.GetRoleRootTransform( _trans, _layerMask );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.GraphicsUtils.GetRoleRootTransform!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsMainRun(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, Honor.Runtime.GraphicsUtils.IsMainRun);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
