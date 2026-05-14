using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 网格单元格数据类
    /// 存储单个格子的坐标、占用状态、占用物体
    /// </summary>
    public class GridCell
    {
        /// <summary>
        /// 网格坐标（X,Y）
        /// </summary>
        public Vector2Int GridPosition { get; private set; }

        /// <summary>
        /// 当前格子是否被物体占用
        /// </summary>
        public bool IsOccupied { get; set; }

        /// <summary>
        /// 占用当前格子的游戏物体
        /// </summary>
        public GameObject Occupant { get; set; }

        /// <summary>
        /// 网格单元格构造函数
        /// </summary>
        /// <param name="x">网格X坐标</param>
        /// <param name="y">网格Y坐标</param>
        public GridCell(int x, int y)
        {
            GridPosition = new Vector2Int(x, y);
            IsOccupied = false;
            Occupant = null;
        }
    }

    /// <summary>
    /// 地图管理核心类
    /// 负责网格生成、坐标转换、物体放置与移除、格子占用检测
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        [Header("地图设置")]
        /// <summary>
        /// 正方形地图边长（格子数量）
        /// </summary>
        public int size = 10;

        /// <summary>
        /// 单个格子宽度
        /// </summary>
        public float cellWidth = 1f;

        /// <summary>
        /// 单个格子高度
        /// </summary>
        public float cellHeight = 0.5f;

        /// <summary>
        /// 二维网格数组（存储所有格子数据）
        /// </summary>
        private GridCell[,] grid;

        /// <summary>
        /// 初始化：生成地图网格
        /// </summary>
        void Awake()
        {
            GenerateGrid();
        }

        /// <summary>
        /// 生成正方形网格
        /// 初始化所有格子数据
        /// </summary>
        void GenerateGrid()
        {
            grid = new GridCell[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    grid[x, y] = new GridCell(x, y);
                }
            }
        }

        /// <summary>
        /// 世界坐标转换为网格坐标
        /// </summary>
        /// <param name="worldPos">世界空间坐标</param>
        /// <returns>对应的网格二维坐标</returns>
        public Vector2Int WorldToGrid(Vector3 worldPos)
        {
            float x = worldPos.x / (cellWidth / 2f);
            float y = worldPos.y / (cellHeight / 2f);
            int gridX = Mathf.RoundToInt((x + y) / 2f);
            int gridY = Mathf.RoundToInt((y - x) / 2f);

            return new Vector2Int(gridX, gridY);
        }

        /// <summary>
        /// 网格坐标转换为世界坐标
        /// </summary>
        /// <param name="gridPos">网格二维坐标</param>
        /// <returns>对应的世界空间坐标</returns>
        public Vector3 GridToWorld(Vector2Int gridPos)
        {
            float worldX = gridPos.x * (cellWidth / 2) - gridPos.y * (cellWidth / 2);
            float worldY = gridPos.x * (cellHeight / 2) + gridPos.y * (cellHeight / 2);
            return new Vector3(worldX, worldY, 0);
        }

        /// <summary>
        /// 检查指定网格是否被占用
        /// </summary>
        /// <param name="gridPos">网格坐标</param>
        /// <returns>true=被占用 / false=未占用</returns>
        public bool IsCellOccupied(Vector2Int gridPos)
        {
            if (gridPos.x < 0 || gridPos.x >= size || gridPos.y < 0 || gridPos.y >= size)
                return true;

            return grid[gridPos.x, gridPos.y].IsOccupied;
        }

        /// <summary>
        /// 在指定网格放置物体
        /// </summary>
        /// <param name="obj">要放置的物体</param>
        /// <param name="gridPos">目标网格坐标</param>
        /// <returns>true=放置成功 / false=放置失败</returns>
        public bool PlaceObject(GameObject obj, Vector2Int gridPos)
        {
            if (IsCellOccupied(gridPos))
                return false;

            grid[gridPos.x, gridPos.y].IsOccupied = true;
            grid[gridPos.x, gridPos.y].Occupant = obj;
            obj.transform.position = GridToWorld(gridPos);
            return true;
        }

        /// <summary>
        /// 移除指定网格上的物体
        /// </summary>
        /// <param name="gridPos">目标网格坐标</param>
        public void RemoveObject(Vector2Int gridPos)
        {
            if (gridPos.x < 0 || gridPos.x >= size || gridPos.y < 0 || gridPos.y >= size)
                return;

            Destroy(grid[gridPos.x, gridPos.y].Occupant);
            grid[gridPos.x, gridPos.y].IsOccupied = false;
            grid[gridPos.x, gridPos.y].Occupant = null;
        }

        /// <summary>
        /// 在Scene视图绘制网格Gizmos
        /// 绿色=未占用 红色=已占用
        /// </summary>
        void OnDrawGizmos()
        {
            if (grid == null)
                GenerateGrid();

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Vector3 worldPos = GridToWorld(new Vector2Int(x, y));
                    Gizmos.color = grid[x, y].IsOccupied ? Color.red : Color.green;
                    Gizmos.DrawWireCube(worldPos, new Vector3(cellWidth, cellHeight, 0));
                }
            }
        }
    }
}