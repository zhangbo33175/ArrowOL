/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapData.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图系统核心数据类
 *            管理地图配置、相机参数、视图边界、图标数据等核心逻辑
 *            新增：剔除上下HUD的红框视口计算、动态自适应最小缩放尺寸
 ***************************************************************/

using UnityEngine;
using System.Collections.Generic;
using GameLib;
using UnityEngine.Serialization;

//=========================================================================
// 地图枚举定义
//=========================================================================
/// <summary>
/// 地图图标显示效果类型
/// </summary>
public enum RMapIconType
{
    /// <summary>
    /// 染色显示模式
    /// </summary>
    Coloring = 0,

    /// <summary>
    /// 选中消散效果模式
    /// </summary>
    Dissipate = 1
}

/// <summary>
/// 关卡游玩时HUD界面对齐方式
/// </summary>
public enum RMapPlayHudPosType
{
    /// <summary>
    /// 左对齐
    /// </summary>
    Left = 0,

    /// <summary>
    /// 居中对齐
    /// </summary>
    Center = 1
}

/// <summary>
/// 地图功能类型
/// </summary>
public enum RMapType
{
    /// <summary>
    /// 常规主线地图
    /// </summary>
    Normal = 0,

    /// <summary>
    /// 每日挑战地图
    /// </summary>
    DailyLevel = 1
}

/// <summary>
/// 地图默认相机初始位置类型
/// </summary>
public enum RMapCamPosType
{
    /// <summary>
    /// 相机默认靠左
    /// </summary>
    Left = 0,

    /// <summary>
    /// 相机默认靠右
    /// </summary>
    Right = 1,

    /// <summary>
    /// 相机居中对齐
    /// </summary>
    Center = 2
}

//=========================================================================
// 地图数据实体类
//=========================================================================
/// <summary>
/// 地图单个图标/节点数据实体
/// </summary>
[System.Serializable]
public class RMapData
{
    /// <summary>
    /// 唯一标识ID
    /// </summary>
    public int m_Id;

    /// <summary>
    /// 显示效果类型
    /// </summary>
    public RMapIconType m_Type = RMapIconType.Dissipate;

    /// <summary>
    /// 世界空间位置
    /// </summary>
    public Vector3 m_Position;

    /// <summary>
    /// 世界空间旋转
    /// </summary>
    public Vector3 m_Rotation;

    /// <summary>
    /// 世界空间缩放
    /// </summary>
    public Vector3 m_Scale;

    /// <summary>
    /// 图片名字
    /// </summary>
    public string m_Sprite;

    /// <summary>
    /// 显示名称
    /// </summary>
    public string m_Name;

    /// <summary>
    /// 图片尺寸
    /// </summary>
    public Vector2 m_Size;

    /// <summary>
    /// 是否被选中
    /// </summary>
    public bool m_IsChoose;
}

/// <summary>
/// 地图章节配置数据实体
/// </summary>
[System.Serializable]
public class RMapChapterTypeData
{
    /// <summary>
    /// 地图名称
    /// </summary>
    public string m_MapName;

    /// <summary>
    /// 所属章节ID
    /// </summary>
    public string ChapterId = string.Empty;

    /// <summary>
    /// 关卡ID
    /// </summary>
    public int LevelId;

    /// <summary>
    /// 背景图片资源路径
    /// </summary>
    public string m_BackgroundPath;

    /// <summary>
    /// 地图子物体数据列表
    /// </summary>
    public List<RMapData> m_MapObjectData = new List<RMapData>();
    /// <summary>
    /// 配置创建时间
    /// </summary>
    public string m_CreateTime;
    /// <summary>
    /// 配置保存路径
    /// </summary>
    public string SavePath = string.Empty;
}

/// <summary>
/// 地图相机参数数据
/// 负责相机尺寸、位置计算与多状态适配
/// </summary>
[System.Serializable]
public class MapCamData
{
    /// <summary>
    /// 相机正交大小
    /// </summary>
    public float size;

