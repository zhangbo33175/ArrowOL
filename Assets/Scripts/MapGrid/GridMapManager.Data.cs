using System.Collections.Generic;
using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 存储地图数据需要用到的信息
    /// </summary>
    public partial class GridMapManager
    {
        
        /// <summary>
        /// 单个地块的信息
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
            /// 
            /// </summary>
            public List<MapTileData> tiles = new List<MapTileData>();
        }
    }
}