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
    public class SpineUnitySkeletonMecanimMecanimTranslatorWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Spine.Unity.SkeletonMecanim.MecanimTranslator);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 7, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Initialize", _m_Initialize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Apply", _m_Apply);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetActiveAnimationAndTime", _m_GetActiveAnimationAndTime);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnClipApplied", _e_OnClipApplied);
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Animator", _g_get_Animator);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MecanimLayerCount", _g_get_MecanimLayerCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MecanimLayerNames", _g_get_MecanimLayerNames);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "autoReset", _g_get_autoReset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "useCustomMixMode", _g_get_useCustomMixMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "layerMixModes", _g_get_layerMixModes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "layerBlendModes", _g_get_layerBlendModes);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "autoReset", _s_set_autoReset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "useCustomMixMode", _s_set_useCustomMixMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "layerMixModes", _s_set_layerMixModes);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "layerBlendModes", _s_set_layerBlendModes);
            
			
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
					
					Spine.Unity.SkeletonMecanim.MecanimTranslator gen_ret = new Spine.Unity.SkeletonMecanim.MecanimTranslator();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonMecanim.MecanimTranslator constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Initialize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Animator _animator = (UnityEngine.Animator)translator.GetObject(L, 2, typeof(UnityEngine.Animator));
                    Spine.Unity.SkeletonDataAsset _skeletonDataAsset = (Spine.Unity.SkeletonDataAsset)translator.GetObject(L, 3, typeof(Spine.Unity.SkeletonDataAsset));
                    
                    gen_to_be_invoked.Initialize( _animator, _skeletonDataAsset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Apply(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Spine.Skeleton _skeleton = (Spine.Skeleton)translator.GetObject(L, 2, typeof(Spine.Skeleton));
                    
                    gen_to_be_invoked.Apply( _skeleton );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetActiveAnimationAndTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _layer = LuaAPI.xlua_tointeger(L, 2);
                    
                        System.Collections.Generic.KeyValuePair<Spine.Animation, float> gen_ret = gen_to_be_invoked.GetActiveAnimationAndTime( _layer );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Animator(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Animator);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MecanimLayerCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MecanimLayerCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MecanimLayerNames(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MecanimLayerNames);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_autoReset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.autoReset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_useCustomMixMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.useCustomMixMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_layerMixModes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.layerMixModes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_layerBlendModes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.layerBlendModes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_autoReset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.autoReset = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_useCustomMixMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.useCustomMixMode = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_layerMixModes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.layerMixModes = (Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode[])translator.GetObject(L, 2, typeof(Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_layerBlendModes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.layerBlendModes = (Spine.MixBlend[])translator.GetObject(L, 2, typeof(Spine.MixBlend[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnClipApplied(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Spine.Unity.SkeletonMecanim.MecanimTranslator gen_to_be_invoked = (Spine.Unity.SkeletonMecanim.MecanimTranslator)translator.FastGetCSObj(L, 1);
                Spine.Unity.SkeletonMecanim.MecanimTranslator.OnClipAppliedDelegate gen_delegate = translator.GetDelegate<Spine.Unity.SkeletonMecanim.MecanimTranslator.OnClipAppliedDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need Spine.Unity.SkeletonMecanim.MecanimTranslator.OnClipAppliedDelegate!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnClipApplied += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnClipApplied -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonMecanim.MecanimTranslator.OnClipApplied!");
            return 0;
        }
        
		
		
    }
}
