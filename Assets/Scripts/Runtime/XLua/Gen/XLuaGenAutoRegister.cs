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
using System.Collections.Generic;
using System.Reflection;


namespace XLua.CSObjectWrap
{
    public class XLua_Gen_Initer_Register__
	{
        
        
        static void wrapInit0(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(GameLib.HiddenObjectCS), GameLibHiddenObjectCSWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(object), SystemObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Object), UnityEngineObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.DateTime), SystemDateTimeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Vector2), UnityEngineVector2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Vector3), UnityEngineVector3Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Vector4), UnityEngineVector4Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Quaternion), UnityEngineQuaternionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Color), UnityEngineColorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Ray), UnityEngineRayWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Bounds), UnityEngineBoundsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.GameObject), UnityEngineGameObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Component), UnityEngineComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Transform), UnityEngineTransformWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Resources), UnityEngineResourcesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.TextAsset), UnityEngineTextAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Time), UnityEngineTimeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Mathf), UnityEngineMathfWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimationCurve), UnityEngineAnimationCurveWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Animator), UnityEngineAnimatorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimatorStateInfo), UnityEngineAnimatorStateInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Text), UnityEngineUITextWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(TMPro.TextMeshProUGUI), TMProTextMeshProUGUIWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Image), UnityEngineUIImageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Button), UnityEngineUIButtonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.ScrollRect), UnityEngineUIScrollRectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Toggle), UnityEngineUIToggleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Slider), UnityEngineUISliderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Dropdown), UnityEngineUIDropdownWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.InputField), UnityEngineUIInputFieldWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Canvas), UnityEngineCanvasWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CanvasGroup), UnityEngineCanvasGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.EventSystems.PointerEventData), UnityEngineEventSystemsPointerEventDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.EventSystems.BaseEventData), UnityEngineEventSystemsBaseEventDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Events.UnityEvent), UnityEngineEventsUnityEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.List<int>), SystemCollectionsGenericList_1_SystemInt32_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.List<string>), SystemCollectionsGenericList_1_SystemString_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.List<UnityEngine.GameObject>), SystemCollectionsGenericList_1_UnityEngineGameObject_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.List<XLua.LuaTable>), SystemCollectionsGenericList_1_XLuaLuaTable_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.SortedDictionary<string, string>), SystemCollectionsGenericSortedDictionary_2_SystemStringSystemString_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.Tween), DGTweeningTweenWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.Tweener), DGTweeningTweenerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.IEnumerator), SystemCollectionsIEnumeratorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WaitForSeconds), UnityEngineWaitForSecondsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WaitForEndOfFrame), UnityEngineWaitForEndOfFrameWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.ITilemap), UnityEngineTilemapsITilemapWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.Tile), UnityEngineTilemapsTileWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.Tile.ColliderType), UnityEngineTilemapsTileColliderTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.TileBase), UnityEngineTilemapsTileBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.Tilemap), UnityEngineTilemapsTilemapWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.Tilemap.Orientation), UnityEngineTilemapsTilemapOrientationWrap.__Register);
        
        }
        
        static void wrapInit1(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.TileFlags), UnityEngineTilemapsTileFlagsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.TileAnimationFlags), UnityEngineTilemapsTileAnimationFlagsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.TilemapRenderer), UnityEngineTilemapsTilemapRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.TilemapRenderer.SortOrder), UnityEngineTilemapsTilemapRendererSortOrderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.TilemapRenderer.Mode), UnityEngineTilemapsTilemapRendererModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.TilemapRenderer.DetectChunkCullingBounds), UnityEngineTilemapsTilemapRendererDetectChunkCullingBoundsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.TileData), UnityEngineTilemapsTileDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.TileChangeData), UnityEngineTilemapsTileChangeDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.TileAnimationData), UnityEngineTilemapsTileAnimationDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.TilemapCollider2D), UnityEngineTilemapsTilemapCollider2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AssetComponent), HonorRuntimeAssetComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PrefabObject), HonorRuntimePrefabObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AssetBundleObject), HonorRuntimeAssetBundleObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AssetObject), HonorRuntimeAssetObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PreloadAssetObject), HonorRuntimePreloadAssetObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.OriginType), HonorRuntimeOriginTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PrefabDetailType), HonorRuntimePrefabDetailTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PrefabType), HonorRuntimePrefabTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AssetBundleLoadManager), HonorRuntimeAssetBundleLoadManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AssetLoadManager), HonorRuntimeAssetLoadManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PrefabInstanceGOBehaviour), HonorRuntimePrefabInstanceGOBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PrefabLoadManager), HonorRuntimePrefabLoadManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameComponent), HonorRuntimeGameComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameComponentsGroup), HonorRuntimeGameComponentsGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.TimerComponent), HonorRuntimeTimerComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.TimerCounter), HonorRuntimeTimerCounterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants), HonorRuntimeGameConstantsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameDefinitions), HonorRuntimeGameDefinitionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameEventCmd), HonorRuntimeGameEventCmdWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameBinaryExtension), HonorRuntimeGameBinaryExtensionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameException), HonorRuntimeGameExceptionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameExtension), HonorRuntimeGameExtensionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameExtensionForUnity), HonorRuntimeGameExtensionForUnityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AnimationStateBehaviour), HonorRuntimeAnimationStateBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.FocusObjToCamera), HonorRuntimeFocusObjToCameraWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.CameraOutlineAnimation), HonorRuntimeCameraOutlineAnimationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.CameraOutlineBuffer), HonorRuntimeCameraOutlineBufferWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ObjectOutline), HonorRuntimeObjectOutlineWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorClickEventListener), HonorRuntimeAorClickEventListenerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.SnapStatus), HonorRuntimeSnapStatusWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ItemCornerEnum), HonorRuntimeItemCornerEnumWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ListItemArrangeType), HonorRuntimeListItemArrangeTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GridItemArrangeType), HonorRuntimeGridItemArrangeTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GridFixedType), HonorRuntimeGridFixedTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.RowColumnPair), HonorRuntimeRowColumnPairWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ItemSizeGroup), HonorRuntimeItemSizeGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorItemPosMgr), HonorRuntimeAorItemPosMgrWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ItemPool), HonorRuntimeItemPoolWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ItemPrefabConfData), HonorRuntimeItemPrefabConfDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ListViewInitParam), HonorRuntimeListViewInitParamWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.DotElem), HonorRuntimeDotElemWrap.__Register);
        
        }
        
        static void wrapInit2(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorListView), HonorRuntimeAorListViewWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorListViewItem), HonorRuntimeAorListViewItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorSwitchButton), HonorRuntimeAorSwitchButtonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorTextArea), HonorRuntimeAorTextAreaWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorTextEffectCurve), HonorRuntimeAorTextEffectCurveWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorTextEffectGradient), HonorRuntimeAorTextEffectGradientWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorTextEffectOutline), HonorRuntimeAorTextEffectOutlineWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorTextEffectSpacing), HonorRuntimeAorTextEffectSpacingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorTextLocalizing), HonorRuntimeAorTextLocalizingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorTextPicMixed), HonorRuntimeAorTextPicMixedWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ConfigComponent), HonorRuntimeConfigComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ConfigManager), HonorRuntimeConfigManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.EventComponent), HonorRuntimeEventComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.EventParams), HonorRuntimeEventParamsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.EventManager), HonorRuntimeEventManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.LauncherComponent), HonorRuntimeLauncherComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.LocalizationComponent), HonorRuntimeLocalizationComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.LocalizationFontData), HonorRuntimeLocalizationFontDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.LocalizationManager), HonorRuntimeLocalizationManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.LuaBehaviour), HonorRuntimeLuaBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.LuaBindValue), HonorRuntimeLuaBindValueWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.LuaInjection), HonorRuntimeLuaInjectionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.Collider2DLifeCyclesBehaviour), HonorRuntimeCollider2DLifeCyclesBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.Collider3DLifeCyclesBehaviour), HonorRuntimeCollider3DLifeCyclesBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.Trigger2DLifeCyclesBehaviour), HonorRuntimeTrigger2DLifeCyclesBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.Trigger3DLifeCyclesBehaviour), HonorRuntimeTrigger3DLifeCyclesBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PatternType), HonorRuntimePatternTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.NonePatternType), HonorRuntimeNonePatternTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.MVVMPatternType), HonorRuntimeMVVMPatternTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.LuaComponent), HonorRuntimeLuaComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.DirectoryWatcher), HonorRuntimeDirectoryWatcherWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.FileOperation), HonorRuntimeFileOperationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.LuaFileWatcher), HonorRuntimeLuaFileWatcherWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PersistComponent), HonorRuntimePersistComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PersistWayType), HonorRuntimePersistWayTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.FileFragmentItemGroup), HonorRuntimeFileFragmentItemGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.FileFragmentManager), HonorRuntimeFileFragmentManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.FileFragmentForWebGLManager), HonorRuntimeFileFragmentForWebGLManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PlayerPrefsManager), HonorRuntimePlayerPrefsManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PlayingComponent), HonorRuntimePlayingComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GridManager), HonorRuntimeGridManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIFadeInEvent), HonorRuntimeUIFadeInEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIFadeOutEvent), HonorRuntimeUIFadeOutEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIFadeStopEvent), HonorRuntimeUIFadeStopEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIFader), HonorRuntimeUIFaderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ProcedureComponent), HonorRuntimeProcedureComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ProcedureLaunch), HonorRuntimeProcedureLaunchWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ProcedurePlaying), HonorRuntimeProcedurePlayingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ProcedurePreload), HonorRuntimeProcedurePreloadWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ProcedureState), HonorRuntimeProcedureStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ProcedureStateMachine), HonorRuntimeProcedureStateMachineWrap.__Register);
        
        }
        
        static void wrapInit3(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameMainRoot), HonorRuntimeGameMainRootWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.SceneCameraActor), HonorRuntimeSceneCameraActorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.SceneComponent), HonorRuntimeSceneComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.SceneManager), HonorRuntimeSceneManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.SoundComponent), HonorRuntimeSoundComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PlaySoundErrorCode), HonorRuntimePlaySoundErrorCodeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PlaySoundInfo), HonorRuntimePlaySoundInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PlaySoundParams), HonorRuntimePlaySoundParamsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.SoundAgent), HonorRuntimeSoundAgentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.SoundAgentHelper), HonorRuntimeSoundAgentHelperWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.SoundGroup), HonorRuntimeSoundGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.SoundGroupHelper), HonorRuntimeSoundGroupHelperWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.SoundManager), HonorRuntimeSoundManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.PlaySoundInfoShell), HonorRuntimePlaySoundInfoShellWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.SoundGroupShell), HonorRuntimeSoundGroupShellWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.TableComponent), HonorRuntimeTableComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AESEncrypt), HonorRuntimeAESEncryptWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.Assembly), HonorRuntimeAssemblyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.Converter), HonorRuntimeConverterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.DevicePerformanceLevel), HonorRuntimeDevicePerformanceLevelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.QualityLevel), HonorRuntimeQualityLevelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.DevicePerformance), HonorRuntimeDevicePerformanceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.Encryption), HonorRuntimeEncryptionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils), HonorRuntimeGamePathUtilsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GZip), HonorRuntimeGZipWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.Log), HonorRuntimeLogWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.LuaHandler), HonorRuntimeLuaHandlerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.MD5Handler), HonorRuntimeMD5HandlerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.Timer), HonorRuntimeTimerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorTxt), HonorRuntimeAorTxtWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.TouchComponent), HonorRuntimeTouchComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.Gestures2D), HonorRuntimeGestures2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.Gestures3D), HonorRuntimeGestures3DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GesturesUI), HonorRuntimeGesturesUIWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIComponent), HonorRuntimeUIComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIType), HonorRuntimeUITypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIFlagBehaviour), HonorRuntimeUIFlagBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIManager), HonorRuntimeUIManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIInfo), HonorRuntimeUIInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIWebGLWebView), HonorRuntimeUIWebGLWebViewWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.VibrateComponent), HonorRuntimeVibrateComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.VibrateType), HonorRuntimeVibrateTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.VibrateManager), HonorRuntimeVibrateManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.VibrateInfo), HonorRuntimeVibrateInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIAppDownloadBehaviour), HonorRuntimeUIAppDownloadBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIAppFeedbackBehaviour), HonorRuntimeUIAppFeedbackBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIAppReviewBehaviour), HonorRuntimeUIAppReviewBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIConnectionWaitingView), HonorRuntimeUIConnectionWaitingViewWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIFloatWordsBehaviour), HonorRuntimeUIFloatWordsBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIGDPRBehaviour), HonorRuntimeUIGDPRBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UILauncherLoadingView), HonorRuntimeUILauncherLoadingViewWrap.__Register);
        
        }
        
        static void wrapInit4(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UILauncherLogoBehaviour), HonorRuntimeUILauncherLogoBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UILauncherLogoView), HonorRuntimeUILauncherLogoViewWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UILauncherView), HonorRuntimeUILauncherViewWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist), HonorRuntimeGameConstantsPersistWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameDefinitions.DimensionMode), HonorRuntimeGameDefinitionsDimensionModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameDefinitions.AssetType), HonorRuntimeGameDefinitionsAssetTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameDefinitions.PathType), HonorRuntimeGameDefinitionsPathTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameDefinitions.DownloadStep), HonorRuntimeGameDefinitionsDownloadStepWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameDefinitions.DebugMode), HonorRuntimeGameDefinitionsDebugModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameDefinitions.Language), HonorRuntimeGameDefinitionsLanguageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameDefinitions.DebugWindowModel), HonorRuntimeGameDefinitionsDebugWindowModelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorTextEffectOutline.HorizontalAligmentType), HonorRuntimeAorTextEffectOutlineHorizontalAligmentTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorTextEffectOutline.Line), HonorRuntimeAorTextEffectOutlineLineWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorTextEffectSpacing.HorizontalAligmentType), HonorRuntimeAorTextEffectSpacingHorizontalAligmentTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorTextEffectSpacing.Line), HonorRuntimeAorTextEffectSpacingLineWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.ConfigManager.ConfigData), HonorRuntimeConfigManagerConfigDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.LauncherComponent.DevicePerformanceData), HonorRuntimeLauncherComponentDevicePerformanceDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.LuaBindValue.BindValueType), HonorRuntimeLuaBindValueBindValueTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.LuaInjection.InjectionType), HonorRuntimeLuaInjectionInjectionTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UIFader.InitState), HonorRuntimeUIFaderInitStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.AppDownload), HonorRuntimeGamePathUtilsAppDownloadWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.AB), HonorRuntimeGamePathUtilsABWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.FileFragment), HonorRuntimeGamePathUtilsFileFragmentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.LuaScript), HonorRuntimeGamePathUtilsLuaScriptWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Json), HonorRuntimeGamePathUtilsJsonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Font), HonorRuntimeGamePathUtilsFontWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Prefab), HonorRuntimeGamePathUtilsPrefabWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Texture), HonorRuntimeGamePathUtilsTextureWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.PicsForAtlas), HonorRuntimeGamePathUtilsPicsForAtlasWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Localization), HonorRuntimeGamePathUtilsLocalizationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Table), HonorRuntimeGamePathUtilsTableWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Proto), HonorRuntimeGamePathUtilsProtoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Save), HonorRuntimeGamePathUtilsSaveWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Config), HonorRuntimeGamePathUtilsConfigWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.UI), HonorRuntimeGamePathUtilsUIWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Sound), HonorRuntimeGamePathUtilsSoundWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Vibrate), HonorRuntimeGamePathUtilsVibrateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.IDEDebugger), HonorRuntimeGamePathUtilsIDEDebuggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Debugger), HonorRuntimeGamePathUtilsDebuggerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Purchase), HonorRuntimeGamePathUtilsPurchaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Track), HonorRuntimeGamePathUtilsTrackWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.CachedPackageInfo), HonorRuntimeGamePathUtilsCachedPackageInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Project), HonorRuntimeGamePathUtilsProjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Native), HonorRuntimeGamePathUtilsNativeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Tool), HonorRuntimeGamePathUtilsToolWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Editor), HonorRuntimeGamePathUtilsEditorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.GetImage), HonorRuntimeGamePathUtilsGetImageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.Log.LogLevel), HonorRuntimeLogLogLevelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UILauncherLoadingView.LoadingMode), HonorRuntimeUILauncherLoadingViewLoadingModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.Common), HonorRuntimeGameConstantsPersistCommonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.IAP), HonorRuntimeGameConstantsPersistIAPWrap.__Register);
        
        }
        
        static void wrapInit5(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.GDPR), HonorRuntimeGameConstantsPersistGDPRWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.MAX), HonorRuntimeGameConstantsPersistMAXWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.Permission), HonorRuntimeGameConstantsPersistPermissionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.AF), HonorRuntimeGameConstantsPersistAFWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.AppDownload.Uri), HonorRuntimeGamePathUtilsAppDownloadUriWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.AB.ForServer), HonorRuntimeGamePathUtilsABForServerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.AB.Persistent), HonorRuntimeGamePathUtilsABPersistentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.AB.PersistentTmp), HonorRuntimeGamePathUtilsABPersistentTmpWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.AB.Caching), HonorRuntimeGamePathUtilsABCachingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.AB.Streaming), HonorRuntimeGamePathUtilsABStreamingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.AB.Uri), HonorRuntimeGamePathUtilsABUriWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.LuaScript.Framework), HonorRuntimeGamePathUtilsLuaScriptFrameworkWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.LuaScript.Game), HonorRuntimeGamePathUtilsLuaScriptGameWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Proto.Net), HonorRuntimeGamePathUtilsProtoNetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Proto.Save), HonorRuntimeGamePathUtilsProtoSaveWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Editor.ResDef), HonorRuntimeGamePathUtilsEditorResDefWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Editor.HierarchyExpandSettings), HonorRuntimeGamePathUtilsEditorHierarchyExpandSettingsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Editor.LocalizationFontTMPExportSettings), HonorRuntimeGamePathUtilsEditorLocalizationFontTMPExportSettingsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Editor.ABGeneration), HonorRuntimeGamePathUtilsEditorABGenerationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Editor.ChatGPT), HonorRuntimeGamePathUtilsEditorChatGPTWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Editor.CDN), HonorRuntimeGamePathUtilsEditorCDNWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Editor.AssetBundleBrowser), HonorRuntimeGamePathUtilsEditorAssetBundleBrowserWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GamePathUtils.Editor.LuaCloudScript), HonorRuntimeGamePathUtilsEditorLuaCloudScriptWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.Common.ItemKey), HonorRuntimeGameConstantsPersistCommonItemKeyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.IAP.ItemKey), HonorRuntimeGameConstantsPersistIAPItemKeyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.GDPR.ItemKey), HonorRuntimeGameConstantsPersistGDPRItemKeyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.MAX.ItemKey), HonorRuntimeGameConstantsPersistMAXItemKeyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.Permission.ItemKey), HonorRuntimeGameConstantsPersistPermissionItemKeyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.AF.ItemKey), HonorRuntimeGameConstantsPersistAFItemKeyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.ActivationTrack), UnityEngineTimelineActivationTrackWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.AnimationPlayableAsset), UnityEngineTimelineAnimationPlayableAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.MatchTargetFields), UnityEngineTimelineMatchTargetFieldsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.TrackOffset), UnityEngineTimelineTrackOffsetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.AnimationTrack), UnityEngineTimelineAnimationTrackWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.TimelineClip), UnityEngineTimelineTimelineClipWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.TimelineAsset), UnityEngineTimelineTimelineAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.TrackAsset), UnityEngineTimelineTrackAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.AudioPlayableAsset), UnityEngineTimelineAudioPlayableAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.AudioTrack), UnityEngineTimelineAudioTrackWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.ClipCaps), UnityEngineTimelineClipCapsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.ControlPlayableAsset), UnityEngineTimelineControlPlayableAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.ControlTrack), UnityEngineTimelineControlTrackWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.Marker), UnityEngineTimelineMarkerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.MarkerTrack), UnityEngineTimelineMarkerTrackWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.SignalAsset), UnityEngineTimelineSignalAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.SignalEmitter), UnityEngineTimelineSignalEmitterWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.SignalReceiver), UnityEngineTimelineSignalReceiverWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.SignalTrack), UnityEngineTimelineSignalTrackWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.TrackAssetExtensions), UnityEngineTimelineTrackAssetExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.GroupTrack), UnityEngineTimelineGroupTrackWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.ActivationControlPlayable), UnityEngineTimelineActivationControlPlayableWrap.__Register);
        
        }
        
        static void wrapInit6(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.DirectorControlPlayable), UnityEngineTimelineDirectorControlPlayableWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.NotificationFlags), UnityEngineTimelineNotificationFlagsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.ParticleControlPlayable), UnityEngineTimelineParticleControlPlayableWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.PrefabControlPlayable), UnityEngineTimelinePrefabControlPlayableWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.TimeControlPlayable), UnityEngineTimelineTimeControlPlayableWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.TimeNotificationBehaviour), UnityEngineTimelineTimeNotificationBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.PlayableTrack), UnityEngineTimelinePlayableTrackWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.TrackBindingFlags), UnityEngineTimelineTrackBindingFlagsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.TimelinePlayable), UnityEngineTimelineTimelinePlayableWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.TimelineClipExtensions), UnityEngineTimelineTimelineClipExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.StandardFrameRates), UnityEngineTimelineStandardFrameRatesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.ActivationTrack.PostPlaybackState), UnityEngineTimelineActivationTrackPostPlaybackStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.AnimationPlayableAsset.LoopMode), UnityEngineTimelineAnimationPlayableAssetLoopModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.TimelineClip.ClipExtrapolation), UnityEngineTimelineTimelineClipClipExtrapolationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.TimelineClip.BlendCurveMode), UnityEngineTimelineTimelineClipBlendCurveModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.TimelineAsset.DurationMode), UnityEngineTimelineTimelineAssetDurationModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.TimelineAsset.EditorSettings), UnityEngineTimelineTimelineAssetEditorSettingsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.ActivationControlPlayable.PostPlaybackState), UnityEngineTimelineActivationControlPlayablePostPlaybackStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.AnimationReferenceAsset), SpineUnityAnimationReferenceAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.AtlasAssetBase), SpineUnityAtlasAssetBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.BlendModeMaterials), SpineUnityBlendModeMaterialsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.EventDataReferenceAsset), SpineUnityEventDataReferenceAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.RegionlessAttachmentLoader), SpineUnityRegionlessAttachmentLoaderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonDataAsset), SpineUnitySkeletonDataAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonDataModifierAsset), SpineUnitySkeletonDataModifierAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SpineAtlasAsset), SpineUnitySpineAtlasAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.NoOpTextureLoader), SpineUnityNoOpTextureLoaderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.MaterialsTextureLoader), SpineUnityMaterialsTextureLoaderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SpineSpriteAtlasAsset), SpineUnitySpineSpriteAtlasAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.BoneFollower), SpineUnityBoneFollowerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.BoneFollowerGraphic), SpineUnityBoneFollowerGraphicWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.BoundingBoxFollower), SpineUnityBoundingBoxFollowerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.BoundingBoxFollowerGraphic), SpineUnityBoundingBoxFollowerGraphicWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.PointFollower), SpineUnityPointFollowerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonSubmeshGraphic), SpineUnitySkeletonSubmeshGraphicWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonAnimation), SpineUnitySkeletonAnimationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonGraphic), SpineUnitySkeletonGraphicWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonMecanim), SpineUnitySkeletonMecanimWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonRenderer), SpineUnitySkeletonRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonGraphicCustomMaterials), SpineUnitySkeletonGraphicCustomMaterialsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonRendererCustomMaterials), SpineUnitySkeletonRendererCustomMaterialsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonPartsRenderer), SpineUnitySkeletonPartsRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonRenderSeparator), SpineUnitySkeletonRenderSeparatorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.ActivateBasedOnFlipDirection), SpineUnityActivateBasedOnFlipDirectionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.FollowLocationRigidbody), SpineUnityFollowLocationRigidbodyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.FollowLocationRigidbody2D), SpineUnityFollowLocationRigidbody2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.FollowSkeletonUtilityRootRotation), SpineUnityFollowSkeletonUtilityRootRotationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonUtility), SpineUnitySkeletonUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonUtilityBone), SpineUnitySkeletonUtilityBoneWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonUtilityConstraint), SpineUnitySkeletonUtilityConstraintWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.UpdateMode), SpineUnityUpdateModeWrap.__Register);
        
        }
        
        static void wrapInit7(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(Spine.Unity.UpdateTiming), SpineUnityUpdateTimingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.ISpineComponentExtensions), SpineUnityISpineComponentExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.MeshGeneratorBuffers), SpineUnityMeshGeneratorBuffersWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.MeshGenerator), SpineUnityMeshGeneratorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.MeshRendererBuffers), SpineUnityMeshRendererBuffersWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonRendererInstruction), SpineUnitySkeletonRendererInstructionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SpineMesh), SpineUnitySpineMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SubmeshInstruction), SpineUnitySubmeshInstructionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.BlendModeMaterialsAsset), SpineUnityBlendModeMaterialsAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SpineBone), SpineUnitySpineBoneWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SpineSlot), SpineUnitySpineSlotWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SpineAnimation), SpineUnitySpineAnimationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SpineEvent), SpineUnitySpineEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SpineIkConstraint), SpineUnitySpineIkConstraintWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SpineTransformConstraint), SpineUnitySpineTransformConstraintWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SpinePathConstraint), SpineUnitySpinePathConstraintWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SpineSkin), SpineUnitySpineSkinWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SpineAttachment), SpineUnitySpineAttachmentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SpineAtlasRegion), SpineUnitySpineAtlasRegionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.MathUtilities), SpineUnityMathUtilitiesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SettingsTriState), SpineUnitySettingsTriStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonExtensions), SpineUnitySkeletonExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.WaitForSpineAnimation), SpineUnityWaitForSpineAnimationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.WaitForSpineAnimationComplete), SpineUnityWaitForSpineAnimationCompleteWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.WaitForSpineAnimationEnd), SpineUnityWaitForSpineAnimationEndWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.WaitForSpineEvent), SpineUnityWaitForSpineEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.WaitForSpineTrackEntryEnd), SpineUnityWaitForSpineTrackEntryEndWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.BlendModeMaterials.ReplacementMaterial), SpineUnityBlendModeMaterialsReplacementMaterialWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.BoneFollower.AxisOrientation), SpineUnityBoneFollowerAxisOrientationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonGraphic.LayoutMode), SpineUnitySkeletonGraphicLayoutModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonMecanim.MecanimTranslator), SpineUnitySkeletonMecanimMecanimTranslatorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonRenderer.SpriteMaskInteractionMaterials), SpineUnitySkeletonRendererSpriteMaskInteractionMaterialsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonGraphicCustomMaterials.AtlasMaterialOverride), SpineUnitySkeletonGraphicCustomMaterialsAtlasMaterialOverrideWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonGraphicCustomMaterials.AtlasTextureOverride), SpineUnitySkeletonGraphicCustomMaterialsAtlasTextureOverrideWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonRendererCustomMaterials.SlotMaterialOverride), SpineUnitySkeletonRendererCustomMaterialsSlotMaterialOverrideWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonRendererCustomMaterials.AtlasMaterialOverride), SpineUnitySkeletonRendererCustomMaterialsAtlasMaterialOverrideWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonUtilityBone.Mode), SpineUnitySkeletonUtilityBoneModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonUtilityBone.UpdatePhase), SpineUnitySkeletonUtilityBoneUpdatePhaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.MeshGenerator.Settings), SpineUnityMeshGeneratorSettingsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.MeshRendererBuffers.SmartMesh), SpineUnityMeshRendererBuffersSmartMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SpineAttachment.Hierarchy), SpineUnitySpineAttachmentHierarchyWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.SkeletonMecanim.MecanimTranslator.MixMode), SpineUnitySkeletonMecanimMecanimTranslatorMixModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.AutoPlay), DGTweeningAutoPlayWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.AxisConstraint), DGTweeningAxisConstraintWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.Color2), DGTweeningColor2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.DOTween), DGTweeningDOTweenWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.DOVirtual), DGTweeningDOVirtualWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.Ease), DGTweeningEaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.EaseFactory), DGTweeningEaseFactoryWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.PathMode), DGTweeningPathModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.PathType), DGTweeningPathTypeWrap.__Register);
        
        }
        
        static void wrapInit8(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(DG.Tweening.RotateMode), DGTweeningRotateModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.ScrambleMode), DGTweeningScrambleModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.TweenExtensions), DGTweeningTweenExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.LoopType), DGTweeningLoopTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.Sequence), DGTweeningSequenceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.ShortcutExtensions), DGTweeningShortcutExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.TweenParams), DGTweeningTweenParamsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.TweenSettingsExtensions), DGTweeningTweenSettingsExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.LogBehaviour), DGTweeningLogBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.TweenType), DGTweeningTweenTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.UpdateType), DGTweeningUpdateTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.DOTweenUtils46), DGTweeningDOTweenUtils46Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.ShortcutExtensions46), DGTweeningShortcutExtensions46Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.DOTweenVisualManager), DGTweeningDOTweenVisualManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.HandlesDrawMode), DGTweeningHandlesDrawModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.HandlesType), DGTweeningHandlesTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.DOTweenInspectorMode), DGTweeningDOTweenInspectorModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.DOTweenPath), DGTweeningDOTweenPathWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.ShortcutExtensionsPro), DGTweeningShortcutExtensionsProWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.SpiralMode), DGTweeningSpiralModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.ShortcutExtensions43), DGTweeningShortcutExtensions43Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GaussianBlur), GaussianBlurWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(RMapIconType), RMapIconTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(RMapPlayHudPosType), RMapPlayHudPosTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(RMapType), RMapTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(RMapCamPosType), RMapCamPosTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(RMapData), RMapDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(RMapChapterTypeData), RMapChapterTypeDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(MapCamData), MapCamDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(MapData), MapDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(MapController), MapControllerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(MapMarker), MapMarkerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(MapPathDrawer), MapPathDrawerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.GameManager), GameLibGameManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.ProcedureUtils), GameLibProcedureUtilsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.ItemMap), GameLibItemMapWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.AorClickItem), GameLibAorClickItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.AorExtendImage), GameLibAorExtendImageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.AorUIFullScreenImage), GameLibAorUIFullScreenImageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.AorUIMaskLayerBehaviour), GameLibAorUIMaskLayerBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.HonorClickItemAnimation), GameLibHonorClickItemAnimationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.EGameMode), GameLibEGameModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.GridMapManager), GameLibGridMapManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.GridCell), GameLibGridCellWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.MapManager), GameLibMapManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.CSharpLuaTableBridge), GameLibCSharpLuaTableBridgeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.RSetupLuaTableValue), GameLibRSetupLuaTableValueWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.TablesBridge), GameLibTablesBridgeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.JsonHelper), GameLibJsonHelperWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.MapCamHelper), GameLibMapCamHelperWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.MapInBounds), GameLibMapInBoundsWrap.__Register);
        
        }
        
        static void wrapInit9(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(GameLib.ObjectCommon), GameLibObjectCommonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.RTransformExtension), GameLibRTransformExtensionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.Util), GameLibUtilWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.WaitForFrames), GameLibWaitForFramesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.GridMapManager.MapTileData), GameLibGridMapManagerMapTileDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.GridMapManager.MapTilemapData), GameLibGridMapManagerMapTilemapDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.GridMapManager.LayerData), GameLibGridMapManagerLayerDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.GridMapManager.GridTile), GameLibGridMapManagerGridTileWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.GridMapManager.GridType), GameLibGridMapManagerGridTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.GridMapManager.LayerLevel), GameLibGridMapManagerLayerLevelWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.GridMapManager.GridUtils), GameLibGridMapManagerGridUtilsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.RSetupLuaTableValue.RDateTimeWarpYMDHMS), GameLibRSetupLuaTableValueRDateTimeWarpYMDHMSWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.TablesBridge.TableCustomizeItem), GameLibTablesBridgeTableCustomizeItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.TablesBridge.TableAvatarCustomizeItem), GameLibTablesBridgeTableAvatarCustomizeItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.TablesBridge.TableAvatarCustomizeItemColor), GameLibTablesBridgeTableAvatarCustomizeItemColorWrap.__Register);
        
        
        
        }
        
        static void Init(LuaEnv luaenv, ObjectTranslator translator)
        {
            
            wrapInit0(luaenv, translator);
            
            wrapInit1(luaenv, translator);
            
            wrapInit2(luaenv, translator);
            
            wrapInit3(luaenv, translator);
            
            wrapInit4(luaenv, translator);
            
            wrapInit5(luaenv, translator);
            
            wrapInit6(luaenv, translator);
            
            wrapInit7(luaenv, translator);
            
            wrapInit8(luaenv, translator);
            
            wrapInit9(luaenv, translator);
            
            
            translator.AddInterfaceBridgeCreator(typeof(System.Collections.IEnumerator), SystemCollectionsIEnumeratorBridge.__Create);
            
        }
        
	    static XLua_Gen_Initer_Register__()
        {
		    XLua.LuaEnv.AddIniter(Init);
		}
		
		
	}
	
}
namespace XLua
{
	public partial class ObjectTranslator
	{
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ s_gen_reg_dumb_obj = new XLua.CSObjectWrap.XLua_Gen_Initer_Register__();
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ gen_reg_dumb_obj {get{return s_gen_reg_dumb_obj;}}
	}
	
