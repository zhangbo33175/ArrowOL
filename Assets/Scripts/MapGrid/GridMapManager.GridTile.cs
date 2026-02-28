using UnityEngine;

namespace GameLib
{
    public partial class GridMapManager
    {
        /// <summary>
        /// 单个地块信息
        /// </summary>
        public class GridTile
        {
            /// <summary>
            /// 
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
            /// 
            /// </summary>
            public GridType ObjectType { get; set; } // 0: 空, 1: 障碍物, 2: NPC, 3: 道具等

            public GridTile(int x, int y)
            {
                GridPosition = new Vector2Int(x, y);
                IsOccupied = false;
                Occupant = null;
                ObjectType = 0;
            }
        }
    }
}