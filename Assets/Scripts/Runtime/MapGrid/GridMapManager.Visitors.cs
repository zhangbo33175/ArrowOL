/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GridMapManager.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   网格地图管理器 - 公开字段与网格数据集合定义
 ***************************************************************/

using UnityEngine;

namespace GameLib
{
    public partial class GridMapManager
    {
        #region 公开字段

        /// <summary>
        /// 地图的大小
        /// </summary>
        public int m_GridSize = 1;

        /// <summary>
        /// 地图的宽度，网格列数
        /// </summary>
        public int m_GridWidth = 10;

        /// <summary>
        /// 地图的高度，网格行数
        /// </summary>
        public int m_GridHeight = 10;

        /// <summary>
        /// 网格单元宽度
        /// </summary>
        public float m_CellWidth = 1f;

        /// <summary>
        /// 网格单元高度
        /// </summary>
        public float m_CellHeight = 0.5f;

        /// <summary>
        /// 普通的颜色
        /// </summary>
        public Color m_NormalColor = new Color(1, 1, 1, 0.1f);

        /// <summary>
        /// 可行走路径颜色
        /// </summary>
        public Color m_WalkableColor = new Color(1, 1, 1, 0.1f);

        /// <summary>
        /// 不可行走路径颜色
        /// </summary>
        public Color m_BlockedColor = new Color(1, 1, 1, 0.1f);

        /// <summary>
        /// 是否可以行走集合
        /// </summary>
        public bool[,] m_GridData;

        /// <summary>
        /// 地图图块的集合
        /// </summary>
        public GridTile[,] m_GridTile;

        /// <summary>
        /// 是否显示网格
        /// </summary>
        public bool m_ShowGrid = false;

        /// <summary>
        /// 挂载 Tilemap 的 Grid 组件
        /// </summary>
        public Grid m_Grid;

        #endregion
    }
}
