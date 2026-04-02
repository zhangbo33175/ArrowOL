using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MapObjectData
{
    public int id; // 物体唯一ID
    public string type; // 物体类型（如Turtle/Stone）
    public Vector3 position; // 世界坐标
    public Vector3 rotation; // 旋转
    public Vector3 scale; // 缩放
    public Sprite sprite; // 图片信息
    public string name;
    public string m_IconPath;
    public Vector2 size; // 大小
}

[System.Serializable]
public class MapAssetData
{
    /// <summary>
    /// 地图名称
    /// </summary>
    public string m_MapName;
    /// <summary>
    /// 章节Id
    /// </summary>
    public string ChapterId = string.Empty;

    /// <summary>
    /// 关卡Id
    /// </summary>
    public int LevelId;
    
    /// <summary>
    /// 背景图片路径
    /// </summary>
    public string m_BackgroundPath;

    /// <summary>
    /// Icon图片路径
    /// </summary>
    public string m_IconPath;

    /// <summary>
    /// 子物体信息集合
    /// </summary>
    public List<MapObjectData> m_MapObjectData = new List<MapObjectData>();

    /// <summary>
    /// 创建时间
    /// </summary>
    public string m_CcreateTime;

    /// <summary>
    /// 
    /// </summary>
    public int m_MapWidth = 0;

    /// <summary>
    /// 
    /// </summary>
    public int m_MapHeight = 0;
}

/// <summary>
/// 地图数据
/// 适配方式：
/// </summary>
public class MapData : MonoBehaviour
{
    
}