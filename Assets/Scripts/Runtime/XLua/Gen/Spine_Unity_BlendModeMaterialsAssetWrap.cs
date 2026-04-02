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
    public class SpineUnityBlendModeMaterialsAssetWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Spine.Unity.BlendModeMaterialsAsset);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 4, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Apply", _m_Apply);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "multiplyMaterialTemplate", _g_get_multiplyMaterialTemplate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "screenMaterialTemplate", _g_get_screenMaterialTemplate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "additiveMaterialTemplate", _g_get_additiveMaterialTemplate);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "applyAdditiveMaterial", _g_get_applyAdditiveMaterial);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "multiplyMaterialTemplate", _s_set_multiplyMaterialTemplate);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "screenMaterialTemplate", _s_set_screenMaterialTemplate);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "additiveMaterialTemplate", _s_set_additiveMaterialTemplate);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "applyAdditiveMaterial", _s_set_applyAdditiveMaterial);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "ApplyMaterials", _m_ApplyMaterials_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Spine.Unity.BlendModeMaterialsAsset gen_ret = new Spine.Unity.BlendModeMaterialsAsset();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.BlendModeMaterialsAsset constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Apply(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.BlendModeMaterialsAsset gen_to_be_invoked = (Spine.Unity.BlendModeMaterialsAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Spine.SkeletonData _skeletonData = (Spine.SkeletonData)translator.GetObject(L, 2, typeof(Spine.SkeletonData));
                    
                    gen_to_be_invoked.Apply( _skeletonData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ApplyMaterials_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Spine.SkeletonData _skeletonData = (Spine.SkeletonData)translator.GetObject(L, 1, typeof(Spine.SkeletonData));
                    UnityEngine.Material _multiplyTemplate = (UnityEngine.Material)translator.GetObject(L, 2, typeof(UnityEngine.Material));
                    UnityEngine.Material _screenTemplate = (UnityEngine.Material)translator.GetObject(L, 3, typeof(UnityEngine.Material));
                    UnityEngine.Material _additiveTemplate = (UnityEngine.Material)translator.GetObject(L, 4, typeof(UnityEngine.Material));
                    bool _includeAdditiveSlots = LuaAPI.lua_toboolean(L, 5);
                    
                    Spine.Unity.BlendModeMaterialsAsset.ApplyMaterials( _skeletonData, _multiplyTemplate, _screenTemplate, _additiveTemplate, _includeAdditiveSlots );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_multiplyMaterialTemplate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterialsAsset gen_to_be_invoked = (Spine.Unity.BlendModeMaterialsAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.multiplyMaterialTemplate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_screenMaterialTemplate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterialsAsset gen_to_be_invoked = (Spine.Unity.BlendModeMaterialsAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.screenMaterialTemplate);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_additiveMaterialTemplate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterialsAsset gen_to_be_invoked = (Spine.Unity.BlendModeMaterialsAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.additiveMaterialTemplate);
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
			
                Spine.Unity.BlendModeMaterialsAsset gen_to_be_invoked = (Spine.Unity.BlendModeMaterialsAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.applyAdditiveMaterial);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_multiplyMaterialTemplate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterialsAsset gen_to_be_invoked = (Spine.Unity.BlendModeMaterialsAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.multiplyMaterialTemplate = (UnityEngine.Material)translator.GetObject(L, 2, typeof(UnityEngine.Material));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_screenMaterialTemplate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterialsAsset gen_to_be_invoked = (Spine.Unity.BlendModeMaterialsAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.screenMaterialTemplate = (UnityEngine.Material)translator.GetObject(L, 2, typeof(UnityEngine.Material));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_additiveMaterialTemplate(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.BlendModeMaterialsAsset gen_to_be_invoked = (Spine.Unity.BlendModeMaterialsAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.additiveMaterialTemplate = (UnityEngine.Material)translator.GetObject(L, 2, typeof(UnityEngine.Material));
            
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
			
                Spine.Unity.BlendModeMaterialsAsset gen_to_be_invoked = (Spine.Unity.BlendModeMaterialsAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.applyAdditiveMaterial = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
