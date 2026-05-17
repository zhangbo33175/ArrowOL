/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorGenConfig.cs
 * author:    云毅
 *  created:   2026
 * descrip:   Honor框架 XLua 绑定代码自动生成配置
 *            作用：配置 Lua 可调用的 C# 类型、过滤不需要生成的类型、配置黑白名单
 ***************************************************************/

using CSObjectWrapEditor;
using System;
using System.IO;
using System.Linq;
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

#region XLua 绑定代码自动生成配置
/// <summary>
/// XLua 绑定代码自动生成配置类（编辑器-only）
/// 功能：
/// 1. 自动扫描程序集，生成 Lua 可调用的 C# 绑定代码
/// 2. 配置白名单：Lua 能调用哪些 C# 类/方法
/// 3. 配置黑名单：屏蔽不安全/不需要的 API
/// 4. 自动检测并触发代码生成
/// </summary>
[InitializeOnLoad]
public static class HonorGenConfig
{
    #region 过滤配置
    /// <summary>
    /// 【过滤黑名单】
    /// 不希望 XLua 生成绑定的类型关键字（包含即过滤）
    /// 作用：减少生成代码体积、屏蔽编辑器/废弃/不安全API
    /// </summary>
    static List<string> exclude = new List<string>
    {
        "HideInInspector", 
        "ExecuteInEditMode",
        "AddComponentMenu", 
        "ContextMenu",
        "RequireComponent", 
        "DisallowMultipleComponent",
        "SerializeField", 
        "AssemblyIsEditorAssembly",
        "Attribute", 
        "Types",
        "UnitySurrogateSelector", 
        "TrackedReference",
        "TypeInferenceRules", 
        "FFTWindow",
        "RPC", 
        "Network", 
        "MasterServer",
        "BitStream", 
        "HostData",
        "ConnectionTesterStatus", 
        "GUI", 
        "EventType",
        "EventModifiers", 
        "FontStyle", 
        "TextAlignment",
        "TextEditor", 
        "TextEditorDblClickSnapping",
        "TextGenerator", 
        "TextClipping", 
        "Gizmos",
        "ADBannerView", 
        "ADInterstitialAd",
        "Android", 
        "Tizen", 
        "jvalue",
        "iPhone", 
        "iOS", 
        "Windows", 
        "CalendarIdentifier",
        "CalendarUnit", 
        "CalendarUnit",
        "ClusterInput",
        "FullScreenMovieControlMode",
        "FullScreenMovieScalingMode", 
        "Handheld",
        "LocalNotification", 
        "NotificationServices",
        "RemoteNotificationType", 
        "RemoteNotification",
        "SamsungTV", 
        "TextureCompressionQuality",
        "TouchScreenKeyboardType", 
        "TouchScreenKeyboard",
        "MovieTexture", 
        "UnityEngineInternal",
        "Terrain", 
        "Tree", 
        "SplatPrototype",
        "DetailPrototype", 
        "DetailRenderMode",
        "MeshSubsetCombineUtility", 
        "AOT", 
        "Social", 
        "Enumerator",
        "SendMouseEvents", 
        "Cursor", 
        "Flash", 
        "ActionScript",
        "OnRequestRebuild", 
        "Ping",
        "ShaderVariantCollection", 
        "SimpleJson.Reflection",
        "CoroutineTween", 
        "GraphicRebuildTracker",
        "Advertisements", 
        "UnityEditor", 
        "WSA",
        "EventProvider", 
        "Apple",
        "ClusterInput", 
        "Motion",
        "UnityEngine.UI.ReflectionMethodsCache", 
        "NativeLeakDetection",
        "NativeLeakDetectionMode", 
        "WWWAudioExtensions", 
        "UnityEngine.Experimental",

        // Spine 库过滤
        "Spine.Unity.MaterialChecks",
        "ExampleEditor",
        "Spine.Unity.BuildUtilities",
        "Spine.Unity.SkeletonDataCompatibility",

        // 2dxFX 编辑器过滤
        "_Editor",
        "Tilemap+SyncTile",

        // IAP 过滤
        "AmazonAppStoreStoreExtensions", 
        "AmazonApps", 
        "DemoInventory", 
        "FacebookStore", 
        "FakeAmazonExtensions",
        "FakeGooglePlayStoreExtensions", 
        "FakeSamsungAppsExtensions", 
        "FakeUDPExtension", 
        "MacAppStore",
        "SamsungApps", 
        "ThinkingAnalytics",

        // UI Extension 过滤
        "UISquircleEditor", 
        "UISquircle+UISquircleEditor",

        // Excel 过滤
        "ExcelDataReader",

#if !BT_ENABLE
        // BehaviorDesigner 过滤
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
    #endregion

    #region 静态构造
    /// <summary>
    /// 静态构造：编辑器启动时注册更新检查
    /// </summary>
    static HonorGenConfig()
    {
        EditorApplication.update += () => { CheckXLuaGen(); };
    }
    #endregion

    #region 自动生成检测
    /// <summary>
    /// 检查 XLua 绑定代码是否需要生成
    /// 若生成目录为空，则自动执行 GenAll()
    /// </summary>
    static void CheckXLuaGen()
    {
        if (Application.isPlaying || EditorApplication.isCompiling || EditorApplication.isUpdating)
        {
            return;
        }

        if (!Directory.Exists(GeneratorConfig.common_path) ||
            Directory.GetFiles(GeneratorConfig.common_path, "*").Length == 0)
        {
            Generator.GenAll();
            CompilationPipeline.RequestScriptCompilation();
        }
    }
    #endregion

    #region 类型过滤方法
    /// <summary>
    /// 类型过滤方法：判断是否需要排除
    /// </summary>
    /// <param name="type">待检测的类型</param>
    /// <returns>true：需要排除  false：保留</returns>
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
    #endregion

    #region 命名空间白名单
    /// <summary>
    /// 【命名空间白名单】
    /// 按命名空间批量开放：Lua 可以调用这些命名空间下的所有 C# 类
    /// </summary>
    [LuaCallCSharp]
    public static IEnumerable<Type> LuaCallCSharpFromNamespace
    {
        get
        {
            List<string> namespaces = new List<string>()
            {
                "Honor.Runtime", // 游戏框架
                "DG.Tweening", // DOTween
#if SPINE_ENABLE
                "Spine.Unity", // Spine
#endif
                "UnityEngine.Tilemaps", // 瓦片地图
                "UnityEngine.Timeline", // 时间线
            };

            // 程序集扫描：Unity 系统程序集
            var unityTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                where !(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder)
                from type in assembly.GetExportedTypes()
                where type.Namespace != null && namespaces.Contains(type.Namespace) && !isExcluded(type)
                      && type.BaseType != typeof(MulticastDelegate) && !type.IsInterface
                select type);

            // 程序集扫描：自定义代码 Assembly-CSharp
            string[] customAssemblys = new string[]
            {
                "Assembly-CSharp",
            };
            var customTypes = (from assembly in customAssemblys.Select(s => System.Reflection.Assembly.Load(s))
                from type in assembly.GetExportedTypes()
                where (!isExcluded(type)) && ((type.Namespace == null) || !type.Namespace.StartsWith("XLua")
                    && type.BaseType != typeof(MulticastDelegate) && !type.IsInterface)
                select type);

            return unityTypes.Concat(customTypes);
        }
    }
    #endregion

