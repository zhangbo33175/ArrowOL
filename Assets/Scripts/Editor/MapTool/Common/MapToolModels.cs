/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapToolModels.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器数据模型（物体类型枚举/PlacedObject/地图配置JSON序列化结构）
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图物体类型
    /// </summary>
    public enum MapObjectType
    {
        /// <summary>建筑</summary>
        Building = 0,
        /// <summary>人物</summary>
        Character = 1,
        /// <summary>物件</summary>
        Prop = 2
    }

    /// <summary>
    /// 已放置在地图上的物体（编辑器运行时数据）
    /// </summary>
    public class PlacedObject
    {
        /// <summary>物体ID</summary>
        public string Id;

        /// <summary>物体名称</summary>
        public string Name;

        /// <summary>物体类型</summary>
        public MapObjectType Type;

        /// <summary>左上角世界坐标</summary>
        public Vector2 Position;

        /// <summary>宽度（格子数）</summary>
        public int Width;

        /// <summary>高度（格子数）</summary>
        public int Height;

        /// <summary>预览图</summary>
        public Texture PreviewTexture;

        /// <summary>人物行走路径（世界坐标，吸附 32px 网格）</summary>
        public List<Vector2> PathPoints = new List<Vector2>();

        /// <summary>动画名称</summary>
        public string AnimName = "Idle";

        /// <summary>是否循环播放</summary>
        public bool Loop = true;

        /// <summary>播放速度</summary>
        public float Speed = 1f;

        /// <summary>是否可交互</summary>
        public bool Interactable = true;

        /// <summary>是否允许与其他物体重叠覆盖</summary>
        public bool AllowOverlap = false;

        /// <summary>所属分区索引</summary>
        public int RegionIndex;
    }

    #region 地图配置 JSON 数据结构

    /// <summary>
    /// 地图配置数据（对应保存的 JSON 文件）
    /// 每个地图区域下挂属于自己的建筑 / 物件 / 人物集合
    /// </summary>
    [System.Serializable]
    public class MapConfigData
    {
        /// <summary>场景ID</summary>
        public string sceneId;

        /// <summary>场景名</summary>
        public string sceneName;

        /// <summary>地图宽度（已解锁分区包围盒）</summary>
        public int width;

        /// <summary>地图高度（已解锁分区包围盒）</summary>
        public int height;

        /// <summary>分区信息（含各区建筑 / 物件 / 人物集合）</summary>
        public List<MapRegionData> regions = new List<MapRegionData>();
    }

    /// <summary>
    /// 分区信息（每个区域下挂自己的建筑 / 物件 / 人物集合）
    /// </summary>
    [System.Serializable]
    public class MapRegionData
    {
        /// <summary>地图区域ID</summary>
        public string regionId;

        /// <summary>地图背景名称</summary>
        public string bgName;

        /// <summary>是否解锁</summary>
        public bool unlocked;

        /// <summary>迷雾是否开启</summary>
        public bool fogEnabled;

        /// <summary>该区域建筑集合</summary>
        public List<MapObjectData> buildings = new List<MapObjectData>();

        /// <summary>该区域物件集合</summary>
        public List<MapObjectData> objects = new List<MapObjectData>();

        /// <summary>该区域人物集合</summary>
        public List<MapCharacterData> characters = new List<MapCharacterData>();
    }

    /// <summary>
    /// 物件 / 建筑数据
    /// </summary>
    [System.Serializable]
    public class MapObjectData
    {
        /// <summary>物体ID</summary>
        public string id;

        /// <summary>物体名称</summary>
        public string name;

        /// <summary>位置X（世界坐标）</summary>
        public float posX;

        /// <summary>位置Y（世界坐标）</summary>
        public float posY;

        /// <summary>宽度（格子数）</summary>
        public int width;

        /// <summary>高度（格子数）</summary>
        public int height;

        /// <summary>图片信息（资源路径）</summary>
        public string image;

        /// <summary>动画信息（动画名称）</summary>
        public string anim;

        /// <summary>是否循环播放动画</summary>
        public bool loop = true;

        /// <summary>动画播放速度</summary>
        public float speed = 1f;

        /// <summary>是否可交互</summary>
        public bool interactable = true;

        /// <summary>交互类型（点击/自动/拖拽等）</summary>
        public string interactType = "click";

        /// <summary>位移信息（如果有移动）</summary>
        public MapMoveData moveData;
    }

    /// <summary>
    /// 位移数据（物体移动信息）
    /// </summary>
    [System.Serializable]
    public class MapMoveData
    {
        /// <summary>是否启用位移</summary>
        public bool enabled = false;

        /// <summary>起始点X</summary>
        public float startX;

        /// <summary>起始点Y</summary>
        public float startY;

        /// <summary>终点X</summary>
        public float endX;

        /// <summary>终点Y</summary>
        public float endY;

        /// <summary>移动速度</summary>
        public float moveSpeed = 1f;

        /// <summary>是否循环移动</summary>
        public bool loop = true;

        /// <summary>移动方式（往返/单向/巡逻）</summary>
        public string moveType = "pingpong";
    }

    /// <summary>
    /// 人物数据（含可走路径集合）
    /// </summary>
    [System.Serializable]
    public class MapCharacterData
    {
        /// <summary>人物ID</summary>
        public string id;

        /// <summary>人物名称</summary>
        public string name;

        /// <summary>位置X（世界坐标）</summary>
        public float posX;

        /// <summary>位置Y（世界坐标）</summary>
        public float posY;

        /// <summary>宽度（格子数）</summary>
        public int width;

        /// <summary>高度（格子数）</summary>
        public int height;

        /// <summary>图片信息（资源路径）</summary>
        public string image;

        /// <summary>动画信息（动画名称）</summary>
        public string anim;

        /// <summary>是否可交互</summary>
        public bool interactable;

        /// <summary>可走路径集合</summary>
        public List<MapPathPointData> pathPoints = new List<MapPathPointData>();
    }

    /// <summary>
    /// 路径点数据
    /// </summary>
    [System.Serializable]
    public class MapPathPointData
    {
        /// <summary>X坐标（世界坐标）</summary>
        public float x;

        /// <summary>Y坐标（世界坐标）</summary>
        public float y;
    }

    #endregion
}