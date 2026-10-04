/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PackWindow.cs
 * author:    Honor.Editor
 * created:   2026
 * descrip:   Honor 打包工具主窗口（Tab 窗口）
 *            Tab0 = AB资源打包：平台下拉 / 输出目录展示 / 增量与Lua加密开关 /
 *                  打AB·打Lua·打Res·打全平台AB·清空输出·打开输出目录 / 步骤勾选
 *            Tab1 = 玩家包打包：版本号·BundleCode·开发包·应用名·包名·输出根目录·
 *                  场景列表·Keystore 四字段 / 打APK·打PC·打iOS·一键全平台
 *            本窗口只负责交互与参数持久化，所有打包动作均经由
 *            BuildOrderHandler.Inst 分发（打包逻辑由并行任务实现）
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Honor.Editor.BuildPackage;

namespace Honor.Editor.BuildTool
{
    //=========================================================================
    // 打包工具主窗口：平台选择 + 步骤勾选 + 进度 + 日志
    //=========================================================================
    /// <summary>
    /// Honor 打包工具主窗口
    /// 说明：
    /// 1. 顶部 Toolbar 两个 Tab（AB资源打包 / 玩家包打包）；
    /// 2. 所有打包参数读写 <see cref="BuildSettingsStore"/>（EditorPrefs 持久化），改完即 Save；
    /// 3. 步骤勾选直接 new <see cref="BuildOrder"/>{ Name = 步骤中文名 }，
    ///    其 bEnable 读写 EditorPrefs("honor_build_order_" + Name)，与流水线共享同一勾选状态；
    /// 4. 日志经 <see cref="BuildLog.OnLog"/> 事件接收，自动追加并滚底，上限 500 条。
    /// </summary>
    public class PackWindow : EditorWindow
    {
        #region 单例访问
        /// <summary>
        /// 当前打开的窗口实例（OnEnable 赋值 / OnDisable 清空）
        /// </summary>
        public static PackWindow Inst { get; private set; }
        #endregion

        #region 常量
        /// <summary>
        /// Tab 名称（0=AB资源打包，1=玩家包打包，2=Lua可视化，3=AB打包工具）
        /// </summary>
        private static readonly string[] s_TabNames = { "AB资源打包", "玩家包打包", "Lua可视化", "AB打包工具" };

        /// <summary>
        /// 本窗口允许选择的打包目标平台（仅 Android / iOS / PC，见用户需求）
        /// </summary>
        private static readonly BuildTarget[] s_SupportedTargets =
        {
            BuildTarget.Android,
            BuildTarget.iOS,
            BuildTarget.StandaloneWindows64,
        };

        /// <summary>
        /// 与 <see cref="s_SupportedTargets"/> 一一对应的下拉显示名
        /// 说明：顺序必须与 s_SupportedTargets 完全一致，用于受限下拉的索引映射
        /// </summary>
        private static readonly string[] s_SupportedTargetLabels =
        {
            "Android",
            "iOS",
            "PC (Windows 64)",
        };

        /// <summary>
        /// 日志区最大保留条数（超出后从头截断）
        /// </summary>
        private const int MaxLogLines = 500;
        #endregion

        #region 步骤勾选条目（与流水线共享持久化键）
        /// <summary>
        /// 步骤勾选条目：OrderType 用于组装 BuildCustom 订单；
        /// Order 为临时构造的 BuildOrder 实例，其 bEnable 与流水线共享 EditorPrefs
        /// </summary>
        private sealed class StepEntry
        {
            /// <summary>对应订单类型</summary>
            public OrderType Type;

            /// <summary>临时订单实例（Name 即持久化键后缀，与流水线一致）</summary>
            public BuildOrder Order;
        }

        /// <summary>
        /// 步骤勾选列表（OnEnable 时构造一次）
        /// 注意：下方每个 Name 必须与流水线 BuildOrderHandler 构造的 BuildOrder.Name 完全一致，
        ///       才能共享 EditorPrefs("honor_build_order_" + Name) 的勾选状态
        /// </summary>
        private readonly List<StepEntry> m_Steps = new List<StepEntry>();
        #endregion