    #region 单个类型白名单
    /// <summary>
    /// 【单个类型白名单】
    /// 手动指定 Lua 能调用的 C# 类型（基础类、UI、动画、委托、列表等）
    /// </summary>
    [LuaCallCSharp] public static List<Type> LuaCallCSharp = new List<Type>()
    {
        // 基础结构
        typeof(System.Object),
        typeof(UnityEngine.Object),
        typeof(DateTime),
        typeof(Vector2), typeof(Vector3), typeof(Vector4),
        typeof(Quaternion), typeof(Color), typeof(Ray), typeof(Bounds),

        // 游戏核心
        typeof(GameObject), typeof(Component), typeof(Transform),
        typeof(Resources), typeof(TextAsset), typeof(Time), typeof(Mathf),

        // 动画
        typeof(AnimationCurve), typeof(Animator), typeof(AnimatorStateInfo),

        // UI
        typeof(Text), typeof(TextMeshProUGUI), typeof(Image), typeof(Button),
        typeof(ScrollRect), typeof(Toggle), typeof(Slider), typeof(Dropdown),
        typeof(InputField), typeof(Canvas), typeof(CanvasGroup),

        // 事件
        typeof(PointerEventData), typeof(BaseEventData),
        typeof(UnityEngine.Events.UnityAction),
        typeof(UnityEngine.Events.UnityEvent),

        // 委托
        typeof(Action), typeof(Action<bool>), typeof(Action<int>), typeof(Action<string>),
        typeof(Func<bool>),

        // 集合
        typeof(List<int>), typeof(List<string>), typeof(List<GameObject>),
        typeof(List<LuaTable>), typeof(SortedDictionary<string, string>),

        // 框架委托
        typeof(PrefabLoadOverCallback),
        typeof(AssetLoadOverCallback),
        typeof(SceneLoadOverCallback),
        typeof(UILoadOverCallback),

        // DOTween
        typeof(DG.Tweening.Tween),
        typeof(DG.Tweening.Tweener),

        // 协程
        typeof(IEnumerator),
        typeof(WaitForSeconds),
        typeof(WaitForEndOfFrame),
    };
    #endregion

    #region C# 调用 Lua 委托配置
    /// <summary>
    /// 【C# 调用 Lua 委托】
    /// 配置 C# 可以调用的 Lua 函数类型（与白名单共用）
    /// </summary>
    [CSharpCallLua] public static List<Type> CSharpCallLua = LuaCallCSharp;
    #endregion

    #region 终极黑名单
    /// <summary>
    /// 【终极黑名单】
    /// 精确屏蔽某个类的某个方法（即使在白名单也无法调用）
    /// 用于屏蔽：过时、崩溃、编辑器、平台相关 API
    /// </summary>
    [BlackList] public static List<List<string>> BlackList = new List<List<string>>()
    {
        // 系统/Unity 废弃/危险API
        new List<string>() { "System.Xml.XmlNodeList", "ItemOf" },
        new List<string>() { "UnityEngine.WWW", "movie" },
        new List<string>() { "UnityEngine.CanvasRenderer", "onRequestRebuild" },
        new List<string>() { "UnityEngine.MonoBehaviour", "runInEditMode" },

        // 音频/手柄
        new List<string>() { "UnityEngine.AudioSource", "PlayOnGamepad", "System.Int32" },

        // Spine
        new List<string>() { "Spine.Unity.SkeletonRenderer", "Start" },

        // Tilemap 编辑器API
        new List<string>() { "UnityEngine.Tilemaps.Tilemap", "GetEditorPreviewTile", "UnityEngine.Vector3Int" },

        // UIExtension
        new List<string>() { "UnityEngine.UI.Extensions.UISquircle+UISquircleEditor", "UISquircle" },
    };
    #endregion
}
#endregion