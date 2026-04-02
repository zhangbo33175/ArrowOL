#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;


namespace XLua
{
    public partial class ObjectTranslator
    {
        
        class IniterAdderUnityEngineVector2
        {
            static IniterAdderUnityEngineVector2()
            {
                LuaEnv.AddIniter(Init);
            }
			
			static void Init(LuaEnv luaenv, ObjectTranslator translator)
			{
			
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Vector2>(translator.PushUnityEngineVector2, translator.Get, translator.UpdateUnityEngineVector2);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Vector3>(translator.PushUnityEngineVector3, translator.Get, translator.UpdateUnityEngineVector3);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Vector4>(translator.PushUnityEngineVector4, translator.Get, translator.UpdateUnityEngineVector4);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Color>(translator.PushUnityEngineColor, translator.Get, translator.UpdateUnityEngineColor);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Quaternion>(translator.PushUnityEngineQuaternion, translator.Get, translator.UpdateUnityEngineQuaternion);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Ray>(translator.PushUnityEngineRay, translator.Get, translator.UpdateUnityEngineRay);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Bounds>(translator.PushUnityEngineBounds, translator.Get, translator.UpdateUnityEngineBounds);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Ray2D>(translator.PushUnityEngineRay2D, translator.Get, translator.UpdateUnityEngineRay2D);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.AnimatorUpdateMode>(translator.PushUnityEngineAnimatorUpdateMode, translator.Get, translator.UpdateUnityEngineAnimatorUpdateMode);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Tilemaps.Tile.ColliderType>(translator.PushUnityEngineTilemapsTileColliderType, translator.Get, translator.UpdateUnityEngineTilemapsTileColliderType);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Tilemaps.Tilemap.Orientation>(translator.PushUnityEngineTilemapsTilemapOrientation, translator.Get, translator.UpdateUnityEngineTilemapsTilemapOrientation);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Tilemaps.TileFlags>(translator.PushUnityEngineTilemapsTileFlags, translator.Get, translator.UpdateUnityEngineTilemapsTileFlags);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Tilemaps.TileAnimationFlags>(translator.PushUnityEngineTilemapsTileAnimationFlags, translator.Get, translator.UpdateUnityEngineTilemapsTileAnimationFlags);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Tilemaps.TilemapRenderer.SortOrder>(translator.PushUnityEngineTilemapsTilemapRendererSortOrder, translator.Get, translator.UpdateUnityEngineTilemapsTilemapRendererSortOrder);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Tilemaps.TilemapRenderer.Mode>(translator.PushUnityEngineTilemapsTilemapRendererMode, translator.Get, translator.UpdateUnityEngineTilemapsTilemapRendererMode);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds>(translator.PushUnityEngineTilemapsTilemapRendererDetectChunkCullingBounds, translator.Get, translator.UpdateUnityEngineTilemapsTilemapRendererDetectChunkCullingBounds);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.OriginType>(translator.PushHonorRuntimeOriginType, translator.Get, translator.UpdateHonorRuntimeOriginType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.PrefabDetailType>(translator.PushHonorRuntimePrefabDetailType, translator.Get, translator.UpdateHonorRuntimePrefabDetailType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.PrefabType>(translator.PushHonorRuntimePrefabType, translator.Get, translator.UpdateHonorRuntimePrefabType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.GameEventCmd>(translator.PushHonorRuntimeGameEventCmd, translator.Get, translator.UpdateHonorRuntimeGameEventCmd);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.SnapStatus>(translator.PushHonorRuntimeSnapStatus, translator.Get, translator.UpdateHonorRuntimeSnapStatus);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.ItemCornerEnum>(translator.PushHonorRuntimeItemCornerEnum, translator.Get, translator.UpdateHonorRuntimeItemCornerEnum);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.ListItemArrangeType>(translator.PushHonorRuntimeListItemArrangeType, translator.Get, translator.UpdateHonorRuntimeListItemArrangeType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.GridItemArrangeType>(translator.PushHonorRuntimeGridItemArrangeType, translator.Get, translator.UpdateHonorRuntimeGridItemArrangeType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.GridFixedType>(translator.PushHonorRuntimeGridFixedType, translator.Get, translator.UpdateHonorRuntimeGridFixedType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.PatternType>(translator.PushHonorRuntimePatternType, translator.Get, translator.UpdateHonorRuntimePatternType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.NonePatternType>(translator.PushHonorRuntimeNonePatternType, translator.Get, translator.UpdateHonorRuntimeNonePatternType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.MVVMPatternType>(translator.PushHonorRuntimeMVVMPatternType, translator.Get, translator.UpdateHonorRuntimeMVVMPatternType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.PersistWayType>(translator.PushHonorRuntimePersistWayType, translator.Get, translator.UpdateHonorRuntimePersistWayType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.PlaySoundErrorCode>(translator.PushHonorRuntimePlaySoundErrorCode, translator.Get, translator.UpdateHonorRuntimePlaySoundErrorCode);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.DevicePerformanceLevel>(translator.PushHonorRuntimeDevicePerformanceLevel, translator.Get, translator.UpdateHonorRuntimeDevicePerformanceLevel);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.QualityLevel>(translator.PushHonorRuntimeQualityLevel, translator.Get, translator.UpdateHonorRuntimeQualityLevel);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.UIType>(translator.PushHonorRuntimeUIType, translator.Get, translator.UpdateHonorRuntimeUIType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.VibrateType>(translator.PushHonorRuntimeVibrateType, translator.Get, translator.UpdateHonorRuntimeVibrateType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.GameDefinitions.DimensionMode>(translator.PushHonorRuntimeGameDefinitionsDimensionMode, translator.Get, translator.UpdateHonorRuntimeGameDefinitionsDimensionMode);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.GameDefinitions.AssetType>(translator.PushHonorRuntimeGameDefinitionsAssetType, translator.Get, translator.UpdateHonorRuntimeGameDefinitionsAssetType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.GameDefinitions.PathType>(translator.PushHonorRuntimeGameDefinitionsPathType, translator.Get, translator.UpdateHonorRuntimeGameDefinitionsPathType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.GameDefinitions.DownloadStep>(translator.PushHonorRuntimeGameDefinitionsDownloadStep, translator.Get, translator.UpdateHonorRuntimeGameDefinitionsDownloadStep);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.GameDefinitions.DebugMode>(translator.PushHonorRuntimeGameDefinitionsDebugMode, translator.Get, translator.UpdateHonorRuntimeGameDefinitionsDebugMode);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.GameDefinitions.Language>(translator.PushHonorRuntimeGameDefinitionsLanguage, translator.Get, translator.UpdateHonorRuntimeGameDefinitionsLanguage);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.GameDefinitions.DebugWindowModel>(translator.PushHonorRuntimeGameDefinitionsDebugWindowModel, translator.Get, translator.UpdateHonorRuntimeGameDefinitionsDebugWindowModel);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType>(translator.PushHonorRuntimeAorTextEffectOutlineHorizontalAligmentType, translator.Get, translator.UpdateHonorRuntimeAorTextEffectOutlineHorizontalAligmentType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType>(translator.PushHonorRuntimeAorTextEffectSpacingHorizontalAligmentType, translator.Get, translator.UpdateHonorRuntimeAorTextEffectSpacingHorizontalAligmentType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.LuaBindValue.BindValueType>(translator.PushHonorRuntimeLuaBindValueBindValueType, translator.Get, translator.UpdateHonorRuntimeLuaBindValueBindValueType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.LuaInjection.InjectionType>(translator.PushHonorRuntimeLuaInjectionInjectionType, translator.Get, translator.UpdateHonorRuntimeLuaInjectionInjectionType);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.UIFader.InitState>(translator.PushHonorRuntimeUIFaderInitState, translator.Get, translator.UpdateHonorRuntimeUIFaderInitState);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.Log.LogLevel>(translator.PushHonorRuntimeLogLogLevel, translator.Get, translator.UpdateHonorRuntimeLogLogLevel);
				translator.RegisterPushAndGetAndUpdate<Honor.Runtime.UILauncherLoadingView.LoadingMode>(translator.PushHonorRuntimeUILauncherLoadingViewLoadingMode, translator.Get, translator.UpdateHonorRuntimeUILauncherLoadingViewLoadingMode);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Timeline.MatchTargetFields>(translator.PushUnityEngineTimelineMatchTargetFields, translator.Get, translator.UpdateUnityEngineTimelineMatchTargetFields);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Timeline.TrackOffset>(translator.PushUnityEngineTimelineTrackOffset, translator.Get, translator.UpdateUnityEngineTimelineTrackOffset);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Timeline.ClipCaps>(translator.PushUnityEngineTimelineClipCaps, translator.Get, translator.UpdateUnityEngineTimelineClipCaps);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Timeline.NotificationFlags>(translator.PushUnityEngineTimelineNotificationFlags, translator.Get, translator.UpdateUnityEngineTimelineNotificationFlags);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Timeline.TrackBindingFlags>(translator.PushUnityEngineTimelineTrackBindingFlags, translator.Get, translator.UpdateUnityEngineTimelineTrackBindingFlags);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Timeline.StandardFrameRates>(translator.PushUnityEngineTimelineStandardFrameRates, translator.Get, translator.UpdateUnityEngineTimelineStandardFrameRates);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Timeline.ActivationTrack.PostPlaybackState>(translator.PushUnityEngineTimelineActivationTrackPostPlaybackState, translator.Get, translator.UpdateUnityEngineTimelineActivationTrackPostPlaybackState);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Timeline.AnimationPlayableAsset.LoopMode>(translator.PushUnityEngineTimelineAnimationPlayableAssetLoopMode, translator.Get, translator.UpdateUnityEngineTimelineAnimationPlayableAssetLoopMode);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Timeline.TimelineClip.ClipExtrapolation>(translator.PushUnityEngineTimelineTimelineClipClipExtrapolation, translator.Get, translator.UpdateUnityEngineTimelineTimelineClipClipExtrapolation);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Timeline.TimelineClip.BlendCurveMode>(translator.PushUnityEngineTimelineTimelineClipBlendCurveMode, translator.Get, translator.UpdateUnityEngineTimelineTimelineClipBlendCurveMode);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Timeline.TimelineAsset.DurationMode>(translator.PushUnityEngineTimelineTimelineAssetDurationMode, translator.Get, translator.UpdateUnityEngineTimelineTimelineAssetDurationMode);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState>(translator.PushUnityEngineTimelineActivationControlPlayablePostPlaybackState, translator.Get, translator.UpdateUnityEngineTimelineActivationControlPlayablePostPlaybackState);
				translator.RegisterPushAndGetAndUpdate<Spine.Unity.UpdateMode>(translator.PushSpineUnityUpdateMode, translator.Get, translator.UpdateSpineUnityUpdateMode);
				translator.RegisterPushAndGetAndUpdate<Spine.Unity.UpdateTiming>(translator.PushSpineUnityUpdateTiming, translator.Get, translator.UpdateSpineUnityUpdateTiming);
				translator.RegisterPushAndGetAndUpdate<Spine.Unity.SettingsTriState>(translator.PushSpineUnitySettingsTriState, translator.Get, translator.UpdateSpineUnitySettingsTriState);
				translator.RegisterPushAndGetAndUpdate<Spine.Unity.BoneFollower.AxisOrientation>(translator.PushSpineUnityBoneFollowerAxisOrientation, translator.Get, translator.UpdateSpineUnityBoneFollowerAxisOrientation);
				translator.RegisterPushAndGetAndUpdate<Spine.Unity.SkeletonGraphic.LayoutMode>(translator.PushSpineUnitySkeletonGraphicLayoutMode, translator.Get, translator.UpdateSpineUnitySkeletonGraphicLayoutMode);
				translator.RegisterPushAndGetAndUpdate<Spine.Unity.SkeletonUtilityBone.Mode>(translator.PushSpineUnitySkeletonUtilityBoneMode, translator.Get, translator.UpdateSpineUnitySkeletonUtilityBoneMode);
				translator.RegisterPushAndGetAndUpdate<Spine.Unity.SkeletonUtilityBone.UpdatePhase>(translator.PushSpineUnitySkeletonUtilityBoneUpdatePhase, translator.Get, translator.UpdateSpineUnitySkeletonUtilityBoneUpdatePhase);
				translator.RegisterPushAndGetAndUpdate<Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode>(translator.PushSpineUnitySkeletonMecanimMecanimTranslatorMixMode, translator.Get, translator.UpdateSpineUnitySkeletonMecanimMecanimTranslatorMixMode);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.AutoPlay>(translator.PushDGTweeningAutoPlay, translator.Get, translator.UpdateDGTweeningAutoPlay);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.AxisConstraint>(translator.PushDGTweeningAxisConstraint, translator.Get, translator.UpdateDGTweeningAxisConstraint);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.Ease>(translator.PushDGTweeningEase, translator.Get, translator.UpdateDGTweeningEase);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.PathMode>(translator.PushDGTweeningPathMode, translator.Get, translator.UpdateDGTweeningPathMode);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.PathType>(translator.PushDGTweeningPathType, translator.Get, translator.UpdateDGTweeningPathType);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.RotateMode>(translator.PushDGTweeningRotateMode, translator.Get, translator.UpdateDGTweeningRotateMode);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.ScrambleMode>(translator.PushDGTweeningScrambleMode, translator.Get, translator.UpdateDGTweeningScrambleMode);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.LoopType>(translator.PushDGTweeningLoopType, translator.Get, translator.UpdateDGTweeningLoopType);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.LogBehaviour>(translator.PushDGTweeningLogBehaviour, translator.Get, translator.UpdateDGTweeningLogBehaviour);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.TweenType>(translator.PushDGTweeningTweenType, translator.Get, translator.UpdateDGTweeningTweenType);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.UpdateType>(translator.PushDGTweeningUpdateType, translator.Get, translator.UpdateDGTweeningUpdateType);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.HandlesDrawMode>(translator.PushDGTweeningHandlesDrawMode, translator.Get, translator.UpdateDGTweeningHandlesDrawMode);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.HandlesType>(translator.PushDGTweeningHandlesType, translator.Get, translator.UpdateDGTweeningHandlesType);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.DOTweenInspectorMode>(translator.PushDGTweeningDOTweenInspectorMode, translator.Get, translator.UpdateDGTweeningDOTweenInspectorMode);
				translator.RegisterPushAndGetAndUpdate<DG.Tweening.SpiralMode>(translator.PushDGTweeningSpiralMode, translator.Get, translator.UpdateDGTweeningSpiralMode);
				translator.RegisterPushAndGetAndUpdate<GameLib.EGameMode>(translator.PushGameLibEGameMode, translator.Get, translator.UpdateGameLibEGameMode);
				translator.RegisterPushAndGetAndUpdate<GameLib.GridMapManager.GridType>(translator.PushGameLibGridMapManagerGridType, translator.Get, translator.UpdateGameLibGridMapManagerGridType);
				translator.RegisterPushAndGetAndUpdate<GameLib.GridMapManager.LayerLevel>(translator.PushGameLibGridMapManagerLayerLevel, translator.Get, translator.UpdateGameLibGridMapManagerLayerLevel);
			
			}
        }
        
        static IniterAdderUnityEngineVector2 s_IniterAdderUnityEngineVector2_dumb_obj = new IniterAdderUnityEngineVector2();
        static IniterAdderUnityEngineVector2 IniterAdderUnityEngineVector2_dumb_obj {get{return s_IniterAdderUnityEngineVector2_dumb_obj;}}
        
        
        int UnityEngineVector2_TypeID = -1;
        public void PushUnityEngineVector2(RealStatePtr L, UnityEngine.Vector2 val)
        {
            if (UnityEngineVector2_TypeID == -1)
            {
			    bool is_first;
                UnityEngineVector2_TypeID = getTypeId(L, typeof(UnityEngine.Vector2), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 8, UnityEngineVector2_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Vector2 ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Vector2 val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector2_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector2");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Vector2");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Vector2)objectCasters.GetCaster(typeof(UnityEngine.Vector2))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineVector2(RealStatePtr L, int index, UnityEngine.Vector2 val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector2_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector2");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Vector2 ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineVector3_TypeID = -1;
        public void PushUnityEngineVector3(RealStatePtr L, UnityEngine.Vector3 val)
        {
            if (UnityEngineVector3_TypeID == -1)
            {
			    bool is_first;
                UnityEngineVector3_TypeID = getTypeId(L, typeof(UnityEngine.Vector3), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 12, UnityEngineVector3_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Vector3 ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Vector3 val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector3_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector3");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Vector3");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Vector3)objectCasters.GetCaster(typeof(UnityEngine.Vector3))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineVector3(RealStatePtr L, int index, UnityEngine.Vector3 val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector3_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector3");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Vector3 ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineVector4_TypeID = -1;
        public void PushUnityEngineVector4(RealStatePtr L, UnityEngine.Vector4 val)
        {
            if (UnityEngineVector4_TypeID == -1)
            {
			    bool is_first;
                UnityEngineVector4_TypeID = getTypeId(L, typeof(UnityEngine.Vector4), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 16, UnityEngineVector4_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Vector4 ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Vector4 val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector4_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector4");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Vector4");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Vector4)objectCasters.GetCaster(typeof(UnityEngine.Vector4))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineVector4(RealStatePtr L, int index, UnityEngine.Vector4 val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector4_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector4");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Vector4 ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineColor_TypeID = -1;
        public void PushUnityEngineColor(RealStatePtr L, UnityEngine.Color val)
        {
            if (UnityEngineColor_TypeID == -1)
            {
			    bool is_first;
                UnityEngineColor_TypeID = getTypeId(L, typeof(UnityEngine.Color), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 16, UnityEngineColor_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Color ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Color val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineColor_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Color");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Color");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Color)objectCasters.GetCaster(typeof(UnityEngine.Color))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineColor(RealStatePtr L, int index, UnityEngine.Color val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineColor_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Color");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Color ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineQuaternion_TypeID = -1;
        public void PushUnityEngineQuaternion(RealStatePtr L, UnityEngine.Quaternion val)
        {
            if (UnityEngineQuaternion_TypeID == -1)
            {
			    bool is_first;
                UnityEngineQuaternion_TypeID = getTypeId(L, typeof(UnityEngine.Quaternion), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 16, UnityEngineQuaternion_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Quaternion ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Quaternion val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineQuaternion_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Quaternion");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Quaternion");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Quaternion)objectCasters.GetCaster(typeof(UnityEngine.Quaternion))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineQuaternion(RealStatePtr L, int index, UnityEngine.Quaternion val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineQuaternion_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Quaternion");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Quaternion ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineRay_TypeID = -1;
        public void PushUnityEngineRay(RealStatePtr L, UnityEngine.Ray val)
        {
            if (UnityEngineRay_TypeID == -1)
            {
			    bool is_first;
                UnityEngineRay_TypeID = getTypeId(L, typeof(UnityEngine.Ray), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 24, UnityEngineRay_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Ray ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Ray val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRay_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Ray");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Ray");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Ray)objectCasters.GetCaster(typeof(UnityEngine.Ray))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineRay(RealStatePtr L, int index, UnityEngine.Ray val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRay_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Ray");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Ray ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineBounds_TypeID = -1;
        public void PushUnityEngineBounds(RealStatePtr L, UnityEngine.Bounds val)
        {
            if (UnityEngineBounds_TypeID == -1)
            {
			    bool is_first;
                UnityEngineBounds_TypeID = getTypeId(L, typeof(UnityEngine.Bounds), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 24, UnityEngineBounds_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Bounds ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Bounds val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineBounds_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Bounds");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Bounds");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Bounds)objectCasters.GetCaster(typeof(UnityEngine.Bounds))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineBounds(RealStatePtr L, int index, UnityEngine.Bounds val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineBounds_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Bounds");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Bounds ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineRay2D_TypeID = -1;
        public void PushUnityEngineRay2D(RealStatePtr L, UnityEngine.Ray2D val)
        {
            if (UnityEngineRay2D_TypeID == -1)
            {
			    bool is_first;
                UnityEngineRay2D_TypeID = getTypeId(L, typeof(UnityEngine.Ray2D), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 16, UnityEngineRay2D_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Ray2D ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Ray2D val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRay2D_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Ray2D");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Ray2D");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Ray2D)objectCasters.GetCaster(typeof(UnityEngine.Ray2D))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineRay2D(RealStatePtr L, int index, UnityEngine.Ray2D val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRay2D_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Ray2D");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Ray2D ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineAnimatorUpdateMode_TypeID = -1;
		int UnityEngineAnimatorUpdateMode_EnumRef = -1;
        
        public void PushUnityEngineAnimatorUpdateMode(RealStatePtr L, UnityEngine.AnimatorUpdateMode val)
        {
            if (UnityEngineAnimatorUpdateMode_TypeID == -1)
            {
			    bool is_first;
                UnityEngineAnimatorUpdateMode_TypeID = getTypeId(L, typeof(UnityEngine.AnimatorUpdateMode), out is_first);
				
				if (UnityEngineAnimatorUpdateMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.AnimatorUpdateMode));
				    UnityEngineAnimatorUpdateMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineAnimatorUpdateMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineAnimatorUpdateMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.AnimatorUpdateMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineAnimatorUpdateMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.AnimatorUpdateMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineAnimatorUpdateMode_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.AnimatorUpdateMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.AnimatorUpdateMode");
                }
				val = (UnityEngine.AnimatorUpdateMode)e;
                
            }
            else
            {
                val = (UnityEngine.AnimatorUpdateMode)objectCasters.GetCaster(typeof(UnityEngine.AnimatorUpdateMode))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineAnimatorUpdateMode(RealStatePtr L, int index, UnityEngine.AnimatorUpdateMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineAnimatorUpdateMode_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.AnimatorUpdateMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.AnimatorUpdateMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTilemapsTileColliderType_TypeID = -1;
		int UnityEngineTilemapsTileColliderType_EnumRef = -1;
        
        public void PushUnityEngineTilemapsTileColliderType(RealStatePtr L, UnityEngine.Tilemaps.Tile.ColliderType val)
        {
            if (UnityEngineTilemapsTileColliderType_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTilemapsTileColliderType_TypeID = getTypeId(L, typeof(UnityEngine.Tilemaps.Tile.ColliderType), out is_first);
				
				if (UnityEngineTilemapsTileColliderType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Tilemaps.Tile.ColliderType));
				    UnityEngineTilemapsTileColliderType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTilemapsTileColliderType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTilemapsTileColliderType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Tilemaps.Tile.ColliderType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTilemapsTileColliderType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Tilemaps.Tile.ColliderType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTilemapsTileColliderType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Tilemaps.Tile.ColliderType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Tilemaps.Tile.ColliderType");
                }
				val = (UnityEngine.Tilemaps.Tile.ColliderType)e;
                
            }
            else
            {
                val = (UnityEngine.Tilemaps.Tile.ColliderType)objectCasters.GetCaster(typeof(UnityEngine.Tilemaps.Tile.ColliderType))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTilemapsTileColliderType(RealStatePtr L, int index, UnityEngine.Tilemaps.Tile.ColliderType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTilemapsTileColliderType_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Tilemaps.Tile.ColliderType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Tilemaps.Tile.ColliderType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTilemapsTilemapOrientation_TypeID = -1;
		int UnityEngineTilemapsTilemapOrientation_EnumRef = -1;
        
        public void PushUnityEngineTilemapsTilemapOrientation(RealStatePtr L, UnityEngine.Tilemaps.Tilemap.Orientation val)
        {
            if (UnityEngineTilemapsTilemapOrientation_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTilemapsTilemapOrientation_TypeID = getTypeId(L, typeof(UnityEngine.Tilemaps.Tilemap.Orientation), out is_first);
				
				if (UnityEngineTilemapsTilemapOrientation_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Tilemaps.Tilemap.Orientation));
				    UnityEngineTilemapsTilemapOrientation_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTilemapsTilemapOrientation_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTilemapsTilemapOrientation_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Tilemaps.Tilemap.Orientation ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTilemapsTilemapOrientation_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Tilemaps.Tilemap.Orientation val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTilemapsTilemapOrientation_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Tilemaps.Tilemap.Orientation");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Tilemaps.Tilemap.Orientation");
                }
				val = (UnityEngine.Tilemaps.Tilemap.Orientation)e;
                
            }
            else
            {
                val = (UnityEngine.Tilemaps.Tilemap.Orientation)objectCasters.GetCaster(typeof(UnityEngine.Tilemaps.Tilemap.Orientation))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTilemapsTilemapOrientation(RealStatePtr L, int index, UnityEngine.Tilemaps.Tilemap.Orientation val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTilemapsTilemapOrientation_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Tilemaps.Tilemap.Orientation");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Tilemaps.Tilemap.Orientation ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTilemapsTileFlags_TypeID = -1;
		int UnityEngineTilemapsTileFlags_EnumRef = -1;
        
        public void PushUnityEngineTilemapsTileFlags(RealStatePtr L, UnityEngine.Tilemaps.TileFlags val)
        {
            if (UnityEngineTilemapsTileFlags_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTilemapsTileFlags_TypeID = getTypeId(L, typeof(UnityEngine.Tilemaps.TileFlags), out is_first);
				
				if (UnityEngineTilemapsTileFlags_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Tilemaps.TileFlags));
				    UnityEngineTilemapsTileFlags_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTilemapsTileFlags_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTilemapsTileFlags_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Tilemaps.TileFlags ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTilemapsTileFlags_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Tilemaps.TileFlags val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTilemapsTileFlags_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Tilemaps.TileFlags");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Tilemaps.TileFlags");
                }
				val = (UnityEngine.Tilemaps.TileFlags)e;
                
            }
            else
            {
                val = (UnityEngine.Tilemaps.TileFlags)objectCasters.GetCaster(typeof(UnityEngine.Tilemaps.TileFlags))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTilemapsTileFlags(RealStatePtr L, int index, UnityEngine.Tilemaps.TileFlags val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTilemapsTileFlags_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Tilemaps.TileFlags");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Tilemaps.TileFlags ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTilemapsTileAnimationFlags_TypeID = -1;
		int UnityEngineTilemapsTileAnimationFlags_EnumRef = -1;
        
        public void PushUnityEngineTilemapsTileAnimationFlags(RealStatePtr L, UnityEngine.Tilemaps.TileAnimationFlags val)
        {
            if (UnityEngineTilemapsTileAnimationFlags_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTilemapsTileAnimationFlags_TypeID = getTypeId(L, typeof(UnityEngine.Tilemaps.TileAnimationFlags), out is_first);
				
				if (UnityEngineTilemapsTileAnimationFlags_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Tilemaps.TileAnimationFlags));
				    UnityEngineTilemapsTileAnimationFlags_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTilemapsTileAnimationFlags_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTilemapsTileAnimationFlags_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Tilemaps.TileAnimationFlags ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTilemapsTileAnimationFlags_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Tilemaps.TileAnimationFlags val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTilemapsTileAnimationFlags_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Tilemaps.TileAnimationFlags");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Tilemaps.TileAnimationFlags");
                }
				val = (UnityEngine.Tilemaps.TileAnimationFlags)e;
                
            }
            else
            {
                val = (UnityEngine.Tilemaps.TileAnimationFlags)objectCasters.GetCaster(typeof(UnityEngine.Tilemaps.TileAnimationFlags))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTilemapsTileAnimationFlags(RealStatePtr L, int index, UnityEngine.Tilemaps.TileAnimationFlags val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTilemapsTileAnimationFlags_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Tilemaps.TileAnimationFlags");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Tilemaps.TileAnimationFlags ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTilemapsTilemapRendererSortOrder_TypeID = -1;
		int UnityEngineTilemapsTilemapRendererSortOrder_EnumRef = -1;
        
        public void PushUnityEngineTilemapsTilemapRendererSortOrder(RealStatePtr L, UnityEngine.Tilemaps.TilemapRenderer.SortOrder val)
        {
            if (UnityEngineTilemapsTilemapRendererSortOrder_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTilemapsTilemapRendererSortOrder_TypeID = getTypeId(L, typeof(UnityEngine.Tilemaps.TilemapRenderer.SortOrder), out is_first);
				
				if (UnityEngineTilemapsTilemapRendererSortOrder_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Tilemaps.TilemapRenderer.SortOrder));
				    UnityEngineTilemapsTilemapRendererSortOrder_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTilemapsTilemapRendererSortOrder_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTilemapsTilemapRendererSortOrder_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Tilemaps.TilemapRenderer.SortOrder ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTilemapsTilemapRendererSortOrder_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Tilemaps.TilemapRenderer.SortOrder val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTilemapsTilemapRendererSortOrder_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Tilemaps.TilemapRenderer.SortOrder");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Tilemaps.TilemapRenderer.SortOrder");
                }
				val = (UnityEngine.Tilemaps.TilemapRenderer.SortOrder)e;
                
            }
            else
            {
                val = (UnityEngine.Tilemaps.TilemapRenderer.SortOrder)objectCasters.GetCaster(typeof(UnityEngine.Tilemaps.TilemapRenderer.SortOrder))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTilemapsTilemapRendererSortOrder(RealStatePtr L, int index, UnityEngine.Tilemaps.TilemapRenderer.SortOrder val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTilemapsTilemapRendererSortOrder_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Tilemaps.TilemapRenderer.SortOrder");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Tilemaps.TilemapRenderer.SortOrder ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTilemapsTilemapRendererMode_TypeID = -1;
		int UnityEngineTilemapsTilemapRendererMode_EnumRef = -1;
        
        public void PushUnityEngineTilemapsTilemapRendererMode(RealStatePtr L, UnityEngine.Tilemaps.TilemapRenderer.Mode val)
        {
            if (UnityEngineTilemapsTilemapRendererMode_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTilemapsTilemapRendererMode_TypeID = getTypeId(L, typeof(UnityEngine.Tilemaps.TilemapRenderer.Mode), out is_first);
				
				if (UnityEngineTilemapsTilemapRendererMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Tilemaps.TilemapRenderer.Mode));
				    UnityEngineTilemapsTilemapRendererMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTilemapsTilemapRendererMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTilemapsTilemapRendererMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Tilemaps.TilemapRenderer.Mode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTilemapsTilemapRendererMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Tilemaps.TilemapRenderer.Mode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTilemapsTilemapRendererMode_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Tilemaps.TilemapRenderer.Mode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Tilemaps.TilemapRenderer.Mode");
                }
				val = (UnityEngine.Tilemaps.TilemapRenderer.Mode)e;
                
            }
            else
            {
                val = (UnityEngine.Tilemaps.TilemapRenderer.Mode)objectCasters.GetCaster(typeof(UnityEngine.Tilemaps.TilemapRenderer.Mode))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTilemapsTilemapRendererMode(RealStatePtr L, int index, UnityEngine.Tilemaps.TilemapRenderer.Mode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTilemapsTilemapRendererMode_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Tilemaps.TilemapRenderer.Mode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Tilemaps.TilemapRenderer.Mode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTilemapsTilemapRendererDetectChunkCullingBounds_TypeID = -1;
		int UnityEngineTilemapsTilemapRendererDetectChunkCullingBounds_EnumRef = -1;
        
        public void PushUnityEngineTilemapsTilemapRendererDetectChunkCullingBounds(RealStatePtr L, UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds val)
        {
            if (UnityEngineTilemapsTilemapRendererDetectChunkCullingBounds_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTilemapsTilemapRendererDetectChunkCullingBounds_TypeID = getTypeId(L, typeof(UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds), out is_first);
				
				if (UnityEngineTilemapsTilemapRendererDetectChunkCullingBounds_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds));
				    UnityEngineTilemapsTilemapRendererDetectChunkCullingBounds_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTilemapsTilemapRendererDetectChunkCullingBounds_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTilemapsTilemapRendererDetectChunkCullingBounds_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTilemapsTilemapRendererDetectChunkCullingBounds_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTilemapsTilemapRendererDetectChunkCullingBounds_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds");
                }
				val = (UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds)e;
                
            }
            else
            {
                val = (UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds)objectCasters.GetCaster(typeof(UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTilemapsTilemapRendererDetectChunkCullingBounds(RealStatePtr L, int index, UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTilemapsTilemapRendererDetectChunkCullingBounds_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeOriginType_TypeID = -1;
		int HonorRuntimeOriginType_EnumRef = -1;
        
        public void PushHonorRuntimeOriginType(RealStatePtr L, Honor.Runtime.OriginType val)
        {
            if (HonorRuntimeOriginType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeOriginType_TypeID = getTypeId(L, typeof(Honor.Runtime.OriginType), out is_first);
				
				if (HonorRuntimeOriginType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.OriginType));
				    HonorRuntimeOriginType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeOriginType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeOriginType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.OriginType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeOriginType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.OriginType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeOriginType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.OriginType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.OriginType");
                }
				val = (Honor.Runtime.OriginType)e;
                
            }
            else
            {
                val = (Honor.Runtime.OriginType)objectCasters.GetCaster(typeof(Honor.Runtime.OriginType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeOriginType(RealStatePtr L, int index, Honor.Runtime.OriginType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeOriginType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.OriginType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.OriginType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimePrefabDetailType_TypeID = -1;
		int HonorRuntimePrefabDetailType_EnumRef = -1;
        
        public void PushHonorRuntimePrefabDetailType(RealStatePtr L, Honor.Runtime.PrefabDetailType val)
        {
            if (HonorRuntimePrefabDetailType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimePrefabDetailType_TypeID = getTypeId(L, typeof(Honor.Runtime.PrefabDetailType), out is_first);
				
				if (HonorRuntimePrefabDetailType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.PrefabDetailType));
				    HonorRuntimePrefabDetailType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimePrefabDetailType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimePrefabDetailType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.PrefabDetailType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimePrefabDetailType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.PrefabDetailType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimePrefabDetailType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.PrefabDetailType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.PrefabDetailType");
                }
				val = (Honor.Runtime.PrefabDetailType)e;
                
            }
            else
            {
                val = (Honor.Runtime.PrefabDetailType)objectCasters.GetCaster(typeof(Honor.Runtime.PrefabDetailType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimePrefabDetailType(RealStatePtr L, int index, Honor.Runtime.PrefabDetailType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimePrefabDetailType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.PrefabDetailType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.PrefabDetailType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimePrefabType_TypeID = -1;
		int HonorRuntimePrefabType_EnumRef = -1;
        
        public void PushHonorRuntimePrefabType(RealStatePtr L, Honor.Runtime.PrefabType val)
        {
            if (HonorRuntimePrefabType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimePrefabType_TypeID = getTypeId(L, typeof(Honor.Runtime.PrefabType), out is_first);
				
				if (HonorRuntimePrefabType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.PrefabType));
				    HonorRuntimePrefabType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimePrefabType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimePrefabType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.PrefabType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimePrefabType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.PrefabType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimePrefabType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.PrefabType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.PrefabType");
                }
				val = (Honor.Runtime.PrefabType)e;
                
            }
            else
            {
                val = (Honor.Runtime.PrefabType)objectCasters.GetCaster(typeof(Honor.Runtime.PrefabType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimePrefabType(RealStatePtr L, int index, Honor.Runtime.PrefabType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimePrefabType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.PrefabType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.PrefabType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeGameEventCmd_TypeID = -1;
		int HonorRuntimeGameEventCmd_EnumRef = -1;
        
        public void PushHonorRuntimeGameEventCmd(RealStatePtr L, Honor.Runtime.GameEventCmd val)
        {
            if (HonorRuntimeGameEventCmd_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeGameEventCmd_TypeID = getTypeId(L, typeof(Honor.Runtime.GameEventCmd), out is_first);
				
				if (HonorRuntimeGameEventCmd_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.GameEventCmd));
				    HonorRuntimeGameEventCmd_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeGameEventCmd_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeGameEventCmd_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.GameEventCmd ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeGameEventCmd_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.GameEventCmd val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameEventCmd_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameEventCmd");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.GameEventCmd");
                }
				val = (Honor.Runtime.GameEventCmd)e;
                
            }
            else
            {
                val = (Honor.Runtime.GameEventCmd)objectCasters.GetCaster(typeof(Honor.Runtime.GameEventCmd))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeGameEventCmd(RealStatePtr L, int index, Honor.Runtime.GameEventCmd val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameEventCmd_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameEventCmd");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.GameEventCmd ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeSnapStatus_TypeID = -1;
		int HonorRuntimeSnapStatus_EnumRef = -1;
        
        public void PushHonorRuntimeSnapStatus(RealStatePtr L, Honor.Runtime.SnapStatus val)
        {
            if (HonorRuntimeSnapStatus_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeSnapStatus_TypeID = getTypeId(L, typeof(Honor.Runtime.SnapStatus), out is_first);
				
				if (HonorRuntimeSnapStatus_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.SnapStatus));
				    HonorRuntimeSnapStatus_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeSnapStatus_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeSnapStatus_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.SnapStatus ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeSnapStatus_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.SnapStatus val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeSnapStatus_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.SnapStatus");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.SnapStatus");
                }
				val = (Honor.Runtime.SnapStatus)e;
                
            }
            else
            {
                val = (Honor.Runtime.SnapStatus)objectCasters.GetCaster(typeof(Honor.Runtime.SnapStatus))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeSnapStatus(RealStatePtr L, int index, Honor.Runtime.SnapStatus val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeSnapStatus_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.SnapStatus");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.SnapStatus ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeItemCornerEnum_TypeID = -1;
		int HonorRuntimeItemCornerEnum_EnumRef = -1;
        
        public void PushHonorRuntimeItemCornerEnum(RealStatePtr L, Honor.Runtime.ItemCornerEnum val)
        {
            if (HonorRuntimeItemCornerEnum_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeItemCornerEnum_TypeID = getTypeId(L, typeof(Honor.Runtime.ItemCornerEnum), out is_first);
				
				if (HonorRuntimeItemCornerEnum_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.ItemCornerEnum));
				    HonorRuntimeItemCornerEnum_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeItemCornerEnum_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeItemCornerEnum_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.ItemCornerEnum ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeItemCornerEnum_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.ItemCornerEnum val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeItemCornerEnum_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.ItemCornerEnum");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.ItemCornerEnum");
                }
				val = (Honor.Runtime.ItemCornerEnum)e;
                
            }
            else
            {
                val = (Honor.Runtime.ItemCornerEnum)objectCasters.GetCaster(typeof(Honor.Runtime.ItemCornerEnum))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeItemCornerEnum(RealStatePtr L, int index, Honor.Runtime.ItemCornerEnum val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeItemCornerEnum_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.ItemCornerEnum");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.ItemCornerEnum ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeListItemArrangeType_TypeID = -1;
		int HonorRuntimeListItemArrangeType_EnumRef = -1;
        
        public void PushHonorRuntimeListItemArrangeType(RealStatePtr L, Honor.Runtime.ListItemArrangeType val)
        {
            if (HonorRuntimeListItemArrangeType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeListItemArrangeType_TypeID = getTypeId(L, typeof(Honor.Runtime.ListItemArrangeType), out is_first);
				
				if (HonorRuntimeListItemArrangeType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.ListItemArrangeType));
				    HonorRuntimeListItemArrangeType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeListItemArrangeType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeListItemArrangeType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.ListItemArrangeType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeListItemArrangeType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.ListItemArrangeType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeListItemArrangeType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.ListItemArrangeType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.ListItemArrangeType");
                }
				val = (Honor.Runtime.ListItemArrangeType)e;
                
            }
            else
            {
                val = (Honor.Runtime.ListItemArrangeType)objectCasters.GetCaster(typeof(Honor.Runtime.ListItemArrangeType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeListItemArrangeType(RealStatePtr L, int index, Honor.Runtime.ListItemArrangeType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeListItemArrangeType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.ListItemArrangeType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.ListItemArrangeType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeGridItemArrangeType_TypeID = -1;
		int HonorRuntimeGridItemArrangeType_EnumRef = -1;
        
        public void PushHonorRuntimeGridItemArrangeType(RealStatePtr L, Honor.Runtime.GridItemArrangeType val)
        {
            if (HonorRuntimeGridItemArrangeType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeGridItemArrangeType_TypeID = getTypeId(L, typeof(Honor.Runtime.GridItemArrangeType), out is_first);
				
				if (HonorRuntimeGridItemArrangeType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.GridItemArrangeType));
				    HonorRuntimeGridItemArrangeType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeGridItemArrangeType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeGridItemArrangeType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.GridItemArrangeType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeGridItemArrangeType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.GridItemArrangeType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGridItemArrangeType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GridItemArrangeType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.GridItemArrangeType");
                }
				val = (Honor.Runtime.GridItemArrangeType)e;
                
            }
            else
            {
                val = (Honor.Runtime.GridItemArrangeType)objectCasters.GetCaster(typeof(Honor.Runtime.GridItemArrangeType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeGridItemArrangeType(RealStatePtr L, int index, Honor.Runtime.GridItemArrangeType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGridItemArrangeType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GridItemArrangeType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.GridItemArrangeType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeGridFixedType_TypeID = -1;
		int HonorRuntimeGridFixedType_EnumRef = -1;
        
        public void PushHonorRuntimeGridFixedType(RealStatePtr L, Honor.Runtime.GridFixedType val)
        {
            if (HonorRuntimeGridFixedType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeGridFixedType_TypeID = getTypeId(L, typeof(Honor.Runtime.GridFixedType), out is_first);
				
				if (HonorRuntimeGridFixedType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.GridFixedType));
				    HonorRuntimeGridFixedType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeGridFixedType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeGridFixedType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.GridFixedType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeGridFixedType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.GridFixedType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGridFixedType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GridFixedType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.GridFixedType");
                }
				val = (Honor.Runtime.GridFixedType)e;
                
            }
            else
            {
                val = (Honor.Runtime.GridFixedType)objectCasters.GetCaster(typeof(Honor.Runtime.GridFixedType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeGridFixedType(RealStatePtr L, int index, Honor.Runtime.GridFixedType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGridFixedType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GridFixedType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.GridFixedType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimePatternType_TypeID = -1;
		int HonorRuntimePatternType_EnumRef = -1;
        
        public void PushHonorRuntimePatternType(RealStatePtr L, Honor.Runtime.PatternType val)
        {
            if (HonorRuntimePatternType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimePatternType_TypeID = getTypeId(L, typeof(Honor.Runtime.PatternType), out is_first);
				
				if (HonorRuntimePatternType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.PatternType));
				    HonorRuntimePatternType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimePatternType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimePatternType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.PatternType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimePatternType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.PatternType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimePatternType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.PatternType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.PatternType");
                }
				val = (Honor.Runtime.PatternType)e;
                
            }
            else
            {
                val = (Honor.Runtime.PatternType)objectCasters.GetCaster(typeof(Honor.Runtime.PatternType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimePatternType(RealStatePtr L, int index, Honor.Runtime.PatternType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimePatternType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.PatternType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.PatternType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeNonePatternType_TypeID = -1;
		int HonorRuntimeNonePatternType_EnumRef = -1;
        
        public void PushHonorRuntimeNonePatternType(RealStatePtr L, Honor.Runtime.NonePatternType val)
        {
            if (HonorRuntimeNonePatternType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeNonePatternType_TypeID = getTypeId(L, typeof(Honor.Runtime.NonePatternType), out is_first);
				
				if (HonorRuntimeNonePatternType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.NonePatternType));
				    HonorRuntimeNonePatternType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeNonePatternType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeNonePatternType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.NonePatternType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeNonePatternType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.NonePatternType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeNonePatternType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.NonePatternType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.NonePatternType");
                }
				val = (Honor.Runtime.NonePatternType)e;
                
            }
            else
            {
                val = (Honor.Runtime.NonePatternType)objectCasters.GetCaster(typeof(Honor.Runtime.NonePatternType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeNonePatternType(RealStatePtr L, int index, Honor.Runtime.NonePatternType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeNonePatternType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.NonePatternType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.NonePatternType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeMVVMPatternType_TypeID = -1;
		int HonorRuntimeMVVMPatternType_EnumRef = -1;
        
        public void PushHonorRuntimeMVVMPatternType(RealStatePtr L, Honor.Runtime.MVVMPatternType val)
        {
            if (HonorRuntimeMVVMPatternType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeMVVMPatternType_TypeID = getTypeId(L, typeof(Honor.Runtime.MVVMPatternType), out is_first);
				
				if (HonorRuntimeMVVMPatternType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.MVVMPatternType));
				    HonorRuntimeMVVMPatternType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeMVVMPatternType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeMVVMPatternType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.MVVMPatternType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeMVVMPatternType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.MVVMPatternType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeMVVMPatternType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.MVVMPatternType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.MVVMPatternType");
                }
				val = (Honor.Runtime.MVVMPatternType)e;
                
            }
            else
            {
                val = (Honor.Runtime.MVVMPatternType)objectCasters.GetCaster(typeof(Honor.Runtime.MVVMPatternType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeMVVMPatternType(RealStatePtr L, int index, Honor.Runtime.MVVMPatternType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeMVVMPatternType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.MVVMPatternType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.MVVMPatternType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimePersistWayType_TypeID = -1;
		int HonorRuntimePersistWayType_EnumRef = -1;
        
        public void PushHonorRuntimePersistWayType(RealStatePtr L, Honor.Runtime.PersistWayType val)
        {
            if (HonorRuntimePersistWayType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimePersistWayType_TypeID = getTypeId(L, typeof(Honor.Runtime.PersistWayType), out is_first);
				
				if (HonorRuntimePersistWayType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.PersistWayType));
				    HonorRuntimePersistWayType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimePersistWayType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimePersistWayType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.PersistWayType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimePersistWayType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.PersistWayType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimePersistWayType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.PersistWayType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.PersistWayType");
                }
				val = (Honor.Runtime.PersistWayType)e;
                
            }
            else
            {
                val = (Honor.Runtime.PersistWayType)objectCasters.GetCaster(typeof(Honor.Runtime.PersistWayType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimePersistWayType(RealStatePtr L, int index, Honor.Runtime.PersistWayType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimePersistWayType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.PersistWayType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.PersistWayType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimePlaySoundErrorCode_TypeID = -1;
		int HonorRuntimePlaySoundErrorCode_EnumRef = -1;
        
        public void PushHonorRuntimePlaySoundErrorCode(RealStatePtr L, Honor.Runtime.PlaySoundErrorCode val)
        {
            if (HonorRuntimePlaySoundErrorCode_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimePlaySoundErrorCode_TypeID = getTypeId(L, typeof(Honor.Runtime.PlaySoundErrorCode), out is_first);
				
				if (HonorRuntimePlaySoundErrorCode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.PlaySoundErrorCode));
				    HonorRuntimePlaySoundErrorCode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimePlaySoundErrorCode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimePlaySoundErrorCode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.PlaySoundErrorCode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimePlaySoundErrorCode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.PlaySoundErrorCode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimePlaySoundErrorCode_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.PlaySoundErrorCode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.PlaySoundErrorCode");
                }
				val = (Honor.Runtime.PlaySoundErrorCode)e;
                
            }
            else
            {
                val = (Honor.Runtime.PlaySoundErrorCode)objectCasters.GetCaster(typeof(Honor.Runtime.PlaySoundErrorCode))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimePlaySoundErrorCode(RealStatePtr L, int index, Honor.Runtime.PlaySoundErrorCode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimePlaySoundErrorCode_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.PlaySoundErrorCode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.PlaySoundErrorCode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeDevicePerformanceLevel_TypeID = -1;
		int HonorRuntimeDevicePerformanceLevel_EnumRef = -1;
        
        public void PushHonorRuntimeDevicePerformanceLevel(RealStatePtr L, Honor.Runtime.DevicePerformanceLevel val)
        {
            if (HonorRuntimeDevicePerformanceLevel_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeDevicePerformanceLevel_TypeID = getTypeId(L, typeof(Honor.Runtime.DevicePerformanceLevel), out is_first);
				
				if (HonorRuntimeDevicePerformanceLevel_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.DevicePerformanceLevel));
				    HonorRuntimeDevicePerformanceLevel_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeDevicePerformanceLevel_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeDevicePerformanceLevel_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.DevicePerformanceLevel ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeDevicePerformanceLevel_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.DevicePerformanceLevel val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeDevicePerformanceLevel_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.DevicePerformanceLevel");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.DevicePerformanceLevel");
                }
				val = (Honor.Runtime.DevicePerformanceLevel)e;
                
            }
            else
            {
                val = (Honor.Runtime.DevicePerformanceLevel)objectCasters.GetCaster(typeof(Honor.Runtime.DevicePerformanceLevel))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeDevicePerformanceLevel(RealStatePtr L, int index, Honor.Runtime.DevicePerformanceLevel val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeDevicePerformanceLevel_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.DevicePerformanceLevel");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.DevicePerformanceLevel ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeQualityLevel_TypeID = -1;
		int HonorRuntimeQualityLevel_EnumRef = -1;
        
        public void PushHonorRuntimeQualityLevel(RealStatePtr L, Honor.Runtime.QualityLevel val)
        {
            if (HonorRuntimeQualityLevel_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeQualityLevel_TypeID = getTypeId(L, typeof(Honor.Runtime.QualityLevel), out is_first);
				
				if (HonorRuntimeQualityLevel_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.QualityLevel));
				    HonorRuntimeQualityLevel_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeQualityLevel_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeQualityLevel_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.QualityLevel ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeQualityLevel_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.QualityLevel val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeQualityLevel_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.QualityLevel");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.QualityLevel");
                }
				val = (Honor.Runtime.QualityLevel)e;
                
            }
            else
            {
                val = (Honor.Runtime.QualityLevel)objectCasters.GetCaster(typeof(Honor.Runtime.QualityLevel))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeQualityLevel(RealStatePtr L, int index, Honor.Runtime.QualityLevel val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeQualityLevel_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.QualityLevel");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.QualityLevel ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeUIType_TypeID = -1;
		int HonorRuntimeUIType_EnumRef = -1;
        
        public void PushHonorRuntimeUIType(RealStatePtr L, Honor.Runtime.UIType val)
        {
            if (HonorRuntimeUIType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeUIType_TypeID = getTypeId(L, typeof(Honor.Runtime.UIType), out is_first);
				
				if (HonorRuntimeUIType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.UIType));
				    HonorRuntimeUIType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeUIType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeUIType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.UIType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeUIType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.UIType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeUIType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.UIType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.UIType");
                }
				val = (Honor.Runtime.UIType)e;
                
            }
            else
            {
                val = (Honor.Runtime.UIType)objectCasters.GetCaster(typeof(Honor.Runtime.UIType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeUIType(RealStatePtr L, int index, Honor.Runtime.UIType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeUIType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.UIType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.UIType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeVibrateType_TypeID = -1;
		int HonorRuntimeVibrateType_EnumRef = -1;
        
        public void PushHonorRuntimeVibrateType(RealStatePtr L, Honor.Runtime.VibrateType val)
        {
            if (HonorRuntimeVibrateType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeVibrateType_TypeID = getTypeId(L, typeof(Honor.Runtime.VibrateType), out is_first);
				
				if (HonorRuntimeVibrateType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.VibrateType));
				    HonorRuntimeVibrateType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeVibrateType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeVibrateType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.VibrateType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeVibrateType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.VibrateType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeVibrateType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.VibrateType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.VibrateType");
                }
				val = (Honor.Runtime.VibrateType)e;
                
            }
            else
            {
                val = (Honor.Runtime.VibrateType)objectCasters.GetCaster(typeof(Honor.Runtime.VibrateType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeVibrateType(RealStatePtr L, int index, Honor.Runtime.VibrateType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeVibrateType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.VibrateType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.VibrateType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeGameDefinitionsDimensionMode_TypeID = -1;
		int HonorRuntimeGameDefinitionsDimensionMode_EnumRef = -1;
        
        public void PushHonorRuntimeGameDefinitionsDimensionMode(RealStatePtr L, Honor.Runtime.GameDefinitions.DimensionMode val)
        {
            if (HonorRuntimeGameDefinitionsDimensionMode_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeGameDefinitionsDimensionMode_TypeID = getTypeId(L, typeof(Honor.Runtime.GameDefinitions.DimensionMode), out is_first);
				
				if (HonorRuntimeGameDefinitionsDimensionMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.GameDefinitions.DimensionMode));
				    HonorRuntimeGameDefinitionsDimensionMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeGameDefinitionsDimensionMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeGameDefinitionsDimensionMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.GameDefinitions.DimensionMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeGameDefinitionsDimensionMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.GameDefinitions.DimensionMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameDefinitionsDimensionMode_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameDefinitions.DimensionMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.GameDefinitions.DimensionMode");
                }
				val = (Honor.Runtime.GameDefinitions.DimensionMode)e;
                
            }
            else
            {
                val = (Honor.Runtime.GameDefinitions.DimensionMode)objectCasters.GetCaster(typeof(Honor.Runtime.GameDefinitions.DimensionMode))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeGameDefinitionsDimensionMode(RealStatePtr L, int index, Honor.Runtime.GameDefinitions.DimensionMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameDefinitionsDimensionMode_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameDefinitions.DimensionMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.GameDefinitions.DimensionMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeGameDefinitionsAssetType_TypeID = -1;
		int HonorRuntimeGameDefinitionsAssetType_EnumRef = -1;
        
        public void PushHonorRuntimeGameDefinitionsAssetType(RealStatePtr L, Honor.Runtime.GameDefinitions.AssetType val)
        {
            if (HonorRuntimeGameDefinitionsAssetType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeGameDefinitionsAssetType_TypeID = getTypeId(L, typeof(Honor.Runtime.GameDefinitions.AssetType), out is_first);
				
				if (HonorRuntimeGameDefinitionsAssetType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.GameDefinitions.AssetType));
				    HonorRuntimeGameDefinitionsAssetType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeGameDefinitionsAssetType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeGameDefinitionsAssetType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.GameDefinitions.AssetType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeGameDefinitionsAssetType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.GameDefinitions.AssetType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameDefinitionsAssetType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameDefinitions.AssetType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.GameDefinitions.AssetType");
                }
				val = (Honor.Runtime.GameDefinitions.AssetType)e;
                
            }
            else
            {
                val = (Honor.Runtime.GameDefinitions.AssetType)objectCasters.GetCaster(typeof(Honor.Runtime.GameDefinitions.AssetType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeGameDefinitionsAssetType(RealStatePtr L, int index, Honor.Runtime.GameDefinitions.AssetType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameDefinitionsAssetType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameDefinitions.AssetType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.GameDefinitions.AssetType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeGameDefinitionsPathType_TypeID = -1;
		int HonorRuntimeGameDefinitionsPathType_EnumRef = -1;
        
        public void PushHonorRuntimeGameDefinitionsPathType(RealStatePtr L, Honor.Runtime.GameDefinitions.PathType val)
        {
            if (HonorRuntimeGameDefinitionsPathType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeGameDefinitionsPathType_TypeID = getTypeId(L, typeof(Honor.Runtime.GameDefinitions.PathType), out is_first);
				
				if (HonorRuntimeGameDefinitionsPathType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.GameDefinitions.PathType));
				    HonorRuntimeGameDefinitionsPathType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeGameDefinitionsPathType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeGameDefinitionsPathType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.GameDefinitions.PathType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeGameDefinitionsPathType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.GameDefinitions.PathType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameDefinitionsPathType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameDefinitions.PathType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.GameDefinitions.PathType");
                }
				val = (Honor.Runtime.GameDefinitions.PathType)e;
                
            }
            else
            {
                val = (Honor.Runtime.GameDefinitions.PathType)objectCasters.GetCaster(typeof(Honor.Runtime.GameDefinitions.PathType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeGameDefinitionsPathType(RealStatePtr L, int index, Honor.Runtime.GameDefinitions.PathType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameDefinitionsPathType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameDefinitions.PathType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.GameDefinitions.PathType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeGameDefinitionsDownloadStep_TypeID = -1;
		int HonorRuntimeGameDefinitionsDownloadStep_EnumRef = -1;
        
        public void PushHonorRuntimeGameDefinitionsDownloadStep(RealStatePtr L, Honor.Runtime.GameDefinitions.DownloadStep val)
        {
            if (HonorRuntimeGameDefinitionsDownloadStep_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeGameDefinitionsDownloadStep_TypeID = getTypeId(L, typeof(Honor.Runtime.GameDefinitions.DownloadStep), out is_first);
				
				if (HonorRuntimeGameDefinitionsDownloadStep_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.GameDefinitions.DownloadStep));
				    HonorRuntimeGameDefinitionsDownloadStep_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeGameDefinitionsDownloadStep_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeGameDefinitionsDownloadStep_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.GameDefinitions.DownloadStep ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeGameDefinitionsDownloadStep_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.GameDefinitions.DownloadStep val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameDefinitionsDownloadStep_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameDefinitions.DownloadStep");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.GameDefinitions.DownloadStep");
                }
				val = (Honor.Runtime.GameDefinitions.DownloadStep)e;
                
            }
            else
            {
                val = (Honor.Runtime.GameDefinitions.DownloadStep)objectCasters.GetCaster(typeof(Honor.Runtime.GameDefinitions.DownloadStep))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeGameDefinitionsDownloadStep(RealStatePtr L, int index, Honor.Runtime.GameDefinitions.DownloadStep val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameDefinitionsDownloadStep_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameDefinitions.DownloadStep");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.GameDefinitions.DownloadStep ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeGameDefinitionsDebugMode_TypeID = -1;
		int HonorRuntimeGameDefinitionsDebugMode_EnumRef = -1;
        
        public void PushHonorRuntimeGameDefinitionsDebugMode(RealStatePtr L, Honor.Runtime.GameDefinitions.DebugMode val)
        {
            if (HonorRuntimeGameDefinitionsDebugMode_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeGameDefinitionsDebugMode_TypeID = getTypeId(L, typeof(Honor.Runtime.GameDefinitions.DebugMode), out is_first);
				
				if (HonorRuntimeGameDefinitionsDebugMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.GameDefinitions.DebugMode));
				    HonorRuntimeGameDefinitionsDebugMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeGameDefinitionsDebugMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeGameDefinitionsDebugMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.GameDefinitions.DebugMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeGameDefinitionsDebugMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.GameDefinitions.DebugMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameDefinitionsDebugMode_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameDefinitions.DebugMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.GameDefinitions.DebugMode");
                }
				val = (Honor.Runtime.GameDefinitions.DebugMode)e;
                
            }
            else
            {
                val = (Honor.Runtime.GameDefinitions.DebugMode)objectCasters.GetCaster(typeof(Honor.Runtime.GameDefinitions.DebugMode))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeGameDefinitionsDebugMode(RealStatePtr L, int index, Honor.Runtime.GameDefinitions.DebugMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameDefinitionsDebugMode_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameDefinitions.DebugMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.GameDefinitions.DebugMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeGameDefinitionsLanguage_TypeID = -1;
		int HonorRuntimeGameDefinitionsLanguage_EnumRef = -1;
        
        public void PushHonorRuntimeGameDefinitionsLanguage(RealStatePtr L, Honor.Runtime.GameDefinitions.Language val)
        {
            if (HonorRuntimeGameDefinitionsLanguage_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeGameDefinitionsLanguage_TypeID = getTypeId(L, typeof(Honor.Runtime.GameDefinitions.Language), out is_first);
				
				if (HonorRuntimeGameDefinitionsLanguage_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.GameDefinitions.Language));
				    HonorRuntimeGameDefinitionsLanguage_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeGameDefinitionsLanguage_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeGameDefinitionsLanguage_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.GameDefinitions.Language ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeGameDefinitionsLanguage_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.GameDefinitions.Language val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameDefinitionsLanguage_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameDefinitions.Language");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.GameDefinitions.Language");
                }
				val = (Honor.Runtime.GameDefinitions.Language)e;
                
            }
            else
            {
                val = (Honor.Runtime.GameDefinitions.Language)objectCasters.GetCaster(typeof(Honor.Runtime.GameDefinitions.Language))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeGameDefinitionsLanguage(RealStatePtr L, int index, Honor.Runtime.GameDefinitions.Language val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameDefinitionsLanguage_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameDefinitions.Language");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.GameDefinitions.Language ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeGameDefinitionsDebugWindowModel_TypeID = -1;
		int HonorRuntimeGameDefinitionsDebugWindowModel_EnumRef = -1;
        
        public void PushHonorRuntimeGameDefinitionsDebugWindowModel(RealStatePtr L, Honor.Runtime.GameDefinitions.DebugWindowModel val)
        {
            if (HonorRuntimeGameDefinitionsDebugWindowModel_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeGameDefinitionsDebugWindowModel_TypeID = getTypeId(L, typeof(Honor.Runtime.GameDefinitions.DebugWindowModel), out is_first);
				
				if (HonorRuntimeGameDefinitionsDebugWindowModel_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.GameDefinitions.DebugWindowModel));
				    HonorRuntimeGameDefinitionsDebugWindowModel_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeGameDefinitionsDebugWindowModel_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeGameDefinitionsDebugWindowModel_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.GameDefinitions.DebugWindowModel ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeGameDefinitionsDebugWindowModel_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.GameDefinitions.DebugWindowModel val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameDefinitionsDebugWindowModel_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameDefinitions.DebugWindowModel");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.GameDefinitions.DebugWindowModel");
                }
				val = (Honor.Runtime.GameDefinitions.DebugWindowModel)e;
                
            }
            else
            {
                val = (Honor.Runtime.GameDefinitions.DebugWindowModel)objectCasters.GetCaster(typeof(Honor.Runtime.GameDefinitions.DebugWindowModel))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeGameDefinitionsDebugWindowModel(RealStatePtr L, int index, Honor.Runtime.GameDefinitions.DebugWindowModel val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeGameDefinitionsDebugWindowModel_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.GameDefinitions.DebugWindowModel");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.GameDefinitions.DebugWindowModel ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeAorTextEffectOutlineHorizontalAligmentType_TypeID = -1;
		int HonorRuntimeAorTextEffectOutlineHorizontalAligmentType_EnumRef = -1;
        
        public void PushHonorRuntimeAorTextEffectOutlineHorizontalAligmentType(RealStatePtr L, Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType val)
        {
            if (HonorRuntimeAorTextEffectOutlineHorizontalAligmentType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeAorTextEffectOutlineHorizontalAligmentType_TypeID = getTypeId(L, typeof(Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType), out is_first);
				
				if (HonorRuntimeAorTextEffectOutlineHorizontalAligmentType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType));
				    HonorRuntimeAorTextEffectOutlineHorizontalAligmentType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeAorTextEffectOutlineHorizontalAligmentType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeAorTextEffectOutlineHorizontalAligmentType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeAorTextEffectOutlineHorizontalAligmentType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeAorTextEffectOutlineHorizontalAligmentType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType");
                }
				val = (Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType)e;
                
            }
            else
            {
                val = (Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType)objectCasters.GetCaster(typeof(Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeAorTextEffectOutlineHorizontalAligmentType(RealStatePtr L, int index, Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeAorTextEffectOutlineHorizontalAligmentType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeAorTextEffectSpacingHorizontalAligmentType_TypeID = -1;
		int HonorRuntimeAorTextEffectSpacingHorizontalAligmentType_EnumRef = -1;
        
        public void PushHonorRuntimeAorTextEffectSpacingHorizontalAligmentType(RealStatePtr L, Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType val)
        {
            if (HonorRuntimeAorTextEffectSpacingHorizontalAligmentType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeAorTextEffectSpacingHorizontalAligmentType_TypeID = getTypeId(L, typeof(Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType), out is_first);
				
				if (HonorRuntimeAorTextEffectSpacingHorizontalAligmentType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType));
				    HonorRuntimeAorTextEffectSpacingHorizontalAligmentType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeAorTextEffectSpacingHorizontalAligmentType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeAorTextEffectSpacingHorizontalAligmentType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeAorTextEffectSpacingHorizontalAligmentType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeAorTextEffectSpacingHorizontalAligmentType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType");
                }
				val = (Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType)e;
                
            }
            else
            {
                val = (Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType)objectCasters.GetCaster(typeof(Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeAorTextEffectSpacingHorizontalAligmentType(RealStatePtr L, int index, Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeAorTextEffectSpacingHorizontalAligmentType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeLuaBindValueBindValueType_TypeID = -1;
		int HonorRuntimeLuaBindValueBindValueType_EnumRef = -1;
        
        public void PushHonorRuntimeLuaBindValueBindValueType(RealStatePtr L, Honor.Runtime.LuaBindValue.BindValueType val)
        {
            if (HonorRuntimeLuaBindValueBindValueType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeLuaBindValueBindValueType_TypeID = getTypeId(L, typeof(Honor.Runtime.LuaBindValue.BindValueType), out is_first);
				
				if (HonorRuntimeLuaBindValueBindValueType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.LuaBindValue.BindValueType));
				    HonorRuntimeLuaBindValueBindValueType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeLuaBindValueBindValueType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeLuaBindValueBindValueType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.LuaBindValue.BindValueType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeLuaBindValueBindValueType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.LuaBindValue.BindValueType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeLuaBindValueBindValueType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.LuaBindValue.BindValueType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.LuaBindValue.BindValueType");
                }
				val = (Honor.Runtime.LuaBindValue.BindValueType)e;
                
            }
            else
            {
                val = (Honor.Runtime.LuaBindValue.BindValueType)objectCasters.GetCaster(typeof(Honor.Runtime.LuaBindValue.BindValueType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeLuaBindValueBindValueType(RealStatePtr L, int index, Honor.Runtime.LuaBindValue.BindValueType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeLuaBindValueBindValueType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.LuaBindValue.BindValueType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.LuaBindValue.BindValueType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeLuaInjectionInjectionType_TypeID = -1;
		int HonorRuntimeLuaInjectionInjectionType_EnumRef = -1;
        
        public void PushHonorRuntimeLuaInjectionInjectionType(RealStatePtr L, Honor.Runtime.LuaInjection.InjectionType val)
        {
            if (HonorRuntimeLuaInjectionInjectionType_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeLuaInjectionInjectionType_TypeID = getTypeId(L, typeof(Honor.Runtime.LuaInjection.InjectionType), out is_first);
				
				if (HonorRuntimeLuaInjectionInjectionType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.LuaInjection.InjectionType));
				    HonorRuntimeLuaInjectionInjectionType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeLuaInjectionInjectionType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeLuaInjectionInjectionType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.LuaInjection.InjectionType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeLuaInjectionInjectionType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.LuaInjection.InjectionType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeLuaInjectionInjectionType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.LuaInjection.InjectionType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.LuaInjection.InjectionType");
                }
				val = (Honor.Runtime.LuaInjection.InjectionType)e;
                
            }
            else
            {
                val = (Honor.Runtime.LuaInjection.InjectionType)objectCasters.GetCaster(typeof(Honor.Runtime.LuaInjection.InjectionType))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeLuaInjectionInjectionType(RealStatePtr L, int index, Honor.Runtime.LuaInjection.InjectionType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeLuaInjectionInjectionType_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.LuaInjection.InjectionType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.LuaInjection.InjectionType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeUIFaderInitState_TypeID = -1;
		int HonorRuntimeUIFaderInitState_EnumRef = -1;
        
        public void PushHonorRuntimeUIFaderInitState(RealStatePtr L, Honor.Runtime.UIFader.InitState val)
        {
            if (HonorRuntimeUIFaderInitState_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeUIFaderInitState_TypeID = getTypeId(L, typeof(Honor.Runtime.UIFader.InitState), out is_first);
				
				if (HonorRuntimeUIFaderInitState_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.UIFader.InitState));
				    HonorRuntimeUIFaderInitState_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeUIFaderInitState_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeUIFaderInitState_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.UIFader.InitState ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeUIFaderInitState_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.UIFader.InitState val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeUIFaderInitState_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.UIFader.InitState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.UIFader.InitState");
                }
				val = (Honor.Runtime.UIFader.InitState)e;
                
            }
            else
            {
                val = (Honor.Runtime.UIFader.InitState)objectCasters.GetCaster(typeof(Honor.Runtime.UIFader.InitState))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeUIFaderInitState(RealStatePtr L, int index, Honor.Runtime.UIFader.InitState val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeUIFaderInitState_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.UIFader.InitState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.UIFader.InitState ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeLogLogLevel_TypeID = -1;
		int HonorRuntimeLogLogLevel_EnumRef = -1;
        
        public void PushHonorRuntimeLogLogLevel(RealStatePtr L, Honor.Runtime.Log.LogLevel val)
        {
            if (HonorRuntimeLogLogLevel_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeLogLogLevel_TypeID = getTypeId(L, typeof(Honor.Runtime.Log.LogLevel), out is_first);
				
				if (HonorRuntimeLogLogLevel_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.Log.LogLevel));
				    HonorRuntimeLogLogLevel_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeLogLogLevel_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeLogLogLevel_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.Log.LogLevel ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeLogLogLevel_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.Log.LogLevel val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeLogLogLevel_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.Log.LogLevel");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.Log.LogLevel");
                }
				val = (Honor.Runtime.Log.LogLevel)e;
                
            }
            else
            {
                val = (Honor.Runtime.Log.LogLevel)objectCasters.GetCaster(typeof(Honor.Runtime.Log.LogLevel))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeLogLogLevel(RealStatePtr L, int index, Honor.Runtime.Log.LogLevel val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeLogLogLevel_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.Log.LogLevel");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.Log.LogLevel ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int HonorRuntimeUILauncherLoadingViewLoadingMode_TypeID = -1;
		int HonorRuntimeUILauncherLoadingViewLoadingMode_EnumRef = -1;
        
        public void PushHonorRuntimeUILauncherLoadingViewLoadingMode(RealStatePtr L, Honor.Runtime.UILauncherLoadingView.LoadingMode val)
        {
            if (HonorRuntimeUILauncherLoadingViewLoadingMode_TypeID == -1)
            {
			    bool is_first;
                HonorRuntimeUILauncherLoadingViewLoadingMode_TypeID = getTypeId(L, typeof(Honor.Runtime.UILauncherLoadingView.LoadingMode), out is_first);
				
				if (HonorRuntimeUILauncherLoadingViewLoadingMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Honor.Runtime.UILauncherLoadingView.LoadingMode));
				    HonorRuntimeUILauncherLoadingViewLoadingMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, HonorRuntimeUILauncherLoadingViewLoadingMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, HonorRuntimeUILauncherLoadingViewLoadingMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Honor.Runtime.UILauncherLoadingView.LoadingMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, HonorRuntimeUILauncherLoadingViewLoadingMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Honor.Runtime.UILauncherLoadingView.LoadingMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeUILauncherLoadingViewLoadingMode_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.UILauncherLoadingView.LoadingMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Honor.Runtime.UILauncherLoadingView.LoadingMode");
                }
				val = (Honor.Runtime.UILauncherLoadingView.LoadingMode)e;
                
            }
            else
            {
                val = (Honor.Runtime.UILauncherLoadingView.LoadingMode)objectCasters.GetCaster(typeof(Honor.Runtime.UILauncherLoadingView.LoadingMode))(L, index, null);
            }
        }
		
        public void UpdateHonorRuntimeUILauncherLoadingViewLoadingMode(RealStatePtr L, int index, Honor.Runtime.UILauncherLoadingView.LoadingMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != HonorRuntimeUILauncherLoadingViewLoadingMode_TypeID)
				{
				    throw new Exception("invalid userdata for Honor.Runtime.UILauncherLoadingView.LoadingMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Honor.Runtime.UILauncherLoadingView.LoadingMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTimelineMatchTargetFields_TypeID = -1;
		int UnityEngineTimelineMatchTargetFields_EnumRef = -1;
        
        public void PushUnityEngineTimelineMatchTargetFields(RealStatePtr L, UnityEngine.Timeline.MatchTargetFields val)
        {
            if (UnityEngineTimelineMatchTargetFields_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTimelineMatchTargetFields_TypeID = getTypeId(L, typeof(UnityEngine.Timeline.MatchTargetFields), out is_first);
				
				if (UnityEngineTimelineMatchTargetFields_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Timeline.MatchTargetFields));
				    UnityEngineTimelineMatchTargetFields_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTimelineMatchTargetFields_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTimelineMatchTargetFields_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Timeline.MatchTargetFields ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTimelineMatchTargetFields_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Timeline.MatchTargetFields val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineMatchTargetFields_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.MatchTargetFields");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Timeline.MatchTargetFields");
                }
				val = (UnityEngine.Timeline.MatchTargetFields)e;
                
            }
            else
            {
                val = (UnityEngine.Timeline.MatchTargetFields)objectCasters.GetCaster(typeof(UnityEngine.Timeline.MatchTargetFields))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTimelineMatchTargetFields(RealStatePtr L, int index, UnityEngine.Timeline.MatchTargetFields val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineMatchTargetFields_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.MatchTargetFields");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Timeline.MatchTargetFields ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTimelineTrackOffset_TypeID = -1;
		int UnityEngineTimelineTrackOffset_EnumRef = -1;
        
        public void PushUnityEngineTimelineTrackOffset(RealStatePtr L, UnityEngine.Timeline.TrackOffset val)
        {
            if (UnityEngineTimelineTrackOffset_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTimelineTrackOffset_TypeID = getTypeId(L, typeof(UnityEngine.Timeline.TrackOffset), out is_first);
				
				if (UnityEngineTimelineTrackOffset_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Timeline.TrackOffset));
				    UnityEngineTimelineTrackOffset_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTimelineTrackOffset_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTimelineTrackOffset_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Timeline.TrackOffset ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTimelineTrackOffset_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Timeline.TrackOffset val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineTrackOffset_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.TrackOffset");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Timeline.TrackOffset");
                }
				val = (UnityEngine.Timeline.TrackOffset)e;
                
            }
            else
            {
                val = (UnityEngine.Timeline.TrackOffset)objectCasters.GetCaster(typeof(UnityEngine.Timeline.TrackOffset))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTimelineTrackOffset(RealStatePtr L, int index, UnityEngine.Timeline.TrackOffset val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineTrackOffset_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.TrackOffset");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Timeline.TrackOffset ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTimelineClipCaps_TypeID = -1;
		int UnityEngineTimelineClipCaps_EnumRef = -1;
        
        public void PushUnityEngineTimelineClipCaps(RealStatePtr L, UnityEngine.Timeline.ClipCaps val)
        {
            if (UnityEngineTimelineClipCaps_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTimelineClipCaps_TypeID = getTypeId(L, typeof(UnityEngine.Timeline.ClipCaps), out is_first);
				
				if (UnityEngineTimelineClipCaps_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Timeline.ClipCaps));
				    UnityEngineTimelineClipCaps_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTimelineClipCaps_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTimelineClipCaps_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Timeline.ClipCaps ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTimelineClipCaps_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Timeline.ClipCaps val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineClipCaps_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.ClipCaps");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Timeline.ClipCaps");
                }
				val = (UnityEngine.Timeline.ClipCaps)e;
                
            }
            else
            {
                val = (UnityEngine.Timeline.ClipCaps)objectCasters.GetCaster(typeof(UnityEngine.Timeline.ClipCaps))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTimelineClipCaps(RealStatePtr L, int index, UnityEngine.Timeline.ClipCaps val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineClipCaps_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.ClipCaps");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Timeline.ClipCaps ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTimelineNotificationFlags_TypeID = -1;
		int UnityEngineTimelineNotificationFlags_EnumRef = -1;
        
        public void PushUnityEngineTimelineNotificationFlags(RealStatePtr L, UnityEngine.Timeline.NotificationFlags val)
        {
            if (UnityEngineTimelineNotificationFlags_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTimelineNotificationFlags_TypeID = getTypeId(L, typeof(UnityEngine.Timeline.NotificationFlags), out is_first);
				
				if (UnityEngineTimelineNotificationFlags_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Timeline.NotificationFlags));
				    UnityEngineTimelineNotificationFlags_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTimelineNotificationFlags_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTimelineNotificationFlags_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Timeline.NotificationFlags ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTimelineNotificationFlags_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Timeline.NotificationFlags val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineNotificationFlags_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.NotificationFlags");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Timeline.NotificationFlags");
                }
				val = (UnityEngine.Timeline.NotificationFlags)e;
                
            }
            else
            {
                val = (UnityEngine.Timeline.NotificationFlags)objectCasters.GetCaster(typeof(UnityEngine.Timeline.NotificationFlags))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTimelineNotificationFlags(RealStatePtr L, int index, UnityEngine.Timeline.NotificationFlags val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineNotificationFlags_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.NotificationFlags");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Timeline.NotificationFlags ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTimelineTrackBindingFlags_TypeID = -1;
		int UnityEngineTimelineTrackBindingFlags_EnumRef = -1;
        
        public void PushUnityEngineTimelineTrackBindingFlags(RealStatePtr L, UnityEngine.Timeline.TrackBindingFlags val)
        {
            if (UnityEngineTimelineTrackBindingFlags_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTimelineTrackBindingFlags_TypeID = getTypeId(L, typeof(UnityEngine.Timeline.TrackBindingFlags), out is_first);
				
				if (UnityEngineTimelineTrackBindingFlags_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Timeline.TrackBindingFlags));
				    UnityEngineTimelineTrackBindingFlags_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTimelineTrackBindingFlags_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTimelineTrackBindingFlags_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Timeline.TrackBindingFlags ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTimelineTrackBindingFlags_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Timeline.TrackBindingFlags val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineTrackBindingFlags_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.TrackBindingFlags");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Timeline.TrackBindingFlags");
                }
				val = (UnityEngine.Timeline.TrackBindingFlags)e;
                
            }
            else
            {
                val = (UnityEngine.Timeline.TrackBindingFlags)objectCasters.GetCaster(typeof(UnityEngine.Timeline.TrackBindingFlags))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTimelineTrackBindingFlags(RealStatePtr L, int index, UnityEngine.Timeline.TrackBindingFlags val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineTrackBindingFlags_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.TrackBindingFlags");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Timeline.TrackBindingFlags ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTimelineStandardFrameRates_TypeID = -1;
		int UnityEngineTimelineStandardFrameRates_EnumRef = -1;
        
        public void PushUnityEngineTimelineStandardFrameRates(RealStatePtr L, UnityEngine.Timeline.StandardFrameRates val)
        {
            if (UnityEngineTimelineStandardFrameRates_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTimelineStandardFrameRates_TypeID = getTypeId(L, typeof(UnityEngine.Timeline.StandardFrameRates), out is_first);
				
				if (UnityEngineTimelineStandardFrameRates_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Timeline.StandardFrameRates));
				    UnityEngineTimelineStandardFrameRates_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTimelineStandardFrameRates_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTimelineStandardFrameRates_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Timeline.StandardFrameRates ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTimelineStandardFrameRates_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Timeline.StandardFrameRates val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineStandardFrameRates_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.StandardFrameRates");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Timeline.StandardFrameRates");
                }
				val = (UnityEngine.Timeline.StandardFrameRates)e;
                
            }
            else
            {
                val = (UnityEngine.Timeline.StandardFrameRates)objectCasters.GetCaster(typeof(UnityEngine.Timeline.StandardFrameRates))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTimelineStandardFrameRates(RealStatePtr L, int index, UnityEngine.Timeline.StandardFrameRates val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineStandardFrameRates_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.StandardFrameRates");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Timeline.StandardFrameRates ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTimelineActivationTrackPostPlaybackState_TypeID = -1;
		int UnityEngineTimelineActivationTrackPostPlaybackState_EnumRef = -1;
        
        public void PushUnityEngineTimelineActivationTrackPostPlaybackState(RealStatePtr L, UnityEngine.Timeline.ActivationTrack.PostPlaybackState val)
        {
            if (UnityEngineTimelineActivationTrackPostPlaybackState_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTimelineActivationTrackPostPlaybackState_TypeID = getTypeId(L, typeof(UnityEngine.Timeline.ActivationTrack.PostPlaybackState), out is_first);
				
				if (UnityEngineTimelineActivationTrackPostPlaybackState_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Timeline.ActivationTrack.PostPlaybackState));
				    UnityEngineTimelineActivationTrackPostPlaybackState_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTimelineActivationTrackPostPlaybackState_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTimelineActivationTrackPostPlaybackState_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Timeline.ActivationTrack.PostPlaybackState ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTimelineActivationTrackPostPlaybackState_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Timeline.ActivationTrack.PostPlaybackState val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineActivationTrackPostPlaybackState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.ActivationTrack.PostPlaybackState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Timeline.ActivationTrack.PostPlaybackState");
                }
				val = (UnityEngine.Timeline.ActivationTrack.PostPlaybackState)e;
                
            }
            else
            {
                val = (UnityEngine.Timeline.ActivationTrack.PostPlaybackState)objectCasters.GetCaster(typeof(UnityEngine.Timeline.ActivationTrack.PostPlaybackState))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTimelineActivationTrackPostPlaybackState(RealStatePtr L, int index, UnityEngine.Timeline.ActivationTrack.PostPlaybackState val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineActivationTrackPostPlaybackState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.ActivationTrack.PostPlaybackState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Timeline.ActivationTrack.PostPlaybackState ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTimelineAnimationPlayableAssetLoopMode_TypeID = -1;
		int UnityEngineTimelineAnimationPlayableAssetLoopMode_EnumRef = -1;
        
        public void PushUnityEngineTimelineAnimationPlayableAssetLoopMode(RealStatePtr L, UnityEngine.Timeline.AnimationPlayableAsset.LoopMode val)
        {
            if (UnityEngineTimelineAnimationPlayableAssetLoopMode_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTimelineAnimationPlayableAssetLoopMode_TypeID = getTypeId(L, typeof(UnityEngine.Timeline.AnimationPlayableAsset.LoopMode), out is_first);
				
				if (UnityEngineTimelineAnimationPlayableAssetLoopMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Timeline.AnimationPlayableAsset.LoopMode));
				    UnityEngineTimelineAnimationPlayableAssetLoopMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTimelineAnimationPlayableAssetLoopMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTimelineAnimationPlayableAssetLoopMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Timeline.AnimationPlayableAsset.LoopMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTimelineAnimationPlayableAssetLoopMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Timeline.AnimationPlayableAsset.LoopMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineAnimationPlayableAssetLoopMode_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.AnimationPlayableAsset.LoopMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Timeline.AnimationPlayableAsset.LoopMode");
                }
				val = (UnityEngine.Timeline.AnimationPlayableAsset.LoopMode)e;
                
            }
            else
            {
                val = (UnityEngine.Timeline.AnimationPlayableAsset.LoopMode)objectCasters.GetCaster(typeof(UnityEngine.Timeline.AnimationPlayableAsset.LoopMode))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTimelineAnimationPlayableAssetLoopMode(RealStatePtr L, int index, UnityEngine.Timeline.AnimationPlayableAsset.LoopMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineAnimationPlayableAssetLoopMode_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.AnimationPlayableAsset.LoopMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Timeline.AnimationPlayableAsset.LoopMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTimelineTimelineClipClipExtrapolation_TypeID = -1;
		int UnityEngineTimelineTimelineClipClipExtrapolation_EnumRef = -1;
        
        public void PushUnityEngineTimelineTimelineClipClipExtrapolation(RealStatePtr L, UnityEngine.Timeline.TimelineClip.ClipExtrapolation val)
        {
            if (UnityEngineTimelineTimelineClipClipExtrapolation_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTimelineTimelineClipClipExtrapolation_TypeID = getTypeId(L, typeof(UnityEngine.Timeline.TimelineClip.ClipExtrapolation), out is_first);
				
				if (UnityEngineTimelineTimelineClipClipExtrapolation_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Timeline.TimelineClip.ClipExtrapolation));
				    UnityEngineTimelineTimelineClipClipExtrapolation_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTimelineTimelineClipClipExtrapolation_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTimelineTimelineClipClipExtrapolation_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Timeline.TimelineClip.ClipExtrapolation ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTimelineTimelineClipClipExtrapolation_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Timeline.TimelineClip.ClipExtrapolation val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineTimelineClipClipExtrapolation_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.TimelineClip.ClipExtrapolation");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Timeline.TimelineClip.ClipExtrapolation");
                }
				val = (UnityEngine.Timeline.TimelineClip.ClipExtrapolation)e;
                
            }
            else
            {
                val = (UnityEngine.Timeline.TimelineClip.ClipExtrapolation)objectCasters.GetCaster(typeof(UnityEngine.Timeline.TimelineClip.ClipExtrapolation))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTimelineTimelineClipClipExtrapolation(RealStatePtr L, int index, UnityEngine.Timeline.TimelineClip.ClipExtrapolation val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineTimelineClipClipExtrapolation_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.TimelineClip.ClipExtrapolation");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Timeline.TimelineClip.ClipExtrapolation ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTimelineTimelineClipBlendCurveMode_TypeID = -1;
		int UnityEngineTimelineTimelineClipBlendCurveMode_EnumRef = -1;
        
        public void PushUnityEngineTimelineTimelineClipBlendCurveMode(RealStatePtr L, UnityEngine.Timeline.TimelineClip.BlendCurveMode val)
        {
            if (UnityEngineTimelineTimelineClipBlendCurveMode_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTimelineTimelineClipBlendCurveMode_TypeID = getTypeId(L, typeof(UnityEngine.Timeline.TimelineClip.BlendCurveMode), out is_first);
				
				if (UnityEngineTimelineTimelineClipBlendCurveMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Timeline.TimelineClip.BlendCurveMode));
				    UnityEngineTimelineTimelineClipBlendCurveMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTimelineTimelineClipBlendCurveMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTimelineTimelineClipBlendCurveMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Timeline.TimelineClip.BlendCurveMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTimelineTimelineClipBlendCurveMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Timeline.TimelineClip.BlendCurveMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineTimelineClipBlendCurveMode_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.TimelineClip.BlendCurveMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Timeline.TimelineClip.BlendCurveMode");
                }
				val = (UnityEngine.Timeline.TimelineClip.BlendCurveMode)e;
                
            }
            else
            {
                val = (UnityEngine.Timeline.TimelineClip.BlendCurveMode)objectCasters.GetCaster(typeof(UnityEngine.Timeline.TimelineClip.BlendCurveMode))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTimelineTimelineClipBlendCurveMode(RealStatePtr L, int index, UnityEngine.Timeline.TimelineClip.BlendCurveMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineTimelineClipBlendCurveMode_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.TimelineClip.BlendCurveMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Timeline.TimelineClip.BlendCurveMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTimelineTimelineAssetDurationMode_TypeID = -1;
		int UnityEngineTimelineTimelineAssetDurationMode_EnumRef = -1;
        
        public void PushUnityEngineTimelineTimelineAssetDurationMode(RealStatePtr L, UnityEngine.Timeline.TimelineAsset.DurationMode val)
        {
            if (UnityEngineTimelineTimelineAssetDurationMode_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTimelineTimelineAssetDurationMode_TypeID = getTypeId(L, typeof(UnityEngine.Timeline.TimelineAsset.DurationMode), out is_first);
				
				if (UnityEngineTimelineTimelineAssetDurationMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Timeline.TimelineAsset.DurationMode));
				    UnityEngineTimelineTimelineAssetDurationMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTimelineTimelineAssetDurationMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTimelineTimelineAssetDurationMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Timeline.TimelineAsset.DurationMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTimelineTimelineAssetDurationMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Timeline.TimelineAsset.DurationMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineTimelineAssetDurationMode_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.TimelineAsset.DurationMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Timeline.TimelineAsset.DurationMode");
                }
				val = (UnityEngine.Timeline.TimelineAsset.DurationMode)e;
                
            }
            else
            {
                val = (UnityEngine.Timeline.TimelineAsset.DurationMode)objectCasters.GetCaster(typeof(UnityEngine.Timeline.TimelineAsset.DurationMode))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTimelineTimelineAssetDurationMode(RealStatePtr L, int index, UnityEngine.Timeline.TimelineAsset.DurationMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineTimelineAssetDurationMode_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.TimelineAsset.DurationMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Timeline.TimelineAsset.DurationMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineTimelineActivationControlPlayablePostPlaybackState_TypeID = -1;
		int UnityEngineTimelineActivationControlPlayablePostPlaybackState_EnumRef = -1;
        
        public void PushUnityEngineTimelineActivationControlPlayablePostPlaybackState(RealStatePtr L, UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState val)
        {
            if (UnityEngineTimelineActivationControlPlayablePostPlaybackState_TypeID == -1)
            {
			    bool is_first;
                UnityEngineTimelineActivationControlPlayablePostPlaybackState_TypeID = getTypeId(L, typeof(UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState), out is_first);
				
				if (UnityEngineTimelineActivationControlPlayablePostPlaybackState_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState));
				    UnityEngineTimelineActivationControlPlayablePostPlaybackState_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, UnityEngineTimelineActivationControlPlayablePostPlaybackState_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, UnityEngineTimelineActivationControlPlayablePostPlaybackState_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState ,value="+val);
            }
			
			LuaAPI.lua_getref(L, UnityEngineTimelineActivationControlPlayablePostPlaybackState_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineActivationControlPlayablePostPlaybackState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState");
                }
				val = (UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState)e;
                
            }
            else
            {
                val = (UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState)objectCasters.GetCaster(typeof(UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineTimelineActivationControlPlayablePostPlaybackState(RealStatePtr L, int index, UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineTimelineActivationControlPlayablePostPlaybackState_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int SpineUnityUpdateMode_TypeID = -1;
		int SpineUnityUpdateMode_EnumRef = -1;
        
        public void PushSpineUnityUpdateMode(RealStatePtr L, Spine.Unity.UpdateMode val)
        {
            if (SpineUnityUpdateMode_TypeID == -1)
            {
			    bool is_first;
                SpineUnityUpdateMode_TypeID = getTypeId(L, typeof(Spine.Unity.UpdateMode), out is_first);
				
				if (SpineUnityUpdateMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Spine.Unity.UpdateMode));
				    SpineUnityUpdateMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, SpineUnityUpdateMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, SpineUnityUpdateMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Spine.Unity.UpdateMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, SpineUnityUpdateMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Spine.Unity.UpdateMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnityUpdateMode_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.UpdateMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Spine.Unity.UpdateMode");
                }
				val = (Spine.Unity.UpdateMode)e;
                
            }
            else
            {
                val = (Spine.Unity.UpdateMode)objectCasters.GetCaster(typeof(Spine.Unity.UpdateMode))(L, index, null);
            }
        }
		
        public void UpdateSpineUnityUpdateMode(RealStatePtr L, int index, Spine.Unity.UpdateMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnityUpdateMode_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.UpdateMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Spine.Unity.UpdateMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int SpineUnityUpdateTiming_TypeID = -1;
		int SpineUnityUpdateTiming_EnumRef = -1;
        
        public void PushSpineUnityUpdateTiming(RealStatePtr L, Spine.Unity.UpdateTiming val)
        {
            if (SpineUnityUpdateTiming_TypeID == -1)
            {
			    bool is_first;
                SpineUnityUpdateTiming_TypeID = getTypeId(L, typeof(Spine.Unity.UpdateTiming), out is_first);
				
				if (SpineUnityUpdateTiming_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Spine.Unity.UpdateTiming));
				    SpineUnityUpdateTiming_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, SpineUnityUpdateTiming_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, SpineUnityUpdateTiming_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Spine.Unity.UpdateTiming ,value="+val);
            }
			
			LuaAPI.lua_getref(L, SpineUnityUpdateTiming_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Spine.Unity.UpdateTiming val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnityUpdateTiming_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.UpdateTiming");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Spine.Unity.UpdateTiming");
                }
				val = (Spine.Unity.UpdateTiming)e;
                
            }
            else
            {
                val = (Spine.Unity.UpdateTiming)objectCasters.GetCaster(typeof(Spine.Unity.UpdateTiming))(L, index, null);
            }
        }
		
        public void UpdateSpineUnityUpdateTiming(RealStatePtr L, int index, Spine.Unity.UpdateTiming val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnityUpdateTiming_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.UpdateTiming");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Spine.Unity.UpdateTiming ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int SpineUnitySettingsTriState_TypeID = -1;
		int SpineUnitySettingsTriState_EnumRef = -1;
        
        public void PushSpineUnitySettingsTriState(RealStatePtr L, Spine.Unity.SettingsTriState val)
        {
            if (SpineUnitySettingsTriState_TypeID == -1)
            {
			    bool is_first;
                SpineUnitySettingsTriState_TypeID = getTypeId(L, typeof(Spine.Unity.SettingsTriState), out is_first);
				
				if (SpineUnitySettingsTriState_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Spine.Unity.SettingsTriState));
				    SpineUnitySettingsTriState_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, SpineUnitySettingsTriState_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, SpineUnitySettingsTriState_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Spine.Unity.SettingsTriState ,value="+val);
            }
			
			LuaAPI.lua_getref(L, SpineUnitySettingsTriState_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Spine.Unity.SettingsTriState val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnitySettingsTriState_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.SettingsTriState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Spine.Unity.SettingsTriState");
                }
				val = (Spine.Unity.SettingsTriState)e;
                
            }
            else
            {
                val = (Spine.Unity.SettingsTriState)objectCasters.GetCaster(typeof(Spine.Unity.SettingsTriState))(L, index, null);
            }
        }
		
        public void UpdateSpineUnitySettingsTriState(RealStatePtr L, int index, Spine.Unity.SettingsTriState val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnitySettingsTriState_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.SettingsTriState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Spine.Unity.SettingsTriState ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int SpineUnityBoneFollowerAxisOrientation_TypeID = -1;
		int SpineUnityBoneFollowerAxisOrientation_EnumRef = -1;
        
        public void PushSpineUnityBoneFollowerAxisOrientation(RealStatePtr L, Spine.Unity.BoneFollower.AxisOrientation val)
        {
            if (SpineUnityBoneFollowerAxisOrientation_TypeID == -1)
            {
			    bool is_first;
                SpineUnityBoneFollowerAxisOrientation_TypeID = getTypeId(L, typeof(Spine.Unity.BoneFollower.AxisOrientation), out is_first);
				
				if (SpineUnityBoneFollowerAxisOrientation_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Spine.Unity.BoneFollower.AxisOrientation));
				    SpineUnityBoneFollowerAxisOrientation_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, SpineUnityBoneFollowerAxisOrientation_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, SpineUnityBoneFollowerAxisOrientation_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Spine.Unity.BoneFollower.AxisOrientation ,value="+val);
            }
			
			LuaAPI.lua_getref(L, SpineUnityBoneFollowerAxisOrientation_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Spine.Unity.BoneFollower.AxisOrientation val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnityBoneFollowerAxisOrientation_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.BoneFollower.AxisOrientation");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Spine.Unity.BoneFollower.AxisOrientation");
                }
				val = (Spine.Unity.BoneFollower.AxisOrientation)e;
                
            }
            else
            {
                val = (Spine.Unity.BoneFollower.AxisOrientation)objectCasters.GetCaster(typeof(Spine.Unity.BoneFollower.AxisOrientation))(L, index, null);
            }
        }
		
        public void UpdateSpineUnityBoneFollowerAxisOrientation(RealStatePtr L, int index, Spine.Unity.BoneFollower.AxisOrientation val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnityBoneFollowerAxisOrientation_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.BoneFollower.AxisOrientation");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Spine.Unity.BoneFollower.AxisOrientation ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int SpineUnitySkeletonGraphicLayoutMode_TypeID = -1;
		int SpineUnitySkeletonGraphicLayoutMode_EnumRef = -1;
        
        public void PushSpineUnitySkeletonGraphicLayoutMode(RealStatePtr L, Spine.Unity.SkeletonGraphic.LayoutMode val)
        {
            if (SpineUnitySkeletonGraphicLayoutMode_TypeID == -1)
            {
			    bool is_first;
                SpineUnitySkeletonGraphicLayoutMode_TypeID = getTypeId(L, typeof(Spine.Unity.SkeletonGraphic.LayoutMode), out is_first);
				
				if (SpineUnitySkeletonGraphicLayoutMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Spine.Unity.SkeletonGraphic.LayoutMode));
				    SpineUnitySkeletonGraphicLayoutMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, SpineUnitySkeletonGraphicLayoutMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, SpineUnitySkeletonGraphicLayoutMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Spine.Unity.SkeletonGraphic.LayoutMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, SpineUnitySkeletonGraphicLayoutMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Spine.Unity.SkeletonGraphic.LayoutMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnitySkeletonGraphicLayoutMode_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.SkeletonGraphic.LayoutMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Spine.Unity.SkeletonGraphic.LayoutMode");
                }
				val = (Spine.Unity.SkeletonGraphic.LayoutMode)e;
                
            }
            else
            {
                val = (Spine.Unity.SkeletonGraphic.LayoutMode)objectCasters.GetCaster(typeof(Spine.Unity.SkeletonGraphic.LayoutMode))(L, index, null);
            }
        }
		
        public void UpdateSpineUnitySkeletonGraphicLayoutMode(RealStatePtr L, int index, Spine.Unity.SkeletonGraphic.LayoutMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnitySkeletonGraphicLayoutMode_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.SkeletonGraphic.LayoutMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Spine.Unity.SkeletonGraphic.LayoutMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int SpineUnitySkeletonUtilityBoneMode_TypeID = -1;
		int SpineUnitySkeletonUtilityBoneMode_EnumRef = -1;
        
        public void PushSpineUnitySkeletonUtilityBoneMode(RealStatePtr L, Spine.Unity.SkeletonUtilityBone.Mode val)
        {
            if (SpineUnitySkeletonUtilityBoneMode_TypeID == -1)
            {
			    bool is_first;
                SpineUnitySkeletonUtilityBoneMode_TypeID = getTypeId(L, typeof(Spine.Unity.SkeletonUtilityBone.Mode), out is_first);
				
				if (SpineUnitySkeletonUtilityBoneMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Spine.Unity.SkeletonUtilityBone.Mode));
				    SpineUnitySkeletonUtilityBoneMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, SpineUnitySkeletonUtilityBoneMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, SpineUnitySkeletonUtilityBoneMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Spine.Unity.SkeletonUtilityBone.Mode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, SpineUnitySkeletonUtilityBoneMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Spine.Unity.SkeletonUtilityBone.Mode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnitySkeletonUtilityBoneMode_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.SkeletonUtilityBone.Mode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Spine.Unity.SkeletonUtilityBone.Mode");
                }
				val = (Spine.Unity.SkeletonUtilityBone.Mode)e;
                
            }
            else
            {
                val = (Spine.Unity.SkeletonUtilityBone.Mode)objectCasters.GetCaster(typeof(Spine.Unity.SkeletonUtilityBone.Mode))(L, index, null);
            }
        }
		
        public void UpdateSpineUnitySkeletonUtilityBoneMode(RealStatePtr L, int index, Spine.Unity.SkeletonUtilityBone.Mode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnitySkeletonUtilityBoneMode_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.SkeletonUtilityBone.Mode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Spine.Unity.SkeletonUtilityBone.Mode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int SpineUnitySkeletonUtilityBoneUpdatePhase_TypeID = -1;
		int SpineUnitySkeletonUtilityBoneUpdatePhase_EnumRef = -1;
        
        public void PushSpineUnitySkeletonUtilityBoneUpdatePhase(RealStatePtr L, Spine.Unity.SkeletonUtilityBone.UpdatePhase val)
        {
            if (SpineUnitySkeletonUtilityBoneUpdatePhase_TypeID == -1)
            {
			    bool is_first;
                SpineUnitySkeletonUtilityBoneUpdatePhase_TypeID = getTypeId(L, typeof(Spine.Unity.SkeletonUtilityBone.UpdatePhase), out is_first);
				
				if (SpineUnitySkeletonUtilityBoneUpdatePhase_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Spine.Unity.SkeletonUtilityBone.UpdatePhase));
				    SpineUnitySkeletonUtilityBoneUpdatePhase_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, SpineUnitySkeletonUtilityBoneUpdatePhase_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, SpineUnitySkeletonUtilityBoneUpdatePhase_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Spine.Unity.SkeletonUtilityBone.UpdatePhase ,value="+val);
            }
			
			LuaAPI.lua_getref(L, SpineUnitySkeletonUtilityBoneUpdatePhase_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Spine.Unity.SkeletonUtilityBone.UpdatePhase val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnitySkeletonUtilityBoneUpdatePhase_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.SkeletonUtilityBone.UpdatePhase");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Spine.Unity.SkeletonUtilityBone.UpdatePhase");
                }
				val = (Spine.Unity.SkeletonUtilityBone.UpdatePhase)e;
                
            }
            else
            {
                val = (Spine.Unity.SkeletonUtilityBone.UpdatePhase)objectCasters.GetCaster(typeof(Spine.Unity.SkeletonUtilityBone.UpdatePhase))(L, index, null);
            }
        }
		
        public void UpdateSpineUnitySkeletonUtilityBoneUpdatePhase(RealStatePtr L, int index, Spine.Unity.SkeletonUtilityBone.UpdatePhase val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnitySkeletonUtilityBoneUpdatePhase_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.SkeletonUtilityBone.UpdatePhase");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Spine.Unity.SkeletonUtilityBone.UpdatePhase ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int SpineUnitySkeletonMecanimMecanimTranslatorMixMode_TypeID = -1;
		int SpineUnitySkeletonMecanimMecanimTranslatorMixMode_EnumRef = -1;
        
        public void PushSpineUnitySkeletonMecanimMecanimTranslatorMixMode(RealStatePtr L, Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode val)
        {
            if (SpineUnitySkeletonMecanimMecanimTranslatorMixMode_TypeID == -1)
            {
			    bool is_first;
                SpineUnitySkeletonMecanimMecanimTranslatorMixMode_TypeID = getTypeId(L, typeof(Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode), out is_first);
				
				if (SpineUnitySkeletonMecanimMecanimTranslatorMixMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode));
				    SpineUnitySkeletonMecanimMecanimTranslatorMixMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, SpineUnitySkeletonMecanimMecanimTranslatorMixMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, SpineUnitySkeletonMecanimMecanimTranslatorMixMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, SpineUnitySkeletonMecanimMecanimTranslatorMixMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnitySkeletonMecanimMecanimTranslatorMixMode_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode");
                }
				val = (Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode)e;
                
            }
            else
            {
                val = (Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode)objectCasters.GetCaster(typeof(Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode))(L, index, null);
            }
        }
		
        public void UpdateSpineUnitySkeletonMecanimMecanimTranslatorMixMode(RealStatePtr L, int index, Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != SpineUnitySkeletonMecanimMecanimTranslatorMixMode_TypeID)
				{
				    throw new Exception("invalid userdata for Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningAutoPlay_TypeID = -1;
		int DGTweeningAutoPlay_EnumRef = -1;
        
        public void PushDGTweeningAutoPlay(RealStatePtr L, DG.Tweening.AutoPlay val)
        {
            if (DGTweeningAutoPlay_TypeID == -1)
            {
			    bool is_first;
                DGTweeningAutoPlay_TypeID = getTypeId(L, typeof(DG.Tweening.AutoPlay), out is_first);
				
				if (DGTweeningAutoPlay_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.AutoPlay));
				    DGTweeningAutoPlay_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningAutoPlay_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningAutoPlay_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.AutoPlay ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningAutoPlay_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.AutoPlay val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningAutoPlay_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.AutoPlay");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.AutoPlay");
                }
				val = (DG.Tweening.AutoPlay)e;
                
            }
            else
            {
                val = (DG.Tweening.AutoPlay)objectCasters.GetCaster(typeof(DG.Tweening.AutoPlay))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningAutoPlay(RealStatePtr L, int index, DG.Tweening.AutoPlay val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningAutoPlay_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.AutoPlay");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.AutoPlay ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningAxisConstraint_TypeID = -1;
		int DGTweeningAxisConstraint_EnumRef = -1;
        
        public void PushDGTweeningAxisConstraint(RealStatePtr L, DG.Tweening.AxisConstraint val)
        {
            if (DGTweeningAxisConstraint_TypeID == -1)
            {
			    bool is_first;
                DGTweeningAxisConstraint_TypeID = getTypeId(L, typeof(DG.Tweening.AxisConstraint), out is_first);
				
				if (DGTweeningAxisConstraint_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.AxisConstraint));
				    DGTweeningAxisConstraint_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningAxisConstraint_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningAxisConstraint_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.AxisConstraint ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningAxisConstraint_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.AxisConstraint val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningAxisConstraint_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.AxisConstraint");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.AxisConstraint");
                }
				val = (DG.Tweening.AxisConstraint)e;
                
            }
            else
            {
                val = (DG.Tweening.AxisConstraint)objectCasters.GetCaster(typeof(DG.Tweening.AxisConstraint))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningAxisConstraint(RealStatePtr L, int index, DG.Tweening.AxisConstraint val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningAxisConstraint_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.AxisConstraint");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.AxisConstraint ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningEase_TypeID = -1;
		int DGTweeningEase_EnumRef = -1;
        
        public void PushDGTweeningEase(RealStatePtr L, DG.Tweening.Ease val)
        {
            if (DGTweeningEase_TypeID == -1)
            {
			    bool is_first;
                DGTweeningEase_TypeID = getTypeId(L, typeof(DG.Tweening.Ease), out is_first);
				
				if (DGTweeningEase_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.Ease));
				    DGTweeningEase_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningEase_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningEase_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.Ease ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningEase_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.Ease val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningEase_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.Ease");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.Ease");
                }
				val = (DG.Tweening.Ease)e;
                
            }
            else
            {
                val = (DG.Tweening.Ease)objectCasters.GetCaster(typeof(DG.Tweening.Ease))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningEase(RealStatePtr L, int index, DG.Tweening.Ease val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningEase_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.Ease");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.Ease ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningPathMode_TypeID = -1;
		int DGTweeningPathMode_EnumRef = -1;
        
        public void PushDGTweeningPathMode(RealStatePtr L, DG.Tweening.PathMode val)
        {
            if (DGTweeningPathMode_TypeID == -1)
            {
			    bool is_first;
                DGTweeningPathMode_TypeID = getTypeId(L, typeof(DG.Tweening.PathMode), out is_first);
				
				if (DGTweeningPathMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.PathMode));
				    DGTweeningPathMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningPathMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningPathMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.PathMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningPathMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.PathMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningPathMode_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.PathMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.PathMode");
                }
				val = (DG.Tweening.PathMode)e;
                
            }
            else
            {
                val = (DG.Tweening.PathMode)objectCasters.GetCaster(typeof(DG.Tweening.PathMode))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningPathMode(RealStatePtr L, int index, DG.Tweening.PathMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningPathMode_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.PathMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.PathMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningPathType_TypeID = -1;
		int DGTweeningPathType_EnumRef = -1;
        
        public void PushDGTweeningPathType(RealStatePtr L, DG.Tweening.PathType val)
        {
            if (DGTweeningPathType_TypeID == -1)
            {
			    bool is_first;
                DGTweeningPathType_TypeID = getTypeId(L, typeof(DG.Tweening.PathType), out is_first);
				
				if (DGTweeningPathType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.PathType));
				    DGTweeningPathType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningPathType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningPathType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.PathType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningPathType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.PathType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningPathType_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.PathType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.PathType");
                }
				val = (DG.Tweening.PathType)e;
                
            }
            else
            {
                val = (DG.Tweening.PathType)objectCasters.GetCaster(typeof(DG.Tweening.PathType))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningPathType(RealStatePtr L, int index, DG.Tweening.PathType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningPathType_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.PathType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.PathType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningRotateMode_TypeID = -1;
		int DGTweeningRotateMode_EnumRef = -1;
        
        public void PushDGTweeningRotateMode(RealStatePtr L, DG.Tweening.RotateMode val)
        {
            if (DGTweeningRotateMode_TypeID == -1)
            {
			    bool is_first;
                DGTweeningRotateMode_TypeID = getTypeId(L, typeof(DG.Tweening.RotateMode), out is_first);
				
				if (DGTweeningRotateMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.RotateMode));
				    DGTweeningRotateMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningRotateMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningRotateMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.RotateMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningRotateMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.RotateMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningRotateMode_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.RotateMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.RotateMode");
                }
				val = (DG.Tweening.RotateMode)e;
                
            }
            else
            {
                val = (DG.Tweening.RotateMode)objectCasters.GetCaster(typeof(DG.Tweening.RotateMode))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningRotateMode(RealStatePtr L, int index, DG.Tweening.RotateMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningRotateMode_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.RotateMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.RotateMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningScrambleMode_TypeID = -1;
		int DGTweeningScrambleMode_EnumRef = -1;
        
        public void PushDGTweeningScrambleMode(RealStatePtr L, DG.Tweening.ScrambleMode val)
        {
            if (DGTweeningScrambleMode_TypeID == -1)
            {
			    bool is_first;
                DGTweeningScrambleMode_TypeID = getTypeId(L, typeof(DG.Tweening.ScrambleMode), out is_first);
				
				if (DGTweeningScrambleMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.ScrambleMode));
				    DGTweeningScrambleMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningScrambleMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningScrambleMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.ScrambleMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningScrambleMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.ScrambleMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningScrambleMode_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.ScrambleMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.ScrambleMode");
                }
				val = (DG.Tweening.ScrambleMode)e;
                
            }
            else
            {
                val = (DG.Tweening.ScrambleMode)objectCasters.GetCaster(typeof(DG.Tweening.ScrambleMode))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningScrambleMode(RealStatePtr L, int index, DG.Tweening.ScrambleMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningScrambleMode_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.ScrambleMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.ScrambleMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningLoopType_TypeID = -1;
		int DGTweeningLoopType_EnumRef = -1;
        
        public void PushDGTweeningLoopType(RealStatePtr L, DG.Tweening.LoopType val)
        {
            if (DGTweeningLoopType_TypeID == -1)
            {
			    bool is_first;
                DGTweeningLoopType_TypeID = getTypeId(L, typeof(DG.Tweening.LoopType), out is_first);
				
				if (DGTweeningLoopType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.LoopType));
				    DGTweeningLoopType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningLoopType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningLoopType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.LoopType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningLoopType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.LoopType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningLoopType_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.LoopType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.LoopType");
                }
				val = (DG.Tweening.LoopType)e;
                
            }
            else
            {
                val = (DG.Tweening.LoopType)objectCasters.GetCaster(typeof(DG.Tweening.LoopType))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningLoopType(RealStatePtr L, int index, DG.Tweening.LoopType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningLoopType_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.LoopType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.LoopType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningLogBehaviour_TypeID = -1;
		int DGTweeningLogBehaviour_EnumRef = -1;
        
        public void PushDGTweeningLogBehaviour(RealStatePtr L, DG.Tweening.LogBehaviour val)
        {
            if (DGTweeningLogBehaviour_TypeID == -1)
            {
			    bool is_first;
                DGTweeningLogBehaviour_TypeID = getTypeId(L, typeof(DG.Tweening.LogBehaviour), out is_first);
				
				if (DGTweeningLogBehaviour_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.LogBehaviour));
				    DGTweeningLogBehaviour_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningLogBehaviour_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningLogBehaviour_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.LogBehaviour ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningLogBehaviour_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.LogBehaviour val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningLogBehaviour_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.LogBehaviour");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.LogBehaviour");
                }
				val = (DG.Tweening.LogBehaviour)e;
                
            }
            else
            {
                val = (DG.Tweening.LogBehaviour)objectCasters.GetCaster(typeof(DG.Tweening.LogBehaviour))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningLogBehaviour(RealStatePtr L, int index, DG.Tweening.LogBehaviour val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningLogBehaviour_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.LogBehaviour");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.LogBehaviour ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningTweenType_TypeID = -1;
		int DGTweeningTweenType_EnumRef = -1;
        
        public void PushDGTweeningTweenType(RealStatePtr L, DG.Tweening.TweenType val)
        {
            if (DGTweeningTweenType_TypeID == -1)
            {
			    bool is_first;
                DGTweeningTweenType_TypeID = getTypeId(L, typeof(DG.Tweening.TweenType), out is_first);
				
				if (DGTweeningTweenType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.TweenType));
				    DGTweeningTweenType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningTweenType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningTweenType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.TweenType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningTweenType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.TweenType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningTweenType_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.TweenType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.TweenType");
                }
				val = (DG.Tweening.TweenType)e;
                
            }
            else
            {
                val = (DG.Tweening.TweenType)objectCasters.GetCaster(typeof(DG.Tweening.TweenType))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningTweenType(RealStatePtr L, int index, DG.Tweening.TweenType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningTweenType_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.TweenType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.TweenType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningUpdateType_TypeID = -1;
		int DGTweeningUpdateType_EnumRef = -1;
        
        public void PushDGTweeningUpdateType(RealStatePtr L, DG.Tweening.UpdateType val)
        {
            if (DGTweeningUpdateType_TypeID == -1)
            {
			    bool is_first;
                DGTweeningUpdateType_TypeID = getTypeId(L, typeof(DG.Tweening.UpdateType), out is_first);
				
				if (DGTweeningUpdateType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.UpdateType));
				    DGTweeningUpdateType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningUpdateType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningUpdateType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.UpdateType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningUpdateType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.UpdateType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningUpdateType_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.UpdateType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.UpdateType");
                }
				val = (DG.Tweening.UpdateType)e;
                
            }
            else
            {
                val = (DG.Tweening.UpdateType)objectCasters.GetCaster(typeof(DG.Tweening.UpdateType))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningUpdateType(RealStatePtr L, int index, DG.Tweening.UpdateType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningUpdateType_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.UpdateType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.UpdateType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningHandlesDrawMode_TypeID = -1;
		int DGTweeningHandlesDrawMode_EnumRef = -1;
        
        public void PushDGTweeningHandlesDrawMode(RealStatePtr L, DG.Tweening.HandlesDrawMode val)
        {
            if (DGTweeningHandlesDrawMode_TypeID == -1)
            {
			    bool is_first;
                DGTweeningHandlesDrawMode_TypeID = getTypeId(L, typeof(DG.Tweening.HandlesDrawMode), out is_first);
				
				if (DGTweeningHandlesDrawMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.HandlesDrawMode));
				    DGTweeningHandlesDrawMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningHandlesDrawMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningHandlesDrawMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.HandlesDrawMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningHandlesDrawMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.HandlesDrawMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningHandlesDrawMode_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.HandlesDrawMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.HandlesDrawMode");
                }
				val = (DG.Tweening.HandlesDrawMode)e;
                
            }
            else
            {
                val = (DG.Tweening.HandlesDrawMode)objectCasters.GetCaster(typeof(DG.Tweening.HandlesDrawMode))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningHandlesDrawMode(RealStatePtr L, int index, DG.Tweening.HandlesDrawMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningHandlesDrawMode_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.HandlesDrawMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.HandlesDrawMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningHandlesType_TypeID = -1;
		int DGTweeningHandlesType_EnumRef = -1;
        
        public void PushDGTweeningHandlesType(RealStatePtr L, DG.Tweening.HandlesType val)
        {
            if (DGTweeningHandlesType_TypeID == -1)
            {
			    bool is_first;
                DGTweeningHandlesType_TypeID = getTypeId(L, typeof(DG.Tweening.HandlesType), out is_first);
				
				if (DGTweeningHandlesType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.HandlesType));
				    DGTweeningHandlesType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningHandlesType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningHandlesType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.HandlesType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningHandlesType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.HandlesType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningHandlesType_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.HandlesType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.HandlesType");
                }
				val = (DG.Tweening.HandlesType)e;
                
            }
            else
            {
                val = (DG.Tweening.HandlesType)objectCasters.GetCaster(typeof(DG.Tweening.HandlesType))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningHandlesType(RealStatePtr L, int index, DG.Tweening.HandlesType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningHandlesType_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.HandlesType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.HandlesType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningDOTweenInspectorMode_TypeID = -1;
		int DGTweeningDOTweenInspectorMode_EnumRef = -1;
        
        public void PushDGTweeningDOTweenInspectorMode(RealStatePtr L, DG.Tweening.DOTweenInspectorMode val)
        {
            if (DGTweeningDOTweenInspectorMode_TypeID == -1)
            {
			    bool is_first;
                DGTweeningDOTweenInspectorMode_TypeID = getTypeId(L, typeof(DG.Tweening.DOTweenInspectorMode), out is_first);
				
				if (DGTweeningDOTweenInspectorMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.DOTweenInspectorMode));
				    DGTweeningDOTweenInspectorMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningDOTweenInspectorMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningDOTweenInspectorMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.DOTweenInspectorMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningDOTweenInspectorMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.DOTweenInspectorMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningDOTweenInspectorMode_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.DOTweenInspectorMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.DOTweenInspectorMode");
                }
				val = (DG.Tweening.DOTweenInspectorMode)e;
                
            }
            else
            {
                val = (DG.Tweening.DOTweenInspectorMode)objectCasters.GetCaster(typeof(DG.Tweening.DOTweenInspectorMode))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningDOTweenInspectorMode(RealStatePtr L, int index, DG.Tweening.DOTweenInspectorMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningDOTweenInspectorMode_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.DOTweenInspectorMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.DOTweenInspectorMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int DGTweeningSpiralMode_TypeID = -1;
		int DGTweeningSpiralMode_EnumRef = -1;
        
        public void PushDGTweeningSpiralMode(RealStatePtr L, DG.Tweening.SpiralMode val)
        {
            if (DGTweeningSpiralMode_TypeID == -1)
            {
			    bool is_first;
                DGTweeningSpiralMode_TypeID = getTypeId(L, typeof(DG.Tweening.SpiralMode), out is_first);
				
				if (DGTweeningSpiralMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(DG.Tweening.SpiralMode));
				    DGTweeningSpiralMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, DGTweeningSpiralMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, DGTweeningSpiralMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for DG.Tweening.SpiralMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, DGTweeningSpiralMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out DG.Tweening.SpiralMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningSpiralMode_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.SpiralMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for DG.Tweening.SpiralMode");
                }
				val = (DG.Tweening.SpiralMode)e;
                
            }
            else
            {
                val = (DG.Tweening.SpiralMode)objectCasters.GetCaster(typeof(DG.Tweening.SpiralMode))(L, index, null);
            }
        }
		
        public void UpdateDGTweeningSpiralMode(RealStatePtr L, int index, DG.Tweening.SpiralMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != DGTweeningSpiralMode_TypeID)
				{
				    throw new Exception("invalid userdata for DG.Tweening.SpiralMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for DG.Tweening.SpiralMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int GameLibEGameMode_TypeID = -1;
		int GameLibEGameMode_EnumRef = -1;
        
        public void PushGameLibEGameMode(RealStatePtr L, GameLib.EGameMode val)
        {
            if (GameLibEGameMode_TypeID == -1)
            {
			    bool is_first;
                GameLibEGameMode_TypeID = getTypeId(L, typeof(GameLib.EGameMode), out is_first);
				
				if (GameLibEGameMode_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(GameLib.EGameMode));
				    GameLibEGameMode_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, GameLibEGameMode_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, GameLibEGameMode_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for GameLib.EGameMode ,value="+val);
            }
			
			LuaAPI.lua_getref(L, GameLibEGameMode_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out GameLib.EGameMode val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != GameLibEGameMode_TypeID)
				{
				    throw new Exception("invalid userdata for GameLib.EGameMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for GameLib.EGameMode");
                }
				val = (GameLib.EGameMode)e;
                
            }
            else
            {
                val = (GameLib.EGameMode)objectCasters.GetCaster(typeof(GameLib.EGameMode))(L, index, null);
            }
        }
		
        public void UpdateGameLibEGameMode(RealStatePtr L, int index, GameLib.EGameMode val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != GameLibEGameMode_TypeID)
				{
				    throw new Exception("invalid userdata for GameLib.EGameMode");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for GameLib.EGameMode ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int GameLibGridMapManagerGridType_TypeID = -1;
		int GameLibGridMapManagerGridType_EnumRef = -1;
        
        public void PushGameLibGridMapManagerGridType(RealStatePtr L, GameLib.GridMapManager.GridType val)
        {
            if (GameLibGridMapManagerGridType_TypeID == -1)
            {
			    bool is_first;
                GameLibGridMapManagerGridType_TypeID = getTypeId(L, typeof(GameLib.GridMapManager.GridType), out is_first);
				
				if (GameLibGridMapManagerGridType_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(GameLib.GridMapManager.GridType));
				    GameLibGridMapManagerGridType_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, GameLibGridMapManagerGridType_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, GameLibGridMapManagerGridType_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for GameLib.GridMapManager.GridType ,value="+val);
            }
			
			LuaAPI.lua_getref(L, GameLibGridMapManagerGridType_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out GameLib.GridMapManager.GridType val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != GameLibGridMapManagerGridType_TypeID)
				{
				    throw new Exception("invalid userdata for GameLib.GridMapManager.GridType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for GameLib.GridMapManager.GridType");
                }
				val = (GameLib.GridMapManager.GridType)e;
                
            }
            else
            {
                val = (GameLib.GridMapManager.GridType)objectCasters.GetCaster(typeof(GameLib.GridMapManager.GridType))(L, index, null);
            }
        }
		
        public void UpdateGameLibGridMapManagerGridType(RealStatePtr L, int index, GameLib.GridMapManager.GridType val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != GameLibGridMapManagerGridType_TypeID)
				{
				    throw new Exception("invalid userdata for GameLib.GridMapManager.GridType");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for GameLib.GridMapManager.GridType ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int GameLibGridMapManagerLayerLevel_TypeID = -1;
		int GameLibGridMapManagerLayerLevel_EnumRef = -1;
        
        public void PushGameLibGridMapManagerLayerLevel(RealStatePtr L, GameLib.GridMapManager.LayerLevel val)
        {
            if (GameLibGridMapManagerLayerLevel_TypeID == -1)
            {
			    bool is_first;
                GameLibGridMapManagerLayerLevel_TypeID = getTypeId(L, typeof(GameLib.GridMapManager.LayerLevel), out is_first);
				
				if (GameLibGridMapManagerLayerLevel_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(GameLib.GridMapManager.LayerLevel));
				    GameLibGridMapManagerLayerLevel_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, GameLibGridMapManagerLayerLevel_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, GameLibGridMapManagerLayerLevel_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for GameLib.GridMapManager.LayerLevel ,value="+val);
            }
			
			LuaAPI.lua_getref(L, GameLibGridMapManagerLayerLevel_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out GameLib.GridMapManager.LayerLevel val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != GameLibGridMapManagerLayerLevel_TypeID)
				{
				    throw new Exception("invalid userdata for GameLib.GridMapManager.LayerLevel");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for GameLib.GridMapManager.LayerLevel");
                }
				val = (GameLib.GridMapManager.LayerLevel)e;
                
            }
            else
            {
                val = (GameLib.GridMapManager.LayerLevel)objectCasters.GetCaster(typeof(GameLib.GridMapManager.LayerLevel))(L, index, null);
            }
        }
		
        public void UpdateGameLibGridMapManagerLayerLevel(RealStatePtr L, int index, GameLib.GridMapManager.LayerLevel val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != GameLibGridMapManagerLayerLevel_TypeID)
				{
				    throw new Exception("invalid userdata for GameLib.GridMapManager.LayerLevel");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for GameLib.GridMapManager.LayerLevel ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        
		// table cast optimze
		
        
    }
	
	public partial class StaticLuaCallbacks
    {
	    internal static bool __tryArrayGet(Type type, RealStatePtr L, ObjectTranslator translator, object obj, int index)
		{
		
			if (type == typeof(UnityEngine.Vector2[]))
			{
			    UnityEngine.Vector2[] array = obj as UnityEngine.Vector2[];
				translator.PushUnityEngineVector2(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Vector3[]))
			{
			    UnityEngine.Vector3[] array = obj as UnityEngine.Vector3[];
				translator.PushUnityEngineVector3(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Vector4[]))
			{
			    UnityEngine.Vector4[] array = obj as UnityEngine.Vector4[];
				translator.PushUnityEngineVector4(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Color[]))
			{
			    UnityEngine.Color[] array = obj as UnityEngine.Color[];
				translator.PushUnityEngineColor(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Quaternion[]))
			{
			    UnityEngine.Quaternion[] array = obj as UnityEngine.Quaternion[];
				translator.PushUnityEngineQuaternion(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Ray[]))
			{
			    UnityEngine.Ray[] array = obj as UnityEngine.Ray[];
				translator.PushUnityEngineRay(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Bounds[]))
			{
			    UnityEngine.Bounds[] array = obj as UnityEngine.Bounds[];
				translator.PushUnityEngineBounds(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Ray2D[]))
			{
			    UnityEngine.Ray2D[] array = obj as UnityEngine.Ray2D[];
				translator.PushUnityEngineRay2D(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.AnimatorUpdateMode[]))
			{
			    UnityEngine.AnimatorUpdateMode[] array = obj as UnityEngine.AnimatorUpdateMode[];
				translator.PushUnityEngineAnimatorUpdateMode(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Tilemaps.Tile.ColliderType[]))
			{
			    UnityEngine.Tilemaps.Tile.ColliderType[] array = obj as UnityEngine.Tilemaps.Tile.ColliderType[];
				translator.PushUnityEngineTilemapsTileColliderType(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Tilemaps.Tilemap.Orientation[]))
			{
			    UnityEngine.Tilemaps.Tilemap.Orientation[] array = obj as UnityEngine.Tilemaps.Tilemap.Orientation[];
				translator.PushUnityEngineTilemapsTilemapOrientation(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Tilemaps.TileFlags[]))
			{
			    UnityEngine.Tilemaps.TileFlags[] array = obj as UnityEngine.Tilemaps.TileFlags[];
				translator.PushUnityEngineTilemapsTileFlags(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Tilemaps.TileAnimationFlags[]))
			{
			    UnityEngine.Tilemaps.TileAnimationFlags[] array = obj as UnityEngine.Tilemaps.TileAnimationFlags[];
				translator.PushUnityEngineTilemapsTileAnimationFlags(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Tilemaps.TilemapRenderer.SortOrder[]))
			{
			    UnityEngine.Tilemaps.TilemapRenderer.SortOrder[] array = obj as UnityEngine.Tilemaps.TilemapRenderer.SortOrder[];
				translator.PushUnityEngineTilemapsTilemapRendererSortOrder(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Tilemaps.TilemapRenderer.Mode[]))
			{
			    UnityEngine.Tilemaps.TilemapRenderer.Mode[] array = obj as UnityEngine.Tilemaps.TilemapRenderer.Mode[];
				translator.PushUnityEngineTilemapsTilemapRendererMode(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds[]))
			{
			    UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds[] array = obj as UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds[];
				translator.PushUnityEngineTilemapsTilemapRendererDetectChunkCullingBounds(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.OriginType[]))
			{
			    Honor.Runtime.OriginType[] array = obj as Honor.Runtime.OriginType[];
				translator.PushHonorRuntimeOriginType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.PrefabDetailType[]))
			{
			    Honor.Runtime.PrefabDetailType[] array = obj as Honor.Runtime.PrefabDetailType[];
				translator.PushHonorRuntimePrefabDetailType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.PrefabType[]))
			{
			    Honor.Runtime.PrefabType[] array = obj as Honor.Runtime.PrefabType[];
				translator.PushHonorRuntimePrefabType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameEventCmd[]))
			{
			    Honor.Runtime.GameEventCmd[] array = obj as Honor.Runtime.GameEventCmd[];
				translator.PushHonorRuntimeGameEventCmd(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.SnapStatus[]))
			{
			    Honor.Runtime.SnapStatus[] array = obj as Honor.Runtime.SnapStatus[];
				translator.PushHonorRuntimeSnapStatus(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.ItemCornerEnum[]))
			{
			    Honor.Runtime.ItemCornerEnum[] array = obj as Honor.Runtime.ItemCornerEnum[];
				translator.PushHonorRuntimeItemCornerEnum(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.ListItemArrangeType[]))
			{
			    Honor.Runtime.ListItemArrangeType[] array = obj as Honor.Runtime.ListItemArrangeType[];
				translator.PushHonorRuntimeListItemArrangeType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GridItemArrangeType[]))
			{
			    Honor.Runtime.GridItemArrangeType[] array = obj as Honor.Runtime.GridItemArrangeType[];
				translator.PushHonorRuntimeGridItemArrangeType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GridFixedType[]))
			{
			    Honor.Runtime.GridFixedType[] array = obj as Honor.Runtime.GridFixedType[];
				translator.PushHonorRuntimeGridFixedType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.PatternType[]))
			{
			    Honor.Runtime.PatternType[] array = obj as Honor.Runtime.PatternType[];
				translator.PushHonorRuntimePatternType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.NonePatternType[]))
			{
			    Honor.Runtime.NonePatternType[] array = obj as Honor.Runtime.NonePatternType[];
				translator.PushHonorRuntimeNonePatternType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.MVVMPatternType[]))
			{
			    Honor.Runtime.MVVMPatternType[] array = obj as Honor.Runtime.MVVMPatternType[];
				translator.PushHonorRuntimeMVVMPatternType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.PersistWayType[]))
			{
			    Honor.Runtime.PersistWayType[] array = obj as Honor.Runtime.PersistWayType[];
				translator.PushHonorRuntimePersistWayType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.PlaySoundErrorCode[]))
			{
			    Honor.Runtime.PlaySoundErrorCode[] array = obj as Honor.Runtime.PlaySoundErrorCode[];
				translator.PushHonorRuntimePlaySoundErrorCode(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.DevicePerformanceLevel[]))
			{
			    Honor.Runtime.DevicePerformanceLevel[] array = obj as Honor.Runtime.DevicePerformanceLevel[];
				translator.PushHonorRuntimeDevicePerformanceLevel(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.QualityLevel[]))
			{
			    Honor.Runtime.QualityLevel[] array = obj as Honor.Runtime.QualityLevel[];
				translator.PushHonorRuntimeQualityLevel(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.UIType[]))
			{
			    Honor.Runtime.UIType[] array = obj as Honor.Runtime.UIType[];
				translator.PushHonorRuntimeUIType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.VibrateType[]))
			{
			    Honor.Runtime.VibrateType[] array = obj as Honor.Runtime.VibrateType[];
				translator.PushHonorRuntimeVibrateType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameDefinitions.DimensionMode[]))
			{
			    Honor.Runtime.GameDefinitions.DimensionMode[] array = obj as Honor.Runtime.GameDefinitions.DimensionMode[];
				translator.PushHonorRuntimeGameDefinitionsDimensionMode(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameDefinitions.AssetType[]))
			{
			    Honor.Runtime.GameDefinitions.AssetType[] array = obj as Honor.Runtime.GameDefinitions.AssetType[];
				translator.PushHonorRuntimeGameDefinitionsAssetType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameDefinitions.PathType[]))
			{
			    Honor.Runtime.GameDefinitions.PathType[] array = obj as Honor.Runtime.GameDefinitions.PathType[];
				translator.PushHonorRuntimeGameDefinitionsPathType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameDefinitions.DownloadStep[]))
			{
			    Honor.Runtime.GameDefinitions.DownloadStep[] array = obj as Honor.Runtime.GameDefinitions.DownloadStep[];
				translator.PushHonorRuntimeGameDefinitionsDownloadStep(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameDefinitions.DebugMode[]))
			{
			    Honor.Runtime.GameDefinitions.DebugMode[] array = obj as Honor.Runtime.GameDefinitions.DebugMode[];
				translator.PushHonorRuntimeGameDefinitionsDebugMode(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameDefinitions.Language[]))
			{
			    Honor.Runtime.GameDefinitions.Language[] array = obj as Honor.Runtime.GameDefinitions.Language[];
				translator.PushHonorRuntimeGameDefinitionsLanguage(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameDefinitions.DebugWindowModel[]))
			{
			    Honor.Runtime.GameDefinitions.DebugWindowModel[] array = obj as Honor.Runtime.GameDefinitions.DebugWindowModel[];
				translator.PushHonorRuntimeGameDefinitionsDebugWindowModel(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType[]))
			{
			    Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType[] array = obj as Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType[];
				translator.PushHonorRuntimeAorTextEffectOutlineHorizontalAligmentType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType[]))
			{
			    Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType[] array = obj as Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType[];
				translator.PushHonorRuntimeAorTextEffectSpacingHorizontalAligmentType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.LuaBindValue.BindValueType[]))
			{
			    Honor.Runtime.LuaBindValue.BindValueType[] array = obj as Honor.Runtime.LuaBindValue.BindValueType[];
				translator.PushHonorRuntimeLuaBindValueBindValueType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.LuaInjection.InjectionType[]))
			{
			    Honor.Runtime.LuaInjection.InjectionType[] array = obj as Honor.Runtime.LuaInjection.InjectionType[];
				translator.PushHonorRuntimeLuaInjectionInjectionType(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.UIFader.InitState[]))
			{
			    Honor.Runtime.UIFader.InitState[] array = obj as Honor.Runtime.UIFader.InitState[];
				translator.PushHonorRuntimeUIFaderInitState(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.Log.LogLevel[]))
			{
			    Honor.Runtime.Log.LogLevel[] array = obj as Honor.Runtime.Log.LogLevel[];
				translator.PushHonorRuntimeLogLogLevel(L, array[index]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.UILauncherLoadingView.LoadingMode[]))
			{
			    Honor.Runtime.UILauncherLoadingView.LoadingMode[] array = obj as Honor.Runtime.UILauncherLoadingView.LoadingMode[];
				translator.PushHonorRuntimeUILauncherLoadingViewLoadingMode(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.MatchTargetFields[]))
			{
			    UnityEngine.Timeline.MatchTargetFields[] array = obj as UnityEngine.Timeline.MatchTargetFields[];
				translator.PushUnityEngineTimelineMatchTargetFields(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.TrackOffset[]))
			{
			    UnityEngine.Timeline.TrackOffset[] array = obj as UnityEngine.Timeline.TrackOffset[];
				translator.PushUnityEngineTimelineTrackOffset(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.ClipCaps[]))
			{
			    UnityEngine.Timeline.ClipCaps[] array = obj as UnityEngine.Timeline.ClipCaps[];
				translator.PushUnityEngineTimelineClipCaps(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.NotificationFlags[]))
			{
			    UnityEngine.Timeline.NotificationFlags[] array = obj as UnityEngine.Timeline.NotificationFlags[];
				translator.PushUnityEngineTimelineNotificationFlags(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.TrackBindingFlags[]))
			{
			    UnityEngine.Timeline.TrackBindingFlags[] array = obj as UnityEngine.Timeline.TrackBindingFlags[];
				translator.PushUnityEngineTimelineTrackBindingFlags(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.StandardFrameRates[]))
			{
			    UnityEngine.Timeline.StandardFrameRates[] array = obj as UnityEngine.Timeline.StandardFrameRates[];
				translator.PushUnityEngineTimelineStandardFrameRates(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.ActivationTrack.PostPlaybackState[]))
			{
			    UnityEngine.Timeline.ActivationTrack.PostPlaybackState[] array = obj as UnityEngine.Timeline.ActivationTrack.PostPlaybackState[];
				translator.PushUnityEngineTimelineActivationTrackPostPlaybackState(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.AnimationPlayableAsset.LoopMode[]))
			{
			    UnityEngine.Timeline.AnimationPlayableAsset.LoopMode[] array = obj as UnityEngine.Timeline.AnimationPlayableAsset.LoopMode[];
				translator.PushUnityEngineTimelineAnimationPlayableAssetLoopMode(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.TimelineClip.ClipExtrapolation[]))
			{
			    UnityEngine.Timeline.TimelineClip.ClipExtrapolation[] array = obj as UnityEngine.Timeline.TimelineClip.ClipExtrapolation[];
				translator.PushUnityEngineTimelineTimelineClipClipExtrapolation(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.TimelineClip.BlendCurveMode[]))
			{
			    UnityEngine.Timeline.TimelineClip.BlendCurveMode[] array = obj as UnityEngine.Timeline.TimelineClip.BlendCurveMode[];
				translator.PushUnityEngineTimelineTimelineClipBlendCurveMode(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.TimelineAsset.DurationMode[]))
			{
			    UnityEngine.Timeline.TimelineAsset.DurationMode[] array = obj as UnityEngine.Timeline.TimelineAsset.DurationMode[];
				translator.PushUnityEngineTimelineTimelineAssetDurationMode(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState[]))
			{
			    UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState[] array = obj as UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState[];
				translator.PushUnityEngineTimelineActivationControlPlayablePostPlaybackState(L, array[index]);
				return true;
			}
			else if (type == typeof(Spine.Unity.UpdateMode[]))
			{
			    Spine.Unity.UpdateMode[] array = obj as Spine.Unity.UpdateMode[];
				translator.PushSpineUnityUpdateMode(L, array[index]);
				return true;
			}
			else if (type == typeof(Spine.Unity.UpdateTiming[]))
			{
			    Spine.Unity.UpdateTiming[] array = obj as Spine.Unity.UpdateTiming[];
				translator.PushSpineUnityUpdateTiming(L, array[index]);
				return true;
			}
			else if (type == typeof(Spine.Unity.SettingsTriState[]))
			{
			    Spine.Unity.SettingsTriState[] array = obj as Spine.Unity.SettingsTriState[];
				translator.PushSpineUnitySettingsTriState(L, array[index]);
				return true;
			}
			else if (type == typeof(Spine.Unity.BoneFollower.AxisOrientation[]))
			{
			    Spine.Unity.BoneFollower.AxisOrientation[] array = obj as Spine.Unity.BoneFollower.AxisOrientation[];
				translator.PushSpineUnityBoneFollowerAxisOrientation(L, array[index]);
				return true;
			}
			else if (type == typeof(Spine.Unity.SkeletonGraphic.LayoutMode[]))
			{
			    Spine.Unity.SkeletonGraphic.LayoutMode[] array = obj as Spine.Unity.SkeletonGraphic.LayoutMode[];
				translator.PushSpineUnitySkeletonGraphicLayoutMode(L, array[index]);
				return true;
			}
			else if (type == typeof(Spine.Unity.SkeletonUtilityBone.Mode[]))
			{
			    Spine.Unity.SkeletonUtilityBone.Mode[] array = obj as Spine.Unity.SkeletonUtilityBone.Mode[];
				translator.PushSpineUnitySkeletonUtilityBoneMode(L, array[index]);
				return true;
			}
			else if (type == typeof(Spine.Unity.SkeletonUtilityBone.UpdatePhase[]))
			{
			    Spine.Unity.SkeletonUtilityBone.UpdatePhase[] array = obj as Spine.Unity.SkeletonUtilityBone.UpdatePhase[];
				translator.PushSpineUnitySkeletonUtilityBoneUpdatePhase(L, array[index]);
				return true;
			}
			else if (type == typeof(Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode[]))
			{
			    Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode[] array = obj as Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode[];
				translator.PushSpineUnitySkeletonMecanimMecanimTranslatorMixMode(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.AutoPlay[]))
			{
			    DG.Tweening.AutoPlay[] array = obj as DG.Tweening.AutoPlay[];
				translator.PushDGTweeningAutoPlay(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.AxisConstraint[]))
			{
			    DG.Tweening.AxisConstraint[] array = obj as DG.Tweening.AxisConstraint[];
				translator.PushDGTweeningAxisConstraint(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.Ease[]))
			{
			    DG.Tweening.Ease[] array = obj as DG.Tweening.Ease[];
				translator.PushDGTweeningEase(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.PathMode[]))
			{
			    DG.Tweening.PathMode[] array = obj as DG.Tweening.PathMode[];
				translator.PushDGTweeningPathMode(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.PathType[]))
			{
			    DG.Tweening.PathType[] array = obj as DG.Tweening.PathType[];
				translator.PushDGTweeningPathType(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.RotateMode[]))
			{
			    DG.Tweening.RotateMode[] array = obj as DG.Tweening.RotateMode[];
				translator.PushDGTweeningRotateMode(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.ScrambleMode[]))
			{
			    DG.Tweening.ScrambleMode[] array = obj as DG.Tweening.ScrambleMode[];
				translator.PushDGTweeningScrambleMode(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.LoopType[]))
			{
			    DG.Tweening.LoopType[] array = obj as DG.Tweening.LoopType[];
				translator.PushDGTweeningLoopType(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.LogBehaviour[]))
			{
			    DG.Tweening.LogBehaviour[] array = obj as DG.Tweening.LogBehaviour[];
				translator.PushDGTweeningLogBehaviour(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.TweenType[]))
			{
			    DG.Tweening.TweenType[] array = obj as DG.Tweening.TweenType[];
				translator.PushDGTweeningTweenType(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.UpdateType[]))
			{
			    DG.Tweening.UpdateType[] array = obj as DG.Tweening.UpdateType[];
				translator.PushDGTweeningUpdateType(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.HandlesDrawMode[]))
			{
			    DG.Tweening.HandlesDrawMode[] array = obj as DG.Tweening.HandlesDrawMode[];
				translator.PushDGTweeningHandlesDrawMode(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.HandlesType[]))
			{
			    DG.Tweening.HandlesType[] array = obj as DG.Tweening.HandlesType[];
				translator.PushDGTweeningHandlesType(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.DOTweenInspectorMode[]))
			{
			    DG.Tweening.DOTweenInspectorMode[] array = obj as DG.Tweening.DOTweenInspectorMode[];
				translator.PushDGTweeningDOTweenInspectorMode(L, array[index]);
				return true;
			}
			else if (type == typeof(DG.Tweening.SpiralMode[]))
			{
			    DG.Tweening.SpiralMode[] array = obj as DG.Tweening.SpiralMode[];
				translator.PushDGTweeningSpiralMode(L, array[index]);
				return true;
			}
			else if (type == typeof(GameLib.EGameMode[]))
			{
			    GameLib.EGameMode[] array = obj as GameLib.EGameMode[];
				translator.PushGameLibEGameMode(L, array[index]);
				return true;
			}
			else if (type == typeof(GameLib.GridMapManager.GridType[]))
			{
			    GameLib.GridMapManager.GridType[] array = obj as GameLib.GridMapManager.GridType[];
				translator.PushGameLibGridMapManagerGridType(L, array[index]);
				return true;
			}
			else if (type == typeof(GameLib.GridMapManager.LayerLevel[]))
			{
			    GameLib.GridMapManager.LayerLevel[] array = obj as GameLib.GridMapManager.LayerLevel[];
				translator.PushGameLibGridMapManagerLayerLevel(L, array[index]);
				return true;
			}
            return false;
		}
		
		internal static bool __tryArraySet(Type type, RealStatePtr L, ObjectTranslator translator, object obj, int array_idx, int obj_idx)
		{
		
			if (type == typeof(UnityEngine.Vector2[]))
			{
			    UnityEngine.Vector2[] array = obj as UnityEngine.Vector2[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Vector3[]))
			{
			    UnityEngine.Vector3[] array = obj as UnityEngine.Vector3[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Vector4[]))
			{
			    UnityEngine.Vector4[] array = obj as UnityEngine.Vector4[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Color[]))
			{
			    UnityEngine.Color[] array = obj as UnityEngine.Color[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Quaternion[]))
			{
			    UnityEngine.Quaternion[] array = obj as UnityEngine.Quaternion[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Ray[]))
			{
			    UnityEngine.Ray[] array = obj as UnityEngine.Ray[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Bounds[]))
			{
			    UnityEngine.Bounds[] array = obj as UnityEngine.Bounds[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Ray2D[]))
			{
			    UnityEngine.Ray2D[] array = obj as UnityEngine.Ray2D[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.AnimatorUpdateMode[]))
			{
			    UnityEngine.AnimatorUpdateMode[] array = obj as UnityEngine.AnimatorUpdateMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Tilemaps.Tile.ColliderType[]))
			{
			    UnityEngine.Tilemaps.Tile.ColliderType[] array = obj as UnityEngine.Tilemaps.Tile.ColliderType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Tilemaps.Tilemap.Orientation[]))
			{
			    UnityEngine.Tilemaps.Tilemap.Orientation[] array = obj as UnityEngine.Tilemaps.Tilemap.Orientation[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Tilemaps.TileFlags[]))
			{
			    UnityEngine.Tilemaps.TileFlags[] array = obj as UnityEngine.Tilemaps.TileFlags[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Tilemaps.TileAnimationFlags[]))
			{
			    UnityEngine.Tilemaps.TileAnimationFlags[] array = obj as UnityEngine.Tilemaps.TileAnimationFlags[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Tilemaps.TilemapRenderer.SortOrder[]))
			{
			    UnityEngine.Tilemaps.TilemapRenderer.SortOrder[] array = obj as UnityEngine.Tilemaps.TilemapRenderer.SortOrder[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Tilemaps.TilemapRenderer.Mode[]))
			{
			    UnityEngine.Tilemaps.TilemapRenderer.Mode[] array = obj as UnityEngine.Tilemaps.TilemapRenderer.Mode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds[]))
			{
			    UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds[] array = obj as UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.OriginType[]))
			{
			    Honor.Runtime.OriginType[] array = obj as Honor.Runtime.OriginType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.PrefabDetailType[]))
			{
			    Honor.Runtime.PrefabDetailType[] array = obj as Honor.Runtime.PrefabDetailType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.PrefabType[]))
			{
			    Honor.Runtime.PrefabType[] array = obj as Honor.Runtime.PrefabType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameEventCmd[]))
			{
			    Honor.Runtime.GameEventCmd[] array = obj as Honor.Runtime.GameEventCmd[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.SnapStatus[]))
			{
			    Honor.Runtime.SnapStatus[] array = obj as Honor.Runtime.SnapStatus[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.ItemCornerEnum[]))
			{
			    Honor.Runtime.ItemCornerEnum[] array = obj as Honor.Runtime.ItemCornerEnum[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.ListItemArrangeType[]))
			{
			    Honor.Runtime.ListItemArrangeType[] array = obj as Honor.Runtime.ListItemArrangeType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GridItemArrangeType[]))
			{
			    Honor.Runtime.GridItemArrangeType[] array = obj as Honor.Runtime.GridItemArrangeType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GridFixedType[]))
			{
			    Honor.Runtime.GridFixedType[] array = obj as Honor.Runtime.GridFixedType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.PatternType[]))
			{
			    Honor.Runtime.PatternType[] array = obj as Honor.Runtime.PatternType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.NonePatternType[]))
			{
			    Honor.Runtime.NonePatternType[] array = obj as Honor.Runtime.NonePatternType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.MVVMPatternType[]))
			{
			    Honor.Runtime.MVVMPatternType[] array = obj as Honor.Runtime.MVVMPatternType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.PersistWayType[]))
			{
			    Honor.Runtime.PersistWayType[] array = obj as Honor.Runtime.PersistWayType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.PlaySoundErrorCode[]))
			{
			    Honor.Runtime.PlaySoundErrorCode[] array = obj as Honor.Runtime.PlaySoundErrorCode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.DevicePerformanceLevel[]))
			{
			    Honor.Runtime.DevicePerformanceLevel[] array = obj as Honor.Runtime.DevicePerformanceLevel[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.QualityLevel[]))
			{
			    Honor.Runtime.QualityLevel[] array = obj as Honor.Runtime.QualityLevel[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.UIType[]))
			{
			    Honor.Runtime.UIType[] array = obj as Honor.Runtime.UIType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.VibrateType[]))
			{
			    Honor.Runtime.VibrateType[] array = obj as Honor.Runtime.VibrateType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameDefinitions.DimensionMode[]))
			{
			    Honor.Runtime.GameDefinitions.DimensionMode[] array = obj as Honor.Runtime.GameDefinitions.DimensionMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameDefinitions.AssetType[]))
			{
			    Honor.Runtime.GameDefinitions.AssetType[] array = obj as Honor.Runtime.GameDefinitions.AssetType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameDefinitions.PathType[]))
			{
			    Honor.Runtime.GameDefinitions.PathType[] array = obj as Honor.Runtime.GameDefinitions.PathType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameDefinitions.DownloadStep[]))
			{
			    Honor.Runtime.GameDefinitions.DownloadStep[] array = obj as Honor.Runtime.GameDefinitions.DownloadStep[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameDefinitions.DebugMode[]))
			{
			    Honor.Runtime.GameDefinitions.DebugMode[] array = obj as Honor.Runtime.GameDefinitions.DebugMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameDefinitions.Language[]))
			{
			    Honor.Runtime.GameDefinitions.Language[] array = obj as Honor.Runtime.GameDefinitions.Language[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.GameDefinitions.DebugWindowModel[]))
			{
			    Honor.Runtime.GameDefinitions.DebugWindowModel[] array = obj as Honor.Runtime.GameDefinitions.DebugWindowModel[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType[]))
			{
			    Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType[] array = obj as Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType[]))
			{
			    Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType[] array = obj as Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.LuaBindValue.BindValueType[]))
			{
			    Honor.Runtime.LuaBindValue.BindValueType[] array = obj as Honor.Runtime.LuaBindValue.BindValueType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.LuaInjection.InjectionType[]))
			{
			    Honor.Runtime.LuaInjection.InjectionType[] array = obj as Honor.Runtime.LuaInjection.InjectionType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.UIFader.InitState[]))
			{
			    Honor.Runtime.UIFader.InitState[] array = obj as Honor.Runtime.UIFader.InitState[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.Log.LogLevel[]))
			{
			    Honor.Runtime.Log.LogLevel[] array = obj as Honor.Runtime.Log.LogLevel[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Honor.Runtime.UILauncherLoadingView.LoadingMode[]))
			{
			    Honor.Runtime.UILauncherLoadingView.LoadingMode[] array = obj as Honor.Runtime.UILauncherLoadingView.LoadingMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.MatchTargetFields[]))
			{
			    UnityEngine.Timeline.MatchTargetFields[] array = obj as UnityEngine.Timeline.MatchTargetFields[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.TrackOffset[]))
			{
			    UnityEngine.Timeline.TrackOffset[] array = obj as UnityEngine.Timeline.TrackOffset[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.ClipCaps[]))
			{
			    UnityEngine.Timeline.ClipCaps[] array = obj as UnityEngine.Timeline.ClipCaps[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.NotificationFlags[]))
			{
			    UnityEngine.Timeline.NotificationFlags[] array = obj as UnityEngine.Timeline.NotificationFlags[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.TrackBindingFlags[]))
			{
			    UnityEngine.Timeline.TrackBindingFlags[] array = obj as UnityEngine.Timeline.TrackBindingFlags[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.StandardFrameRates[]))
			{
			    UnityEngine.Timeline.StandardFrameRates[] array = obj as UnityEngine.Timeline.StandardFrameRates[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.ActivationTrack.PostPlaybackState[]))
			{
			    UnityEngine.Timeline.ActivationTrack.PostPlaybackState[] array = obj as UnityEngine.Timeline.ActivationTrack.PostPlaybackState[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.AnimationPlayableAsset.LoopMode[]))
			{
			    UnityEngine.Timeline.AnimationPlayableAsset.LoopMode[] array = obj as UnityEngine.Timeline.AnimationPlayableAsset.LoopMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.TimelineClip.ClipExtrapolation[]))
			{
			    UnityEngine.Timeline.TimelineClip.ClipExtrapolation[] array = obj as UnityEngine.Timeline.TimelineClip.ClipExtrapolation[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.TimelineClip.BlendCurveMode[]))
			{
			    UnityEngine.Timeline.TimelineClip.BlendCurveMode[] array = obj as UnityEngine.Timeline.TimelineClip.BlendCurveMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.TimelineAsset.DurationMode[]))
			{
			    UnityEngine.Timeline.TimelineAsset.DurationMode[] array = obj as UnityEngine.Timeline.TimelineAsset.DurationMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState[]))
			{
			    UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState[] array = obj as UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Spine.Unity.UpdateMode[]))
			{
			    Spine.Unity.UpdateMode[] array = obj as Spine.Unity.UpdateMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Spine.Unity.UpdateTiming[]))
			{
			    Spine.Unity.UpdateTiming[] array = obj as Spine.Unity.UpdateTiming[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Spine.Unity.SettingsTriState[]))
			{
			    Spine.Unity.SettingsTriState[] array = obj as Spine.Unity.SettingsTriState[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Spine.Unity.BoneFollower.AxisOrientation[]))
			{
			    Spine.Unity.BoneFollower.AxisOrientation[] array = obj as Spine.Unity.BoneFollower.AxisOrientation[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Spine.Unity.SkeletonGraphic.LayoutMode[]))
			{
			    Spine.Unity.SkeletonGraphic.LayoutMode[] array = obj as Spine.Unity.SkeletonGraphic.LayoutMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Spine.Unity.SkeletonUtilityBone.Mode[]))
			{
			    Spine.Unity.SkeletonUtilityBone.Mode[] array = obj as Spine.Unity.SkeletonUtilityBone.Mode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Spine.Unity.SkeletonUtilityBone.UpdatePhase[]))
			{
			    Spine.Unity.SkeletonUtilityBone.UpdatePhase[] array = obj as Spine.Unity.SkeletonUtilityBone.UpdatePhase[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode[]))
			{
			    Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode[] array = obj as Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.AutoPlay[]))
			{
			    DG.Tweening.AutoPlay[] array = obj as DG.Tweening.AutoPlay[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.AxisConstraint[]))
			{
			    DG.Tweening.AxisConstraint[] array = obj as DG.Tweening.AxisConstraint[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.Ease[]))
			{
			    DG.Tweening.Ease[] array = obj as DG.Tweening.Ease[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.PathMode[]))
			{
			    DG.Tweening.PathMode[] array = obj as DG.Tweening.PathMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.PathType[]))
			{
			    DG.Tweening.PathType[] array = obj as DG.Tweening.PathType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.RotateMode[]))
			{
			    DG.Tweening.RotateMode[] array = obj as DG.Tweening.RotateMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.ScrambleMode[]))
			{
			    DG.Tweening.ScrambleMode[] array = obj as DG.Tweening.ScrambleMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.LoopType[]))
			{
			    DG.Tweening.LoopType[] array = obj as DG.Tweening.LoopType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.LogBehaviour[]))
			{
			    DG.Tweening.LogBehaviour[] array = obj as DG.Tweening.LogBehaviour[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.TweenType[]))
			{
			    DG.Tweening.TweenType[] array = obj as DG.Tweening.TweenType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.UpdateType[]))
			{
			    DG.Tweening.UpdateType[] array = obj as DG.Tweening.UpdateType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.HandlesDrawMode[]))
			{
			    DG.Tweening.HandlesDrawMode[] array = obj as DG.Tweening.HandlesDrawMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.HandlesType[]))
			{
			    DG.Tweening.HandlesType[] array = obj as DG.Tweening.HandlesType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.DOTweenInspectorMode[]))
			{
			    DG.Tweening.DOTweenInspectorMode[] array = obj as DG.Tweening.DOTweenInspectorMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(DG.Tweening.SpiralMode[]))
			{
			    DG.Tweening.SpiralMode[] array = obj as DG.Tweening.SpiralMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(GameLib.EGameMode[]))
			{
			    GameLib.EGameMode[] array = obj as GameLib.EGameMode[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(GameLib.GridMapManager.GridType[]))
			{
			    GameLib.GridMapManager.GridType[] array = obj as GameLib.GridMapManager.GridType[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(GameLib.GridMapManager.LayerLevel[]))
			{
			    GameLib.GridMapManager.LayerLevel[] array = obj as GameLib.GridMapManager.LayerLevel[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
            return false;
		}
	}
}