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

        /// <summary>是否有碰撞体积</summary>
        public bool CollisionEnabled = true;

        /// <summary>初始朝向：up/down/left/right</summary>
        public string FacingDirection = "down";

        /// <summary>是否显示地面阴影</summary>
        public bool ShadowEnabled = true;

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

        /// <summary>所属分区索引</summary>
        public int RegionIndex = 0;

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

        /// <summary>是否允许与其他物体重叠覆盖</summary>
        public bool AllowOverlap = false;

        #endregion

        #region 渲染与物理属性

        /// <summary>渲染层级排序（数值越大越靠前）</summary>
        public int SortOrder = 0;

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

        /// <summary>是否循环巡逻（NPC来回走动）</summary>
        public bool PatrolLoop = false;

        /// <summary>人物行走路径点列表（世界坐标，吸附32px网格）</summary>
        public List<Vector2> PathPoints = new List<Vector2>();

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
        public int width = 2048;

        /// <summary>地图高度（像素）</summary>
        public int height = 2048;

        /// <summary>分区信息列表</summary>
        public List<MapRegionData> regions = new List<MapRegionData>();
    }

    /// <summary>
    /// 地图分区信息
    /// 每个区域下挂自己的建筑/物件/人物集合
    /// </summary>
    [Serializable]
    public class MapRegionData
    {
        /// <summary>区域ID</summary>
        public string regionId = string.Empty;

        /// <summary>背景图资源名称</summary>
        public string bgName = string.Empty;

        /// <summary>是否默认解锁</summary>
        public bool unlocked = false;

        /// <summary>是否开启迷雾效果</summary>
        public bool fogEnabled = false;

        /// <summary>该区域建筑集合</summary>
        public List<MapObjectData> buildings = new List<MapObjectData>();

        /// <summary>该区域物件集合</summary>
        public List<MapObjectData> objects = new List<MapObjectData>();

        /// <summary>该区域人物集合</summary>
        public List<MapCharacterData> characters = new List<MapCharacterData>();
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

        /// <summary>宽度（格子数）</summary>
        public int width = 1;

        /// <summary>高度（格子数）</summary>
        public int height = 1;

        /// <summary>图片资源路径</summary>
        public string image = string.Empty;

        /// <summary>动画名称</summary>
        public string anim = "idle";

        /// <summary>是否循环播放动画</summary>
        public bool loop = true;

        /// <summary>动画播放速度</summary>
        public float speed = 1f;

        /// <summary>是否可交互</summary>
        public bool interactable = true;

        /// <summary>交互类型</summary>
        public string interactType = "click";

        /// <summary>渲染层级排序</summary>
        public int sortOrder = 0;

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

        /// <summary>宽度（格子数）</summary>
        public int width = 1;

        /// <summary>高度（格子数）</summary>
        public int height = 1;

        /// <summary>图片资源路径</summary>
        public string image = string.Empty;

        /// <summary>动画名称</summary>
        public string anim = "idle";

        /// <summary>是否循环播放动画</summary>
        public bool loop = true;

        /// <summary>动画播放速度</summary>
        public float speed = 1f;

        /// <summary>是否可交互</summary>
        public bool interactable = false;

        /// <summary>交互类型</summary>
        public string interactType = "none";

        /// <summary>是否循环巡逻</summary>
        public bool patrolLoop = true;

        /// <summary>初始朝向：up/down/left/right</summary>
        public string facingDirection = "down";

        /// <summary>对话气泡内容</summary>
        public string dialogBubble = string.Empty;

        /// <summary>渲染层级排序</summary>
        public int sortOrder = 50;

        /// <summary>是否显示地面阴影</summary>
        public bool shadowEnabled = true;

        /// <summary>可走路径点集合</summary>
        public List<MapPathPointData> pathPoints = new List<MapPathPointData>();
    }

    /// <summary>
    /// 路径点数据
    /// </summary>
    [Serializable]
    public class MapPathPointData
    {
        /// <summary>X坐标（世界坐标）</summary>
        public float x = 0;

        /// <summary>Y坐标（世界坐标）</summary>
        public float y = 0;

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

    #endregion
}