	internal partial class InternalGlobals
    {
	    
		delegate int __GEN_DELEGATE0( System.IO.BinaryReader binaryReader);
		
		delegate void __GEN_DELEGATE1( System.IO.BinaryWriter binaryWriter,  int value);
		
		delegate uint __GEN_DELEGATE2( System.IO.BinaryReader binaryReader);
		
		delegate void __GEN_DELEGATE3( System.IO.BinaryWriter binaryWriter,  uint value);
		
		delegate long __GEN_DELEGATE4( System.IO.BinaryReader binaryReader);
		
		delegate void __GEN_DELEGATE5( System.IO.BinaryWriter binaryWriter,  long value);
		
		delegate ulong __GEN_DELEGATE6( System.IO.BinaryReader binaryReader);
		
		delegate void __GEN_DELEGATE7( System.IO.BinaryWriter binaryWriter,  ulong value);
		
		delegate string __GEN_DELEGATE8( System.IO.BinaryReader binaryReader,  byte[] encryptKey);
		
		delegate void __GEN_DELEGATE9( System.IO.BinaryWriter binaryWriter,  string value,  byte[] encryptKey);
		
		delegate string __GEN_DELEGATE10( string s);
		
		delegate string __GEN_DELEGATE11( string title);
		
		delegate string __GEN_DELEGATE12( string s);
		