        #region 窗口状态
        /// <summary>当前 Tab 下标（0=AB资源打包，1=玩家包打包）</summary>
        private int m_CurTab;

        /// <summary>Tab 内容滚动位置</summary>
        private Vector2 m_TabScroll;

        /// <summary>日志区滚动位置</summary>
        private Vector2 m_LogScroll;

        /// <summary>日志缓存（含时间戳前缀）</summary>
        private readonly List<string> m_Logs = new List<string>();

        /// <summary>最近一条日志（状态栏展示）</summary>
        private string m_LastLog = "空闲";

        /// <summary>是否正在打包（同步执行期间置位）</summary>
        private bool m_IsBuilding;

        /// <summary>进度条数值（忙碌 0.5 / 空闲或完成 1）</summary>
        private float m_Progress = 1f;

        /// <summary>进度条文案</summary>
        private string m_ProgressLabel = "空闲";
        #endregion

        #region Lua 可视化状态
        /// <summary>
        /// Lua 可视化单行条目：资源路径 + 归属 bundle + 类别 + 体积
        /// </summary>
        private sealed class LuaVizEntry
        {
            /// <summary>资源相对路径（Assets 开头，正斜杠）</summary>
            public string AssetPath;

            /// <summary>归属 bundle 名</summary>
            public string BundleName;

            /// <summary>是否为配置资源（Config 目录下 .json/.bytes）</summary>
            public bool IsConfig;

            /// <summary>文件字节数</summary>
            public long SizeBytes;
        }

        /// <summary>Lua 可视化条目缓存（由 <see cref="RefreshLuaViz"/> 填充）</summary>
        private readonly List<LuaVizEntry> m_LuaVizEntries = new List<LuaVizEntry>();

        /// <summary>是否已做首次收集</summary>
        private bool m_LuaVizInitialized;

        /// <summary>Lua 面板滚动位置</summary>
        private Vector2 m_LuaVizScroll;

        /// <summary>Lua 面板搜索关键字（按资源路径过滤）</summary>
        private string m_LuaVizSearch = string.Empty;

        /// <summary>脚本分组折叠开关</summary>
        private bool m_LuaVizFoldScripts = true;

        /// <summary>配置分组折叠开关</summary>
        private bool m_LuaVizFoldConfigs = true;

        /// <summary>统计：lua 脚本数</summary>
        private int m_LuaVizScriptCount;

        /// <summary>统计：配置资源数</summary>
        private int m_LuaVizConfigCount;

        /// <summary>统计：bundle 数</summary>
        private int m_LuaVizBundleCount;

        /// <summary>统计：总体积（字节）</summary>
        private long m_LuaVizTotalBytes;
        #endregion

        #region 窗口打开与生命周期
        /// <summary>
        /// 打开打包窗口（菜单与外部调用统一入口）
        /// </summary>
        /// <returns>窗口实例</returns>
        public static PackWindow Open()
        {
            PackWindow win = GetWindow<PackWindow>("Honor 打包工具");
            win.minSize = new Vector2(760, 520);
            win.titleContent = new GUIContent("Honor 打包工具");
            return win;
        }

        /// <summary>
        /// OnEnable：登记单例、初始化步骤勾选、订阅日志事件
        /// </summary>
        private void OnEnable()
        {
            Inst = this;
            minSize = new Vector2(760, 520);
            EnsureSteps();

            BuildLog.OnLog -= OnLogAppend;
            BuildLog.OnLog += OnLogAppend;
        }

        /// <summary>
        /// OnDisable：退订日志事件、清空单例
        /// </summary>
        private void OnDisable()
        {
            BuildLog.OnLog -= OnLogAppend;
            if (Inst == this) Inst = null;
        }
        #endregion

