/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GridManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   网格管理系统 - 坐标转换、格子占用、路径追踪、调试绘制
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 网格管理系统（单例）
    /// 提供：世界坐标 ↔ 网格坐标转换、格子占用管理、对象路径追踪、调试网格绘制
    /// 支持 2D / 3D 双模式
    /// </summary>
    [AddComponentMenu("Honor Core/Manager/GridManager")]
    [LuaCallCSharp]
    public class GridManager : MonoSingleton<GridManager>
    {
        
        //=========================================================================
        #region 序列化字段（Inspector 配置）
        //=========================================================================

        [GameHeader("网格配置")]
        [GameTitle("网格原点 (GridOrigin)")]
        [Tooltip("网格在世界空间下的原始点。")]
        public Transform GridOrigin;

        [GameTitle("格子尺寸 (GridUnitSize)")]
        [Tooltip("网格区域中每个格子的尺寸大小（与地图编辑器 GridSize=32px 对齐）。")]
        public float GridUnitSize = 32f;

        [GameHeader("调试配置")]
        [GameTitle("绘制调试网格 (DrawDebugGrid)")]
        [Tooltip("作为是否绘制调试网格的总开关。")]
        public bool DrawDebugGrid = true;

        [GameTitle("2D/3D模式 (DebugDrawMode)")]
        [GameCondition("DrawDebugGrid", true)]
        [Tooltip("以2D/3D的哪一种空间维度来绘制网格信息。")]
        public GameDefinitions.DimensionMode DebugDrawMode = GameDefinitions.DimensionMode.Two;

        [GameTitle("网格数量 (DebugGridSize)")]
        [GameCondition("DrawDebugGrid", true)]
        [Tooltip("宽 = 网格数量 X 2，高 = 网格数量 X 2。")]
        public int DebugGridSize = 30;

        [GameTitle("网格线颜色 (CellBorderColor)")]
        [GameCondition("DrawDebugGrid", true)]
        [Tooltip("网格绘制用到的网格线颜色。")]
        public Color CellBorderColor = new Color(60f, 221f, 255f, 1f);

        [GameTitle("网格填充颜色 (InnerColor)")]
        [GameCondition("DrawDebugGrid", true)]
        [Tooltip("网格绘制用到的网格填充色。")]
        public Color InnerColor = new Color(60f, 221f, 255f, 0.3f);

        #endregion

        //=========================================================================
        #region 运行时数据（隐藏 Inspector）
        //=========================================================================

        /// <summary>
        /// 所有已被占用的格子世界坐标列表
        /// </summary>
        [HideInInspector]
        public List<Vector3> OccupiedGridCells;

        /// <summary>
        /// 所有注册对象的上一次网格索引位置
        /// </summary>
        [HideInInspector]
        public Dictionary<GameObject, Vector3Int> LastPositions;

        /// <summary>
        /// 所有注册对象的下一个目标网格索引位置
        /// </summary>
        [HideInInspector]
        public Dictionary<GameObject, Vector3Int> NextPositions;

        #endregion

        //=========================================================================
        #region 临时计算变量（避免 GC）
        //=========================================================================

        protected Vector3 m_NewGridPosition;
        protected Vector3 m_DebugOrigin = Vector3.zero;
        protected Vector3 m_DebugDestination = Vector3.zero;
        protected Vector3Int m_WorkCoordinate = Vector3Int.zero;

        #endregion

        //=========================================================================
        #region 生命周期
        //=========================================================================

        /// <summary>
        /// 本管理器挂载在 GameMainMap（Canvas 下的 RectTransform 子节点），
        /// 不需要跨场景持久化；覆写为 false 避免 Awake 时被 SetParent(null) 拔根，
        /// 导致 GridOrigin 世界坐标跳变、调试网格画到相机看不到的位置。
        /// </summary>
        protected override bool ShouldPersistAcrossScenes => false;

        /// <summary>
        /// 初始化所有网格管理容器
        /// </summary>
        protected virtual void Start()
        {
            OccupiedGridCells = new List<Vector3>();
            LastPositions = new Dictionary<GameObject, Vector3Int>();
            NextPositions = new Dictionary<GameObject, Vector3Int>();
        }

        #endregion

        //=========================================================================
        #region 格子占用管理
        //=========================================================================

        /// <summary>
        /// 判断指定格子是否被占用
        /// </summary>
        /// <param name="cellCoordinates">格子世界坐标</param>
        public virtual bool CellIsOccupied(Vector3 cellCoordinates)
        {
            return OccupiedGridCells.Contains(cellCoordinates);
        }

        /// <summary>
        /// 标记一个格子为占用状态
        /// </summary>
        /// <param name="cellCoordinates">格子世界坐标</param>
        public virtual void OccupyCell(Vector3 cellCoordinates)
        {
            if (!OccupiedGridCells.Contains(cellCoordinates))
            {
                OccupiedGridCells.Add(cellCoordinates);
            }
        }

        /// <summary>
        /// 释放一个格子的占用状态
        /// </summary>
        /// <param name="cellCoordinates">格子世界坐标</param>
        public virtual void FreeCell(Vector3 cellCoordinates)
        {
            if (OccupiedGridCells.Contains(cellCoordinates))
            {
                OccupiedGridCells.Remove(cellCoordinates);
            }
        }

        #endregion

        //=========================================================================
        #region 对象位置追踪
        //=========================================================================

        /// <summary>
        /// 设置对象的下一个目标网格位置
        /// </summary>
        /// <param name="trackedObject">追踪对象</param>
        /// <param name="posIndex">网格索引</param>
        public virtual void SetNextPosition(GameObject trackedObject, Vector3Int posIndex)
        {
            if (NextPositions.ContainsKey(trackedObject))
            {
                NextPositions[trackedObject] = posIndex;
            }
            else
            {
                NextPositions.Add(trackedObject, posIndex);
            }
        }

        /// <summary>
        /// 设置对象的上一次经过的网格位置
        /// </summary>
        /// <param name="trackedObject">追踪对象</param>
        /// <param name="posIndex">网格索引</param>
        public virtual void SetLastPosition(GameObject trackedObject, Vector3Int posIndex)
        {
            if (LastPositions.ContainsKey(trackedObject))
            {
                LastPositions[trackedObject] = posIndex;
            }
            else
            {
                LastPositions.Add(trackedObject, posIndex);
            }
        }

        #endregion

        //=========================================================================
        #region 坐标转换
        //=========================================================================

        /// <summary>
        /// 世界坐标 → 网格索引坐标
        /// </summary>
        /// <param name="position">世界坐标</param>
        public virtual Vector3Int PositionToPosIndex(Vector3 position)
        {
            m_NewGridPosition = (position - GridOrigin.position) / GridUnitSize;

            m_WorkCoordinate.x = Mathf.FloorToInt(m_NewGridPosition.x);
            m_WorkCoordinate.y = Mathf.FloorToInt(m_NewGridPosition.y);
            m_WorkCoordinate.z = Mathf.FloorToInt(m_NewGridPosition.z);

            return m_WorkCoordinate;
        }

        /// <summary>
        /// 网格索引坐标 → 世界坐标（格子中心点）
        /// </summary>
        /// <param name="posIndex">网格索引</param>
        public virtual Vector3 PosIndexToPosition(Vector3Int posIndex)
        {
            m_NewGridPosition = (Vector3)posIndex * GridUnitSize + GridOrigin.position;
            m_NewGridPosition += Vector3.one * (GridUnitSize / 2f);
            return m_NewGridPosition;
        }

        #endregion

        //=========================================================================
        #region 调试绘制
        //=========================================================================

        /// <summary>
        /// 单分区像素尺寸（与 MapToolConstants.RegionPixelSize 对齐）
        /// </summary>
        private const float RegionPixelSize = 2048f;

        /// <summary>
        /// 绘制调试网格 Gizmos
        /// 自动检测 GridOrigin 是否为 RectTransform：
        /// - 是 → 铺满整个 RectTransform 区域，按 GridUnitSize 间隔画线
        /// - 否 → 退化行为，以 GridOrigin.position 为中心 ±DebugGridSize 绘制
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
            if (!DrawDebugGrid || GridOrigin == null)
            {
                return;
            }

            var rect = GridOrigin as RectTransform;
            if (rect != null)
            {
                DrawRectTransformGrid(rect);
            }
            else
            {
                DrawLegacyGrid();
            }
        }

        /// <summary>
        /// 绘制基于 RectTransform 的网格（铺满整个 ZoneRoot 区域）
        /// </summary>
        protected virtual void DrawRectTransformGrid(RectTransform rect)
        {
            // GetWorldCorners 返回顺序：[0]左下 [1]左上 [2]右上 [3]右下
            Vector3[] corners = new Vector3[4];
            rect.GetWorldCorners(corners);

            // --- 用 RectTransform 本地像素尺寸，不用世界距离（Canvas 缩放会压缩世界尺寸） ---
            float width = rect.rect.width;
            float height = rect.rect.height;
            if (width < 1f || height < 1f) return;

            float cellSize = GridUnitSize;
            int cellCountX = Mathf.CeilToInt(width / cellSize);
            int cellCountY = Mathf.CeilToInt(height / cellSize);

            // --- 细网格线（半透明青色，不遮挡背景） ---
            Gizmos.color = new Color(0, 1, 1, 0.15f);
            // X 方向竖线：从 corners[0] 到 corners[1] 逐列插值
            for (int x = 0; x <= cellCountX; x++)
            {
                float t = x / (float)cellCountX;
                m_DebugOrigin = Vector3.Lerp(corners[0], corners[3], t); // bottom-left → bottom-right
                m_DebugDestination = Vector3.Lerp(corners[1], corners[2], t); // top-left → top-right
                Gizmos.DrawLine(m_DebugOrigin, m_DebugDestination);
            }
            // Y 方向横线
            for (int y = 0; y <= cellCountY; y++)
            {
                float t = y / (float)cellCountY;
                m_DebugOrigin = Vector3.Lerp(corners[0], corners[1], t); // bottom-left → top-left
                m_DebugDestination = Vector3.Lerp(corners[3], corners[2], t); // bottom-right → top-right
                Gizmos.DrawLine(m_DebugOrigin, m_DebugDestination);
            }

            // --- 分区边界线（每 RegionPixelSize 像素一条） ---
            Gizmos.color = Color.yellow;
            int regionCountX = Mathf.CeilToInt(width / RegionPixelSize);
            int regionCountY = Mathf.CeilToInt(height / RegionPixelSize);
            for (int x = 0; x <= regionCountX; x++)
            {
                float t = x * RegionPixelSize / width;
                m_DebugOrigin = Vector3.Lerp(corners[0], corners[3], t);
                m_DebugDestination = Vector3.Lerp(corners[1], corners[2], t);
                Gizmos.DrawLine(m_DebugOrigin, m_DebugDestination);
            }
            for (int y = 0; y <= regionCountY; y++)
            {
                float t = y * RegionPixelSize / height;
                m_DebugOrigin = Vector3.Lerp(corners[0], corners[1], t);
                m_DebugDestination = Vector3.Lerp(corners[3], corners[2], t);
                Gizmos.DrawLine(m_DebugOrigin, m_DebugDestination);
            }

            // --- 原点标记（左下角） ---
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(corners[0], cellSize * 2f);
        }

        /// <summary>
        /// 旧版网格绘制（非 RectTransform 时退化使用）
        /// </summary>
        protected virtual void DrawLegacyGrid()
        {
            Gizmos.color = CellBorderColor;

            if (DebugDrawMode == GameDefinitions.DimensionMode.Three)
            {
                Draw3DDebugGrid();
            }
            else
            {
                Draw2DDebugGrid();
            }
        }

        /// <summary>
        /// 绘制 2D 调试网格
        /// </summary>
        protected virtual void Draw2DDebugGrid()
        {
            int i = -DebugGridSize;
            while (i <= DebugGridSize)
            {
                m_DebugOrigin.x = GridOrigin.position.x - DebugGridSize * GridUnitSize;
                m_DebugOrigin.y = GridOrigin.position.y + i * GridUnitSize;
                m_DebugOrigin.z = GridOrigin.position.z;

                m_DebugDestination.x = GridOrigin.position.x + DebugGridSize * GridUnitSize;
                m_DebugDestination.y = GridOrigin.position.y + i * GridUnitSize;
                m_DebugDestination.z = GridOrigin.position.z;

                Debug.DrawLine(m_DebugOrigin, m_DebugDestination, CellBorderColor);

                m_DebugOrigin.x = GridOrigin.position.x + i * GridUnitSize;
                m_DebugOrigin.y = GridOrigin.position.y - DebugGridSize * GridUnitSize;
                m_DebugOrigin.z = GridOrigin.position.z;

                m_DebugDestination.x = GridOrigin.position.x + i * GridUnitSize;
                m_DebugDestination.y = GridOrigin.position.y + DebugGridSize * GridUnitSize;
                m_DebugDestination.z = GridOrigin.position.z;

                Debug.DrawLine(m_DebugOrigin, m_DebugDestination, CellBorderColor);

                i++;
            }

            Gizmos.color = InnerColor;
            for (int col = -DebugGridSize; col < DebugGridSize; col++)
            {
                for (int row = -DebugGridSize; row < DebugGridSize; row++)
                {
                    if ((col % 2 == 0 && row % 2 != 0) || (col % 2 != 0 && row % 2 == 0))
                    {
                        DrawCell2D(col, row);
                    }
                }
            }
        }

        /// <summary>
        /// 绘制 3D 调试网格
        /// </summary>
        protected virtual void Draw3DDebugGrid()
        {
            int i = -DebugGridSize;
            while (i <= DebugGridSize)
            {
                m_DebugOrigin.x = GridOrigin.position.x - DebugGridSize * GridUnitSize;
                m_DebugOrigin.y = GridOrigin.position.y;
                m_DebugOrigin.z = GridOrigin.position.z + i * GridUnitSize;

                m_DebugDestination.x = GridOrigin.position.x + DebugGridSize * GridUnitSize;
                m_DebugDestination.y = GridOrigin.position.y;
                m_DebugDestination.z = GridOrigin.position.z + i * GridUnitSize;

                Debug.DrawLine(m_DebugOrigin, m_DebugDestination, CellBorderColor);

                m_DebugOrigin.x = GridOrigin.position.x + i * GridUnitSize;
                m_DebugOrigin.y = GridOrigin.position.y;
                m_DebugOrigin.z = GridOrigin.position.z - DebugGridSize * GridUnitSize;

                m_DebugDestination.x = GridOrigin.position.x + i * GridUnitSize;
                m_DebugDestination.y = GridOrigin.position.y;
                m_DebugDestination.z = GridOrigin.position.z + DebugGridSize * GridUnitSize;

                Debug.DrawLine(m_DebugOrigin, m_DebugDestination, CellBorderColor);

                i++;
            }

            Gizmos.color = InnerColor;
            for (int col = -DebugGridSize; col < DebugGridSize; col++)
            {
                for (int row = -DebugGridSize; row < DebugGridSize; row++)
                {
                    if ((col % 2 == 0 && row % 2 != 0) || (col % 2 != 0 && row % 2 == 0))
                    {
                        DrawCell3D(col, row);
                    }
                }
            }
        }

        /// <summary>
        /// 绘制单个 2D 格子
        /// </summary>
        protected virtual void DrawCell2D(int col, int row)
        {
            m_DebugOrigin.x = GridOrigin.position.x + col * GridUnitSize + GridUnitSize / 2f;
            m_DebugOrigin.y = GridOrigin.position.y + row * GridUnitSize + GridUnitSize / 2f;
            m_DebugOrigin.z = GridOrigin.position.z;
            Gizmos.DrawCube(m_DebugOrigin, GridUnitSize * new Vector3(1f, 1f, 0f));
        }

        /// <summary>
        /// 绘制单个 3D 格子
        /// </summary>
        protected virtual void DrawCell3D(int col, int row)
        {
            m_DebugOrigin.x = GridOrigin.position.x + col * GridUnitSize + GridUnitSize / 2f;
            m_DebugOrigin.y = GridOrigin.position.y;
            m_DebugOrigin.z = GridOrigin.position.z + row * GridUnitSize + GridUnitSize / 2f;
            Gizmos.DrawCube(m_DebugOrigin, GridUnitSize * new Vector3(1f, 0f, 1f));
        }

        #endregion

        //=========================================================================
        #region Lua 入口（MapLoadManager.lua 调用）
        //=========================================================================

        /// <summary>
        /// 分区列数
        /// </summary>
        [HideInInspector] public int RegionCols = 1;
        /// <summary>
        /// 分区行数
        /// </summary>
        [HideInInspector] public int RegionRows = 1;
        /// <summary>
        /// 已解锁分区数
        /// </summary>
        [HideInInspector] public int UnlockedCount = 1;
        /// <summary>
        /// 地图总列数（格数）
        /// </summary>
        [HideInInspector] public int GridCols = 128;
        /// <summary>
        /// 地图总行数（格数）
        /// </summary>
        [HideInInspector] public int GridRows = 128;
        /// <summary>
        /// 占用格原始数组（X,Y 交替）
        /// </summary>
        private int[] _rawOccupied;

        /// <summary>
        /// Lua 调用：初始化网格参数
        /// </summary>
        public void Setup(int gridSize, int gridCols, int gridRows,
            int regionCols, int regionRows, float scale, int unlockedCount, Transform zoneRoot)
        {
            GridUnitSize = gridSize;
            GridCols = gridCols;
            GridRows = gridRows;
            RegionCols = regionCols;
            RegionRows = regionRows;
            UnlockedCount = unlockedCount;
            GridOrigin = zoneRoot;

            Debug.Log($"[GridManager] Setup: grid={gridSize} cols={gridCols} rows={gridRows} region={regionCols}x{regionRows} unlocked={unlockedCount}");
        }

        /// <summary>
        /// Lua 调用：设置占用格数组（X,Y 交替排列）
        /// </summary>
        public void SetOccupiedCells(int[] occupied)
        {
            _rawOccupied = occupied;
            if (occupied != null)
            {
                OccupiedGridCells.Clear();
                for (int i = 0; i + 1 < occupied.Length; i += 2)
                {
                    OccupiedGridCells.Add(new Vector3(occupied[i], occupied[i + 1], 0));
                }
            }
            Debug.Log($"[GridManager] SetOccupiedCells: {OccupiedGridCells.Count} 格");
        }

        /// <summary>
        /// Lua 调用：构建网格 Overlay 可视化
        /// </summary>
        public void BuildOverlay()
        {
            var comp = GetComponent("GameLib.MapGridOverlayComponent");
            if (comp == null)
            {
                var gridOverlay = transform.Find("GridOverlay");
                if (gridOverlay != null)
                    comp = gridOverlay.GetComponent("GameLib.MapGridOverlayComponent");
            }

            if (comp != null)
            {
                var m = comp.GetType().GetMethod("BuildOverlay");
                m?.Invoke(comp, new object[] {
                    (int)GridUnitSize, GridCols, GridRows, 1f,
                    _rawOccupied, RegionCols, RegionRows, true, true, UnlockedCount
                });
                Debug.Log($"[GridManager] BuildOverlay cols={GridCols} rows={GridRows} unlocked={UnlockedCount}");
            }
            else
            {
                Debug.LogWarning("[GridManager] 未找到 MapGridOverlayComponent");
            }
        }

        #endregion
    }
}