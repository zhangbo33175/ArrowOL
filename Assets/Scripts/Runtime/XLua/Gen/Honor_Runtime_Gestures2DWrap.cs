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
    public class HonorRuntimeGestures2DWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.Gestures2D);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 51, 32);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetSwipe", _m_ResetSwipe);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetPinch", _m_ResetPinch);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "TouchBeginCallbacks", _g_get_TouchBeginCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TouchDownCallbacks", _g_get_TouchDownCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TouchEndCallbacks", _g_get_TouchEndCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TouchesBeginCallbacks", _g_get_TouchesBeginCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TouchesDownCallbacks", _g_get_TouchesDownCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TouchesEndCallbacks", _g_get_TouchesEndCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SwipeBeginCallbacks", _g_get_SwipeBeginCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SwipeCallbacks", _g_get_SwipeCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SwipeEndCallbacks", _g_get_SwipeEndCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SwipeStableCallbacks", _g_get_SwipeStableCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PinchCallbacks", _g_get_PinchCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PinchStableCallbacks", _g_get_PinchStableCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectedObjCallbacks", _g_get_SelectedObjCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UpdateSelectedObjCallbacks", _g_get_UpdateSelectedObjCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnselectedObjCallbacks", _g_get_UnselectedObjCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectedObjDragBeginCallbacks", _g_get_SelectedObjDragBeginCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectedObjDragCallbacks", _g_get_SelectedObjDragCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectedObjDragEndCallbacks", _g_get_SelectedObjDragEndCallbacks);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SceneCameraIndex", _g_get_SceneCameraIndex);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsOrthographic", _g_get_IsOrthographic);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GestureCameraDistance", _g_get_GestureCameraDistance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EnableSwitch", _g_get_EnableSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SwipeSwitch", _g_get_SwipeSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PinchSwitch", _g_get_PinchSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MouseWheelPinchSwitch", _g_get_MouseWheelPinchSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectSwitch", _g_get_SelectSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DragSwitch", _g_get_DragSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SpaceCenterPosition", _g_get_SpaceCenterPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SpaceHorizontalLength", _g_get_SpaceHorizontalLength);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SpaceVerticalLength", _g_get_SpaceVerticalLength);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SpaceHorizontalEdgeMoveElasticLength", _g_get_SpaceHorizontalEdgeMoveElasticLength);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SpaceVerticalEdgeMoveElasticLength", _g_get_SpaceVerticalEdgeMoveElasticLength);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PinchRatio", _g_get_PinchRatio);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PinchMinScale", _g_get_PinchMinScale);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PinchMaxScale", _g_get_PinchMaxScale);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MouseWheelPinchOffset", _g_get_MouseWheelPinchOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SpaceEdgeScaleElasticValue", _g_get_SpaceEdgeScaleElasticValue);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SafeTimeOnGestureOver", _g_get_SafeTimeOnGestureOver);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectHoldMode", _g_get_SelectHoldMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectReboundInSelectHoldMode", _g_get_SelectReboundInSelectHoldMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectReboundAfterDragEndInSelectHoldMode", _g_get_SelectReboundAfterDragEndInSelectHoldMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PressTimeOfSelectingObj", _g_get_PressTimeOfSelectingObj);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ColliderDetectMode", _g_get_ColliderDetectMode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PosYSortSelectSwitch", _g_get_PosYSortSelectSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RayDistanceSortSelectSwitch", _g_get_RayDistanceSortSelectSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SortingLayerOrderSortSelectSwitch", _g_get_SortingLayerOrderSortSelectSwitch);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SelectingTypes", _g_get_SelectingTypes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FindSelectingTypesOnSelf", _g_get_FindSelectingTypesOnSelf);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FindSelectingTypesOnParent", _g_get_FindSelectingTypesOnParent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FindSelectingTypesOnChildren", _g_get_FindSelectingTypesOnChildren);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SceneCamera", _g_get_SceneCamera);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "SceneCameraIndex", _s_set_SceneCameraIndex);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "IsOrthographic", _s_set_IsOrthographic);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GestureCameraDistance", _s_set_GestureCameraDistance);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EnableSwitch", _s_set_EnableSwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SwipeSwitch", _s_set_SwipeSwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PinchSwitch", _s_set_PinchSwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MouseWheelPinchSwitch", _s_set_MouseWheelPinchSwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SelectSwitch", _s_set_SelectSwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DragSwitch", _s_set_DragSwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SpaceCenterPosition", _s_set_SpaceCenterPosition);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SpaceHorizontalLength", _s_set_SpaceHorizontalLength);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SpaceVerticalLength", _s_set_SpaceVerticalLength);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SpaceHorizontalEdgeMoveElasticLength", _s_set_SpaceHorizontalEdgeMoveElasticLength);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SpaceVerticalEdgeMoveElasticLength", _s_set_SpaceVerticalEdgeMoveElasticLength);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PinchRatio", _s_set_PinchRatio);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PinchMinScale", _s_set_PinchMinScale);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PinchMaxScale", _s_set_PinchMaxScale);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MouseWheelPinchOffset", _s_set_MouseWheelPinchOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SpaceEdgeScaleElasticValue", _s_set_SpaceEdgeScaleElasticValue);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SafeTimeOnGestureOver", _s_set_SafeTimeOnGestureOver);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SelectHoldMode", _s_set_SelectHoldMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SelectReboundInSelectHoldMode", _s_set_SelectReboundInSelectHoldMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SelectReboundAfterDragEndInSelectHoldMode", _s_set_SelectReboundAfterDragEndInSelectHoldMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PressTimeOfSelectingObj", _s_set_PressTimeOfSelectingObj);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ColliderDetectMode", _s_set_ColliderDetectMode);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PosYSortSelectSwitch", _s_set_PosYSortSelectSwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RayDistanceSortSelectSwitch", _s_set_RayDistanceSortSelectSwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SortingLayerOrderSortSelectSwitch", _s_set_SortingLayerOrderSortSelectSwitch);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SelectingTypes", _s_set_SelectingTypes);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FindSelectingTypesOnSelf", _s_set_FindSelectingTypesOnSelf);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FindSelectingTypesOnParent", _s_set_FindSelectingTypesOnParent);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FindSelectingTypesOnChildren", _s_set_FindSelectingTypesOnChildren);
            
			
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
					
					Honor.Runtime.Gestures2D gen_ret = new Honor.Runtime.Gestures2D();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.Gestures2D constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetSwipe(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetSwipe(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetPinch(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ResetPinch(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TouchBeginCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TouchBeginCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TouchDownCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TouchDownCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TouchEndCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TouchEndCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TouchesBeginCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TouchesBeginCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TouchesDownCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TouchesDownCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TouchesEndCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.TouchesEndCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SwipeBeginCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SwipeBeginCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SwipeCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SwipeCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SwipeEndCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SwipeEndCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SwipeStableCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SwipeStableCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PinchCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PinchCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PinchStableCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PinchStableCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectedObjCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SelectedObjCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UpdateSelectedObjCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UpdateSelectedObjCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnselectedObjCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UnselectedObjCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectedObjDragBeginCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SelectedObjDragBeginCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectedObjDragCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SelectedObjDragCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectedObjDragEndCallbacks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SelectedObjDragEndCallbacks);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SceneCameraIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SceneCameraIndex);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsOrthographic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsOrthographic);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GestureCameraDistance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.GestureCameraDistance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EnableSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.EnableSwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SwipeSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.SwipeSwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PinchSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.PinchSwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MouseWheelPinchSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.MouseWheelPinchSwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.SelectSwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DragSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.DragSwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SpaceCenterPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, gen_to_be_invoked.SpaceCenterPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SpaceHorizontalLength(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.SpaceHorizontalLength);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SpaceVerticalLength(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.SpaceVerticalLength);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SpaceHorizontalEdgeMoveElasticLength(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.SpaceHorizontalEdgeMoveElasticLength);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SpaceVerticalEdgeMoveElasticLength(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.SpaceVerticalEdgeMoveElasticLength);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PinchRatio(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.PinchRatio);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PinchMinScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.PinchMinScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PinchMaxScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.PinchMaxScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MouseWheelPinchOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.MouseWheelPinchOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SpaceEdgeScaleElasticValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.SpaceEdgeScaleElasticValue);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SafeTimeOnGestureOver(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.SafeTimeOnGestureOver);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectHoldMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.SelectHoldMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectReboundInSelectHoldMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.SelectReboundInSelectHoldMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectReboundAfterDragEndInSelectHoldMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.SelectReboundAfterDragEndInSelectHoldMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PressTimeOfSelectingObj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.PressTimeOfSelectingObj);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ColliderDetectMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.PushHonorRuntimeGameDefinitionsDimensionMode(L, gen_to_be_invoked.ColliderDetectMode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PosYSortSelectSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.PosYSortSelectSwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RayDistanceSortSelectSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.RayDistanceSortSelectSwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SortingLayerOrderSortSelectSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.SortingLayerOrderSortSelectSwitch);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SelectingTypes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SelectingTypes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FindSelectingTypesOnSelf(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.FindSelectingTypesOnSelf);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FindSelectingTypesOnParent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.FindSelectingTypesOnParent);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FindSelectingTypesOnChildren(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.FindSelectingTypesOnChildren);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SceneCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SceneCamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SceneCameraIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SceneCameraIndex = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_IsOrthographic(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.IsOrthographic = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GestureCameraDistance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GestureCameraDistance = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EnableSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EnableSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SwipeSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SwipeSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PinchSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PinchSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MouseWheelPinchSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MouseWheelPinchSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SelectSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SelectSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DragSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DragSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SpaceCenterPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.SpaceCenterPosition = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SpaceHorizontalLength(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SpaceHorizontalLength = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SpaceVerticalLength(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SpaceVerticalLength = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SpaceHorizontalEdgeMoveElasticLength(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SpaceHorizontalEdgeMoveElasticLength = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SpaceVerticalEdgeMoveElasticLength(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SpaceVerticalEdgeMoveElasticLength = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PinchRatio(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PinchRatio = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PinchMinScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PinchMinScale = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PinchMaxScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PinchMaxScale = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MouseWheelPinchOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MouseWheelPinchOffset = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SpaceEdgeScaleElasticValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SpaceEdgeScaleElasticValue = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SafeTimeOnGestureOver(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SafeTimeOnGestureOver = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SelectHoldMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SelectHoldMode = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SelectReboundInSelectHoldMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SelectReboundInSelectHoldMode = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SelectReboundAfterDragEndInSelectHoldMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SelectReboundAfterDragEndInSelectHoldMode = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PressTimeOfSelectingObj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PressTimeOfSelectingObj = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ColliderDetectMode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                Honor.Runtime.GameDefinitions.DimensionMode gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.ColliderDetectMode = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PosYSortSelectSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PosYSortSelectSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RayDistanceSortSelectSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RayDistanceSortSelectSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SortingLayerOrderSortSelectSwitch(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SortingLayerOrderSortSelectSwitch = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SelectingTypes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SelectingTypes = (System.Collections.Generic.List<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<string>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FindSelectingTypesOnSelf(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FindSelectingTypesOnSelf = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FindSelectingTypesOnParent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FindSelectingTypesOnParent = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FindSelectingTypesOnChildren(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.Gestures2D gen_to_be_invoked = (Honor.Runtime.Gestures2D)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FindSelectingTypesOnChildren = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
