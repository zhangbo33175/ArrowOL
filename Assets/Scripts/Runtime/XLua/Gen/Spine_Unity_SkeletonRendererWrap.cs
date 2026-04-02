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
    public class SpineUnitySkeletonRendererWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Spine.Unity.SkeletonRenderer);
			Utils.BeginObjectRegister(type, L, translator, 0, 15, 27, 22);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetMeshSettings", _m_SetMeshSettings);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Awake", _m_Awake);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearState", _m_ClearState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EnsureMeshGeneratorCapacity", _m_EnsureMeshGeneratorCapacity);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Initialize", _m_Initialize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LateUpdate", _m_LateUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LateUpdateMesh", _m_LateUpdateMesh);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBecameVisible", _m_OnBecameVisible);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBecameInvisible", _m_OnBecameInvisible);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FindAndApplySeparatorSlots", _m_FindAndApplySeparatorSlots);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReapplySeparatorSlotNames", _m_ReapplySeparatorSlotNames);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GenerateMeshOverride", _e_GenerateMeshOverride);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPostProcessVertices", _e_OnPostProcessVertices);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnRebuild", _e_OnRebuild);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnMeshAndMaterialsUpdated", _e_OnMeshAndMaterialsUpdated);
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "UpdateMode", _g_get_UpdateMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CustomMaterialOverride", _g_get_CustomMaterialOverride);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CustomSlotMaterials", _g_get_CustomSlotMaterials);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Skeleton", _g_get_Skeleton);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SkeletonDataAsset", _g_get_SkeletonDataAsset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "skeletonDataAsset", _g_get_skeletonDataAsset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "initialSkinName", _g_get_initialSkinName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "initialFlipX", _g_get_initialFlipX);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "initialFlipY", _g_get_initialFlipY);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "updateWhenInvisible", _g_get_updateWhenInvisible);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "separatorSlotNames", _g_get_separatorSlotNames);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "separatorSlots", _g_get_separatorSlots);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "zSpacing", _g_get_zSpacing);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "useClipping", _g_get_useClipping);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "immutableTriangles", _g_get_immutableTriangles);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pmaVertexColors", _g_get_pmaVertexColors);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "clearStateOnDisable", _g_get_clearStateOnDisable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "tintBlack", _g_get_tintBlack);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "singleSubmesh", _g_get_singleSubmesh);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "fixDrawOrder", _g_get_fixDrawOrder);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "addNormals", _g_get_addNormals);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "calculateTangents", _g_get_calculateTangents);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "maskInteraction", _g_get_maskInteraction);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "maskMaterials", _g_get_maskMaterials);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "disableRenderingOnOverride", _g_get_disableRenderingOnOverride);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "valid", _g_get_valid);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "skeleton", _g_get_skeleton);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "UpdateMode", _s_set_UpdateMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "skeletonDataAsset", _s_set_skeletonDataAsset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "initialSkinName", _s_set_initialSkinName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "initialFlipX", _s_set_initialFlipX);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "initialFlipY", _s_set_initialFlipY);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "updateWhenInvisible", _s_set_updateWhenInvisible);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "separatorSlotNames", _s_set_separatorSlotNames);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "zSpacing", _s_set_zSpacing);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "useClipping", _s_set_useClipping);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "immutableTriangles", _s_set_immutableTriangles);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pmaVertexColors", _s_set_pmaVertexColors);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "clearStateOnDisable", _s_set_clearStateOnDisable);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "tintBlack", _s_set_tintBlack);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "singleSubmesh", _s_set_singleSubmesh);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "fixDrawOrder", _s_set_fixDrawOrder);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "addNormals", _s_set_addNormals);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "calculateTangents", _s_set_calculateTangents);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "maskInteraction", _s_set_maskInteraction);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "maskMaterials", _s_set_maskMaterials);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "disableRenderingOnOverride", _s_set_disableRenderingOnOverride);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "valid", _s_set_valid);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "skeleton", _s_set_skeleton);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 6, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "STENCIL_COMP_PARAM_ID", Spine.Unity.SkeletonRenderer.STENCIL_COMP_PARAM_ID);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "STENCIL_COMP_MASKINTERACTION_NONE", Spine.Unity.SkeletonRenderer.STENCIL_COMP_MASKINTERACTION_NONE);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "STENCIL_COMP_MASKINTERACTION_VISIBLE_INSIDE", Spine.Unity.SkeletonRenderer.STENCIL_COMP_MASKINTERACTION_VISIBLE_INSIDE);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "STENCIL_COMP_MASKINTERACTION_VISIBLE_OUTSIDE", Spine.Unity.SkeletonRenderer.STENCIL_COMP_MASKINTERACTION_VISIBLE_OUTSIDE);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SUBMESH_DUMMY_PARAM_ID", Spine.Unity.SkeletonRenderer.SUBMESH_DUMMY_PARAM_ID);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Spine.Unity.SkeletonRenderer gen_ret = new Spine.Unity.SkeletonRenderer();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonRenderer constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetMeshSettings(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Spine.Unity.MeshGenerator.Settings _settings;translator.Get(L, 2, out _settings);
                    
                    gen_to_be_invoked.SetMeshSettings( _settings );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Awake(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Awake(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearState(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EnsureMeshGeneratorCapacity(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _minimumVertexCount = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.EnsureMeshGeneratorCapacity( _minimumVertexCount );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Initialize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    bool _overwrite = LuaAPI.lua_toboolean(L, 2);
                    bool _quiet = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.Initialize( _overwrite, _quiet );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _overwrite = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.Initialize( _overwrite );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonRenderer.Initialize!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LateUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LateUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LateUpdateMesh(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LateUpdateMesh(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnBecameVisible(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnBecameVisible(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnBecameInvisible(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnBecameInvisible(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FindAndApplySeparatorSlots(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    string _startsWith = LuaAPI.lua_tostring(L, 2);
                    bool _clearExistingSeparators = LuaAPI.lua_toboolean(L, 3);
                    bool _updateStringArray = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.FindAndApplySeparatorSlots( _startsWith, _clearExistingSeparators, _updateStringArray );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    string _startsWith = LuaAPI.lua_tostring(L, 2);
                    bool _clearExistingSeparators = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.FindAndApplySeparatorSlots( _startsWith, _clearExistingSeparators );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _startsWith = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.FindAndApplySeparatorSlots( _startsWith );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<System.Func<string, bool>>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    System.Func<string, bool> _slotNamePredicate = translator.GetDelegate<System.Func<string, bool>>(L, 2);
                    bool _clearExistingSeparators = LuaAPI.lua_toboolean(L, 3);
                    bool _updateStringArray = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.FindAndApplySeparatorSlots( _slotNamePredicate, _clearExistingSeparators, _updateStringArray );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<System.Func<string, bool>>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    System.Func<string, bool> _slotNamePredicate = translator.GetDelegate<System.Func<string, bool>>(L, 2);
                    bool _clearExistingSeparators = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.FindAndApplySeparatorSlots( _slotNamePredicate, _clearExistingSeparators );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<System.Func<string, bool>>(L, 2)) 
                {
                    System.Func<string, bool> _slotNamePredicate = translator.GetDelegate<System.Func<string, bool>>(L, 2);
                    
                    gen_to_be_invoked.FindAndApplySeparatorSlots( _slotNamePredicate );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonRenderer.FindAndApplySeparatorSlots!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReapplySeparatorSlotNames(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ReapplySeparatorSlotNames(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UpdateMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                translator.PushSpineUnityUpdateMode(L, gen_to_be_invoked.UpdateMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CustomMaterialOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.CustomMaterialOverride);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CustomSlotMaterials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.CustomSlotMaterials);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Skeleton(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Skeleton);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SkeletonDataAsset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SkeletonDataAsset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_skeletonDataAsset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.skeletonDataAsset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_initialSkinName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.initialSkinName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_initialFlipX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.initialFlipX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_initialFlipY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.initialFlipY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_updateWhenInvisible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                translator.PushSpineUnityUpdateMode(L, gen_to_be_invoked.updateWhenInvisible);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_separatorSlotNames(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.separatorSlotNames);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_separatorSlots(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.separatorSlots);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_zSpacing(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.zSpacing);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_useClipping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.useClipping);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_immutableTriangles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.immutableTriangles);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pmaVertexColors(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.pmaVertexColors);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_clearStateOnDisable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.clearStateOnDisable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_tintBlack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.tintBlack);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_singleSubmesh(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.singleSubmesh);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_fixDrawOrder(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.fixDrawOrder);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_addNormals(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.addNormals);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_calculateTangents(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.calculateTangents);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maskInteraction(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.maskInteraction);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maskMaterials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.maskMaterials);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_disableRenderingOnOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.disableRenderingOnOverride);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_valid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.valid);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_skeleton(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.skeleton);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UpdateMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                Spine.Unity.UpdateMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.UpdateMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_skeletonDataAsset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.skeletonDataAsset = (Spine.Unity.SkeletonDataAsset)translator.GetObject(L, 2, typeof(Spine.Unity.SkeletonDataAsset));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_initialSkinName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.initialSkinName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_initialFlipX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.initialFlipX = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_initialFlipY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.initialFlipY = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_updateWhenInvisible(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                Spine.Unity.UpdateMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.updateWhenInvisible = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_separatorSlotNames(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.separatorSlotNames = (string[])translator.GetObject(L, 2, typeof(string[]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_zSpacing(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.zSpacing = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_useClipping(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.useClipping = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_immutableTriangles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.immutableTriangles = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pmaVertexColors(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.pmaVertexColors = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_clearStateOnDisable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.clearStateOnDisable = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_tintBlack(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.tintBlack = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_singleSubmesh(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.singleSubmesh = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_fixDrawOrder(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.fixDrawOrder = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_addNormals(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.addNormals = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_calculateTangents(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.calculateTangents = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_maskInteraction(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                UnityEngine.SpriteMaskInteraction gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.maskInteraction = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_maskMaterials(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.maskMaterials = (Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials)translator.GetObject(L, 2, typeof(Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_disableRenderingOnOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.disableRenderingOnOverride = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_valid(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.valid = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_skeleton(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.skeleton = (Spine.Skeleton)translator.GetObject(L, 2, typeof(Spine.Skeleton));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_GenerateMeshOverride(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                Spine.Unity.SkeletonRenderer.InstructionDelegate gen_delegate = translator.GetDelegate<Spine.Unity.SkeletonRenderer.InstructionDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need Spine.Unity.SkeletonRenderer.InstructionDelegate!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.GenerateMeshOverride += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.GenerateMeshOverride -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonRenderer.GenerateMeshOverride!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnPostProcessVertices(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                Spine.Unity.MeshGeneratorDelegate gen_delegate = translator.GetDelegate<Spine.Unity.MeshGeneratorDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need Spine.Unity.MeshGeneratorDelegate!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnPostProcessVertices += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnPostProcessVertices -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonRenderer.OnPostProcessVertices!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnRebuild(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                Spine.Unity.SkeletonRenderer.SkeletonRendererDelegate gen_delegate = translator.GetDelegate<Spine.Unity.SkeletonRenderer.SkeletonRendererDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need Spine.Unity.SkeletonRenderer.SkeletonRendererDelegate!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnRebuild += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnRebuild -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonRenderer.OnRebuild!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnMeshAndMaterialsUpdated(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Spine.Unity.SkeletonRenderer gen_to_be_invoked = (Spine.Unity.SkeletonRenderer)translator.FastGetCSObj(L, 1);
                Spine.Unity.SkeletonRenderer.SkeletonRendererDelegate gen_delegate = translator.GetDelegate<Spine.Unity.SkeletonRenderer.SkeletonRendererDelegate>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need Spine.Unity.SkeletonRenderer.SkeletonRendererDelegate!");
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
			LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.SkeletonRenderer.OnMeshAndMaterialsUpdated!");
            return 0;
        }
        
		
		
    }
}
