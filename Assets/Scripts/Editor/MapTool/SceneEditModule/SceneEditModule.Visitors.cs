/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SceneEditModule.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景编辑模块 - 字段与属性声明部分
 ***************************************************************/

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Editor.MapEditor;
using System.Linq;
using Honor.Editor.MapTool.PsdImport;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 场景编辑模块 - 字段与属性声明
    /// </summary>
    public partial class SceneEditModule
    {
        #region 字段
        /// <summary>
        /// 模块根节点
        /// </summary>
        private VisualElement m_Root;
        /// <summary>
        /// 场景视图容器
        /// </summary>
        private VisualElement m_SceneView;
        /// <summary>
        /// IMGUI渲染容器（场景绘制入口）
        /// </summary>
        private IMGUIContainer m_GuiContainer;
        /// <summary>
        /// 状态栏管理器（UI 引用 + 增量刷新 + 临时消息）
        /// </summary>
        private readonly EditorStatusBar m_StatusBar = new EditorStatusBar();
        /// <summary>
        /// 地图预制体选择框
        /// </summary>
        private ObjectField m_MapPrefabField;
        /// <summary>
        /// 场景名称输入框
        /// </summary>
        private TextField m_SceneNameField;
        /// <summary>
        /// 场景ID输入框
        /// </summary>
        private TextField m_SceneIdField;
        /// <summary>
        /// 地图宽度输入框
        /// </summary>
        private TextField m_WidthField;
        /// <summary>
        /// 地图高度输入框
        /// </summary>
        private TextField m_HeightField;
        /// <summary>
        /// 摆放模式启用开关（勾选启用当前形状绘制）
        /// </summary>
        private Toggle m_PlacementEnabledToggle;
        /// <summary>
        /// 绘制形状下拉框（矩形/菱形，默认菱形）
        /// </summary>
        private DropdownField m_DrawShapeField;
        /// <summary>
        /// 当前绘制形状 code（DrawShape.Rect / Diamond），来自 EditorPrefs 持久化配置
        /// </summary>
        private string m_DrawShape = MapToolConstants.DrawShapeDefault;
        /// <summary>
        /// DrawClear 启用开关（勾选启用当前区域类型绘制）
        /// </summary>
        private Toggle m_DrawClearEnabledToggle;
        /// <summary>
        /// 区域类型下拉框（摆放区/禁区/通用区，默认摆放区）
        /// </summary>
        private DropdownField m_RegionTypeField;
        /// <summary>
        /// 当前区域类型 code（RegionType.Place / Forbid / Common），来自 EditorPrefs 持久化配置
        /// </summary>
        private string m_RegionType = MapToolConstants.RegionTypeDefault;
        // 地图数据
        /// <summary>
        /// 鼠标世界坐标
        /// </summary>
        private Vector2 m_MousePos;
        /// <summary>
        /// 鼠标所在格子列
        /// </summary>
        private int m_TileX;
        /// <summary>
        /// 鼠标所在格子行
        /// </summary>
        private int m_TileY;
        /// <summary>
        /// 当前缩放倍率
        /// </summary>
        private float m_Zoom = 1.0f;
        /// <summary>
        /// 地图保存路径
        /// </summary>
        private string m_MapSavePath = MapToolConstants.MapSaveDir + "/";
        // 地图尺寸
        /// <summary>
        /// 地图编辑画布宽度（像素）；默认与单关卡基准一致，加载 JSON / 背景图后以其宽高为准
        /// </summary>
        private int m_MapWidth = MapToolConstants.LevelPixelSize;
        /// <summary>
        /// 地图编辑画布高度（像素）；默认与单关卡基准一致，加载 JSON / 背景图后以其宽高为准
        /// </summary>
        private int m_MapHeight = MapToolConstants.LevelPixelSize;
        // 配置尺寸（按已解锁分区包围盒计算，保存到 JSON 的宽高）
        /// <summary>
        /// 配置地图宽度（按已解锁分区包围盒计算，用于JSON保存）
        /// </summary>
        private int m_ConfigMapWidth = MapToolConstants.LevelPixelSize;
        /// <summary>
        /// 配置地图高度（按已解锁分区包围盒计算，用于JSON保存）
        /// </summary>
        private int m_ConfigMapHeight = MapToolConstants.LevelPixelSize;
        // 章节配置表数据（场景名/场景ID/分区配置来源）
        /// <summary>
        /// 章节配置表数据（场景名/场景ID/分区配置来源）
        /// </summary>
        private readonly List<TableChapterEditor> m_ChapterDataList = new List<TableChapterEditor>();

        // ---TODO 双格式保存：当前加载章节的上下文缓存（LoadChapterMap 匹配到章节时赋值）
        /// <summary>当前加载章节ID（未匹配到配置表章节时用加载入参 chapterId 兜底）</summary>
        private string m_CurrentChapterId = string.Empty;

        /// <summary>当前加载章节的配置表行数据（未匹配到时为 null）</summary>
        private TableChapterEditor m_CurrentChapterData;
        // 地图拖动
        /// <summary>
        /// 地图拖动偏移
        /// </summary>
        private Vector2 m_MapOffset = Vector2.zero;
        /// <summary>
        /// 是否正在拖动地图
        /// </summary>
        private bool m_IsDragging = false;
        /// <summary>
        /// 拖动起始鼠标位置
        /// </summary>
        private Vector2 m_DragStartPos = Vector2.zero;
        /// <summary>
        /// 拖动起始地图偏移
        /// </summary>
        private Vector2 m_DragStartOffset = Vector2.zero;
        // 地图预制体
        /// <summary>
        /// 当前地图预制体
        /// </summary>
        private GameObject m_MapPrefab;
        // 图层显示开关（与层级档八层口径一致：background/greenery/road/building/prop/character/foreground/bubble）
        /// <summary>
        /// 摆放网格层显示开关（非层级覆盖）
        /// </summary>
        private bool m_ShowPlaceGrid = true;
        /// <summary>
        /// 地图底图显示开关（非层级覆盖）
        /// </summary>
        private bool m_ShowMapLayer = true;
        /// <summary>
        /// 背景层（background）显示开关
        /// </summary>
        private bool m_ShowBackgroundLayer = true;
        /// <summary>
        /// 绿化层（greenery）显示开关
        /// </summary>
        private bool m_ShowGreeneryLayer = true;
        /// <summary>
        /// 道路市政层（road）显示开关
        /// </summary>
        private bool m_ShowRoadLayer = true;
        /// <summary>
        /// 建筑层（building）显示开关
        /// </summary>
        private bool m_ShowBuildingLayer = true;
        /// <summary>
        /// 道具层（prop）显示开关
        /// </summary>
        private bool m_ShowObjectLayer = true;
        /// <summary>
        /// 角色/载具层（character，含旧猫层）显示开关
        /// </summary>
        private bool m_ShowCharacterLayer = true;
        /// <summary>
        /// 前景近景层（foreground）显示开关
        /// </summary>
        private bool m_ShowForegroundLayer = true;
        /// <summary>
        /// 气泡层（bubble）显示开关
        /// </summary>
        private bool m_ShowBubblesLayer = true;
        /// <summary>
        /// 包围盒调试开关（非层级覆盖）
        /// </summary>
        private bool m_ShowBoundingBox = false;
        /// <summary>
        /// 路径网格默认关闭（非层级覆盖）
        /// </summary>
        private bool m_ShowPathGrid = false;
        /// <summary>
        /// 摆放网格层开关控件
        /// </summary>
        private Toggle m_PlaceGridToggle;
        /// <summary>
        /// 地图底图开关控件（非层级覆盖）
        /// </summary>
        private Toggle m_MapLayerToggle;
        /// <summary>
        /// 背景层开关控件（background）
        /// </summary>
        private Toggle m_BackgroundLayerToggle;
        /// <summary>
        /// 绿化层开关控件（greenery）
        /// </summary>
        private Toggle m_GreeneryLayerToggle;
        /// <summary>
        /// 道路市政层开关控件（road）
        /// </summary>
        private Toggle m_RoadLayerToggle;
        /// <summary>
        /// 建筑层开关控件（building）
        /// </summary>
        private Toggle m_BuildingLayerToggle;
        /// <summary>
        /// 道具层开关控件（prop）
        /// </summary>
        private Toggle m_ObjectLayerToggle;
        /// <summary>
        /// 角色/载具层开关控件（character）
        /// </summary>
        private Toggle m_CharacterLayerToggle;
        /// <summary>
        /// 前景近景层开关控件（foreground）
        /// </summary>
        private Toggle m_ForegroundLayerToggle;
        /// <summary>
        /// 气泡层开关控件（bubble）
        /// </summary>
        private Toggle m_BubblesLayerToggle;
        /// <summary>
        /// 包围盒调试开关控件（非层级覆盖）
        /// </summary>
        private Toggle m_BoundingBoxToggle;
        /// <summary>
        /// 路径网格开关控件（非层级覆盖）
        /// </summary>
        private Toggle m_PathGridToggle;
        // 路径点列表
        /// <summary>
        /// 路径点集合（世界坐标，吸附32px网格）
        /// </summary>
        private List<Vector2> m_PathPoints = new List<Vector2>();
        /// <summary>
        /// 是否处于路径编辑模式
        /// </summary>
        private bool m_IsEditingPath = false;
        // 关卡相关
        /// <summary>
        /// 当前关卡ID
        /// </summary>
        private string m_CurrentLevelId = "1";
        /// <summary>
        /// 场景中实例化的 ZoneRoot 引用（用于增删物体后刷新场景同步）
        /// </summary>
        private RectTransform m_ZoneRootInstance;
        // 关卡配置表（关卡ID -> 背景图名）
        /// <summary>
        /// 关卡背景配置表（关卡ID -> 背景图名）
        /// </summary>
        private Dictionary<string, string> m_LevelBgDict = new Dictionary<string, string>();
        // 已放置的物体
        /// <summary>
        /// 已放置物体列表
        /// </summary>
        private List<PlacedObject> m_PlacedObjects = new List<PlacedObject>();

        // ---TODO 交通规则：编辑器工作态交通数据（落盘进 MapConfigData 交通子表）
        /// <summary>红绿灯工作态集合</summary>
        private readonly List<MapTrafficLightData> m_TrafficLights = new List<MapTrafficLightData>();

        /// <summary>斑马线工作态集合</summary>
        private readonly List<MapCrosswalkData> m_Crosswalks = new List<MapCrosswalkData>();

        /// <summary>矩形区域回显工作态集合</summary>
        private readonly List<MapRegionData> m_Regions = new List<MapRegionData>();

        /// <summary>人行道可走格工作态集合</summary>
        private readonly List<MapGridCellData> m_SidewalkCells = new List<MapGridCellData>();

        /// <summary>车辆行驶格工作态集合</summary>
        private readonly List<MapGridCellData> m_RoadCells = new List<MapGridCellData>();

        /// <summary>
        /// 模板预制体缓存（类型 -> 预制体），避免每次场景同步重复 LoadAssetAtPath
        /// </summary>
        private readonly Dictionary<MapObjectType, GameObject> _templatePrefabCache = new Dictionary<MapObjectType, GameObject>();

        /// <summary>
        /// Sprite 资产缓存（资产路径 -> Sprite），避免同一张图在多次同步时重复加载
        /// </summary>
        private readonly Dictionary<string, UnityEngine.Sprite> _spriteCache = new Dictionary<string, UnityEngine.Sprite>();

        /// <summary>
        /// 背景纹理缓存（资产路径 -> Texture2D），避免每帧绘制背景时重复走 AssetDatabase
        /// </summary>
        private readonly Dictionary<string, Texture2D> _bgTextureCache = new Dictionary<string, Texture2D>();

        /// <summary>
        /// PSD/PSB 导入模块
        /// </summary>
        private PsdImportModule m_PsdImportModule;
        // 当前选中要放置的物体
        /// <summary>
        /// 当前选中放置的物体ID
        /// </summary>
        private string m_SelectedObjectId;
        /// <summary>
        /// 当前选中放置的物体名称
        /// </summary>
        private string m_SelectedObjectName;
        /// <summary>
        /// 当前选中的放置物体类型
        /// </summary>
        private MapObjectType m_SelectedObjectType = MapObjectType.Prop;
        /// <summary>
        /// 待放置物体预览（鼠标跟随）
        /// </summary>
        private PlacedObject m_PendingPlace;
        /// <summary>
        /// 预览位置是否合法
        /// </summary>
        private bool m_PreviewValid;
        /// <summary>
        /// 预览不合法原因
        /// </summary>
        private string m_PreviewReason;
        /// <summary>
        /// 地图预制体变化事件
        /// </summary>
        public System.Action<string> OnMapPrefabChanged;
        // ===== 对象编辑（路径 / 动画）=====
        /// <summary>
        /// 当前选中编辑的物体（人物）
        /// </summary>
        private PlacedObject m_EditTarget;
        /// <summary>
        /// 路径绘制模式开关
        /// </summary>
        private bool m_IsPathDrawing;
        /// <summary>
        /// 本次按下后是否发生拖动（区分 点击选中 / 拖动）
        /// </summary>
        private bool m_HasMoved;
        /// <summary>
        /// 鼠标按下位置
        /// </summary>
        private Vector2 m_MouseDownPos;
        /// <summary>
        /// 鼠标按下时命中的物体（抬起时若未拖动则选中）
        /// </summary>
        private PlacedObject m_PotentialClickObject;
        /// <summary>
        /// 路径点列表选中索引
        /// </summary>
        private int m_SelectedPathPointIndex = -1;
        // 物体拖动
        /// <summary>
        /// 当前拖动的物体
        /// </summary>
        private PlacedObject m_DraggingObject;
        /// <summary>
        /// 拖动时鼠标与物体左上角的偏移
        /// </summary>
        private Vector2 m_DragOffset;
        // 对象编辑面板 UI 引用
        /// <summary>
        /// 对象编辑面板
        /// </summary>
        private VisualElement m_ObjectEditPanel;
        /// <summary>
        /// 对象编辑标题
        /// </summary>
        private Label m_ObjEditTitle;
        /// <summary>
        /// 可编辑路径切页按钮
        /// </summary>
        private Button m_TabEditPath;
        /// <summary>
        /// 编辑动画切页按钮
        /// </summary>
        private Button m_TabEditAnim;
        /// <summary>
        /// 路径编辑内容区
        /// </summary>
        private VisualElement m_PathEditContent;
        /// <summary>
        /// 动画编辑内容区
        /// </summary>
        private VisualElement m_AnimEditContent;
        /// <summary>
        /// 路径绘制按钮
        /// </summary>
        private Button m_BtnPathDraw;
        /// <summary>
        /// 路径撤销按钮
        /// </summary>
        private Button m_BtnPathUndo;
        /// <summary>
        /// 路径清空按钮
        /// </summary>
        private Button m_BtnPathClear;
        /// <summary>
        /// 清除目标按钮
        /// </summary>
        private Button m_BtnClearTarget;
        /// <summary>
        /// 路径点列表
        /// </summary>
        private ScrollView m_PathPointList;
        // 动画切页字段
        /// <summary>
        /// 动画名称输入框
        /// </summary>
        private TextField m_AnimNameField;
        /// <summary>
        /// 动画循环开关
        /// </summary>
        private Toggle m_AnimLoopToggle;
        /// <summary>
        /// 动画速度输入框
        /// </summary>
        private TextField m_AnimSpeedField;
        // ===== 底部属性面板（选中物体时显示）=====
        /// <summary>
        /// 模板ID输入框
        /// </summary>
        private TextField m_TemplateIdField;
        /// <summary>
        /// tag输入框
        /// </summary>
        private TextField m_TagField;
        /// <summary>
        /// 格子坐标输入框
        /// </summary>
        private TextField m_GridPosField;
        /// <summary>
        /// 建筑模板输入框
        /// </summary>
        private TextField m_BuildingTemplateField;
        /// <summary>
        /// 格子前缀输入框
        /// </summary>
        private TextField m_GridPrefixField;
        /// <summary>
        /// 交互状态输入框
        /// </summary>
        private TextField m_InteractStateField;
        /// <summary>
        /// 删除物体按钮
        /// </summary>
        private Button m_DeleteObjectBtn;
        /// <summary>
        /// 发布按钮
        /// </summary>
        private Button m_PublishBtn;
        /// <summary>
        /// 底部属性面板（选中物体时显示）
        /// </summary>
        private VisualElement m_PropertyPanel;
        /// <summary>
        /// 属性面板右侧操作卡片（变换区 + 交通规则区挂载点）
        /// </summary>
        private VisualElement m_PropertyActionArea;
        /// <summary>
        /// 属性流内变换/交通区挂载容器（删除按钮行之前）
        /// </summary>
        private VisualElement m_PropertyActionHost;
        /// <summary>
        /// 收集物专属变换容器（层级/比例/角度三行），仅勾选收集时显示
        /// </summary>
        private VisualElement m_CollectibleAction;
        // 新增属性面板字段
        private TextField m_SortOrderField;
        private DropdownField m_InteractTypeField;
        private Toggle m_CollisionToggle;
        private DropdownField m_DialogBubbleField;
        // ---TODO 气泡配置表通用化：气泡ID输入框（引用 Tables.Bubble，<=0 不导出）
        private IntegerField m_BubbleIdField;
        private DropdownField m_FacingField;
        private DropdownField m_DisplayLayerField;
        private Toggle m_ShadowToggle;
        private Toggle m_CollectibleToggle;
        /// <summary>收集标记分组（标签+开关），仅可收集物件显示</summary>
        private VisualElement m_CollectibleGroup;
        /// <summary>交通标记分组（标签+开关），仅参与交通物件显示</summary>
        private VisualElement m_TrafficGroup;
        // 变换（比例/角度）回显标签
        private Label m_ScaleLabel;
        private Label m_RotationLabel;
        #endregion

        #region 属性
        /// <summary>
        /// 模块根节点
        /// </summary>
        public VisualElement Root => m_Root;
        #endregion

        #region 状态栏统计（借鉴 Tilemap 网格口径：占用格子去重计数 + 网格行列数）

        /// <summary>当前已占用格子去重计数（摆放物跨格展开，不含人行道/道路格）</summary>
        protected int OccupiedCellCount => MapToolEditorUtils.CollectOccupiedCells(m_PlacedObjects).Count;

        /// <summary>当前网格总列数（按画布宽 / GridSize）</summary>
        protected int GridCols => MapToolEditorUtils.GridCols(m_MapWidth);

        /// <summary>当前网格总行数（按画布高 / GridSize）</summary>
        protected int GridRows => MapToolEditorUtils.GridRows(m_MapHeight);

        #endregion

    }
}