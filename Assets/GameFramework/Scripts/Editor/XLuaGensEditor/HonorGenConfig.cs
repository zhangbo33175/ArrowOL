/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorGenConfig.cs
 * descrip:   Honor端XLua自动配置代码生成器
 ***************************************************************/
using CSObjectWrapEditor;
using System;
using System.IO;
using System.Linq;
#if EASY_TOUCH_ENABLE
#endif
using TMPro;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using XLua;
using static UnityEngine.EventSystems.EventTrigger;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using Honor.Runtime;

[InitializeOnLoad]
public static class HonorGenConfig
{
    /// <summary>
    /// 不希望生成的名字空间/类型在这里声明即可
    /// 注意：这里填写的是需要过滤的类型的关键字，不一定非要全称！
    /// </summary>
    static List<string> exclude = new List<string> {
        "HideInInspector", "ExecuteInEditMode",
        "AddComponentMenu", "ContextMenu",
        "RequireComponent", "DisallowMultipleComponent",
        "SerializeField", "AssemblyIsEditorAssembly",
        "Attribute", "Types",
        "UnitySurrogateSelector", "TrackedReference",
        "TypeInferenceRules", "FFTWindow",
        "RPC", "Network", "MasterServer",
        "BitStream", "HostData",
        "ConnectionTesterStatus", "GUI", "EventType",
        "EventModifiers", "FontStyle", "TextAlignment",
        "TextEditor", "TextEditorDblClickSnapping",
        "TextGenerator", "TextClipping", "Gizmos",
        "ADBannerView", "ADInterstitialAd",
        "Android", "Tizen", "jvalue",
        "iPhone", "iOS", "Windows", "CalendarIdentifier",
        "CalendarUnit", "CalendarUnit",
        "ClusterInput", "FullScreenMovieControlMode",
        "FullScreenMovieScalingMode", "Handheld",
        "LocalNotification", "NotificationServices",
        "RemoteNotificationType", "RemoteNotification",
        "SamsungTV", "TextureCompressionQuality",
        "TouchScreenKeyboardType", "TouchScreenKeyboard",
        "MovieTexture", "UnityEngineInternal",
        "Terrain", "Tree", "SplatPrototype",
        "DetailPrototype", "DetailRenderMode",
        "MeshSubsetCombineUtility", "AOT", "Social", "Enumerator",
        "SendMouseEvents", "Cursor", "Flash", "ActionScript",
        "OnRequestRebuild", "Ping",
        "ShaderVariantCollection", "SimpleJson.Reflection",
        "CoroutineTween", "GraphicRebuildTracker",
        "Advertisements", "UnityEditor", "WSA",
        "EventProvider", "Apple",
        "ClusterInput", "Motion",
        "UnityEngine.UI.ReflectionMethodsCache", "NativeLeakDetection",
        "NativeLeakDetectionMode", "WWWAudioExtensions", "UnityEngine.Experimental",
        // Spine库中的MaterialChecks需要过滤
        "Spine.Unity.MaterialChecks",
        "ExampleEditor",
        "Spine.Unity.BuildUtilities",
        "Spine.Unity.SkeletonDataCompatibility",
        // _2dxFX库中需要过滤的Editor类型的关键字
        "_Editor",
        "Tilemap+SyncTile",
        // Unity Purchasing中需要过滤的关键字
        "AmazonAppStoreStoreExtensions","AmazonApps","DemoInventory","FacebookStore","FakeAmazonExtensions",
        "FakeGooglePlayStoreExtensions","FakeSamsungAppsExtensions","FakeUDPExtension","MacAppStore",
        "SamsungApps","ThinkingAnalytics",
        // UI Extentsion
        "UISquircleEditor","UISquircle+UISquircleEditor",
        // Excel文件导出功能源码
        "ExcelDataReader",
#if !BT_ENABLE
        // BehaviorDesigner
        "BehaviorDesigner.Runtime",
#endif

        "UnityEngine.GamepadSpeakerOutputType",
     
        "AnimatedTileEditor",
       
        "MikuLuaProfiler",

        "AlmostEngine.Screenshot",
        "AlmostEngine",
        
        "AIHelpCore",
        "SafeAreaUtils"
    };

    /// <summary>
    /// 构造方法
    /// </summary>
    static HonorGenConfig()
    {
        EditorApplication.update += () => { CheckXLuaGen(); };
    }

