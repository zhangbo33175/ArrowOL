/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GridMapManager.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   网格地图管理器 - Tilemap 数据序列化保存方法
 ***************************************************************/

using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameLib
{
    public partial class GridMapManager
    {
        #region 地图数据保存

        /// <summary>
        /// 将 Grid 下所有 Tilemap 的瓦片信息序列化为 JSON 并写入文件
        /// </summary>
        /// <param name="grid">挂载 Tilemap 的 Grid 组件</param>
        /// <param name="path">保存文件的完整路径</param>
        public void SaveTilemap(Grid grid, string path)
        {
            MapTilemapData data = new MapTilemapData();

            // 获取 Grid 下所有的 Tilemap
            Tilemap[] tilemaps = grid.GetComponentsInChildren<Tilemap>();

            // 遍历所有的图层信息
            foreach (var tilemap in tilemaps)
            {
                LayerData layerData = new LayerData();
                layerData.layerName = tilemap.name;

                BoundsInt bounds = tilemap.cellBounds;
                layerData.origin = bounds.position;
                layerData.width = bounds.size.x;
                layerData.height = bounds.size.y;

                for (int x = 0; x < bounds.size.x; x++)
                {
                    for (int y = 0; y < bounds.size.y; y++)
                    {
                        Vector3Int pos = new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0);
                        TileBase tile = tilemap.GetTile(pos);
                        if (tile != null)
                        {
                            layerData.tiles.Add(new MapTileData
                            {
                                gridPosition = new Vector2Int(x, y),
                                tilePath = tile.name
                            });
                        }
                    }
                }

                data.layers.Add(layerData);
            }

            string json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(path, json);
            Debug.Log("地图已保存到: " + path);
        }

        #endregion
    }
}
