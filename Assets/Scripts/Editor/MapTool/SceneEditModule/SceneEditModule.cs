/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SceneEditModule.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景编辑模块 - 公有接口部分（初始化/地图保存/物体选择/显隐）
 ***************************************************************/

using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Editor.MapEditor;
using System.Linq;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 场景编辑模块 - 公有接口（初始化/保存/选择/显隐）
    /// </summary>
    public partial class SceneEditModule
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="root">根节点</param>
        public void Init(VisualElement root)
        {
            m_Root = root;
            // 初始化物体模板管理器
            ObjectTemplateManager.Instance.Initialize();
            FindElements();
            BindEvents();
        }

        /// <summary>
        /// 保存地图配置为 JSON 文件
        /// 包含分区信息（区域ID/背景名/是否解锁/迷雾是否开启）、
        /// 物件集合、建筑集合、人物集合（含可走路径）
        /// </summary>
        public void SaveMapConfig()
        {
            if (m_RegionIds == null || m_RegionIds.Length == 0)
            {
                Debug.LogWarning("没有分区数据，无法保存地图配置（请先选择章节加载地图）");
                return;
            }
            // 组装配置数据
            MapConfigData data = new MapConfigData();
            data.sceneId = GetFieldValue(m_SceneIdField, "105");
            data.sceneName = GetFieldValue(m_SceneNameField, "105");
            // ---TODO 修复地图尺寸：width/height 用地图实际渲染尺寸（分区拼接总宽高 m_MapWidth/m_MapHeight），
            // 而非 UI 字段默认值——否则多分区章节（如 3 分区→4096×4096）被存成 2048，运行时/还原时角色坐标（3648/3808）超出地图不可见
            data.width = m_MapWidth > 0 ? m_MapWidth : ParseInt(GetFieldValue(m_WidthField, m_ConfigMapWidth.ToString()), m_ConfigMapWidth);
            data.height = m_MapHeight > 0 ? m_MapHeight : ParseInt(GetFieldValue(m_HeightField, m_ConfigMapHeight.ToString()), m_ConfigMapHeight);
            // 分区信息（每个分区下挂属于自己的建筑/物件/人物集合）
            int regionPixelSize = MapToolConstants.RegionPixelSize;
            for (int i = 0; i < m_RegionCount; i++)
            {
                int row = i / m_RegionCols;
                int col = i % m_RegionCols;
                string regionId = m_RegionIds[i];
                bool unlocked = m_RegionUnlocked != null && row < m_RegionRows && col < m_RegionCols && m_RegionUnlocked[row, col];
                string bgName = "";
                if (m_LevelBgDict != null && m_LevelBgDict.TryGetValue(regionId, out string bg))
                    bgName = bg;
                data.regions.Add(new MapRegionData
                {
                    regionId = regionId,
                    bgName = bgName,
                    unlocked = unlocked,
                    fogEnabled = !unlocked, // 未解锁分区迷雾开启
                    col = col,
                    row = row,
                    offsetX = col * regionPixelSize,
                    offsetY = row * regionPixelSize,
                    width = regionPixelSize,
                    height = regionPixelSize,
                    gridCols = regionPixelSize / MapToolConstants.GridSize,
                    gridRows = regionPixelSize / MapToolConstants.GridSize,
                });
            }
            // 已放置物体按所在分区归组，挂到对应区域下
            foreach (var obj in m_PlacedObjects)
            {
                int regionIndex = GetRegionIndexForObject(obj);
                if (regionIndex < 0 || regionIndex >= data.regions.Count)
                    continue;
                MapRegionData region = data.regions[regionIndex];
                switch (obj.Type)
                {
                    case MapObjectType.Character: // 人物：含可走路径集合
                        region.characters.Add(BuildMapCharacterData(obj));
                        break;
                    case MapObjectType.Building: // 建筑
                        region.buildings.Add(BuildMapObjectData(obj));
                        break;
                    case MapObjectType.Prop: // 物件
                        region.objects.Add(BuildMapObjectData(obj));
                        break;
                }
            }
            // ---TODO 网格信息：填充格子尺寸/行列 + 摆放物占用格子（供运行时检查位置）
            data.gridSize = MapToolConstants.GridSize;
            data.gridCols = data.width > 0 ? data.width / MapToolConstants.GridSize : 0;
            data.gridRows = data.height > 0 ? data.height / MapToolConstants.GridSize : 0;
            var occupiedKeys = new HashSet<long>();
            foreach (var obj in m_PlacedObjects)
            {
                int gx = Mathf.FloorToInt(obj.Position.x / MapToolConstants.GridSize);
                int gy = Mathf.FloorToInt(obj.Position.y / MapToolConstants.GridSize);
                int spanX = Mathf.Max(1, obj.Width);
                int spanY = Mathf.Max(1, obj.Height);
                for (int dy = 0; dy < spanY; dy++)
                {
                    for (int dx = 0; dx < spanX; dx++)
                    {
                        occupiedKeys.Add(((long)(gy + dy)) * 100000 + (gx + dx)); // 行列打包去重
                    }
                }
            }
            data.occupiedCells = new List<MapGridCellData>();
            foreach (var key in occupiedKeys)
            {
                data.occupiedCells.Add(new MapGridCellData { x = (int)(key % 100000), y = (int)(key / 100000) });
            }
            // ---TODO 双格式保存：组装完成后补填章节信息（JsonUtility 序列化 public 字段）
            data.chapterId = !string.IsNullOrEmpty(m_CurrentChapterId) ? m_CurrentChapterId : data.sceneId;
            data.chapterName = m_CurrentChapterData != null && !string.IsNullOrEmpty(m_CurrentChapterData.ChapterTitle)
                ? m_CurrentChapterData.ChapterTitle
                : data.sceneName;
            data.regionCount = m_RegionCount;
            data.regionRows = m_RegionRows;
            data.regionCols = m_RegionCols;
            // 写入 JSON 文件（m_MapSavePath 为 Assets 相对路径）
            string relativePath = m_MapSavePath.Replace("\\", "/");
            if (!relativePath.StartsWith("Assets/"))
                relativePath = "Assets/" + relativePath.TrimStart('/');
            string fullPath = Path.Combine(Application.dataPath, relativePath.Substring("Assets/".Length));
            try
            {
                string dir = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(fullPath, json, new UTF8Encoding(false));
                AssetDatabase.Refresh();
                // 统计各区域物体数量
                int totalBuildings = 0, totalObjects = 0, totalCharacters = 0;
                foreach (var r in data.regions)
                {
                    totalBuildings += r.buildings.Count;
                    totalObjects += r.objects.Count;
                    totalCharacters += r.characters.Count;
                }
                Debug.Log($"地图配置已保存: {relativePath}（分区 {data.regions.Count}，建筑 {totalBuildings}，物件 {totalObjects}，人物 {totalCharacters}）");
                // ---TODO 双格式保存：JSON 写盘成功后导出 Lua 分区数据 + 重写主表索引
                try
                {
                    string exportChapterId = data.chapterId;
                    string exportChapterName = data.chapterName;
                    string exportMapName = m_CurrentChapterData != null && !string.IsNullOrEmpty(m_CurrentChapterData.MapName)
                        ? m_CurrentChapterData.MapName
                        : data.sceneName;
                    // LevelIdBase：当前章节在 m_ChapterDataList 中之前各章节的分区数累加（§7）；不在列表则 0
                    int levelIdBase = 0;
                    int currentChapterIndex = m_CurrentChapterData != null ? m_ChapterDataList.IndexOf(m_CurrentChapterData) : -1;
                    if (currentChapterIndex >= 0)
                    {
                        for (int k = 0; k < currentChapterIndex; k++)
                        {
                            levelIdBase += MapToolEditorUtils.ParseRegionIds(m_ChapterDataList[k].ChapterInfoID).Length;
                        }
                    }
                    MapChapterExportContext exportContext = new MapChapterExportContext
                    {
                        ChapterId = exportChapterId,
                        ChapterName = exportChapterName,
                        MapName = exportMapName,
                        RegionIds = m_RegionIds,
                        LevelIdBase = levelIdBase,
                    };
                    List<string> luaFiles = MapDataLuaExporter.Export(data, exportContext, m_ChapterDataList);
                    AssetDatabase.Refresh();
                    Debug.Log($"Lua 分区数据已导出: 共 {luaFiles.Count} 个文件\n{string.Join("\n", luaFiles)}");
                }
                catch (System.Exception luaException)
                {
                    // Lua 导出失败不影响 JSON 已保存成功
                    Debug.LogError($"Lua 数据导出失败（JSON 已保存成功，不影响）: {luaException.Message}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"保存地图配置失败: {e.Message}");
            }
        }

        /// <summary>
        /// 显示模块
        /// </summary>
        public void Show()
        {
            m_Root.style.display = DisplayStyle.Flex;
        }

        /// <summary>
        /// 隐藏模块
        /// </summary>
        public void Hide()
        {
            m_Root.style.display = DisplayStyle.None;
        }

        /// <summary>
        /// 设置地图保存路径
        /// </summary>
        /// <param name="path">保存路径</param>
        public void SetMapSavePath(string path)
        {
            m_MapSavePath = path;
            UpdateStatusBar();
        }

        /// <summary>
        /// 获取地图保存路径
        /// </summary>
        public string GetMapSavePath()
        {
            return m_MapSavePath;
        }

        /// <summary>
        /// 加载章节地图
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="prefabPath">地图预制体路径</param>
        public void LoadChapterMap(string chapterId, string prefabPath = "", string chapterInfoID = "9")
        {
            Debug.Log($"加载章节地图: {chapterId}, 预制体路径: {prefabPath}, 分区配置: {chapterInfoID}");
            // 加载章节配置表，获取场景名称/场景ID/分区配置
            LoadChapterConfig();
            var chapterData = m_ChapterDataList.FirstOrDefault(c => c.ID == chapterId);
            if (chapterData != null)
            {
                // ---TODO 双格式保存：缓存当前章节上下文（JSON/Lua 同源导出用）
                m_CurrentChapterId = chapterId;
                m_CurrentChapterData = chapterData;
                // 场景名：优先用章节标题，其次地图资源名
                string sceneName = !string.IsNullOrEmpty(chapterData.ChapterTitle) ? chapterData.ChapterTitle : chapterData.MapName;
                if (string.IsNullOrEmpty(sceneName))
                    sceneName = chapterId;
                if (m_SceneNameField != null)
                    m_SceneNameField.SetValueWithoutNotify(sceneName);
                if (m_SceneIdField != null)
                    m_SceneIdField.SetValueWithoutNotify(chapterId);
                // 调用方未指定分区配置时，用章节配置表里的分区配置
                if (string.IsNullOrEmpty(chapterInfoID) || chapterInfoID == "9")
                    chapterInfoID = chapterData.ChapterInfoID ?? chapterInfoID;
            }
            else
            {
                // ---TODO 双格式保存：配置表无此章节时清空章节上下文缓存
                m_CurrentChapterId = chapterId;
                m_CurrentChapterData = null;
                // 配置表中无此章节：场景ID用章节ID，场景名用ID兜底
                if (m_SceneNameField != null)
                    m_SceneNameField.SetValueWithoutNotify(chapterId);
                if (m_SceneIdField != null)
                    m_SceneIdField.SetValueWithoutNotify(chapterId);
            }
            // 解析 ChapterInfoID 配置，格式如 {"1","2","3"}
            // 有几个元素就有几个分区
            string[] regionIds = MapToolEditorUtils.ParseRegionIds(chapterInfoID);
            m_RegionIds = regionIds;
            int regionCount = regionIds.Length;
            Debug.Log($"分区数量: {regionCount}");
            // 加载 TableMainLevels 配置表
            LoadMainLevelsConfig();
            // 计算行列数（尽量接近正方形）
            int cols = Mathf.CeilToInt(Mathf.Sqrt(regionCount));
            int rows = Mathf.CeilToInt((float)regionCount / cols);
            m_RegionRows = rows;
            m_RegionCols = cols;
            Debug.Log($"分区布局: {rows}行 x {cols}列");
            // 初始化分区解锁状态
            m_RegionUnlocked = new bool[rows, cols];
            m_RegionCount = regionCount; // 记录实际分区数量
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    int index = row * cols + col;
                    // 只有配置中存在的分区才显示，第一个默认解锁
                    if (index < regionCount)
                    {
                        m_RegionUnlocked[row, col] = (index == 0);
                    }
                    else
                    {
                        m_RegionUnlocked[row, col] = false; // 多余的格子不显示
                    }
                }
            }
            string chapterPrefabPath = $"{MapToolConstants.MapSaveDir}/Map_{chapterId}.prefab";
            // ---TODO 模板更新检测：章节预制体不存在，或模板预制体比章节预制体更新时，重新从模板克隆
            // 用户修改 GameMainMap.prefab 模板后，下次加载自动同步到章节预制体
            bool needClone = false;
            var existingChapterPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(chapterPrefabPath);
            if (existingChapterPrefab == null)
            {
                needClone = true; // 章节预制体不存在，需要克隆
            }
            else if (!string.IsNullOrEmpty(prefabPath))
            {
                // 模板预制体存在，比较两者的最后修改时间
                string templateFullPath = System.IO.Path.Combine(Application.dataPath, prefabPath.Replace("Assets/", ""));
                string chapterFullPath = System.IO.Path.Combine(Application.dataPath, chapterPrefabPath.Replace("Assets/", ""));
                if (System.IO.File.Exists(templateFullPath) && System.IO.File.Exists(chapterFullPath))
                {
                    DateTime templateTime = System.IO.File.GetLastWriteTime(templateFullPath);
                    DateTime chapterTime = System.IO.File.GetLastWriteTime(chapterFullPath);
                    if (templateTime > chapterTime)
                    {
                        needClone = true; // 模板比章节预制体新，重新克隆
                        Debug.Log($"模板预制体已更新（模板: {templateTime:HH:mm:ss} > 章节: {chapterTime:HH:mm:ss}），重新克隆");
                    }
                }
            }
            if (needClone && !string.IsNullOrEmpty(prefabPath))
            {
                Debug.Log($"从模板克隆: {prefabPath} -> {chapterPrefabPath}");
                // 确保目录存在（Assets/GameData/Maps）
                string dir = System.IO.Path.GetDirectoryName(chapterPrefabPath).Replace("\\", "/");
                if (!UnityEditor.AssetDatabase.IsValidFolder(dir))
                {
                    if (!UnityEditor.AssetDatabase.IsValidFolder(MapToolConstants.GameDataDir))
                        UnityEditor.AssetDatabase.CreateFolder("Assets", "GameData");
                    UnityEditor.AssetDatabase.CreateFolder(MapToolConstants.GameDataDir, "Maps");
                }
                // 克隆模板预制体
                bool copyResult = UnityEditor.AssetDatabase.CopyAsset(prefabPath, chapterPrefabPath);
                Debug.Log($"克隆结果: {copyResult}");
                UnityEditor.AssetDatabase.Refresh();
            }
            // 加载章节预制体
            m_MapPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(chapterPrefabPath);
            Debug.Log($"加载章节预制体: {chapterPrefabPath}, 结果: {m_MapPrefab != null}");
            if (m_MapPrefab != null)
            {
                Debug.Log($"加载地图预制体成功: {m_MapPrefab.name}");
                // 同步更新左上角的 ObjectField 显示
                Debug.Log($"m_MapPrefabField 是否为 null: {m_MapPrefabField == null}");
                if (m_MapPrefabField != null)
                {
                    m_MapPrefabField.value = m_MapPrefab;
                    Debug.Log($"已设置 ObjectField 值: {m_MapPrefab.name}");
                }
                // 注意：不能直接修改预制体资源，迷雾覆盖在 IMGUI 中绘制
            }
            else
            {
                Debug.LogWarning("地图预制体加载失败，m_MapPrefab 为 null");
            }
            // 更新保存路径
            // ---TODO 双格式保存：JSON 改落 MapDatas（预制体路径仍走 MapSaveDir，保持 Maps 不动）
            m_MapSavePath = $"{MapToolConstants.MapJsonSaveDir}/Map_{chapterId}.json";
            m_MapWidth = MapToolConstants.RegionPixelSize * cols;   // 渲染用全布局宽度
            m_MapHeight = MapToolConstants.RegionPixelSize * rows;  // 渲染用全布局高度
            // 根据已解锁分区包围盒更新配置宽高（解锁新区域时同步变化）
            UpdateMapBoundsFromUnlockedRegions();
            // 重置偏移和缩放
            m_MapOffset = Vector2.zero;
            m_Zoom = 1.0f;
            UpdateStatusBar();

            // ---TODO 编辑器加载还原：有已保存 JSON 时优先加载已保存的摆放物/分区解锁/背景（LoadMapConfig）
            LoadMapConfig();

            // ---TODO 所有数据加载完成后，再同步到场景（分区背景/摆放物/网格）
            if (m_MapPrefab != null)
            {
                InstantiateMapInScene(rows, cols);
            }
        }

        /// <summary>
        /// 在场景中实例化 2DSceneRoot，地图预制体挂载到 SceneLauncher 下，
        /// 并在 ZoneRoot 下按从左到右、从上到下排列 ZoneItem 分区
        /// </summary>
        /// <param name="rows">分区行数</param>
        /// <param name="cols">分区列数</param>
        private void InstantiateMapInScene(int rows, int cols)
        {
            // 1. 清理旧实例
            var oldSceneRoot = GameObject.Find("2DSceneRoot");
            if (oldSceneRoot != null) UnityEngine.Object.DestroyImmediate(oldSceneRoot);

            // 2. 查找 Launcher 节点作为父级
            var launcher = GameObject.Find("Launcher");
            if (launcher == null)
            {
                Debug.LogWarning("场景中未找到 Launcher 节点，请先打开包含 GameApp/GameMain/Launcher 的场景");
                return;
            }

            // 3. 实例化 2DSceneRoot.prefab
            string sceneRootPath = "Assets/Res/Prefabs/SceneRoot/2DSceneRoot.prefab";
            var sceneRootPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(sceneRootPath);
            if (sceneRootPrefab == null)
            {
                Debug.LogWarning($"未找到 2DSceneRoot.prefab: {sceneRootPath}");
                return;
            }
            var sceneRootInstance = (GameObject)PrefabUtility.InstantiatePrefab(sceneRootPrefab, launcher.transform);
            sceneRootInstance.name = "2DSceneRoot";
            Undo.RegisterCreatedObjectUndo(sceneRootInstance, "Instantiate SceneRoot");

            // 4. 查找 SceneLauncher 节点（2DSceneRoot → 2DSceneLayers → SceneLauncher）
            Transform sceneLauncher = null;
            foreach (Transform child in sceneRootInstance.transform)
            {
                if (child.name == "2DSceneLayers")
                {
                    foreach (Transform layer in child)
                    {
                        if (layer.name == "SceneLauncher")
                        {
                            sceneLauncher = layer;
                            break;
                        }
                    }
                }
            }
            if (sceneLauncher == null)
            {
                Debug.LogWarning("2DSceneRoot 中未找到 SceneLauncher 节点");
                return;
            }

            // 5. 实例化地图预制体，挂载到 SceneLauncher 下
            var mapInstance = (GameObject)PrefabUtility.InstantiatePrefab(m_MapPrefab, sceneLauncher);
            mapInstance.name = "GameMainMap";
            Undo.RegisterCreatedObjectUndo(mapInstance, "Instantiate Map");

            // 6. 查找 ZoneRoot（递归搜索所有子节点）
            RectTransform zoneRoot = null;
            var allRects = mapInstance.GetComponentsInChildren<RectTransform>(true);
            foreach (var rt in allRects)
            {
                if (rt.name == "ZoneRoot")
                {
                    zoneRoot = rt;
                    break;
                }
            }
            if (zoneRoot == null)
            {
                Debug.LogWarning($"GameMainMap 中未找到 ZoneRoot 节点，现有子节点: {string.Join(", ", allRects.Select(r => r.name))}");
                return;
            }

            // 记录 ZoneRoot 引用，供增删物体后刷新场景同步
            m_ZoneRootInstance = zoneRoot;

            // ---TODO ZoneRoot 自带 Grid Layout Group（Cell 2048x2048, UpperLeft→Horizontal），自动排列子分区
            // 不需要手动设置 anchoredPosition，GridLayoutGroup 会从左到右从上到下自动排列

            // ---TODO 禁用 GameMainMap 自带的默认背景 Image 组件（分区背景由 ZoneItem 提供）
            foreach (Transform child in mapInstance.transform)
            {
                var img = child.GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                {
                    img.enabled = false;
                    Debug.Log($"[分区背景] 已禁用默认 Image: {child.name}");
                    break;
                }
            }

            // 7. 加载 ZoneItem.prefab
            string zoneItemPath = "Assets/Res/Prefabs/Map/ZoneItem.prefab";
            var zoneItemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(zoneItemPath);
            if (zoneItemPrefab == null)
            {
                Debug.LogWarning($"未找到 ZoneItem.prefab: {zoneItemPath}");
                return;
            }

            // 8. 清理 ZoneRoot 下旧的分区实例
            for (int i = zoneRoot.childCount - 1; i >= 0; i--)
            {
                var child = zoneRoot.GetChild(i);
                if (child.name.StartsWith("ZoneItem"))
                {
                    UnityEngine.Object.DestroyImmediate(child.gameObject);
                }
            }

            // 9. 按从左到右、从上到下排列实例化分区
            float regionSize = MapToolConstants.RegionPixelSize;
            int totalRegions = m_RegionCount;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    int index = row * cols + col;
                    if (index >= totalRegions) break;

                    string regionId = m_RegionIds[index];
                    var zoneItem = (GameObject)PrefabUtility.InstantiatePrefab(zoneItemPrefab, zoneRoot);
                    zoneItem.name = $"ZoneItem_{regionId}";

                    // ---TODO 同步分区背景图到 ZoneItem 的 RawImage
                    Texture bgTex = LoadRegionBackground(regionId);
                    if (bgTex != null)
                    {
                        // ZoneItem.prefab 用的是 Image 组件（Sprite），不是 RawImage
                        var img = zoneItem.GetComponentInChildren<UnityEngine.UI.Image>();
                        if (img != null)
                        {
                            Texture2D tex2d = bgTex as Texture2D;
                            if (tex2d != null)
                            {
                                img.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0.5f, 0.5f));
                                Debug.Log($"[分区背景] {regionId} 赋值成功: {bgTex.name}");
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"[分区背景] {regionId} 找到纹理但 ZoneItem 内无 Image 组件");
                        }
                    }
                    else
                    {
                        string bgName = m_LevelBgDict.TryGetValue(regionId, out string bn) ? bn : "<未配置>";
                        Debug.LogWarning($"[分区背景] {regionId} 加载失败 bgName={bgName}，字典Keys: {string.Join(",", m_LevelBgDict.Keys)}");
                    }
                }
            }

            // ---TODO 修正 ZoneRoot 尺寸为实际分区布局大小（Content Size Fitter 会按最大9格计算，需要覆盖）
            float regionPixelSize = MapToolConstants.RegionPixelSize;
            zoneRoot.sizeDelta = new Vector2(cols * regionPixelSize, rows * regionPixelSize);
            m_RegionCols = cols;
            m_RegionRows = rows;

            // ---TODO 同步摆放物到场景中（在 ZoneRoot 下生成占位 Image）
            SyncPlacedObjectsToScene(zoneRoot);

            // ---TODO 自动开启 GridManager 调试网格（递归搜索整个地图实例）
            var gridMgr = mapInstance.GetComponentInChildren<Honor.Runtime.GridManager>(true);
            if (gridMgr != null)
            {
                gridMgr.DrawDebugGrid = true;
                Debug.Log($"[网格] GridManager 已开启，挂载在: {gridMgr.gameObject.name}");
            }
            else
            {
                Debug.LogWarning("[网格] 未在地图实例中找到 GridManager 组件");
            }

            Debug.Log($"场景地图实例化完成: {rows}行 x {cols}列，共 {totalRegions} 个分区");
            Selection.activeGameObject = mapInstance;
        }

        /// <summary>递归查找子节点</summary>
        private Transform FindChildRecursive(Transform parent, string name)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                if (child.name == name) return child;
                var found = FindChildRecursive(child, name);
                if (found != null) return found;
            }
            return null;
        }

        /// <summary>
        /// 将编辑器中的摆放物同步到场景 ZoneItem 下，每个物体生成一个带预览图的 RawImage
        /// 物体根据位置自动归属到对应分区的 ZoneItem 节点
        /// </summary>
        /// <param name="zoneRoot">ZoneRoot 节点（分区父节点）</param>
        private void SyncPlacedObjectsToScene(RectTransform zoneRoot)
        {
            if (zoneRoot == null) return;

            // 收集所有 ZoneItem，记录每个分区的 ZoneItem Transform
            var zoneItems = new List<Transform>();
            for (int i = 0; i < zoneRoot.childCount; i++)
            {
                var child = zoneRoot.GetChild(i);
                if (child.name.StartsWith("ZoneItem_"))
                    zoneItems.Add(child);
            }

            // 清理旧摆放物（遍历所有 ZoneItem，删除 Char_/Obj_/Path_ 开头的节点）
            foreach (var zi in zoneItems)
            {
                for (int i = zi.childCount - 1; i >= 0; i--)
                {
                    var c = zi.GetChild(i);
                    if (c.name.StartsWith("Obj_") || c.name.StartsWith("Char_")
                        || c.name.StartsWith("Path_") || c.name.StartsWith("PathLine_"))
                        UnityEngine.Object.DestroyImmediate(c.gameObject);
                    // 同时清理 Root 下的旧物体
                    if (c.name == "Root")
                    {
                        for (int j = c.childCount - 1; j >= 0; j--)
                        {
                            var cc = c.GetChild(j);
                            if (cc.name.StartsWith("Obj_") || cc.name.StartsWith("Char_")
                                || cc.name.StartsWith("Path_") || cc.name.StartsWith("PathLine_"))
                                UnityEngine.Object.DestroyImmediate(cc.gameObject);
                        }
                    }
                }
            }

            if (m_PlacedObjects == null || m_PlacedObjects.Count == 0) return;

            float gridSize = MapToolConstants.GridSize;
            float regionSize = MapToolConstants.RegionPixelSize;

            foreach (var obj in m_PlacedObjects)
            {
                int col = Mathf.FloorToInt(obj.Position.x / regionSize);
                int row = Mathf.FloorToInt(obj.Position.y / regionSize);
                int cols = m_RegionCols > 0 ? m_RegionCols : zoneItems.Count;
                int idx = row * cols + col;
                if (idx < 0 || idx >= zoneItems.Count) continue;

                // 挂到分区的 Root 节点下（Root 已修正为 stretch 填满 ZoneItem）
                Transform zi = zoneItems[idx];
                Transform rootNode = FindChildRecursive(zi, "Root");
                RectTransform parent = rootNode != null ? rootNode as RectTransform : zi as RectTransform;

                Vector2 localPos = new Vector2(obj.Position.x - col * regionSize, obj.Position.y - row * regionSize);

                var go = new GameObject();
                go.transform.SetParent(parent, false);
                go.name = obj.Type == MapObjectType.Character ? $"Char_{obj.Name}" : $"Obj_{obj.Name}";

                var rt = go.AddComponent<RectTransform>();
                // 和编辑器一致：左上角原点，Y 向下
                rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 1);
                rt.anchoredPosition = new Vector2(localPos.x, -localPos.y);
                rt.sizeDelta = new Vector2(obj.Width * gridSize, obj.Height * gridSize);

                var rawImg = go.AddComponent<UnityEngine.UI.RawImage>();
                rawImg.texture = obj.PreviewTexture as Texture2D;
                rawImg.color = obj.Type == MapObjectType.Character ? new Color(1,1,0,0.8f)
                             : obj.Type == MapObjectType.Building ? new Color(0,1,1,0.6f)
                             : new Color(0,1,0,0.6f);

                int tileX = Mathf.RoundToInt(localPos.x / gridSize);
                int tileY = Mathf.RoundToInt(localPos.y / gridSize);
                Debug.Log($"[坐标校验] {go.name}: parent={parent.name} pos=({localPos.x:F0},{localPos.y:F0}) anchored=({rt.anchoredPosition.x:F0},{rt.anchoredPosition.y:F0}) 格子=({tileX},{tileY})");
            }
            Debug.Log($"已同步 {m_PlacedObjects.Count} 个摆放物到场景");
            SyncPathPointsToScene(zoneRoot);
        }

        /// <summary>
        /// 将全局路径点和物体路径点同步到场景：蓝色圆点 + 连线
        /// </summary>
        private void SyncPathPointsToScene(RectTransform zoneRoot)
        {
            // 清理旧路径点标记（在所有分区 Root 下清理）
            for (int i = 0; i < zoneRoot.childCount; i++)
            {
                var zoneChild = zoneRoot.GetChild(i);
                if (!zoneChild.name.StartsWith("ZoneItem_")) continue;
                // 找内部 Root 节点
                for (int j = 0; j < zoneChild.childCount; j++)
                {
                    if (zoneChild.GetChild(j).name != "Root") continue;
                    var rootT = zoneChild.GetChild(j);
                    for (int k = rootT.childCount - 1; k >= 0; k--)
                    {
                        var c = rootT.GetChild(k);
                        if (c.name.StartsWith("Path_") || c.name.StartsWith("PathLine_"))
                            UnityEngine.Object.DestroyImmediate(c.gameObject);
                    }
                }
            }

            float regionSize = MapToolConstants.RegionPixelSize;
            int cols = m_RegionCols > 0 ? m_RegionCols : 1;

            // 辅助：根据坐标找到对应分区的 Root 节点，并返回分区内局部坐标
            RectTransform GetZoneRootAt(Vector2 globalPos, out Vector2 localPos)
            {
                int col = Mathf.FloorToInt(globalPos.x / regionSize);
                int row = Mathf.FloorToInt(globalPos.y / regionSize);
                localPos = new Vector2(globalPos.x - col * regionSize, globalPos.y - row * regionSize);
                int idx = row * cols + col;
                int zi = 0;
                for (int i = 0; i < zoneRoot.childCount; i++)
                {
                    if (!zoneRoot.GetChild(i).name.StartsWith("ZoneItem_")) continue;
                    if (zi == idx)
                    {
                        var zc = zoneRoot.GetChild(i);
                        for (int j = 0; j < zc.childCount; j++)
                            if (zc.GetChild(j).name == "Root") return zc.GetChild(j) as RectTransform;
                        return zc as RectTransform;
                    }
                    zi++;
                }
                localPos = globalPos;
                return zoneRoot;
            }

            // 每个物体的路径点
            int lineIndex = 0;
            foreach (var obj in m_PlacedObjects)
            {
                if (obj.PathPoints == null || obj.PathPoints.Count < 2) continue;
                foreach (var p in obj.PathPoints)
                {
                    var parent = GetZoneRootAt(p, out Vector2 lp);
                    CreatePathMarker(parent, lp, $"Path_{obj.Name}_{obj.PathPoints.IndexOf(p)}");
                }
                for (int i = 0; i < obj.PathPoints.Count - 1; i++)
                {
                    var parent = GetZoneRootAt(obj.PathPoints[i], out Vector2 lp1);
                    GetZoneRootAt(obj.PathPoints[i + 1], out Vector2 lp2);
                    CreatePathLine(parent, lp1, lp2, $"PathLine_{lineIndex++}");
                }
            }

            // 全局路径点
            if (m_PathPoints != null && m_PathPoints.Count >= 2)
            {
                for (int i = 0; i < m_PathPoints.Count; i++)
                {
                    var parent = GetZoneRootAt(m_PathPoints[i], out Vector2 lp);
                    CreatePathMarker(parent, lp, $"Path_Global_{i}");
                }
                for (int i = 0; i < m_PathPoints.Count - 1; i++)
                {
                    var parent = GetZoneRootAt(m_PathPoints[i], out Vector2 lp1);
                    GetZoneRootAt(m_PathPoints[i + 1], out Vector2 lp2);
                    CreatePathLine(parent, lp1, lp2, $"PathLine_G_{i}");
                }
            }
        }

        /// <summary>创建单个路径点标记（蓝色小圆点）</summary>
        private void CreatePathMarker(RectTransform parent, Vector2 pos, string name)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rt = go.AddComponent<RectTransform>();
            // ---TODO [坐标] 与父节点 Root 一致 top-left anchor，pos.y 向下取反
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 1);
            rt.sizeDelta = new Vector2(16, 16);
            rt.anchoredPosition = new Vector2(pos.x, -pos.y);
            var img = go.AddComponent<UnityEngine.UI.Image>();
            img.color = new Color(0.2f, 0.6f, 1f, 0.9f); // 蓝色
        }

        /// <summary>创建路径连线（细青色矩形）</summary>
        private void CreatePathLine(RectTransform parent, Vector2 p1, Vector2 p2, string name)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rt = go.AddComponent<RectTransform>();
            // ---TODO [坐标] top-left anchor + center pivot（旋转围绕中心），中点直接映射
            rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0.5f, 0.5f);
            Vector2 mid = (p1 + p2) / 2f;
            rt.anchoredPosition = new Vector2(mid.x, -mid.y);
            float dist = Vector2.Distance(p1, p2);
            rt.sizeDelta = new Vector2(dist, 3);
            float angle = Mathf.Atan2(-(p2.y - p1.y), p2.x - p1.x) * Mathf.Rad2Deg;
            rt.localRotation = Quaternion.Euler(0, 0, angle);
            var img = go.AddComponent<UnityEngine.UI.Image>();
            img.color = new Color(0.2f, 1f, 1f, 0.7f); // 青色
        }

        /// <summary>
        /// 外部触发：摆放物增删改后调用，刷新场景中的同步节点
        /// </summary>
        public void RefreshSceneObjects()
        {
            if (m_ZoneRootInstance != null)
                SyncPlacedObjectsToScene(m_ZoneRootInstance);
        }

        /// <summary>
        /// 设置选中要放置的物体
        /// </summary>
        /// <param name="objectId">物体ID</param>
        /// <param name="objectName">物体名称</param>
        /// <param name="type">类型：0=建筑 1=人物 2=物件</param>
        public void SetSelectedObject(string objectId, string objectName, int type)
        {
            MapObjectType objectType = (MapObjectType)type;
            m_SelectedObjectId = objectId;
            m_SelectedObjectName = objectName;
            m_SelectedObjectType = objectType;

            // 创建待放置预览对象
            Vector2Int defaultSize = MapToolEditorUtils.GetDefaultObjectSize(objectType);
            Texture previewTex = LoadObjectPreview(objectId);

            // 从模板继承属性
            ObjectTemplate template = ObjectTemplateManager.Instance.GetTemplateById(objectId);
            m_PendingPlace = new PlacedObject
            {
                Id = System.Guid.NewGuid().ToString("N").Substring(0, 8), // 实例ID
                TemplateId = objectId, // 模板ID
                Name = objectName,
                Type = objectType,
                Width = defaultSize.x,
                Height = defaultSize.y,
                PreviewTexture = previewTex,
            };

            // 从模板复制属性
            if (template != null)
            {
                m_PendingPlace.AnimName = template.AnimName;
                m_PendingPlace.Loop = template.Loop;
                m_PendingPlace.Speed = template.Speed;
                m_PendingPlace.Interactable = template.Interactable;
                m_PendingPlace.InteractType = template.InteractType;
                m_PendingPlace.SortOrder = template.SortOrder;
                m_PendingPlace.CollisionEnabled = template.CollisionEnabled;
                m_PendingPlace.DialogBubble = template.DialogBubble;
                m_PendingPlace.FacingDirection = template.FacingDirection;
                m_PendingPlace.ShadowEnabled = template.ShadowEnabled;
                m_PendingPlace.IsCollectible = template.IsCollectible;
                m_PendingPlace.RespawnTime = template.RespawnTime;
            }

            // 选中人物资源时：自动绑定画布上第一个同名实例作为路径编辑目标，底部显示切页
            if (objectType == MapObjectType.Character)
            {
                PlacedObject target = m_PlacedObjects.FirstOrDefault(o => o.Name == objectName);
                if (target != null)
                {
                    SelectEditTarget(target);
                }
                else
                {
                    // 地图上还没有该人物实例：显示面板占位（放置后自动绑定）
                    ShowEditPanelWithName(objectName);
                }
            }
            Debug.Log($"选中物体: {objectName} ({objectId}), 类型: {type}");
        }

        /// <summary>
        /// 设置地图尺寸
        /// </summary>
        /// <param name="width">地图宽度</param>
        /// <param name="height">地图高度</param>
        public void SetMapSize(int width, int height)
        {
            m_MapWidth = width;
            m_MapHeight = height;
        }

        /// <summary>
        /// 获取地图宽度
        /// </summary>
        public int GetMapWidth()
        {
            return m_MapWidth;
        }

        /// <summary>
        /// 获取地图高度
        /// </summary>
        public int GetMapHeight()
        {
            return m_MapHeight;
        }

        /// <summary>
        /// 选中编辑目标物体（单击地图上已放置的人物/物体触发）
        /// </summary>
        /// <param name="obj">目标物体</param>
        public void SelectEditTarget(PlacedObject obj)
        {
            if (obj == null) return;
            m_EditTarget = obj;
            m_SelectedPathPointIndex = -1;

            // 只有人物才进入路径绘制模式
            bool isCharacter = obj.Type == MapObjectType.Character;
            m_IsPathDrawing = isCharacter;

            // 显示对象编辑面板
            if (m_ObjectEditPanel != null)
            {
                m_ObjectEditPanel.style.display = DisplayStyle.Flex;
                // 人物默认切到"可编辑路径"，其他类型切到"编辑动画"
                SetActiveObjectTab(isCharacter ? 0 : 1);
            }

            // 路径绘制按钮只在人物时显示
            if (m_BtnPathDraw != null)
            {
                m_BtnPathDraw.style.display = isCharacter ? DisplayStyle.Flex : DisplayStyle.None;
                m_BtnPathDraw.text = "停止绘制路径";
            }

            // 填充动画切页字段
            if (m_AnimNameField != null)
                m_AnimNameField.SetValueWithoutNotify(obj.AnimName);
            if (m_AnimLoopToggle != null)
                m_AnimLoopToggle.SetValueWithoutNotify(obj.Loop);
            if (m_AnimSpeedField != null)
                m_AnimSpeedField.SetValueWithoutNotify(obj.Speed.ToString("F1"));

            // 更新底部属性面板字段
            if (m_TemplateIdField != null)
                m_TemplateIdField.SetValueWithoutNotify(obj.TemplateId);
            if (m_TagField != null)
                m_TagField.SetValueWithoutNotify(obj.Type.ToString());
            if (m_GridPosField != null)
            {
                int gridX = Mathf.FloorToInt(obj.Position.x / 32f);
                int gridY = Mathf.FloorToInt(obj.Position.y / 32f);
                m_GridPosField.SetValueWithoutNotify($"{gridX}/{gridY}");
            }
            if (m_BuildingTemplateField != null)
                m_BuildingTemplateField.SetValueWithoutNotify($"{obj.Id}#{Mathf.FloorToInt(obj.Position.x / 32f)}");
            if (m_GridPrefixField != null)
                m_GridPrefixField.SetValueWithoutNotify("前缀");
            if (m_InteractStateField != null)
                m_InteractStateField.SetValueWithoutNotify(obj.Interactable ? "点击&移动" : "不可交互");

            // 填充新增属性面板字段
            if (m_SortOrderField != null)
                m_SortOrderField.SetValueWithoutNotify(obj.SortOrder.ToString());
            if (m_InteractTypeField != null)
                m_InteractTypeField.SetValueWithoutNotify(obj.InteractType);
            if (m_CollisionToggle != null)
                m_CollisionToggle.SetValueWithoutNotify(obj.CollisionEnabled);
            if (m_DialogBubbleField != null)
                m_DialogBubbleField.SetValueWithoutNotify(obj.DialogBubble);
            if (m_FacingField != null)
                m_FacingField.SetValueWithoutNotify(obj.FacingDirection);
            if (m_ShadowToggle != null)
                m_ShadowToggle.SetValueWithoutNotify(obj.ShadowEnabled);
            if (m_CollectibleToggle != null)
                m_CollectibleToggle.SetValueWithoutNotify(obj.IsCollectible);
            // ---TODO 气泡配置表通用化：回显气泡ID（引用 Tables.Bubble）
            if (m_BubbleIdField != null)
                m_BubbleIdField.SetValueWithoutNotify(obj.bubbleId);

            // 显示底部属性面板
            if (m_PropertyPanel != null)
                m_PropertyPanel.style.display = DisplayStyle.Flex;

            UpdateEditTargetUI();
            Debug.Log($"选中编辑对象: {obj.Name}（类型: {MapToolEditorUtils.FormatObjectTypeName(obj.Type)}，路径点数: {obj.PathPoints.Count}）");
        }

        /// <summary>
        /// 取消选中编辑目标
        /// </summary>
        public void ClearEditTarget()
        {
            m_EditTarget = null;
            m_SelectedPathPointIndex = -1;
            m_IsPathDrawing = false;
            if (m_ObjectEditPanel != null)
                m_ObjectEditPanel.style.display = DisplayStyle.None;
            // 隐藏底部属性面板
            if (m_PropertyPanel != null)
                m_PropertyPanel.style.display = DisplayStyle.None;
            Debug.Log("取消选中编辑对象");
        }

        /// <summary>
        /// 获取当前编辑目标
        /// </summary>
        public PlacedObject GetEditTarget()
        {
            return m_EditTarget;
        }

    }
}