		delegate int __GEN_DELEGATE13( string richText);
		
		delegate float __GEN_DELEGATE14( string timeInStringNotation);
		
		delegate string __GEN_DELEGATE15( float t,  bool displayHours,  bool displayMinutes,  bool displaySeconds,  bool displayMilliseconds);
		
		delegate int __GEN_DELEGATE16( string s);
		
		delegate void __GEN_DELEGATE17( UnityEngine.RectTransform rectTransform,  float left);
		
		delegate void __GEN_DELEGATE18( UnityEngine.RectTransform rectTransform,  float right);
		
		delegate void __GEN_DELEGATE19( UnityEngine.RectTransform rectTransform,  float top);
		
		delegate void __GEN_DELEGATE20( UnityEngine.RectTransform rectTransform,  float bottom);
		
		delegate void __GEN_DELEGATE21( UnityEngine.RectTransform rectTransform,  float newValue);
		
		delegate void __GEN_DELEGATE22( UnityEngine.RectTransform rectTransform,  float newValue);
		
		delegate void __GEN_DELEGATE23( UnityEngine.RectTransform rectTransform,  float newValue);
		
		delegate void __GEN_DELEGATE24( UnityEngine.RectTransform rectTransform,  float deltaValue);
		
		delegate void __GEN_DELEGATE25( UnityEngine.RectTransform rectTransform,  float deltaValue);
		
