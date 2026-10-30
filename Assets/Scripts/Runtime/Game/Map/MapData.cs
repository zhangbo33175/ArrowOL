/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapData.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图系统核心数据类
 *            管理地图配置、相机参数、视图边界、图标数据等核心逻辑
 ***************************************************************/

using UnityEngine;
using System.Collections.Generic;
using GameLib;

//=========================================================================
// 地图枚举定义
//=========================================================================

/// <summary>地图图标显示效果类型</summary>
public enum RMapIconType { Coloring = 0, Dissipate = 1 }

/// <summary>关卡游玩时HUD界面对齐方式</summary>
public enum RMapPlayHudPosType { Left = 0, Center = 1 }

/// <summary>地图功能类型</summary>
public enum RMapType { Normal = 0, DailyLevel = 1 }

/// <summary>走路路径循环模式</summary>
public enum RMapWalkLoopMode { Once = 0, PingPong = 1, Loop = 2 }

/// <summary>地图默认相机初始位置类型</summary>
public enum RMapCamPosType { Left = 0, Right = 1, Center = 2 }

//=========================================================================
// 地图数据实体类
//=========================================================================

/// <summary>地图单个图标/节点数据实体</summary>
[System.Serializable]
public class RMapData
{
    public int m_Id;
    public RMapIconType m_Type = RMapIconType.Dissipate;
    public Vector3 m_Position;
    public Vector3 m_Rotation;
    public Vector3 m_Scale;
    public string m_Sprite;
    public string m_Name;
    public Vector2 m_Size;
    public bool m_IsChoose;

    // 动画相关
    public bool m_IsAnimated;
    public string m_AnimationName = string.Empty;
    public float m_AnimationSpeed = 1.0f;
    public bool m_IsAnimationLoop = true;

    // 走路路径相关
    public bool m_HasWalkPath;
    public List<Vector2> m_WalkPathPoints = new List<Vector2>();
    public float m_WalkSpeed = 100f;
    public RMapWalkLoopMode m_WalkLoopMode = RMapWalkLoopMode.Loop;
}

/// <summary>地图章节配置数据实体</summary>
[System.Serializable]
public class RMapChapterTypeData
{
    public string m_MapName;
    public string ChapterId = string.Empty;
    public int LevelId;
    public int MaplId;
    public string m_BackgroundPath;
    public List<RMapData> m_MapObjectData = new List<RMapData>();
    public string m_CreateTime;
    public string SavePath = string.Empty;

    /// <summary>新建地图章节配置数据实体</summary>
    public static RMapChapterTypeData Create(string chapterId, int levelId, int maplId, string mapName,
        string createTime, string backgroundPath)
    {
        return new RMapChapterTypeData
        {
            ChapterId = chapterId,
            LevelId = levelId,
            MaplId = maplId,
            m_MapName = mapName,
            m_CreateTime = createTime,
            m_BackgroundPath = backgroundPath,
        };
    }
}

/// <summary>
/// 地图相机参数数据
/// 负责相机尺寸、位置计算与多状态适配
/// </summary>
[System.Serializable]
public class MapCamData
{
    public float size;
    public float posX;
    public float posY;

    #region 相机位置计算方法

    /// <summary>像素高度 → 世界坐标换算系数</summary>
    private const float k_PixelToWorldScale = 0.01f;

    /// <summary>标准化调整相机Y轴位置</summary>
    public void NormalAdjustPosY(float designHeight)
    {
        posY = (size * 2f - designHeight * k_PixelToWorldScale) * 0.5f;
    }

    #endregion
}

//=========================================================================
// 地图核心组件
//=========================================================================

/// <summary>
/// 地图相机运动配置数据
/// 管理拖动、缩放、惯性滑动、自动扫描等可调参数（Inspector 可编辑）
/// </summary>
[System.Serializable]
public class MapMotionConfig
{
    [Tooltip("Canvas 设计高度")]
    public float m_CanvasDesignHeight = 1334f;
    
