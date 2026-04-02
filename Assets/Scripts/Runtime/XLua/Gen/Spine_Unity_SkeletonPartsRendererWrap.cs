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
    public class SpineUnitySkeletonPartsRendererWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Spine.Unity.SkeletonPartsRenderer);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 3, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearMesh", _m_ClearMesh);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RenderParts", _m_RenderParts);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetPropertyBlock", _m_SetPropertyBlock);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnMeshAndMaterialsUpdated", _e_OnMeshAndMaterialsUpdated);
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "MeshGenerator", _g_get_MeshGenerator);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MeshRenderer", _g_get_MeshRenderer);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MeshFilter", _g_get_MeshFilter);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "NewPartsRendererGameObject", _m_NewPartsRendererGameObject_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Spine.Unity.SkeletonPartsRenderer gen_ret = new Spine.Unity.SkeletonPartsRenderer();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonPartsRenderer constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearMesh(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonPartsRenderer gen_to_be_invoked = (Spine.Unity.SkeletonPartsRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearMesh(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RenderParts(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonPartsRenderer gen_to_be_invoked = (Spine.Unity.SkeletonPartsRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Spine.ExposedList<Spine.Unity.SubmeshInstruction> _instructions = (Spine.ExposedList<Spine.Unity.SubmeshInstruction>)translator.GetObject(L, 2, typeof(Spine.ExposedList<Spine.Unity.SubmeshInstruction>));
                    int _startSubmesh = LuaAPI.xlua_tointeger(L, 3);
                    int _endSubmesh = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.RenderParts( _instructions, _startSubmesh, _endSubmesh );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetPropertyBlock(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonPartsRenderer gen_to_be_invoked = (Spine.Unity.SkeletonPartsRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.MaterialPropertyBlock _block = (UnityEngine.MaterialPropertyBlock)translator.GetObject(L, 2, typeof(UnityEngine.MaterialPropertyBlock));
                    
                    gen_to_be_invoked.SetPropertyBlock( _block );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NewPartsRendererGameObject_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.Transform>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    string _name = LuaAPI.lua_tostring(L, 2);
                    int _sortingOrder = LuaAPI.xlua_tointeger(L, 3);
                    
                        Spine.Unity.SkeletonPartsRenderer gen_ret = Spine.Unity.SkeletonPartsRenderer.NewPartsRendererGameObject( _parent, _name, _sortingOrder );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Transform>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                        Spine.Unity.SkeletonPartsRenderer gen_ret = Spine.Unity.SkeletonPartsRenderer.NewPartsRendererGameObject( _parent, _name );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonPartsRenderer.NewPartsRendererGameObject!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MeshGenerator(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonPartsRenderer gen_to_be_invoked = (Spine.Unity.SkeletonPartsRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MeshGenerator);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MeshRenderer(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonPartsRenderer gen_to_be_invoked = (Spine.Unity.SkeletonPartsRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MeshRenderer);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MeshFilter(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonPartsRenderer gen_to_be_invoked = (Spine.Unity.SkeletonPartsRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MeshFilter);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnMeshAndMaterialsUpdated(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Spine.Unity.SkeletonPartsRenderer gen_to_be_invoked = (Spine.Unity.SkeletonPartsRenderer)translator.FastGetCSObj(L, 1);
                Spine.Unity.SkeletonPartsRenderer.SkeletonPartsRendererDelegate gen_delegate = translator.GetDelegate<Spine.Unity.SkeletonPartsRenderer.SkeletonPartsRendererDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need Spine.Unity.SkeletonPartsRenderer.SkeletonPartsRendererDelegate!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnMeshAndMaterialsUpdated += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnMeshAndMaterialsUpdated -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonPartsRenderer.OnMeshAndMaterialsUpdated!");
            return 0;
        }
        
		
		
    }
}