        #region 步骤勾选初始化
        /// <summary>
        /// 构造步骤勾选列表（仅一次）
        /// Name 即 BuildOrder.bEnable 的 EditorPrefs 键后缀("honor_build_order_"+Name)，
        /// 必须与流水线具体订单类构造函数里的 Name 逐字一致，窗口勾选才能与流水线共享状态：
        ///   BuildOrderPackageRes  = "打Res资源AB"
        ///   BuildOrderPackageLua  = "打Lua资源AB"
        ///   BuildOrderPackageAll  = "打全部AB(res+lua)"
        ///   BuildOrderPackageAPK  = "打APK包"
        ///   BuildOrderPackagePC   = "打PC包"
        ///   BuildOrderPackageIOS  = "打iOS包"
        /// （以上均已与并行任务落地的订单类逐字核对）
        /// </summary>
        private void EnsureSteps()
        {
            if (m_Steps.Count > 0) return;

            AddStep(OrderType.PackageRes, "打Res资源AB");
            AddStep(OrderType.PackageLua, "打Lua资源AB");
            AddStep(OrderType.PackageAll, "打全部AB(res+lua)");
            AddStep(OrderType.PackageAPK, "打APK包");
            AddStep(OrderType.PackagePC, "打PC包");
            AddStep(OrderType.PackageIOS, "打iOS包");
        }

        /// <summary>
        /// 追加一个步骤勾选条目
        /// </summary>
        private void AddStep(OrderType type, string name)
        {
            m_Steps.Add(new StepEntry { Type = type, Order = new BuildOrder { Name = name } });
        }
        #endregion

        #region 日志接收
        /// <summary>
        /// BuildLog.OnLog 回调：追加时间戳前缀，截断上限，自动滚到底部
        /// </summary>
        /// <param name="msg">日志内容</param>
        private void OnLogAppend(string msg)
        {
            if (msg == null) msg = string.Empty;

            m_Logs.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + msg);
            if (m_Logs.Count > MaxLogLines)
                m_Logs.RemoveRange(0, m_Logs.Count - MaxLogLines);

            m_LastLog = msg;
            m_LogScroll.y = float.MaxValue; // 自动滚底
            Repaint();
        }
        #endregion

        #region OnGUI 主流程（异常捕获防窗口卡死）
        /// <summary>
        /// OnGUI：整体 try/catch 包裹，绘制异常只记日志不崩窗口
        /// </summary>
        private void OnGUI()
        {
            try
            {
                DrawWindow();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                BuildLog.Log("[窗口异常] " + e.Message);
            }
        }

        /// <summary>
        /// 绘制窗口主体：Tab -> 内容 -> 进度条 -> 状态栏 -> 日志区
        /// </summary>
        private void DrawWindow()
        {
            // 顶部 Tab
            m_CurTab = GUILayout.Toolbar(m_CurTab, s_TabNames, GUILayout.Height(24));

            // Tab 内容区
            using (var scroll = new EditorGUILayout.ScrollViewScope(m_TabScroll))
            {
                m_TabScroll = scroll.scrollPosition;

                if (m_CurTab == 0) DrawTabAB();
                else if (m_CurTab == 1) DrawTabPlayer();
                else if (m_CurTab == 2) DrawTabLuaViz();
                else DrawTabBuildPackage();
            }

            EditorGUILayout.Space();

            // 进度条
            Rect progressRect = EditorGUILayout.GetControlRect(false, 22f);
            EditorGUI.ProgressBar(progressRect, m_IsBuilding ? 0.5f : m_Progress, m_ProgressLabel);

            // 最近日志状态行
            EditorGUILayout.LabelField(
                new GUIContent("最近日志", "最近一条 BuildLog 事件"),
                new GUIContent(m_LastLog),
                EditorStyles.wordWrappedLabel);

            // 日志区
            using (var logScroll = new EditorGUILayout.ScrollViewScope(m_LogScroll, GUILayout.MinHeight(120f)))
            {
                m_LogScroll = logScroll.scrollPosition;
                for (int i = 0; i < m_Logs.Count; i++)
                {
                    EditorGUILayout.LabelField(m_Logs[i], EditorStyles.wordWrappedLabel);
                }
            }
        }
        #endregion