		delegate void __GEN_DELEGATE26( UnityEngine.RectTransform rectTransform,  float deltaValue);
		
		delegate void __GEN_DELEGATE27( UnityEngine.RectTransform rectTransform,  float newValue);
		
		delegate void __GEN_DELEGATE28( UnityEngine.RectTransform rectTransform,  float newValue);
		
		delegate void __GEN_DELEGATE29( UnityEngine.RectTransform rectTransform,  float deltaValue);
		
		delegate void __GEN_DELEGATE30( UnityEngine.RectTransform rectTransform,  float deltaValue);
		
		delegate void __GEN_DELEGATE31( UnityEngine.RectTransform rectTransform);
		
		delegate string __GEN_DELEGATE32( byte b);
		
		delegate string __GEN_DELEGATE33( byte[] bytes);
		
		delegate string __GEN_DELEGATE34( byte[] bytes,  string format);
		
		delegate string __GEN_DELEGATE35( byte[] bytes,  int offset,  int count);
		
		delegate bool __GEN_DELEGATE36( Spine.Unity.ISpineComponent component);
		
		delegate UnityEngine.Color __GEN_DELEGATE37( Spine.Skeleton s);
		
		delegate UnityEngine.Color __GEN_DELEGATE38( Spine.RegionAttachment a);
		
