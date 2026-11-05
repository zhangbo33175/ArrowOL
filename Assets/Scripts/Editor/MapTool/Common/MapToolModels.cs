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
    #region 枚举定义

    /// <summary>
    /// 地图物体类型枚举
    /// </summary>
    public enum MapObjectType
    {
        /// <summary>建筑类元素</summary>
        Building = 0,

        /// <summary>人物/NPC类元素</summary>
        Character = 1,

        /// <summary>场景物件类元素</summary>
        Prop = 2
    }

    #endregion

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
        public string InteractType = "none";

        /// <summary>对话气泡内容</summary>
        public string DialogBubble = string.Empty;

        #endregion

        #region 渲染与物理属性

        /// <summary>渲染层级排序（数值越大越靠前）</summary>
        public int SortOrder = 0;

        /// <summary>是否启用Y坐标自动深度排序</summary>
        public bool YSortEnabled = true;

        /// <summary>Y坐标深度排序的基准偏移</summary>
        public float YSortOffset = 0f;

        /// <summary>是否有碰撞体积</summary>
        public bool CollisionEnabled = true;

        /// <summary>初始朝向：up/down/left/right</summary>
        public string FacingDirection = "down";

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

        #endregion

        #region 动画属性

        /// <summary>动画名称</summary>
        public string AnimName = "Idle";

        /// <summary>是否循环播放动画</summary>
        public bool Loop = true;

        /// <summary>播放速度倍率</summary>
        public float Speed = 1f;

        #endregion

        #region 交互属性

        /// <summary>是否可交互</summary>
        public bool Interactable = true;

        /// <summary>交互类型：none/enter/dialog/pickup/view</summary>
        public string InteractType = "none";

        /// <summary>对话气泡内容</summary>
        public string DialogBubble = string.Empty;

        // ---TODO 气泡配置表通用化：引用 Tables.Bubble（>0 生效，<=0 不输出）
        /// <summary>气泡配置表ID（引用 Tables.Bubble；<=0 表示无气泡，Lua 导出时整行省略）</summary>
        public int bubbleId = 0;

        /// <summary>是否允许与其他物体重叠覆盖</summary>
        public bool AllowOverlap = false;

        #endregion

        #region 渲染与物理属性

        /// <summary>渲染层级排序（数值越大越靠前：物件=10 建筑=30 人物=50）</summary>
        public int SortOrder = 10;

        /// <summary>是否启用Y坐标自动深度排序（启用后根据Y坐标自动计算前后关系）</summary>
        public bool YSortEnabled = true;

        /// <summary>Y坐标深度排序的基准偏移（越大越靠前，默认0）</summary>
        public float YSortOffset = 0f;

        /// <summary>是否有碰撞体积</summary>
        public bool CollisionEnabled = true;

        /// <summary>初始朝向：up/down/left/right</summary>
        public string FacingDirection = "down";

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
        public string TriggerEventType = "none";

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
        /// 从模板创建实例
        /// </summary>
        /// <param name="template">模板对象</param>
        /// <returns>新创建的实例对象</returns>
        public static PlacedObject CreateFromTemplate(ObjectTemplate template)
        {
            if (template == null)
                return null;

            return new PlacedObject
            {
                TemplateId = template.TemplateId,
                Name = template.TemplateName,
                Type = template.Type,
                AnimName = template.AnimName,
                Loop = template.Loop,
                Speed = template.Speed,
                Interactable = template.Interactable,
                InteractType = template.InteractType,
                SortOrder = template.SortOrder,
                CollisionEnabled = template.CollisionEnabled,
                DialogBubble = template.DialogBubble,
                FacingDirection = template.FacingDirection,
                ShadowEnabled = template.ShadowEnabled,
                IsCollectible = template.IsCollectible,
                RespawnTime = template.RespawnTime
            };
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
            SortOrder = template.SortOrder;
            CollisionEnabled = template.CollisionEnabled;
            DialogBubble = template.DialogBubble;
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
        public int width = 4096;

        /// <summary>地图高度（像素）</summary>
        public int height = 4096;

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
        public string facing = "down";
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

        /// <summary>渲染层级排序（建筑默认=30，在物件和人物之间）</summary>
        public int sortOrder = 30;

        /// <summary>是否启用Y坐标自动深度排序</summary>
        public bool ySortEnabled = true;

        /// <summary>Y坐标深度排序的基准偏移</summary>
        public float ySortOffset = 0f;

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
        public string interactType = "none";

        /// <summary>行走路径模式：once（单次）/ pingpong（往返）/ loop（循环）</summary>
        public string walkMode = "loop";

        /// <summary>初始朝向：up/down/left/right</summary>
        public string facingDirection = "down";

        /// <summary>对话气泡内容</summary>
        public string dialogBubble = string.Empty;

        /// <summary>渲染层级排序</summary>
        public int sortOrder = 50;

        /// <summary>是否启用Y坐标自动深度排序</summary>
        public bool ySortEnabled = true;

        /// <summary>Y坐标深度排序的基准偏移</summary>
        public float ySortOffset = 0f;

        /// <summary>是否显示地面阴影</summary>
        public bool shadowEnabled = true;

        /// <summary>循环行驶：离开地图后重新开始的延迟时间（秒）</summary>
        public float loopRestartDelay = 3f;

        /// <summary>离开地图时是否隐藏</summary>
        public bool hideWhenOffMap = true;

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
}
