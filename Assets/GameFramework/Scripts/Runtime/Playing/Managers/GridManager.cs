using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    [AddComponentMenu("Honor Core/Manager/GridManager")]
    public class GridManager : MonoSingleton<GridManager>
    {
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

        /// <summary>
        /// 所有已经被占用的单位格子列表
        /// </summary>
        [HideInInspector]
        public List<Vector3> OccupiedGridCells;

        /// <summary>
        /// 所有注册对象在网格中行进的前一次位置信息
        /// </summary>
        [HideInInspector]
        public Dictionary<GameObject, Vector3Int> LastPositions;

        /// <summary>
        /// 所有注册对象在网格中即将进行的下一次目标位置信息
        /// </summary>
        [HideInInspector]
        public Dictionary<GameObject, Vector3Int> NextPositions;

        /// <summary>
        /// 以下向量均为避免反复开销的临时变量
        /// </summary>
        protected Vector3 m_NewGridPosition;
        protected Vector3 m_DebugOrigin = Vector3.zero;
        protected Vector3 m_DebugDestination = Vector3.zero;
        protected Vector3Int m_WorkCoordinate = Vector3Int.zero;

        protected virtual void Start()
        {
            OccupiedGridCells = new List<Vector3>();
            LastPositions = new Dictionary<GameObject, Vector3Int>();
            NextPositions = new Dictionary<GameObject, Vector3Int>();
        }

        /// <summary>
        /// 指定位置的单位格子是否被占用
        /// </summary>
        /// <param name="cellCoordinates">单位格子坐标</param>
        /// <returns></returns>
        public virtual bool CellIsOccupied(Vector3 cellCoordinates)
        {
            return OccupiedGridCells.Contains(cellCoordinates);
        }

        /// <summary>
        /// 标记指定位置的单位格子为占用状态
        /// </summary>
        /// <param name="cellCoordinates">单位格子坐标</param>
        public virtual void OccupyCell(Vector3 cellCoordinates)
        {
            if (!OccupiedGridCells.Contains(cellCoordinates))
            {
                OccupiedGridCells.Add(cellCoordinates);
            }
        }

        /// <summary>
        /// 移除指定位置单位格子的占用状态
        /// </summary>
        /// <param name="cellCoordinates">单位格子坐标</param>
        public virtual void FreeCell(Vector3 cellCoordinates)
        {
            if (OccupiedGridCells.Contains(cellCoordinates))
            {
                OccupiedGridCells.Remove(cellCoordinates);
            }
        }

        /// <summary>
        /// 设置指定对象下一次的目标位置索引
        /// 下一次的目标位置为当前对象到达当前目标位置后的下一个目标位置
        /// </summary>
        /// <param name="trackedObject">指定对象</param>
        /// <param name="posIndex">目标位置索引</param>
        public virtual void SetNextPosition(GameObject trackedObject, Vector3Int posIndex)
        {
            // we add that to our dictionary
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
        /// 设置指定对象前一次的目标位置索引
        /// 前一次的目标位置为当前对象前一次已经经过的目标位置
        /// </summary>
        /// <param name="trackedObject">指定对象</param>
        /// <param name="posIndex">目标位置索引</param>
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

        /// <summary>
        /// 世界坐标转换为索引坐标
        /// </summary>
        /// <param name="position">世界坐标</param>
        /// <returns></returns>
        public virtual Vector3Int PositionToPosIndex(Vector3 position)
        {
            m_NewGridPosition = (position - GridOrigin.position) / GridUnitSize;

            m_WorkCoordinate.x = Mathf.FloorToInt(m_NewGridPosition.x);
            m_WorkCoordinate.y = Mathf.FloorToInt(m_NewGridPosition.y);
            m_WorkCoordinate.z = Mathf.FloorToInt(m_NewGridPosition.z);

            return m_WorkCoordinate;
        }

        /// <summary>
        /// 索引坐标转换为世界坐标
        /// </summary>
        /// <param name="posIndex">位置索引</param>
        /// <returns></returns>
        public virtual Vector3 PosIndexToPosition(Vector3Int posIndex)
        {
            m_NewGridPosition = (Vector3)posIndex * GridUnitSize + GridOrigin.position;
            m_NewGridPosition += Vector3.one * (GridUnitSize / 2f);
            return m_NewGridPosition;
        }

        /// <summary>
        /// 绘制Gizmos信息-网格绘制
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
            if (!DrawDebugGrid)
            {
                return;
            }

            Gizmos.color = CellBorderColor;

            if (DebugDrawMode == GameDefinitions.DimensionMode.Three)
            {
                int i = -DebugGridSize;

                // 网格线绘制
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

                // 网格绘制（交替颜色区分）
                Gizmos.color = InnerColor;
                for (int col = -DebugGridSize; col < DebugGridSize; col++)
                {
                    for (int row = -DebugGridSize; row < DebugGridSize; row++)
                    {
                        if ((col % 2 == 0) && (row % 2 != 0))
                        {
                            DrawCell3D(col, row);
                        }
                        if ((col % 2 != 0) && (row % 2 == 0))
                        {
                            DrawCell3D(col, row);
                        }
                    }
                }
            }
            else
            {
                int i = -DebugGridSize;
                // 网格线绘制
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
                    m_DebugOrigin.y = GridOrigin.position.y - DebugGridSize * GridUnitSize; ;
                    m_DebugOrigin.z = GridOrigin.position.z;

                    m_DebugDestination.x = GridOrigin.position.x + i * GridUnitSize;
                    m_DebugDestination.y = GridOrigin.position.y + DebugGridSize * GridUnitSize;
                    m_DebugDestination.z = GridOrigin.position.z;

                    Debug.DrawLine(m_DebugOrigin, m_DebugDestination, CellBorderColor);

                    i++;
                }

                // 网格绘制（交替颜色区分）
                Gizmos.color = InnerColor;
                for (int col = -DebugGridSize; col < DebugGridSize; col++)
                {
                    for (int row = -DebugGridSize; row < DebugGridSize; row++)
                    {
                        if ((col % 2 == 0) && (row % 2 != 0))
                        {
                            DrawCell2D(col, row);
                        }
                        if ((col % 2 != 0) && (row % 2 == 0))
                        {
                            DrawCell2D(col, row);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绘制一个2D的单元网格
        /// </summary>
        /// <param name="col">列</param>
        /// <param name="row">行</param>
        protected virtual void DrawCell2D(int col, int row)
        {
            m_DebugOrigin.x = GridOrigin.position.x + col * GridUnitSize + GridUnitSize / 2f;
            m_DebugOrigin.y = GridOrigin.position.y + row * GridUnitSize + GridUnitSize / 2f;
            m_DebugOrigin.z = GridOrigin.position.z;
            Gizmos.DrawCube(m_DebugOrigin, GridUnitSize * new Vector3(1f, 1f, 0f));
        }

        /// <summary>
        /// 绘制一个3D的单元网格
        /// </summary>
        /// <param name="col">列</param>
        /// <param name="row">行</param>
        protected virtual void DrawCell3D(int col, int row)
        {
            m_DebugOrigin.x = GridOrigin.position.x + col * GridUnitSize + GridUnitSize / 2f;
            m_DebugOrigin.y = GridOrigin.position.y;
            m_DebugOrigin.z = GridOrigin.position.z + row * GridUnitSize + GridUnitSize / 2f;
            Gizmos.DrawCube(m_DebugOrigin, GridUnitSize * new Vector3(1f, 0f, 1f));
        }
    }
}
