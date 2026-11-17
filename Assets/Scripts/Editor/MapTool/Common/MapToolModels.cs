/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  MapToolModels.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器数据模型
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Editor.MapTool
{
    #region 编辑器运行时数据

    /// <summary>
    /// 物体模板（物品元件编辑器中配置）
    /// 同一类物体的公共属性只配置一次，场景中放置时通过TemplateId引用
    /// </summary>
    [Serializable]
    public class ObjectTemplate
    {
        #region 基础标识

        /// <summary>模板唯一ID</summary>
        public string TemplateId = string.Empty;

        /// <summary>模板显示名称</summary>
        public string TemplateName = string.Empty;

        /// <summary>物体类型</summary>
        public MapObjectType Type = MapObjectType.Prop;

        /// <summary>预览图资源路径</summary>
        public string ImagePath = string.Empty;

        /// <summary>落地预制体资源名（对应 Res/Prefabs/Model 下的预制体文件名）</summary>
        public string ResName = string.Empty;

        /// <summary>是否为内置模板（从 Excel 读取的为 true，用户通过编辑器添加的为 false，内置模板不可删除）</summary>
        public bool IsBuiltIn = true;

        #endregion

        #region 动画属性

        /// <summary>默认动画名称</summary>
        public string AnimName = "idle";

        /// <summary>是否循环播放动画</summary>
        public bool Loop = true;

        /// <summary>动画播放速度倍率</summary>
        public float Speed = 1f;

        #endregion

        #region 交互属性

        /// <summary>是否可与玩家交互</summary>
        public bool Interactable = true;

        /// <summary>交互类型：none/enter/dialog/pickup/view</summary>
        public string InteractType = TemplateInteractType.None;

        /// <summary>对话气泡内容</summary>
        public string DialogBubble = string.Empty;

        /// <summary>气泡配置表ID（引用 Tables.Bubble；<=0 表示无气泡，放置实例时作为默认值继承）</summary>
        public int BubbleId = 0;

        #endregion

        #region 渲染与物理属性

        /// <summary>渲染层级排序（八层基线+层内槽位全局键，与实例同坐标系；0 表示未配置）</summary>
        public int SortOrder = 0;

        /// <summary>显示层键（渲染前后档位；空=跟随类型归属层，见 MapLayerCatalog.ResolveLayerKey）。独立显示档不改变素材归属与挂载节点，与实例 DisplayLayer 同口径</summary>
        public string DisplayLayer = string.Empty;

        /// <summary>是否启用Y坐标自动深度排序</summary>
        public bool YSortEnabled = true;

        /// <summary>Y坐标深度排序的基准偏移</summary>
        public float YSortOffset = 0f;

        /// <summary>是否有碰撞体积</summary>
        public bool CollisionEnabled = true;

        /// <summary>初始朝向：up/down/left/right</summary>
        public string FacingDirection = TemplateFacing.Down;

        /// <summary>是否显示地面阴影</summary>
        public bool ShadowEnabled = true;

        #endregion

        #region 移动与循环属性

        /// <summary>是否可移动</summary>
        public bool IsMovable = false;

        /// <summary>移动动画名称</summary>
        public string MoveAnimName = "walk";

        /// <summary>循环行驶：离开地图后重新开始的延迟时间（秒）</summary>
        public float LoopRestartDelay = 3f;

        /// <summary>离开地图时是否隐藏</summary>
        public bool HideWhenOffMap = true;

        #endregion

        #region 传送门属性

        /// <summary>是否为传送门</summary>
        public bool IsTeleport = false;

        /// <summary>目标场景ID</summary>
        public string TeleportTargetScene = "";

        /// <summary>目标位置X</summary>
        public float TeleportTargetX = 0;

        /// <summary>目标位置Y</summary>
        public float TeleportTargetY = 0;

        #endregion

        #region 收集物属性

        /// <summary>是否为可收集物</summary>
        public bool IsCollectible = false;

        /// <summary>收集物重生时间（秒，0表示不重生）</summary>
        public float RespawnTime = 0;

        #endregion

        #region 交通规则属性

        /// <summary>
        /// 交通类别键（见 <see cref="TemplateTrafficCategory"/>）：none 不参与 / streetlight 路灯 /
        /// vehicle 汽车 / pedestrian 人物 / road 马路路面 / crosswalk 斑马线。
        /// 仅作为模板默认分类，放置实例时按类别继承交通角色/载具标记，实例可在右栏单独调整。
        /// </summary>
        public string TrafficCategory = TemplateTrafficCategory.None;

        #endregion

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ObjectTemplate()
        {
        }

        #endregion
    }

    /// <summary>
    /// 已放置在地图上的物体实例（编辑器运行时数据）
    /// 只保存位置、大小等实例属性，模板属性通过TemplateId引用
    /// </summary>
    [Serializable]
    public class PlacedObject
    {
        #region 基础标识

        /// <summary>实例唯一ID（自动生成）</summary>
        public string Id = string.Empty;

        /// <summary>引用的模板ID</summary>
        public string TemplateId = string.Empty;

        /// <summary>物体显示名称</summary>
        public string Name = string.Empty;

        /// <summary>物体类型</summary>
        public MapObjectType Type = MapObjectType.Prop;

        #endregion

        #region 位置与尺寸

        /// <summary>左上角世界坐标</summary>
        public Vector2 Position = Vector2.zero;

        /// <summary>宽度（格子数）</summary>
        public int Width = 1;

        /// <summary>高度（格子数）</summary>
        public int Height = 1;

        /// <summary>PSD 图层原始像素宽（&gt;0 时场景/配置优先按像素尺寸还原，不再走格子换算）</summary>
        public float PixelWidth = 0;

        /// <summary>PSD 图层原始像素高（&gt;0 时场景/配置优先按像素尺寸还原）</summary>
        public float PixelHeight = 0;

        /// <summary>是否拉伸到配置尺寸（false=保持原图比例，按原生尺寸 SetNativeSize）</summary>
        public bool Stretch = false;

        /// <summary>PSD 图层类型键（background/greenery/road/building/prop/character/foreground/bubble，见 MapLayerCatalog）</summary>
        public string LayerType = MapLayerCatalog.Prop;

        /// <summary>显示层键（渲染前后档位；空=跟随归属层 LayerType）。独立显示档不改变素材归属与挂载节点</summary>
        public string DisplayLayer = string.Empty;

        #endregion

        #region 动画属性

        /// <summary>Spine 动画资源路径（SkeletonDataAsset）</summary>
        public string SpineAssetPath = string.Empty;

        /// <summary>动画名称（默认待机动画，口径见 MapToolConstants.DefaultAnimName）</summary>
        public string AnimName = MapToolConstants.DefaultAnimName;

        /// <summary>是否循环播放动画</summary>
        public bool Loop = true;

        /// <summary>播放速度倍率</summary>
        public float Speed = 1f;

        #endregion

        #region 交互属性

        /// <summary>是否可交互</summary>
        public bool Interactable = true;

        /// <summary>交互类型：none/enter/dialog/pickup/view</summary>
        public string InteractType = TemplateInteractType.None;

        /// <summary>对话气泡内容</summary>
        public string DialogBubble = string.Empty;

        // ---TODO 气泡配置表通用化：引用 Tables.Bubble（>0 生效，<=0 不输出）
        /// <summary>气泡配置表ID（引用 Tables.Bubble；<=0 表示无气泡，Lua 导出时整行省略）</summary>
        public int bubbleId = 0;

        /// <summary>是否允许与其他物体重叠覆盖</summary>
        public bool AllowOverlap = false;

        #endregion

        #region 渲染与物理属性

        /// <summary>渲染层级排序（八层基线+层内槽位全局键；0 表示未配置，落位时按所属显示档基线归一）</summary>
        public int SortOrder = 0;

        /// <summary>是否启用Y坐标自动深度排序（启用后根据Y坐标自动计算前后关系）</summary>
        public bool YSortEnabled = true;

        /// <summary>Y坐标深度排序的基准偏移（越大越靠前，默认0）</summary>
        public float YSortOffset = 0f;

        /// <summary>旋转角度（度，绕 Z 轴顺时针为正；0=不旋转）</summary>
        public float Rotation = 0f;

        /// <summary>缩放倍率（1=原始比例；>1 放大，<1 缩小）</summary>
        public float Scale = 1f;

        /// <summary>是否有碰撞体积</summary>
        public bool CollisionEnabled = true;

        /// <summary>初始朝向：up/down/left/right</summary>
        public string FacingDirection = TemplateFacing.Down;

        /// <summary>是否显示地面阴影</summary>
        public bool ShadowEnabled = true;

        #endregion

        #region 收集物与NPC属性

        /// <summary>是否为可收集物</summary>
        public bool IsCollectible = false;

        /// <summary>收集物重生时间（秒）</summary>
        public float RespawnTime = 0;

        /// <summary>行走路径模式：once（单次）/ pingpong（往返）/ loop（循环）</summary>
        public string WalkMode = "once";

        /// <summary>行走路径点列表（世界坐标，吸附32px网格）</summary>
        public List<Vector2> PathPoints = new List<Vector2>();

        #endregion

        #region 事件系统

        /// <summary>触发事件类型：none/chat（聊天）/interact（交互）</summary>
        public string TriggerEventType = TemplateInteractType.None;

        /// <summary>触发目标ID（遇到哪个物体触发）</summary>
        public string TriggerTargetId = "";

        /// <summary>触发后停留时间（秒）</summary>
        public float TriggerStayTime = 2f;

        #endregion

        #region 物件移动

        /// <summary>是否可移动（建筑/物件也能移动）</summary>
        public bool IsMovable = false;

        /// <summary>移动到目标点的动画名称</summary>
        public string MoveAnimName = "walk";

        /// <summary>循环行驶：离开地图后重新开始的延迟时间（秒，0=立即重新开始）</summary>
        public float LoopRestartDelay = 3f;

        /// <summary>离开地图时是否隐藏</summary>
        public bool HideWhenOffMap = true;

        #endregion

        #region 传送门

        /// <summary>是否为传送门</summary>
        public bool IsTeleport = false;

        /// <summary>目标场景ID</summary>
        public string TeleportTargetScene = "";

        /// <summary>目标位置X（世界坐标）</summary>
        public float TeleportTargetX = 0;

        /// <summary>目标位置Y（世界坐标）</summary>
        public float TeleportTargetY = 0;

        #endregion

        #region 交通规则

        /// <summary>是否为载具（汽车/巴士/货车…）</summary>
        public bool IsVehicle = false;

        /// <summary>交通角色：pedestrian 行人 / vehicle 载具 / none 不参与</summary>
        public string TrafficRole = TemplateTrafficCategory.Pedestrian;

        /// <summary>车辆停止线 X（世界像素，仅载具）</summary>
        public float StopLineX = 0;

        /// <summary>车辆停止线 Y（世界像素，仅载具）</summary>
        public float StopLineY = 0;

        /// <summary>绑定红绿灯实例 ID（红灯在此等待）；兼容旧单值，多选结果见 <see cref="BoundLightIds"/></summary>
        public string BoundLightId = string.Empty;

        /// <summary>绑定红绿灯实例 ID 列表（多选；全选后即当前地图全部灯）</summary>
        public List<string> BoundLightIds = new List<string>();

        /// <summary>黄灯是否按红灯处理（仅车辆消费）</summary>
        public bool YellowTreatAsRed = true;

        #endregion

        #region 状态配置

        /// <summary>初始状态：idle/move/hidden</summary>
        public string InitialState = "idle";

        /// <summary>状态贴图配置（key=状态名, value=贴图路径）</summary>
        public List<StateSpriteConfig> StateSprites = new List<StateSpriteConfig>();

        /// <summary>状态动画配置（key=状态名, value=动画名）</summary>
        public List<StateAnimConfig> StateAnims = new List<StateAnimConfig>();

        #endregion

        #region 运行时引用

        /// <summary>预览图纹理（编辑器运行时使用，不序列化）</summary>
        [NonSerialized]
        public Texture PreviewTexture;

        #endregion

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public PlacedObject()
        {
            // 自动生成实例ID
            Id = Guid.NewGuid().ToString("N").Substring(0, 8);
        }

        /// <summary>
        /// 从模板同步配置（不改变位置和大小）
        /// </summary>
        /// <param name="template">模板对象</param>
        public void SyncFromTemplate(ObjectTemplate template)
        {
            if (template == null) return;

            // 同步基础属性
            Name = template.TemplateName;
            Type = template.Type;
            AnimName = template.AnimName;
            Loop = template.Loop;
            Speed = template.Speed;
            Interactable = template.Interactable;
            InteractType = template.InteractType;
            SortOrder = MapToolEditorUtils.SortOrderFromTemplate(this, template);
            DisplayLayer = template.DisplayLayer;
            CollisionEnabled = template.CollisionEnabled;
            DialogBubble = template.DialogBubble;
            bubbleId = template.BubbleId;
            FacingDirection = template.FacingDirection;
            ShadowEnabled = template.ShadowEnabled;
            IsCollectible = template.IsCollectible;
            RespawnTime = template.RespawnTime;

            // 同步扩展属性
            YSortEnabled = template.YSortEnabled;
            YSortOffset = template.YSortOffset;
            IsMovable = template.IsMovable;
            MoveAnimName = template.MoveAnimName;
            LoopRestartDelay = template.LoopRestartDelay;
            HideWhenOffMap = template.HideWhenOffMap;
            IsTeleport = template.IsTeleport;
            TeleportTargetScene = template.TeleportTargetScene;
            TeleportTargetX = template.TeleportTargetX;
            TeleportTargetY = template.TeleportTargetY;

            // 交通角色/载具标记：由模板交通类别重算，与放置时继承口径一致
            TrafficRole = TemplateTrafficCategory.ResolveTrafficRole(template.TrafficCategory);
            IsVehicle = TemplateTrafficCategory.IsVehicle(template.TrafficCategory);
        }

        #endregion
    }

    #endregion

    #region 地图配置JSON数据结构

    /// <summary>
    /// 地图配置数据（对应保存的JSON文件）
    /// 每个地图区域下挂属于自己的建筑/物件/人物集合
    /// </summary>
    [Serializable]
    public class MapConfigData
    {
        /// <summary>场景ID</summary>
        public string sceneId = string.Empty;

        /// <summary>场景名称</summary>
        public string sceneName = string.Empty;

        /// <summary>地图宽度（像素）</summary>
        public int width = MapToolConstants.LevelPixelSize;

        /// <summary>地图高度（像素）</summary>
        public int height = MapToolConstants.LevelPixelSize;

        // ---TODO 双格式保存：新增章节信息字段（JSON 落 MapDatas，Lua 同源导出）
        /// <summary>章节ID（来自 TableChapterEditor.ID）</summary>
        public string chapterId = string.Empty;

        /// <summary>章节名称（来自 TableChapterEditor.ChapterTitle）</summary>
        public string chapterName = string.Empty;

        /// <summary>背景图资源名称</summary>
        public string bgName = string.Empty;

        // ---TODO 网格信息：保存格子尺寸/行列/占用，供运行时检查位置
        /// <summary>格子尺寸（像素，= MapToolConstants.GridSize）</summary>
        public int gridSize = 32;

        /// <summary>相邻显示层基线 SortOrder 步长（落盘地图数据；缺失/非法时读取侧回退默认步长）</summary>
        public int baselineStep = MapToolConstants.LayerBaselineStepDefault;

        /// <summary>绘制形状 code（DrawShape.Rect/Diamond；缺失/非法时读取侧回退默认菱形）</summary>
        public string drawShape = MapToolConstants.DrawShapeDefault;

        /// <summary>区域类型 code（RegionType.Place/Forbid/Common；缺失/非法时读取侧回退默认摆放区）</summary>
        public string regionType = MapToolConstants.RegionTypeDefault;

        /// <summary>网格列数（= width / gridSize）</summary>
        public int gridCols = 0;

        /// <summary>网格行数（= height / gridSize）</summary>
        public int gridRows = 0;

        /// <summary>被摆放物占用的格子集合（格子行列坐标，运行时位置/占用校验）</summary>
        public List<MapGridCellData> occupiedCells = new List<MapGridCellData>();

        /// <summary>建筑集合</summary>
        public List<MapObjectData> buildings = new List<MapObjectData>();

        /// <summary>物件集合</summary>
        public List<MapObjectData> objects = new List<MapObjectData>();

        /// <summary>人物集合</summary>
        public List<MapCharacterData> characters = new List<MapCharacterData>();

        // ---TODO 交通规则：红绿灯 / 斑马线 / 区域回显 / 人行道格 / 道路格（旧地图缺省为空，兼容读取）
        /// <summary>红绿灯集合</summary>
        public List<MapTrafficLightData> trafficLights = new List<MapTrafficLightData>();

        /// <summary>斑马线集合</summary>
        public List<MapCrosswalkData> crosswalks = new List<MapCrosswalkData>();

        /// <summary>矩形区域回显（人行道/道路刷选区域，可选持久化）</summary>
        public List<MapRegionData> regions = new List<MapRegionData>();

        /// <summary>人行道可走格集合（行人可站立/行走）</summary>
        public List<MapGridCellData> sidewalkCells = new List<MapGridCellData>();

        /// <summary>车辆行驶格集合（行人禁入）</summary>
        public List<MapGridCellData> roadCells = new List<MapGridCellData>();
    }

    /// <summary>
    /// 网格占用格子数据（行列坐标）
    /// </summary>
    [Serializable]
    public class MapGridCellData
    {
        /// <summary>列（X）</summary>
        public int x = 0;

        /// <summary>行（Y）</summary>
        public int y = 0;
    }

    /// <summary>
    /// 出生点数据
    /// </summary>
    [Serializable]
    public class MapSpawnPointData
    {
        /// <summary>出生点ID</summary>
        public string id = string.Empty;

        /// <summary>名称</summary>
        public string name = string.Empty;

        /// <summary>X坐标（世界像素）</summary>
        public float x = 0;

        /// <summary>Y坐标（世界像素）</summary>
        public float y = 0;

        /// <summary>格子X</summary>
        public int gridX = 0;

        /// <summary>格子Y</summary>
        public int gridY = 0;

        /// <summary>初始朝向：up/down/left/right</summary>
        public string facing = TemplateFacing.Down;
    }

    /// <summary>
    /// 地图物体数据（建筑/物件）
    /// </summary>
    [Serializable]
    public class MapObjectData
    {
        /// <summary>实例ID</summary>
        public string id = string.Empty;

        /// <summary>模板ID（引用物品元件模板）</summary>
        public string templateId = string.Empty;

        /// <summary>物体名称</summary>
        public string name = string.Empty;

        /// <summary>位置X（世界坐标）</summary>
        public float posX = 0;

        /// <summary>位置Y（世界坐标）</summary>
        public float posY = 0;

        // ---TODO 网格信息：格子坐标（= FloorToInt(pos / gridSize)），供运行时检查位置
        /// <summary>格子X（= FloorToInt(posX / gridSize)）</summary>
        public int gridX = 0;

        /// <summary>格子Y（= FloorToInt(posY / gridSize)）</summary>
        public int gridY = 0;

        /// <summary>宽度（格子数）</summary>
        public int width = 1;

        /// <summary>高度（格子数）</summary>
        public int height = 1;

        /// <summary>PSD 图层原始像素宽（&gt;0 时运行时按像素尺寸还原）</summary>
        public float pixelWidth = 0;

        /// <summary>PSD 图层原始像素高（&gt;0 时运行时按像素尺寸还原）</summary>
        public float pixelHeight = 0;

        /// <summary>是否拉伸到配置尺寸（false=保持原图比例）</summary>
        public bool stretch = false;

        /// <summary>图层类型键（background/greenery/road/building/prop/character/foreground/bubble；旧键 ground/cat/effect 读取时归一）</summary>
        public string layerType = MapLayerCatalog.Prop;

        /// <summary>显示层键（渲染前后档位；空=跟随 layerType）</summary>
        public string displayLayer = string.Empty;

        /// <summary>图片资源路径</summary>
        public string image = string.Empty;

        /// <summary>动画名称</summary>
        public string anim = "idle";

        /// <summary>是否启用动画（false=静态图）</summary>
        public bool animEnabled = true;

        /// <summary>是否循环播放动画</summary>
        public bool loop = true;

        /// <summary>动画播放速度</summary>
        public float speed = 1f;

        /// <summary>是否可交互</summary>
        public bool interactable = true;

        /// <summary>交互类型</summary>
        public string interactType = "click";

        /// <summary>渲染层级排序（八层基线+层内槽位全局键；0=未配置，读取时经 NormalizeLegacySortOrder 换算到所属层基线）</summary>
        public int sortOrder = 0;

        /// <summary>是否启用Y坐标自动深度排序</summary>
        public bool ySortEnabled = true;

        /// <summary>Y坐标深度排序的基准偏移</summary>
        public float ySortOffset = 0f;

        /// <summary>旋转角度（度，绕 Z 轴；0=不旋转）</summary>
        public float rotation = 0f;

        /// <summary>缩放倍率（1=原始比例）</summary>
        public float scale = 1f;

        /// <summary>是否有碰撞体积</summary>
        public bool collisionEnabled = true;

        /// <summary>对话气泡内容</summary>
        public string dialogBubble = string.Empty;

        /// <summary>是否显示地面阴影</summary>
        public bool shadowEnabled = true;

        /// <summary>是否为收集物</summary>
        public bool isCollectible = false;

        /// <summary>收集物重生时间（秒）</summary>
        public float respawnTime = 0;

        /// <summary>通用气泡配置ID（引用 Tables.Bubble，0=不弹泡）</summary>
        public int bubbleId = 0;

        /// <summary>是否可移动（建筑/物件也能移动）</summary>
        public bool isMovable = false;

        /// <summary>循环行驶：离开地图后重新开始的延迟时间（秒）</summary>
        public float loopRestartDelay = 3f;

        /// <summary>离开地图时是否隐藏</summary>
        public bool hideWhenOffMap = true;

        /// <summary>是否为传送门</summary>
        public bool isTeleport = false;

        /// <summary>目标场景ID</summary>
        public string teleportTargetScene = "";

        /// <summary>目标位置X（世界坐标）</summary>
        public float teleportTargetX = 0;

        /// <summary>目标位置Y（世界坐标）</summary>
        public float teleportTargetY = 0;

        /// <summary>位移数据（如果有移动）</summary>
        public MapMoveData moveData;
    }

    /// <summary>
    /// 物体位移数据
    /// </summary>
    [Serializable]
    public class MapMoveData
    {
        /// <summary>是否启用位移</summary>
        public bool enabled = false;

        /// <summary>起始点X坐标</summary>
        public float startX = 0;

        /// <summary>起始点Y坐标</summary>
        public float startY = 0;

        /// <summary>终点X坐标</summary>
        public float endX = 0;

        /// <summary>终点Y坐标</summary>
        public float endY = 0;

        /// <summary>移动速度</summary>
        public float moveSpeed = 1f;

        /// <summary>是否循环移动</summary>
        public bool loop = true;

        /// <summary>移动方式：pingpong（往返）/once（单向）/patrol（巡逻）</summary>
        public string moveType = "pingpong";
    }

    /// <summary>
    /// 人物数据（含行走路径集合）
    /// </summary>
    [Serializable]
    public class MapCharacterData
    {
        /// <summary>实例ID</summary>
        public string id = string.Empty;

        /// <summary>模板ID（引用物品元件模板）</summary>
        public string templateId = string.Empty;

        /// <summary>人物名称</summary>
        public string name = string.Empty;

        /// <summary>位置X（世界坐标）</summary>
        public float posX = 0;

        /// <summary>位置Y（世界坐标）</summary>
        public float posY = 0;

        // ---TODO 网格信息：格子坐标（= FloorToInt(pos / gridSize)），供运行时检查位置
        /// <summary>格子X（= FloorToInt(posX / gridSize)）</summary>
        public int gridX = 0;

        /// <summary>格子Y（= FloorToInt(posY / gridSize)）</summary>
        public int gridY = 0;

        /// <summary>宽度（格子数）</summary>
        public int width = 1;

        /// <summary>高度（格子数）</summary>
        public int height = 1;

        /// <summary>PSD 图层原始像素宽（&gt;0 时运行时按像素尺寸还原）</summary>
        public float pixelWidth = 0;

        /// <summary>PSD 图层原始像素高（&gt;0 时运行时按像素尺寸还原）</summary>
        public float pixelHeight = 0;

        /// <summary>是否拉伸到配置尺寸（false=保持原图比例）</summary>
        public bool stretch = false;

        /// <summary>图层类型键（background/greenery/road/building/prop/character/foreground/bubble；旧键 ground/cat/effect 读取时归一）</summary>
        public string layerType = MapLayerCatalog.Character;

        /// <summary>显示层键（渲染前后档位；空=跟随 layerType）</summary>
        public string displayLayer = string.Empty;

        /// <summary>图片资源路径</summary>
        public string image = string.Empty;

        /// <summary>动画名称</summary>
        public string anim = "idle";

        /// <summary>是否启用动画（false=静态图）</summary>
        public bool animEnabled = true;

        /// <summary>是否循环播放动画</summary>
        public bool loop = true;

        /// <summary>动画播放速度</summary>
        public float speed = 1f;

        /// <summary>是否可交互</summary>
        public bool interactable = false;

        /// <summary>交互类型</summary>
        public string interactType = TemplateInteractType.None;

        /// <summary>行走路径模式：once（单次）/ pingpong（往返）/ loop（循环）</summary>
        public string walkMode = "loop";

        /// <summary>初始朝向：up/down/left/right</summary>
        public string facingDirection = TemplateFacing.Down;

        /// <summary>对话气泡内容</summary>
        public string dialogBubble = string.Empty;

        /// <summary>渲染层级排序（八层基线+层内槽位全局键；0=未配置，读取时经 NormalizeLegacySortOrder 换算到角色层基线）</summary>
        public int sortOrder = 0;

        /// <summary>是否启用Y坐标自动深度排序</summary>
        public bool ySortEnabled = true;

        /// <summary>Y坐标深度排序的基准偏移</summary>
        public float ySortOffset = 0f;

        /// <summary>旋转角度（度，绕 Z 轴；0=不旋转）</summary>
        public float rotation = 0f;

        /// <summary>缩放倍率（1=原始比例）</summary>
        public float scale = 1f;

        /// <summary>是否显示地面阴影</summary>
        public bool shadowEnabled = true;

        /// <summary>循环行驶：离开地图后重新开始的延迟时间（秒）</summary>
        public float loopRestartDelay = 3f;

        /// <summary>离开地图时是否隐藏</summary>
        public bool hideWhenOffMap = true;

        // ---TODO 交通规则：载具标记与停止线/绑定灯（行人条目不消费）
        /// <summary>是否为载具（汽车/巴士/货车…）</summary>
        public bool isVehicle = false;

        /// <summary>交通角色：pedestrian 行人 / vehicle 载具 / none 不参与</summary>
        public string trafficRole = TemplateTrafficCategory.Pedestrian;

        /// <summary>车辆停止线 X（世界像素，仅载具）</summary>
        public float stopLineX = 0;

        /// <summary>车辆停止线 Y（世界像素，仅载具）</summary>
        public float stopLineY = 0;

        /// <summary>绑定红绿灯实例 ID（红灯在此等待）；兼容旧单值，多选结果见 <see cref="boundLightIds"/></summary>
        public string boundLightId = string.Empty;

        /// <summary>绑定红绿灯实例 ID 列表（多选；全选后即当前地图全部灯）</summary>
        public List<string> boundLightIds = new List<string>();

        /// <summary>黄灯是否按红灯处理（仅车辆消费）</summary>
        public bool yellowTreatAsRed = true;

        /// <summary>可走路径点集合</summary>
        public List<MapPathPointData> pathPoints = new List<MapPathPointData>();
    }

    /// <summary>
    /// 路径点数据
    /// </summary>
    [Serializable]
    public class MapPathPointData
    {
        /// <summary>X坐标（世界坐标，像素）</summary>
        public float x = 0;

        /// <summary>Y坐标（世界坐标，像素）</summary>
        public float y = 0;

        /// <summary>格子X（X/GridSize 取整，运行时对照）</summary>
        public int gridX = 0;

        /// <summary>格子Y（Y/GridSize 取整，运行时对照）</summary>
        public int gridY = 0;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MapPathPointData()
        {
        }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        public MapPathPointData(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    /// <summary>
    /// 状态贴图配置
    /// </summary>
    [Serializable]
    public class StateSpriteConfig
    {
        /// <summary>状态名称</summary>
        public string stateName = "idle";

        /// <summary>贴图路径</summary>
        public string spritePath = "";
    }

    /// <summary>
    /// 状态动画配置
    /// </summary>
    [Serializable]
    public class StateAnimConfig
    {
        /// <summary>状态名称</summary>
        public string stateName = "idle";

        /// <summary>动画名称</summary>
        public string animName = "idle";
    }

    #endregion

    #region 交通规则序列化数据

    /// <summary>
    /// 红绿灯数据（绑定斑马线，控制行人/载具放行相位）
    /// </summary>
    [Serializable]
    public class MapTrafficLightData
    {
        /// <summary>实例 ID（8 位）</summary>
        public string id = string.Empty;

        /// <summary>模板 ID</summary>
        public string templateId = string.Empty;

        /// <summary>显示名（如 路口A-竖向灯）</summary>
        public string name = string.Empty;

        /// <summary>灯体位置 X（世界像素）</summary>
        public float posX = 0;

        /// <summary>灯体位置 Y（世界像素）</summary>
        public float posY = 0;

        /// <summary>格子 X</summary>
        public int gridX = 0;

        /// <summary>格子 Y</summary>
        public int gridY = 0;

        /// <summary>管控横道朝向：horizontal 横向 / vertical 竖向</summary>
        public string direction = MapToolConstants.TrafficDirectionHorizontal;

        /// <summary>对向端位：north/south/east/west</summary>
        public string side = "north";

        /// <summary>路口聚合键（预留；同路口多横道共享相位，单横道留空）</summary>
        public string intersectionId = string.Empty;

        /// <summary>红灯时长（秒）</summary>
        public float redDuration = 5f;

        /// <summary>绿灯时长（秒）</summary>
        public float greenDuration = 5f;

        /// <summary>黄灯时长（秒）</summary>
        public float yellowDuration = 1f;

        /// <summary>相位偏移（秒），多灯错相；0=同时相</summary>
        public float offsetPhase = 0f;

        /// <summary>绑定斑马线 ID</summary>
        public string boundCrosswalkId = string.Empty;

        /// <summary>初始相位：red/green/yellow</summary>
        public string initialPhase = "red";

        /// <summary>贴图资源路径</summary>
        public string image = string.Empty;

        /// <summary>动画名称</summary>
        public string anim = "idle";
    }

    /// <summary>
    /// 斑马线（人行横道）数据：连接两侧人行道，行人唯一允许横穿马路的通道
    /// </summary>
    [Serializable]
    public class MapCrosswalkData
    {
        /// <summary>实例 ID</summary>
        public string id = string.Empty;

        /// <summary>显示名</summary>
        public string name = string.Empty;

        /// <summary>横道起点（接一侧人行道）</summary>
        public MapPathPointData pointA = new MapPathPointData();

        /// <summary>横道终点（接另一侧人行道）</summary>
        public MapPathPointData pointB = new MapPathPointData();

        /// <summary>横道宽度（像素，吸附网格）</summary>
        public float width = 32f;

        /// <summary>横穿方向：horizontal/vertical</summary>
        public string direction = MapToolConstants.TrafficDirectionHorizontal;

        /// <summary>绑定红绿灯 ID（对向双灯取主灯）</summary>
        public string boundLightId = string.Empty;
    }

    /// <summary>
    /// 矩形区域回显数据（编辑器框选刷格的原始矩形，便于二次编辑；运行时以格子集合为准）
    /// </summary>
    [Serializable]
    public class MapRegionData
    {
        /// <summary>区域标识</summary>
        public string id = string.Empty;

        /// <summary>区域类型：sidewalk 人行道 / road 道路</summary>
        public string type = "sidewalk";

        /// <summary>矩形左上角 X（世界像素）</summary>
        public float x = 0;

        /// <summary>矩形左上角 Y（世界像素）</summary>
        public float y = 0;

        /// <summary>矩形宽度（像素）</summary>
        public float width = 0;

        /// <summary>矩形高度（像素）</summary>
        public float height = 0;
    }

    #endregion
}
