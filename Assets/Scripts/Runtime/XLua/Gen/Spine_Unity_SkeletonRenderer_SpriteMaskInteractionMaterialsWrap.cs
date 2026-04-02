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
    public class SpineUnitySkeletonRendererSpriteMaskInteractionMaterialsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 4, 3);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "AnyMaterialCreated", _g_get_AnyMaterialCreated);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "materialsMaskDisabled", _g_get_materialsMaskDisabled);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "materialsInsideMask", _g_get_materialsInsideMask);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "materialsOutsideMask", _g_get_materialsOutsideMask);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "materialsMaskDisabled", _s_set_materialsMaskDisabled);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "materialsInsideMask", _s_set_materialsInsideMask);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "materialsOutsideMask", _s_set_materialsOutsideMask);
            
			
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
					
					Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials gen_ret = new Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AnyMaterialCreated(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials gen_to_be_invoked = (Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.AnyMaterialCreated);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_materialsMaskDisabled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials gen_to_be_invoked = (Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.materialsMaskDisabled);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_materialsInsideMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials gen_to_be_invoked = (Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.materialsInsideMask);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_materialsOutsideMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials gen_to_be_invoked = (Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.materialsOutsideMask);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_materialsMaskDisabled(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials gen_to_be_invoked = (Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.materialsMaskDisabled = (UnityEngine.Material[])translator.GetObject(L, 2, typeof(UnityEngine.Material[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_materialsInsideMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials gen_to_be_invoked = (Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.materialsInsideMask = (UnityEngine.Material[])translator.GetObject(L, 2, typeof(UnityEngine.Material[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_materialsOutsideMask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials gen_to_be_invoked = (Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.materialsOutsideMask = (UnityEngine.Material[])translator.GetObject(L, 2, typeof(UnityEngine.Material[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