        #region Tab0：AB资源打包
        /// <summary>
        /// 绘制 Tab0：打包设置（平台/输出/开关）/ 打包动作 / 打包步骤，按分组框规范布局
        /// </summary>
        private void DrawTabAB()
        {
            GUILayout.Label("AB 资源打包", EditorStyles.boldLabel);
            GUILayout.Space(4);

            // 输出目录（只读展示；提升到方法作用域，供打包动作/步骤区块共用）
            string outRoot = BuildPackage_Define.GetBuildOutputFullRoot(BuildSettingsStore.CurrentPlatform);

            // ================= 打包设置 =================
            DrawSectionHeader("打包设置");
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                // 目标平台受限下拉（仅 Android / iOS / PC，避免全量 EnumPopup 列出无关平台）
                int curIdx = Array.IndexOf(s_SupportedTargets, BuildSettingsStore.CurrentPlatform);
                if (curIdx < 0) curIdx = 0; // 持久化的平台不在支持列表（如旧值 WebGL）时回退到 Android
                int newIdx = EditorGUILayout.Popup(
                    new GUIContent("目标平台", "选择AB打包目标平台；仅支持 Android / iOS / PC(Windows64)"),
                    curIdx,
                    s_SupportedTargetLabels);
                BuildTarget picked = s_SupportedTargets[newIdx];
                if (picked != BuildSettingsStore.CurrentPlatform)
                {
                    BuildSettingsStore.CurrentPlatform = picked;
                    BuildSettingsStore.Save();
                    BuildLog.Log("目标平台切换为：" + picked);
                }

                // 输出目录展示（只读）
                EditorGUILayout.LabelField(
                    new GUIContent("输出目录", "当前平台AB输出目录（只读）"),
                    new GUIContent(outRoot),
                    EditorStyles.wordWrappedLabel);

                EditorGUILayout.Space(4);
                // 开关并排
                using (new EditorGUILayout.HorizontalScope())
                {
                    DrawToggle("增量打包", "无内容变更则跳过打包",
                        v => BuildSettingsStore.Incremental = v, () => BuildSettingsStore.Incremental);
                    DrawToggle("Lua加密", "预留功能：需运行时 LuacMode 解密配合，默认关闭",
                        v => BuildSettingsStore.LuaEncrypt = v, () => BuildSettingsStore.LuaEncrypt);
                }
            }

