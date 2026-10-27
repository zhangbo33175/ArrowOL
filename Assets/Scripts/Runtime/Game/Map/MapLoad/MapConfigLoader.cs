/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapConfigLoader.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图配置运行时加载器
 *            读取编辑器保存的 Map_X.json，还原分区背景/摆放物/路径/网格
 ***************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XLua;

namespace GameLib
{
    /// <summary>
    /// 地图配置运行时加载器（Lua 驱动薄壳）
    /// 读取编辑器保存的 JSON 配置，实例化分区、建筑、物件、角色
    /// </summary>
    [LuaCallCSharp]
    public static class MapConfigLoader
    {
        #region 序列化数据结构

        /// <summary>地图配置根结构</summary>
        [System.Serializable]
        private class MapConfigJson
        {
            public string sceneId;
            public string sceneName;
            public int width;
            public int height;
            public string chapterId;
            public string chapterName;
            public int regionCount;
            public int regionRows;
            public int regionCols;
            public int gridSize;
            public int gridCols;
            public int gridRows;
            public List<RegionJson> regions = new List<RegionJson>();
            public List<GridCellJson> occupiedCells = new List<GridCellJson>();
        }

        /// <summary>分区配置</summary>
        [System.Serializable]
        private class RegionJson
        {
            public string regionId;
            public string bgName;
            public bool unlocked;
            public int col;
            public int row;
            public int offsetX;
            public int offsetY;
            public int width;
            public int height;
            public bool fogEnabled;
            public int gridCols;
            public int gridRows;
            public List<MapObjectJson> buildings = new List<MapObjectJson>();
            public List<MapObjectJson> objects = new List<MapObjectJson>();
            public List<MapCharacterJson> characters = new List<MapCharacterJson>();
        }

        /// <summary>摆放物配置（建筑/物件通用）</summary>
        [System.Serializable]
        private class MapObjectJson
        {
            public string id;
            public string templateId;
            public string name;
            public float posX;
            public float posY;
            public int gridX;
            public int gridY;
            public int width;
            public int height;
            public string image;
            public string anim;
            public bool animEnabled;
            public bool loop;
            public float speed;
            public bool interactable;
            public string interactType;
            public int sortOrder;
            public bool collisionEnabled;
            public string dialogBubble;
            public bool shadowEnabled;
            public bool isCollectible;
            public float respawnTime;
            public int bubbleId;
        }

        /// <summary>角色配置（含路径点）</summary>
        [System.Serializable]
        private class MapCharacterJson
        {
            public string id;
            public string templateId;
            public string name;
            public float posX;
            public float posY;
            public int gridX;
            public int gridY;
            public int width;
            public int height;
            public string image;
            public string anim;
            public bool animEnabled;
            public bool loop;
            public float speed;
            public bool interactable;
            public string interactType;
            public bool patrolLoop;
            public string facingDirection;
            public string dialogBubble;
            public int sortOrder;
            public bool shadowEnabled;
            public List<PathPointJson> pathPoints = new List<PathPointJson>();
        }

        /// <summary>路径点配置</summary>
        [System.Serializable]
        private class PathPointJson
        {
            public float x;
            public float y;
            public int gridX;
            public int gridY;
        }

        /// <summary>网格占用格</summary>
        [System.Serializable]
        private class GridCellJson
        {
            public int x;
            public int y;
        }

        #endregion

        #region 公开接口

