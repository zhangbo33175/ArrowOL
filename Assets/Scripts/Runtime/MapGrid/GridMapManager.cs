/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GridMapManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   网格地图管理器 - 主类入口（网格生成、物体放置/移除、Gizmos 绘制）
 ***************************************************************/

using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 网格地图管理器（GridMapManager）
    /// 负责二维网格的生成、物体放置/移除与格子占用检测
    /// </summary>
    public partial class GridMapManager : MonoBehaviour
    {
        #region 序列化配置字段

        [Header("网格设置")]
        /// <summary>
        /// 网格列数（X 方向）
        /// </summary>
        public int gridWidth = 10;

        /// <summary>
        /// 网格行数（Y 方向）
        /// </summary>
        public int gridHeight = 10;

        //[Header("层级设置")]
        //public Transform mapLayer;
        //public Transform objectLayer;
        //public Transform characterLayer;

        #endregion

        #region Unity 生命周期

        /// <summary>
        /// 唤醒时生成网格
        /// </summary>
        private void Awake()
        {
            GenerateGrid();
        }

        #endregion

        #region 网格生成

        /// <summary>
        /// 按 gridWidth × gridHeight 生成网格地块数组
        /// </summary>
        private void GenerateGrid()
        {
            m_GridTile = new GridTile[gridWidth, gridHeight];
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    m_GridTile[x, y] = new GridTile(x, y);
                }
            }
        }

        #endregion

        #region 物体放置与移除

        /// <summary>
        /// 在指定网格放置物体
        /// </summary>
        /// <param name="obj">要放置的物体</param>
        /// <param name="gridPos">目标网格坐标</param>
        /// <param name="objectType">物体类型</param>
        /// <param name="parent">父节点（预留，当前实现未启用）</param>
        /// <returns>true=放置成功 / false=格子被占用或坐标越界</returns>
        public bool PlaceObject(GameObject obj, Vector2Int gridPos, GridType objectType, Transform parent)
        {
            // 修复：第三个实参原为 gridWidth，与数组第二维 gridHeight 不一致，改为 gridHeight 以匹配数组维度
            if (GridUtils.IsCellOccupied(gridPos, gridWidth, gridHeight, m_GridTile))
                return false;

            m_GridTile[gridPos.x, gridPos.y].IsOccupied = true;
            m_GridTile[gridPos.x, gridPos.y].Occupant = obj;
            m_GridTile[gridPos.x, gridPos.y].ObjectType = objectType;
            // obj.transform.position = GridUtils.GridToWorld(gridPos);
            // obj.transform.SetParent(parent);
            return true;
        }

        /// <summary>
        /// 移除指定网格上的物体
        /// </summary>
        /// <param name="gridPos">目标网格坐标</param>
        public void RemoveObject(Vector2Int gridPos)
        {
            if (gridPos.x < 0 || gridPos.x >= gridWidth || gridPos.y < 0 || gridPos.y >= gridHeight)
                return;

            Destroy(m_GridTile[gridPos.x, gridPos.y].Occupant);
            m_GridTile[gridPos.x, gridPos.y].IsOccupied = false;
            m_GridTile[gridPos.x, gridPos.y].Occupant = null;
            m_GridTile[gridPos.x, gridPos.y].ObjectType = 0;
        }

        #endregion

        #region Gizmos 绘制

        /// <summary>
        /// Scene 视图绘制网格 Gizmos（调试用，当前绘制语句已注释）
        /// </summary>
        private void OnDrawGizmos()
        {
            if (m_GridTile == null)
                GenerateGrid();

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    // Vector3 worldPos = GridUtils.GridToWorld(gridPos);
                    // Gizmos.color = m_GridTile[x, y].IsOccupied ? Color.red : Color.green;
                    // Gizmos.DrawWireCube(worldPos, new Vector3(cellWidth, cellHeight, 0));
                }
            }
        }

        #endregion
    }
}
