/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GridManager.cs
 * author:    云毅
 * created:
 * descrip:   网格管理系统 - 坐标转换、格子占用、路径追踪、调试绘制
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 网格管理系统（单例）
    /// 提供：世界坐标 ↔ 网格坐标转换、格子占用管理、对象路径追踪、调试网格绘制
    /// 支持 2D / 3D 双模式
    /// </summary>
    [AddComponentMenu("Honor Core/Manager/GridManager")]
    public class GridManager : MonoSingleton<GridManager>
    {
        //=========================================================================
        #region 序列化字段（Inspector 配置）
        //=========================================================================

        [GameHeader("网格")]
        [GameTitle("参考根节点")]
        [Tooltip("网格在世界空间下的原始点。")]
        public Transform GridOrigin;

        [GameTitle("每个格子的大小")]
        [Tooltip("网格区域中每个格子的尺寸大小。")]
        public float GridUnitSize = 1f;

        [GameHeader("调试")]
        [GameTitle("绘制调试网格")]
        [Tooltip("作为是否绘制调试网格的总开关。")]
        public bool DrawDebugGrid = true;

        [GameTitle("2D/3D模式")]
        [GameCondition("DrawDebugGrid", true)]
        [Tooltip("以2D/3D的哪一种空间维度来绘制网格信息。")]
        public GameDefinitions.DimensionMode DebugDrawMode = GameDefinitions.DimensionMode.Two;

        [GameTitle("网格数量")]
        [GameCondition("DrawDebugGrid", true)]
        [Tooltip("宽 = 网格数量 X 2，高 = 网格数量 X 2。")]
        public int DebugGridSize = 30;

        [GameTitle("网格线颜色")]
        [GameCondition("DrawDebugGrid", true)]
        [Tooltip("网格绘制用到的网格线颜色。")]
        public Color CellBorderColor = new Color(60f, 221f, 255f, 1f);

        [GameTitle("网格填充颜色")]
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
        /// 绘制调试网格 Gizmos
        /// 支持 2D / 3D 模式
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
            if (!DrawDebugGrid || GridOrigin == null)
            {
                return;
            }

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
    }
}