		delegate UnityEngine.Color __GEN_DELEGATE39( Spine.MeshAttachment a);
		
		delegate UnityEngine.Color __GEN_DELEGATE40( Spine.Slot s);
		
		delegate UnityEngine.Color __GEN_DELEGATE41( Spine.Slot s);
		
		delegate void __GEN_DELEGATE42( Spine.Skeleton skeleton,  UnityEngine.Color color);
		
		delegate void __GEN_DELEGATE43( Spine.Skeleton skeleton,  UnityEngine.Color32 color);
		
		delegate void __GEN_DELEGATE44( Spine.Slot slot,  UnityEngine.Color color);
		
		delegate void __GEN_DELEGATE45( Spine.Slot slot,  UnityEngine.Color32 color);
		
		delegate void __GEN_DELEGATE46( Spine.RegionAttachment attachment,  UnityEngine.Color color);
		
		delegate void __GEN_DELEGATE47( Spine.RegionAttachment attachment,  UnityEngine.Color32 color);
		
		delegate void __GEN_DELEGATE48( Spine.MeshAttachment attachment,  UnityEngine.Color color);
		
		delegate void __GEN_DELEGATE49( Spine.MeshAttachment attachment,  UnityEngine.Color32 color);
		
		delegate void __GEN_DELEGATE50( Spine.Skeleton skeleton,  UnityEngine.Vector2 scale);
		
		delegate UnityEngine.Matrix4x4 __GEN_DELEGATE51( Spine.Bone bone);
		
		delegate void __GEN_DELEGATE52( Spine.Bone bone,  UnityEngine.Vector2 position);
		
		delegate void __GEN_DELEGATE53( Spine.Bone bone,  UnityEngine.Vector3 position);
		
		delegate UnityEngine.Vector2 __GEN_DELEGATE54( Spine.Bone bone);
		
		delegate UnityEngine.Vector2 __GEN_DELEGATE55( Spine.Bone bone);
		
		delegate UnityEngine.Vector2 __GEN_DELEGATE56( Spine.Bone bone,  UnityEngine.Vector2 boneLocal);
		
		delegate UnityEngine.Vector3 __GEN_DELEGATE57( Spine.Bone bone,  UnityEngine.Transform spineGameObjectTransform);
		
		delegate UnityEngine.Vector3 __GEN_DELEGATE58( Spine.Bone bone,  UnityEngine.Transform spineGameObjectTransform,  float positionScale);
		
		delegate UnityEngine.Quaternion __GEN_DELEGATE59( Spine.Bone bone);
		
		delegate UnityEngine.Quaternion __GEN_DELEGATE60( Spine.Bone bone);
		
		delegate UnityEngine.Vector2 __GEN_DELEGATE61( Spine.Skeleton skeleton);
		
		delegate void __GEN_DELEGATE62( Spine.Bone bone, out  float ia, out  float ib, out  float ic, out  float id);
		
		delegate UnityEngine.Vector2 __GEN_DELEGATE63( Spine.Bone bone,  UnityEngine.Vector2 worldPosition);
		
		delegate UnityEngine.Vector2 __GEN_DELEGATE64( Spine.Bone bone,  UnityEngine.Vector2 skeletonSpacePosition);
		
		delegate UnityEngine.Material __GEN_DELEGATE65( Spine.Attachment a);
		
		delegate UnityEngine.Vector2[] __GEN_DELEGATE66( Spine.VertexAttachment va,  Spine.Slot slot,  UnityEngine.Vector2[] buffer);
		
		delegate UnityEngine.Vector2[] __GEN_DELEGATE67( Spine.VertexAttachment a,  Spine.Slot slot,  UnityEngine.Vector2[] buffer);
		
		delegate UnityEngine.Vector3 __GEN_DELEGATE68( Spine.PointAttachment attachment,  Spine.Slot slot,  UnityEngine.Transform spineGameObjectTransform);
		
