/***************************************************************
 * (c) copyright 2026 - 2030, 项目版权所有
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapPathDrawer.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图路径绘制器
 *            用于在据点之间绘制连线，支持颜色修改、路径清理
 ***************************************************************/

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 地图路径绘制器
/// 绘制据点之间的连接路径
/// </summary>
public class MapPathDrawer : MonoBehaviour
{
    #region 序列化配置字段
    //=========================================================================
    // 序列化配置字段
    //=========================================================================
    [Header("路径配置")]
    [Tooltip("路径线条预制体（带LineRenderer）")]
    public LineRenderer pathPrefab;

    [Tooltip("路径默认颜色")]
    public Color pathColor = Color.gray;

    [Tooltip("路径线条宽度")]
    public float pathWidth = 0.1f;
    #endregion

    #region 私有字段
    //=========================================================================
    // 私有字段
    //=========================================================================
    /// <summary>
    /// 路径字典：存储所有已创建的路径（ID -> LineRenderer）
    /// </summary>
    private readonly Dictionary<int, LineRenderer> _pathDict = new Dictionary<int, LineRenderer>();
    #endregion

    #region 公共路径控制方法
    //=========================================================================
    // 公共路径控制方法
    //=========================================================================
    /// <summary>
    /// 绘制两个标记之间的路径
    /// </summary>
    /// <param name="pathId">路径唯一ID</param>
    /// <param name="startMarker">起点标记</param>
    /// <param name="endMarker">终点标记</param>
    public void DrawPath(int pathId, MapMarker startMarker, MapMarker endMarker)
    {
        // 如果路径已存在，先销毁旧路径
        if (_pathDict.ContainsKey(pathId))
        {
            Destroy(_pathDict[pathId].gameObject);
            _pathDict.Remove(pathId);
        }

        // 创建新路径线条
        LineRenderer line = Instantiate(pathPrefab, transform);
        line.name = $"Path_{pathId}";
        line.startColor = pathColor;
        line.endColor = pathColor;
        line.startWidth = pathWidth;
        line.endWidth = pathWidth;

        // 设置路径起点与终点
        line.positionCount = 2;
        line.SetPosition(0, startMarker.GetWorldPosition());
        line.SetPosition(1, endMarker.GetWorldPosition());

        // 存入字典管理
        _pathDict.Add(pathId, line);
    }

    /// <summary>
    /// 清除所有已绘制的路径
    /// </summary>
    public void ClearAllPaths()
    {
        foreach (var path in _pathDict.Values)
        {
            Destroy(path.gameObject);
        }
        _pathDict.Clear();
    }

    /// <summary>
    /// 修改指定路径的颜色
    /// </summary>
    /// <param name="pathId">路径ID</param>
    /// <param name="color">目标颜色</param>
    public void SetPathColor(int pathId, Color color)
    {
        if (_pathDict.TryGetValue(pathId, out LineRenderer line))
        {
            line.startColor = color;
            line.endColor = color;
        }
    }
    #endregion
}