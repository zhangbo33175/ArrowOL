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
    public class SpineUnityBlendModeMaterialsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Spine.Unity.BlendModeMaterials);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 5, 5);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BlendModeForMaterial", _m_BlendModeForMaterial);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ApplyMaterials", _m_ApplyMaterials);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "RequiresBlendModeMaterials", _g_get_RequiresBlendModeMaterials);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "applyAdditiveMaterial", _g_get_applyAdditiveMaterial);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "additiveMaterials", _g_get_additiveMaterials);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "multiplyMaterials", _g_get_multiplyMaterials);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "screenMaterials", _g_get_screenMaterials);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "RequiresBlendModeMaterials", _s_set_RequiresBlendModeMaterials);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "applyAdditiveMaterial", _s_set_applyAdditiveMaterial);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "additiveMaterials", _s_set_additiveMaterials);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "multiplyMaterials", _s_set_multiplyMaterials);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "screenMaterials", _s_set_screenMaterials);
            
			
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
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Spine.Unity.BlendModeMaterials gen_ret = new Spine.Unity.BlendModeMaterials();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.BlendModeMaterials constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BlendModeForMaterial(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.BlendModeMaterials gen_to_be_invoked = (Spine.Unity.BlendModeMaterials)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Material _material = (UnityEngine.Material)translator.GetObject(L, 2, typeof(UnityEngine.Material));
                    
                        Spine.BlendMode gen_ret = gen_to_be_invoked.BlendModeForMaterial( _material );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ApplyMaterials(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.BlendModeMaterials gen_to_be_invoked = (Spine.Unity.BlendModeMaterials)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Spine.SkeletonData _skeletonData = (Spine.SkeletonData)translator.GetObject(L, 2, typeof(Spine.SkeletonData));
                    
                    gen_to_be_invoked.ApplyMaterials( _skeletonData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RequiresBlendModeMaterials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterials gen_to_be_invoked = (Spine.Unity.BlendModeMaterials)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.RequiresBlendModeMaterials);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_applyAdditiveMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterials gen_to_be_invoked = (Spine.Unity.BlendModeMaterials)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.applyAdditiveMaterial);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_additiveMaterials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterials gen_to_be_invoked = (Spine.Unity.BlendModeMaterials)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.additiveMaterials);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_multiplyMaterials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterials gen_to_be_invoked = (Spine.Unity.BlendModeMaterials)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.multiplyMaterials);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_screenMaterials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterials gen_to_be_invoked = (Spine.Unity.BlendModeMaterials)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.screenMaterials);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RequiresBlendModeMaterials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterials gen_to_be_invoked = (Spine.Unity.BlendModeMaterials)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RequiresBlendModeMaterials = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_applyAdditiveMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterials gen_to_be_invoked = (Spine.Unity.BlendModeMaterials)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.applyAdditiveMaterial = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_additiveMaterials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterials gen_to_be_invoked = (Spine.Unity.BlendModeMaterials)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.additiveMaterials = (System.Collections.Generic.List<Spine.Unity.BlendModeMaterials.ReplacementMaterial>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<Spine.Unity.BlendModeMaterials.ReplacementMaterial>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_multiplyMaterials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterials gen_to_be_invoked = (Spine.Unity.BlendModeMaterials)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.multiplyMaterials = (System.Collections.Generic.List<Spine.Unity.BlendModeMaterials.ReplacementMaterial>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<Spine.Unity.BlendModeMaterials.ReplacementMaterial>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_screenMaterials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterials gen_to_be_invoked = (Spine.Unity.BlendModeMaterials)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.screenMaterials = (System.Collections.Generic.List<Spine.Unity.BlendModeMaterials.ReplacementMaterial>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<Spine.Unity.BlendModeMaterials.ReplacementMaterial>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
