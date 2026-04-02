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
    public class HonorRuntimeCameraOutlineBufferWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Honor.Runtime.CameraOutlineBuffer);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 21, 21);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnPreRender", _m_OnPreRender);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdateMaterialsPublicProperties", _m_UpdateMaterialsPublicProperties);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddOutline", _m_AddOutline);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveOutline", _m_RemoveOutline);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "LineThickness", _g_get_LineThickness);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LineIntensity", _g_get_LineIntensity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FillAmount", _g_get_FillAmount);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LineColor0", _g_get_LineColor0);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LineColor1", _g_get_LineColor1);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LineColor2", _g_get_LineColor2);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AdditiveRendering", _g_get_AdditiveRendering);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BackfaceCulling", _g_get_BackfaceCulling);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FillColor", _g_get_FillColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseFillColor", _g_get_UseFillColor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CornerOutlines", _g_get_CornerOutlines);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AddLinesBetweenColors", _g_get_AddLinesBetweenColors);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ScaleWithScreenSize", _g_get_ScaleWithScreenSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AlphaCutoff", _g_get_AlphaCutoff);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FlipY", _g_get_FlipY);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SourceCamera", _g_get_SourceCamera);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AutoEnableOutlines", _g_get_AutoEnableOutlines);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OutlineCamera", _g_get_OutlineCamera);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OutlineShaderMaterial", _g_get_OutlineShaderMaterial);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RenderTexture", _g_get_RenderTexture);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ExtraRenderTexture", _g_get_ExtraRenderTexture);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "LineThickness", _s_set_LineThickness);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LineIntensity", _s_set_LineIntensity);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FillAmount", _s_set_FillAmount);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LineColor0", _s_set_LineColor0);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LineColor1", _s_set_LineColor1);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LineColor2", _s_set_LineColor2);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AdditiveRendering", _s_set_AdditiveRendering);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BackfaceCulling", _s_set_BackfaceCulling);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FillColor", _s_set_FillColor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseFillColor", _s_set_UseFillColor);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CornerOutlines", _s_set_CornerOutlines);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AddLinesBetweenColors", _s_set_AddLinesBetweenColors);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ScaleWithScreenSize", _s_set_ScaleWithScreenSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AlphaCutoff", _s_set_AlphaCutoff);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FlipY", _s_set_FlipY);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SourceCamera", _s_set_SourceCamera);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AutoEnableOutlines", _s_set_AutoEnableOutlines);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OutlineCamera", _s_set_OutlineCamera);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "OutlineShaderMaterial", _s_set_OutlineShaderMaterial);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RenderTexture", _s_set_RenderTexture);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ExtraRenderTexture", _s_set_ExtraRenderTexture);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 0);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Instance", _g_get_Instance);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Honor.Runtime.CameraOutlineBuffer gen_ret = new Honor.Runtime.CameraOutlineBuffer();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Honor.Runtime.CameraOutlineBuffer constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnPreRender(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnPreRender(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateMaterialsPublicProperties(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.UpdateMaterialsPublicProperties(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddOutline(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.ObjectOutline _outline = (Honor.Runtime.ObjectOutline)translator.GetObject(L, 2, typeof(Honor.Runtime.ObjectOutline));
                    
                    gen_to_be_invoked.AddOutline( _outline );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveOutline(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Honor.Runtime.ObjectOutline _outline = (Honor.Runtime.ObjectOutline)translator.GetObject(L, 2, typeof(Honor.Runtime.ObjectOutline));
                    
                    gen_to_be_invoked.RemoveOutline( _outline );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Honor.Runtime.CameraOutlineBuffer.Instance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LineThickness(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.LineThickness);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LineIntensity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.LineIntensity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FillAmount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.FillAmount);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LineColor0(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.LineColor0);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LineColor1(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.LineColor1);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LineColor2(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.LineColor2);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AdditiveRendering(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.AdditiveRendering);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BackfaceCulling(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.BackfaceCulling);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FillColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineColor(L, gen_to_be_invoked.FillColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseFillColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseFillColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CornerOutlines(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.CornerOutlines);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AddLinesBetweenColors(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.AddLinesBetweenColors);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ScaleWithScreenSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.ScaleWithScreenSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AlphaCutoff(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.AlphaCutoff);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FlipY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.FlipY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SourceCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SourceCamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AutoEnableOutlines(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.AutoEnableOutlines);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OutlineCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OutlineCamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OutlineShaderMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.OutlineShaderMaterial);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RenderTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.RenderTexture);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ExtraRenderTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ExtraRenderTexture);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LineThickness(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LineThickness = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LineIntensity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LineIntensity = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FillAmount(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FillAmount = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LineColor0(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.LineColor0 = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LineColor1(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.LineColor1 = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LineColor2(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.LineColor2 = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AdditiveRendering(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AdditiveRendering = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BackfaceCulling(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BackfaceCulling = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FillColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                UnityEngine.Color gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.FillColor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseFillColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseFillColor = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CornerOutlines(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CornerOutlines = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AddLinesBetweenColors(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AddLinesBetweenColors = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ScaleWithScreenSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ScaleWithScreenSize = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AlphaCutoff(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AlphaCutoff = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FlipY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FlipY = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SourceCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SourceCamera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AutoEnableOutlines(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AutoEnableOutlines = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OutlineCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OutlineCamera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_OutlineShaderMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.OutlineShaderMaterial = (UnityEngine.Material)translator.GetObject(L, 2, typeof(UnityEngine.Material));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RenderTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RenderTexture = (UnityEngine.RenderTexture)translator.GetObject(L, 2, typeof(UnityEngine.RenderTexture));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ExtraRenderTexture(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Honor.Runtime.CameraOutlineBuffer gen_to_be_invoked = (Honor.Runtime.CameraOutlineBuffer)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ExtraRenderTexture = (UnityEngine.RenderTexture)translator.GetObject(L, 2, typeof(UnityEngine.RenderTexture));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
