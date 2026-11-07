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
        /// 【取消分区】单关卡=单张地图，直接保存整图所有物体
        /// </summary>
        public void SaveMapConfig()
        {
            if (string.IsNullOrEmpty(m_CurrentLevelId))
            {
                Debug.LogWarning("没有关卡数据，无法保存地图配置（请先选择关卡加载地图）");
                return;
            }
            // 组装配置数据
            MapConfigData data = new MapConfigData();
            data.sceneId = GetFieldValue(m_SceneIdField, "1");
            data.sceneName = GetFieldValue(m_SceneNameField, "关卡1");
            data.width = m_MapWidth > 0 ? m_MapWidth : ParseInt(GetFieldValue(m_WidthField, m_ConfigMapWidth.ToString()), m_ConfigMapWidth);
            data.height = m_MapHeight > 0 ? m_MapHeight : ParseInt(GetFieldValue(m_HeightField, m_ConfigMapHeight.ToString()), m_ConfigMapHeight);

            // 背景图
            string regionId = m_CurrentLevelId;
            string bgName = "";
            if (m_LevelBgDict != null && m_LevelBgDict.TryGetValue(regionId, out string bg))
                bgName = bg;
            data.bgName = bgName;

            // 所有物体直接挂到地图下（单关卡模式）
            foreach (var obj in m_PlacedObjects)
            {
                switch (obj.Type)
                {
                    case MapObjectType.Character:
                        data.characters.Add(BuildMapCharacterData(obj));
                        break;
                    case MapObjectType.Building:
                        data.buildings.Add(BuildMapObjectData(obj));
                        break;
                    case MapObjectType.Prop:
                        data.objects.Add(BuildMapObjectData(obj));
                        break;
                }
            }

            // 网格信息
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
                        occupiedKeys.Add(((long)(gy + dy)) * MapToolConstants.CellEncodeMultiplier + (gx + dx));
                    }
                }
            }
            data.occupiedCells = new List<MapGridCellData>();
            foreach (var key in occupiedKeys)
            {
                data.occupiedCells.Add(new MapGridCellData { x = (int)(key % MapToolConstants.CellEncodeMultiplier), y = (int)(key / MapToolConstants.CellEncodeMultiplier) });
            }

            // 章节信息
            data.chapterId = !string.IsNullOrEmpty(m_CurrentChapterId) ? m_CurrentChapterId : data.sceneId;
            data.chapterName = m_CurrentChapterData != null && !string.IsNullOrEmpty(m_CurrentChapterData.ChapterTitle)
                ? m_CurrentChapterData.ChapterTitle
                : data.sceneName;

            // 写入 JSON 文件（委托给 MapConfigRepository，本类只负责组装数据）
            if (!MapConfigRepository.SaveToJson(data, m_MapSavePath))
                return;

            // Lua 导出（可选，失败不影响 JSON 保存）
            try
            {
                string exportChapterId = data.chapterId;
                string exportChapterName = data.chapterName;
                string exportMapName = m_CurrentChapterData != null && !string.IsNullOrEmpty(m_CurrentChapterData.MapName)
                    ? m_CurrentChapterData.MapName
                    : data.sceneName;
                MapChapterExportContext exportContext = new MapChapterExportContext
                {
                    ChapterId = exportChapterId,
                    ChapterName = exportChapterName,
                    MapName = exportMapName,
                    LevelId = m_CurrentLevelId,
                    LevelIdBase = 0,
                };
                List<string> luaFiles = MapDataLuaExporter.Export(data, exportContext, m_ChapterDataList);
                AssetDatabase.Refresh();
                Debug.Log($"Lua 数据已导出: 共 {luaFiles.Count} 个文件\n{string.Join("\n", luaFiles)}");
            }
            catch (System.Exception luaException)
            {
                Debug.LogError($"Lua 数据导出失败（JSON 已保存成功，不影响）: {luaException.Message}");
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
            m_StatusBar.Refresh(m_Zoom, m_MousePos, m_TileX, m_TileY, m_MapSavePath);
        }

        /// <summary>
        /// 获取地图保存路径
        /// </summary>
        public string GetMapSavePath()
        {
            return m_MapSavePath;
        }

        /// <summary>
        /// 加载关卡地图（每关一张独立地图，取消分区）
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="levelId">关卡ID</param>
        /// <param name="prefabPath">地图预制体路径</param>
        public void LoadLevelMap(string chapterId, string levelId, string prefabPath = "")
        {
            Debug.Log($"加载关卡地图: 章节={chapterId}, 关卡={levelId}, 预制体路径: {prefabPath}");
            // 加载章节配置表
            LoadChapterConfig();
            var chapterData = m_ChapterDataList.FirstOrDefault(c => c.ID == chapterId);
            if (chapterData != null)
            {
                // 缓存当前章节上下文
                m_CurrentChapterId = chapterId;
                m_CurrentChapterData = chapterData;
                // 场景名：优先用章节标题+关卡号
                string sceneName = !string.IsNullOrEmpty(chapterData.ChapterTitle)
                    ? $"{chapterData.ChapterTitle}-关卡{levelId}"
                    : $"关卡{levelId}";
                if (m_SceneNameField != null)
                    m_SceneNameField.SetValueWithoutNotify(sceneName);
                if (m_SceneIdField != null)
                    m_SceneIdField.SetValueWithoutNotify(levelId);
            }
            else
            {
                m_CurrentChapterId = chapterId;
                m_CurrentChapterData = null;
                if (m_SceneNameField != null)
                    m_SceneNameField.SetValueWithoutNotify($"关卡{levelId}");
                if (m_SceneIdField != null)
                    m_SceneIdField.SetValueWithoutNotify(levelId);
            }

            // 记录当前关卡ID
            m_CurrentLevelId = levelId;

            // 加载主关卡配置表（背景图）
            LoadMainLevelsConfig();

            // 关卡预制体路径
            string levelPrefabPath = $"{MapToolConstants.MapSaveDir}/Map_{chapterId}_{levelId}.prefab";
            // 模板更新检测
            bool needClone = false;
            var existingLevelPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(levelPrefabPath);
            if (existingLevelPrefab == null)
            {
                needClone = true;
            }
            else if (!string.IsNullOrEmpty(prefabPath))
            {
                string templateFullPath = System.IO.Path.Combine(Application.dataPath, prefabPath.Replace("Assets/", ""));
                string levelFullPath = System.IO.Path.Combine(Application.dataPath, levelPrefabPath.Replace("Assets/", ""));
                if (System.IO.File.Exists(templateFullPath) && System.IO.File.Exists(levelFullPath))
                {
                    DateTime templateTime = System.IO.File.GetLastWriteTime(templateFullPath);
                    DateTime levelTime = System.IO.File.GetLastWriteTime(levelFullPath);
                    if (templateTime > levelTime)
                    {
                        needClone = true;
                        Debug.Log($"模板预制体已更新（模板: {templateTime:HH:mm:ss} > 关卡: {levelTime:HH:mm:ss}），重新克隆");
                    }
                }
            }
            if (needClone && !string.IsNullOrEmpty(prefabPath))
            {
                Debug.Log($"从模板克隆: {prefabPath} -> {levelPrefabPath}");
                string dir = System.IO.Path.GetDirectoryName(levelPrefabPath).Replace("\\", "/");
                if (!UnityEditor.AssetDatabase.IsValidFolder(dir))
                {
                    if (!UnityEditor.AssetDatabase.IsValidFolder(MapToolConstants.GameDataDir))
                        UnityEditor.AssetDatabase.CreateFolder("Assets", "GameData");
                    UnityEditor.AssetDatabase.CreateFolder(MapToolConstants.GameDataDir, "Maps");
                }
                bool copyResult = UnityEditor.AssetDatabase.CopyAsset(prefabPath, levelPrefabPath);
                Debug.Log($"克隆结果: {copyResult}");
                UnityEditor.AssetDatabase.Refresh();
            }
            // 加载关卡预制体
            m_MapPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(levelPrefabPath);
            Debug.Log($"加载关卡预制体: {levelPrefabPath}, 结果: {m_MapPrefab != null}");
            if (m_MapPrefab != null)
            {
                Debug.Log($"加载地图预制体成功: {m_MapPrefab.name}");
                if (m_MapPrefabField != null)
                {
                    m_MapPrefabField.value = m_MapPrefab;
                    Debug.Log($"已设置 ObjectField 值: {m_MapPrefab.name}");
                }
            }
            else
            {
                Debug.LogWarning("地图预制体加载失败，m_MapPrefab 为 null");
            }

            // 【取消分区】JSON 保存路径：按 章节_关卡 命名
            m_MapSavePath = $"{MapToolConstants.MapJsonSaveDir}/Map_{chapterId}_{levelId}.json";

            // 地图尺寸：默认 2048x2048，从 JSON 加载后会自动覆盖
            m_MapWidth = MapToolConstants.LevelPixelSize;
            m_MapHeight = MapToolConstants.LevelPixelSize;
            m_ConfigMapWidth = m_MapWidth;
            m_ConfigMapHeight = m_MapHeight;

            // 重置偏移和缩放
            m_MapOffset = Vector2.zero;
            m_Zoom = 1.0f;
            m_StatusBar.Refresh(m_Zoom, m_MousePos, m_TileX, m_TileY, m_MapSavePath);

            // 加载已保存的地图配置
            LoadMapConfig();

            // 同步宽高到输入框显示
            if (m_MapWidth <= 0) m_MapWidth = MapToolConstants.LevelPixelSize;
            if (m_MapHeight <= 0) m_MapHeight = MapToolConstants.LevelPixelSize;
            m_ConfigMapWidth = m_MapWidth;
            m_ConfigMapHeight = m_MapHeight;
            if (m_WidthField != null) m_WidthField.SetValueWithoutNotify(m_MapWidth.ToString());
            if (m_HeightField != null) m_HeightField.SetValueWithoutNotify(m_MapHeight.ToString());

            // 实例化到场景
            if (m_MapPrefab != null)
            {
                InstantiateMapInScene();
            }
        }

        /// <summary>
        /// 在场景中实例化 2DSceneRoot，地图预制体挂载到 SceneLauncher 下
        /// 【取消分区】单关卡=单张地图，不再按分区排列 ZoneItem
        /// </summary>
        private void InstantiateMapInScene()
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

            // 4. 查找 SceneLauncher 节点
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

            // 6. 查找 ZoneRoot
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

            // 记录 ZoneRoot 引用
            m_ZoneRootInstance = zoneRoot;

            // 【取消分区】禁用 GameMainMap 自带的默认背景 Image 组件
            foreach (Transform child in mapInstance.transform)
            {
                var img = child.GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                {
                    img.enabled = false;
                    Debug.Log($"[地图背景] 已禁用默认 Image: {child.name}");
                    break;
                }
            }

            // 7. 清理 ZoneRoot 下旧实例
            for (int i = zoneRoot.childCount - 1; i >= 0; i--)
            {
                var child = zoneRoot.GetChild(i);
                if (child.name.StartsWith("ZoneItem"))
                {
                    UnityEngine.Object.DestroyImmediate(child.gameObject);
                }
            }

            // 8. 加载并实例化 ZoneItem.prefab（单张地图）
            string zoneItemPath = "Assets/Res/Prefabs/Map/ZoneItem.prefab";
            var zoneItemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(zoneItemPath);
            if (zoneItemPrefab == null)
            {
                Debug.LogWarning($"未找到 ZoneItem.prefab: {zoneItemPath}");
                return;
            }
            string levelId = m_CurrentLevelId;
            var zoneItem = (GameObject)PrefabUtility.InstantiatePrefab(zoneItemPrefab, zoneRoot);
            zoneItem.name = $"ZoneItem_{levelId}";

            // ZoneItem 尺寸 = 整图尺寸
            var zrt = zoneItem.GetComponent<RectTransform>();
            if (zrt != null)
                zrt.sizeDelta = new Vector2(m_MapWidth, m_MapHeight);

            // 9. 设置 ZoneRoot 尺寸 = 整图尺寸
            zoneRoot.sizeDelta = new Vector2(m_MapWidth, m_MapHeight);
            var gridLayout = zoneRoot.GetComponent<UnityEngine.UI.GridLayoutGroup>();
            if (gridLayout != null)
                gridLayout.cellSize = new Vector2(m_MapWidth, m_MapHeight);

            // 10. 同步关卡背景图到 ZoneItem 的 Image 组件
            Texture bgTex = LoadRegionBackground(levelId);
            if (bgTex != null)
            {
                var img = zoneItem.GetComponentInChildren<UnityEngine.UI.Image>();
                if (img != null)
                {
                    Texture2D tex2d = bgTex as Texture2D;
                    if (tex2d != null)
                    {
                        img.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0.5f, 0.5f));
                        img.enabled = true;
                        Debug.Log($"[关卡背景] {levelId} 赋值成功: {bgTex.name}");
                    }
                }
                else
                {
                    Debug.LogWarning($"[关卡背景] {levelId} 找到纹理但 ZoneItem 内无 Image 组件");
                }
            }
            else
            {
                string bgName = m_LevelBgDict.TryGetValue(levelId, out string bn) ? bn : "<未配置>";
                Debug.LogWarning($"[关卡背景] {levelId} 加载失败 bgName={bgName}");
            }

            // 同步摆放物到场景（挂到 ZoneItem 的 Root 下）
            SyncPlacedObjectsToScene(zoneRoot);

            // 自动开启 GridManager 调试网格
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

            Debug.Log($"场景地图实例化完成: {m_MapWidth}x{m_MapHeight}（关卡 {levelId}）");
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
        /// 将编辑器中的摆放物同步到场景 ZoneItem 下
        /// </summary>
        /// <param name="zoneRoot">ZoneRoot 节点</param>
        private void SyncPlacedObjectsToScene(RectTransform zoneRoot)
        {
            if (zoneRoot == null) return;

            // 收集第一个 ZoneItem
            Transform zoneItem = null;
            for (int i = 0; i < zoneRoot.childCount; i++)
            {
                var child = zoneRoot.GetChild(i);
                if (child.name.StartsWith("ZoneItem_"))
                {
                    zoneItem = child;
                    break;
                }
            }
            if (zoneItem == null) return;

            // 清理旧摆放物（清理 ZoneItem 及其 Root 下的物体）
            for (int i = zoneItem.childCount - 1; i >= 0; i--)
            {
                var c = zoneItem.GetChild(i);
                if (c.name.StartsWith("Obj_") || c.name.StartsWith("Char_")
                    || c.name.StartsWith("Path_") || c.name.StartsWith("PathLine_"))
                    UnityEngine.Object.DestroyImmediate(c.gameObject);
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

            if (m_PlacedObjects == null || m_PlacedObjects.Count == 0) return;

            float gridSize = MapToolConstants.GridSize;

            // 所有物体挂到 ZoneItem 的 Root 下
            Transform rootNode = FindChildRecursive(zoneItem, "Root");
            RectTransform parent = rootNode != null ? rootNode as RectTransform : zoneItem as RectTransform;

            foreach (var obj in m_PlacedObjects)
            {
                var go = new GameObject();
                go.transform.SetParent(parent, false);
                go.name = obj.Type == MapObjectType.Character ? $"Char_{obj.Name}" : $"Obj_{obj.Name}";

                var rt = go.AddComponent<RectTransform>();
                // 和编辑器一致：左上角原点，Y 向下
                rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 1);
                rt.anchoredPosition = new Vector2(obj.Position.x, -obj.Position.y);
                rt.sizeDelta = new Vector2(obj.Width * gridSize, obj.Height * gridSize);

                var rawImg = go.AddComponent<UnityEngine.UI.RawImage>();
                rawImg.texture = obj.PreviewTexture as Texture2D;
                rawImg.color = obj.Type == MapObjectType.Character ? new Color(1,1,0,0.8f)
                             : obj.Type == MapObjectType.Building ? new Color(0,1,1,0.6f)
                             : new Color(0,1,0,0.6f);
            }
            Debug.Log($"已同步 {m_PlacedObjects.Count} 个摆放物到场景");
            SyncPathPointsToScene(zoneRoot);
        }

        /// <summary>
        /// 将全局路径点和物体路径点同步到场景：蓝色圆点 + 连线
        /// </summary>
        private void SyncPathPointsToScene(RectTransform zoneRoot)
        {
            // 收集第一个 ZoneItem
            Transform zoneItem = null;
            for (int i = 0; i < zoneRoot.childCount; i++)
            {
                var child = zoneRoot.GetChild(i);
                if (child.name.StartsWith("ZoneItem_"))
                {
                    zoneItem = child;
                    break;
                }
            }
            if (zoneItem == null) return;

            // 清理旧路径点标记（清理 ZoneItem 及其 Root 下的路径点）
            for (int i = zoneItem.childCount - 1; i >= 0; i--)
            {
                var c = zoneItem.GetChild(i);
                if (c.name.StartsWith("Path_") || c.name.StartsWith("PathLine_"))
                    UnityEngine.Object.DestroyImmediate(c.gameObject);
                if (c.name == "Root")
                {
                    for (int k = c.childCount - 1; k >= 0; k--)
                    {
                        var cc = c.GetChild(k);
                        if (cc.name.StartsWith("Path_") || cc.name.StartsWith("PathLine_"))
                            UnityEngine.Object.DestroyImmediate(cc.gameObject);
                    }
                }
            }

            // 路径点挂到 ZoneItem 的 Root 下
            RectTransform GetZoneRootAt(Vector2 globalPos, out Vector2 localPos)
            {
                localPos = globalPos;
                Transform rootNode = FindChildRecursive(zoneItem, "Root");
                return rootNode != null ? rootNode as RectTransform : zoneItem as RectTransform;
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