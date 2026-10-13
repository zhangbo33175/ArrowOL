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
        // 状态栏 Labels
        /// <summary>
        /// 状态栏缩放显示
        /// </summary>
        private Label m_ZoomLabel;
        /// <summary>
        /// 状态栏鼠标X坐标显示
        /// </summary>
        private Label m_MouseXLabel;
        /// <summary>
        /// 状态栏鼠标Y坐标显示
        /// </summary>
        private Label m_MouseYLabel;
        /// <summary>
        /// 状态栏格子X显示
        /// </summary>
        private Label m_TileXLabel;
        /// <summary>
        /// 状态栏格子Y显示
        /// </summary>
        private Label m_TileYLabel;
        /// <summary>
        /// 状态栏格子计数显示
        /// </summary>
        private Label m_TileCountLabel;
        /// <summary>
        /// 保存路径按钮
        /// </summary>
        private Button m_SavePathBtn;
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
        /// 放置模式开关
        /// </summary>
        private Toggle m_PlaceModeToggle;
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
        /// 地图编辑画布宽度（像素）
        /// </summary>
        private int m_MapWidth = 1320;
        /// <summary>
        /// 地图编辑画布高度（像素）
        /// </summary>
        private int m_MapHeight = 990;
        // 配置尺寸（按已解锁分区包围盒计算，保存到 JSON 的宽高）
        /// <summary>
        /// 配置地图宽度（按已解锁分区包围盒计算，用于JSON保存）
        /// </summary>
        private int m_ConfigMapWidth = MapToolConstants.RegionPixelSize;
        /// <summary>
        /// 配置地图高度（按已解锁分区包围盒计算，用于JSON保存）
        /// </summary>
        private int m_ConfigMapHeight = MapToolConstants.RegionPixelSize;
        // 章节配置表数据（场景名/场景ID/分区配置来源）
        /// <summary>
        /// 章节配置表数据（场景名/场景ID/分区配置来源）
        /// </summary>
        private readonly List<TableChapterEditor> m_ChapterDataList = new List<TableChapterEditor>();
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
        // 图层显示开关
        /// <summary>
        /// 摆放网格层显示开关
        /// </summary>
        private bool m_ShowPlaceGrid = true;
        /// <summary>
        /// 地图层显示开关
        /// </summary>
        private bool m_ShowMapLayer = true;
        /// <summary>
        /// 建筑层显示开关
        /// </summary>
        private bool m_ShowBuildingLayer = true;
        /// <summary>
        /// 人物层显示开关
        /// </summary>
        private bool m_ShowCharacterLayer = true;
        /// <summary>
        /// 物件层显示开关
        /// </summary>
        private bool m_ShowObjectLayer = true;
        /// <summary>
        /// 背景层显示开关
        /// </summary>
        private bool m_ShowBackgroundLayer = true;
        /// <summary>
        /// 包围盒显示开关
        /// </summary>
        private bool m_ShowBoundingBox = false;
        /// <summary>
        /// 路径网格默认关闭
        /// </summary>
        private bool m_ShowPathGrid = false;
        /// <summary>
        /// 摆放网格层开关控件
        /// </summary>
        private Toggle m_PlaceGridToggle;
        /// <summary>
        /// 地图层开关控件
        /// </summary>
        private Toggle m_MapLayerToggle;
        /// <summary>
        /// 建筑层开关控件
        /// </summary>
        private Toggle m_BuildingLayerToggle;
        /// <summary>
        /// 人物层开关控件
        /// </summary>
        private Toggle m_CharacterLayerToggle;
        /// <summary>
        /// 物件层开关控件
        /// </summary>
        private Toggle m_ObjectLayerToggle;
        /// <summary>
        /// 背景层开关控件
        /// </summary>
        private Toggle m_BackgroundLayerToggle;
        /// <summary>
        /// 包围盒开关控件
        /// </summary>
        private Toggle m_BoundingBoxToggle;
        /// <summary>
        /// 路径网格开关控件
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
        // 分区网格（行列数与分区ID）
        /// <summary>
        /// 分区行数
        /// </summary>
        private int m_RegionRows = 3;
        /// <summary>
        /// 分区列数
        /// </summary>
        private int m_RegionCols = 3;
        /// <summary>
        /// 实际分区数量
        /// </summary>
        private int m_RegionCount = 9;
        /// <summary>
        /// 每个分区的 ID
        /// </summary>
        private string[] m_RegionIds;
        // 关卡配置表（ID -> Level_bg）
        /// <summary>
        /// 关卡配置表（分区ID -> 背景图名）
        /// </summary>
        private Dictionary<string, string> m_LevelBgDict = new Dictionary<string, string>();
        // 已放置的物体
        /// <summary>
        /// 已放置物体列表
        /// </summary>
        private List<PlacedObject> m_PlacedObjects = new List<PlacedObject>();
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
        /// 地图分区解锁状态（动态大小）
        /// </summary>
        private bool[,] m_RegionUnlocked;
        /// <summary>
        /// 地图预制体变化事件
        /// </summary>
        public System.Action<string> OnMapPrefabChanged;
        // 状态栏上次值（用于优化更新）
        /// <summary>
        /// 上次缩放值（状态栏增量更新）
        /// </summary>
        private float m_LastZoom = -1;
        /// <summary>
        /// 上次鼠标X（状态栏增量更新）
        /// </summary>
        private float m_LastMouseX = float.MinValue;
        /// <summary>
        /// 上次鼠标Y（状态栏增量更新）
        /// </summary>
        private float m_LastMouseY = float.MinValue;
        /// <summary>
        /// 上次格子X（状态栏增量更新）
        /// </summary>
        private int m_LastTileX = int.MinValue;
        /// <summary>
        /// 上次格子Y（状态栏增量更新）
        /// </summary>
        private int m_LastTileY = int.MinValue;
        /// <summary>
        /// 上次保存路径（状态栏增量更新）
        /// </summary>
        private string m_LastSavePath = "";
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
        #endregion

        #region 属性
        /// <summary>
        /// 模块根节点
        /// </summary>
        public VisualElement Root => m_Root;
        #endregion

    }
}