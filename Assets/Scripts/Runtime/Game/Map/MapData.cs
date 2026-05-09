using UnityEngine;
using System.Collections.Generic;
using GameLib;
using UnityEngine.Serialization;

/// <summary>
/// 地图图标显示类型
/// </summary>
public enum RMapIconType
{
    /// <summary>
    /// 染色模式
    /// </summary>
    Coloring = 0,

    /// <summary>
    /// 选中消散效果
    /// </summary>
    Dissipate = 1,
}

/// <summary>
/// 地图单个物体数据（图标/节点）
/// </summary>
[System.Serializable]
public class RMapData
{
    /// <summary>
    /// 物体唯一标识ID
    /// </summary>
    public int m_Id;

    /// <summary>
    /// 物体显示类型
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
    /// 精灵图片路径/名称
    /// </summary>
    public string m_Sprite;

    /// <summary>
    /// 物体显示名称
    /// </summary>
    public string m_Name;

    /// <summary>
    /// 图片尺寸大小
    /// </summary>
    public Vector2 m_Size;

    /// <summary>
    /// 当前是否被选中
    /// </summary>
    public bool m_IsChoose;
}

/// <summary>
/// 地图章节配置数据
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
    public int m_MapWidth = 0;

    /// <summary>
    /// 地图高度（像素/单位）
    /// </summary>
    public int m_MapHeight = 0;

    /// <summary>
    /// 配置保存路径
    /// </summary>
    public string SavePath = "";
}

/// <summary>
/// 关卡游玩时 HUD 对齐方式
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
    Center = 1,
}

/// <summary>
/// 地图相机参数数据
/// 负责相机尺寸、位置计算与适配逻辑
/// </summary>
[System.Serializable]
public class MapCamData
{
    /// <summary>
    /// 相机正交大小
    /// </summary>
    public float size;

    /// <summary>
    /// 相机 X 轴位置
    /// </summary>
    public float posX;

    /// <summary>
    /// 相机 Y 轴位置
    /// </summary>
    public float posY;

    /// <summary>
    /// 标准化调整相机 Y 轴位置
    /// </summary>
    /// <param name="designHeight">设计高度</param>
    public void NormalAdjustPosY(float designHeight)
    {
        posY = (size * 2 - designHeight * 0.01f) * 0.5f;
    }

    /// <summary>
    /// 标准化左对齐模式下的相机 X 轴位置
    /// </summary>
    /// <param name="viewSize">视图尺寸</param>
    /// <param name="mapLeftWorldPosX">地图左边界世界坐标X</param>
    public void NormalAdjustLeftPosX(Vector2 viewSize, float mapLeftWorldPosX)
    {
        var viewWidth = viewSize.x / (viewSize.y * 1.000f) * size * 2 * 100;
        var viewBoundPosX = viewWidth / 200;

        posX = mapLeftWorldPosX + viewBoundPosX;
    }

    /// <summary>
    /// 标准化右对齐模式下的相机 X 轴位置
    /// </summary>
    /// <param name="viewSize">视图尺寸</param>
    /// <param name="mapRightWorldPosX">地图右边界世界坐标X</param>
    public void NormalAdjustRightPosX(Vector2 viewSize, float mapRightWorldPosX)
    {
        var viewWidth = viewSize.x / (viewSize.y * 1.000f) * size * 2 * 100;
        var viewBoundPosX = viewWidth / 200;

        posX = mapRightWorldPosX - viewBoundPosX;
    }

    /// <summary>
    /// 退出关卡时重新计算相机 X 轴位置
    /// 防止超出地图边界
    /// </summary>
    /// <param name="viewSize">屏幕分辨率</param>
    /// <param name="playPosX">游玩时相机X坐标</param>
    /// <param name="mapRightWorldPosX">地图右边界X</param>
    /// <param name="mapPlayHudPosType">HUD对齐方式</param>
    public void ExitPlayAdjustPosX(Vector2 viewSize, float playPosX, float mapRightWorldPosX, RMapPlayHudPosType mapPlayHudPosType)
    {
        if (mapPlayHudPosType == RMapPlayHudPosType.Center)
        {
            posX = playPosX;
            return;
        }

        var viewWidth = viewSize.x / (viewSize.y * 1.000f) * size * 2 * 100;
        var viewBoundPosX = playPosX + viewWidth / 200;

        if (viewBoundPosX > mapRightWorldPosX)
        {
            posX = playPosX + mapRightWorldPosX - viewBoundPosX;
        }
        else
        {
            posX = playPosX;
        }
    }

    /// <summary>
    /// 游玩关卡时根据距离底部高度计算相机 Y 轴位置
    /// </summary>
    /// <param name="normalCamData">正常状态相机数据</param>
    /// <param name="distance">距离底部高度</param>
    public void PlayAdjustPosY(MapCamData normalCamData, float distance)
    {
        posY = distance * 0.01f - (normalCamData.size - size) + normalCamData.posY;
    }
}

/// <summary>
/// 地图类型
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
    DailyLevel = 1,
}

/// <summary>
/// 地图默认相机位置类型
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
}

/// <summary>
/// 地图主配置组件
/// 负责地图参数、相机配置、视图边界计算
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
    /// 地图相机初始位置类型
    /// </summary>
    [Tooltip("地图相机位置类型")]
    public RMapCamPosType m_MapCamPosType = RMapCamPosType.Right;

    /// <summary>
    /// 关卡游玩时 HUD 对齐方式
    /// </summary>
    [Tooltip("游玩关卡对齐方式类型")]
    public RMapPlayHudPosType m_MapPlayHudPosType = RMapPlayHudPosType.Left;

    /// <summary>
    /// 地图类型（主线/每日）
    /// </summary>
    [Tooltip("地图类型")]
    public RMapType m_MapType = RMapType.Normal;

    /// <summary>
    /// 是否为平板设备适配
    /// </summary>
    [Tooltip("是否是Pad")]
    public bool m_IsPad = false;

    /// <summary>
    /// 大厅/正常状态相机数据
    /// </summary>
    public MapCamData m_NormalCamData;

    /// <summary>
    /// 关卡游玩中相机数据
    /// </summary>
    public MapCamData m_PlayCamData;

    /// <summary>
    /// 退出关卡后相机恢复数据
    /// </summary>
    public MapCamData m_ExitPlayCamData;

    /// <summary>
    /// 地图章节配置数据
    /// </summary>
    [FormerlySerializedAs("m_RMapChapterData")]
    public RMapChapterTypeData mRMapChapterTypeData = new();

    /// <summary>
    /// 获取游玩相机可视区域 **右边界** 世界坐标X
    /// </summary>
    public float PlayHudRightViewWorldPosX()
    {
        var viewSize = Util.GameViewSize();
        var size = m_PlayCamData.size;
        var viewWidth = viewSize.x / (viewSize.y * 1.000f) * size * 2 * 100;
        var viewBoundPosX = viewWidth / 200;

        var posX = m_PlayCamData.posX + viewBoundPosX;
        return posX;
    }

    /// <summary>
    /// 获取游玩相机可视区域 **左边界** 世界坐标X
    /// </summary>
    public float PlayHudLeftViewWorldPosX()
    {
        var viewSize = Util.GameViewSize();
        var size = m_PlayCamData.size;
        var viewWidth = viewSize.x / (viewSize.y * 1.000f) * size * 2 * 100;
        var viewBoundPosX = viewWidth / 200;

        var posX = m_PlayCamData.posX - viewBoundPosX;
        return posX;
    }
}