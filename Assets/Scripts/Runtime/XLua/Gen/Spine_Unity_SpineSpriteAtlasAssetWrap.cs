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
    public class SpineUnitySpineSpriteAtlasAssetWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Spine.Unity.SpineSpriteAtlasAsset);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 7, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAtlas", _m_GetAtlas);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsLoaded", _g_get_IsLoaded);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Materials", _g_get_Materials);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaterialCount", _g_get_MaterialCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PrimaryMaterial", _g_get_PrimaryMaterial);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "spriteAtlasFile", _g_get_spriteAtlasFile);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "materials", _g_get_materials);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "updateRegionsInPlayMode", _g_get_updateRegionsInPlayMode);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "spriteAtlasFile", _s_set_spriteAtlasFile);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "materials", _s_set_materials);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "updateRegionsInPlayMode", _s_set_updateRegionsInPlayMode);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 4, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateRuntimeInstance", _m_CreateRuntimeInstance_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AccessPackedTexture", _m_AccessPackedTexture_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AccessPackedSprites", _m_AccessPackedSprites_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Spine.Unity.SpineSpriteAtlasAsset gen_ret = new Spine.Unity.SpineSpriteAtlasAsset();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SpineSpriteAtlasAsset constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateRuntimeInstance_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.U2D.SpriteAtlas _spriteAtlasFile = (UnityEngine.U2D.SpriteAtlas)translator.GetObject(L, 1, typeof(UnityEngine.U2D.SpriteAtlas));
                    UnityEngine.Material[] _materials = (UnityEngine.Material[])translator.GetObject(L, 2, typeof(UnityEngine.Material[]));
                    bool _initialize = LuaAPI.lua_toboolean(L, 3);
                    
                        Spine.Unity.SpineSpriteAtlasAsset gen_ret = Spine.Unity.SpineSpriteAtlasAsset.CreateRuntimeInstance( _spriteAtlasFile, _materials, _initialize );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SpineSpriteAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineSpriteAtlasAsset)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAtlas(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SpineSpriteAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineSpriteAtlasAsset)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _onlyMetaData = LuaAPI.lua_toboolean(L, 2);
                    
                        Spine.Atlas gen_ret = gen_to_be_invoked.GetAtlas( _onlyMetaData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1) 
                {
                    
                        Spine.Atlas gen_ret = gen_to_be_invoked.GetAtlas(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SpineSpriteAtlasAsset.GetAtlas!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AccessPackedTexture_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Sprite[] _sprites = (UnityEngine.Sprite[])translator.GetObject(L, 1, typeof(UnityEngine.Sprite[]));
                    
                        UnityEngine.Texture2D gen_ret = Spine.Unity.SpineSpriteAtlasAsset.AccessPackedTexture( _sprites );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AccessPackedSprites_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.U2D.SpriteAtlas _spriteAtlas = (UnityEngine.U2D.SpriteAtlas)translator.GetObject(L, 1, typeof(UnityEngine.U2D.SpriteAtlas));
                    
                        UnityEngine.Sprite[] gen_ret = Spine.Unity.SpineSpriteAtlasAsset.AccessPackedSprites( _spriteAtlas );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsLoaded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineSpriteAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineSpriteAtlasAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsLoaded);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Materials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineSpriteAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineSpriteAtlasAsset)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.Materials);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaterialCount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineSpriteAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineSpriteAtlasAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MaterialCount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PrimaryMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineSpriteAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineSpriteAtlasAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PrimaryMaterial);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_spriteAtlasFile(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineSpriteAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineSpriteAtlasAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.spriteAtlasFile);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_materials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineSpriteAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineSpriteAtlasAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.materials);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_updateRegionsInPlayMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineSpriteAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineSpriteAtlasAsset)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.updateRegionsInPlayMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_spriteAtlasFile(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineSpriteAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineSpriteAtlasAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.spriteAtlasFile = (UnityEngine.U2D.SpriteAtlas)translator.GetObject(L, 2, typeof(UnityEngine.U2D.SpriteAtlas));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_materials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineSpriteAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineSpriteAtlasAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.materials = (UnityEngine.Material[])translator.GetObject(L, 2, typeof(UnityEngine.Material[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_updateRegionsInPlayMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineSpriteAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineSpriteAtlasAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.updateRegionsInPlayMode = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
