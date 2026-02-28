using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 地图路径绘制器
/// 绘制据点之间的连接路径
/// </summary>
public class MapPathDrawer : MonoBehaviour
{
    [Header("路径配置")]
    public LineRenderer pathPrefab; // 路径线预制体
    public Color pathColor = Color.gray; // 路径颜色
    public float pathWidth = 0.1f; // 路径宽度

    private Dictionary<int, LineRenderer> _pathDict = new Dictionary<int, LineRenderer>();

    /// <summary>
    /// 绘制两个标记之间的路径
    /// </summary>
    /// <param name="pathId">路径唯一ID</param>
    /// <param name="startMarker">起点标记</param>
    /// <param name="endMarker">终点标记</param>
    public void DrawPath(int pathId, MapMarker startMarker, MapMarker endMarker)
    {
        // 如果路径已存在，先销毁
        if (_pathDict.ContainsKey(pathId))
        {
            Destroy(_pathDict[pathId].gameObject);
            _pathDict.Remove(pathId);
        }

        // 创建路径线
        LineRenderer line = Instantiate(pathPrefab, transform);
        line.name = $"Path_{pathId}";
        line.startColor = pathColor;
        line.endColor = pathColor;
        line.startWidth = pathWidth;
        line.endWidth = pathWidth;

        // 设置路径点
        line.positionCount = 2;
        line.SetPosition(0, startMarker.GetWorldPosition());
        line.SetPosition(1, endMarker.GetWorldPosition());

        // 保存路径引用
        _pathDict.Add(pathId, line);
    }

    /// <summary>
    /// 清除所有路径
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
    /// 修改路径颜色
    /// </summary>
    public void SetPathColor(int pathId, Color color)
    {
        if (_pathDict.ContainsKey(pathId))
        {
            LineRenderer line = _pathDict[pathId];
            line.startColor = color;
            line.endColor = color;
        }
    }
}