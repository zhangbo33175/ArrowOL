using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameLib
{
    public partial class GridMapManager
    {
        /// <summary>
        /// 存储地图信息
        /// </summary>
        /// <param name="grid">地图信息</param>
        /// <param name="path"></param>
        public void SaveTilemap(Grid grid, string path)
        {
            MapTilemapData data = new MapTilemapData();
            // 获取 Grid 下所有的 Tilemap
            Tilemap[] tilemaps = grid.GetComponentsInChildren<Tilemap>();
            //遍历所有的图层信息
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
                                gridPosition =new Vector2Int(x,y),
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
    }
}