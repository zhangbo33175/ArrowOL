/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GridMapManager.GridTile.cs
 * author:    云毅
 * created:   2026
 * descrip:   网格地图管理器 - 单个地块（GridTile）数据定义
 ***************************************************************/

using UnityEngine;

namespace GameLib
{
    public partial class GridMapManager
    {
        #region 地块类型

        /// <summary>
        /// 单个地块信息
        /// </summary>
        public class GridTile
        {
            /// <summary>
            /// 地块网格坐标（只读）
            /// </summary>
            public Vector2Int GridPosition { get; private set; }

            /// <summary>
            /// 是否被占用
            /// </summary>
            public bool IsOccupied { get; set; }

            /// <summary>
            /// 占用的物体
            /// </summary>
            public GameObject Occupant { get; set; }

            /// <summary>
            /// 地块物体类型（0: 空, 1: 障碍物, 2: NPC, 3: 道具等）
            /// </summary>
            public GridType ObjectType { get; set; }

            /// <summary>
            /// 构造单个地块
            /// </summary>
            /// <param name="x">网格 X 坐标</param>
            /// <param name="y">网格 Y 坐标</param>
            public GridTile(int x, int y)
            {
                GridPosition = new Vector2Int(x, y);
                IsOccupied = false;
                Occupant = null;
                ObjectType = 0;
            }
        }

        #endregion
    }
}