        /// <summary>
        /// 加载地图配置 JSON 并还原到场景
        /// </summary>
        /// <param name="mapName">地图名称（如 Map_1）</param>
        /// <param name="zoneRoot">分区根节点 Transform</param>
        /// <param name="zonePrefab">ZoneItem 预制体</param>
        /// <returns>加载结果信息</returns>
        public static string LoadMapConfig(string mapName, Transform zoneRoot, GameObject zonePrefab)
        {
            if (zoneRoot == null || zonePrefab == null)
                return "[MapConfigLoader] zoneRoot 或 zonePrefab 为空";

            // 1. 读取 JSON
            string jsonPath = $"Assets/GameData/MapDatas/{mapName}.json";
            string fullPath = Path.Combine(Application.dataPath, "GameData/MapDatas", $"{mapName}.json");
            if (!File.Exists(fullPath))
                return $"[MapConfigLoader] 配置文件不存在: {jsonPath}";

            MapConfigJson config;
            try
            {
                config = JsonUtility.FromJson<MapConfigJson>(File.ReadAllText(fullPath, new UTF8Encoding(false)));
            }
            catch (System.Exception e)
            {
                return $"[MapConfigLoader] JSON 解析失败: {e.Message}";
            }
            if (config == null || config.regions == null || config.regions.Count == 0)
                return "[MapConfigLoader] 配置数据为空";

            // 2. 清除旧分区
            for (int i = zoneRoot.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(zoneRoot.GetChild(i).gameObject);
            }

            // 3. 实例化分区
            int charCount = 0, objCount = 0, bldCount = 0;
            foreach (var region in config.regions)
            {
                if (!region.unlocked) continue;

                var zoneGo = Object.Instantiate(zonePrefab, zoneRoot, false);
                zoneGo.name = $"ZoneItem_{region.col}_{region.row}";
                var zoneRt = zoneGo.transform as RectTransform;

                // 定位分区（左下角原点坐标系）
                float worldX = region.col * region.width;
                float worldY = region.row * region.height;
                zoneRt.anchoredPosition = new Vector2(worldX, worldY);

                // 设置背景图
                var bg = zoneGo.transform.Find("RawImage");
                if (bg != null && !string.IsNullOrEmpty(region.bgName))
                {
                    var img = bg.GetComponent<Image>();
                    if (img != null)
                    {
                        Sprite sprite = LoadBackgroundSprite(region.bgName);
                        if (sprite != null) img.sprite = sprite;
                    }
                }

                // 4. 实例化摆放物
                var root = FindChildRecursive(zoneGo.transform, "Root");
                Transform parent = root != null ? root : zoneGo.transform;

                foreach (var b in region.buildings)
                {
                    SpawnPlacedObject(parent, b, MapObjectType.Building);
                    bldCount++;
                }
                foreach (var o in region.objects)
                {
                    SpawnPlacedObject(parent, o, MapObjectType.Prop);
                    objCount++;
                }
                foreach (var c in region.characters)
                {
                    SpawnCharacter(parent, c, config.gridSize);
                    charCount++;
                }

                zoneGo.SetActive(true);
            }

            // 5. 适配 ZoneRoot 包围盒 + 相机
            MapLoadManager.RefreshMapBoundsAndCamera(zoneRoot.parent != null ? zoneRoot.parent.gameObject : zoneRoot.gameObject, 2048f, null);

            string result = $"[MapConfigLoader] 加载完成: {mapName} 分区={config.regions.Count} " +
                           $"建筑={bldCount} 物件={objCount} 人物={charCount} " +
                           $"地图尺寸={config.width}x{config.height} 网格={config.gridCols}x{config.gridRows}";
            Debug.Log(result);
            return result;
        }

        /// <summary>
        /// Lua 调用入口：按地图名称加载配置并还原场景
        /// </summary>
        /// <param name="mapName">地图名称（Map_1）</param>
        /// <param name="gameMainMap">GameMainMap 根节点</param>
        /// <param name="zonePrefabPath">ZoneItem 预制体路径</param>
        public static string LoadMapConfigLua(string mapName, GameObject gameMainMap, string zonePrefabPath)
        {
            if (gameMainMap == null) return "[MapConfigLoader] gameMainMap 为空";

            var zoneRoot = MapLoadManager.CreateZoneRoot(gameMainMap.transform);

            GameObject zonePrefab = null;
#if UNITY_EDITOR
            zonePrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(zonePrefabPath);
#endif
            if (zonePrefab == null)
                zonePrefab = Resources.Load<GameObject>(zonePrefabPath);
            if (zonePrefab == null)
                return $"[MapConfigLoader] 预制体未找到: {zonePrefabPath}";

            return LoadMapConfig(mapName, zoneRoot, zonePrefab);
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 加载背景 Sprite（Resources 路径优先，Editor 下兼容 AssetDatabase）
        /// </summary>
        private static Sprite LoadBackgroundSprite(string bgName)
        {
            if (string.IsNullOrEmpty(bgName)) return null;

            // 运行时：从 Resources 加载
            Sprite sprite = Resources.Load<Sprite>($"MapBgs/{bgName}");
            if (sprite != null) return sprite;

            // Editor 下：从 AssetDatabase 加载
#if UNITY_EDITOR
            string[] candidates = new string[]
            {
                $"Assets/Res/Textures/Map/{bgName}.png",
                $"Assets/Res/Textures/Map/{bgName}.jpg",
                $"Assets/GameData/MapBgs/{bgName}.png",
            };
            foreach (var path in candidates)
            {
                Texture2D tex = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (tex != null)
                {
                    return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect);
                }
            }
#endif
            Debug.LogWarning($"[MapConfigLoader] 未找到背景图: {bgName}");
            return null;
        }

