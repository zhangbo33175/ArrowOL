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
        
        
            translator.DelayWrapLoader(typeof(System.TimeSpan), SystemTimeSpanWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Rect), UnityEngineRectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Vector2), UnityEngineVector2Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Vector3), UnityEngineVector3Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Vector4), UnityEngineVector4Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Quaternion), UnityEngineQuaternionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Color), UnityEngineColorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Color32), UnityEngineColor32Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Ray), UnityEngineRayWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Bounds), UnityEngineBoundsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Ray2D), UnityEngineRay2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Time), UnityEngineTimeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.GameObject), UnityEngineGameObjectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Component), UnityEngineComponentWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Behaviour), UnityEngineBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Transform), UnityEngineTransformWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Resources), UnityEngineResourcesWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.TextAsset), UnityEngineTextAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Keyframe), UnityEngineKeyframeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimationCurve), UnityEngineAnimationCurveWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimationClip), UnityEngineAnimationClipWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Animator), UnityEngineAnimatorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimatorStateInfo), UnityEngineAnimatorStateInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AnimatorUpdateMode), UnityEngineAnimatorUpdateModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RuntimeAnimatorController), UnityEngineRuntimeAnimatorControllerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.MonoBehaviour), UnityEngineMonoBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ParticleSystem), UnityEngineParticleSystemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SkinnedMeshRenderer), UnityEngineSkinnedMeshRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Renderer), UnityEngineRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Light), UnityEngineLightWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Mathf), UnityEngineMathfWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Debug), UnityEngineDebugWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.SpriteRenderer), UnityEngineSpriteRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Text), UnityEngineUITextWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(TMPro.TextMeshProUGUI), TMProTextMeshProUGUIWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Image), UnityEngineUIImageWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Button), UnityEngineUIButtonWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Scrollbar), UnityEngineUIScrollbarWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.ScrollRect), UnityEngineUIScrollRectWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Toggle), UnityEngineUIToggleWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Slider), UnityEngineUISliderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Dropdown), UnityEngineUIDropdownWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.InputField), UnityEngineUIInputFieldWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Canvas), UnityEngineCanvasWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Camera), UnityEngineCameraWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Button.ButtonClickedEvent), UnityEngineUIButtonButtonClickedEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.InputField.OnChangeEvent), UnityEngineUIInputFieldOnChangeEventWrap.__Register);
        
        }
        
        static void wrapInit1(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.InputField.SubmitEvent), UnityEngineUIInputFieldSubmitEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Dropdown.DropdownEvent), UnityEngineUIDropdownDropdownEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Toggle.ToggleEvent), UnityEngineUIToggleToggleEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.UI.Slider.SliderEvent), UnityEngineUISliderSliderEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.EventSystems.PointerEventData), UnityEngineEventSystemsPointerEventDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.EventSystems.BaseEventData), UnityEngineEventSystemsBaseEventDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.EventArgs), SystemEventArgsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioSource), UnityEngineAudioSourceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.AudioClip), UnityEngineAudioClipWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Rendering.SortingGroup), UnityEngineRenderingSortingGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CanvasGroup), UnityEngineCanvasGroupWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.LineRenderer), UnityEngineLineRendererWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.RectTransformUtility), UnityEngineRectTransformUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.MaterialPropertyBlock), UnityEngineMaterialPropertyBlockWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Rigidbody2D), UnityEngineRigidbody2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Collider2D), UnityEngineCollider2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.CircleCollider2D), UnityEngineCircleCollider2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.EdgeCollider2D), UnityEngineEdgeCollider2DWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.ColorUtility), UnityEngineColorUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.IEnumerator), SystemCollectionsIEnumeratorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Events.UnityEvent), UnityEngineEventsUnityEventWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Events.UnityEvent<string>), UnityEngineEventsUnityEvent_1_SystemString_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Events.UnityEvent<int>), UnityEngineEventsUnityEvent_1_SystemInt32_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Events.UnityEvent<bool>), UnityEngineEventsUnityEvent_1_SystemBoolean_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Events.UnityEvent<float>), UnityEngineEventsUnityEvent_1_SystemSingle_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Events.UnityEvent<UnityEngine.Vector2>), UnityEngineEventsUnityEvent_1_UnityEngineVector2_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.List<int>), SystemCollectionsGenericList_1_SystemInt32_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.List<string>), SystemCollectionsGenericList_1_SystemString_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.List<bool>), SystemCollectionsGenericList_1_SystemBoolean_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.List<float>), SystemCollectionsGenericList_1_SystemSingle_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.List<UnityEngine.GameObject>), SystemCollectionsGenericList_1_UnityEngineGameObject_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.List<Honor.Runtime.PrefabInstanceGOBehaviour>), SystemCollectionsGenericList_1_HonorRuntimePrefabInstanceGOBehaviour_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.List<Honor.Runtime.UIFlagBehaviour>), SystemCollectionsGenericList_1_HonorRuntimeUIFlagBehaviour_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.List<XLua.LuaTable>), SystemCollectionsGenericList_1_XLuaLuaTable_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.List<UnityEngine.EventSystems.EventTrigger.Entry>), SystemCollectionsGenericList_1_UnityEngineEventSystemsEventTriggerEntry_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Collections.Generic.SortedDictionary<string, string>), SystemCollectionsGenericSortedDictionary_2_SystemStringSystemString_Wrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.ShortcutExtensions), DGTweeningShortcutExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.Tween), DGTweeningTweenWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.Tweener), DGTweeningTweenerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WaitForSeconds), UnityEngineWaitForSecondsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WaitForEndOfFrame), UnityEngineWaitForEndOfFrameWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WaitForFixedUpdate), UnityEngineWaitForFixedUpdateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.WaitUntil), UnityEngineWaitUntilWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(System.Uri), SystemUriWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.ITilemap), UnityEngineTilemapsITilemapWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.Tile), UnityEngineTilemapsTileWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.Tile.ColliderType), UnityEngineTilemapsTileColliderTypeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.TileBase), UnityEngineTilemapsTileBaseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.Tilemap), UnityEngineTilemapsTilemapWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.Tilemap.Orientation), UnityEngineTilemapsTilemapOrientationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Tilemaps.TileFlags), UnityEngineTilemapsTileFlagsWrap.__Register);
        
        }
        
        static void wrapInit2(LuaEnv luaenv, ObjectTranslator translator)
        {
        
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
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.AorListView), HonorRuntimeAorListViewWrap.__Register);
        
        }
        
        static void wrapInit3(LuaEnv luaenv, ObjectTranslator translator)
        {
        
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
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameMainRoot), HonorRuntimeGameMainRootWrap.__Register);
        
        }
        
        static void wrapInit4(LuaEnv luaenv, ObjectTranslator translator)
        {
        
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
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UILauncherLogoBehaviour), HonorRuntimeUILauncherLogoBehaviourWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UILauncherLogoView), HonorRuntimeUILauncherLogoViewWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.UILauncherView), HonorRuntimeUILauncherViewWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist), HonorRuntimeGameConstantsPersistWrap.__Register);
        
        }
        
        static void wrapInit5(LuaEnv luaenv, ObjectTranslator translator)
        {
        
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
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.GDPR), HonorRuntimeGameConstantsPersistGDPRWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.MAX), HonorRuntimeGameConstantsPersistMAXWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.Permission), HonorRuntimeGameConstantsPersistPermissionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Honor.Runtime.GameConstants.Persist.AF), HonorRuntimeGameConstantsPersistAFWrap.__Register);
        
        }
        
        static void wrapInit6(LuaEnv luaenv, ObjectTranslator translator)
        {
        
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
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.DirectorControlPlayable), UnityEngineTimelineDirectorControlPlayableWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.NotificationFlags), UnityEngineTimelineNotificationFlagsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.ParticleControlPlayable), UnityEngineTimelineParticleControlPlayableWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(UnityEngine.Timeline.PrefabControlPlayable), UnityEngineTimelinePrefabControlPlayableWrap.__Register);
        
        }
        
        static void wrapInit7(LuaEnv luaenv, ObjectTranslator translator)
        {
        
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
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.UpdateTiming), SpineUnityUpdateTimingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.ISpineComponentExtensions), SpineUnityISpineComponentExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.MeshGeneratorBuffers), SpineUnityMeshGeneratorBuffersWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Spine.Unity.MeshGenerator), SpineUnityMeshGeneratorWrap.__Register);
        
        }
        
        static void wrapInit8(LuaEnv luaenv, ObjectTranslator translator)
        {
        
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
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.RotateMode), DGTweeningRotateModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.ScrambleMode), DGTweeningScrambleModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.TweenExtensions), DGTweeningTweenExtensionsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(DG.Tweening.LoopType), DGTweeningLoopTypeWrap.__Register);
        
        }
        
        static void wrapInit9(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(DG.Tweening.Sequence), DGTweeningSequenceWrap.__Register);
        
        
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
        
        
            translator.DelayWrapLoader(typeof(MapObjectData), MapObjectDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(MapAssetData), MapAssetDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(MapData), MapDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(MapController), MapControllerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(MapMarker), MapMarkerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(MapPathDrawer), MapPathDrawerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(MapEditor), MapEditorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.GameManager), GameLibGameManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.ProcedureUtils), GameLibProcedureUtilsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.ItemMap), GameLibItemMapWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.AorClickItem), GameLibAorClickItemWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.HonorClickItemAnimation), GameLibHonorClickItemAnimationWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.EGameMode), GameLibEGameModeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.GridMapManager), GameLibGridMapManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.GridCell), GameLibGridCellWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.MapManager), GameLibMapManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.CSharpLuaTableBridge), GameLibCSharpLuaTableBridgeWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.RSetupLuaTableValue), GameLibRSetupLuaTableValueWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(GameLib.TablesBridge), GameLibTablesBridgeWrap.__Register);
        
        
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
	    
		delegate DG.Tweening.Tweener __GEN_DELEGATE0( UnityEngine.Material target,  UnityEngine.Color endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE1( UnityEngine.Material target,  UnityEngine.Color endValue,  string property,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE2( UnityEngine.Material target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE3( UnityEngine.Material target,  float endValue,  string property,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE4( UnityEngine.Material target,  float endValue,  string property,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE5( UnityEngine.Material target,  UnityEngine.Vector2 endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE6( UnityEngine.Material target,  UnityEngine.Vector2 endValue,  string property,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE7( UnityEngine.Material target,  UnityEngine.Vector2 endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE8( UnityEngine.Material target,  UnityEngine.Vector2 endValue,  string property,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE9( UnityEngine.Material target,  UnityEngine.Vector4 endValue,  string property,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE10( UnityEngine.Rigidbody target,  UnityEngine.Vector3 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE11( UnityEngine.Rigidbody target,  float endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE12( UnityEngine.Rigidbody target,  float endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE13( UnityEngine.Rigidbody target,  float endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE14( UnityEngine.Rigidbody target,  UnityEngine.Vector3 endValue,  float duration,  DG.Tweening.RotateMode mode);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE15( UnityEngine.Rigidbody target,  UnityEngine.Vector3 towards,  float duration,  DG.Tweening.AxisConstraint axisConstraint,  System.Nullable<UnityEngine.Vector3> up);
		
		delegate DG.Tweening.Sequence __GEN_DELEGATE16( UnityEngine.Rigidbody target,  UnityEngine.Vector3 endValue,  float jumpPower,  int numJumps,  float duration,  bool snapping);
		
		delegate DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> __GEN_DELEGATE17( UnityEngine.Rigidbody target,  UnityEngine.Vector3[] path,  float duration,  DG.Tweening.PathType pathType,  DG.Tweening.PathMode pathMode,  int resolution,  System.Nullable<UnityEngine.Color> gizmoColor);
		
		delegate DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> __GEN_DELEGATE18( UnityEngine.Rigidbody target,  UnityEngine.Vector3[] path,  float duration,  DG.Tweening.PathType pathType,  DG.Tweening.PathMode pathMode,  int resolution,  System.Nullable<UnityEngine.Color> gizmoColor);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE19( UnityEngine.TrailRenderer target,  float toStartWidth,  float toEndWidth,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE20( UnityEngine.TrailRenderer target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE21( UnityEngine.Material target,  UnityEngine.Color endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE22( UnityEngine.Material target,  UnityEngine.Color endValue,  string property,  float duration);
		
		delegate int __GEN_DELEGATE23( UnityEngine.Material target,  bool withCallbacks);
		
		delegate int __GEN_DELEGATE24( UnityEngine.Material target,  bool complete);
		
		delegate int __GEN_DELEGATE25( UnityEngine.Material target);
		
		delegate int __GEN_DELEGATE26( UnityEngine.Material target,  float to,  bool andPlay);
		
		delegate int __GEN_DELEGATE27( UnityEngine.Material target);
		
		delegate int __GEN_DELEGATE28( UnityEngine.Material target);
		
		delegate int __GEN_DELEGATE29( UnityEngine.Material target);
		
		delegate int __GEN_DELEGATE30( UnityEngine.Material target);
		
		delegate int __GEN_DELEGATE31( UnityEngine.Material target,  bool includeDelay);
		
		delegate int __GEN_DELEGATE32( UnityEngine.Material target,  bool includeDelay);
		
		delegate int __GEN_DELEGATE33( UnityEngine.Material target);
		
		delegate int __GEN_DELEGATE34( UnityEngine.Material target);
		
		delegate int __GEN_DELEGATE35( System.IO.BinaryReader binaryReader);
		
		delegate void __GEN_DELEGATE36( System.IO.BinaryWriter binaryWriter,  int value);
		
		delegate uint __GEN_DELEGATE37( System.IO.BinaryReader binaryReader);
		
		delegate void __GEN_DELEGATE38( System.IO.BinaryWriter binaryWriter,  uint value);
		
		delegate long __GEN_DELEGATE39( System.IO.BinaryReader binaryReader);
		
		delegate void __GEN_DELEGATE40( System.IO.BinaryWriter binaryWriter,  long value);
		
		delegate ulong __GEN_DELEGATE41( System.IO.BinaryReader binaryReader);
		
		delegate void __GEN_DELEGATE42( System.IO.BinaryWriter binaryWriter,  ulong value);
		
		delegate string __GEN_DELEGATE43( System.IO.BinaryReader binaryReader,  byte[] encryptBytes);
		
		delegate void __GEN_DELEGATE44( System.IO.BinaryWriter binaryWriter,  string value,  byte[] encryptBytes);
		
		delegate string __GEN_DELEGATE45( string s);
		
		delegate int __GEN_DELEGATE46( string richText);
		
		delegate string __GEN_DELEGATE47( string title);
		
		delegate string __GEN_DELEGATE48( string s);
		
		delegate float __GEN_DELEGATE49( string timeInStringNotation);
		
		delegate int __GEN_DELEGATE50( string s);
		
		delegate string __GEN_DELEGATE51( float t,  bool displayHours,  bool displayMinutes,  bool displaySeconds,  bool displayMilliseconds);
		
		delegate void __GEN_DELEGATE52( UnityEngine.RectTransform rectTransform,  float left);
		
		delegate void __GEN_DELEGATE53( UnityEngine.RectTransform rectTransform,  float right);
		
		delegate void __GEN_DELEGATE54( UnityEngine.RectTransform rectTransform,  float top);
		
		delegate void __GEN_DELEGATE55( UnityEngine.RectTransform rectTransform,  float bottom);
		
		delegate void __GEN_DELEGATE56( UnityEngine.RectTransform rectTransform,  float newValue);
		
		delegate void __GEN_DELEGATE57( UnityEngine.RectTransform rectTransform,  float newValue);
		
		delegate void __GEN_DELEGATE58( UnityEngine.RectTransform rectTransform,  float newValue);
		
		delegate void __GEN_DELEGATE59( UnityEngine.RectTransform rectTransform,  float deltaValue);
		
		delegate void __GEN_DELEGATE60( UnityEngine.RectTransform rectTransform,  float deltaValue);
		
		delegate void __GEN_DELEGATE61( UnityEngine.RectTransform rectTransform,  float deltaValue);
		
		delegate void __GEN_DELEGATE62( UnityEngine.RectTransform rectTransform,  float newValue);
		
		delegate void __GEN_DELEGATE63( UnityEngine.RectTransform rectTransform,  float newValue);
		
		delegate void __GEN_DELEGATE64( UnityEngine.RectTransform rectTransform,  float deltaValue);
		
		delegate void __GEN_DELEGATE65( UnityEngine.RectTransform rectTransform,  float deltaValue);
		
		delegate void __GEN_DELEGATE66( UnityEngine.RectTransform rectTransform);
		
		delegate string __GEN_DELEGATE67( byte b);
		
		delegate string __GEN_DELEGATE68( byte[] bytes);
		
		delegate string __GEN_DELEGATE69( byte[] bytes,  string format);
		
		delegate string __GEN_DELEGATE70( byte[] bytes,  int offset,  int count);
		
		delegate bool __GEN_DELEGATE71( Spine.Unity.ISpineComponent component);
		
		delegate UnityEngine.Color __GEN_DELEGATE72( Spine.Skeleton s);
		
		delegate UnityEngine.Color __GEN_DELEGATE73( Spine.RegionAttachment a);
		
		delegate UnityEngine.Color __GEN_DELEGATE74( Spine.MeshAttachment a);
		
		delegate UnityEngine.Color __GEN_DELEGATE75( Spine.Slot s);
		
		delegate UnityEngine.Color __GEN_DELEGATE76( Spine.Slot s);
		
		delegate void __GEN_DELEGATE77( Spine.Skeleton skeleton,  UnityEngine.Color color);
		
		delegate void __GEN_DELEGATE78( Spine.Skeleton skeleton,  UnityEngine.Color32 color);
		
		delegate void __GEN_DELEGATE79( Spine.Slot slot,  UnityEngine.Color color);
		
		delegate void __GEN_DELEGATE80( Spine.Slot slot,  UnityEngine.Color32 color);
		
		delegate void __GEN_DELEGATE81( Spine.RegionAttachment attachment,  UnityEngine.Color color);
		
		delegate void __GEN_DELEGATE82( Spine.RegionAttachment attachment,  UnityEngine.Color32 color);
		
		delegate void __GEN_DELEGATE83( Spine.MeshAttachment attachment,  UnityEngine.Color color);
		
		delegate void __GEN_DELEGATE84( Spine.MeshAttachment attachment,  UnityEngine.Color32 color);
		
		delegate void __GEN_DELEGATE85( Spine.Skeleton skeleton,  UnityEngine.Vector2 scale);
		
		delegate UnityEngine.Matrix4x4 __GEN_DELEGATE86( Spine.Bone bone);
		
		delegate void __GEN_DELEGATE87( Spine.Bone bone,  UnityEngine.Vector2 position);
		
		delegate void __GEN_DELEGATE88( Spine.Bone bone,  UnityEngine.Vector3 position);
		
		delegate UnityEngine.Vector2 __GEN_DELEGATE89( Spine.Bone bone);
		
		delegate UnityEngine.Vector2 __GEN_DELEGATE90( Spine.Bone bone);
		
		delegate UnityEngine.Vector2 __GEN_DELEGATE91( Spine.Bone bone,  UnityEngine.Vector2 boneLocal);
		
		delegate UnityEngine.Vector3 __GEN_DELEGATE92( Spine.Bone bone,  UnityEngine.Transform spineGameObjectTransform);
		
		delegate UnityEngine.Vector3 __GEN_DELEGATE93( Spine.Bone bone,  UnityEngine.Transform spineGameObjectTransform,  float positionScale);
		
		delegate UnityEngine.Quaternion __GEN_DELEGATE94( Spine.Bone bone);
		
		delegate UnityEngine.Quaternion __GEN_DELEGATE95( Spine.Bone bone);
		
		delegate UnityEngine.Vector2 __GEN_DELEGATE96( Spine.Skeleton skeleton);
		
		delegate void __GEN_DELEGATE97( Spine.Bone bone, out  float ia, out  float ib, out  float ic, out  float id);
		
		delegate UnityEngine.Vector2 __GEN_DELEGATE98( Spine.Bone bone,  UnityEngine.Vector2 worldPosition);
		
		delegate UnityEngine.Vector2 __GEN_DELEGATE99( Spine.Bone bone,  UnityEngine.Vector2 skeletonSpacePosition);
		
		delegate UnityEngine.Material __GEN_DELEGATE100( Spine.Attachment a);
		
		delegate UnityEngine.Vector2[] __GEN_DELEGATE101( Spine.VertexAttachment va,  Spine.Slot slot,  UnityEngine.Vector2[] buffer);
		
		delegate UnityEngine.Vector2[] __GEN_DELEGATE102( Spine.VertexAttachment a,  Spine.Slot slot,  UnityEngine.Vector2[] buffer);
		
		delegate UnityEngine.Vector3 __GEN_DELEGATE103( Spine.PointAttachment attachment,  Spine.Slot slot,  UnityEngine.Transform spineGameObjectTransform);
		
		delegate UnityEngine.Vector3 __GEN_DELEGATE104( Spine.PointAttachment attachment,  Spine.Bone bone,  UnityEngine.Transform spineGameObjectTransform);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE105( DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> t,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE106( DG.Tweening.Core.TweenerCore<UnityEngine.Vector2, UnityEngine.Vector2, DG.Tweening.Plugins.Options.VectorOptions> t,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE107( DG.Tweening.Core.TweenerCore<UnityEngine.Vector2, UnityEngine.Vector2, DG.Tweening.Plugins.Options.VectorOptions> t,  DG.Tweening.AxisConstraint axisConstraint,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE108( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions> t,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE109( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions> t,  DG.Tweening.AxisConstraint axisConstraint,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE110( DG.Tweening.Core.TweenerCore<UnityEngine.Vector4, UnityEngine.Vector4, DG.Tweening.Plugins.Options.VectorOptions> t,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE111( DG.Tweening.Core.TweenerCore<UnityEngine.Vector4, UnityEngine.Vector4, DG.Tweening.Plugins.Options.VectorOptions> t,  DG.Tweening.AxisConstraint axisConstraint,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE112( DG.Tweening.Core.TweenerCore<UnityEngine.Quaternion, UnityEngine.Vector3, DG.Tweening.Plugins.Options.QuaternionOptions> t,  bool useShortest360Route);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE113( DG.Tweening.Core.TweenerCore<UnityEngine.Color, UnityEngine.Color, DG.Tweening.Plugins.Options.ColorOptions> t,  bool alphaOnly);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE114( DG.Tweening.Core.TweenerCore<UnityEngine.Rect, UnityEngine.Rect, DG.Tweening.Plugins.Options.RectOptions> t,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE115( DG.Tweening.Core.TweenerCore<string, string, DG.Tweening.Plugins.Options.StringOptions> t,  bool richTextEnabled,  DG.Tweening.ScrambleMode scrambleMode,  string scrambleChars);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE116( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3[], DG.Tweening.Plugins.Options.Vector3ArrayOptions> t,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE117( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3[], DG.Tweening.Plugins.Options.Vector3ArrayOptions> t,  DG.Tweening.AxisConstraint axisConstraint,  bool snapping);
		
		delegate DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> __GEN_DELEGATE118( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> t,  DG.Tweening.AxisConstraint lockPosition,  DG.Tweening.AxisConstraint lockRotation);
		
		delegate DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> __GEN_DELEGATE119( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> t,  bool closePath,  DG.Tweening.AxisConstraint lockPosition,  DG.Tweening.AxisConstraint lockRotation);
		
		delegate DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> __GEN_DELEGATE120( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> t,  UnityEngine.Vector3 lookAtPosition,  System.Nullable<UnityEngine.Vector3> forwardDirection,  System.Nullable<UnityEngine.Vector3> up);
		
		delegate DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> __GEN_DELEGATE121( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> t,  UnityEngine.Transform lookAtTransform,  System.Nullable<UnityEngine.Vector3> forwardDirection,  System.Nullable<UnityEngine.Vector3> up);
		
		delegate DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> __GEN_DELEGATE122( DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions> t,  float lookAhead,  System.Nullable<UnityEngine.Vector3> forwardDirection,  System.Nullable<UnityEngine.Vector3> up);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE123( UnityEngine.UI.Graphic target,  UnityEngine.Color endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE124( UnityEngine.UI.Graphic target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE125( UnityEngine.UI.LayoutElement target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE126( UnityEngine.UI.LayoutElement target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE127( UnityEngine.UI.LayoutElement target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE128( UnityEngine.UI.Outline target,  UnityEngine.Color endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE129( UnityEngine.UI.Outline target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE130( UnityEngine.UI.Outline target,  UnityEngine.Vector2 endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE131( UnityEngine.RectTransform target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE132( UnityEngine.RectTransform target,  float endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE133( UnityEngine.RectTransform target,  float endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE134( UnityEngine.RectTransform target,  UnityEngine.Vector3 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE135( UnityEngine.RectTransform target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE136( UnityEngine.RectTransform target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE137( UnityEngine.RectTransform target,  UnityEngine.Vector2 endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE138( UnityEngine.RectTransform target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE139( UnityEngine.RectTransform target,  float endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE140( UnityEngine.RectTransform target,  UnityEngine.Vector2 endValue,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE141( UnityEngine.RectTransform target,  UnityEngine.Vector2 punch,  float duration,  int vibrato,  float elasticity,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE142( UnityEngine.RectTransform target,  float duration,  float strength,  int vibrato,  float randomness,  bool snapping,  bool fadeOut);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE143( UnityEngine.RectTransform target,  float duration,  UnityEngine.Vector2 strength,  int vibrato,  float randomness,  bool snapping,  bool fadeOut);
		
		delegate DG.Tweening.Sequence __GEN_DELEGATE144( UnityEngine.RectTransform target,  UnityEngine.Vector2 endValue,  float jumpPower,  int numJumps,  float duration,  bool snapping);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE145( UnityEngine.UI.Graphic target,  UnityEngine.Color endValue,  float duration);
		
		delegate DG.Tweening.Tweener __GEN_DELEGATE146( UnityEngine.Rigidbody target,  float duration,  System.Nullable<UnityEngine.Vector3> axis,  DG.Tweening.SpiralMode mode,  float speed,  float frequency,  float depth,  bool snapping);
		
		delegate DG.Tweening.Sequence __GEN_DELEGATE147( UnityEngine.Material target,  UnityEngine.Gradient gradient,  float duration);
		
		delegate DG.Tweening.Sequence __GEN_DELEGATE148( UnityEngine.Material target,  UnityEngine.Gradient gradient,  string property,  float duration);
		
	    static InternalGlobals()
		{
		    extensionMethodMap = new Dictionary<Type, IEnumerable<MethodInfo>>()
			{
			    
				{typeof(UnityEngine.Material), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE0(DG.Tweening.ShortcutExtensions.DOColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE1(DG.Tweening.ShortcutExtensions.DOColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE2(DG.Tweening.ShortcutExtensions.DOFade)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE3(DG.Tweening.ShortcutExtensions.DOFade)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE4(DG.Tweening.ShortcutExtensions.DOFloat)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE5(DG.Tweening.ShortcutExtensions.DOOffset)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE6(DG.Tweening.ShortcutExtensions.DOOffset)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE7(DG.Tweening.ShortcutExtensions.DOTiling)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE8(DG.Tweening.ShortcutExtensions.DOTiling)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE9(DG.Tweening.ShortcutExtensions.DOVector)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE21(DG.Tweening.ShortcutExtensions.DOBlendableColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE22(DG.Tweening.ShortcutExtensions.DOBlendableColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE23(DG.Tweening.ShortcutExtensions.DOComplete)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE24(DG.Tweening.ShortcutExtensions.DOKill)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE25(DG.Tweening.ShortcutExtensions.DOFlip)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE26(DG.Tweening.ShortcutExtensions.DOGoto)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE27(DG.Tweening.ShortcutExtensions.DOPause)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE28(DG.Tweening.ShortcutExtensions.DOPlay)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE29(DG.Tweening.ShortcutExtensions.DOPlayBackwards)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE30(DG.Tweening.ShortcutExtensions.DOPlayForward)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE31(DG.Tweening.ShortcutExtensions.DORestart)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE32(DG.Tweening.ShortcutExtensions.DORewind)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE33(DG.Tweening.ShortcutExtensions.DOSmoothRewind)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE34(DG.Tweening.ShortcutExtensions.DOTogglePause)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE147(DG.Tweening.ShortcutExtensions43.DOGradientColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE148(DG.Tweening.ShortcutExtensions43.DOGradientColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.Rigidbody), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE10(DG.Tweening.ShortcutExtensions.DOMove)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE11(DG.Tweening.ShortcutExtensions.DOMoveX)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE12(DG.Tweening.ShortcutExtensions.DOMoveY)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE13(DG.Tweening.ShortcutExtensions.DOMoveZ)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE14(DG.Tweening.ShortcutExtensions.DORotate)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE15(DG.Tweening.ShortcutExtensions.DOLookAt)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE16(DG.Tweening.ShortcutExtensions.DOJump)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE17(DG.Tweening.ShortcutExtensions.DOPath)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE18(DG.Tweening.ShortcutExtensions.DOLocalPath)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE146(DG.Tweening.ShortcutExtensionsPro.DOSpiral)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.TrailRenderer), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE19(DG.Tweening.ShortcutExtensions.DOResize)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE20(DG.Tweening.ShortcutExtensions.DOTime)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(System.IO.BinaryReader), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE35(Honor.Runtime.GameBinaryExtension.Read7BitEncodedInt32)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE37(Honor.Runtime.GameBinaryExtension.Read7BitEncodedUInt32)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE39(Honor.Runtime.GameBinaryExtension.Read7BitEncodedInt64)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE41(Honor.Runtime.GameBinaryExtension.Read7BitEncodedUInt64)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE43(Honor.Runtime.GameBinaryExtension.ReadEncryptedString)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(System.IO.BinaryWriter), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE36(Honor.Runtime.GameBinaryExtension.Write7BitEncodedInt32)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE38(Honor.Runtime.GameBinaryExtension.Write7BitEncodedUInt32)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE40(Honor.Runtime.GameBinaryExtension.Write7BitEncodedInt64)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE42(Honor.Runtime.GameBinaryExtension.Write7BitEncodedUInt64)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE44(Honor.Runtime.GameBinaryExtension.WriteEncryptedString)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(string), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE45(Honor.Runtime.GameExtension.UppercaseFirst)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE46(Honor.Runtime.GameExtension.RichTextLength)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE47(Honor.Runtime.GameExtension.ToTitleCase)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE48(Honor.Runtime.GameExtension.RemoveExtraSpaces)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE49(Honor.Runtime.GameExtension.TimeStringToFloat)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE50(Honor.Runtime.GameExtension.GetChineseNum)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(float), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE51(Honor.Runtime.GameExtension.FloatToTimeString)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.RectTransform), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE52(Honor.Runtime.GameExtensionForUnity.SetLeft)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE53(Honor.Runtime.GameExtensionForUnity.SetRight)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE54(Honor.Runtime.GameExtensionForUnity.SetTop)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE55(Honor.Runtime.GameExtensionForUnity.SetBottom)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE56(Honor.Runtime.GameExtensionForUnity.SetAnchoredPositionX3D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE57(Honor.Runtime.GameExtensionForUnity.SetAnchoredPositionY3D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE58(Honor.Runtime.GameExtensionForUnity.SetAnchoredPositionZ3D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE59(Honor.Runtime.GameExtensionForUnity.AddAnchoredPositionX3D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE60(Honor.Runtime.GameExtensionForUnity.AddAnchoredPositionY3D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE61(Honor.Runtime.GameExtensionForUnity.AddAnchoredPositionZ3D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE62(Honor.Runtime.GameExtensionForUnity.SetAnchoredPositionX)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE63(Honor.Runtime.GameExtensionForUnity.SetAnchoredPositionY)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE64(Honor.Runtime.GameExtensionForUnity.AddAnchoredPositionX)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE65(Honor.Runtime.GameExtensionForUnity.AddAnchoredPositionY)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE66(Honor.Runtime.GameExtensionForUnity.ForceRebuildLayoutImmediate)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE131(DG.Tweening.ShortcutExtensions46.DOAnchorPos)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE132(DG.Tweening.ShortcutExtensions46.DOAnchorPosX)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE133(DG.Tweening.ShortcutExtensions46.DOAnchorPosY)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE134(DG.Tweening.ShortcutExtensions46.DOAnchorPos3D)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE135(DG.Tweening.ShortcutExtensions46.DOAnchorMax)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE136(DG.Tweening.ShortcutExtensions46.DOAnchorMin)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE137(DG.Tweening.ShortcutExtensions46.DOPivot)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE138(DG.Tweening.ShortcutExtensions46.DOPivotX)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE139(DG.Tweening.ShortcutExtensions46.DOPivotY)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE140(DG.Tweening.ShortcutExtensions46.DOSizeDelta)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE141(DG.Tweening.ShortcutExtensions46.DOPunchAnchorPos)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE142(DG.Tweening.ShortcutExtensions46.DOShakeAnchorPos)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE143(DG.Tweening.ShortcutExtensions46.DOShakeAnchorPos)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE144(DG.Tweening.ShortcutExtensions46.DOJumpAnchorPos)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(byte), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE67(Honor.Runtime.Converter.ToHex)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(byte[]), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE68(Honor.Runtime.Converter.ToHex)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE69(Honor.Runtime.Converter.ToHex)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE70(Honor.Runtime.Converter.ToHex)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.Unity.ISpineComponent), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE71(Spine.Unity.ISpineComponentExtensions.IsNullOrDestroyed)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.Skeleton), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE72(Spine.Unity.SkeletonExtensions.GetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE77(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE78(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE85(Spine.Unity.SkeletonExtensions.SetLocalScale)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE96(Spine.Unity.SkeletonExtensions.GetLocalScale)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.RegionAttachment), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE73(Spine.Unity.SkeletonExtensions.GetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE81(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE82(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.MeshAttachment), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE74(Spine.Unity.SkeletonExtensions.GetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE83(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE84(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.Slot), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE75(Spine.Unity.SkeletonExtensions.GetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE76(Spine.Unity.SkeletonExtensions.GetColorTintBlack)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE79(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE80(Spine.Unity.SkeletonExtensions.SetColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.Bone), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE86(Spine.Unity.SkeletonExtensions.GetMatrix4x4)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE87(Spine.Unity.SkeletonExtensions.SetLocalPosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE88(Spine.Unity.SkeletonExtensions.SetLocalPosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE89(Spine.Unity.SkeletonExtensions.GetLocalPosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE90(Spine.Unity.SkeletonExtensions.GetSkeletonSpacePosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE91(Spine.Unity.SkeletonExtensions.GetSkeletonSpacePosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE92(Spine.Unity.SkeletonExtensions.GetWorldPosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE93(Spine.Unity.SkeletonExtensions.GetWorldPosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE94(Spine.Unity.SkeletonExtensions.GetQuaternion)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE95(Spine.Unity.SkeletonExtensions.GetLocalQuaternion)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE97(Spine.Unity.SkeletonExtensions.GetWorldToLocalMatrix)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE98(Spine.Unity.SkeletonExtensions.WorldToLocal)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE99(Spine.Unity.SkeletonExtensions.SetPositionSkeletonSpace)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.Attachment), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE100(Spine.Unity.SkeletonExtensions.GetMaterial)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.VertexAttachment), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE101(Spine.Unity.SkeletonExtensions.GetLocalVertices)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE102(Spine.Unity.SkeletonExtensions.GetWorldVertices)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(Spine.PointAttachment), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE103(Spine.Unity.SkeletonExtensions.GetWorldPosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE104(Spine.Unity.SkeletonExtensions.GetWorldPosition)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE105(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Vector2, UnityEngine.Vector2, DG.Tweening.Plugins.Options.VectorOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE106(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE107(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE108(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE109(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Vector4, UnityEngine.Vector4, DG.Tweening.Plugins.Options.VectorOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE110(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE111(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Quaternion, UnityEngine.Vector3, DG.Tweening.Plugins.Options.QuaternionOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE112(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Color, UnityEngine.Color, DG.Tweening.Plugins.Options.ColorOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE113(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Rect, UnityEngine.Rect, DG.Tweening.Plugins.Options.RectOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE114(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<string, string, DG.Tweening.Plugins.Options.StringOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE115(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3[], DG.Tweening.Plugins.Options.Vector3ArrayOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE116(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE117(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, DG.Tweening.Plugins.Core.PathCore.Path, DG.Tweening.Plugins.Options.PathOptions>), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE118(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE119(DG.Tweening.TweenSettingsExtensions.SetOptions)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE120(DG.Tweening.TweenSettingsExtensions.SetLookAt)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE121(DG.Tweening.TweenSettingsExtensions.SetLookAt)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE122(DG.Tweening.TweenSettingsExtensions.SetLookAt)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.UI.Graphic), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE123(DG.Tweening.ShortcutExtensions46.DOColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE124(DG.Tweening.ShortcutExtensions46.DOFade)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE145(DG.Tweening.ShortcutExtensions46.DOBlendableColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.UI.LayoutElement), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE125(DG.Tweening.ShortcutExtensions46.DOFlexibleSize)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE126(DG.Tweening.ShortcutExtensions46.DOMinSize)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE127(DG.Tweening.ShortcutExtensions46.DOPreferredSize)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
				{typeof(UnityEngine.UI.Outline), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE128(DG.Tweening.ShortcutExtensions46.DOColor)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE129(DG.Tweening.ShortcutExtensions46.DOFade)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				  new __GEN_DELEGATE130(DG.Tweening.ShortcutExtensions46.DOScale)
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
