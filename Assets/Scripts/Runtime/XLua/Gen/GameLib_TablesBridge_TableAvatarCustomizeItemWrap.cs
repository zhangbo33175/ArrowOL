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
    public class GameLibTablesBridgeTableAvatarCustomizeItemWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameLib.TablesBridge.TableAvatarCustomizeItem);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 5, 5);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "ID", _g_get_ID);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EnableColorChange", _g_get_EnableColorChange);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ColorSlotName", _g_get_ColorSlotName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ColorRGB", _g_get_ColorRGB);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RandomColorID", _g_get_RandomColorID);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "ID", _s_set_ID);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EnableColorChange", _s_set_EnableColorChange);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ColorSlotName", _s_set_ColorSlotName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ColorRGB", _s_set_ColorRGB);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RandomColorID", _s_set_RandomColorID);
            
			
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
					
					GameLib.TablesBridge.TableAvatarCustomizeItem gen_ret = new GameLib.TablesBridge.TableAvatarCustomizeItem();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameLib.TablesBridge.TableAvatarCustomizeItem constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.TablesBridge.TableAvatarCustomizeItem gen_to_be_invoked = (GameLib.TablesBridge.TableAvatarCustomizeItem)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.ID);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EnableColorChange(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.TablesBridge.TableAvatarCustomizeItem gen_to_be_invoked = (GameLib.TablesBridge.TableAvatarCustomizeItem)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.EnableColorChange);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ColorSlotName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.TablesBridge.TableAvatarCustomizeItem gen_to_be_invoked = (GameLib.TablesBridge.TableAvatarCustomizeItem)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ColorSlotName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ColorRGB(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.TablesBridge.TableAvatarCustomizeItem gen_to_be_invoked = (GameLib.TablesBridge.TableAvatarCustomizeItem)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ColorRGB);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RandomColorID(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.TablesBridge.TableAvatarCustomizeItem gen_to_be_invoked = (GameLib.TablesBridge.TableAvatarCustomizeItem)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.RandomColorID);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.TablesBridge.TableAvatarCustomizeItem gen_to_be_invoked = (GameLib.TablesBridge.TableAvatarCustomizeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ID = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EnableColorChange(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.TablesBridge.TableAvatarCustomizeItem gen_to_be_invoked = (GameLib.TablesBridge.TableAvatarCustomizeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EnableColorChange = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ColorSlotName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.TablesBridge.TableAvatarCustomizeItem gen_to_be_invoked = (GameLib.TablesBridge.TableAvatarCustomizeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ColorSlotName = (System.Collections.Generic.List<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<string>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ColorRGB(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.TablesBridge.TableAvatarCustomizeItem gen_to_be_invoked = (GameLib.TablesBridge.TableAvatarCustomizeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ColorRGB = (System.Collections.Generic.List<GameLib.TablesBridge.TableAvatarCustomizeItemColor>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<GameLib.TablesBridge.TableAvatarCustomizeItemColor>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RandomColorID(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GameLib.TablesBridge.TableAvatarCustomizeItem gen_to_be_invoked = (GameLib.TablesBridge.TableAvatarCustomizeItem)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RandomColorID = (System.Collections.Generic.List<int>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<int>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
