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
    public class SpineUnityWaitForSpineAnimationWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Spine.Unity.WaitForSpineAnimation);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NowWaitFor", _m_NowWaitFor);
			
			
			
			
			
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
				if(LuaAPI.lua_gettop(L) == 3 && translator.Assignable<Spine.TrackEntry>(L, 2) && translator.Assignable<Spine.Unity.WaitForSpineAnimation.AnimationEventTypes>(L, 3))
				{
					Spine.TrackEntry _trackEntry = (Spine.TrackEntry)translator.GetObject(L, 2, typeof(Spine.TrackEntry));
					Spine.Unity.WaitForSpineAnimation.AnimationEventTypes _eventsToWaitFor;translator.Get(L, 3, out _eventsToWaitFor);
					
					Spine.Unity.WaitForSpineAnimation gen_ret = new Spine.Unity.WaitForSpineAnimation(_trackEntry, _eventsToWaitFor);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Spine.Unity.WaitForSpineAnimation constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NowWaitFor(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Spine.Unity.WaitForSpineAnimation gen_to_be_invoked = (Spine.Unity.WaitForSpineAnimation)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Spine.TrackEntry _trackEntry = (Spine.TrackEntry)translator.GetObject(L, 2, typeof(Spine.TrackEntry));
                    Spine.Unity.WaitForSpineAnimation.AnimationEventTypes _eventsToWaitFor;translator.Get(L, 3, out _eventsToWaitFor);
                    
                        Spine.Unity.WaitForSpineAnimation gen_ret = gen_to_be_invoked.NowWaitFor( _trackEntry, _eventsToWaitFor );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
