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
    public class HonorRuntimeFxDeathWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.FxDeath);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 16, 16);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetRole", _m_SetRole);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetDescription", _m_GetDescription);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "life", _g_get_life);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "dir", _g_get_dir);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "quantity", _g_get_quantity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "range", _g_get_range);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "color", _g_get_color);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "bright", _g_get_bright);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gloss", _g_get_gloss);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "show", _g_get_show);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "smrs", _g_get_smrs);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "enableRendererList", _g_get_enableRendererList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "disableRendererList", _g_get_disableRendererList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "originMats", _g_get_originMats);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "fxMatList", _g_get_fxMatList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "newMatList", _g_get_newMatList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "roleRootTrans", _g_get_roleRootTrans);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "setRoleObj", _g_get_setRoleObj);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "life", _s_set_life);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "dir", _s_set_dir);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "quantity", _s_set_quantity);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "range", _s_set_range);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "color", _s_set_color);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "bright", _s_set_bright);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "gloss", _s_set_gloss);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "show", _s_set_show);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "smrs", _s_set_smrs);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "enableRendererList", _s_set_enableRendererList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "disableRendererList", _s_set_disableRendererList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "originMats", _s_set_originMats);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "fxMatList", _s_set_fxMatList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "newMatList", _s_set_newMatList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "roleRootTrans", _s_set_roleRootTrans);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "setRoleObj", _s_set_setRoleObj);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Description", Honor.Runtime.FxDeath.Description);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Honor.Runtime.FxDeath gen_ret = new Honor.Runtime.FxDeath();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.FxDeath constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetRole(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject _obj = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    
                    gen_to_be_invoked.SetRole( _obj );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDescription(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        string gen_ret = gen_to_be_invoked.GetDescription(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_life(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.life);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_dir(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.dir);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_quantity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.quantity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_range(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.range);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_color(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.color);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_bright(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.bright);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gloss(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.gloss);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_show(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.show);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_smrs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.smrs);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_enableRendererList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.enableRendererList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_disableRendererList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.disableRendererList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_originMats(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.originMats);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_fxMatList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.fxMatList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_newMatList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.newMatList);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_roleRootTrans(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.roleRootTrans);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_setRoleObj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.setRoleObj);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_life(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.life = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_dir(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.dir = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_quantity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.quantity = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_range(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.range = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_color(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.color = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_bright(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.bright = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gloss(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.gloss = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_show(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.show = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_smrs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.smrs = (UnityEngine.Renderer[])translator.GetObject(L, 2, typeof(UnityEngine.Renderer[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_enableRendererList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.enableRendererList = (System.Collections.Generic.List<UnityEngine.Renderer>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Renderer>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_disableRendererList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.disableRendererList = (System.Collections.Generic.List<UnityEngine.Renderer>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Renderer>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_originMats(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.originMats = (UnityEngine.Material[][])translator.GetObject(L, 2, typeof(UnityEngine.Material[][]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_fxMatList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.fxMatList = (System.Collections.Generic.List<UnityEngine.Material>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Material>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_newMatList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.newMatList = (System.Collections.Generic.List<UnityEngine.Material>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Material>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_roleRootTrans(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.roleRootTrans = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_setRoleObj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.FxDeath gen_to_be_invoked = (Honor.Runtime.FxDeath)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.setRoleObj = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