        /// <summary>
        /// 生成摆放物占位节点（建筑/物件）
        /// </summary>
        private static GameObject SpawnPlacedObject(Transform parent, MapObjectJson data, MapObjectType type)
        {
            string tag = type == MapObjectType.Building ? "Bld" : "Obj";
            var go = new GameObject($"{tag}_{data.templateId}_{data.id}");
            go.transform.SetParent(parent, false);
            go.transform.localPosition = Vector3.zero;

            var rt = go.AddComponent<RectTransform>();
            float gridSize = 32f;
            rt.sizeDelta = new Vector2(data.width * gridSize, data.height * gridSize);
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 1);
            rt.anchoredPosition = new Vector2(data.posX, -data.posY);

            var img = go.AddComponent<Image>();
            img.color = type == MapObjectType.Building
                ? new Color(0.2f, 0.8f, 0.8f, 0.5f)
                : new Color(0.2f, 0.8f, 0.2f, 0.5f);
            img.raycastTarget = false;

            go.SetActive(true);
            return go;
        }

        /// <summary>
        /// 生成人物节点（含路径点标记）
        /// </summary>
        private static GameObject SpawnCharacter(Transform parent, MapCharacterJson data, float gridSize)
        {
            var go = new GameObject($"Char_{data.templateId}_{data.id}");
            go.transform.SetParent(parent, false);

            var rt = go.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(data.width * gridSize, data.height * gridSize);
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 1);
            rt.anchoredPosition = new Vector2(data.posX, -data.posY);

            var img = go.AddComponent<Image>();
            img.color = new Color(1f, 0.9f, 0.2f, 0.6f);
            img.raycastTarget = false;

            // 路径点标记
            if (data.pathPoints != null && data.pathPoints.Count > 0)
            {
                var pathGo = new GameObject("PathPoints");
                pathGo.transform.SetParent(parent, false);
                var pathRt = pathGo.AddComponent<RectTransform>();
                pathRt.anchorMin = pathRt.anchorMax = pathRt.pivot = new Vector2(0, 1);
                pathRt.sizeDelta = Vector2.zero;
                pathRt.anchoredPosition = Vector2.zero;

                for (int i = 0; i < data.pathPoints.Count; i++)
                {
                    var pt = data.pathPoints[i];
                    var ptGo = new GameObject($"Path_{i}");
                    ptGo.transform.SetParent(pathGo.transform, false);
                    var ptRt = ptGo.AddComponent<RectTransform>();
                    ptRt.sizeDelta = new Vector2(16, 16);
                    ptRt.anchorMin = ptRt.anchorMax = ptRt.pivot = new Vector2(0, 1);
                    float absX = pt.gridX * gridSize;
                    float absY = pt.gridY * gridSize;
                    ptRt.anchoredPosition = new Vector2(absX, -absY);
                    var ptImg = ptGo.AddComponent<Image>();
                    ptImg.color = Color.cyan;
                    ptImg.raycastTarget = false;
                }
            }

            go.SetActive(true);
            return go;
        }

        /// <summary>递归查找子节点</summary>
        private static Transform FindChildRecursive(Transform parent, string name)
        {
            if (parent.name == name) return parent;
            foreach (Transform child in parent)
            {
                var result = FindChildRecursive(child, name);
                if (result != null) return result;
            }
            return null;
        }

        #endregion
    }

    /// <summary>
    /// 物体类型枚举（运行时镜像）
    /// </summary>
    public enum MapObjectType
    {
        Building = 0,
        Character = 1,
        Prop = 2
    }
}