    [Tooltip("缩放速度系数（双指/滚轮）")]
    public float m_ZoomSpeed = 1.1f;
    [Tooltip("最小缩放比例（相对于铺满屏幕 ortho）")]
    public float m_MinZoomRatio = 0.5f;
    [Tooltip("拖动阈值（像素，小于则判定为单击）")]
    public float m_DragThreshold = 12f;
    
    [Tooltip("平滑滑动最小值")]
    public float m_SwipeSmoothMin = 0.3f;
    [Tooltip("平滑滑动最大值")]
    public float m_SwipeSmoothMax = 1.0f;
    [Tooltip("平滑步进系数")]
    public float m_StepAddFactor = 2f;
    [Tooltip("惯性衰减系数")]
    public float m_InertiaDecay = 0.025f;
    [Tooltip("惯性后平滑下限")]
    public float m_InertiaSmoothFloor = 0.75f;
    
    [Tooltip("扫描步进重叠率（0.8=步长为视野80%）")]
    public float m_AutoScanOverlap = 0.8f;
    [Tooltip("扫描每段动画时长（秒）")]
    public float m_AutoScanSegmentDuration = 1.2f;
    [Tooltip("扫描默认正交尺寸")]
    public float m_AutoScanDefaultOrtho = 300f;
    [Tooltip("扫描启动宽限帧数（避免启动点击立刻停掉）")]
    public int m_AutoScanGraceFrames = 15;
}

/// <summary>
/// 地图核心配置组件
/// 管理地图参数、相机配置、视图边界计算
/// </summary>
public class MapData : MonoBehaviour
{
    #region 公开字段

    [Header("地图基础配置")]
    [Tooltip("章节ID")]
    public string m_ChapterId = string.Empty;
    [Tooltip("关卡ID")]
    public int m_LevelId;
    [Tooltip("地图边界组件")]
    public MapMotionLayer mapBounds;
    [Tooltip("地图类型")]
    public RMapType m_MapType = RMapType.Normal;
    [Tooltip("是否Pad端")]
    public bool m_IsPad;

    [Header("相机配置")]
    [Tooltip("大厅相机数据")]
    public MapCamData m_NormalCamData;
    [Tooltip("游玩相机数据")]
    public MapCamData m_PlayCamData;
    [Tooltip("退出游玩相机数据")]
    public MapCamData m_ExitPlayCamData;
    [Tooltip("主HUD最大高度")]
    public float m_MainHudMaxHeight = 768f;
    [Tooltip("地图章节配置数据")]
    public RMapChapterTypeData m_RMapChapterTypeData = new();

    [Header("地图缩放配置")]
    [Tooltip("最小相机尺寸")]
    public float m_MinCamSize = 250f;
    [Tooltip("最大相机尺寸")]
    public float m_MaxCamSize = 1024f;
    [Tooltip("滚轮灵敏度")]
    public float m_ScrollSensitivity = 0.5f;

    [Header("中间地图红框视口配置")]
    [Tooltip("顶部HUD像素高度")]
    public float m_TopHudPixelHeight = 160f;
    [Tooltip("底部HUD像素高度")]
    public float m_BottomHudPixelHeight = 180f;
    [Tooltip("最小缩放时锁定中心")]
    public bool m_MinScaleLockCenter = true;

    [Header("相机运动配置")]
    [Tooltip("相机运动参数（拖动/缩放/惯性/自动扫描）")]
    public MapMotionConfig m_MotionConfig = new MapMotionConfig();

    #endregion

    #region 公共工具方法

    /// <summary>动态计算适配红框的最小相机正交Size</summary>
    public float CalcAutoMinCamSize()
    {
        float screenH = Screen.height;
        float viewH = Mathf.Max(0, screenH - m_TopHudPixelHeight - m_BottomHudPixelHeight);
        float ratio = viewH > 0 ? viewH / screenH : 1f;
        return Mathf.Max(m_NormalCamData.size * ratio, m_MinCamSize);
    }

    /// <summary>重新计算大厅相机参数（根据当前视口自动适配）</summary>
    public void RecalcNormalCamData()
    {
        m_NormalCamData.size = CalcAutoMinCamSize();
        m_NormalCamData.NormalAdjustPosY(m_MainHudMaxHeight);
    }

    #endregion
}
