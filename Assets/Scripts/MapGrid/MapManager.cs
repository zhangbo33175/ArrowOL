using UnityEngine;

namespace GameLib
{
    public class GridCell
    {
        public Vector2Int GridPosition { get; private set; }
        public bool IsOccupied { get; set; }
        public GameObject Occupant { get; set; }

        public GridCell(int x, int y)
        {
            GridPosition = new Vector2Int(x, y);
            IsOccupied = false;
            Occupant = null;
        }
    }
    public class MapManager:MonoBehaviour
    {
        [Header("地图设置")] public int size = 10; // 正方形地图的边长（格子数）
        public float cellWidth = 1f; // 格子宽度
        public float cellHeight = 0.5f; // 格子高度

        private GridCell[,] grid;

        void Awake()
        {
            GenerateGrid();
        }

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

        // 世界坐标 → 网格坐标
        public Vector2Int WorldToGrid(Vector3 worldPos)
        {
            float x = worldPos.x / (cellWidth / 2f);
            float y = worldPos.y / (cellHeight / 2f);
            int gridX = Mathf.RoundToInt((x + y) / 2f);
            int gridY = Mathf.RoundToInt((y - x) / 2f);
            
            
            
            return new Vector2Int(gridX, gridY);
        }

        // 网格坐标 → 世界坐标
        public Vector3 GridToWorld(Vector2Int gridPos)
        {
            float x = gridPos.x * (cellWidth / 2f) - gridPos.y * (cellWidth / 2f);
            float y = gridPos.x * (cellHeight / 2f) + gridPos.y * (cellHeight / 2f);
            float worldX = gridPos.x * (cellWidth / 2) - gridPos.y * (cellWidth / 2);
            float worldY = gridPos.x * (cellHeight / 2) + gridPos.y * (cellHeight / 2);
            return new Vector3(worldX, worldY, 0);
        }

        // 检查格子是否被占用
        public bool IsCellOccupied(Vector2Int gridPos)
        {
            if (gridPos.x < 0 || gridPos.x >= size || gridPos.y < 0 || gridPos.y >= size)
                return true;
            return grid[gridPos.x, gridPos.y].IsOccupied;
        }

        // 放置物体
        public bool PlaceObject(GameObject obj, Vector2Int gridPos)
        {
            if (IsCellOccupied(gridPos))
                return false;

            grid[gridPos.x, gridPos.y].IsOccupied = true;
            grid[gridPos.x, gridPos.y].Occupant = obj;
            obj.transform.position = GridToWorld(gridPos);
            return true;
        }

        // 移除物体
        public void RemoveObject(Vector2Int gridPos)
        {
            if (gridPos.x < 0 || gridPos.x >= size || gridPos.y < 0 || gridPos.y >= size)
                return;

            Destroy(grid[gridPos.x, gridPos.y].Occupant);
            grid[gridPos.x, gridPos.y].IsOccupied = false;
            grid[gridPos.x, gridPos.y].Occupant = null;
        }

        // Scene 视图绘制格子
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