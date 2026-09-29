/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GridMapManager.Utils.cs
 * author:    云毅
 * created:   2026
 * descrip:   网格地图管理器 - 45° 等距网格坐标转换与占用检测工具
 ***************************************************************/

using UnityEngine;

namespace GameLib
{
    public partial class GridMapManager
    {
        /// <summary>
        /// 网格坐标转换与占用检测工具（静态嵌套类）
        /// </summary>
        public class GridUtils
        {
            #region 45°网格系统坐标转换

            /// <summary>
            /// 世界坐标 → 网格坐标
            /// </summary>
            /// <param name="worldPos">世界坐标</param>
            /// <param name="cellWidth">网格单元宽度</param>
            /// <param name="cellHeight">网格单元高度</param>
            /// <returns>换算后的网格坐标</returns>
            public static Vector2Int WorldToGrid(Vector3 worldPos, int cellWidth, int cellHeight)
            {
                float x = worldPos.x / (cellWidth / 2f);
                float y = worldPos.y / (cellHeight / 2f);
                int gridX = Mathf.RoundToInt((x + y) / 2f);
                int gridY = Mathf.RoundToInt((y - x) / 2f);
                return new Vector2Int(gridX, gridY);
            }

            /// <summary>
            /// 网格坐标 → 世界坐标
            /// </summary>
            /// <param name="gridPos">网格坐标</param>
            /// <param name="cellWidth">网格单元宽度</param>
            /// <param name="cellHeight">网格单元高度</param>
            /// <returns>换算后的世界坐标</returns>
            public static Vector3 GridToWorld(Vector2Int gridPos, int cellWidth, int cellHeight)
            {
                float x = gridPos.x * (cellWidth / 2f) - gridPos.y * (cellWidth / 2f);
                float y = gridPos.x * (cellHeight / 2f) + gridPos.y * (cellHeight / 2f);
                return new Vector3(x, y, 0);
            }

            /// <summary>
            /// 检查网格是否被占用
            /// </summary>
            /// <param name="gridPos">当前坐标信息</param>
            /// <param name="gridWidth">网格列数（X 方向长度）</param>
            /// <param name="gridHeight">网格行数（Y 方向长度）</param>
            /// <param name="grid">网格地块数组</param>
            /// <returns>true=被占用或越界 / false=空闲可用</returns>
            public static bool IsCellOccupied(Vector2Int gridPos, int gridWidth, int gridHeight, GridTile[,] grid)
            {
                if (gridPos.x < 0 || gridPos.x >= gridWidth || gridPos.y < 0 || gridPos.y >= gridHeight)
                    return true; // 边界视为占用
                return grid[gridPos.x, gridPos.y].IsOccupied;
            }

            #endregion
        }
    }
}
