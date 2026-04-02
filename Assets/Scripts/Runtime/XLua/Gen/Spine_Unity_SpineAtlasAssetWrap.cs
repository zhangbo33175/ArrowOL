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
    public class SpineUnitySpineAtlasAssetWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Spine.Unity.SpineAtlasAsset);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 7, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clear", _m_Clear);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAtlas", _m_GetAtlas);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GenerateMesh", _m_GenerateMesh);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsLoaded", _g_get_IsLoaded);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Materials", _g_get_Materials);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaterialCount", _g_get_MaterialCount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PrimaryMaterial", _g_get_PrimaryMaterial);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "atlasFile", _g_get_atlasFile);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "materials", _g_get_materials);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "customTextureLoader", _g_get_customTextureLoader);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "atlasFile", _s_set_atlasFile);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "materials", _s_set_materials);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "customTextureLoader", _s_set_customTextureLoader);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateRuntimeInstance", _m_CreateRuntimeInstance_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Spine.Unity.SpineAtlasAsset gen_ret = new Spine.Unity.SpineAtlasAsset();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SpineAtlasAsset constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateRuntimeInstance_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.TextAsset>(L, 1)&& translator.Assignable<UnityEngine.Material[]>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& translator.Assignable<System.Func<Spine.Unity.SpineAtlasAsset, Spine.TextureLoader>>(L, 4)) 
                {
                    UnityEngine.TextAsset _atlasText = (UnityEngine.TextAsset)translator.GetObject(L, 1, typeof(UnityEngine.TextAsset));
                    UnityEngine.Material[] _materials = (UnityEngine.Material[])translator.GetObject(L, 2, typeof(UnityEngine.Material[]));
                    bool _initialize = LuaAPI.lua_toboolean(L, 3);
                    System.Func<Spine.Unity.SpineAtlasAsset, Spine.TextureLoader> _newCustomTextureLoader = translator.GetDelegate<System.Func<Spine.Unity.SpineAtlasAsset, Spine.TextureLoader>>(L, 4);
                    
                        Spine.Unity.SpineAtlasAsset gen_ret = Spine.Unity.SpineAtlasAsset.CreateRuntimeInstance( _atlasText, _materials, _initialize, _newCustomTextureLoader );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.TextAsset>(L, 1)&& translator.Assignable<UnityEngine.Material[]>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.TextAsset _atlasText = (UnityEngine.TextAsset)translator.GetObject(L, 1, typeof(UnityEngine.TextAsset));
                    UnityEngine.Material[] _materials = (UnityEngine.Material[])translator.GetObject(L, 2, typeof(UnityEngine.Material[]));
                    bool _initialize = LuaAPI.lua_toboolean(L, 3);
                    
                        Spine.Unity.SpineAtlasAsset gen_ret = Spine.Unity.SpineAtlasAsset.CreateRuntimeInstance( _atlasText, _materials, _initialize );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.TextAsset>(L, 1)&& translator.Assignable<UnityEngine.Texture2D[]>(L, 2)&& translator.Assignable<UnityEngine.Material>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Func<Spine.Unity.SpineAtlasAsset, Spine.TextureLoader>>(L, 5)) 
                {
                    UnityEngine.TextAsset _atlasText = (UnityEngine.TextAsset)translator.GetObject(L, 1, typeof(UnityEngine.TextAsset));
                    UnityEngine.Texture2D[] _textures = (UnityEngine.Texture2D[])translator.GetObject(L, 2, typeof(UnityEngine.Texture2D[]));
                    UnityEngine.Material _materialPropertySource = (UnityEngine.Material)translator.GetObject(L, 3, typeof(UnityEngine.Material));
                    bool _initialize = LuaAPI.lua_toboolean(L, 4);
                    System.Func<Spine.Unity.SpineAtlasAsset, Spine.TextureLoader> _newCustomTextureLoader = translator.GetDelegate<System.Func<Spine.Unity.SpineAtlasAsset, Spine.TextureLoader>>(L, 5);
                    
                        Spine.Unity.SpineAtlasAsset gen_ret = Spine.Unity.SpineAtlasAsset.CreateRuntimeInstance( _atlasText, _textures, _materialPropertySource, _initialize, _newCustomTextureLoader );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.TextAsset>(L, 1)&& translator.Assignable<UnityEngine.Texture2D[]>(L, 2)&& translator.Assignable<UnityEngine.Material>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.TextAsset _atlasText = (UnityEngine.TextAsset)translator.GetObject(L, 1, typeof(UnityEngine.TextAsset));
                    UnityEngine.Texture2D[] _textures = (UnityEngine.Texture2D[])translator.GetObject(L, 2, typeof(UnityEngine.Texture2D[]));
                    UnityEngine.Material _materialPropertySource = (UnityEngine.Material)translator.GetObject(L, 3, typeof(UnityEngine.Material));
                    bool _initialize = LuaAPI.lua_toboolean(L, 4);
                    
                        Spine.Unity.SpineAtlasAsset gen_ret = Spine.Unity.SpineAtlasAsset.CreateRuntimeInstance( _atlasText, _textures, _materialPropertySource, _initialize );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.TextAsset>(L, 1)&& translator.Assignable<UnityEngine.Texture2D[]>(L, 2)&& translator.Assignable<UnityEngine.Shader>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Func<Spine.Unity.SpineAtlasAsset, Spine.TextureLoader>>(L, 5)) 
                {
                    UnityEngine.TextAsset _atlasText = (UnityEngine.TextAsset)translator.GetObject(L, 1, typeof(UnityEngine.TextAsset));
                    UnityEngine.Texture2D[] _textures = (UnityEngine.Texture2D[])translator.GetObject(L, 2, typeof(UnityEngine.Texture2D[]));
                    UnityEngine.Shader _shader = (UnityEngine.Shader)translator.GetObject(L, 3, typeof(UnityEngine.Shader));
                    bool _initialize = LuaAPI.lua_toboolean(L, 4);
                    System.Func<Spine.Unity.SpineAtlasAsset, Spine.TextureLoader> _newCustomTextureLoader = translator.GetDelegate<System.Func<Spine.Unity.SpineAtlasAsset, Spine.TextureLoader>>(L, 5);
                    
                        Spine.Unity.SpineAtlasAsset gen_ret = Spine.Unity.SpineAtlasAsset.CreateRuntimeInstance( _atlasText, _textures, _shader, _initialize, _newCustomTextureLoader );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.TextAsset>(L, 1)&& translator.Assignable<UnityEngine.Texture2D[]>(L, 2)&& translator.Assignable<UnityEngine.Shader>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.TextAsset _atlasText = (UnityEngine.TextAsset)translator.GetObject(L, 1, typeof(UnityEngine.TextAsset));
                    UnityEngine.Texture2D[] _textures = (UnityEngine.Texture2D[])translator.GetObject(L, 2, typeof(UnityEngine.Texture2D[]));
                    UnityEngine.Shader _shader = (UnityEngine.Shader)translator.GetObject(L, 3, typeof(UnityEngine.Shader));
                    bool _initialize = LuaAPI.lua_toboolean(L, 4);
                    
                        Spine.Unity.SpineAtlasAsset gen_ret = Spine.Unity.SpineAtlasAsset.CreateRuntimeInstance( _atlasText, _textures, _shader, _initialize );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SpineAtlasAsset.CreateRuntimeInstance!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SpineAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineAtlasAsset)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                Spine.Unity.SpineAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineAtlasAsset)translator.FastGetCSObj(L, 1);
            
            
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SpineAtlasAsset.GetAtlas!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GenerateMesh(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SpineAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineAtlasAsset)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Mesh>(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.Mesh _mesh = (UnityEngine.Mesh)translator.GetObject(L, 3, typeof(UnityEngine.Mesh));
                    UnityEngine.Material _material;
                    float _scale = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        UnityEngine.Mesh gen_ret = gen_to_be_invoked.GenerateMesh( _name, _mesh, out _material, _scale );
                        translator.Push(L, gen_ret);
                    translator.Push(L, _material);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Mesh>(L, 3)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.Mesh _mesh = (UnityEngine.Mesh)translator.GetObject(L, 3, typeof(UnityEngine.Mesh));
                    UnityEngine.Material _material;
                    
                        UnityEngine.Mesh gen_ret = gen_to_be_invoked.GenerateMesh( _name, _mesh, out _material );
                        translator.Push(L, gen_ret);
                    translator.Push(L, _material);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SpineAtlasAsset.GenerateMesh!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsLoaded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineAtlasAsset)translator.FastGetCSObj(L, 1);
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
			
                Spine.Unity.SpineAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineAtlasAsset)translator.FastGetCSObj(L, 1);
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
			
                Spine.Unity.SpineAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineAtlasAsset)translator.FastGetCSObj(L, 1);
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
			
                Spine.Unity.SpineAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineAtlasAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PrimaryMaterial);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_atlasFile(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineAtlasAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.atlasFile);
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
			
                Spine.Unity.SpineAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineAtlasAsset)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.materials);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_customTextureLoader(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineAtlasAsset)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.customTextureLoader);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_atlasFile(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineAtlasAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.atlasFile = (UnityEngine.TextAsset)translator.GetObject(L, 2, typeof(UnityEngine.TextAsset));
            
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
			
                Spine.Unity.SpineAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineAtlasAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.materials = (UnityEngine.Material[])translator.GetObject(L, 2, typeof(UnityEngine.Material[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_customTextureLoader(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SpineAtlasAsset gen_to_be_invoked = (Spine.Unity.SpineAtlasAsset)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.customTextureLoader = (Spine.TextureLoader)translator.GetObject(L, 2, typeof(Spine.TextureLoader));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