    /// <summary>
    /// 检查XLua生成
    /// </summary>
    static void CheckXLuaGen()
    {
        if (Application.isPlaying || EditorApplication.isCompiling || EditorApplication.isUpdating)
        {
            return;
        }

        if (!Directory.Exists(GeneratorConfig.common_path) || Directory.GetFiles(GeneratorConfig.common_path, "*").Length == 0)
        {
            Generator.GenAll();
            CompilationPipeline.RequestScriptCompilation();
        }
    }

    /// <summary>
    /// 筛选器：判断是否为不生成类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    static bool isExcluded(Type type)
    {
        var fullName = type.FullName;
        for (int i = 0; i < exclude.Count; i++)
        {
            if (fullName.Contains(exclude[i]))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等
    /// </summary>
    [LuaCallCSharp]
    public static IEnumerable<Type> LuaCallCSharpFromNamespace
    {
        get
        {
            // 在这里添加第三方库的名字空间(这里不需要添加UnityEngine，避免生成的库过大，如果有个别unityEngine下的类找不到，可以缺什么加什么，添加到UnityEngineGenConfig的LuaCallCSharp中)
            List<string> namespaces = new List<string>()
            {
                "Honor.Runtime",
                "DG.Tweening",
#if SPINE_ENABLE
                "Spine.Unity",
#endif
                "UnityEngine.Tilemaps",
                "UnityEngine.Timeline",
            };
            var unityTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                              where !(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder)
                              from type in assembly.GetExportedTypes()
                              where type.Namespace != null && namespaces.Contains(type.Namespace) && !isExcluded(type)
                                      && type.BaseType != typeof(MulticastDelegate) && !type.IsInterface //&& !type.IsEnum
                              select type);

            // 这里包括了所有自定义C#内容，也包括没有提供名字空间的第三方库
            // 所以无论是自定义C#内容还是第三方库内容，所有不需要生成的类型都必须统一填写到exclude中进行过滤！
            string[] customAssemblys = new string[] {
                "Assembly-CSharp",
            };
            var customTypes = (from assembly in customAssemblys.Select(s => System.Reflection.Assembly.Load(s))
                               from type in assembly.GetExportedTypes()
                               where (!isExcluded(type)) && ((type.Namespace == null) || !type.Namespace.StartsWith("XLua")
                                       && type.BaseType != typeof(MulticastDelegate) && !type.IsInterface /*&& !type.IsEnum*/)
                               select type);
            return unityTypes.Concat(customTypes);
        }
    }

    /// <summary>
    /// Lua调用C#的类型声明
    /// </summary>
    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>()
    {
        typeof(System.Object),
        typeof(UnityEngine.Object),
        typeof(DateTime),
        typeof(TimeSpan),
        typeof(TimeZone),
        typeof(Rect),
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(Quaternion),
        typeof(Color),
        typeof(Color32),
        typeof(Ray),
        typeof(Bounds),
        typeof(Ray2D),
        typeof(Time),
        typeof(GameObject),
        typeof(Component),
        typeof(Behaviour),
        typeof(Transform),
        typeof(Resources),
        typeof(TextAsset),
        typeof(Keyframe),
        typeof(AnimationCurve),
        typeof(AnimationClip),
        typeof(Animator),
        typeof(AnimatorStateInfo),
        typeof(AnimatorUpdateMode),
        typeof(RuntimeAnimatorController),
        typeof(MonoBehaviour),
        typeof(ParticleSystem),
        typeof(SkinnedMeshRenderer),
        typeof(Renderer),
        typeof(Light),
        typeof(Mathf),
        typeof(Debug),
        typeof(SpriteRenderer),
        typeof(Text),
        typeof(TextMeshProUGUI),
        typeof(Image),
        typeof(Button),
        typeof(Scrollbar),
        typeof(ScrollRect),
        typeof(Toggle),
        typeof(Slider),
        typeof(Dropdown),
        typeof(InputField),
        typeof(Canvas),
        typeof(Camera),
        typeof(Button.ButtonClickedEvent),
        typeof(InputField.OnChangeEvent),
        typeof(InputField.SubmitEvent),
        typeof(Dropdown.DropdownEvent),
        typeof(Toggle.ToggleEvent),
        typeof(Slider.SliderEvent),
        typeof(PointerEventData),
        typeof(BaseEventData),
        typeof(EventArgs),
        typeof(AudioSource),
        typeof(AudioClip),
        typeof(SortingGroup),
        typeof(CanvasGroup),
        typeof(LineRenderer),
        typeof(RectTransformUtility),
        typeof(MaterialPropertyBlock),
        typeof(Rigidbody2D),
        typeof(Collider2D),
        typeof(CircleCollider2D),
        typeof(EdgeCollider2D),
        typeof(ColorUtility),

        typeof(UnityEngine.Events.UnityAction),
        typeof(UnityEngine.Events.UnityAction<string>),
        typeof(UnityEngine.Events.UnityAction<int>),
        typeof(UnityEngine.Events.UnityAction<bool>),
        typeof(UnityEngine.Events.UnityAction<float>),
        typeof(UnityEngine.Events.UnityAction<Vector2>),
        typeof(System.Collections.IEnumerator),
        typeof(UnityEngine.Events.UnityEvent),
        typeof(UnityEngine.Events.UnityEvent<string>),
        typeof(UnityEngine.Events.UnityEvent<int>),
        typeof(UnityEngine.Events.UnityEvent<bool>),
        typeof(UnityEngine.Events.UnityEvent<float>),
        typeof(UnityEngine.Events.UnityEvent<Vector2>),

        typeof(Action),
        typeof(Action<bool>),
        typeof(Action<int>),
        typeof(Action<string>),
        typeof(Action<bool, int, string>),
        typeof(Action<StateMachine<ProcedureComponent>>),
        //todo
        //typeof(Action<PermissionState>),
        
        typeof(Action<GameObject, int, int>),
        typeof(Action<LuaBehaviour, int>),
        typeof(Action<LuaBehaviour, int, int>),
        typeof(Action<LuaBehaviour, int, int, bool>),
        typeof(Action<PointerEventData>),

        typeof(Action<string, string, string, string>),
        typeof(Action<string, int>),
        typeof(Action<bool, int>),
        typeof(Action<LuaTable>),
        typeof(List<int>),
        typeof(List<string>),
        typeof(List<bool>),
        typeof(List<float>),
        typeof(List<GameObject>),
        typeof(List<PrefabInstanceGOBehaviour>),
        typeof(List<UIFlagBehaviour>),
        typeof(List<LuaTable>),
        typeof(List<Entry>),
        typeof(SortedDictionary<string, string>),
        typeof(PrefabLoadOverCallback),
        typeof(AssetLoadOverCallback),
        typeof(AssetUnloadOverCallback),
        typeof(LuaCreateLuaClassFromCSEventDelegate),
        typeof(LuaRelateLocalizationTableDataFromCSEventDelegate),
        typeof(LuaCreateProcedureLuaClassFromCSEventDelegate),
        typeof(LuaApplicationPauseFromCSEventDelegate),
        typeof(LuaApplicationQuitFromCSEventDelegate),
        typeof(LuaKeysUpFromCSEventDelegate),
        
        typeof(LuaSignInWithAppleCSEventDelegate),
        typeof(LuaSignInWithAppleStateCSEventDelegate),

        typeof(LuaLocalizingCSEventDelegate),

        typeof(LuaReceiveEventCSEventDelegate),
        typeof(OnListViewGetItemByIndex),
        typeof(OnListViewStart),
        typeof(OnListViewSnapItemFinished),
        typeof(OnListViewSnapNearestChanged),
        typeof(UILoadOverCallback),
        typeof(SceneLoadOverCallback),
        typeof(SceneUnloadOverCallback),
        typeof(ReloadDelegate),
        typeof(DG.Tweening.ShortcutExtensions),
        typeof(DG.Tweening.Tween),
        typeof(DG.Tweening.Tweener),
        typeof(DG.Tweening.Core.DOGetter<int>),
        typeof(DG.Tweening.Core.DOSetter<int>),
        typeof(DG.Tweening.Core.DOGetter<float>),
        typeof(DG.Tweening.Core.DOSetter<float>),
        typeof(DG.Tweening.Core.DOGetter<Color>),
        typeof(DG.Tweening.Core.DOSetter<Color>),
        typeof(DG.Tweening.Core.DOGetter<Vector2>),
        typeof(DG.Tweening.Core.DOSetter<Vector2>),
        typeof(DG.Tweening.Core.DOGetter<Vector3>),
        typeof(DG.Tweening.Core.DOSetter<Vector3>),
        typeof(WaitForSeconds),
        typeof(WaitForEndOfFrame),
        typeof(WaitForFixedUpdate),
        typeof(WaitUntil),
        typeof(Func<bool>),
        typeof(Uri),
    };

    /// <summary>
    /// C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    /// </summary>
    [CSharpCallLua] public static List<Type> CSharpCallLua = LuaCallCSharp;

    /// <summary>
    /// 黑名单
    /// 注意：每一条配置项的内容顺序："含有名字空间的类名","方法名 or 属性名","含有名字空间的方法参数1类型",...,"含有名字空间的方法参数N类型"
    /// </summary>
    [BlackList]
    public static List<List<string>> BlackList = new List<List<string>>()
    {
        new List<string>(){"System.Xml.XmlNodeList", "ItemOf"},
        new List<string>(){"UnityEngine.WWW", "movie"},
        new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
        new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
        new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
        new List<string>(){"UnityEngine.Light", "areaSize"},
        new List<string>(){"UnityEngine.Light", "lightmapBakeType"},
        new List<string>(){"UnityEngine.WWW", "MovieTexture"},
        new List<string>(){"UnityEngine.WWW", "GetMovieTexture"},
        new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
        new List<string>(){"UnityEngine.Application", "ExternalEval"},
        new List<string>(){"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
        new List<string>(){"UnityEngine.Component", "networkView"},  //4.6.2 not support
        new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
        new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
        new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
        new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
        new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
        new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
        new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"},
        new List<string>(){"UnityEngine.AudioSettings", "GetSpatializerPluginNames"},
        new List<string>(){"UnityEngine.AudioSettings", "SetSpatializerPluginName"},
        new List<string>(){"UnityEngine.Caching", "SetNoBackupFlag"},
        new List<string>(){"UnityEngine.Caching", "ResetNoBackupFlag"},
        new List<string>(){"UnityEngine.DrivenRectTransformTracker", "StopRecordingUndo"},
        new List<string>(){"UnityEngine.LightProbeGroup", "dering"},
        new List<string>(){"UnityEngine.Input", "IsJoystickPreconfigured", "System.String"},
        new List<string>(){"UnityEngine.Light", "SetLightDirty"},
        new List<string>(){"UnityEngine.Light", "shadowAngle"},
        new List<string>(){"UnityEngine.Light", "shadowRadius"},
        new List<string>(){"UnityEngine.UI.Text", "OnRebuildRequested"},
        new List<string>(){"UnityEngine.UI.Graphic", "OnRebuildRequested"},
        new List<string>(){"UnityEngine.MeshRenderer", "scaleInLightmap"},
        new List<string>(){"UnityEngine.MeshRenderer", "receiveGI"},
        new List<string>(){"UnityEngine.MeshRenderer", "stitchLightmapSeams"},
        new List<string>(){"UnityEngine.ParticleSystemRenderer", "supportsMeshInstancing"},
        new List<string>(){"UnityEngine.Texture", "imageContentsHash"},
        new List<string>(){"Spine.Unity.BlendModeMaterials", "TransferSettingsFrom", "Spine.Unity.BlendModeMaterialsAsset"},
        new List<string>(){"Spine.Unity.BlendModeMaterials", "UpdateBlendmodeMaterialsRequiredState", "Spine.SkeletonData"},
        new List<string>(){"Spine.Unity.SkeletonDataCompatibility", "GetCompatibilityProblemInfo", "Spine.Unity.SkeletonDataCompatibility+VersionInfo"},
        new List<string>(){"Spine.Unity.SkeletonDataCompatibility", "DisplayCompatibilityProblem", "System.String", "UnityEngine.TextAsset"},
        new List<string>(){"Spine.Unity.SkeletonDataCompatibility", "GetVersionInfo", "UnityEngine.TextAsset", "System.Boolean", "System.String"},
        new List<string>(){"Spine.Unity.SkeletonDataCompatibility", "IsJsonFile", "UnityEngine.TextAsset"},
        new List<string>(){"Spine.Unity.SkeletonRenderer", "Start"},
        new List<string>(){"Spine.Unity.SkeletonRenderer", "EditorSkipSkinSync"},
        new List<string>(){"Spine.Unity.SkeletonRenderer", "EditorUpdateMeshFilterHideFlags"},
        new List<string>(){"Spine.Unity.SkeletonRenderer", "fixPrefabOverrideViaMeshFilter"},
        new List<string>(){"Spine.Unity.SkeletonRenderer", "fixPrefabOverrideViaMeshFilterGlobal"},
        new List<string>(){"Spine.Unity.SkeletonDataAsset", "errorIfSkeletonFileNullGlobal"},
        new List<string>(){"Spine.Unity.SpineSpriteAtlasAsset", "UpdateByStartingEditorPlayMode"},
        new List<string>(){"Spine.Unity.SpineSpriteAtlasAsset", "AnySpriteAtlasNeedsRegionsLoaded"},
        new List<string>(){"Spine.Unity.SpineSpriteAtlasAsset", "UpdateWhenEditorPlayModeStarted"},
        new List<string>(){"Spine.Unity.SpineSpriteAtlasAsset", "LoadRegionsInEditorPlayMode"},
        new List<string>(){"Spine.Unity.SpineSpriteAtlasAsset", "AccessPackedTextureEditor", "UnityEngine.U2D.SpriteAtlas"},
        new List<string>(){"Spine.Unity.SpineSpriteAtlasAsset", "RegionsNeedLoading"},
        new List<string>(){"Spine.Unity.SkeletonGraphic", "ResetRectToReferenceRectSize"},
        new List<string>(){"Spine.Unity.SkeletonGraphic", "GetReferenceRectSize"},
        new List<string>(){"Spine.Unity.SkeletonGraphic", "EditReferenceRect"},
        new List<string>(){"Spine.Unity.SkeletonGraphic", "RectTransformSize"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "GetEditorPreviewTile","UnityEngine.Vector3Int"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "SetEditorPreviewTile", "UnityEngine.Vector3Int", "UnityEngine.Tilemaps.TileBase"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "HasEditorPreviewTile", "UnityEngine.Vector3Int"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "GetEditorPreviewSprite", "UnityEngine.Vector3Int"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "GetEditorPreviewTransformMatrix", "UnityEngine.Vector3Int"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "SetEditorPreviewTransformMatrix", "UnityEngine.Vector3Int","UnityEngine.Matrix4x4"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "GetEditorPreviewColor", "UnityEngine.Vector3Int"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "SetEditorPreviewColor", "UnityEngine.Vector3Int","UnityEngine.Color"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "GetEditorPreviewTileFlags", "UnityEngine.Vector3Int"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "EditorPreviewFloodFill", "UnityEngine.Vector3Int","UnityEngine.Tilemaps.TileBase"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "EditorPreviewBoxFill", "UnityEngine.Vector3Int","UnityEngine.Object","System.Int32","System.Int32","System.Int32","System.Int32"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "ClearAllEditorPreviewTiles"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "editorPreviewOrigin"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "editorPreviewSize"},
        new List<string>(){"UnityEngine.Tilemaps.Tilemap", "tilemapTileChanged"},
        new List<string>(){"ThinkingAnalytics.ThinkingAnalyticsAPI", "onPostProcessBuild", "UnityEditor.BuildTarget", "System.String" },
        new List<string>(){"UnityEngine.Timeline.AnimationPlayableAsset", "LiveLink"},
#if UIEXTENSION_ENABLE
        new List<string>(){"UnityEngine.UI.Extensions.UISquircle+UISquircleEditor", "UISquircle"},
        new List<string>(){"UnityEngine.UI.Extensions.CUIBezierCurve", "EDITOR_ControlPoints"},
        new List<string>(){"UnityEngine.UI.Extensions.CUIGraphic", "EDITOR_RefCurvesControlRatioPoints"},
        new List<string>(){"UnityEngine.UI.Extensions.CUIGraphic", "EDITOR_RefCurves"},
        new List<string>(){"UnityEngine.UI.Extensions.ShineEffect", "OnRebuildRequested"},
#endif
        new List<string>(){"UnityEngine.AudioSource", "PlayOnGamepad", "System.Int32"},
        new List<string>(){"UnityEngine.AudioSource", "DisableGamepadOutput"},
        new List<string>(){"UnityEngine.AudioSource", "SetGamepadSpeakerMixLevel", "System.Int32", "System.Int32"},
        new List<string>(){"UnityEngine.AudioSource", "SetGamepadSpeakerMixLevelDefault", "System.Int32"},
        new List<string>(){"UnityEngine.AudioSource", "SetGamepadSpeakerRestrictedAudio", "System.Int32", "System.Boolean"},
        new List<string>(){"UnityEngine.AudioSource", "GamepadSpeakerSupportsOutputType", "UnityEngine.GamepadSpeakerOutputType"},
        new List<string>(){"UnityEngine.AudioSource", "gamepadSpeakerOutputType"},
    };

}