    /// <summary>
    /// 相机X轴世界位置
    /// </summary>
    public float posX;

    /// <summary>
    /// 相机Y轴世界位置
    /// </summary>
    public float posY;

    #region 相机位置计算方法
    /// <summary>
    /// 标准化调整相机Y轴位置
    /// </summary>
    /// <param name="designHeight">设计高度</param>
    public void NormalAdjustPosY(float designHeight)
    {
        posY = (size * 2 - designHeight * 0.01f) * 0.5f;
    }

    /// <summary>
    /// 左对齐模式下标准化相机X轴位置（全屏尺寸旧接口，保留兼容旧逻辑）
    /// </summary>
    /// <param name="viewSize">视图尺寸</param>
    /// <param name="mapLeftWorldPosX">地图左边界世界X坐标</param>
    public void NormalAdjustLeftPosX(Vector2 viewSize, float mapLeftWorldPosX)
    {
        var viewWidth = viewSize.x / viewSize.y * size * 2 * 100;
        var viewBoundPosX = viewWidth / 200;
        posX = mapLeftWorldPosX + viewBoundPosX;
    }

    /// <summary>
    /// 左对齐模式标准化X（仅中间地图红框视口，剔除上下UI，推荐新逻辑使用）
    /// </summary>
    public void NormalAdjustLeftPosX_Viewport(Vector2 viewportSize, float mapLeftWorldPosX)
    {
        var viewWidth = viewportSize.x / viewportSize.y * size * 2 * 100;
        var viewBoundPosX = viewWidth / 200;
        posX = mapLeftWorldPosX + viewBoundPosX;
    }

    /// <summary>
    /// 右对齐模式下标准化相机X轴位置（全屏尺寸旧接口）
    /// </summary>
    /// <param name="viewSize">视图尺寸</param>
    /// <param name="mapRightWorldPosX">地图右边界世界X坐标</param>
    public void NormalAdjustRightPosX(Vector2 viewSize, float mapRightWorldPosX)
    {
        var viewWidth = viewSize.x / viewSize.y * size * 2 * 100;
        var viewBoundPosX = viewWidth / 200;
        posX = mapRightWorldPosX - viewBoundPosX;
    }

    /// <summary>
    /// 右对齐模式标准化X（仅中间地图红框视口，剔除上下UI）
    /// </summary>
    public void NormalAdjustRightPosX_Viewport(Vector2 viewportSize, float mapRightWorldPosX)
    {
        var viewWidth = viewportSize.x / viewportSize.y * size * 2 * 100;
        var viewBoundPosX = viewWidth / 200;
        posX = mapRightWorldPosX - viewBoundPosX;
    }

    /// <summary>
    /// 退出关卡时修正相机X位置，防止超出地图边界（全屏旧接口）
    /// </summary>
    /// <param name="viewSize">屏幕分辨率</param>
    /// <param name="playPosX">游玩状态相机X坐标</param>
    /// <param name="mapRightWorldPosX">地图右边界X</param>
    /// <param name="mapPlayHudPosType">HUD对齐方式</param>
    public void ExitPlayAdjustPosX(Vector2 viewSize, float playPosX, float mapRightWorldPosX, RMapPlayHudPosType mapPlayHudPosType)
    {
        if (mapPlayHudPosType == RMapPlayHudPosType.Center)
        {
            posX = playPosX;
            return;
        }

        var viewWidth = viewSize.x / viewSize.y * size * 2 * 100;
        var viewBoundPosX = playPosX + viewWidth / 200;

        if (viewBoundPosX > mapRightWorldPosX)
            posX = playPosX + mapRightWorldPosX - viewBoundPosX;
        else
            posX = playPosX;
    }

