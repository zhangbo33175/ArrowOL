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
    public class SpineUnityActivateBasedOnFlipDirectionWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Spine.Unity.ActivateBasedOnFlipDirection);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 4, 4);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "skeletonRenderer", _g_get_skeletonRenderer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "skeletonGraphic", _g_get_skeletonGraphic);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "activeOnNormalX", _g_get_activeOnNormalX);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "activeOnFlippedX", _g_get_activeOnFlippedX);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "skeletonRenderer", _s_set_skeletonRenderer);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "skeletonGraphic", _s_set_skeletonGraphic);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "activeOnNormalX", _s_set_activeOnNormalX);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "activeOnFlippedX", _s_set_activeOnFlippedX);
            
			
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
					
					Spine.Unity.ActivateBasedOnFlipDirection gen_ret = new Spine.Unity.ActivateBasedOnFlipDirection();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.ActivateBasedOnFlipDirection constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_skeletonRenderer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.ActivateBasedOnFlipDirection gen_to_be_invoked = (Spine.Unity.ActivateBasedOnFlipDirection)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.skeletonRenderer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_skeletonGraphic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.ActivateBasedOnFlipDirection gen_to_be_invoked = (Spine.Unity.ActivateBasedOnFlipDirection)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.skeletonGraphic);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_activeOnNormalX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.ActivateBasedOnFlipDirection gen_to_be_invoked = (Spine.Unity.ActivateBasedOnFlipDirection)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.activeOnNormalX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_activeOnFlippedX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.ActivateBasedOnFlipDirection gen_to_be_invoked = (Spine.Unity.ActivateBasedOnFlipDirection)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.activeOnFlippedX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_skeletonRenderer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.ActivateBasedOnFlipDirection gen_to_be_invoked = (Spine.Unity.ActivateBasedOnFlipDirection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.skeletonRenderer = (Spine.Unity.SkeletonRenderer)translator.GetObject(L, 2, typeof(Spine.Unity.SkeletonRenderer));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_skeletonGraphic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.ActivateBasedOnFlipDirection gen_to_be_invoked = (Spine.Unity.ActivateBasedOnFlipDirection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.skeletonGraphic = (Spine.Unity.SkeletonGraphic)translator.GetObject(L, 2, typeof(Spine.Unity.SkeletonGraphic));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_activeOnNormalX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.ActivateBasedOnFlipDirection gen_to_be_invoked = (Spine.Unity.ActivateBasedOnFlipDirection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.activeOnNormalX = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_activeOnFlippedX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.ActivateBasedOnFlipDirection gen_to_be_invoked = (Spine.Unity.ActivateBasedOnFlipDirection)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.activeOnFlippedX = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
