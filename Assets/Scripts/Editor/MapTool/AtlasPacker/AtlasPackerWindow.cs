/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AtlasPackerWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   Sprite Atlas 打包工具（UI Toolkit 版，仿 TexturePacker）
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.U2D;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// Sprite Atlas 打包编辑器窗口（UI Toolkit 版）
    /// 三栏布局：左侧设置 + 中间预览 + 右侧 Sprites 列表
    /// </summary>
    public class AtlasPackerWindow : EditorWindow
    {
        // ===== UI 元素引用 =====
        private Toggle m_ToggleForceSquare;
        private Toggle m_ToggleAllowRotation;
        private Toggle m_ToggleMultipack;
        private Toggle m_TogglePremultiply;
        private Toggle m_ToggleReduceBorder;
        private Toggle m_ToggleShapeOutlines;
        private Toggle m_ToggleAutoAlias;
        private Toggle m_ToggleTrimNames;
        private Toggle m_ToggleForceWord;
        private Toggle m_ToggleHeuristicMask;
        private DropdownField m_PopupMaxWidth;
        private DropdownField m_PopupMaxHeight;
        private DropdownField m_PopupFixedWidth;
        private DropdownField m_PopupFixedHeight;
        private IntegerField m_IntBorderPadding;
        private IntegerField m_IntShapePadding;
        private IntegerField m_IntInnerPadding;
        private IntegerField m_IntExtrude;
        private IntegerField m_IntDivisorX;
        private IntegerField m_IntDivisorY;
        private IntegerField m_IntTrimThreshold;
        private IntegerField m_IntDpi;
        private FloatField m_FloatScale;
        private Slider m_SliderPngOpt;
        private DropdownField m_PopupDataFormat;
        private DropdownField m_PopupTextureFormat;
        private DropdownField m_PopupImageFormat;
        private DropdownField m_PopupDithering;
        private DropdownField m_PopupSizeConstraints;
        private DropdownField m_PopupAlgorithm;
        private DropdownField m_PopupHeuristics;
        private DropdownField m_PopupPack;
        private DropdownField m_PopupTrimMode;
        private DropdownField m_PopupScaleMode;
        private TextField m_TxtDataFilename;
        private TextField m_TxtTextureFile;
        private TextField m_TxtTexturePath;
        private Button m_BtnDataBrowse;
        private Button m_BtnTexBrowse;
        // 上次发布/选择的输出目录（Assets 相对路径），用于"定位"时优先查找、发布时默认目录
        private string m_LastOutputDir = "Assets/Res/Atlas";
        private Label m_LblSpriteCount;
        private Label m_LblAtlasSize;
        private Label m_LblPreviewEmpty;
        private Label m_LblZoom;
        private Slider m_ZoomSlider;
        private FloatField m_ZoomField;
        private Image m_ImgPreview;
        private VisualElement m_PreviewImageContainer;
        private VisualElement m_SpritesList;

        // 选中状态（多选：集合保存所有选中的列表索引，主选中为最后点击项，用作 Shift 范围锚点）
        private readonly HashSet<int> m_SelectedIndices = new HashSet<int>();
        private int m_PrimarySelectedIndex = AtlasConstants.NoSelectionIndex;
        private readonly Dictionary<int, (VisualElement bg, VisualElement frame)> m_HighlightByIndex =
            new Dictionary<int, (VisualElement bg, VisualElement frame)>();
        private float m_ZoomScale = AtlasConstants.DefaultZoomScale;
        private ScrollView m_PreviewScroll;

        // 图集实际大小
        private int m_ActualAtlasWidth = AtlasConstants.DefaultAtlasWidth;
        private int m_ActualAtlasHeight = AtlasConstants.DefaultAtlasHeight;
        // 未缩放时的图集基础尺寸（用于同步 ScrollView 布局占位，配合 scale 缩放）
        private float m_BaseAtlasW = 1;
        private float m_BaseAtlasH = 1;
        // 适应窗口时视口尚未撑开的重试计数（每次生成预览前重置，避免异常小视口把缩放钉到下限）
        private int m_FitRetryCount = 0;
        private const int MaxFitRetries = 12;

        // 右键拖动状态
        private bool m_IsDragging = false;
        private Vector2 m_DragStartPos;
        private Vector2 m_ScrollStartPos;

        // ===== 数据 =====
        private string m_CurrentFolder = AtlasConstants.DefaultFolder;

        // ===== 独立功能模块 =====
        private TextureLoader m_TextureLoader = new TextureLoader();
        private AtlasLayoutCalculator m_LayoutCalculator = new AtlasLayoutCalculator();
        private AtlasProjectIO m_ProjectIO = new AtlasProjectIO();
        private AtlasExporter m_Exporter = new AtlasExporter();
        private SpriteListManager m_SpriteListManager = new SpriteListManager();
        private AtlasPreviewRenderer m_PreviewRenderer = new AtlasPreviewRenderer();
        private AtlasProjectManager m_ProjectManager = new AtlasProjectManager();
        private AtlasPublisher m_Publisher = new AtlasPublisher();
        private AtlasCliPacker m_CliPacker = new AtlasCliPacker();

        [MenuItem(AtlasConstants.MenuItemPath)]
        public static void ShowWindow()
        {
            var window = GetWindow<AtlasPackerWindow>(AtlasConstants.WindowTitle);
            // 默认宽屏尺寸，允许用户自由缩放
            window.position = new Rect(100, 100, AtlasConstants.WindowWidth, AtlasConstants.WindowHeight);
            window.minSize = new Vector2(AtlasConstants.WindowMinWidth, AtlasConstants.WindowMinHeight);
            window.Show();
        }


        /// <summary>
        /// 创建 GUI（UI Toolkit 方式）
        /// </summary>
        public void CreateGUI()
        {
            // 加载 UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(AtlasConstants.UxmlPath);

            if (visualTree == null)
            {
                rootVisualElement.Add(new Label("UXML 文件未找到，请确认路径"));
                return;
            }

            visualTree.CloneTree(rootVisualElement);

            // 加载 USS
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(AtlasConstants.UssPath);
            if (styleSheet != null)
            {
                rootVisualElement.styleSheets.Add(styleSheet);
            }

            BindUIElements();
            BindEvents();
            LoadToolbarIcons();
            InitModules();
        }

        /// <summary>
        /// 初始化独立功能模块
        /// </summary>
        private void InitModules()
        {
            // 纹理加载器：注入路径转换委托
            m_TextureLoader = new TextureLoader();

            // SpriteListManager：注入路径转换委托
            m_SpriteListManager.AssetToFullPath = AssetPathToFullPath;
            m_SpriteListManager.FullToAssetPath = FullPathToAssetPath;

            // 排布计算器：注入纹理加载委托
            m_LayoutCalculator = new AtlasLayoutCalculator
            {
                TextureLoader = (assetPath) => m_TextureLoader.LoadFromDisk(assetPath, AssetPathToFullPath)
            };

            // 导出器：注入纹理加载和路径转换委托
            m_Exporter = new AtlasExporter
            {
                TextureLoader = (fullPath) =>
                {
                    // 注意：这里传入的是全路径，直接加载
                    if (!File.Exists(fullPath)) return null;
                    byte[] bytes = File.ReadAllBytes(fullPath);
                    var tex = new Texture2D(2, 2);
                    tex.LoadImage(bytes);
                    return tex;
                },
                AssetPathToFullPath = AssetPathToFullPath
            };
        }

        /// <summary>
        /// 绑定 UI 元素引用
        /// </summary>
        private void BindUIElements()
        {
            m_ToggleForceSquare = rootVisualElement.Q<Toggle>(AtlasConstants.TogForceSquare);
            m_ToggleAllowRotation = rootVisualElement.Q<Toggle>(AtlasConstants.TogAllowRotation);
            m_ToggleMultipack = rootVisualElement.Q<Toggle>(AtlasConstants.TogMultipack);
            m_TogglePremultiply = rootVisualElement.Q<Toggle>(AtlasConstants.TogPremultiply);
            m_ToggleReduceBorder = rootVisualElement.Q<Toggle>(AtlasConstants.TogReduceBorder);
            m_ToggleShapeOutlines = rootVisualElement.Q<Toggle>(AtlasConstants.TogShapeOutlines);
            m_ToggleAutoAlias = rootVisualElement.Q<Toggle>(AtlasConstants.TogAutoAlias);
            m_ToggleTrimNames = rootVisualElement.Q<Toggle>(AtlasConstants.TogTrimNames);
            m_ToggleForceWord = rootVisualElement.Q<Toggle>(AtlasConstants.TogForceWord);
            m_ToggleHeuristicMask = rootVisualElement.Q<Toggle>(AtlasConstants.TogHeuristicMask);

            m_PopupMaxWidth = rootVisualElement.Q<DropdownField>(AtlasConstants.PopMaxWidth);
            m_PopupMaxHeight = rootVisualElement.Q<DropdownField>(AtlasConstants.PopMaxHeight);
            m_PopupFixedWidth = rootVisualElement.Q<DropdownField>(AtlasConstants.PopFixedWidth);
            m_PopupFixedHeight = rootVisualElement.Q<DropdownField>(AtlasConstants.PopFixedHeight);
            m_IntBorderPadding = rootVisualElement.Q<IntegerField>(AtlasConstants.IntBorderPadding);
            m_IntShapePadding = rootVisualElement.Q<IntegerField>(AtlasConstants.IntShapePadding);
            m_IntInnerPadding = rootVisualElement.Q<IntegerField>(AtlasConstants.IntInnerPadding);
            m_IntExtrude = rootVisualElement.Q<IntegerField>(AtlasConstants.IntExtrude);
            m_IntDivisorX = rootVisualElement.Q<IntegerField>(AtlasConstants.IntDivisorX);
            m_IntDivisorY = rootVisualElement.Q<IntegerField>(AtlasConstants.IntDivisorY);
            m_IntTrimThreshold = rootVisualElement.Q<IntegerField>(AtlasConstants.IntTrimThreshold);
            m_IntDpi = rootVisualElement.Q<IntegerField>(AtlasConstants.IntDpi);
            m_FloatScale = rootVisualElement.Q<FloatField>(AtlasConstants.FloatScale);
            m_SliderPngOpt = rootVisualElement.Q<Slider>(AtlasConstants.SliderPngOpt);

            m_PopupDataFormat = rootVisualElement.Q<DropdownField>(AtlasConstants.PopDataFormat);
            m_PopupTextureFormat = rootVisualElement.Q<DropdownField>(AtlasConstants.PopTextureFormat);
            m_PopupImageFormat = rootVisualElement.Q<DropdownField>(AtlasConstants.PopImageFormat);
            m_PopupDithering = rootVisualElement.Q<DropdownField>(AtlasConstants.PopDithering);
            m_PopupSizeConstraints = rootVisualElement.Q<DropdownField>(AtlasConstants.PopSizeConstraints);
            m_PopupAlgorithm = rootVisualElement.Q<DropdownField>(AtlasConstants.PopAlgorithm);
            m_PopupHeuristics = rootVisualElement.Q<DropdownField>(AtlasConstants.PopHeuristics);
            m_PopupPack = rootVisualElement.Q<DropdownField>(AtlasConstants.PopPack);
            m_PopupTrimMode = rootVisualElement.Q<DropdownField>(AtlasConstants.PopTrimMode);
            m_PopupScaleMode = rootVisualElement.Q<DropdownField>(AtlasConstants.PopScaleMode);
            // DropdownField 在 UXML 中 value 先于 choices 应用，非首项默认值会被回退到首项，这里显式兜底（对齐原版 Trim 默认）
            if (m_PopupTrimMode != null && m_PopupTrimMode.choices != null && m_PopupTrimMode.choices.Contains("Trim"))
            {
                m_PopupTrimMode.value = "Trim";
            }

            m_TxtDataFilename = rootVisualElement.Q<TextField>(AtlasConstants.TxtDataFilename);
            m_TxtTextureFile = rootVisualElement.Q<TextField>(AtlasConstants.TxtTextureFile);
            m_TxtTexturePath = rootVisualElement.Q<TextField>(AtlasConstants.TxtTexturePath);
            m_BtnDataBrowse = rootVisualElement.Q<Button>("btnDataBrowse");
            m_BtnTexBrowse = rootVisualElement.Q<Button>("btnTexBrowse");

            m_LblSpriteCount = rootVisualElement.Q<Label>(AtlasConstants.LblSpriteCount);
            m_LblAtlasSize = rootVisualElement.Q<Label>(AtlasConstants.LblAtlasSize);
            m_LblPreviewEmpty = rootVisualElement.Q<Label>(AtlasConstants.LblPreviewEmpty);
            m_ZoomSlider = rootVisualElement.Q<Slider>("zoomSlider");
            m_ZoomField = rootVisualElement.Q<FloatField>("zoomField");
            m_ImgPreview = rootVisualElement.Q<Image>(AtlasConstants.ImgPreview);
            m_PreviewImageContainer = rootVisualElement.Q<VisualElement>(AtlasConstants.PreviewContainer);
            m_SpritesList = rootVisualElement.Q<VisualElement>(AtlasConstants.SpritesList);

            // 把 UI 元素赋值给 SpriteListManager
            m_SpriteListManager.ListContainer = m_SpritesList;
            m_SpriteListManager.CountLabel = m_LblSpriteCount;
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        private void BindEvents()
        {
            // 工具栏按钮
            rootVisualElement.Q<Button>(AtlasConstants.BtnAddFolder).clicked += OnAddFolder;
            rootVisualElement.Q<Button>(AtlasConstants.BtnAddSprites).clicked += OnAddSprites;
            rootVisualElement.Q<Button>(AtlasConstants.BtnDelete).clicked += OnDelete;
            rootVisualElement.Q<Button>(AtlasConstants.BtnPublish).clicked += OnPublish;
            rootVisualElement.Q<Button>(AtlasConstants.BtnSave).clicked += OnSave;
            rootVisualElement.Q<Button>(AtlasConstants.BtnOpen).clicked += OnOpen;
            rootVisualElement.Q<Button>(AtlasConstants.BtnNew).clicked += OnNew;

            // 输出文件名：定位 / 替换
            if (m_BtnDataBrowse != null) m_BtnDataBrowse.clicked += () => BrowseOutputFile(m_TxtDataFilename, "txt");
            if (m_BtnTexBrowse != null) m_BtnTexBrowse.clicked += () => BrowseOutputFile(m_TxtTextureFile, "png");

            // 监听尺寸变化，自动刷新预览
            m_PopupMaxWidth.RegisterValueChangedCallback(_ => GeneratePreviewImage());
            m_PopupMaxHeight.RegisterValueChangedCallback(_ => GeneratePreviewImage());

            // 排布相关参数变更后自动刷新预览
            RegisterReloadCallback(m_PopupFixedWidth);
            RegisterReloadCallback(m_PopupFixedHeight);
            RegisterReloadCallback(m_PopupSizeConstraints);
            RegisterReloadCallback(m_ToggleForceSquare);
            RegisterReloadCallback(m_PopupAlgorithm);
            RegisterReloadCallback(m_PopupHeuristics);
            RegisterReloadCallback(m_PopupPack);
            RegisterReloadCallback(m_IntBorderPadding);
            RegisterReloadCallback(m_IntShapePadding);
            RegisterReloadCallback(m_IntInnerPadding);
            RegisterReloadCallback(m_IntExtrude);
            RegisterReloadCallback(m_IntDivisorX);
            RegisterReloadCallback(m_IntDivisorY);
            RegisterReloadCallback(m_ToggleForceWord);
            RegisterReloadCallback(m_ToggleAllowRotation);
            RegisterReloadCallback(m_ToggleAutoAlias);
            RegisterReloadCallback(m_PopupTrimMode);
            RegisterReloadCallback(m_IntTrimThreshold);

            // 鼠标滚轮缩放
            var previewScroll = rootVisualElement.Q<ScrollView>("preview-scroll");
            m_PreviewScroll = previewScroll;
            if (previewScroll != null)
            {
                // 隐藏滚动条
                previewScroll.showHorizontal = false;
                previewScroll.showVertical = false;

                // 让图集在预览区居中显示（用 USS flex 居中，不用 margin auto 避免挤压）
                // preview-area 在 USS 中已设 align-items:center; justify-content:center;

                previewScroll.RegisterCallback<WheelEvent>(OnPreviewWheel);
                // 右键拖动
                previewScroll.RegisterCallback<MouseDownEvent>(OnPreviewMouseDown);
                previewScroll.RegisterCallback<MouseMoveEvent>(OnPreviewMouseMove);
                previewScroll.RegisterCallback<MouseUpEvent>(OnPreviewMouseUp);
            }

            // 缩放按钮
            rootVisualElement.Q<Button>(AtlasConstants.BtnZoomIn).clicked += () => SetZoom(m_ZoomScale + 0.1f);
            rootVisualElement.Q<Button>(AtlasConstants.BtnZoomOut).clicked += () => SetZoom(m_ZoomScale - 0.1f);
            rootVisualElement.Q<Button>(AtlasConstants.BtnZoomFit).clicked += FitToWindow;
            rootVisualElement.Q<Button>("btnZoomReset").clicked += () => SetZoom(1.0f);

            // 缩放滑块和数字框
            if (m_ZoomSlider != null)
                m_ZoomSlider.RegisterValueChangedCallback(evt => SetZoom(evt.newValue));
            if (m_ZoomField != null)
                m_ZoomField.RegisterValueChangedCallback(evt => SetZoom(evt.newValue));
        }

        /// <summary>
        /// 给工具栏和缩放栏按钮加载图标
        /// </summary>
        private void LoadToolbarIcons()
        {
            void SetIcon(string buttonName, string fileName)
            {
                var btn = rootVisualElement.Q<Button>(buttonName);
                if (btn == null) return;
                // 直接用 Application.dataPath 拼接，避免路径转换问题
                string fullPath = System.IO.Path.Combine(
                    UnityEngine.Application.dataPath,
                    "Scripts/Editor/MapTool/AtlasPacker/Icons",
                    fileName);
                if (!System.IO.File.Exists(fullPath)) return;
                byte[] bytes = System.IO.File.ReadAllBytes(fullPath);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);
                tex.filterMode = FilterMode.Bilinear;

                // 保存按钮文字，清空 text（否则文字居中与图标重叠）
                string label = btn.text;
                btn.text = string.Empty;

                btn.Clear();
                btn.style.flexDirection = FlexDirection.Column;
                btn.style.alignItems = Align.Center;
                btn.style.justifyContent = Justify.Center;
                btn.style.paddingTop = 2;
                btn.style.paddingBottom = 2;
                btn.style.minWidth = 52;

                var icon = new Image { image = tex };
                icon.style.width = 20;
                icon.style.height = 20;
                icon.style.marginBottom = 1;
                icon.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                btn.Add(icon);

                var textLabel = new Label(label);
                textLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                textLabel.style.fontSize = 10;
                btn.Add(textLabel);
            }

            // 工具栏
            SetIcon(AtlasConstants.BtnNew, "icon_012_32x32.png");
            SetIcon(AtlasConstants.BtnOpen, "icon_020_32x32.png");
            SetIcon(AtlasConstants.BtnSave, "icon_003_32x32.png");
            SetIcon(AtlasConstants.BtnSaveDefaults, "icon_023_32x32.png");
            SetIcon(AtlasConstants.BtnAddSprites, "icon_015_32x32.png");
            SetIcon(AtlasConstants.BtnAddFolder, "icon_002_32x32.png");
            SetIcon(AtlasConstants.BtnDelete, "icon_009_32x32.png");
            SetIcon(AtlasConstants.BtnPublish, "icon_022_32x32.png");
            SetIcon(AtlasConstants.BtnPvr, "icon_005_32x32.png");

            // 缩放栏按钮：只加图标，不加文字
            void SetZoomIcon(string buttonName, string fileName)
            {
                var btn = rootVisualElement.Q<Button>(buttonName);
                if (btn == null) return;
                string fullPath = System.IO.Path.Combine(
                    UnityEngine.Application.dataPath,
                    "Scripts/Editor/MapTool/AtlasPacker/Icons",
                    fileName);
                if (!System.IO.File.Exists(fullPath)) return;
                byte[] bytes = System.IO.File.ReadAllBytes(fullPath);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);
                tex.filterMode = FilterMode.Bilinear;

                btn.text = string.Empty;
                btn.Clear();
                btn.style.flexDirection = FlexDirection.Row;
                btn.style.alignItems = Align.Center;
                btn.style.justifyContent = Justify.Center;
                btn.style.width = 24;
                btn.style.height = 22;
                btn.style.minWidth = 24;
                btn.style.minHeight = 22;

                var icon = new Image { image = tex };
                icon.style.width = 16;
                icon.style.height = 16;
                icon.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
                btn.Add(icon);
            }

            SetZoomIcon("btnZoomReset", "icon_001_16x16.png");
            SetZoomIcon(AtlasConstants.BtnZoomOut, "icon_014_16x16.png");
            SetZoomIcon(AtlasConstants.BtnZoomIn, "icon_011_16x16.png");
            SetZoomIcon(AtlasConstants.BtnZoomFit, "icon_019_16x16.png");
        }

        /// <summary>
        /// 设置缩放比例
        /// </summary>
        private void SetZoom(float scale)
        {
            m_ZoomScale = Mathf.Clamp(scale, 0.01f, 10.0f);
            if (m_ZoomField != null && !Mathf.Approximately(m_ZoomField.value, m_ZoomScale))
                m_ZoomField.SetValueWithoutNotify(m_ZoomScale);
            if (m_ZoomSlider != null && !Mathf.Approximately(m_ZoomSlider.value, m_ZoomScale))
                m_ZoomSlider.SetValueWithoutNotify(m_ZoomScale);
            // 没有图集内容时不调整容器尺寸，避免预览区被挤压
            if (m_PreviewImageContainer == null || m_BaseAtlasW <= 0 || m_BaseAtlasH <= 0) return;

            // 容器保持未缩放基础尺寸，用 transform 做缩放
            m_PreviewImageContainer.style.width = m_BaseAtlasW;
            m_PreviewImageContainer.style.height = m_BaseAtlasH;
            m_PreviewImageContainer.style.scale = new Scale(new Vector3(m_ZoomScale, m_ZoomScale, 1));

            if (m_PreviewScroll == null) return;

            var content = m_PreviewScroll.contentContainer;
            var vp = m_PreviewScroll.contentViewport;
            float viewW = vp.resolvedStyle.width;
            float viewH = vp.resolvedStyle.height;
            float visualW = m_BaseAtlasW * m_ZoomScale;
            float visualH = m_BaseAtlasH * m_ZoomScale;

            // 对齐官方 TexturePackerGUI：主画布固定贴预览区左上，不随缩放居中；
            // 内容小于视口时容器撑满视口并左上排列，内容大于视口时按视觉尺寸占位、左上排列并允许拖动滚动。
            bool fit = viewW > 1f && viewH > 1f && visualW <= viewW && visualH <= viewH;
            if (fit)
            {
                content.style.width = viewW;
                content.style.height = viewH;
                content.style.alignItems = Align.FlexStart;
                content.style.justifyContent = Justify.FlexStart;
                m_PreviewImageContainer.style.transformOrigin = new TransformOrigin(0, 0);
                m_PreviewScroll.scrollOffset = Vector2.zero;
            }
            else
            {
                content.style.width = visualW;
                content.style.height = visualH;
                content.style.alignItems = Align.Auto;
                content.style.justifyContent = Justify.FlexStart;
                m_PreviewImageContainer.style.transformOrigin = new TransformOrigin(0, 0);
            }
        }

        /// <summary>
        /// 适应窗口：按预览可视区与图集实际尺寸计算缩放，让整图一屏放下
        /// </summary>
        private void FitToWindow()
        {
            if (m_PreviewImageContainer == null || m_PreviewScroll == null) return;

            // 用未缩放的基础图集尺寸计算目标缩放
            float atlasW = m_BaseAtlasW;
            float atlasH = m_BaseAtlasH;
            var vp = m_PreviewScroll.contentViewport;
            float viewW = vp.resolvedStyle.width;
            float viewH = vp.resolvedStyle.height;

            // 视口/图集尚未完成布局时，resolvedStyle 可能返回 1~几十的中间小值（而非严格 0）；
            // 守卫阈值若只判 <=1 会漏过，导致 scale 被压到下限 0.01 并钉死。这里按 32px 判定就绪，
            // 算出的缩放明显异常偏小时同样视为未就绪，做有限次延迟重试。
            bool layoutReady = atlasW > 1f && atlasH > 1f && viewW > 32f && viewH > 32f;
            float scale = layoutReady ? Mathf.Min(viewW / atlasW, viewH / atlasH) : 0f;
            bool scaleReasonable = scale >= 0.02f;
            if ((!layoutReady || !scaleReasonable) && m_FitRetryCount < MaxFitRetries)
            {
                m_FitRetryCount++;
                m_PreviewScroll.schedule.Execute(FitToWindow).ExecuteLater(50);
                return;
            }

            m_FitRetryCount = 0;
            float activeScale = scaleReasonable ? scale : m_ZoomScale;
            SetZoom(activeScale);

            // 图集比视口小时居中显示
            float visualW = atlasW * activeScale;
            float visualH = atlasH * activeScale;
            if (visualW < viewW || visualH < viewH)
            {
                m_PreviewScroll.scrollOffset = new Vector2(
                    Mathf.Max(0, (visualW - viewW) / 2f),
                    Mathf.Max(0, (visualH - viewH) / 2f));
            }
        }

        /// <summary>
        /// 鼠标滚轮缩放
        /// </summary>
        private void OnPreviewWheel(WheelEvent evt)
        {
            SetZoom(m_ZoomScale + (evt.delta.y > 0 ? 0.05f : -0.05f));
            evt.StopPropagation();
        }

        /// <summary>
        /// 右键按下开始拖动
        /// </summary>
        private void OnPreviewMouseDown(MouseDownEvent evt)
        {
            if (evt.button == (int)MouseButton.RightMouse)
            {
                m_IsDragging = true;
                m_DragStartPos = evt.mousePosition;
                var scrollView = (ScrollView)evt.currentTarget;
                m_ScrollStartPos = new Vector2(scrollView.scrollOffset.x, scrollView.scrollOffset.y);
                evt.StopPropagation();
            }
        }

        /// <summary>
        /// 拖动中
        /// </summary>
        private void OnPreviewMouseMove(MouseMoveEvent evt)
        {
            if (m_IsDragging)
            {
                var scrollView = (ScrollView)evt.currentTarget;
                Vector2 delta = evt.mousePosition - m_DragStartPos;
                scrollView.scrollOffset = new Vector2(
                    m_ScrollStartPos.x - delta.x,
                    m_ScrollStartPos.y - delta.y
                );
                evt.StopPropagation();
            }
        }

        /// <summary>
        /// 右键松开结束拖动
        /// </summary>
        private void OnPreviewMouseUp(MouseUpEvent evt)
        {
            if (evt.button == (int)MouseButton.RightMouse)
            {
                m_IsDragging = false;
                evt.StopPropagation();
            }
        }

        #region 工具栏事件

        private void OnAddFolder()
        {
            string path = EditorUtility.OpenFolderPanel("选择图片文件夹", "Assets", "");
            if (!string.IsNullOrEmpty(path))
            {
                m_CurrentFolder = PathToAssetPath(path);
                LoadSpritesFromFolder(m_CurrentFolder);
            }
        }

        private void OnAddSprites()
        {
            string path = EditorUtility.OpenFilePanelWithFilters("选择图片", "", new[] { "Images", "png,jpg,jpeg" });
            if (!string.IsNullOrEmpty(path))
            {
                string assetPath = PathToAssetPath(path);
                AddSpriteItem(assetPath);
                UpdateSpriteCount();
                GeneratePreviewImage();
            }
        }

        private void OnDelete()
        {
            // 清除所有 sprite
            m_SpriteListManager.Items.Clear();
            m_SpritesList.Clear();
            UpdateSpriteCount();
            GeneratePreviewImage();
        }

        /// <summary>
        /// 删除指定索引的图片（委托给 SpriteListManager）
        /// </summary>
        private void RemoveSpriteAt(int index)
        {
            if (index < 0 || index >= m_SpriteListManager.Items.Count) return;

            // 委托给 SpriteListManager 删除数据
            m_SpriteListManager.RemoveAt(index);

            // 同步删除右侧列表项（UI 部分）
            if (index < m_SpritesList.Children().Count())
            {
                m_SpritesList.RemoveAt(index);
            }

            // 重置选中状态
            ClearSelection();

            UpdateSpriteCount();
            GeneratePreviewImage();
        }

        /// <summary>
        /// 在系统文件夹中显示（委托给 SpriteAtlasCreator）
        /// </summary>
        private void ShowInFolder(string assetPath)
        {
            SpriteAtlasCreator.ShowInFolder(assetPath);
        }

        private void OnPublish()
        {
            if (m_SpriteListManager.Items.Count == 0)
            {
                EditorUtility.DisplayDialog("提示", AtlasConstants.PleaseAddImagesFirst, "确定");
                return;
            }

            // 选择输出目录（默认上次使用的目录）
            string outputDir = EditorUtility.SaveFolderPanel("选择图集输出目录", m_LastOutputDir, "");
            if (string.IsNullOrEmpty(outputDir)) return;

            string outputAssetDir = FullPathToAssetPath(outputDir.Replace('\\', '/'));
            if (!string.IsNullOrEmpty(outputAssetDir)) m_LastOutputDir = outputAssetDir;

            DoPublish(outputDir, outputAssetDir);
        }

        /// <summary>
        /// 按给定目录生成并写入图集 PNG 与 JSON 描述文件。
        /// </summary>
        private void DoPublish(string outputDir, string outputAssetDir)
        {
            int maxW = int.Parse(m_PopupMaxWidth.value);
            int maxH = int.Parse(m_PopupMaxHeight.value);
            string dataFilename = string.IsNullOrEmpty(m_TxtDataFilename.value) ? "NewAtlas.txt" : m_TxtDataFilename.value;
            string textureFilename = string.IsNullOrEmpty(m_TxtTextureFile.value) ? "NewAtlasTex.png" : m_TxtTextureFile.value;
            string pngPath = Path.Combine(outputDir, textureFilename);
            string jsonPath = Path.Combine(outputDir, dataFilename);

            // 优先走 TexturePacker CLI 官方后端（产物与官方一致）；CLI 缺失或调用失败时回退自研后端。
            string cliLog = null;
            bool cliOk = false;
            if (m_CliPacker.IsAvailable)
            {
                cliOk = TryPublishViaCli(outputDir, textureFilename, dataFilename, out cliLog);
            }

            if (!cliOk)
            {
                // 自研兜底：生成图集纹理
                var atlasTex = GenerateAtlasTexture(maxW, maxH);
                if (atlasTex == null)
                {
                    EditorUtility.DisplayDialog("错误", "生成图集纹理失败", "确定");
                    return;
                }

                // 保存 PNG 文件：优先自建优化编码器（level 9 + 逐行滤镜选优）；失败兜底内置 EncodeToPNG
                byte[] pngBytes;
                try
                {
                    pngBytes = PngAtlasEncoder.Encode(atlasTex);
                }
                catch (Exception encoderEx)
                {
                    Debug.LogWarning($"[AtlasPacker] 自建 PNG 编码器失败，回退 EncodeToPNG：{encoderEx.Message}");
                    pngBytes = atlasTex.EncodeToPNG();
                }
                File.WriteAllBytes(pngPath, pngBytes);

                // 生成 JSON 描述文件（用实际大小）
                string jsonContent = GenerateJsonDescription(textureFilename, m_ActualAtlasWidth, m_ActualAtlasHeight);
                // 对齐官方 TexturePacker 产物：描述文件以 GBK(936) 写入，保证中文帧名不乱码
                File.WriteAllText(jsonPath, jsonContent, GetAtlasDescriptionEncoding());
            }

            AssetDatabase.Refresh();

            int packedCount = m_SpriteListManager.Items.Count - m_LayoutCalculator.ExcludedNames.Count;
            string backend = cliOk ? "TexturePacker CLI" : "自研后端";
            string fallbackNote = cliOk ? "" : $"\n（未使用 CLI：{cliLog}）";
            EditorUtility.DisplayDialog("打包完成",
                $"图集导出完成！\n后端: {backend}\n\nPNG: {textureFilename}\nJSON: {dataFilename}\n图片数: {packedCount}" +
                (m_LayoutCalculator.ExcludedNames.Count > 0 ? $"\n跳过超尺寸: {m_LayoutCalculator.ExcludedNames.Count} 张" : "") +
                $"\n路径: {outputAssetDir}{fallbackNote}",
                "确定");
        }

        /// <summary>
        /// 通过 TexturePacker CLI 官方后端导出图集。
        /// </summary>
        /// <returns>CLI 是否成功产出 PNG 与描述文件</returns>
        private bool TryPublishViaCli(string outputDir, string textureFilename, string dataFilename, out string log)
        {
            var opts = CollectCliOptions();
            var fullPaths = m_SpriteListManager.Items
                .Where(s => !s.Excluded)
                .Select(s => AssetPathToFullPath(s.Path))
                .Where(p => !string.IsNullOrEmpty(p) && File.Exists(p))
                .ToList();

            string pngFull = Path.Combine(outputDir, textureFilename);
            string dataFull = Path.Combine(outputDir, dataFilename);
            return m_CliPacker.Pack(opts, fullPaths, pngFull, dataFull, out log);
        }

        /// <summary>
        /// 预览专用：调用官方 CLI 生成排布，仅读取其描述文件的坐标/朝向回填到精灵项；
        /// 预览贴图仍用本地磁盘纹理，不采用 CLI 导出的 PNG（Essential 模式会标红水印）。
        /// CLI 不可用、调用失败或解析失败时返回 false，由上层回退自研排布。
        /// </summary>
        private bool TryLayoutViaCliForPreview(out int atlasWidth, out int atlasHeight)
        {
            atlasWidth = 0;
            atlasHeight = 0;

            if (!m_CliPacker.IsAvailable) return false;

            // 复位上一轮的排除标记：每一轮都把全部主项重新交给 CLI 判断，
            // 避免上一轮未装入的精灵被 !Excluded 过滤后永远不再参与排布（调大图集上限后也装不回来）。
            foreach (var item in m_SpriteListManager.Items)
            {
                if (item.IsAlias) continue;
                item.Excluded = false;
                item.ExclusionReason = null;
                item.PosX = 0;
                item.PosY = 0;
                item.Rotated = false;
            }

            // 别名跟随主项槽位，不重复提交给 CLI；主项全部参与本轮排布
            var fullPaths = m_SpriteListManager.Items
                .Where(s => !s.IsAlias)
                .Select(s => AssetPathToFullPath(s.Path))
                .Where(p => !string.IsNullOrEmpty(p) && File.Exists(p))
                .ToList();
            if (fullPaths.Count == 0) return false;

            string tempDir = Path.Combine(Path.GetTempPath(), "AtlasPackerPreviewCli", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string pngFull = Path.Combine(tempDir, "preview_sheet.png");
            string dataFull = Path.Combine(tempDir, "preview_data.txt");

            try
            {
                var opts = CollectCliOptions();
                if (!m_CliPacker.Pack(opts, fullPaths, pngFull, dataFull, out string cliLog))
                {
                    Debug.LogWarning($"[AtlasPacker] 预览 CLI 排布失败，回退自研：{cliLog}");
                    return false;
                }

                string data = File.ReadAllText(dataFull, GetAtlasDescriptionEncoding());

                // meta 画布尺寸
                var sizeMatch = Regex.Match(data, "\"size\"\\s*:\\s*\\{\"w\"\\s*:\\s*(\\d+),\\s*\"h\"\\s*:\\s*(\\d+)\\}");
                if (!sizeMatch.Success) return false;
                atlasWidth = int.Parse(sizeMatch.Groups[1].Value);
                atlasHeight = int.Parse(sizeMatch.Groups[2].Value);

                // 逐帧解析：帧名 + frame 坐标 + rotated（帧名归一化去 .png 后缀）
                var frameMap = new Dictionary<string, (int x, int y, bool rot)>(StringComparer.OrdinalIgnoreCase);
                var frameMatches = Regex.Matches(data,
                    "\"([^\"]+)\"\\s*:\\s*\\{\\s*\"frame\"\\s*:\\s*\\{\"x\"\\s*:\\s*(\\d+),\"y\"\\s*:\\s*(\\d+),\"w\"\\s*:\\s*(\\d+),\"h\"\\s*:\\s*(\\d+)\\},\\s*\"rotated\"\\s*:\\s*(true|false)");
                foreach (Match m in frameMatches)
                {
                    string key = Path.GetFileNameWithoutExtension(m.Groups[1].Value);
                    int x = int.Parse(m.Groups[2].Value);
                    int y = int.Parse(m.Groups[3].Value);
                    bool rot = m.Groups[6].Value.Equals("true", StringComparison.OrdinalIgnoreCase);
                    frameMap[key] = (x, y, rot);
                }

                if (frameMap.Count == 0) return false;

                // 回填主项；别名随后跟随主项槽位
                int matched = 0;
                foreach (var item in m_SpriteListManager.Items)
                {
                    if (item.IsAlias) continue;
                    if (frameMap.TryGetValue(item.Name, out var f))
                    {
                        item.PosX = f.x;
                        item.PosY = f.y;
                        item.Rotated = f.rot;
                        item.Excluded = false;
                        item.ExclusionReason = null;
                        matched++;
                    }
                    else
                    {
                        item.Excluded = true;
                        item.ExclusionReason = "CLI 排布未匹配到帧";
                        item.PosX = 0;
                        item.PosY = 0;
                        item.Rotated = false;
                    }
                }

                foreach (var item in m_SpriteListManager.Items)
                {
                    if (!item.IsAlias || item.AliasOf == null) continue;
                    item.PosX = item.AliasOf.PosX;
                    item.PosY = item.AliasOf.PosY;
                    item.Rotated = item.AliasOf.Rotated;
                    item.Excluded = item.AliasOf.Excluded;
                    item.ExclusionReason = item.AliasOf.ExclusionReason;
                }

                if (matched > 0)
                {
                    int inputMasters = m_SpriteListManager.Items.Count(s => !s.IsAlias);
                    var unmatched = m_SpriteListManager.Items
                        .Where(i => !i.IsAlias && i.Excluded)
                        .Select(i => i.Name)
                        .ToList();
                    string unmatchedText = unmatched.Count > 0 ? $"，未装入: {string.Join(", ", unmatched)}" : "";
                    Debug.Log($"[AtlasPacker] 预览采用 CLI 排布：{matched}/{inputMasters} 帧，画布 {atlasWidth}×{atlasHeight}{unmatchedText}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[AtlasPacker] 预览 CLI 排布异常，回退自研：{ex.Message}");
                return false;
            }
            finally
            {
                try { if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true); } catch { /* 临时目录清理失败忽略 */ }
            }
        }

        /// <summary>从当前 UI 控件收集 CLI 打包参数快照。</summary>
        private AtlasCliOptions CollectCliOptions()
        {
            return new AtlasCliOptions
            {
                dataFormat = m_PopupDataFormat?.value ?? "Unity3D",
                maxWidth = ParseDropdownInt(m_PopupMaxWidth, 4096),
                maxHeight = ParseDropdownInt(m_PopupMaxHeight, 4096),
                fixedWidth = ParseDropdownInt(m_PopupFixedWidth, 0),
                fixedHeight = ParseDropdownInt(m_PopupFixedHeight, 0),
                sizeConstraints = m_PopupSizeConstraints?.value ?? "POT",

                borderPadding = m_IntBorderPadding.value,
                shapePadding = m_IntShapePadding.value,
                innerPadding = m_IntInnerPadding != null ? m_IntInnerPadding.value : 0,
                extrude = m_IntExtrude != null ? m_IntExtrude.value : 0,

                allowRotation = m_ToggleAllowRotation != null && m_ToggleAllowRotation.value,
                trimMode = m_PopupTrimMode?.value ?? "Trim",
                trimThreshold = m_IntTrimThreshold != null ? m_IntTrimThreshold.value : 1,

                algorithm = m_PopupAlgorithm?.value ?? "MaxRects",
                heuristics = m_PopupHeuristics?.value ?? "Best",

                scale = m_FloatScale != null ? m_FloatScale.value : 1f,
                scaleMode = m_PopupScaleMode?.value ?? "Smooth",
                premultiplyAlpha = m_TogglePremultiply != null && m_TogglePremultiply.value,
                reduceBorderArtifacts = m_ToggleReduceBorder != null && m_ToggleReduceBorder.value,
                heuristicMask = m_ToggleHeuristicMask != null && m_ToggleHeuristicMask.value,
                trimSpriteNames = m_ToggleTrimNames != null && m_ToggleTrimNames.value,
                texturePath = m_TxtTexturePath?.value ?? "",
                imageFormat = m_PopupImageFormat?.value ?? "RGBA8888",
                dithering = m_PopupDithering?.value ?? "NearestNeighbour",
                dpi = m_IntDpi != null ? m_IntDpi.value : 72,
                autoAlias = m_ToggleAutoAlias == null || m_ToggleAutoAlias.value,
            };
        }

        /// <summary>
        /// 生成图集纹理（把所有小图拼合成一张大图）
        /// </summary>
        /// <summary>
        /// 生成图集纹理（委托给 AtlasExporter）
        /// </summary>
        private Texture2D GenerateAtlasTexture(int maxW, int maxH)
        {
            int padding = m_IntBorderPadding.value;
            int shapePadding = m_IntShapePadding.value;

            // 计算最优排布
            if (!CalculateBestLayout(maxW, maxH, padding, shapePadding, out int bestWidth, out int bestHeight, out var bestPositions))
            {
                return null;
            }

            m_ActualAtlasWidth = bestWidth;
            m_ActualAtlasHeight = bestHeight;
            // 位置已由排布计算器写回各图片项，不再按索引二次配对
            var finalSortedItems = GetSortedItemsByArea();

            // 委托给 Exporter 生成纹理
            m_Exporter.TextureLoader = (fullPath) => LoadTextureFromDisk(fullPath);
            m_Exporter.AssetPathToFullPath = AssetPathToFullPath;
            bool premultiply = m_TogglePremultiply != null && m_TogglePremultiply.value;
            int extrude = m_IntExtrude != null ? m_IntExtrude.value : 0;
            return m_Exporter.GenerateAtlasPng(finalSortedItems, bestWidth, bestHeight, padding, shapePadding,
                premultiply, extrude);
        }

        /// <summary>
        /// 生成 TexturePacker 格式的 JSON 描述文件
        /// </summary>
        /// <summary>
        /// 图集描述文件编码：对齐 TexturePacker 官方产物（GBK / 代码页 936），
        /// 避免中文帧名按 UTF-8 写出后被运行时按 GBK 解析产生乱码。
        /// </summary>
        private static System.Text.Encoding GetAtlasDescriptionEncoding()
        {
            return System.Text.Encoding.GetEncoding(936);
        }

        private string GenerateJsonDescription(string textureFilename, int width, int height)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("{\"frames\": {");
            sb.AppendLine();

            var packedItems = new List<SpriteItemInfo>();
            foreach (var item in m_SpriteListManager.Items)
            {
                if (item.Excluded) continue;
                if (LoadItemTexture(item) == null) continue;
                packedItems.Add(item);
            }

            bool trimNames = m_ToggleTrimNames != null && m_ToggleTrimNames.value;

            for (int i = 0; i < packedItems.Count; i++)
            {
                var item = packedItems[i];
                // 别名复用主项槽位：纹理尺寸、朝向跟随主项
                var tex = item.IsAlias ? LoadItemTexture(item.AliasOf) : LoadItemTexture(item);

                // y 坐标：直接用预览区的 posY（从顶部开始，和 TexturePacker 格式一致）
                int yTop = item.PosY;

                int frameW = item.Rotated ? tex.height : tex.width;
                int frameH = item.Rotated ? tex.width : tex.height;

                // 帧名口径对齐官方：默认保留完整文件名（含 .png 后缀）；
                // 开启 trimSpriteNames 时再去掉扩展名。Name 字段存的是无扩展名名，故从 Path 取全名。
                string fullFileName = Path.GetFileName(item.Path);
                if (string.IsNullOrEmpty(fullFileName)) fullFileName = item.Name;
                string displayName = trimNames ? Path.GetFileNameWithoutExtension(fullFileName) : fullFileName;
                sb.AppendLine($"\"{displayName}\":");
                sb.AppendLine("{");
                sb.AppendLine($"\t\"frame\": {{\"x\":{item.PosX},\"y\":{yTop},\"w\":{frameW},\"h\":{frameH}}},");
                sb.AppendLine($"\t\"rotated\": {item.Rotated.ToString().ToLower()},");
                sb.AppendLine($"\t\"trimmed\": false,");
                sb.AppendLine($"\t\"spriteSourceSize\": {{\"x\":0,\"y\":0,\"w\":{tex.width},\"h\":{tex.height}}},");
                sb.AppendLine($"\t\"sourceSize\": {{\"w\":{tex.width},\"h\":{tex.height}}}");
                sb.Append(i < packedItems.Count - 1 ? "}," : "}");
                sb.AppendLine();
            }

            sb.AppendLine("},");
            sb.AppendLine("\"meta\": {");
            sb.AppendLine($"\t\"app\": \"http://www.codeandweb.com/texturepacker \",");
            sb.AppendLine($"\t\"version\": \"1.0\",");
            sb.AppendLine($"\t\"image\": \"{textureFilename}\",");
            string imageFormat = m_PopupImageFormat?.value ?? "RGBA8888";
            sb.AppendLine($"\t\"format\": \"{imageFormat}\",");
            sb.AppendLine($"\t\"size\": {{\"w\":{width},\"h\":{height}}},");
            float atlasScale = m_FloatScale != null ? m_FloatScale.value : 1f;
            sb.AppendLine($"\t\"scale\": \"{atlasScale.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)}\",");
            sb.AppendLine($"\t\"smartupdate\": \"$TexturePacker:SmartUpdate:{System.Guid.NewGuid():N}:1/1$\"");
            sb.AppendLine("}");
            sb.Append("}");

            return sb.ToString();
        }

        private void OnSave()
        {
            // 选择保存路径
            string path = EditorUtility.SaveFilePanel("保存项目", "Assets/Res/Atlas", "NewAtlas", "atlasproj");
            if (string.IsNullOrEmpty(path)) return;

            // 构建项目数据（用可序列化的类）
            var project = new AtlasProject
            {
                // 输出设置
                dataFormat = m_PopupDataFormat?.value ?? "Unity3D",
                dataFilename = m_TxtDataFilename.value,
                textureFormat = m_PopupTextureFormat?.value ?? "PNG",
                textureFile = m_TxtTextureFile.value,
                texturePath = m_TxtTexturePath?.value,
                pngOptLevel = m_SliderPngOpt != null ? (int)m_SliderPngOpt.value : 2,
                dpi = m_IntDpi != null ? m_IntDpi.value : 72,
                premultiplyAlpha = m_TogglePremultiply != null && m_TogglePremultiply.value,
                imageFormat = m_PopupImageFormat?.value ?? "RGBA8888",
                ditheringAlgorithm = m_PopupDithering?.value ?? "NearestNeighbour",

                // 尺寸设置
                maxWidth = m_PopupMaxWidth.value,
                maxHeight = m_PopupMaxHeight.value,
                fixedWidth = ParseDropdownInt(m_PopupFixedWidth, 0),
                fixedHeight = ParseDropdownInt(m_PopupFixedHeight, 0),
                sizeConstraints = m_PopupSizeConstraints?.value ?? "POT",
                forceSquare = m_ToggleForceSquare != null && m_ToggleForceSquare.value,
                scale = m_FloatScale != null ? m_FloatScale.value : 1f,
                scaleMode = m_PopupScaleMode?.value ?? "Smooth",

                // 布局设置
                layoutAlgorithm = m_PopupAlgorithm?.value ?? "MaxRects",
                heuristicStrategy = m_PopupHeuristics?.value ?? "Best",
                packMode = m_PopupPack?.value ?? "Best",
                borderPadding = m_IntBorderPadding.value,
                shapePadding = m_IntShapePadding.value,
                innerPadding = m_IntInnerPadding != null ? m_IntInnerPadding.value : 0,
                extrude = m_IntExtrude != null ? m_IntExtrude.value : 0,
                divisorX = m_IntDivisorX != null && m_IntDivisorX.value > 0 ? m_IntDivisorX.value : 1,
                divisorY = m_IntDivisorY != null && m_IntDivisorY.value > 0 ? m_IntDivisorY.value : 1,
                allowRotation = m_ToggleAllowRotation != null && m_ToggleAllowRotation.value,
                autoAlias = m_ToggleAutoAlias == null || m_ToggleAutoAlias.value,
                trimMode = m_PopupTrimMode?.value ?? "None",
                trimThreshold = m_IntTrimThreshold != null ? m_IntTrimThreshold.value : 1,
                multipack = m_ToggleMultipack != null && m_ToggleMultipack.value,
                reduceBorderArtifacts = m_ToggleReduceBorder != null && m_ToggleReduceBorder.value,
                shapeOutlines = m_ToggleShapeOutlines != null && m_ToggleShapeOutlines.value,
                heuristicMask = m_ToggleHeuristicMask != null && m_ToggleHeuristicMask.value,
                forceWordAligned = m_ToggleForceWord != null && m_ToggleForceWord.value,
                trimSpriteNames = m_ToggleTrimNames != null && m_ToggleTrimNames.value,

                // 图片列表
                sprites = m_SpriteListManager.Items.Select(s => s.Path).ToArray()
            };

            string json = JsonUtility.ToJson(project, true);
            File.WriteAllText(path, json);

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("成功", $"项目已保存到:\n{path}", "确定");
        }

        private void OnOpen()
        {
            string path = EditorUtility.OpenFilePanel("打开项目", "Assets/Res/Atlas", "atlasproj");
            if (string.IsNullOrEmpty(path)) return;

            LoadProjectFromPath(path);
        }

        /// <summary>
        /// 从指定存档路径加载工程配置与图片列表。
        /// </summary>
        private void LoadProjectFromPath(string path)
        {
            try
            {
                string json = File.ReadAllText(path);
                var project = JsonUtility.FromJson<AtlasProject>(json);

                if (project == null)
                {
                    EditorUtility.DisplayDialog("错误", "项目文件格式无效", "确定");
                    return;
                }

                // 恢复配置（带空值保护）
                if (m_PopupDataFormat != null) m_PopupDataFormat.value = project.dataFormat ?? "Unity3D";
                m_TxtDataFilename.value = project.dataFilename ?? "NewAtlas.txt";
                if (m_PopupTextureFormat != null) m_PopupTextureFormat.value = project.textureFormat ?? "PNG";
                if (m_PopupImageFormat != null) m_PopupImageFormat.value = project.imageFormat ?? "RGBA8888";
                if (m_PopupDithering != null) m_PopupDithering.value = project.ditheringAlgorithm ?? "NearestNeighbour";
                m_TxtTextureFile.value = project.textureFile ?? "NewAtlasTex.png";
                if (m_TxtTexturePath != null) m_TxtTexturePath.value = project.texturePath;
                if (m_SliderPngOpt != null) m_SliderPngOpt.value = project.pngOptLevel;
                if (m_IntDpi != null) m_IntDpi.value = project.dpi;
                if (m_TogglePremultiply != null) m_TogglePremultiply.value = project.premultiplyAlpha;
                m_PopupMaxWidth.value = project.maxWidth ?? "1024";
                m_PopupMaxHeight.value = project.maxHeight ?? "1024";
                if (m_PopupFixedWidth != null) m_PopupFixedWidth.value = project.fixedWidth.ToString();
                if (m_PopupFixedHeight != null) m_PopupFixedHeight.value = project.fixedHeight.ToString();
                if (m_PopupSizeConstraints != null) m_PopupSizeConstraints.value = project.sizeConstraints ?? "POT";
                if (m_ToggleForceSquare != null) m_ToggleForceSquare.value = project.forceSquare;
                if (m_FloatScale != null) m_FloatScale.value = project.scale;
                if (m_PopupScaleMode != null) m_PopupScaleMode.value = project.scaleMode ?? "Smooth";
                m_IntBorderPadding.value = project.borderPadding;
                m_IntShapePadding.value = project.shapePadding;
                if (m_IntInnerPadding != null) m_IntInnerPadding.value = project.innerPadding;
                if (m_IntExtrude != null) m_IntExtrude.value = project.extrude;
                if (m_IntDivisorX != null) m_IntDivisorX.value = project.divisorX;
                if (m_IntDivisorY != null) m_IntDivisorY.value = project.divisorY;
                if (m_PopupAlgorithm != null) m_PopupAlgorithm.value = project.layoutAlgorithm ?? "MaxRects";
                if (m_PopupHeuristics != null) m_PopupHeuristics.value = project.heuristicStrategy ?? "Best";
                if (m_PopupPack != null) m_PopupPack.value = project.packMode ?? "Best";
                if (m_ToggleAllowRotation != null) m_ToggleAllowRotation.value = project.allowRotation;
                if (m_ToggleAutoAlias != null) m_ToggleAutoAlias.value = project.autoAlias;
                if (m_PopupTrimMode != null) m_PopupTrimMode.value = project.trimMode ?? "Trim";
                if (m_IntTrimThreshold != null) m_IntTrimThreshold.value = project.trimThreshold;
                if (m_ToggleMultipack != null) m_ToggleMultipack.value = project.multipack;
                if (m_ToggleReduceBorder != null) m_ToggleReduceBorder.value = project.reduceBorderArtifacts;
                if (m_ToggleShapeOutlines != null) m_ToggleShapeOutlines.value = project.shapeOutlines;
                if (m_ToggleHeuristicMask != null) m_ToggleHeuristicMask.value = project.heuristicMask;
                if (m_ToggleForceWord != null) m_ToggleForceWord.value = project.forceWordAligned;
                if (m_ToggleTrimNames != null) m_ToggleTrimNames.value = project.trimSpriteNames;

                // 加载图片列表
                m_SpriteListManager.Items.Clear();
                m_SpritesList.Clear();
                ClearSelection();
                if (project.sprites != null)
                {
                    foreach (var spritePath in project.sprites)
                    {
                        if (!string.IsNullOrEmpty(spritePath))
                        {
                            AddSpriteItem(spritePath);
                        }
                    }
                }

                UpdateSpriteCount();
                GeneratePreviewImage();

                EditorUtility.DisplayDialog("成功", $"项目已加载，共 {m_SpriteListManager.Items.Count} 张图片", "确定");
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("错误", $"加载项目失败: {e.Message}\n{e.StackTrace}", "确定");
            }
        }

        private void OnNew()
        {
            if (m_SpriteListManager.Items.Count > 0)
            {
                if (!EditorUtility.DisplayDialog("确认", "确定要新建项目吗？当前内容将被清空。", "确定", "取消"))
                    return;
            }

            m_SpriteListManager.Items.Clear();
            m_SpritesList.Clear();
            ClearSelection();
            // 新建项目时重置输出文件名，避免沿用上一图集名导致串档
            if (m_TxtDataFilename != null) m_TxtDataFilename.value = "NewAtlas.txt";
            if (m_TxtTextureFile != null) m_TxtTextureFile.value = "NewAtlasTex.png";

            UpdateSpriteCount();
            GeneratePreviewImage();
        }

        /// <summary>
        /// 项目文件格式（.atlasproj）
        /// </summary>
        [System.Serializable]
        private class AtlasProject
        {
            // 版本号
            public string version = "1.0";

            // 输出设置
            public string dataFormat = "Unity3D";
            public string dataFilename;
            public string textureFormat = "PNG";
            public string textureFile;
            public int pngOptLevel = 2;
            public int dpi = 72;
            public bool premultiplyAlpha = false;
            public string imageFormat = "RGBA8888";
            public string ditheringAlgorithm = "NearestNeighbour";
            public string texturePath;

            // 尺寸设置
            public string maxWidth;
            public string maxHeight;
            public int fixedWidth = 0;
            public int fixedHeight = 0;
            public string sizeConstraints = "POT";
            public bool forceSquare = false;
            public float scale = 1f;
            public string scaleMode = "Smooth";

            // 布局设置
            public string layoutAlgorithm = "MaxRects";
            public string heuristicStrategy = "Best";
            public string packMode = "Best";
            public int borderPadding = 2;
            public int shapePadding = 2;
            public int innerPadding = 0;
            public int extrude = 0;
            public int divisorX = 1;
            public int divisorY = 1;
            public bool allowRotation = true;
            public bool autoAlias = true;
            public string trimMode = "Trim";
            public int trimThreshold = 1;
            public bool multipack = false;
            public bool reduceBorderArtifacts = false;
            public bool shapeOutlines = false;
            public bool heuristicMask = false;
            public bool forceWordAligned = false;
            public bool trimSpriteNames = false;

            // 图片列表
            public string[] sprites;
        }

        #endregion

        #region 核心逻辑

        /// <summary>
        /// 从文件夹加载所有图片（委托给 SpriteListManager）
        /// </summary>
        private void LoadSpritesFromFolder(string folderPath)
        {
            // 清空列表
            m_SpriteListManager.Clear();
            ClearSelection();

            // 确保是 Assets/ 相对路径
            string assetFolder = folderPath;
            if (!assetFolder.StartsWith("Assets/"))
            {
                assetFolder = FullPathToAssetPath(folderPath.Replace('\\', '/'));
            }

            // 委托给 SpriteListManager 加载
            m_SpriteListManager.LoadSpritesFromFolder(assetFolder);

            // 刷新列表 UI
            RefreshSpriteListUI();

            if (m_SpriteListManager.Items.Count == 0)
            {
                EditorUtility.DisplayDialog("提示", $"文件夹里没有找到图片:\n{assetFolder}", "确定");
            }

            UpdateSpriteCount();
            GeneratePreviewImage();
        }

        /// <summary>
        /// 统一刷新图片列表 UI（带缩略图的列表项）
        /// </summary>
        private void RefreshSpriteListUI()
        {
            // 刷新前统一按文件名排序（对齐 TexturePacker 右侧 Sprites 列表规则），兜底所有添加入口
            m_SpriteListManager.SortByName();

            m_SpritesList.Clear();
            for (int i = 0; i < m_SpriteListManager.Items.Count; i++)
            {
                var item = m_SpriteListManager.Items[i];
                string fullPath = AssetPathToFullPath(item.Path);

                // 创建 UI 项
                var itemElement = new VisualElement();
                itemElement.AddToClassList("sprite-item");

                // 缩略图
                var img = new Image();
                img.AddToClassList("sprite-item-image");
                var tex = LoadTextureFromDisk(fullPath);
                if (tex != null) img.image = tex;

                // 信息
                var info = new VisualElement();
                info.AddToClassList("sprite-item-info");

                var nameLabel = new Label(item.Name);
                nameLabel.AddToClassList("sprite-item-name");
                info.Add(nameLabel);

                var sizeLabel = new Label(item.Size);
                sizeLabel.AddToClassList("sprite-item-size");
                info.Add(sizeLabel);

                itemElement.Add(img);
                itemElement.Add(info);

                // 点击选中效果
                int index = i;
                itemElement.RegisterCallback<ClickEvent>(evt => OnSpriteItemClick(evt, index, itemElement));

                // 右键菜单
                itemElement.AddManipulator(new ContextualMenuManipulator(evt =>
                {
                    evt.menu.AppendAction("删除此图片", action => RemoveSpriteAt(index));
                    evt.menu.AppendAction("在文件夹中显示", action => ShowInFolder(m_SpriteListManager.Items[index].Path));
                    evt.menu.AppendSeparator();
                    evt.menu.AppendAction("添加图片...", action => OnAddSprites());
                    evt.menu.AppendAction("添加文件夹...", action => OnAddFolder());
                }));

                m_SpritesList.Add(itemElement);
            }

            // 列表项重建后按选中集合恢复底色
            RefreshListSelectionVisual();
        }

        /// <summary>
        /// 磁盘绝对路径转 Assets 相对路径
        /// </summary>
        private string FullPathToAssetPath(string fullPath)
        {
            return AtlasPathUtils.FullToAssetPath(fullPath);
        }

        /// <summary>
        /// 生成中间预览图（模拟打包效果）
        /// </summary>
        private void GeneratePreviewImage()
        {
            try
            {
                GeneratePreviewImageInternal();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"GeneratePreviewImage error: {e.Message}\n{e.StackTrace}");
            }
        }

        private void GeneratePreviewImageInternal()
        {
            if (m_SpriteListManager.Items.Count == 0)
            {
                m_PreviewImageContainer.visible = false;
                m_LblPreviewEmpty.visible = true;
                m_LblAtlasSize.text = AtlasConstants.EmptyTextureHint;
                return;
            }

            // 隐藏提示，显示预览
            m_LblPreviewEmpty.visible = false;
            m_PreviewImageContainer.visible = true;

            // 读取最大尺寸设置
            int maxAtlasWidth = 1024;
            int maxAtlasHeight = 1024;
            int.TryParse(m_PopupMaxWidth.value, out maxAtlasWidth);
            int.TryParse(m_PopupMaxHeight.value, out maxAtlasHeight);

            int padding = m_IntBorderPadding.value;
            int shapePadding = m_IntShapePadding.value;

            // 清空之前的预览
            DestroyHighlightBox();
            m_PreviewImageContainer.Clear();

            // 重置上一轮的缩放残留与适应重试计数，避免旧的下限缩放被带到新预览
            m_FitRetryCount = 0;
            m_ZoomScale = AtlasConstants.DefaultZoomScale;

            // 优先官方 CLI 排布回填预览坐标（与导出一致）；CLI 不可用/失败时回退自研 MaxRects。
            bool packed = false;
            int usedW = maxAtlasWidth, usedH = maxAtlasHeight;
            List<Vector2Int> usedPositions = null;

            if (TryLayoutViaCliForPreview(out int cliW, out int cliH))
            {
                usedW = cliW;
                usedH = cliH;
                packed = true;
            }

            if (!packed)
            {
                int[] trySizes = { maxAtlasWidth, 4096, 8192 };
                foreach (int trySize in trySizes)
                {
                    if (CalculateBestLayout(trySize, trySize, padding, shapePadding, out int bw, out int bh, out var bpos))
                    {
                        usedW = bw; usedH = bh; usedPositions = bpos;
                        packed = true;
                        break;
                    }
                }
            }
            var bestWidth = usedW;
            var bestHeight = usedH;
            var bestPositions = usedPositions;

            if (!packed)
            {
                m_PreviewImageContainer.visible = false;
                m_LblPreviewEmpty.visible = true;

                int total = m_SpriteListManager.Items.Count;
                int excluded = m_LayoutCalculator.ExcludedNames.Count;
                long validArea = m_LayoutCalculator.LastValidArea;
                long atlasCapacity = (long)maxAtlasWidth * maxAtlasHeight;
                if (total > 0 && excluded >= total)
                {
                    // 全部图片都因尺寸超限或加载失败被排除，多半是图集最大尺寸太小
                    m_LblPreviewEmpty.text =
                        $"无法排布：{excluded}/{total} 张图片超出图集尺寸 {maxAtlasWidth}×{maxAtlasHeight} 或加载失败。\n" +
                        "请调大左侧「最大尺寸」后重试。";
                    m_LblAtlasSize.text = $"全部 {excluded} 张被跳过（图集尺寸过小）";
                }
                else if (validArea > atlasCapacity)
                {
                    // 尺寸能装下单张，但所有图的总面积超过了图集容量——需要更大图集或多页
                    m_LblPreviewEmpty.text =
                        $"无法排布：{m_LayoutCalculator.LastValidCount} 张图共 {validArea / 1000}K 像素，\n" +
                        $"超过图集容量 {maxAtlasWidth}×{maxAtlasHeight}={atlasCapacity / 1000}K 像素。\n" +
                        "请调大「最大尺寸」（如 8192），或减少图片数量。";
                    m_LblAtlasSize.text = $"面积不足：需 {validArea / 1000}K，容量 {atlasCapacity / 1000}K";
                }
                else
                {
                    m_LblPreviewEmpty.text = "拖放图片文件夹到这里\n自动扫描并打包图集";
                    m_LblAtlasSize.text = AtlasConstants.EmptyTextureHint;
                }
                return;
            }

            // 用最优结果
            int actualWidth = bestWidth;
            int actualHeight = bestHeight;
            m_ActualAtlasWidth = bestWidth;
            m_ActualAtlasHeight = bestHeight;
            // 位置已由排布计算器直接写回各图片项，这里只取排序后的列表用于绘制，不再按索引二次配对
            var finalSortedItems = GetSortedItemsByArea();

            // 预览容器大小 = 实际需要的大小；POT 约束下向上对齐到 2 的幂，NPOT 直接贴内容包围盒
            bool potConstraints = !string.Equals(m_PopupSizeConstraints?.value, "NPOT", StringComparison.OrdinalIgnoreCase);
            int previewWidth = potConstraints ? NextPOT(actualWidth) : actualWidth;
            int previewHeight = potConstraints ? NextPOT(actualHeight) : actualHeight;

            m_BaseAtlasW = previewWidth;
            m_BaseAtlasH = previewHeight;
            m_PreviewImageContainer.style.width = previewWidth;
            m_PreviewImageContainer.style.height = previewHeight;

            // 容器整体为深灰纯色背景（对齐 TexturePackerGUI：主画布之外的溢出区为深灰）；棋盘格只铺主画布矩形
            m_PreviewImageContainer.style.backgroundImage = null;
            m_PreviewImageContainer.style.backgroundColor = new Color(0.29f, 0.29f, 0.29f);

            // 生成棋盘格背景纹理（仅覆盖主画布）
            var checkerTex = new Texture2D(previewWidth, previewHeight, TextureFormat.RGBA32, false);
            checkerTex.hideFlags = HideFlags.HideAndDontSave;
            checkerTex.filterMode = FilterMode.Point;
            checkerTex.wrapMode = TextureWrapMode.Clamp;
            int checkerSize = 8;
            for (int py = 0; py < previewHeight; py++)
            {
                for (int px = 0; px < previewWidth; px++)
                {
                    bool isEven = ((px / checkerSize) + (py / checkerSize)) % 2 == 0;
                    Color bg = isEven ? new Color(0.85f, 0.85f, 0.85f) : new Color(0.65f, 0.65f, 0.65f);
                    checkerTex.SetPixel(px, py, bg);
                }
            }
            checkerTex.Apply();

            // 主画布棋盘格底图，置于精灵下层
            var boardBg = new Image { name = "board-bg", image = checkerTex };
            boardBg.scaleMode = ScaleMode.StretchToFill;
            boardBg.style.position = Position.Absolute;
            boardBg.style.left = 0;
            boardBg.style.top = 0;
            boardBg.style.width = previewWidth;
            boardBg.style.height = previewHeight;
            m_PreviewImageContainer.Add(boardBg);

            // 绘制图片（位置已经在前面的 MaxRects 排布中计算好了）
            int packedCount = 0;
            foreach (var item in finalSortedItems)
            {
                // 跳过被排除的超尺寸 / 加载失败图片；别名复用主项槽位，预览不重复绘制（稍后统一画到图集下方）
                if (item.Excluded || item.IsAlias) continue;

                var tex = LoadItemTexture(item);
                if (tex == null) continue;

                // 创建 Image 元素
                var img = new Image();
                img.image = tex;
                img.style.position = Position.Absolute;
                img.style.left = item.PosX;
                img.style.top = item.PosY;
                ApplyPreviewOrientation(img, tex, item);

                // 记录 item 引用到 userData，点击时直接获取
                img.userData = item;

                // 点击选中预览区的图片
                img.RegisterCallback<ClickEvent>(evt => OnPreviewImageClick(evt, img));

                m_PreviewImageContainer.Add(img);
                packedCount++;
            }

            // 被排除的图：按原始完整尺寸排在主画布外侧的深灰背景区（对齐 TexturePackerGUI：
            // 装不下的精灵不缩放、不加框，完整散落在图集矩形之外，仅随视图整体缩放）
            var excludedItems = finalSortedItems.Where(i => i.Excluded).ToList();
            const int overflowGapY = 24;   // 主画布与溢出区纵向间距
            const int overflowGapX = 16;   // 溢出精灵之间横向间距
            // 对齐官方：溢出精灵从主画布下方左侧起横向流式铺开，允许延伸到主画布右侧外；
            // 折行宽度放宽到主画布宽度的两倍，超出后才折到下一排。
            int overflowBandWidth = previewWidth * 2;
            var overflowLayout = new List<(SpriteItemInfo item, int x, int y, int w, int h)>();
            int overflowRight = previewWidth;
            int overflowBottom = previewHeight;

            if (excludedItems.Count > 0)
            {
                int bandX = padding;
                int bandY = previewHeight + overflowGapY;
                int rowH = 0;

                // 第一遍：计算每张溢出精灵的摆放位置与溢出区包围盒（横向流式，超 band 宽度再折行）
                foreach (var item in excludedItems)
                {
                    var tex = LoadItemTexture(item);
                    if (tex == null) continue;
                    int ew = tex.width;
                    int eh = tex.height;

                    if (bandX > padding && bandX + ew > overflowBandWidth)
                    {
                        bandX = padding;
                        bandY += rowH + overflowGapX;
                        rowH = 0;
                    }

                    overflowLayout.Add((item, bandX, bandY, ew, eh));
                    bandX += ew + overflowGapX;
                    rowH = Math.Max(rowH, eh);
                    overflowRight = Math.Max(overflowRight, bandX - overflowGapX);
                    overflowBottom = Math.Max(overflowBottom, bandY + eh);
                }

                // 第二遍：绘制溢出精灵（原始尺寸、无框、不透明）
                foreach (var (item, x, y, ew, eh) in overflowLayout)
                {
                    var tex = LoadItemTexture(item);
                    if (tex == null) continue;
                    var eimg = new Image { image = tex };
                    eimg.style.position = Position.Absolute;
                    eimg.style.left = x;
                    eimg.style.top = y;
                    eimg.style.width = ew;
                    eimg.style.height = eh;
                    eimg.userData = item;
                    eimg.RegisterCallback<ClickEvent>(evt => OnPreviewImageClick(evt, eimg));
                    m_PreviewImageContainer.Add(eimg);
                }
            }

            // 容器总尺寸 = 主画布 + 溢出区包围盒，更新基础尺寸供缩放用
            int containerW = Math.Max(previewWidth, overflowRight);
            int containerH = overflowBottom + 12;
            m_BaseAtlasW = containerW;
            m_BaseAtlasH = containerH;
            m_PreviewImageContainer.style.width = containerW;
            m_PreviewImageContainer.style.height = containerH;

            // 更新状态栏（对齐真实 TexturePacker 格式）
            int memKB = previewWidth * previewHeight * 4 / 1024;
            string statusText;
            int excludedCount = excludedItems.Count;
            if (excludedCount > 0)
            {
                statusText = $"{excludedCount} not fitting sprites - try using multipacking. Size: {previewWidth}x{previewHeight} ({memKB}kB)";
                // 对齐官方：溢出告警以红字显示
                m_LblAtlasSize.style.color = new Color(1f, 0.36f, 0.30f);
            }
            else
            {
                statusText = $"Size: {previewWidth}x{previewHeight} ({memKB}kB)";
                m_LblAtlasSize.style.color = StyleKeyword.Initial;
            }
            m_LblAtlasSize.text = statusText;

            // 排布完成后自动适应窗口，让整图一屏可见
            m_FitRetryCount = 0;
            m_PreviewImageContainer.schedule.Execute(FitToWindow).ExecuteLater(60);

            // 如果有选中项，按选中集合重建预览高亮框
            if (m_SelectedIndices.Count > 0)
            {
                RefreshAllHighlightBoxes();
            }
        }

        /// <summary>
        /// 添加单个 sprite 项（委托给 SpriteListManager）
        /// </summary>
        private void AddSpriteItem(string assetPath)
        {
            // 委托给 SpriteListManager 添加数据
            m_SpriteListManager.AddSpriteItem(assetPath);

            // 统一刷新列表 UI
            RefreshSpriteListUI();
        }

        /// <summary>
        /// 点击预览区的图片
        /// </summary>
        private void OnPreviewImageClick(ClickEvent evt, VisualElement imgElement)
        {
            var item = imgElement.userData as SpriteItemInfo;
            if (item == null) return;

            // 找到这个 item 在列表里的索引
            int index = m_SpriteListManager.Items.IndexOf(item);
            if (index < 0) return;
            if (index >= m_SpritesList.Children().Count()) return;

            ApplySelection(index, evt.modifiers);
        }

        /// <summary>
        /// 点击 sprite 项
        /// </summary>
        private void OnSpriteItemClick(ClickEvent evt, int index, VisualElement itemElement)
        {
            ApplySelection(index, evt.modifiers);
        }

        /// <summary>
        /// 统一选中入口：普通点击清空重选；Ctrl 点击切换追加；Shift 点击以主选中为锚点范围选择
        /// </summary>
        private void ApplySelection(int index, EventModifiers modifiers)
        {
            if (index < 0 || index >= m_SpriteListManager.Items.Count) return;

            bool ctrl = modifiers.HasFlag(EventModifiers.Control);
            bool shift = modifiers.HasFlag(EventModifiers.Shift);

            if (shift && m_PrimarySelectedIndex >= 0)
            {
                int lo = Mathf.Min(m_PrimarySelectedIndex, index);
                int hi = Mathf.Max(m_PrimarySelectedIndex, index);
                m_SelectedIndices.Clear();
                for (int i = lo; i <= hi; i++) m_SelectedIndices.Add(i);
            }
            else if (ctrl)
            {
                if (m_SelectedIndices.Contains(index)) m_SelectedIndices.Remove(index);
                else m_SelectedIndices.Add(index);
                m_PrimarySelectedIndex = index;
            }
            else
            {
                m_SelectedIndices.Clear();
                m_SelectedIndices.Add(index);
                m_PrimarySelectedIndex = index;
            }

            RefreshListSelectionVisual();
            RefreshAllHighlightBoxes();
        }

        /// <summary>
        /// 按选中集合刷新右侧列表项底色（TexturePacker 风格蓝色高亮）
        /// </summary>
        private void RefreshListSelectionVisual()
        {
            int childCount = m_SpritesList.Children().Count();
            for (int i = 0; i < childCount; i++)
            {
                var el = m_SpritesList.ElementAt(i);
                el.style.backgroundColor = m_SelectedIndices.Contains(i)
                    ? new Color(0.2f, 0.4f, 0.8f, 0.8f)
                    : StyleKeyword.Null;
            }
        }

        /// <summary>
        /// 清空全部选中状态与高亮框
        /// </summary>
        private void ClearSelection()
        {
            m_SelectedIndices.Clear();
            m_PrimarySelectedIndex = AtlasConstants.NoSelectionIndex;
            DestroyHighlightBox();
        }

        /// <summary>
        /// 全量重建预览区所有选中精灵的高亮（绿底垫底 + 黑色虚线框置顶）
        /// </summary>
        private void RefreshAllHighlightBoxes()
        {
            DestroyHighlightBox();

            foreach (var index in m_SelectedIndices.ToList())
            {
                if (index < 0 || index >= m_SpriteListManager.Items.Count) continue;
                AddHighlightForItem(index, m_SpriteListManager.Items[index]);
            }
        }

        /// <summary>
        /// 为单个选中精灵挂绿底层与虚线框层
        /// </summary>
        private void AddHighlightForItem(int index, SpriteItemInfo item)
        {
            var tex = LoadTextureFromDisk(AssetPathToFullPath(item.Path));
            if (tex == null) return;

            // 旋转 90° 时占位宽高互换
            int boxW = item.Rotated ? tex.height : tex.width;
            int boxH = item.Rotated ? tex.width : tex.height;
            int w = boxW + 2;
            int h = boxH + 2;
            int left = item.PosX - 1;
            int top = item.PosY - 1;

            // 2022.3 UIElements 不支持虚线边框（无 border-style），用背景色短划线拼出黑色虚线框。
            // 对齐官方 TexturePacker：绿色填充层在精灵图片【下方】（仅透明区域透绿，不遮盖精灵本体），
            // 黑色虚线框层在【最上层】。
            const int dash = 10;
            const int gap = 5;
            const int thick = 5;
            var black = Color.black;
            var selectedGreen = new Color(0.18f, 1f, 0.18f, 0.3f);

            // 绿底背景层
            var bg = new VisualElement();
            bg.style.position = Position.Absolute;
            bg.style.left = left;
            bg.style.top = top;
            bg.style.width = w;
            bg.style.height = h;
            bg.style.backgroundColor = selectedGreen;

            // 插到选中精灵 Image 的下方；找不到对应图片时退化为加到最底层
            var targetImg = FindPreviewImage(item);
            if (targetImg != null)
            {
                int imgIdx = m_PreviewImageContainer.IndexOf(targetImg);
                m_PreviewImageContainer.Insert(Mathf.Max(0, imgIdx), bg);
            }
            else
            {
                m_PreviewImageContainer.Insert(0, bg);
            }

            // 虚线框层：四边短划线，置于最上层
            var frame = new VisualElement();
            frame.style.position = Position.Absolute;
            frame.style.left = left;
            frame.style.top = top;
            frame.style.width = w;
            frame.style.height = h;

            // 上、下边短划线
            for (int x = 0; x < w; x += dash + gap)
            {
                int segW = Mathf.Min(dash, w - x);

                var topDash = new VisualElement();
                topDash.style.position = Position.Absolute;
                topDash.style.left = x;
                topDash.style.top = 0;
                topDash.style.width = segW;
                topDash.style.height = thick;
                topDash.style.backgroundColor = black;
                frame.Add(topDash);

                var botDash = new VisualElement();
                botDash.style.position = Position.Absolute;
                botDash.style.left = x;
                botDash.style.top = h - thick;
                botDash.style.width = segW;
                botDash.style.height = thick;
                botDash.style.backgroundColor = black;
                frame.Add(botDash);
            }

            // 左、右边短划线
            for (int y = 0; y < h; y += dash + gap)
            {
                int segH = Mathf.Min(dash, h - y);

                var leftDash = new VisualElement();
                leftDash.style.position = Position.Absolute;
                leftDash.style.left = 0;
                leftDash.style.top = y;
                leftDash.style.width = thick;
                leftDash.style.height = segH;
                leftDash.style.backgroundColor = black;
                frame.Add(leftDash);

                var rightDash = new VisualElement();
                rightDash.style.position = Position.Absolute;
                rightDash.style.left = w - thick;
                rightDash.style.top = y;
                rightDash.style.width = thick;
                rightDash.style.height = segH;
                rightDash.style.backgroundColor = black;
                frame.Add(rightDash);
            }

            m_PreviewImageContainer.Add(frame);
            m_HighlightByIndex[index] = (bg, frame);
        }

        /// <summary>
        /// 在预览容器中按精灵数据反查对应的图片元素
        /// </summary>
        private VisualElement FindPreviewImage(SpriteItemInfo item)
        {
            if (m_PreviewImageContainer == null || item == null) return null;
            foreach (var child in m_PreviewImageContainer.Children())
            {
                if (ReferenceEquals(child.userData, item)) return child;
            }
            return null;
        }

        /// <summary>
        /// 移除全部选中高亮绿底与虚线框
        /// </summary>
        private void DestroyHighlightBox()
        {
            foreach (var pair in m_HighlightByIndex)
            {
                if (pair.Value.bg != null && pair.Value.bg.parent == m_PreviewImageContainer)
                {
                    m_PreviewImageContainer.Remove(pair.Value.bg);
                }
                if (pair.Value.frame != null && pair.Value.frame.parent == m_PreviewImageContainer)
                {
                    m_PreviewImageContainer.Remove(pair.Value.frame);
                }
            }
            m_HighlightByIndex.Clear();
        }

        /// <summary>
        /// 从磁盘加载纹理（带缓存，委托给 TextureLoader）
        /// </summary>
        private Texture2D LoadTextureFromDisk(string fullPath)
        {
            return m_TextureLoader.LoadFromDisk(fullPath, (path) => path);
        }

        /// <summary>
        /// Assets 相对路径转磁盘绝对路径
        /// </summary>
        private string AssetPathToFullPath(string assetPath)
        {
            return AtlasPathUtils.AssetToFullPath(assetPath);
        }

        /// <summary>
        /// 更新 sprite 计数显示
        /// </summary>
        private void UpdateSpriteCount()
        {
            m_LblSpriteCount.text = string.Format(AtlasConstants.SpriteCountFormat, m_SpriteListManager.Items.Count);
        }

        /// <summary>
        /// 创建 Sprite Atlas 资产（委托给 SpriteAtlasCreator）
        /// </summary>
        private void CreateSpriteAtlas(string atlasPath, string folderPath)
        {
            SpriteAtlasCreator.CreateOrUpdate(atlasPath, folderPath);
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 按面积降序排序图片项（TexturePacker 默认）
        /// </summary>
        private List<SpriteItemInfo> GetSortedItemsByArea()
        {
            return m_SpriteListManager.Items.OrderByDescending(item =>
            {
                var tex = LoadItemTexture(item);
                return tex != null ? (long)tex.width * tex.height : 0;
            }).ToList();
        }

        /// <summary>
        /// 加载图片项的纹理（公共方法，减少重复代码）
        /// </summary>
        private Texture2D LoadItemTexture(SpriteItemInfo item)
        {
            return LoadTextureFromDisk(AssetPathToFullPath(item.Path));
        }

        /// <summary>
        /// 按排布朝向设置预览图片的宽高与旋转（旋转 90° 时占位宽高互换）
        /// </summary>
        private static void ApplyPreviewOrientation(Image img, Texture2D tex, SpriteItemInfo item)
        {
            int w = tex.width;
            int h = tex.height;
            // 纹理铺满所在盒子（原生比例，不变形）；避免等比 Contain 在盒内居中而四周留白
            img.scaleMode = ScaleMode.StretchToFill;

            if (item.Rotated)
            {
                // 盒保持原生 w×h，纹理无变形铺满；绕盒中心旋转 90°，旋转后视觉占 h×w。
                // 以旋转后槽位（左上 PosX,PosY，宽 h、高 w）的中心为基准，反推未旋转盒的
                // left/top，使旋转后的盒子与槽位对齐（旋转不改变盒中心位置）。
                img.style.width = w;
                img.style.height = h;
                img.style.transformOrigin =
                    new TransformOrigin(Length.Percent(50f), Length.Percent(50f));
                img.style.rotate = new Rotate(new Angle(90f));
                float centerX = item.PosX + h * 0.5f;
                float centerY = item.PosY + w * 0.5f;
                img.style.left = centerX - w * 0.5f;
                img.style.top = centerY - h * 0.5f;
            }
            else
            {
                img.style.width = w;
                img.style.height = h;
                img.style.left = item.PosX;
                img.style.top = item.PosY;
            }
        }

        /// <summary>
        /// 计算最优排布（委托给 AtlasLayoutCalculator）
        /// </summary>
        private bool CalculateBestLayout(int maxW, int maxH, int padding, int shapePadding,
            out int atlasWidth, out int atlasHeight, out List<Vector2Int> positions)
        {
            bool allowRotation = m_ToggleAllowRotation != null && m_ToggleAllowRotation.value;
            string heuristic = m_PopupHeuristics?.value;
            string packMode = m_PopupPack?.value ?? "Best";
            int innerPadding = m_IntInnerPadding != null ? m_IntInnerPadding.value : 0;
            int divisorX = m_IntDivisorX != null && m_IntDivisorX.value > 0 ? m_IntDivisorX.value : 1;
            int divisorY = m_IntDivisorY != null && m_IntDivisorY.value > 0 ? m_IntDivisorY.value : 1;
            int fixedW = ParseDropdownInt(m_PopupFixedWidth, 0);
            int fixedH = ParseDropdownInt(m_PopupFixedHeight, 0);
            bool forceSquare = m_ToggleForceSquare != null && m_ToggleForceSquare.value;
            bool autoAlias = m_ToggleAutoAlias == null || m_ToggleAutoAlias.value;
            string sizeConstraints = m_PopupSizeConstraints?.value ?? "POT";
            bool forceWordAligned = m_ToggleForceWord != null && m_ToggleForceWord.value;
            int extrude = m_IntExtrude != null ? m_IntExtrude.value : 0;

            return m_LayoutCalculator.CalculateBestLayout(
                m_SpriteListManager.Items, maxW, maxH, padding, shapePadding,
                allowRotation, heuristic,
                out atlasWidth, out atlasHeight, out positions,
                innerPadding, divisorX, divisorY, fixedW, fixedH, forceSquare, packMode, autoAlias,
                sizeConstraints, forceWordAligned, extrude);
        }

        /// <summary>
        /// 解析下拉框中的整数值，解析失败或为空时返回默认值
        /// </summary>
        private static int ParseDropdownInt(DropdownField popup, int defaultValue)
        {
            if (popup == null || string.IsNullOrEmpty(popup.value)) return defaultValue;
            return int.TryParse(popup.value.Trim(), out int v) ? v : defaultValue;
        }

        // ===== 参数变更自动刷新预览的重载辅助 =====
        private void RegisterReloadCallback(DropdownField field)
        {
            if (field != null) field.RegisterValueChangedCallback(_ => GeneratePreviewImage());
        }

        private void RegisterReloadCallback(IntegerField field)
        {
            if (field != null) field.RegisterValueChangedCallback(_ => GeneratePreviewImage());
        }

        private void RegisterReloadCallback(Toggle field)
        {
            if (field != null) field.RegisterValueChangedCallback(_ => GeneratePreviewImage());
        }

        /// <summary>
        /// 计算下一个 2 的幂（POT 对齐，委托给工具类）
        /// </summary>
        private int NextPOT(int value)
        {
            return AtlasPathUtils.NextPOT(value);
        }

        /// <summary>
        /// 浏览选择一个文件，回填文件名并记录其所在目录为输出目录
        /// </summary>
        private void BrowseOutputFile(TextField target, string extension)
        {
            if (target == null) return;

            string dirFull = string.IsNullOrEmpty(m_LastOutputDir) ? "Assets" : AssetPathToFullPath(m_LastOutputDir);
            string path = EditorUtility.OpenFilePanel("选择文件", dirFull ?? "Assets", extension);
            if (string.IsNullOrEmpty(path)) return;

            target.value = System.IO.Path.GetFileName(path);

            // 记录所在目录作为输出目录
            string fullDir = System.IO.Path.GetDirectoryName(path)?.Replace('\\', '/');
            if (!string.IsNullOrEmpty(fullDir))
            {
                string assetDir = FullPathToAssetPath(fullDir);
                if (!string.IsNullOrEmpty(assetDir)) m_LastOutputDir = assetDir;
            }
        }

        private string PathToAssetPath(string fullPath)
        {
            int index = fullPath.IndexOf("Assets", System.StringComparison.Ordinal);
            if (index >= 0)
                return fullPath.Substring(index).Replace('\\', '/');
            return fullPath;
        }

        #endregion
    }

}

