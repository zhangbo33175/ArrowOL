using UnityEngine;

namespace Honor.Runtime
{
    public class WorldPathUtils
    {
        #region 45°等距坐标转换工具（网格↔世界坐标）
        /// <summary>
        /// 网格尺寸（根据你的素材调整，默认64x32适配45°素材）
        /// </summary>
        public static Vector2 tileSize = new Vector2(64, 32);

        /// <summary>
        /// 网格坐标转世界坐标（核心公式）
        /// </summary>
        public static Vector3 GridToWorld(Vector2Int gridPos)
        {
            float x = (gridPos.x - gridPos.y) * tileSize.x / 2;
            float y = (gridPos.x + gridPos.y) * tileSize.y / 2;
            return new Vector3(x, y, 0);
        }

        /// <summary>
        /// 世界坐标转网格坐标
        /// </summary>
        public static Vector2Int WorldToGrid(Vector3 worldPos)
        {
            float x = (worldPos.x / (tileSize.x / 2)) + (worldPos.y / (tileSize.y / 2));
            float y = (worldPos.y / (tileSize.y / 2)) - (worldPos.x / (tileSize.x / 2));
            return new Vector2Int(Mathf.RoundToInt(x / 2), Mathf.RoundToInt(y / 2));
        }  
        #endregion
        
    }
}