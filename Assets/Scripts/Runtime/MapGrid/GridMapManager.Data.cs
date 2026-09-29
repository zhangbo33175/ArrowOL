/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GridMapManager.Data.cs
 * author:    云毅
 * created:   2026
 * descrip:   网格地图管理器 - 存档数据结构（地块/图层/地图序列化数据）
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 网格地图管理器（数据分部）
    /// 定义地图存档/读取所需的序列化数据结构
    /// </summary>
    public partial class GridMapManager
    {
        #region 地块数据

        /// <summary>
        /// 存储地图数据需要用到的信息
        /// </summary>
        [System.Serializable]
        public class MapTileData
        {
            /// <summary>
            /// 地块的ID
            /// </summary>
            public int tileId;

            /// <summary>
            /// 地块的位置信息
            /// </summary>
            public Vector2Int gridPosition;

            /// <summary>
            /// Tile 的资源路径
            /// </summary>
            public string tilePath;

            /// <summary>
            /// 地块上的物体
            /// </summary>
            public GameObject prefab;

            /// <summary>
            /// 层级信息
            /// </summary>
            public LayerLevel LayerLevel;
        }

        #endregion

        #region 地图与图层数据

        /// <summary>
        /// 地图的信息
        /// </summary>
        [System.Serializable]
        public class MapTilemapData
        {
            /// <summary>
            /// 地图的层级信息
            /// </summary>
            public List<LayerData> layers = new List<LayerData>();
        }

        /// <summary>
        /// 地图的层级信息
        /// </summary>
        [System.Serializable]
        public class LayerData
        {
            /// <summary>
            /// 层级名称
            /// </summary>
            public string layerName;

            /// <summary>
            /// 层级坐标
            /// </summary>
            public Vector3Int origin;

            /// <summary>
            /// 层级的宽
            /// </summary>
            public int width;

            /// <summary>
            /// 层级的高
            /// </summary>
            public int height;

            /// <summary>
            /// 该层级下所有地块数据
            /// </summary>
            public List<MapTileData> tiles = new List<MapTileData>();
        }

        #endregion
    }
}
