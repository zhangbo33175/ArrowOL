using UnityEngine;
using System.Collections.Generic;
using GameLib;
using UnityEngine.Serialization;

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
    /// 精灵图片资源路径
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
    public string m_CcreateTime;

    /// <summary>
    /// 地图宽度（像素/单位）
    /// </summary>
    public int m_MapWidth;

    /// <summary>
    /// 地图高度（像素/单位）
    /// </summary>
    public int m_MapHeight;

    /// <summary>
    /// 配置保存路径
    /// </summary>
    public string SavePath = string.Empty;
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

    /// <summary>
    /// 标准化调整相机Y轴位置
    /// </summary>
    /// <param name="designHeight">设计高度</param>
    public void NormalAdjustPosY(float designHeight)
    {
        posY = (size * 2 - designHeight * 0.01f) * 0.5f;
    }

    /// <summary>
    /// 左对齐模式下标准化相机X轴位置
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
    /// 右对齐模式下标准化相机X轴位置
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
    /// 退出关卡时修正相机X位置，防止超出地图边界
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
    /// 游玩状态根据底部距离计算相机Y位置
    /// </summary>
    /// <param name="normalCamData">正常状态相机数据</param>
    /// <param name="distance">距离底部高度</param>
    public void PlayAdjustPosY(MapCamData normalCamData, float distance)
    {
        posY = distance * 0.01f - (normalCamData.size - size) + normalCamData.posY;
    }
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

/// <summary>
/// 地图核心配置组件
/// 管理地图参数、相机配置、视图边界计算
/// </summary>
public class MapData : MonoBehaviour
{
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
    [Tooltip("最小相机正交Size(相机可放大到的最大值，数值越大画面显示内容越少)")]
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

    /// <summary>
    /// 初始化
    /// </summary>
    private void Awake()
    {
    }

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
    /// 获取游玩相机可视区域右边界世界X坐标
    /// </summary>
    /// <returns>右边界X</returns>
    public float PlayHudRightViewWorldPosX()
    {
        var viewSize = Util.GameViewSize();
        var size = m_PlayCamData.size;
        var viewWidth = viewSize.x / viewSize.y * size * 2 * 100;
        var viewBoundPosX = viewWidth / 200;
        return m_PlayCamData.posX + viewBoundPosX;
    }

    /// <summary>
    /// 获取游玩相机可视区域左边界世界X坐标
    /// </summary>
    /// <returns>左边界X</returns>
    public float PlayHudLeftViewWorldPosX()
    {
        var viewSize = Util.GameViewSize();
        var size = m_PlayCamData.size;
        var viewWidth = viewSize.x / viewSize.y * size * 2 * 100;
        var viewBoundPosX = viewWidth / 200;
        return m_PlayCamData.posX - viewBoundPosX;
    }
}