    /// <summary>
    /// 退出关卡修正X（仅中间地图红框视口）
    /// </summary>
    public void ExitPlayAdjustPosX_Viewport(Vector2 viewportSize, float playPosX, float mapRightWorldPosX, RMapPlayHudPosType mapPlayHudPosType)
    {
        if (mapPlayHudPosType == RMapPlayHudPosType.Center)
        {
            posX = playPosX;
            return;
        }

        var viewWidth = viewportSize.x / viewportSize.y * size * 2 * 100;
        var viewBoundPosX = playPosX + viewWidth / 200;

        if (viewBoundPosX > mapRightWorldPosX)
            posX = playPosX + mapRightWorldPosX - viewBoundPosX;
        else
            posX = playPosX;
    }

    /// <summary>
    /// 游玩状态根据底部距离计算相机Y位置
    /// </summary>
    /// <param name="normalCamData">正常状态相机数据</param>
    /// <param name="distance">距离底部高度</param>
    public void PlayAdjustPosY(MapCamData normalCamData, float distance)
    {
        posY = distance * 0.01f - (normalCamData.size - size) + normalCamData.posY;
    }
    #endregion
}

//=========================================================================
// 地图核心组件
//=========================================================================
/// <summary>
/// 地图核心配置组件
/// 管理地图参数、相机配置、视图边界计算
/// </summary>
public class MapData : MonoBehaviour
{
    #region 公开字段
    /// <summary>
    /// 所属章节ID
    /// </summary>
    [Tooltip("章节ID")]
    public string m_ChapterId = string.Empty;

    /// <summary>
    /// 关卡ID
    /// </summary>
    [Tooltip("关卡ID")]
    public int m_LevelId;

    /// <summary>
    /// 地图区域碰撞边界
    /// </summary>
    [Tooltip("地图区域Bounds")]
    public MapMotionLayer mapBounds;

    /// <summary>
    /// 地图类型（主线/每日挑战）
    /// </summary>
    [Tooltip("地图类型")]
    public RMapType m_MapType = RMapType.Normal;

    /// <summary>
    /// 是否为平板设备适配
    /// </summary>
    [Tooltip("是否是Pad")]
    public bool m_IsPad;

    /// <summary>
    /// 大厅/正常状态相机参数
    /// </summary>
    public MapCamData m_NormalCamData;

    /// <summary>
    /// 关卡游玩中相机参数
    /// </summary>
    public MapCamData m_PlayCamData;

    /// <summary>
    /// 退出关卡后相机恢复参数
    /// </summary>
    public MapCamData m_ExitPlayCamData;

    /// <summary>
    /// 主大厅最大高度
    /// </summary>
    [Tooltip("主大厅最大高度")]
    public float m_MainHudMaxHeight = 768f;

    /// <summary>
    /// 地图章节配置数据
    /// </summary>
    [FormerlySerializedAs("m_RMapChapterData")]
    public RMapChapterTypeData m_RMapChapterTypeData = new();

    [Header("地图缩放配置")]
    /// <summary>
    /// 相机最小正交Size（限制相机的最大放大倍数）
    /// </summary>
    [Tooltip("兜底最小尺寸，动态计算尺寸不会小于该值")]
    public float m_MinCamSize = 250f;

    /// <summary>
    /// 相机最大正交Size（最大缩放）
    /// </summary>
    [Tooltip("最大相机正交Size(最大放大)")]
    public float m_MaxCamSize = 1024f;

    /// <summary>
    /// 滚轮缩放灵敏度
    /// </summary>
    [Tooltip("滚轮缩放灵敏度")]
    public float m_ScrollSensitivity = 0.5f;
    
    [Header("中间地图红框视口配置（剔除上下标题UI）")]
    [Tooltip("顶部标题HUD固定高度（屏幕像素）")]
    public float m_TopHudPixelHeight = 160f;

    [Tooltip("底部道具HUD固定高度（屏幕像素）")]
    public float m_BottomHudPixelHeight = 180f;

