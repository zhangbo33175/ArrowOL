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
    public class UnityEngineTimelineControlPlayableAssetWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Timeline.ControlPlayableAsset);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 11, 9);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnEnable", _m_OnEnable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreatePlayable", _m_CreatePlayable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GatherProperties", _m_GatherProperties);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "duration", _g_get_duration);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "clipCaps", _g_get_clipCaps);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "sourceGameObject", _g_get_sourceGameObject);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "prefabGameObject", _g_get_prefabGameObject);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "updateParticle", _g_get_updateParticle);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "particleRandomSeed", _g_get_particleRandomSeed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "updateDirector", _g_get_updateDirector);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "updateITimeControl", _g_get_updateITimeControl);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "searchHierarchy", _g_get_searchHierarchy);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "active", _g_get_active);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "postPlayback", _g_get_postPlayback);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "sourceGameObject", _s_set_sourceGameObject);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "prefabGameObject", _s_set_prefabGameObject);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "updateParticle", _s_set_updateParticle);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "particleRandomSeed", _s_set_particleRandomSeed);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "updateDirector", _s_set_updateDirector);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "updateITimeControl", _s_set_updateITimeControl);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "searchHierarchy", _s_set_searchHierarchy);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "active", _s_set_active);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "postPlayback", _s_set_postPlayback);
            
			
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
					
					UnityEngine.Timeline.ControlPlayableAsset gen_ret = new UnityEngine.Timeline.ControlPlayableAsset();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Timeline.ControlPlayableAsset constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnEnable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnEnable(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreatePlayable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Playables.PlayableGraph _graph;translator.Get(L, 2, out _graph);
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    
                        UnityEngine.Playables.Playable gen_ret = gen_to_be_invoked.CreatePlayable( _graph, _go );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GatherProperties(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Playables.PlayableDirector _director = (UnityEngine.Playables.PlayableDirector)translator.GetObject(L, 2, typeof(UnityEngine.Playables.PlayableDirector));
                    UnityEngine.Timeline.IPropertyCollector _driver = (UnityEngine.Timeline.IPropertyCollector)translator.GetObject(L, 3, typeof(UnityEngine.Timeline.IPropertyCollector));
                    
                    gen_to_be_invoked.GatherProperties( _director, _driver );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_duration(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.duration);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_clipCaps(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineClipCaps(L, gen_to_be_invoked.clipCaps);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_sourceGameObject(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.sourceGameObject);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_prefabGameObject(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.prefabGameObject);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_updateParticle(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.updateParticle);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_particleRandomSeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushuint(L, gen_to_be_invoked.particleRandomSeed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_updateDirector(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.updateDirector);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_updateITimeControl(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.updateITimeControl);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_searchHierarchy(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.searchHierarchy);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_active(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.active);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_postPlayback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineTimelineActivationControlPlayablePostPlaybackState(L, gen_to_be_invoked.postPlayback);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_sourceGameObject(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                UnityEngine.ExposedReference<UnityEngine.GameObject> gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.sourceGameObject = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_prefabGameObject(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.prefabGameObject = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_updateParticle(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.updateParticle = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_particleRandomSeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.particleRandomSeed = LuaAPI.xlua_touint(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_updateDirector(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.updateDirector = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_updateITimeControl(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.updateITimeControl = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_searchHierarchy(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.searchHierarchy = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_active(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.active = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_postPlayback(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityEngine.Timeline.ControlPlayableAsset gen_to_be_invoked = (UnityEngine.Timeline.ControlPlayableAsset)translator.FastGetCSObj(L, 1);
                UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.postPlayback = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
