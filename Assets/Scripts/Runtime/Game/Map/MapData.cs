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
using UnityEngine.Serialization;

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

    /// <summary>世界坐标换算系数（视图宽高比 × 相机Size × 2 × 100）</summary>
    private const float k_WorldViewScaleFactor = 100f;
    /// <summary>世界半宽换算除数</summary>
    private const float k_HalfViewDivisor = 200f;
    /// <summary>像素高度 → 世界坐标换算系数</summary>
    private const float k_PixelToWorldScale = 0.01f;

    /// <summary>计算可视半宽（世界坐标）</summary>
    private float CalcViewHalfWorldWidth(Vector2 viewSize)
    {
        return viewSize.x / viewSize.y * size * k_WorldViewScaleFactor / k_HalfViewDivisor;
    }

    /// <summary>标准化调整相机Y轴位置</summary>
    public void NormalAdjustPosY(float designHeight)
    {
        posY = (size * 2f - designHeight * k_PixelToWorldScale) * 0.5f;
    }

    /// <summary>左对齐模式标准化相机X轴位置（红框视口）</summary>
    public void NormalAdjustLeftPosX_Viewport(Vector2 viewportSize, float mapLeftWorldPosX)
    {
        posX = mapLeftWorldPosX + CalcViewHalfWorldWidth(viewportSize);
    }

    /// <summary>右对齐模式标准化相机X轴位置（红框视口）</summary>
    public void NormalAdjustRightPosX_Viewport(Vector2 viewportSize, float mapRightWorldPosX)
    {
        posX = mapRightWorldPosX - CalcViewHalfWorldWidth(viewportSize);
    }

    /// <summary>退出关卡修正相机X位置（红框视口）</summary>
    public void ExitPlayAdjustPosX_Viewport(Vector2 viewportSize, float playPosX,
        float mapRightWorldPosX, RMapPlayHudPosType hudPosType)
    {
        if (hudPosType == RMapPlayHudPosType.Center) { posX = playPosX; return; }
        float viewBoundPosX = playPosX + CalcViewHalfWorldWidth(viewportSize);
        posX = viewBoundPosX > mapRightWorldPosX ? playPosX + mapRightWorldPosX - viewBoundPosX : playPosX;
    }

    /// <summary>游玩状态根据底部距离计算相机Y位置</summary>
    public void PlayAdjustPosY(MapCamData normalCamData, float distance)
    {
        posY = distance * k_PixelToWorldScale - (normalCamData.size - size) + normalCamData.posY;
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

    [Tooltip("章节ID")]
    public string m_ChapterId = string.Empty;
    public int m_LevelId;
    public MapMotionLayer mapBounds;
    public RMapType m_MapType = RMapType.Normal;
    public bool m_IsPad;

    [Header("相机配置")]
    public MapCamData m_NormalCamData;
    public MapCamData m_PlayCamData;
    public MapCamData m_ExitPlayCamData;
    public float m_MainHudMaxHeight = 768f;
    public RMapChapterTypeData m_RMapChapterTypeData = new();

    [Header("地图缩放配置")]
    public float m_MinCamSize = 250f;
    public float m_MaxCamSize = 1024f;
    public float m_ScrollSensitivity = 0.5f;

    [Header("中间地图红框视口配置")]
    public float m_TopHudPixelHeight = 160f;
    public float m_BottomHudPixelHeight = 180f;
    public bool m_MinScaleLockCenter = true;

    #endregion

    #region 公共工具方法

    private const float k_WorldViewScaleFactor = 100f;
    private const float k_HalfViewDivisor = 200f;

    /// <summary>计算可视半宽（世界坐标）</summary>
    private float CalcViewHalfWorldWidth(float camSize, Vector2 viewSize)
    {
        return viewSize.x / viewSize.y * camSize * k_WorldViewScaleFactor / k_HalfViewDivisor;
    }

    /// <summary>获取中间地图红框Viewport尺寸</summary>
    public Vector2 GetMapViewportSize() => Util.GetMapViewportSize(this);

    /// <summary>动态计算适配红框的最小相机正交Size</summary>
    public float CalcAutoMinCamSize()
    {
        float screenH = Screen.height;
        float viewH = Mathf.Max(0, screenH - m_TopHudPixelHeight - m_BottomHudPixelHeight);
        float ratio = viewH > 0 ? viewH / screenH : 1f;
        return Mathf.Max(m_NormalCamData.size * ratio, m_MinCamSize);
    }

    /// <summary>设置底部UI高度（运行时动态调整）</summary>
    public void SetBottomHudHeight(float pixelHeight)
    {
        m_BottomHudPixelHeight = Mathf.Max(pixelHeight, 0f);
    }

    /// <summary>重新计算大厅相机参数（根据当前视口自动适配）</summary>
    public void RecalcNormalCamData()
    {
        m_NormalCamData.size = CalcAutoMinCamSize();
        m_NormalCamData.NormalAdjustPosY(m_MainHudMaxHeight);
    }

    /// <summary>获取游玩相机可视区域右边界世界X坐标（红框视口）</summary>
    public float PlayHudRightViewWorldPosX_Viewport()
    {
        return m_PlayCamData.posX + CalcViewHalfWorldWidth(m_PlayCamData.size, GetMapViewportSize());
    }

    /// <summary>获取游玩相机可视区域左边界世界X坐标（红框视口）</summary>
    public float PlayHudLeftViewWorldPosX_Viewport()
    {
        return m_PlayCamData.posX - CalcViewHalfWorldWidth(m_PlayCamData.size, GetMapViewportSize());
    }

    #endregion
}