    [Tooltip("最小缩放是否强制居中地图，锁定不可拖动")]
    public bool m_MinScaleLockCenter = true;
    #endregion

    #region 生命周期
    /// <summary>
    /// 初始化
    /// </summary>
    private void Awake()
    {
    }
    #endregion

    #region 公共工具方法
    /// <summary>
    /// 获取大厅标准相机正交Size
    /// </summary>
    /// <param name="viewSize">视图尺寸</param>
    /// <returns>相机Size</returns>
    public float NormalCamSize(Vector2 viewSize)
    {
        return 1024f;
    }

    /// <summary>
    /// 获取中间地图红框Viewport尺寸（扣除顶部标题、底部UI高度）
    /// 统一调用Util工具类，全项目单数据源
    /// </summary>
    public Vector2 GetMapViewportSize()
    {
        return Util.GetMapViewportSize(this);
    }

    /// <summary>
    /// 动态计算适配红框的最小相机正交Size
    /// 保证整张地图完整显示在中间红框视口，无溢出无裁切
    /// </summary>
    public float CalcAutoMinCamSize()
    {
        if (mapBounds == null)
            return m_MinCamSize;

        Vector2 viewport = GetMapViewportSize();
        float viewAspect = viewport.x / viewport.y;

        // 地图包围盒半宽/半高（世界单位）
        float mapHalfW = mapBounds.m_AreaBounds.extents.x;
        float mapHalfH = mapBounds.m_AreaBounds.extents.y;

        // 两种适配维度：按高度铺满 / 按宽度铺满
        float sizeByHeight = mapHalfH;
        float sizeByWidth = mapHalfW / viewAspect;

        // 取最大值保证完整容纳，兜底不小于面板配置最小尺寸
        float autoMinSize = Mathf.Max(sizeByHeight, sizeByWidth);
        return Mathf.Max(autoMinSize, m_MinCamSize);
    }

    /// <summary>
    /// 获取游玩相机可视区域右边界世界X坐标（旧全屏尺寸逻辑，兼容历史代码）
    /// </summary>
    public float PlayHudRightViewWorldPosX()
    {
        var viewSize = Util.GameViewSize();
        var size = m_PlayCamData.size;
        var viewWidth = viewSize.x / viewSize.y * size * 2 * 100;
        var viewBoundPosX = viewWidth / 200;
        return m_PlayCamData.posX + viewBoundPosX;
    }

    /// <summary>
    /// 获取游玩相机可视区域右边界世界X坐标（新：仅中间红框地图视口，剔除上下UI）
    /// </summary>
    public float PlayHudRightViewWorldPosX_Viewport()
    {
        var viewSize = GetMapViewportSize();
        var size = m_PlayCamData.size;
        var viewWidth = viewSize.x / viewSize.y * size * 2 * 100;
        var viewBoundPosX = viewWidth / 200;
        return m_PlayCamData.posX + viewBoundPosX;
    }

    /// <summary>
    /// 获取游玩相机可视区域左边界世界X坐标（旧全屏尺寸逻辑）
    /// </summary>
    public float PlayHudLeftViewWorldPosX()
    {
        var viewSize = Util.GameViewSize();
        var size = m_PlayCamData.size;
        var viewWidth = viewSize.x / viewSize.y * size * 2 * 100;
        var viewBoundPosX = viewWidth / 200;
        return m_PlayCamData.posX - viewBoundPosX;
    }

    /// <summary>
    /// 获取游玩相机可视区域左边界世界X坐标（新：仅中间红框地图视口）
    /// </summary>
    public float PlayHudLeftViewWorldPosX_Viewport()
    {
        var viewSize = GetMapViewportSize();
        var size = m_PlayCamData.size;
        var viewWidth = viewSize.x / viewSize.y * size * 2 * 100;
        var viewBoundPosX = viewWidth / 200;
        return m_PlayCamData.posX - viewBoundPosX;
    }
    #endregion
}