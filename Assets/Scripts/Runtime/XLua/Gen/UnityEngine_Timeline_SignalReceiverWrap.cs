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
    public class UnityEngineTimelineSignalReceiverWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityEngine.Timeline.SignalReceiver);
			Utils.BeginObjectRegister(type, L, translator, 0, 12, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnNotify", _m_OnNotify);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddReaction", _m_AddReaction);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddEmptyReaction", _m_AddEmptyReaction);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Remove", _m_Remove);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRegisteredSignals", _m_GetRegisteredSignals);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetReaction", _m_GetReaction);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Count", _m_Count);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeSignalAtIndex", _m_ChangeSignalAtIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveAtIndex", _m_RemoveAtIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeReactionAtIndex", _m_ChangeReactionAtIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetReactionAtIndex", _m_GetReactionAtIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSignalAssetAtIndex", _m_GetSignalAssetAtIndex);
			
			
			
			
			
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
					
					UnityEngine.Timeline.SignalReceiver gen_ret = new UnityEngine.Timeline.SignalReceiver();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityEngine.Timeline.SignalReceiver constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnNotify(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.SignalReceiver gen_to_be_invoked = (UnityEngine.Timeline.SignalReceiver)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Playables.Playable _origin;translator.Get(L, 2, out _origin);
                    UnityEngine.Playables.INotification _notification = (UnityEngine.Playables.INotification)translator.GetObject(L, 3, typeof(UnityEngine.Playables.INotification));
                    object _context = translator.GetObject(L, 4, typeof(object));
                    
                    gen_to_be_invoked.OnNotify( _origin, _notification, _context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddReaction(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.SignalReceiver gen_to_be_invoked = (UnityEngine.Timeline.SignalReceiver)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Timeline.SignalAsset _asset = (UnityEngine.Timeline.SignalAsset)translator.GetObject(L, 2, typeof(UnityEngine.Timeline.SignalAsset));
                    UnityEngine.Events.UnityEvent _reaction = (UnityEngine.Events.UnityEvent)translator.GetObject(L, 3, typeof(UnityEngine.Events.UnityEvent));
                    
                    gen_to_be_invoked.AddReaction( _asset, _reaction );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEmptyReaction(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.SignalReceiver gen_to_be_invoked = (UnityEngine.Timeline.SignalReceiver)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Events.UnityEvent _reaction = (UnityEngine.Events.UnityEvent)translator.GetObject(L, 2, typeof(UnityEngine.Events.UnityEvent));
                    
                        int gen_ret = gen_to_be_invoked.AddEmptyReaction( _reaction );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Remove(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.SignalReceiver gen_to_be_invoked = (UnityEngine.Timeline.SignalReceiver)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Timeline.SignalAsset _asset = (UnityEngine.Timeline.SignalAsset)translator.GetObject(L, 2, typeof(UnityEngine.Timeline.SignalAsset));
                    
                    gen_to_be_invoked.Remove( _asset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRegisteredSignals(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.SignalReceiver gen_to_be_invoked = (UnityEngine.Timeline.SignalReceiver)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.Collections.Generic.IEnumerable<UnityEngine.Timeline.SignalAsset> gen_ret = gen_to_be_invoked.GetRegisteredSignals(  );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetReaction(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.SignalReceiver gen_to_be_invoked = (UnityEngine.Timeline.SignalReceiver)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Timeline.SignalAsset _key = (UnityEngine.Timeline.SignalAsset)translator.GetObject(L, 2, typeof(UnityEngine.Timeline.SignalAsset));
                    
                        UnityEngine.Events.UnityEvent gen_ret = gen_to_be_invoked.GetReaction( _key );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Count(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.SignalReceiver gen_to_be_invoked = (UnityEngine.Timeline.SignalReceiver)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        int gen_ret = gen_to_be_invoked.Count(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeSignalAtIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.SignalReceiver gen_to_be_invoked = (UnityEngine.Timeline.SignalReceiver)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _idx = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Timeline.SignalAsset _newKey = (UnityEngine.Timeline.SignalAsset)translator.GetObject(L, 3, typeof(UnityEngine.Timeline.SignalAsset));
                    
                    gen_to_be_invoked.ChangeSignalAtIndex( _idx, _newKey );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveAtIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.SignalReceiver gen_to_be_invoked = (UnityEngine.Timeline.SignalReceiver)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _idx = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.RemoveAtIndex( _idx );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeReactionAtIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.SignalReceiver gen_to_be_invoked = (UnityEngine.Timeline.SignalReceiver)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _idx = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Events.UnityEvent _reaction = (UnityEngine.Events.UnityEvent)translator.GetObject(L, 3, typeof(UnityEngine.Events.UnityEvent));
                    
                    gen_to_be_invoked.ChangeReactionAtIndex( _idx, _reaction );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetReactionAtIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.SignalReceiver gen_to_be_invoked = (UnityEngine.Timeline.SignalReceiver)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _idx = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.Events.UnityEvent gen_ret = gen_to_be_invoked.GetReactionAtIndex( _idx );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSignalAssetAtIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityEngine.Timeline.SignalReceiver gen_to_be_invoked = (UnityEngine.Timeline.SignalReceiver)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _idx = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.Timeline.SignalAsset gen_ret = gen_to_be_invoked.GetSignalAssetAtIndex( _idx );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