		delegate UnityEngine.Vector3 __GEN_DELEGATE69( Spine.PointAttachment attachment,  Spine.Bone bone,  UnityEngine.Transform spineGameObjectTransform);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE70( UnityEngine.AudioSource target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE71( UnityEngine.AudioSource target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE72( UnityEngine.Camera target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE73( UnityEngine.Camera target,  UnityEngine.Color endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE74( UnityEngine.Camera target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE75( UnityEngine.Camera target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE76( UnityEngine.Camera target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE77( UnityEngine.Camera target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE78( UnityEngine.Camera target,  UnityEngine.Rect endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE79( UnityEngine.Camera target,  UnityEngine.Rect endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE80( UnityEngine.Camera target,  float duration,  float strength,  int vibrato,  float randomness,  bool fadeOut);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE81( UnityEngine.Camera target,  float duration,  UnityEngine.Vector3 strength,  int vibrato,  float randomness,  bool fadeOut);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE82( UnityEngine.Camera target,  float duration,  float strength,  int vibrato,  float randomness,  bool fadeOut);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE83( UnityEngine.Camera target,  float duration,  UnityEngine.Vector3 strength,  int vibrato,  float randomness,  bool fadeOut);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE84( UnityEngine.Light target,  UnityEngine.Color endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE85( UnityEngine.Light target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE86( UnityEngine.Light target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE87( UnityEngine.LineRenderer target,  DG.Tweening.Color2 startValue,  DG.Tweening.Color2 endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE88( UnityEngine.Material target,  UnityEngine.Color endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE89( UnityEngine.Material target,  UnityEngine.Color endValue,  string property,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE90( UnityEngine.Material target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE91( UnityEngine.Material target,  float endValue,  string property,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE92( UnityEngine.Material target,  float endValue,  string property,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE93( UnityEngine.Material target,  UnityEngine.Vector2 endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE94( UnityEngine.Material target,  UnityEngine.Vector2 endValue,  string property,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE95( UnityEngine.Material target,  UnityEngine.Vector2 endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE96( UnityEngine.Material target,  UnityEngine.Vector2 endValue,  string property,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE97( UnityEngine.Material target,  UnityEngine.Vector4 endValue,  string property,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE98( UnityEngine.Rigidbody target,  UnityEngine.Vector3 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE99( UnityEngine.Rigidbody target,  float endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE100( UnityEngine.Rigidbody target,  float endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE101( UnityEngine.Rigidbody target,  float endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE102( UnityEngine.Rigidbody target,  UnityEngine.Vector3 endValue,  float duration,  DG.Tweening.RotateMode mode);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE103( UnityEngine.Rigidbody target,  UnityEngine.Vector3 towards,  float duration,  DG.Tweening.AxisConstraint axisConstraint,  System.Nullable<UnityEngine.Vector3> up);
		
		delegate DG.Tweening.Sequence __GEN_DELEGATE104( UnityEngine.Rigidbody target,  UnityEngine.Vector3 endValue,  float jumpPower,  int numJumps,  float duration,  bool snapping);
		
		delegate DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> __GEN_DELEGATE105( UnityEngine.Rigidbody target,  UnityEngine.Vector3[] path,  float duration,  DG.Tweening.PathType pathType,  DG.Tweening.PathMode pathMode,  int resolution,  System.Nullable<UnityEngine.Color> gizmoColor);
		
		delegate DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> __GEN_DELEGATE106( UnityEngine.Rigidbody target,  UnityEngine.Vector3[] path,  float duration,  DG.Tweening.PathType pathType,  DG.Tweening.PathMode pathMode,  int resolution,  System.Nullable<UnityEngine.Color> gizmoColor);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE107( UnityEngine.TrailRenderer target,  float toStartWidth,  float toEndWidth,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE108( UnityEngine.TrailRenderer target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE109( UnityEngine.Light target,  UnityEngine.Color endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE110( UnityEngine.Material target,  UnityEngine.Color endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE111( UnityEngine.Material target,  UnityEngine.Color endValue,  string property,  float duration);
		
		delegate int __GEN_DELEGATE112( UnityEngine.Material target,  bool withCallbacks);
		
		delegate int __GEN_DELEGATE113( UnityEngine.Material target,  bool complete);
		
		delegate int __GEN_DELEGATE114( UnityEngine.Material target);
		
		delegate int __GEN_DELEGATE115( UnityEngine.Material target,  float to,  bool andPlay);
		
		delegate int __GEN_DELEGATE116( UnityEngine.Material target);
		
		delegate int __GEN_DELEGATE117( UnityEngine.Material target);
		
		delegate int __GEN_DELEGATE118( UnityEngine.Material target);
		
		delegate int __GEN_DELEGATE119( UnityEngine.Material target);
		
		delegate int __GEN_DELEGATE120( UnityEngine.Material target,  bool includeDelay);
		
		delegate int __GEN_DELEGATE121( UnityEngine.Material target,  bool includeDelay);
		
		delegate int __GEN_DELEGATE122( UnityEngine.Material target);
		
		delegate int __GEN_DELEGATE123( UnityEngine.Material target);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE124( DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> t,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE125( DG.Tweening.Core.TweenerCore<UnityEngine.Vector2, UnityEngine.Vector2, DG.Tweening.Plugins.Options.VectorOptions> t,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE126( DG.Tweening.Core.TweenerCore<UnityEngine.Vector2, UnityEngine.Vector2, DG.Tweening.Plugins.Options.VectorOptions> t,  DG.Tweening.AxisConstraint axisConstraint,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE127( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions> t,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE128( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions> t,  DG.Tweening.AxisConstraint axisConstraint,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE129( DG.Tweening.Core.TweenerCore<UnityEngine.Vector4, UnityEngine.Vector4, DG.Tweening.Plugins.Options.VectorOptions> t,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE130( DG.Tweening.Core.TweenerCore<UnityEngine.Vector4, UnityEngine.Vector4, DG.Tweening.Plugins.Options.VectorOptions> t,  DG.Tweening.AxisConstraint axisConstraint,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE131( DG.Tweening.Core.TweenerCore<UnityEngine.Quaternion, UnityEngine.Vector3, DG.Tweening.Plugins.Options.QuaternionOptions> t,  bool useShortest360Route);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE132( DG.Tweening.Core.TweenerCore<UnityEngine.Color, UnityEngine.Color, DG.Tweening.Plugins.Options.ColorOptions> t,  bool alphaOnly);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE133( DG.Tweening.Core.TweenerCore<UnityEngine.Rect, UnityEngine.Rect, DG.Tweening.Plugins.Options.RectOptions> t,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE134( DG.Tweening.Core.TweenerCore<string, string, DG.Tweening.Plugins.Options.StringOptions> t,  bool richTextEnabled,  DG.Tweening.ScrambleMode scrambleMode,  string scrambleChars);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE135( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3[], DG.Tweening.Plugins.Options.Vector3ArrayOptions> t,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE136( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3[], DG.Tweening.Plugins.Options.Vector3ArrayOptions> t,  DG.Tweening.AxisConstraint axisConstraint,  bool snapping);
		
		delegate DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> __GEN_DELEGATE137( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> t,  DG.Tweening.AxisConstraint lockPosition,  DG.Tweening.AxisConstraint lockRotation);
		
		delegate DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> __GEN_DELEGATE138( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> t,  bool closePath,  DG.Tweening.AxisConstraint lockPosition,  DG.Tweening.AxisConstraint lockRotation);
		
		delegate DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> __GEN_DELEGATE139( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> t,  UnityEngine.Vector3 lookAtPosition,  System.Nullable<UnityEngine.Vector3> forwardDirection,  System.Nullable<UnityEngine.Vector3> up);
		
		delegate DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> __GEN_DELEGATE140( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> t,  UnityEngine.Transform lookAtTransform,  System.Nullable<UnityEngine.Vector3> forwardDirection,  System.Nullable<UnityEngine.Vector3> up);
		
		delegate DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> __GEN_DELEGATE141( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> t,  float lookAhead,  System.Nullable<UnityEngine.Vector3> forwardDirection,  System.Nullable<UnityEngine.Vector3> up);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE142( UnityEngine.UI.Graphic target,  UnityEngine.Color endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE143( UnityEngine.UI.Graphic target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE144( UnityEngine.UI.LayoutElement target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE145( UnityEngine.UI.LayoutElement target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE146( UnityEngine.UI.LayoutElement target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE147( UnityEngine.UI.Outline target,  UnityEngine.Color endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE148( UnityEngine.UI.Outline target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE149( UnityEngine.UI.Outline target,  UnityEngine.Vector2 endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE150( UnityEngine.RectTransform target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE151( UnityEngine.RectTransform target,  float endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE152( UnityEngine.RectTransform target,  float endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE153( UnityEngine.RectTransform target,  UnityEngine.Vector3 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE154( UnityEngine.RectTransform target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE155( UnityEngine.RectTransform target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE156( UnityEngine.RectTransform target,  UnityEngine.Vector2 endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE157( UnityEngine.RectTransform target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE158( UnityEngine.RectTransform target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE159( UnityEngine.RectTransform target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE160( UnityEngine.RectTransform target,  UnityEngine.Vector2 punch,  float duration,  int vibrato,  float elasticity,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE161( UnityEngine.RectTransform target,  float duration,  float strength,  int vibrato,  float randomness,  bool snapping,  bool fadeOut);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE162( UnityEngine.RectTransform target,  float duration,  UnityEngine.Vector2 strength,  int vibrato,  float randomness,  bool snapping,  bool fadeOut);
		
		delegate DG.Tweening.Sequence __GEN_DELEGATE163( UnityEngine.RectTransform target,  UnityEngine.Vector2 endValue,  float jumpPower,  int numJumps,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE164( UnityEngine.UI.Graphic target,  UnityEngine.Color endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE165( UnityEngine.Rigidbody target,  float duration,  System.Nullable<UnityEngine.Vector3> axis,  DG.Tweening.SpiralMode mode,  float speed,  float frequency,  float depth,  bool snapping);
		
		delegate DG.Tweening.Sequence __GEN_DELEGATE166( UnityEngine.Material target,  UnityEngine.Gradient gradient,  float duration);
		
		delegate DG.Tweening.Sequence __GEN_DELEGATE167( UnityEngine.Material target,  UnityEngine.Gradient gradient,  string property,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE168( UnityEngine.SpriteRenderer target,  UnityEngine.Color endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE169( UnityEngine.SpriteRenderer target,  float endValue,  float duration);
		
		delegate DG.Tweening.Sequence __GEN_DELEGATE170( UnityEngine.SpriteRenderer target,  UnityEngine.Gradient gradient,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE171( UnityEngine.Rigidbody2D target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE172( UnityEngine.Rigidbody2D target,  float endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE173( UnityEngine.Rigidbody2D target,  float endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE174( UnityEngine.Rigidbody2D target,  float endValue,  float duration);
		
		delegate DG.Tweening.Sequence __GEN_DELEGATE175( UnityEngine.Rigidbody2D target,  UnityEngine.Vector2 endValue,  float jumpPower,  int numJumps,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE176( UnityEngine.SpriteRenderer target,  UnityEngine.Color endValue,  float duration);
		
		delegate float __GEN_DELEGATE177( HedgehogTeam.EasyTouch.BaseFinger transform);
		
		delegate float __GEN_DELEGATE178( HedgehogTeam.EasyTouch.BaseFinger transform);
		
		delegate float __GEN_DELEGATE179( HedgehogTeam.EasyTouch.BaseFinger finger);
		
		delegate float __GEN_DELEGATE180( HedgehogTeam.EasyTouch.BaseFinger finger);
		
	    static InternalGlobals()
		{
		    extensionMethodMap = new Dictionary<Type, IEnumerable<MethodInfo>>()
			{
			    
				{typeof(System.IO.BinaryReader), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE0(Honor.Runtime.GameBinaryExtension.Read7BitEncodedInt32)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE2(Honor.Runtime.GameBinaryExtension.Read7BitEncodedUInt32)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE4(Honor.Runtime.GameBinaryExtension.Read7BitEncodedInt64)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE6(Honor.Runtime.GameBinaryExtension.Read7BitEncodedUInt64)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE8(Honor.Runtime.GameBinaryExtension.ReadEncryptedString)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(System.IO.BinaryWriter), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE1(Honor.Runtime.GameBinaryExtension.Write7BitEncodedInt32)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE3(Honor.Runtime.GameBinaryExtension.Write7BitEncodedUInt32)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE5(Honor.Runtime.GameBinaryExtension.Write7BitEncodedInt64)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE7(Honor.Runtime.GameBinaryExtension.Write7BitEncodedUInt64)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE9(Honor.Runtime.GameBinaryExtension.WriteEncryptedString)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(string), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE10(Honor.Runtime.GameExtension.UppercaseFirst)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE11(Honor.Runtime.GameExtension.ToTitleCase)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE12(Honor.Runtime.GameExtension.RemoveExtraSpaces)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE13(Honor.Runtime.GameExtension.RichTextLength)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE14(Honor.Runtime.GameExtension.TimeStringToFloat)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE16(Honor.Runtime.GameExtension.GetChineseNum)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(float), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE15(Honor.Runtime.GameExtension.FloatToTimeString)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.RectTransform), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE17(Honor.Runtime.GameExtensionForUnity.SetLeft)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE18(Honor.Runtime.GameExtensionForUnity.SetRight)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE19(Honor.Runtime.GameExtensionForUnity.SetTop)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE20(Honor.Runtime.GameExtensionForUnity.SetBottom)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE21(Honor.Runtime.GameExtensionForUnity.SetAnchoredPositionX3D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE22(Honor.Runtime.GameExtensionForUnity.SetAnchoredPositionY3D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE23(Honor.Runtime.GameExtensionForUnity.SetAnchoredPositionZ3D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE24(Honor.Runtime.GameExtensionForUnity.AddAnchoredPositionX3D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE25(Honor.Runtime.GameExtensionForUnity.AddAnchoredPositionY3D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE26(Honor.Runtime.GameExtensionForUnity.AddAnchoredPositionZ3D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE27(Honor.Runtime.GameExtensionForUnity.SetAnchoredPositionX)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE28(Honor.Runtime.GameExtensionForUnity.SetAnchoredPositionY)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE29(Honor.Runtime.GameExtensionForUnity.AddAnchoredPositionX)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE30(Honor.Runtime.GameExtensionForUnity.AddAnchoredPositionY)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE31(Honor.Runtime.GameExtensionForUnity.ForceRebuildLayoutImmediate)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE150(DG.Tweening.ShortcutExtensions46.DOAnchorPos)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE151(DG.Tweening.ShortcutExtensions46.DOAnchorPosX)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE152(DG.Tweening.ShortcutExtensions46.DOAnchorPosY)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE153(DG.Tweening.ShortcutExtensions46.DOAnchorPos3D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE154(DG.Tweening.ShortcutExtensions46.DOAnchorMax)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE155(DG.Tweening.ShortcutExtensions46.DOAnchorMin)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE156(DG.Tweening.ShortcutExtensions46.DOPivot)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE157(DG.Tweening.ShortcutExtensions46.DOPivotX)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE158(DG.Tweening.ShortcutExtensions46.DOPivotY)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE159(DG.Tweening.ShortcutExtensions46.DOSizeDelta)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE160(DG.Tweening.ShortcutExtensions46.DOPunchAnchorPos)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE161(DG.Tweening.ShortcutExtensions46.DOShakeAnchorPos)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE162(DG.Tweening.ShortcutExtensions46.DOShakeAnchorPos)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE163(DG.Tweening.ShortcutExtensions46.DOJumpAnchorPos)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(byte), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE32(Honor.Runtime.Converter.ToHex)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(byte[]), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE33(Honor.Runtime.Converter.ToHex)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE34(Honor.Runtime.Converter.ToHex)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE35(Honor.Runtime.Converter.ToHex)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.Unity.ISpineComponent), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE36(Spine.Unity.ISpineComponentExtensions.IsNullOrDestroyed)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.Skeleton), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE37(Spine.Unity.SkeletonExtensions.GetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE42(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE43(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE50(Spine.Unity.SkeletonExtensions.SetLocalScale)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE61(Spine.Unity.SkeletonExtensions.GetLocalScale)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.RegionAttachment), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE38(Spine.Unity.SkeletonExtensions.GetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE46(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE47(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.MeshAttachment), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE39(Spine.Unity.SkeletonExtensions.GetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE48(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE49(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.Slot), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE40(Spine.Unity.SkeletonExtensions.GetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE41(Spine.Unity.SkeletonExtensions.GetColorTintBlack)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE44(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE45(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.Bone), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE51(Spine.Unity.SkeletonExtensions.GetMatrix4x4)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE52(Spine.Unity.SkeletonExtensions.SetLocalPosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE53(Spine.Unity.SkeletonExtensions.SetLocalPosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE54(Spine.Unity.SkeletonExtensions.GetLocalPosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE55(Spine.Unity.SkeletonExtensions.GetSkeletonSpacePosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE56(Spine.Unity.SkeletonExtensions.GetSkeletonSpacePosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE57(Spine.Unity.SkeletonExtensions.GetWorldPosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE58(Spine.Unity.SkeletonExtensions.GetWorldPosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE59(Spine.Unity.SkeletonExtensions.GetQuaternion)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE60(Spine.Unity.SkeletonExtensions.GetLocalQuaternion)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE62(Spine.Unity.SkeletonExtensions.GetWorldToLocalMatrix)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE63(Spine.Unity.SkeletonExtensions.WorldToLocal)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE64(Spine.Unity.SkeletonExtensions.SetPositionSkeletonSpace)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.Attachment), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE65(Spine.Unity.SkeletonExtensions.GetMaterial)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.VertexAttachment), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE66(Spine.Unity.SkeletonExtensions.GetLocalVertices)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE67(Spine.Unity.SkeletonExtensions.GetWorldVertices)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.PointAttachment), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE68(Spine.Unity.SkeletonExtensions.GetWorldPosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE69(Spine.Unity.SkeletonExtensions.GetWorldPosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.AudioSource), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE70(DG.Tweening.ShortcutExtensions.DOFade)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE71(DG.Tweening.ShortcutExtensions.DOPitch)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.Camera), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE72(DG.Tweening.ShortcutExtensions.DOAspect)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE73(DG.Tweening.ShortcutExtensions.DOColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE74(DG.Tweening.ShortcutExtensions.DOFarClipPlane)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE75(DG.Tweening.ShortcutExtensions.DOFieldOfView)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE76(DG.Tweening.ShortcutExtensions.DONearClipPlane)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE77(DG.Tweening.ShortcutExtensions.DOOrthoSize)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE78(DG.Tweening.ShortcutExtensions.DOPixelRect)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE79(DG.Tweening.ShortcutExtensions.DORect)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE80(DG.Tweening.ShortcutExtensions.DOShakePosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE81(DG.Tweening.ShortcutExtensions.DOShakePosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE82(DG.Tweening.ShortcutExtensions.DOShakeRotation)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE83(DG.Tweening.ShortcutExtensions.DOShakeRotation)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.Light), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE84(DG.Tweening.ShortcutExtensions.DOColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE85(DG.Tweening.ShortcutExtensions.DOIntensity)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE86(DG.Tweening.ShortcutExtensions.DOShadowStrength)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE109(DG.Tweening.ShortcutExtensions.DOBlendableColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.LineRenderer), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE87(DG.Tweening.ShortcutExtensions.DOColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.Material), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE88(DG.Tweening.ShortcutExtensions.DOColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE89(DG.Tweening.ShortcutExtensions.DOColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE90(DG.Tweening.ShortcutExtensions.DOFade)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE91(DG.Tweening.ShortcutExtensions.DOFade)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE92(DG.Tweening.ShortcutExtensions.DOFloat)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE93(DG.Tweening.ShortcutExtensions.DOOffset)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE94(DG.Tweening.ShortcutExtensions.DOOffset)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE95(DG.Tweening.ShortcutExtensions.DOTiling)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE96(DG.Tweening.ShortcutExtensions.DOTiling)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE97(DG.Tweening.ShortcutExtensions.DOVector)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE110(DG.Tweening.ShortcutExtensions.DOBlendableColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE111(DG.Tweening.ShortcutExtensions.DOBlendableColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE112(DG.Tweening.ShortcutExtensions.DOComplete)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE113(DG.Tweening.ShortcutExtensions.DOKill)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE114(DG.Tweening.ShortcutExtensions.DOFlip)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE115(DG.Tweening.ShortcutExtensions.DOGoto)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE116(DG.Tweening.ShortcutExtensions.DOPause)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE117(DG.Tweening.ShortcutExtensions.DOPlay)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE118(DG.Tweening.ShortcutExtensions.DOPlayBackwards)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE119(DG.Tweening.ShortcutExtensions.DOPlayForward)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE120(DG.Tweening.ShortcutExtensions.DORestart)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE121(DG.Tweening.ShortcutExtensions.DORewind)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE122(DG.Tweening.ShortcutExtensions.DOSmoothRewind)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE123(DG.Tweening.ShortcutExtensions.DOTogglePause)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE166(DG.Tweening.ShortcutExtensions43.DOGradientColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE167(DG.Tweening.ShortcutExtensions43.DOGradientColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.Rigidbody), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE98(DG.Tweening.ShortcutExtensions.DOMove)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE99(DG.Tweening.ShortcutExtensions.DOMoveX)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE100(DG.Tweening.ShortcutExtensions.DOMoveY)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE101(DG.Tweening.ShortcutExtensions.DOMoveZ)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE102(DG.Tweening.ShortcutExtensions.DORotate)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE103(DG.Tweening.ShortcutExtensions.DOLookAt)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE104(DG.Tweening.ShortcutExtensions.DOJump)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE105(DG.Tweening.ShortcutExtensions.DOPath)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE106(DG.Tweening.ShortcutExtensions.DOLocalPath)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE165(DG.Tweening.ShortcutExtensionsPro.DOSpiral)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.TrailRenderer), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE107(DG.Tweening.ShortcutExtensions.DOResize)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE108(DG.Tweening.ShortcutExtensions.DOTime)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE124(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Vector2, UnityEngine.Vector2, DG.Tweening.Plugins.Options.VectorOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE125(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE126(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE127(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE128(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Vector4, UnityEngine.Vector4, DG.Tweening.Plugins.Options.VectorOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE129(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE130(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Quaternion, UnityEngine.Vector3, DG.Tweening.Plugins.Options.QuaternionOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE131(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Color, UnityEngine.Color, DG.Tweening.Plugins.Options.ColorOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE132(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Rect, UnityEngine.Rect, DG.Tweening.Plugins.Options.RectOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE133(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<string, string, DG.Tweening.Plugins.Options.StringOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE134(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3[], DG.Tweening.Plugins.Options.Vector3ArrayOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE135(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE136(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE137(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE138(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE139(DG.Tweening.TweenSettingsExtensions.SetLookAt)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE140(DG.Tweening.TweenSettingsExtensions.SetLookAt)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE141(DG.Tweening.TweenSettingsExtensions.SetLookAt)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.UI.Graphic), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE142(DG.Tweening.ShortcutExtensions46.DOColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE143(DG.Tweening.ShortcutExtensions46.DOFade)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE164(DG.Tweening.ShortcutExtensions46.DOBlendableColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.UI.LayoutElement), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE144(DG.Tweening.ShortcutExtensions46.DOFlexibleSize)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE145(DG.Tweening.ShortcutExtensions46.DOMinSize)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE146(DG.Tweening.ShortcutExtensions46.DOPreferredSize)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.UI.Outline), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE147(DG.Tweening.ShortcutExtensions46.DOColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE148(DG.Tweening.ShortcutExtensions46.DOFade)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE149(DG.Tweening.ShortcutExtensions46.DOScale)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.SpriteRenderer), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE168(DG.Tweening.ShortcutExtensions43.DOColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE169(DG.Tweening.ShortcutExtensions43.DOFade)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE170(DG.Tweening.ShortcutExtensions43.DOGradientColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE176(DG.Tweening.ShortcutExtensions43.DOBlendableColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.Rigidbody2D), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE171(DG.Tweening.ShortcutExtensions43.DOMove)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE172(DG.Tweening.ShortcutExtensions43.DOMoveX)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE173(DG.Tweening.ShortcutExtensions43.DOMoveY)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE174(DG.Tweening.ShortcutExtensions43.DORotate)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE175(DG.Tweening.ShortcutExtensions43.DOJump)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(HedgehogTeam.EasyTouch.BaseFinger), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE177(GameLib.RTransformExtension.GetPositionX)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE178(GameLib.RTransformExtension.GetPositionY)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE179(GameLib.RTransformExtension.GetDeltaPositionX)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE180(GameLib.RTransformExtension.GetDeltaPositionY)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
			};
			
			genTryArrayGetPtr = StaticLuaCallbacks.__tryArrayGet;
            genTryArraySetPtr = StaticLuaCallbacks.__tryArraySet;
		}
	}
}
