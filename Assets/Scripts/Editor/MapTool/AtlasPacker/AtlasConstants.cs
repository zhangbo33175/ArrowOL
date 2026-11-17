/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AtlasConstants.cs
 * author:    云毅
 * created:   2026
 * descrip:   常量定义（清理硬代码）
 ***************************************************************/

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// 常量配置类
    /// 集中管理所有硬编码的字符串、数值、路径
    /// </summary>
    internal static class AtlasConstants
    {
        // ===== 窗口配置（参考 TexturePacker 宽屏三栏布局）=====
        public const int WindowWidth = 1600;
        public const int WindowHeight = 900;
        public const int WindowMinWidth = 1100;
        public const int WindowMinHeight = 650;
        public const string WindowTitle = "Atlas 打包工具";
        public const string MenuItemPath = "Honor/地图工具/Atlas 打包工具 (UI Toolkit)";

        // ===== 文件路径 =====
        public const string UxmlPath = "Assets/Scripts/Editor/MapTool/AtlasPacker/AtlasPackerWindow.uxml";
        public const string UssPath = "Assets/Scripts/Editor/MapTool/AtlasPacker/AtlasPackerWindow.uss";
        public const string IconDir = "Assets/Scripts/Editor/MapTool/AtlasPacker/Icons/";
        public const string ProjectFileExtension = ".atlasproj";
        public const string ExportFileExtension = ".txt";
        public const string DefaultFolder = MapToolConstants.MapTextureRoot + "/Chapter1";

        /// <summary>最大尺寸下拉默认选项（"1024"）</summary>
        public const string DefaultMaxSizeOption = "1024";

        /// <summary>图集输出根目录（PNG/JSON 默认落盘目录，同时也是项目保存/打开的默认目录）</summary>
        public const string DefaultOutputDir = "Assets/Res/Atlas";

        /// <summary>导出数据文件名默认值</summary>
        public const string DefaultDataFilename = "NewAtlas.txt";

        /// <summary>导出纹理文件名默认值</summary>
        public const string DefaultTextureFilename = "NewAtlasTex.png";

        /// <summary>项目文件默认文件名（不含扩展名）</summary>
        public const string DefaultProjectFileName = "NewAtlas";

        /// <summary>裁剪模式：裁剪透明边（TexturePacker trim 模式 token）</summary>
        public const string TrimModeTrim = "Trim";

        /// <summary>裁剪模式：不裁剪（TexturePacker trim 模式 token）</summary>
        public const string TrimModeNone = "None";

        // ===== 输出/布局参数 token（TexturePacker 语义，项目文件与界面下拉共用）=====

        /// <summary>数据格式：Unity3D</summary>
        public const string TokenUnity3D = "Unity3D";
        /// <summary>纹理格式：PNG</summary>
        public const string TokenPng = "PNG";
        /// <summary>像素格式：RGBA8888</summary>
        public const string TokenRgba8888 = "RGBA8888";
        /// <summary>抖动算法：最近邻</summary>
        public const string TokenNearestNeighbour = "NearestNeighbour";
        /// <summary>布局算法：MaxRects</summary>
        public const string TokenMaxRects = "MaxRects";
        /// <summary>尺寸约束：2 的幂</summary>
        public const string TokenPot = "POT";
        /// <summary>缩放模式：平滑</summary>
        public const string TokenSmooth = "Smooth";
        /// <summary>启发式/打包策略：Best</summary>
        public const string TokenBest = "Best";

        // ===== UI 元素 ID（UXML 中的名字）=====
        public const string TogForceSquare = "toggleForceSquare";
        public const string TogForceWord = "toggleForceWord";
        public const string TogAllowRotation = "toggleAllowRotation";
        public const string TogMultipack = "toggleMultipack";
        public const string TogPremultiply = "togglePremultiply";
        public const string TogReduceBorder = "toggleReduceBorder";
        public const string TogShapeOutlines = "toggleShapeOutlines";
        public const string TogAutoAlias = "toggleAutoAlias";
        public const string TogHeuristicMask = "toggleHeuristicMask";
        public const string TogTrimNames = "toggleTrimNames";

        public const string PopMaxWidth = "popupMaxWidth";
        public const string PopMaxHeight = "popupMaxHeight";
        public const string PopFixedWidth = "popupFixedWidth";
        public const string PopFixedHeight = "popupFixedHeight";

        public const string IntBorderPadding = "intBorderPadding";
        public const string IntShapePadding = "intShapePadding";
        public const string IntInnerPadding = "intInnerPadding";
        public const string IntExtrude = "intExtrude";
        public const string IntDivisorX = "intDivisorX";
        public const string IntDivisorY = "intDivisorY";
        public const string IntTrimThreshold = "intTrimThreshold";
        public const string IntDpi = "intDpi";

        public const string FloatScale = "floatScale";
        public const string SliderPngOpt = "sliderPngOpt";

        public const string PopDataFormat = "popupDataFormat";
        public const string PopTextureFormat = "popupTextureFormat";
        public const string PopImageFormat = "popupImageFormat";
        public const string PopDithering = "popupDithering";
        public const string PopSizeConstraints = "popupSizeConstraints";
        public const string PopAlgorithm = "popupAlgorithm";
        public const string PopHeuristics = "popupHeuristics";
        public const string PopPack = "popupPack";
        public const string PopTrimMode = "popupTrimMode";
        public const string PopScaleMode = "popupScaleMode";

        public const string TxtDataFilename = "txtDataFilename";
        public const string TxtTextureFile = "txtTextureFile";
        public const string TxtTexturePath = "txtTexturePath";

        public const string LblSpriteCount = "lblSpriteCount";
        public const string LblAtlasSize = "lblAtlasSize";
        public const string LblPreviewEmpty = "lblPreviewEmpty";
        public const string LblZoom = "lblZoom";
        public const string ImgPreview = "imgPreview";
        public const string PreviewContainer = "preview-image-container";
        public const string SpritesList = "sprites-list";

        // ===== 按钮 ID =====
        public const string BtnAddFolder = "btnAddFolder";
        public const string BtnAddSprites = "btnAddSprites";
        public const string BtnDelete = "btnDelete";
        public const string BtnPublish = "btnPublish";
        public const string BtnSave = "btnSave";
        public const string BtnSaveDefaults = "btnSaveDefaults";
        public const string BtnOpen = "btnOpen";
        public const string BtnNew = "btnNew";
        public const string BtnZoomIn = "btnZoomIn";
        public const string BtnZoomOut = "btnZoomOut";
        public const string BtnZoomReset = "btnZoomReset";
        public const string BtnZoomFit = "btnZoomFit";
        public const string BtnPvr = "btnPvr";
        public const string ZoomField = "zoomField";

        // ===== 默认值 =====
        public const int DefaultAtlasWidth = 1024;
        public const int DefaultAtlasHeight = 1024;
        public const float DefaultZoomScale = 1.0f;
        public const int NoSelectionIndex = -1;

        // ===== 默认选项列表 =====

        // ===== UI 显示文本 =====
        public const string EmptyTextureHint = "Empty texture";
        public const string SpriteCountFormat = "{0} sprites";
        public const string AtlasSizeFormat = "Size: {0}x{1} ({2}kB)";
        public const string ZoomFormat = "Zoom: {0:F0}%";
        public const string NoImageInFolder = "文件夹里没有找到图片:\n{0}";
        public const string PleaseAddImagesFirst = "请先添加图片";
        public const string CannotLayoutError = "无法排布，请检查图片数量和尺寸";
        public const string ExportSuccessMessage = "导出完成！\nPNG: {0}\nJSON: {1}";
        public const string ProjectLoadedMessage = "项目已加载，共 {0} 张图片";
        public const string ProjectSavedMessage = "项目已保存";
    }
}
