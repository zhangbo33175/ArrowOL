using UnityEngine;

namespace GameLib
{
    public partial class GridMapManager
    {
        public class GridUtils
        {
            #region 45°网格系统坐标转换

            /// <summary>
            /// 世界坐标 → 网格坐标
            /// </summary>
            /// <param name="worldPos">世界坐标</param>
            /// <param name="cellWidth">网格单元宽度</param>
            /// <param name="cellHeight">网格单元高度</param>
            /// <returns></returns>
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
            /// <returns></returns>
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
            /// <param name="gridWidth"></param>
            /// <param name="gridHeight"></param>
            /// <param name="grid"></param>
            /// <returns></returns>
            public static bool IsCellOccupied(Vector2Int gridPos,int gridWidth,int gridHeight,GridTile[,] grid)
            {
                if (gridPos.x < 0 || gridPos.x >= gridWidth || gridPos.y < 0 || gridPos.y >= gridHeight)
                    return true; // 边界视为占用
                return grid[gridPos.x, gridPos.y].IsOccupied;
            }

            
            #endregion
        }
    }
}