/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GridMapManager.GridType.cs
 * author:    云毅
 * created:   2026
 * descrip:   网格地图管理器 - 地块类型与层级枚举定义
 ***************************************************************/

namespace GameLib
{
    public partial class GridMapManager
    {
        #region 枚举定义

        /// <summary>
        /// 地块状态类型
        /// </summary>
        public enum GridType
        {
            /// <summary>空（未占用）</summary>
            Empty = 0,

            /// <summary>障碍物</summary>
            Obstacle = 1,

            /// <summary>NPC</summary>
            NPC = 2,

            /// <summary>道具</summary>
            Props = 3,
        }

        /// <summary>
        /// 地图图层层级
        /// </summary>
        public enum LayerLevel
        {
            /// <summary>地面</summary>
            ground = 0,

            /// <summary>建筑</summary>
            building = 1,

            /// <summary>装饰</summary>
            decoration = 2,
        }

        #endregion
    }
}