            // ================= 打包动作 =================
            EditorGUILayout.Space(6);
            DrawSectionHeader("打包动作");
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (Button("打包全部AB", "按当前选定平台打包全部 AssetBundle"))
                        RunBuild("打包全部AB", () => BuildOrderHandler.Inst.BuildAB());
                    if (Button("单独打包LUA", "仅执行 Lua 打包"))
                        RunBuild("单独打包LUA", () => BuildOrderHandler.Inst.BuildLua());
                    if (Button("单独打包资源", "仅执行资源打包"))
                        RunBuild("单独打包资源", () => BuildOrderHandler.Inst.BuildRes());
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (Button("打包全平台AB", "对所有支持平台依次打AB"))
                        RunBuild("打包全平台AB", () => BuildOrderHandler.Inst.BuildAllABs());
                    if (Button("清空输出", "删除当前平台AB输出目录"))
                    {
                        // 直接按选定平台解析输出目录并删除（与 BuildPackage_Menu.ClearOutput 同一套目录约定）
                        if (Directory.Exists(outRoot))
                        {
                            Directory.Delete(outRoot, true);
                            AssetDatabase.Refresh();
                        }
                        BuildLog.Log("已清空输出目录：" + outRoot);
                    }
                    if (Button("打开输出目录", "在系统文件管理器中打开输出目录"))
                    {
                        if (Directory.Exists(outRoot)) EditorUtility.RevealInFinder(outRoot);
                        else EditorUtility.DisplayDialog("打开输出目录", "目录不存在：\n" + outRoot, "知道了");
                    }
                }
            }

            // ================= 打包步骤 =================
            EditorGUILayout.Space(6);
            DrawSectionHeader("打包步骤（勾选状态与流水线共享）");
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                // 步骤勾选：两列网格，更紧凑规范
                int half = (m_Steps.Count + 1) / 2;
                for (int i = 0; i < half; i++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        DrawStepToggle(m_Steps[i]);
                        int j = i + half;
                        if (j < m_Steps.Count) DrawStepToggle(m_Steps[j]);
                    }
                }

                EditorGUILayout.Space(4);
                if (Button("按勾选步骤执行", "收集勾选步骤的 OrderType，自动补 Start 在最前，调用 BuildCustom 执行"))
                {
                    List<OrderType> orders = new List<OrderType> { OrderType.Start };
                    int count = 0;
                    foreach (StepEntry step in m_Steps)
                    {
                        if (step.Order.bEnable) { orders.Add(step.Type); count++; }
                    }
                    BuildLog.Log("按勾选步骤执行，共 " + count + " 步");
                    RunBuild("按勾选步骤执行", () => BuildOrderHandler.Inst.BuildCustom(orders.ToArray()));
                }
            }
        }

        /// <summary>
        /// 绘制分组标题（加粗标题）
        /// </summary>
        /// <param name="title">分组标题文本</param>
        private void DrawSectionHeader(string title)
        {
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
        }

        /// <summary>
        /// 绘制单个步骤勾选项（提示：状态持久化在 EditorPrefs，与流水线共享）
        /// </summary>
        /// <param name="step">步骤条目</param>
        private void DrawStepToggle(StepEntry step)
        {
            bool v = EditorGUILayout.Toggle(
                new GUIContent(step.Order.Name, "勾选后随【按勾选步骤执行】一并执行；状态持久化在 EditorPrefs，与流水线共享"),
                step.Order.bEnable);
            if (v != step.Order.bEnable) step.Order.bEnable = v;
        }
        #endregion

        #region Tab1：玩家包打包
        /// <summary>
        /// 绘制 Tab1：版本/包名/输出/场景/Keystore 字段 + 平台打包按钮
        /// </summary>
        private void DrawTabPlayer()
        {
            GUILayout.Label("玩家包打包（APK / PC / iOS）", EditorStyles.boldLabel);

            BuildSettingsStore.AppVersion = TextField(
                "版本号", "Bundle 版本号字符串，如 1.0.0", BuildSettingsStore.AppVersion);

            BuildSettingsStore.BundleVersionCode = EditorGUILayout.IntField(
                new GUIContent("BundleCode", "内部版本号（整数，每次发布递增）"),
                BuildSettingsStore.BundleVersionCode);
            if (GUI.changed) BuildSettingsStore.Save();

            BuildSettingsStore.DevelopmentBuild = EditorGUILayout.Toggle(
                new GUIContent("开发包", "Development Build，包含调试信息与脚本调试支持"),
                BuildSettingsStore.DevelopmentBuild);
            if (GUI.changed) BuildSettingsStore.Save();

            BuildSettingsStore.AppName = TextField(
                "应用名", "打包后显示的应用名", BuildSettingsStore.AppName);

            BuildSettingsStore.PackageName = TextField(
                "包名", "应用包名，如 com.xxx.game", BuildSettingsStore.PackageName);

            // 输出根目录（带浏览按钮）
            using (new EditorGUILayout.HorizontalScope())
            {
                BuildSettingsStore.OutRoot = EditorGUILayout.TextField(
                    new GUIContent("输出根目录", "玩家包输出根目录"),
                    BuildSettingsStore.OutRoot);
                if (GUILayout.Button("浏览", GUILayout.Width(60)))
                {
                    string start = string.IsNullOrEmpty(BuildSettingsStore.OutRoot) ? "." : BuildSettingsStore.OutRoot;
                    string picked = EditorUtility.SaveFolderPanel("选择输出根目录", start, "");
                    if (!string.IsNullOrEmpty(picked))
                    {
                        BuildSettingsStore.OutRoot = picked;
                        BuildSettingsStore.Save();
                    }
                }
            }
            if (GUI.changed) BuildSettingsStore.Save();

            BuildSettingsStore.Scenes = TextField(
                "场景列表", "默认 Assets/Scenes/GameApp.unity，多个场景用分号(;)分隔",
                BuildSettingsStore.Scenes);

            EditorGUILayout.Space();
            GUILayout.Label("Keystore 签名配置（Android）", EditorStyles.boldLabel);

            BuildSettingsStore.KeystorePath = TextField(
                "Keystore路径", ".keystore / .jks 签名文件路径", BuildSettingsStore.KeystorePath);
            BuildSettingsStore.KeystorePass = PasswordField(
                "Keystore密码", "Keystore 库密码", BuildSettingsStore.KeystorePass);
            BuildSettingsStore.KeyAliasName = TextField(
                "别名", "签名别名 Alias 名称", BuildSettingsStore.KeyAliasName);
            BuildSettingsStore.KeyAliasPass = PasswordField(
                "别名密码", "签名别名对应的密码", BuildSettingsStore.KeyAliasPass);

            EditorGUILayout.Space();
            GUILayout.Label("打包动作", EditorStyles.boldLabel);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (Button("打APK", "构建 Android APK"))
                    RunBuild("打APK", () => BuildOrderHandler.Inst.BuildAPK());
                if (Button("打PC", "构建 Windows PC 包"))
                    RunBuild("打PC", () => BuildOrderHandler.Inst.BuildPC());
                if (Button("打iOS", "构建 iOS 工程/包（需 macOS 环境）"))
                    RunBuild("打iOS", () => BuildOrderHandler.Inst.BuildIOS());
            }

            if (Button("一键全平台", "依次执行全平台打包"))
                RunBuild("一键全平台", () => BuildOrderHandler.Inst.BuildAllPlatforms());
        }
        #endregion

        #region Tab2：Lua可视化
        /// <summary>
        /// 绘制 Tab2：展示 Assets/LuaScripts 下收集到的 lua 脚本/配置及其 bundle 映射
        /// 数据来源 <see cref="BuildPackage_Lua.CollectLuaBundleMap"/>，与打包收集完全一致；
        /// 提供刷新、搜索过滤与脚本/配置分组折叠查看
        /// </summary>
        private void DrawTabLuaViz()
        {
            GUILayout.Label("Lua 资源可视化", EditorStyles.boldLabel);

            // 首次进入自动收集
            if (!m_LuaVizInitialized)
                RefreshLuaViz();

            // --- 刷新 + 统计 ---
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("刷新", GUILayout.Width(70)))
                    RefreshLuaViz();

                GUILayout.Label(
                    $"脚本 {m_LuaVizScriptCount}   配置 {m_LuaVizConfigCount}   bundle {m_LuaVizBundleCount}   体积 {FormatBytes(m_LuaVizTotalBytes)}",
                    EditorStyles.boldLabel);
            }

            // --- 搜索过滤 ---
            string search = EditorGUILayout.TextField(
                new GUIContent("搜索", "按资源路径关键字过滤（留空显示全部）"),
                m_LuaVizSearch);
            if (search != m_LuaVizSearch)
                m_LuaVizSearch = search;

            // --- 分组滚动列表 ---
            using (var scroll = new EditorGUILayout.ScrollViewScope(m_LuaVizScroll, GUILayout.Height(300)))
            {
                m_LuaVizScroll = scroll.scrollPosition;

                if (m_LuaVizEntries.Count == 0)
                {
                    EditorGUILayout.HelpBox("未收集到任何 Lua 资源。请确认 Assets/LuaScripts 目录存在，或点击【刷新】重扫。", MessageType.Warning);
                }
                else
                {
                    // 分组一：Lua 脚本（*.lua.txt，每文件一个 bundle）
                    m_LuaVizFoldScripts = EditorGUILayout.Foldout(
                        m_LuaVizFoldScripts, $"Lua 脚本（{CountGroup(m_LuaVizEntries, true, false)} 个，每文件一个 bundle）");
                    if (m_LuaVizFoldScripts)
                    {
                        foreach (LuaVizEntry e in m_LuaVizEntries)
                        {
                            if (e.IsConfig) continue;
                            if (!MatchSearch(e)) continue;
                            DrawLuaVizLine(e);
                        }
                    }

                    EditorGUILayout.Space(2);

                    // 分组二：配置资源（.json/.bytes，目录粒度 bundle）
                    m_LuaVizFoldConfigs = EditorGUILayout.Foldout(
                        m_LuaVizFoldConfigs, $"配置资源（{CountGroup(m_LuaVizEntries, false, true)} 个，目录粒度 bundle）");
                    if (m_LuaVizFoldConfigs)
                    {
                        foreach (LuaVizEntry e in m_LuaVizEntries)
                        {
                            if (!e.IsConfig) continue;
                            if (!MatchSearch(e)) continue;
                            DrawLuaVizLine(e);
                        }
                    }
                }
            }

            EditorGUILayout.HelpBox("数据来自 BuildPackage_Lua.CollectLuaBundleMap()，与打包收集一致；点击【刷新】重新扫描目录。", MessageType.Info);
        }

        /// <summary>
        /// 重新扫描 Assets/LuaScripts 并重建可视化条目与统计
        /// </summary>
        private void RefreshLuaViz()
        {
            m_LuaVizEntries.Clear();
            m_LuaVizScriptCount = 0;
            m_LuaVizConfigCount = 0;
            m_LuaVizTotalBytes = 0;

            Dictionary<string, List<string>> map = BuildPackage_Lua.CollectLuaBundleMap();

            foreach (KeyValuePair<string, List<string>> kv in map)
            {
                foreach (string assetPath in kv.Value)
                {
                    string normalized = assetPath.Replace('\\', '/');
                    bool isConfig = BuildPackage_Lua.IsConfigAsset(normalized);

                    long size = 0;
                    try
                    {
                        string full = Path.Combine(Application.dataPath, normalized.Substring("Assets/".Length));
                        size = new FileInfo(full).Length;
                    }
                    catch (Exception)
                    {
                        // 单个文件读取失败不阻断整体统计
                    }

                    m_LuaVizEntries.Add(new LuaVizEntry
                    {
                        AssetPath = normalized,
                        BundleName = kv.Key,
                        IsConfig = isConfig,
                        SizeBytes = size,
                    });

                    if (isConfig) m_LuaVizConfigCount++;
                    else m_LuaVizScriptCount++;
                    m_LuaVizTotalBytes += size;
                }
            }

            m_LuaVizBundleCount = map.Count;
            m_LuaVizInitialized = true;
        }

        /// <summary>
        /// 绘制一行 Lua 资源：左资源路径，右归属 bundle
        /// </summary>
        private static void DrawLuaVizLine(LuaVizEntry e)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(e.AssetPath, EditorStyles.wordWrappedLabel);
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField(e.BundleName, EditorStyles.miniLabel, GUILayout.Width(340));
            }
        }

        /// <summary>
        /// 判断条目是否命中当前搜索关键字（不区分大小写，空关键字恒为 true）
        /// </summary>
        private bool MatchSearch(LuaVizEntry e)
        {
            if (string.IsNullOrEmpty(m_LuaVizSearch))
                return true;
            return e.AssetPath.IndexOf(m_LuaVizSearch, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// 统计某分组的条目数
        /// </summary>
        /// <param name="entries">条目列表</param>
        /// <param name="wantScript">是否统计脚本分组</param>
        /// <param name="wantConfig">是否统计配置分组</param>
        /// <returns>条目数</returns>
        private static int CountGroup(List<LuaVizEntry> entries, bool wantScript, bool wantConfig)
        {
            int count = 0;
            foreach (LuaVizEntry e in entries)
            {
                if (wantScript && !e.IsConfig) count++;
                else if (wantConfig && e.IsConfig) count++;
            }
            return count;
        }

        /// <summary>
        /// 字节数格式化为人类可读字符串（B / KB / MB）
        /// </summary>
        /// <param name="bytes">字节数</param>
        /// <returns>格式化字符串</returns>
        private static string FormatBytes(long bytes)
        {
            if (bytes < 1024) return bytes + " B";
            if (bytes < 1024 * 1024) return (bytes / 1024.0).ToString("0.0") + " KB";
            return (bytes / (1024.0 * 1024.0)).ToString("0.00") + " MB";
        }
        #endregion

        #region Tab3：AB打包工具（嵌入 BuildPackage_EditorWindow 配置界面）
        /// <summary>
        /// 绘制 Tab3：复用 AB 打包工具窗口的打包目录配置界面
        /// （启用/源目录/粒度/后缀过滤/删除/新增 + 一键打包/清理输出/恢复默认）
        /// 说明：复用 <see cref="BuildPackage_EditorWindow.GetEmbedInstance"/> 的嵌入宿主，
        ///       使打包配置编辑状态与本窗口独立 Tab 共享，无需弹出独立窗口。
        /// </summary>
        private static void DrawTabBuildPackage()
        {
            BuildPackage_EditorWindow embed = BuildPackage_EditorWindow.GetEmbedInstance();
            embed.DrawEmbeddedContent();
        }
        #endregion

        #region 通用小控件
        /// <summary>
        /// 绘制带 Tooltip 的按钮
        /// </summary>
        private static bool Button(string text, string tip)
        {
            return GUILayout.Button(new GUIContent(text, tip));
        }

        /// <summary>
        /// 绘制开关：变化即写回 Store 并 Save
        /// </summary>
        private static void DrawToggle(string label, string tip, Action<bool> setter, Func<bool> getter)
        {
            bool v = EditorGUILayout.Toggle(new GUIContent(label, tip), getter());
            if (v != getter())
            {
                setter(v);
                BuildSettingsStore.Save();
            }
        }

        /// <summary>
        /// 绘制文本框：变化即写回 Store 并 Save
        /// </summary>
        private static string TextField(string label, string tip, string value)
        {
            string v = EditorGUILayout.TextField(new GUIContent(label, tip), value);
            if (v != value) BuildSettingsStore.Save();
            return v;
        }

        /// <summary>
        /// 绘制密码框：变化即写回 Store 并 Save
        /// </summary>
        private static string PasswordField(string label, string tip, string value)
        {
            string v = EditorGUILayout.PasswordField(new GUIContent(label, tip), value);
            if (v != value) BuildSettingsStore.Save();
            return v;
        }
        #endregion

        #region 打包执行包装（同步 + 进度标志）
        /// <summary>
        /// 同步执行一次打包动作：置忙碌标志 -> 调用 -> 复位
        /// 注意：本窗口不开后台线程，所有打包经 BuildOrderHandler 同步执行
        /// </summary>
        /// <param name="title">动作名（用于进度条与日志）</param>
        /// <param name="action">实际打包动作（经 BuildOrderHandler.Inst）</param>
        private void RunBuild(string title, Action action)
        {
            if (m_IsBuilding)
            {
                EditorUtility.DisplayDialog(title, "正在打包中，请等待本次打包完成后再操作。", "知道了");
                return;
            }

            BuildLog.Log("===== " + title + " 开始 =====");
            m_IsBuilding = true;
            m_Progress = 0.5f;
            m_ProgressLabel = title + "（进行中...）";
            try
            {
                action();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                BuildLog.Log("[异常] " + title + " 失败：" + e.Message);
            }
            finally
            {
                m_IsBuilding = false;
                m_Progress = 1f;
                m_ProgressLabel = title + "（完成）";
                BuildLog.Log("===== " + title + " 结束 =====");
                Repaint();
            }
        }
        #endregion
    }
}
