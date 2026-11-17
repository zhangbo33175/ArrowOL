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
using Spine.Unity;
using Honor.Editor.MapTool.PsdImport;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 场景编辑模块 - 公有接口（初始化/保存/选择/显隐）
    /// </summary>
    public partial class SceneEditModule : IMapEditModule
    {
        /// <summary>当前选中的章节ID（供PSD导入模块等外部模块读取）</summary>
        public string CurrentChapterId => m_CurrentChapterId;

        /// <summary>当前选中的关卡ID（供PSD导入模块等外部模块读取）</summary>
        public string CurrentLevelId => m_CurrentLevelId;

        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="root">根节点</param>
        public void Init(VisualElement root)
        {
            m_Root = root;
            // 加载所有类型的配置表（建筑/人物/物件/猫猫），而非旧的默认模板表
            ObjectTemplateManager.Instance.LoadAllConfigTables();

            // 初始化 PSD/PSB 导入模块
            m_PsdImportModule = new PsdImportModule(this);
            m_PsdImportModule.OnMapSizeSet = (w, h) => { m_MapWidth = w; m_MapHeight = h; };
            m_PsdImportModule.OnBackgroundSet = (path) =>
            {
                if (!string.IsNullOrEmpty(m_CurrentLevelId))
                    m_LevelBgDict[m_CurrentLevelId] = path;
            };
            m_PsdImportModule.OnObjectCreated = (obj) => { m_PlacedObjects.Add(obj); };
            m_PsdImportModule.OnRefreshScene = RefreshSceneObjects;

            FindElements();
            BindEvents();
        }

        private MapConfigData AssembleMapConfigData()
        {
            MapConfigData data = new MapConfigData();

            ApplyMapMetaToData(data);
            CollectPlacedObjectsIntoData(data);
            ApplyGridMetaToData(data);
            ApplyTrafficToData(data);
            ApplyChapterToData(data);

            return data;
        }

        /// <summary>
        /// 落盘地图基础信息：场景标识 / 宽高 / 背景图
        /// </summary>
        /// <param name="data">目标配置数据</param>
        private void ApplyMapMetaToData(MapConfigData data)
        {
            data.sceneId = GetFieldValue(m_SceneIdField, "1");
            data.sceneName = GetFieldValue(m_SceneNameField, "关卡1");
            data.width = m_MapWidth > 0 ? m_MapWidth : ParseInt(GetFieldValue(m_WidthField, m_ConfigMapWidth.ToString()), m_ConfigMapWidth);
            data.height = m_MapHeight > 0 ? m_MapHeight : ParseInt(GetFieldValue(m_HeightField, m_ConfigMapHeight.ToString()), m_ConfigMapHeight);

            data.bgName = m_LevelBgDict != null && m_LevelBgDict.TryGetValue(m_CurrentLevelId, out string bg) ? bg : "";
        }

        /// <summary>
        /// 落盘摆放物：按类型分流到 characters / buildings / objects 三张表
        /// </summary>
        /// <param name="data">目标配置数据</param>
        private void CollectPlacedObjectsIntoData(MapConfigData data)
        {
            // 所有物体直接挂到地图下（单关卡模式）
            foreach (var obj in m_PlacedObjects)
            {
                // ---TODO 交通规则：载具/行人属于动态链路，强制归入 characters 表落盘与导出，
                // 避免被模板初始类型（Prop）带进静态物件表导致交通字段丢失
                if (IsTrafficRoleActive(obj) && obj.Type != MapObjectType.Building)
                    obj.Type = MapObjectType.Character;

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
        }

        /// <summary>
        /// 落盘网格信息：格尺寸 / 基线步长 / 绘制形状 / 区域类型 / 网格行列 / 占用格
        /// </summary>
        /// <param name="data">目标配置数据</param>
        private void ApplyGridMetaToData(MapConfigData data)
        {
            data.gridSize = MapToolConstants.GridSize;
            // 基线步长：落盘当前编辑器配置（地图数据记录自身排序契约，发布 Lua 同源导出）
            data.baselineStep = MapToolConstants.LayerBaselineStep;
            // 绘制形状 / 区域类型：落盘当前编辑器配置（发布 Lua 同源导出）
            data.drawShape = m_DrawShape;
            data.regionType = m_RegionType;
            data.gridCols = MapToolEditorUtils.GridCols(data.width);
            data.gridRows = MapToolEditorUtils.GridRows(data.height);
            data.occupiedCells = BuildOccupiedCells(MapToolEditorUtils.CollectOccupiedCells(m_PlacedObjects));
        }

        /// <summary>
        /// 占用格编码键 → 网格格子数据列表
        /// </summary>
        /// <param name="occupiedKeys">编码后的占用格键集合</param>
        /// <returns>格子数据列表</returns>
        private static List<MapGridCellData> BuildOccupiedCells(IEnumerable<long> occupiedKeys)
        {
            var cells = new List<MapGridCellData>();
            foreach (var key in occupiedKeys)
            {
                cells.Add(new MapGridCellData
                {
                    x = (int)(key % MapToolConstants.CellEncodeMultiplier),
                    y = (int)(key / MapToolConstants.CellEncodeMultiplier)
                });
            }
            return cells;
        }

        /// <summary>
        /// 落盘交通规则数据（红绿灯 / 斑马线 / 区域 / 人行道格 / 道路格，对称组装）
        /// </summary>
        /// <param name="data">目标配置数据</param>
        private void ApplyTrafficToData(MapConfigData data)
        {
            data.trafficLights = new List<MapTrafficLightData>(m_TrafficLights);
            data.crosswalks = new List<MapCrosswalkData>(m_Crosswalks);
            data.regions = new List<MapRegionData>(m_Regions);
            data.sidewalkCells = new List<MapGridCellData>(m_SidewalkCells);
            data.roadCells = new List<MapGridCellData>(m_RoadCells);
        }

        /// <summary>
        /// 落盘章节信息（章节 ID 缺省回退场景 ID，章节名缺省回退场景名）
        /// </summary>
        /// <param name="data">目标配置数据</param>
        private void ApplyChapterToData(MapConfigData data)
        {
            data.chapterId = !string.IsNullOrEmpty(m_CurrentChapterId) ? m_CurrentChapterId : data.sceneId;
            data.chapterName = m_CurrentChapterData != null && !string.IsNullOrEmpty(m_CurrentChapterData.ChapterTitle)
                ? m_CurrentChapterData.ChapterTitle
                : data.sceneName;
        }

        /// <summary>
        /// 保存编辑器工作态配置到 JSON（仅落盘编辑信息，不导出发布 Lua 表）
        /// 用于 PSD 导入后自动落盘；下次选中关卡由 LoadMapConfig 直接还原，无需重复导入 PSD。
        /// </summary>
        public void SaveWorkingConfig()
        {
            if (string.IsNullOrEmpty(m_CurrentLevelId))
            {
                MapToolLog.Warn("没有关卡数据，无法保存工作态配置（请先选择关卡加载地图）");
                return;
            }

            MapConfigData data = AssembleMapConfigData();

            // 写入 JSON 文件（委托给 MapConfigRepository，本类只负责组装数据）
            if (!MapConfigRepository.SaveToJson(data, m_MapSavePath))
                return;

            MapToolLog.Info($"工作态地图配置已保存: {m_MapSavePath}（摆放物 {m_PlacedObjects.Count}）");
        }

        /// <summary>
        /// 清空当前关卡的编辑器工作态数据（重新导入 PSD 前调用，覆盖旧摆放物与背景）
        /// </summary>
        public void ClearWorkingObjects()
        {
            m_PlacedObjects.Clear();
            ClearTrafficWorkingData();
            m_EditTarget = null;
            if (!string.IsNullOrEmpty(m_CurrentLevelId) && m_LevelBgDict != null
                && m_LevelBgDict.ContainsKey(m_CurrentLevelId))
            {
                m_LevelBgDict.Remove(m_CurrentLevelId);
            }
            m_ObjectEditPanel.SetVisible(false);
        }

        /// <summary>
        /// 清空交通规则工作态数据（红绿灯 / 斑马线 / 区域 / 人行道格 / 道路格）
        /// 与摆放物清理同步调用，避免重建地图后残留上一关卡交通配置
        /// </summary>
        public void ClearTrafficWorkingData()
        {
            m_TrafficLights.Clear();
            m_Crosswalks.Clear();
            m_Regions.Clear();
            m_SidewalkCells.Clear();
            m_RoadCells.Clear();
        }

        /// <summary>
        /// 保存地图配置。
        /// </summary>
        /// <param name="exportLua">
        /// true=发布：落盘 JSON 并导出发布 Lua 表；false=保存：仅落盘 JSON 工作态，不导出 Lua 文件。
        /// </param>
        public void SaveMapConfig(bool exportLua)
        {
            if (string.IsNullOrEmpty(m_CurrentLevelId))
            {
                MapToolLog.Warn("没有关卡数据，无法保存地图配置（请先选择关卡加载地图）");
                return;
            }

            // 保存/发布前先把场景实例的模板级共享属性回写内存模板，保证元件导出取到最新数据
            PushInstancesToTemplates();

            MapConfigData data = AssembleMapConfigData();

            // 写入 JSON 文件（委托给 MapConfigRepository，本类只负责组装数据）
            if (!MapConfigRepository.SaveToJson(data, m_MapSavePath))
                return;

            // 仅发布时导出 Lua 表；普通保存只更新本地 JSON 工作态，不改动 Lua 文件
            if (!exportLua) return;

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
                MapToolLog.Info($"Lua 数据已导出: 共 {luaFiles.Count} 个文件\n{string.Join("\n", luaFiles)}");
            }
            catch (System.Exception luaException)
            {
                MapToolLog.Error($"Lua 数据导出失败（JSON 已保存成功，不影响）: {luaException.Message}");
            }
        }

        /// <summary>
        /// 显示模块
        /// </summary>
        public void Show()
        {
            m_Root.SetVisible(true);
        }

        /// <summary>
        /// 隐藏模块
        /// </summary>
        public void Hide()
        {
            m_Root.SetVisible(false);
        }

        /// <summary>
        /// 加载关卡地图：章节上下文 → 关卡预制体 → 视图状态 → 地图配置 → 场景实例化
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="levelId">关卡ID</param>
        /// <param name="prefabPath">地图预制体路径</param>
        public void LoadLevelMap(string chapterId, string levelId, string prefabPath = "")
        {
            MapToolLog.Info($"加载关卡地图: 章节={chapterId}, 关卡={levelId}, 预制体路径: {prefabPath}");

            // 加载章节配置表并应用章节/关卡上下文
            ApplyChapterContext(chapterId, levelId);

            // 加载主关卡配置表（背景图）
            LoadMainLevelsConfig();

            // 解析（必要时从模板克隆）关卡预制体
            string levelPrefabPath = LevelPrefabPath(chapterId, levelId);
            m_MapPrefab = ResolveLevelPrefab(levelPrefabPath, prefabPath);
            if (m_MapPrefab != null)
            {
                MapToolLog.Info($"加载地图预制体成功: {m_MapPrefab.name}");
                m_MapPrefabField.SetValueSafe(m_MapPrefab);
                MapToolLog.Info($"已设置 ObjectField 值: {m_MapPrefab.name}");
            }
            else
            {
                MapToolLog.Warn("地图预制体加载失败，m_MapPrefab 为 null");
            }

            // 【取消分区】JSON 保存路径：按 章节m关卡 命名
            m_MapSavePath = $"{MapToolConstants.MapJsonSaveDir}/Map_{chapterId}m{levelId}.json";

            // 重置尺寸/偏移/缩放等视图状态
            ResetMapViewState();

            // 加载已保存的地图配置
            LoadMapConfig();

            // 同步宽高到输入框显示
            SyncSizeFields();

            // 实例化到场景
            if (m_MapPrefab != null)
            {
                InstantiateMapInScene();
            }
        }

        /// <summary>
        /// 加载章节配置并写入章节/关卡上下文与场景名、场景ID字段
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="levelId">关卡ID</param>
        private void ApplyChapterContext(string chapterId, string levelId)
        {
            LoadChapterConfig();
            var chapterData = m_ChapterDataList.FirstOrDefault(c => c.ID == chapterId);

            m_CurrentChapterId = chapterId;
            m_CurrentChapterData = chapterData;

            // 场景名：优先用章节标题+关卡号
            string sceneName = chapterData != null && !string.IsNullOrEmpty(chapterData.ChapterTitle)
                ? $"{chapterData.ChapterTitle}-关卡{levelId}"
                : $"关卡{levelId}";
            m_SceneNameField.SetValueSafe(sceneName);
            m_SceneIdField.SetValueSafe(levelId);

            // 记录当前关卡ID
            m_CurrentLevelId = levelId;
        }

        /// <summary>
        /// 关卡预制体的资产路径（章节m关卡 命名）
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="levelId">关卡ID</param>
        /// <returns>资产相对路径</returns>
        private static string LevelPrefabPath(string chapterId, string levelId)
        {
            return $"{MapToolConstants.MapSaveDir}/Map_{chapterId}m{levelId}.prefab";
        }

        /// <summary>
        /// 加载关卡预制体；缺失或模板已更新时先复制模板再加载
        /// </summary>
        /// <param name="levelPrefabPath">关卡预制体资产路径</param>
        /// <param name="templatePrefabPath">模板预制体资产路径（可空）</param>
        /// <returns>关卡预制体，失败返回 null</returns>
        private static GameObject ResolveLevelPrefab(string levelPrefabPath, string templatePrefabPath)
        {
            if (NeedsCloneFromTemplate(levelPrefabPath, templatePrefabPath))
            {
                MapToolLog.Info($"从模板克隆: {templatePrefabPath} -> {levelPrefabPath}");
                EnsureMapsFolder(levelPrefabPath);
                bool copyResult = UnityEditor.AssetDatabase.CopyAsset(templatePrefabPath, levelPrefabPath);
                MapToolLog.Info($"克隆结果: {copyResult}");
                UnityEditor.AssetDatabase.Refresh();
            }

            var levelPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(levelPrefabPath);
            MapToolLog.Info($"加载关卡预制体: {levelPrefabPath}, 结果: {levelPrefab != null}");
            return levelPrefab;
        }

        /// <summary>
        /// 判断是否需要从模板克隆：关卡预制体缺失，或模板文件比关卡更新
        /// </summary>
        /// <param name="levelPrefabPath">关卡预制体资产路径</param>
        /// <param name="templatePrefabPath">模板预制体资产路径（可空）</param>
        /// <returns>需要克隆返回 true</returns>
        private static bool NeedsCloneFromTemplate(string levelPrefabPath, string templatePrefabPath)
        {
            var existingLevelPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(levelPrefabPath);
            if (existingLevelPrefab == null) return true;
            if (string.IsNullOrEmpty(templatePrefabPath)) return false;

            string templateFullPath = MapToolPathHelper.ToAbsolutePath(templatePrefabPath);
            string levelFullPath = MapToolPathHelper.ToAbsolutePath(levelPrefabPath);
            if (!System.IO.File.Exists(templateFullPath) || !System.IO.File.Exists(levelFullPath)) return false;

            DateTime templateTime = System.IO.File.GetLastWriteTime(templateFullPath);
            DateTime levelTime = System.IO.File.GetLastWriteTime(levelFullPath);
            if (templateTime <= levelTime) return false;

            MapToolLog.Info($"模板预制体已更新（模板: {templateTime:HH:mm:ss} > 关卡: {levelTime:HH:mm:ss}），重新克隆");
            return true;
        }

        /// <summary>
        /// 确保关卡预制体所在目录存在（缺 GameData/Maps 时逐级创建）
        /// </summary>
        /// <param name="levelPrefabPath">关卡预制体资产路径</param>
        private static void EnsureMapsFolder(string levelPrefabPath)
        {
            string dir = System.IO.Path.GetDirectoryName(levelPrefabPath).Replace("\\", "/");
            if (UnityEditor.AssetDatabase.IsValidFolder(dir)) return;

            if (!UnityEditor.AssetDatabase.IsValidFolder(MapToolConstants.GameDataDir))
                UnityEditor.AssetDatabase.CreateFolder(MapToolPathHelper.AssetsFolderName, MapToolConstants.GameDataFolderName);
            UnityEditor.AssetDatabase.CreateFolder(MapToolConstants.GameDataDir, "Maps");
        }

        /// <summary>
        /// 重置地图视图状态：尺寸回到默认整图像素，偏移与缩放复位，刷新状态栏
        /// </summary>
        private void ResetMapViewState()
        {
            // 地图尺寸：默认 2048x2048，从 JSON 加载后会自动覆盖
            m_MapWidth = MapToolConstants.LevelPixelSize;
            m_MapHeight = MapToolConstants.LevelPixelSize;
            SyncConfigMapSize();

            m_MapOffset = Vector2.zero;
            m_Zoom = 1.0f;
            m_StatusBar.Refresh(m_Zoom, m_MousePos, m_TileX, m_TileY, OccupiedCellCount, GridCols, GridRows, m_MapSavePath);
        }

        /// <summary>
        /// 把地图宽高同步到输入框显示（非法值回退默认整图尺寸）
        /// </summary>
        private void SyncSizeFields()
        {
            if (m_MapWidth <= 0) m_MapWidth = MapToolConstants.LevelPixelSize;
            if (m_MapHeight <= 0) m_MapHeight = MapToolConstants.LevelPixelSize;
            SyncConfigMapSize();

            m_WidthField.SetValueSafe(m_MapWidth.ToString());
            m_HeightField.SetValueSafe(m_MapHeight.ToString());
        }

        /// <summary>
        /// 在地图中实例化场景骨架：2DSceneRoot → SceneLauncher → GameMainMap → ZoneRoot/ZoneItem，
        /// 随后同步关卡背景与摆放物。各步骤拆为独立方法，任一步失败即中止。
        /// </summary>
        private void InstantiateMapInScene()
        {
            // 1-3. 重建场景根（清理旧实例 + 实例化 2DSceneRoot）
            GameObject sceneRootInstance = CreateSceneRoot();
            if (sceneRootInstance == null) return;

            // 4. 查找 SceneLauncher 节点
            Transform sceneLauncher = FindSceneLauncher(sceneRootInstance.transform);
            if (sceneLauncher == null)
            {
                MapToolLog.Warn("2DSceneRoot 中未找到 SceneLauncher 节点");
                return;
            }

            // 5-9. 实例化地图并配置 ZoneRoot / ZoneItem 层级
            GameObject mapInstance = CreateMapInstance(sceneLauncher, out RectTransform zoneRoot);
            if (mapInstance == null) return;

            m_ZoneRootInstance = zoneRoot;

            GameObject zoneItem = ConfigureZoneHierarchy(mapInstance, zoneRoot);
            if (zoneItem == null) return;

            // 10. 同步关卡背景图到 ZoneItem 的 Image 组件
            ApplyZoneItemBackground(zoneItem, m_CurrentLevelId);

            // 同步摆放物到场景（挂到 ZoneItem 的 Root 下）
            SyncPlacedObjectsToScene(zoneRoot);

            // 自动开启 GridManager 调试网格
            EnableGridDebug(mapInstance);

            MapToolLog.Info($"场景地图实例化完成: {m_MapWidth}x{m_MapHeight}（关卡 {m_CurrentLevelId}）");
            Selection.activeGameObject = mapInstance;
        }

        /// <summary>
        /// 清理旧场景根并实例化 2DSceneRoot.prefab 到 Launcher 下
        /// </summary>
        /// <returns>场景根实例，Launcher 或预制体缺失时返回 null</returns>
        private GameObject CreateSceneRoot()
        {
            var oldSceneRoot = GameObject.Find(MapSceneNodeNames.SceneRoot);
            if (oldSceneRoot != null) UnityEngine.Object.DestroyImmediate(oldSceneRoot);

            // 场景重建时清空资源缓存，避免引用已替换/删除的旧资产
            m_Assets.ClearCaches();

            var launcher = GameObject.Find(MapSceneNodeNames.Launcher);
            if (launcher == null)
            {
                MapToolLog.Warn("场景中未找到 Launcher 节点，请先打开包含 GameApp/GameMain/Launcher 的场景");
                return null;
            }

            var sceneRootPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(MapToolConstants.SceneRootPrefabPath);
            if (sceneRootPrefab == null)
            {
                MapToolLog.Warn($"未找到 2DSceneRoot.prefab: {MapToolConstants.SceneRootPrefabPath}");
                return null;
            }

            var sceneRootInstance = (GameObject)PrefabUtility.InstantiatePrefab(sceneRootPrefab, launcher.transform);
            sceneRootInstance.name = MapSceneNodeNames.SceneRoot;
            Undo.RegisterCreatedObjectUndo(sceneRootInstance, "Instantiate SceneRoot");
            return sceneRootInstance;
        }

        /// <summary>
        /// 在场景根下查找 SceneLauncher 节点（2DSceneLayers/SceneLauncher）
        /// </summary>
        /// <param name="sceneRoot">场景根 Transform</param>
        /// <returns>SceneLauncher 节点，缺失返回 null</returns>
        private static Transform FindSceneLauncher(Transform sceneRoot)
        {
            foreach (Transform child in sceneRoot)
            {
                if (child.name != MapSceneNodeNames.SceneLayers) continue;

                foreach (Transform layer in child)
                {
                    if (layer.name == MapSceneNodeNames.SceneLauncher)
                        return layer;
                }
            }
            return null;
        }

        /// <summary>
        /// 实例化地图预制体到 SceneLauncher 下，并定位 ZoneRoot
        /// </summary>
        /// <param name="sceneLauncher">地图挂载父节点</param>
        /// <param name="zoneRoot">输出：ZoneRoot 节点</param>
        /// <returns>地图实例，ZoneRoot 缺失时返回 null</returns>
        private GameObject CreateMapInstance(Transform sceneLauncher, out RectTransform zoneRoot)
        {
            zoneRoot = null;

            var mapInstance = (GameObject)PrefabUtility.InstantiatePrefab(m_MapPrefab, sceneLauncher);
            mapInstance.name = MapSceneNodeNames.MapInstance;
            Undo.RegisterCreatedObjectUndo(mapInstance, "Instantiate Map");

            var allRects = mapInstance.GetComponentsInChildren<RectTransform>(true);
            foreach (var rt in allRects)
            {
                if (rt.name == MapSceneNodeNames.ZoneRoot)
                {
                    zoneRoot = rt;
                    break;
                }
            }

            if (zoneRoot == null)
            {
                MapToolLog.Warn($"GameMainMap 中未找到 ZoneRoot 节点，现有子节点: {string.Join(", ", allRects.Select(r => r.name))}");
                return null;
            }

            return mapInstance;
        }

        /// <summary>
        /// 配置 ZoneRoot / ZoneItem 层级：禁用默认背景、清理旧分区、实例化 ZoneItem 并统一整图尺寸
        /// </summary>
        /// <param name="mapInstance">地图实例</param>
        /// <param name="zoneRoot">ZoneRoot 节点</param>
        /// <returns>新建的 ZoneItem 实例，预制体缺失时返回 null</returns>
        private GameObject ConfigureZoneHierarchy(GameObject mapInstance, RectTransform zoneRoot)
        {
            // 【取消分区】禁用 GameMainMap 自带的默认背景 Image 组件
            foreach (Transform child in mapInstance.transform)
            {
                var img = child.GetComponent<UnityEngine.UI.Image>();
                if (img != null)
                {
                    img.enabled = false;
                    MapToolLog.Info($"[地图背景] 已禁用默认 Image: {child.name}");
                    break;
                }
            }

            // 清理 ZoneRoot 下旧实例
            for (int i = zoneRoot.childCount - 1; i >= 0; i--)
            {
                var child = zoneRoot.GetChild(i);
                if (child.name.StartsWith(MapSceneNodeNames.ZoneItemPrefix))
                {
                    UnityEngine.Object.DestroyImmediate(child.gameObject);
                }
            }

            // 加载并实例化 ZoneItem.prefab（单张地图）
            var zoneItemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(MapToolConstants.ZoneItemPrefabPath);
            if (zoneItemPrefab == null)
            {
                MapToolLog.Warn($"未找到 ZoneItem.prefab: {MapToolConstants.ZoneItemPrefabPath}");
                return null;
            }

            var zoneItem = (GameObject)PrefabUtility.InstantiatePrefab(zoneItemPrefab, zoneRoot);
            zoneItem.name = $"{MapSceneNodeNames.ZoneItemInstancePrefix}{m_CurrentLevelId}";

            // ZoneItem / ZoneRoot 尺寸 = 整图尺寸；分区布局基线同步
            ZoneItemSizes(zoneItem, zoneRoot);

            return zoneItem;
        }

        /// <summary>
        /// 应用 ZoneItem / ZoneRoot 的整图尺寸与 GridLayout 基线
        /// </summary>
        /// <param name="zoneItem">ZoneItem 实例</param>
        /// <param name="zoneRoot">ZoneRoot 节点</param>
        private void ZoneItemSizes(GameObject zoneItem, RectTransform zoneRoot)
        {
            var zrt = zoneItem.GetComponent<RectTransform>();
            if (zrt != null)
                zrt.sizeDelta = new Vector2(m_MapWidth, m_MapHeight);

            zoneRoot.sizeDelta = new Vector2(m_MapWidth, m_MapHeight);
            var gridLayout = zoneRoot.GetComponent<UnityEngine.UI.GridLayoutGroup>();
            if (gridLayout != null)
                gridLayout.cellSize = new Vector2(m_MapWidth, m_MapHeight);
        }

        /// <summary>
        /// 同步关卡背景图到 ZoneItem 的 Image 组件（Texture2D 现造 Sprite）
        /// </summary>
        /// <param name="zoneItem">ZoneItem 实例</param>
        /// <param name="levelId">关卡ID</param>
        private void ApplyZoneItemBackground(GameObject zoneItem, string levelId)
        {
            Texture bgTex = LoadRegionBackground(levelId);
            if (bgTex == null)
            {
                string bgName = m_LevelBgDict.TryGetValue(levelId, out string bn) ? bn : "<未配置>";
                MapToolLog.Warn($"[关卡背景] {levelId} 加载失败 bgName={bgName}");
                return;
            }

            var img = zoneItem.GetComponentInChildren<UnityEngine.UI.Image>();
            if (img == null)
            {
                MapToolLog.Warn($"[关卡背景] {levelId} 找到纹理但 ZoneItem 内无 Image 组件");
                return;
            }

            if (bgTex is Texture2D tex2d)
            {
                img.sprite = MapAssetLoader.CreateSpriteFromTexture(tex2d);
                img.enabled = true;
                MapToolLog.Info($"[关卡背景] {levelId} 赋值成功: {bgTex.name}");
            }
        }

        /// <summary>
        /// 开启地图实例上 GridManager 的调试网格
        /// </summary>
        /// <param name="mapInstance">地图实例</param>
        private static void EnableGridDebug(GameObject mapInstance)
        {
            var gridMgr = mapInstance.GetComponentInChildren<Honor.Runtime.GridManager>(true);
            if (gridMgr != null)
            {
                gridMgr.DrawDebugGrid = true;
                MapToolLog.Info($"[网格] GridManager 已开启，挂载在: {gridMgr.gameObject.name}");
            }
            else
            {
                MapToolLog.Warn("[网格] 未在地图实例中找到 GridManager 组件");
            }
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
        /// 在 ZoneRoot 下取第一个 ZoneItem 实例节点
        /// </summary>
        /// <param name="zoneRoot">ZoneRoot 节点</param>
        /// <returns>ZoneItem 节点，缺失返回 null</returns>
        private static Transform FindZoneItem(Transform zoneRoot)
        {
            if (zoneRoot == null) return null;
            for (int i = 0; i < zoneRoot.childCount; i++)
            {
                Transform child = zoneRoot.GetChild(i);
                if (child.name.StartsWith(MapSceneNodeNames.ZoneItemInstancePrefix))
                    return child;
            }
            return null;
        }

        /// <summary>
        /// 确保 Background 节点下存在铺满父级的 BG 图片节点（缺失时按统一锚点口径创建）
        /// </summary>
        /// <param name="backgroundNode">Background 层节点</param>
        /// <returns>BG 图片节点</returns>
        private Transform EnsureBackgroundImageNode(Transform backgroundNode)
        {
            Transform bgNode = FindChildRecursive(backgroundNode, MapSceneNodeNames.BackgroundImage);
            if (bgNode != null) return bgNode;

            bgNode = new GameObject(MapSceneNodeNames.BackgroundImage, typeof(RectTransform), typeof(UnityEngine.UI.Image)).transform;
            bgNode.SetParent(backgroundNode, false);
            var bgRt = bgNode as RectTransform;
            bgRt.anchorMin = Vector2.zero;
            bgRt.anchorMax = Vector2.one;
            bgRt.offsetMin = Vector2.zero;
            bgRt.offsetMax = Vector2.zero;
            return bgNode;
        }

        /// <summary>
        /// 应用关卡背景图：PSD 导入记录路径 → 章节三级目录自动查找（含 Texture2D 现造 Sprite 回退）
        /// </summary>
        /// <param name="bgImage">背景 Image 组件</param>
        /// <returns>是否成功设置背景</returns>
        private bool ApplyLevelBackground(UnityEngine.UI.Image bgImage)
        {
            if (bgImage == null) return false;

            // 第1级：从 m_LevelBgDict 取 PSD 导入时记录的路径
            if (m_LevelBgDict.TryGetValue(m_CurrentLevelId, out string bgPath) && !string.IsNullOrEmpty(bgPath))
            {
                var sprite = m_Assets.LoadSpriteOrCreate(bgPath);
                if (sprite != null)
                {
                    bgImage.sprite = sprite;
                    bgImage.color = Color.white;
                    MapToolLog.Info($"[SyncPlacedObjects] 背景图设置成功: {bgPath}");
                    return true;
                }
                MapToolLog.Warn($"[SyncPlacedObjects] 背景图路径无效，尝试自动查找: {bgPath}");
            }

            // 第2级：自动从当前章节 Background 三级目录查找 bg_*.png
            string foundBg = m_Assets.FindBackgroundImageByChapter(m_CurrentChapterId, m_CurrentLevelId);
            if (string.IsNullOrEmpty(foundBg)) return false;

            var foundSprite = m_Assets.LoadSpriteOrCreate(foundBg);
            if (foundSprite == null)
            {
                MapToolLog.Warn($"[SyncPlacedObjects] 背景图加载失败: {foundBg}");
                return false;
            }

            bgImage.sprite = foundSprite;
            bgImage.color = Color.white;
            MapToolLog.Info($"[SyncPlacedObjects] 自动查找背景图成功: {foundBg}");
            return true;
        }

        /// <summary>
        /// 将编辑器中的摆放物同步到场景 ZoneItem 下
        /// </summary>
        /// <param name="zoneRoot">ZoneRoot 节点</param>
        private void SyncPlacedObjectsToScene(RectTransform zoneRoot)
        {
            if (zoneRoot == null) return;

            // 收集第一个 ZoneItem
            Transform zoneItem = FindZoneItem(zoneRoot);
            if (zoneItem == null) return;

            // 清理旧摆放物
            ClearStaleSceneObjects(zoneItem);

            if (m_PlacedObjects == null || m_PlacedObjects.Count == 0) return;

            // 找到各个层级节点（ZoneItem > Root > CoordAdapter > 层节点）
            Transform coordAdapter = FindChildRecursive(zoneItem, MapSceneNodeNames.CoordAdapter);

            // 背景图 + 层级节点坐标系/尺寸标准化
            ApplySceneBackground(coordAdapter);
            StretchCoordAdapterLayers(coordAdapter);

            // 按显示档挂载摆放物实例
            InstantiatePlacedObjects(zoneItem, coordAdapter);

            // 调整层级节点顺序并按 YSort 校准层内渲染次序
            SortSceneLayers(zoneRoot, coordAdapter, MapLayerCatalog.GetLayerNodeNames());

            MapToolLog.Info($"已同步 {m_PlacedObjects.Count} 个摆放物到场景，已按 YSort 调整渲染顺序");
            SyncPathPointsToScene(zoneRoot);
        }

        /// <summary>
        /// 清理 ZoneItem 下所有旧摆放物节点（Obj_/Char_/Path_/PathLine_）
        /// 层级链为 ZoneItem > Root > CoordAdapter > 层节点 > 物件，仅清直接子物体会残留旧实例，多次刷新后叠加干扰层级排序。
        /// </summary>
        /// <param name="zoneItem">ZoneItem 节点</param>
        private static void ClearStaleSceneObjects(Transform zoneItem)
        {
            var staleObjects = zoneItem.GetComponentsInChildren<Transform>(true)
                .Where(t => t != zoneItem
                    && (t.name.StartsWith(MapSceneNodeNames.ObjectPrefix)
                        || t.name.StartsWith(MapSceneNodeNames.CharacterPrefix)
                        || t.name.StartsWith(MapSceneNodeNames.PathPointPrefix)
                        || t.name.StartsWith(MapSceneNodeNames.PathLinePrefix)))
                .ToList();

            foreach (var stale in staleObjects)
            {
                if (stale != null) UnityEngine.Object.DestroyImmediate(stale.gameObject);
            }
        }

        /// <summary>
        /// 把当前关卡背景图同步到 CoordAdapter/Background 下的 BG 子节点
        /// </summary>
        /// <param name="coordAdapter">坐标适配节点（可空，空则跳过）</param>
        private void ApplySceneBackground(Transform coordAdapter)
        {
            if (coordAdapter == null) return;

            var backgroundNode = FindChildRecursive(coordAdapter, MapSceneNodeNames.BackgroundNode);
            if (backgroundNode == null) return;

            Transform bgNode = EnsureBackgroundImageNode(backgroundNode);
            var bgImage = bgNode.GetComponent<UnityEngine.UI.Image>();
            if (bgImage == null) bgImage = bgNode.gameObject.AddComponent<UnityEngine.UI.Image>();

            if (!ApplyLevelBackground(bgImage))
            {
                MapToolLog.Warn($"[SyncPlacedObjects] 未找到背景图（章节: {m_CurrentChapterId}），BG 节点已创建但图片为空");
            }
        }

        /// <summary>
        /// 标准化 CoordAdapter 与各层级节点的锚点/轴心/偏移（铺满整图，左上角为原点）
        /// </summary>
        /// <param name="coordAdapter">坐标适配节点（可空，空则跳过）</param>
        private void StretchCoordAdapterLayers(Transform coordAdapter)
        {
            if (coordAdapter == null) return;

            var caRt = coordAdapter as RectTransform;
            if (caRt != null)
            {
                ApplyTopLeftFill(caRt);
            }

            // Background/Greenery/Road/Buildings/Props/Characters/Foreground/Bubbles
            foreach (var name in MapLayerCatalog.GetLayerNodeNames())
            {
                var layer = FindChildRecursive(coordAdapter, name) as RectTransform;
                if (layer != null)
                {
                    ApplyTopLeftFill(layer);
                }
            }
        }

        /// <summary>
        /// 设置「左上角为原点、铺满父级」的锚点与偏移
        /// </summary>
        /// <param name="rect">目标 RectTransform</param>
        private static void ApplyTopLeftFill(RectTransform rect)
        {
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 0);
            rect.pivot = new Vector2(0, 1);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        /// <summary>
        /// 批量实例化摆放物并应用视觉属性
        /// </summary>
        /// <param name="zoneItem">ZoneItem 节点（层级节点缺失时的兜底父级）</param>
        /// <param name="coordAdapter">坐标适配节点（可空）</param>
        private void InstantiatePlacedObjects(Transform zoneItem, Transform coordAdapter)
        {
            float gridSize = MapToolConstants.GridSize;

            foreach (var obj in m_PlacedObjects)
            {
                if (obj == null) continue;

                Transform parentNode = ResolveObjectParentNode(obj, coordAdapter, zoneItem);
                GameObject go = InstantiateObjectNode(obj, parentNode, gridSize);
                if (go == null) continue;

                ApplyObjectVisual(go, obj);
            }
        }

        /// <summary>
        /// 解析摆放物的物理挂载节点：归属层 LayerType 不变，挂载点跟随独立显示层档位
        /// </summary>
        /// <param name="obj">摆放物</param>
        /// <param name="coordAdapter">坐标适配节点（可空）</param>
        /// <param name="fallback">兜底父级</param>
        /// <returns>父节点 Transform</returns>
        private Transform ResolveObjectParentNode(PlacedObject obj, Transform coordAdapter, Transform fallback)
        {
            if (coordAdapter == null) return fallback;

            string ownerLayer = obj != null ? obj.LayerType : MapLayerCatalog.Prop;
            string effectiveLayer = MapLayerCatalog.ResolveDisplayLayer(ownerLayer, obj != null ? obj.DisplayLayer : null);
            Transform node = FindChildRecursive(coordAdapter, MapLayerCatalog.GetLayerDir(effectiveLayer));
            return node != null ? node : fallback;
        }

        /// <summary>
        /// 实例化物件节点：按类型取模板预制体，缺失时回退空物体 + SpriteRenderer，
        /// 并写入节点名、位置、尺寸、比例与角度（RectTransform 左上原点，Y 向下）。
        /// </summary>
        /// <param name="obj">摆放物数据</param>
        /// <param name="parentNode">挂载父节点（层节点 / ZoneItem 兜底）</param>
        /// <param name="gridSize">网格尺寸（无像素尺寸时的兜底换算）</param>
        /// <returns>场景节点实例</returns>
        private GameObject InstantiateObjectNode(PlacedObject obj, Transform parentNode, float gridSize)
        {
            GameObject templatePrefab = obj.Type switch
            {
                MapObjectType.Building => m_Assets.LoadTemplatePrefab(MapObjectType.Building),
                MapObjectType.Character => m_Assets.LoadTemplatePrefab(MapObjectType.Character),
                _ => m_Assets.LoadTemplatePrefab(MapObjectType.Prop),
            };

            GameObject go;
            if (templatePrefab != null)
            {
                // 实例化模板预制体
                go = UnityEngine.Object.Instantiate(templatePrefab);
                go.transform.SetParent(parentNode, false);
            }
            else
            {
                // 模板不存在时，回退到创建空物体 + SpriteRenderer
                go = new GameObject();
                go.transform.SetParent(parentNode, false);
                go.AddComponent<SpriteRenderer>();
            }

            go.name = obj.Type == MapObjectType.Character
                ? $"{MapSceneNodeNames.CharacterPrefix}{obj.Name}"
                : $"{MapSceneNodeNames.ObjectPrefix}{obj.Name}";

            // 设置位置（RectTransform 坐标系：左上角原点，Y 向下，与 PSD 图层位置 1:1 对应）
            var rt = go.GetComponent<RectTransform>();
            if (rt == null) rt = go.AddComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);
            rt.anchoredPosition = new Vector2(obj.Position.x, -obj.Position.y);
            // 尺寸：优先 PSD 像素宽高；无像素记录时回退格子数×网格尺寸
            float pixelW = obj.PixelWidth > 0 ? obj.PixelWidth : obj.Width * gridSize;
            float pixelH = obj.PixelHeight > 0 ? obj.PixelHeight : obj.Height * gridSize;
            rt.sizeDelta = new Vector2(pixelW, pixelH);
            // 比例 / 角度变换（锚点沿用当前左上 pivot；---TODO 后续统一为中心锚点，与 IMGUI 预览对齐）
            rt.localScale = new Vector3(obj.Scale, obj.Scale, 1f);
            rt.localRotation = Quaternion.Euler(0f, 0f, obj.Rotation);
            return go;
        }

        /// <summary>
        /// 写入物件视觉表现：有 Spine 资源走 Spine 动画，否则走静态图片（UI Image）分支
        /// </summary>
        /// <param name="go">物件节点</param>
        /// <param name="obj">摆放物数据</param>
        private void ApplyObjectVisual(GameObject go, PlacedObject obj)
        {
            if (go == null) return;

            string animName = obj.AnimName;

            if (!string.IsNullOrEmpty(obj.SpineAssetPath))
            {
                var spineAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<Spine.Unity.SkeletonDataAsset>(obj.SpineAssetPath);
                if (spineAsset != null)
                {
                    // 移除模板上的 SpriteRenderer（如果有）
                    var existingSr = go.GetComponent<SpriteRenderer>();
                    if (existingSr != null) UnityEngine.Object.DestroyImmediate(existingSr);

                    var skeletonAnimation = go.AddComponent<Spine.Unity.SkeletonAnimation>();
                    skeletonAnimation.skeletonDataAsset = spineAsset;
                    skeletonAnimation.Initialize(false);
                    skeletonAnimation.AnimationName = animName;
                    skeletonAnimation.state.SetAnimation(0, animName, obj.Loop);
                    return;
                }

                // Spine 加载失败：回退静态 Sprite（从模板获取 ImagePath）
                var sr = go.GetComponent<SpriteRenderer>();
                if (sr == null) sr = go.AddComponent<SpriteRenderer>();
                var spineFallbackTemplate = ObjectTemplateManager.Instance.GetTemplateById(obj.TemplateId);
                if (spineFallbackTemplate != null && !string.IsNullOrEmpty(spineFallbackTemplate.ImagePath))
                {
                    sr.sprite = m_Assets.LoadSprite(spineFallbackTemplate.ImagePath);
                }
                sr.color = Color.white;
                return;
            }

            ApplyStaticSprite(go, obj);
        }

        /// <summary>
        /// 静态图片分支：清除模板自带 SpriteRenderer，改用 UI Image，
        /// 按「模板 ImagePath → PreviewTexture → 目录名查找」三级回退取图，并写入全局排序键。
        /// </summary>
        /// <param name="go">物件节点</param>
        /// <param name="obj">摆放物数据</param>
        private void ApplyStaticSprite(GameObject go, PlacedObject obj)
        {
            // 模板自带 SpriteRenderer 属于世界 2D 渲染，UI 模式下移除，避免 Image 与 SpriteRenderer 双渲染并存、遮挡口径混乱
            var legacySr = go.GetComponent<UnityEngine.SpriteRenderer>();
            if (legacySr != null) UnityEngine.Object.DestroyImmediate(legacySr);

            var img = go.GetComponent<UnityEngine.UI.Image>();
            if (img == null) img = go.AddComponent<UnityEngine.UI.Image>();
            img.raycastTarget = false;

            bool spriteLoaded = LoadObjectSprite(img, obj);

            if (!spriteLoaded)
            {
                MapToolLog.Warn($"[SyncPlacedObjects] 无法加载图片: {obj.Name} (类型: {obj.Type})");
            }

            img.color = Color.white;
            // 层级排序：独立 overrideSorting Canvas 按 sortingOrder 全局比较，
            // 必须使用跨层可比的全局键（层基线 + 层内槽位），不能直接用层内小值/旧三档值，
            // 否则跨层时数值大的低层物件会压过数值小的高层物件，违背八层契约。
            var canvas = img.GetComponent<UnityEngine.Canvas>();
            if (canvas == null) canvas = img.gameObject.AddComponent<UnityEngine.Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = MapToolEditorUtils.GetGlobalSortOrder(obj);
            // 拉伸标志：false=保持原图比例（原生尺寸）；true=按 PSD 像素尺寸铺满，不保持比例
            img.preserveAspect = !obj.Stretch;
            if (!obj.Stretch) img.SetNativeSize();
        }

        /// <summary>
        /// 三级回退加载物件图片：模板 ImagePath → PreviewTexture 资产路径 → Map 目录按名查找
        /// </summary>
        /// <param name="img">目标 Image</param>
        /// <param name="obj">摆放物数据</param>
        /// <returns>是否成功设置 Sprite</returns>
        private bool LoadObjectSprite(UnityEngine.UI.Image img, PlacedObject obj)
        {
            // 第1级：优先从配置表模板加载 Sprite
            var template = ObjectTemplateManager.Instance.GetTemplateById(obj.TemplateId);
            if (template != null && !string.IsNullOrEmpty(template.ImagePath))
            {
                var sprite = m_Assets.LoadSprite(template.ImagePath);
                if (sprite != null)
                {
                    img.sprite = sprite;
                    MapToolLog.Info($"[SyncPlacedObjects] 从模板加载 Sprite 成功: {obj.Name}, 路径: {template.ImagePath}");
                    return true;
                }
            }

            // 第2级：从 PreviewTexture 反查资产路径（Sprite 缺失时按 Texture2D 现造）
            if (obj.PreviewTexture != null)
            {
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(obj.PreviewTexture);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    var sprite = m_Assets.LoadSpriteOrCreate(assetPath);
                    if (sprite != null)
                    {
                        img.sprite = sprite;
                        return true;
                    }
                }
            }

            // 第3级：按图片名在 Map 目录下查找（关卡 → 章节 → 全局）
            // 解决 PSD 导入后资源还未添加到配置表的情况
            string foundPath = m_Assets.FindMapTextureByLayer(obj.Name, obj.LayerType, m_CurrentChapterId, m_CurrentLevelId);
            if (!string.IsNullOrEmpty(foundPath))
            {
                var sprite = m_Assets.LoadSpriteOrCreate(foundPath);
                if (sprite != null)
                {
                    img.sprite = sprite;
                    MapToolLog.Info($"[SyncPlacedObjects] 按文件名查找 Sprite 成功: {obj.Name}, 路径: {foundPath}");
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 校准层节点兄弟次序与层内渲染次序：
        /// 层节点按八层契约顺序排布；层内对象按「全局排序键 → 脚底 Y」排序（与 IMGUI 口径一致）。
        /// </summary>
        /// <param name="zoneRoot">ZoneRoot 节点</param>
        /// <param name="coordAdapter">坐标适配器节点</param>
        /// <param name="layerOrder">层节点名数组</param>
        private void SortSceneLayers(Transform zoneRoot, Transform coordAdapter, string[] layerOrder)
        {
            if (coordAdapter != null)
            {
                for (int i = 0; i < layerOrder.Length; i++)
                {
                    Transform layer = FindChildRecursive(coordAdapter, layerOrder[i]);
                    if (layer != null)
                    {
                        layer.SetSiblingIndex(i);
                    }
                }
            }

            foreach (var layerName in layerOrder)
            {
                Transform layer = FindChildRecursive(zoneRoot, layerName);
                if (layer == null) continue;

                var objects = layer.GetComponentsInChildren<UnityEngine.UI.Graphic>()
                    .Where(g => g.transform.parent == layer) // 只取直接子物体（Image / RawImage 均纳入）
                    .OrderBy(g =>
                    {
                        // 第一键：独立 Canvas 的全局排序键（层基线 + 层内槽位，同层内即手动 z-order，数值越大越靠前）
                        var canvas = g.GetComponent<UnityEngine.Canvas>();
                        return canvas != null ? canvas.sortingOrder : 0;
                    })
                    .ThenBy(g =>
                    {
                        var rt = g.GetComponent<RectTransform>();
                        if (rt == null) return 0f;
                        // 第二键：脚底 Y（与 IMGUI GetObjectBottomY 口径一致：Position.y + 高度）
                        // 场景锚点 pivot(0,1)：anchoredPosition.y = -Position.y，sizeDelta.y = 高度
                        // 脚底 Y = -anchoredPosition.y + sizeDelta.y，越大越靠下、渲染越靠前
                        return -rt.anchoredPosition.y + rt.sizeDelta.y;
                    })
                    .ToList();

                for (int i = 0; i < objects.Count; i++)
                {
                    objects[i].transform.SetSiblingIndex(i);
                }
            }
        }

        /// <summary>
        /// 将全局路径点和物体路径点同步到场景：蓝色圆点 + 连线
        /// </summary>
        private void SyncPathPointsToScene(RectTransform zoneRoot)
        {
            // 收集第一个 ZoneItem
            Transform zoneItem = FindZoneItem(zoneRoot);
            if (zoneItem == null) return;

            // 清理旧路径点标记（清理 ZoneItem 及其 Root 下的路径点）
            for (int i = zoneItem.childCount - 1; i >= 0; i--)
            {
                var c = zoneItem.GetChild(i);
                if (c.name.StartsWith(MapSceneNodeNames.PathPointPrefix) || c.name.StartsWith(MapSceneNodeNames.PathLinePrefix))
                    UnityEngine.Object.DestroyImmediate(c.gameObject);
                if (c.name == MapSceneNodeNames.Root)
                {
                    for (int k = c.childCount - 1; k >= 0; k--)
                    {
                        var cc = c.GetChild(k);
                        if (cc.name.StartsWith(MapSceneNodeNames.PathPointPrefix) || cc.name.StartsWith(MapSceneNodeNames.PathLinePrefix))
                            UnityEngine.Object.DestroyImmediate(cc.gameObject);
                    }
                }
            }

            // 路径点挂到 ZoneItem 的 Root 下
            RectTransform GetZoneRootAt(Vector2 globalPos, out Vector2 localPos)
            {
                localPos = globalPos;
                Transform rootNode = FindChildRecursive(zoneItem, MapSceneNodeNames.Root);
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
                    CreatePathMarker(parent, lp, $"{MapSceneNodeNames.PathPointPrefix}{obj.Name}m{obj.PathPoints.IndexOf(p)}");
                }
                for (int i = 0; i < obj.PathPoints.Count - 1; i++)
                {
                    var parent = GetZoneRootAt(obj.PathPoints[i], out Vector2 lp1);
                    GetZoneRootAt(obj.PathPoints[i + 1], out Vector2 lp2);
                    CreatePathLine(parent, lp1, lp2, $"{MapSceneNodeNames.PathLinePrefix}{lineIndex++}");
                }
            }

            // 全局路径点
            if (m_PathPoints != null && m_PathPoints.Count >= 2)
            {
                for (int i = 0; i < m_PathPoints.Count; i++)
                {
                    var parent = GetZoneRootAt(m_PathPoints[i], out Vector2 lp);
                    CreatePathMarker(parent, lp, $"{MapSceneNodeNames.PathPointPrefix}Globalm{i}");
                }
                for (int i = 0; i < m_PathPoints.Count - 1; i++)
                {
                    var parent = GetZoneRootAt(m_PathPoints[i], out Vector2 lp1);
                    GetZoneRootAt(m_PathPoints[i + 1], out Vector2 lp2);
                    CreatePathLine(parent, lp1, lp2, $"{MapSceneNodeNames.PathLinePrefix}Gm{i}");
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
        /// 模板保存后，按最新模板同步场景中已摆放的对应实例；位置、大小、路径点保持不变。
        /// 匹配键优先 TemplateId，缺失时按名称回退。
        /// </summary>
        public void SyncInstancesFromTemplates()
        {
            if (m_PlacedObjects == null || m_PlacedObjects.Count == 0) return;

            var manager = ObjectTemplateManager.Instance;
            int synced = 0;
            foreach (var obj in m_PlacedObjects)
            {
                if (obj == null) continue;
                ObjectTemplate template = manager.GetTemplateById(obj.TemplateId);
                if (template == null && !string.IsNullOrEmpty(obj.Name))
                    template = manager.GetTemplateByName(obj.Name);
                if (template == null) continue;

                obj.SyncFromTemplate(template);
                synced++;
            }

            RefreshSceneObjects();
            // 刷新当前选中物件的右栏回填（收集/交通标记随新模板条件显隐）
            if (m_EditTarget != null)
                SelectEditTarget(m_EditTarget);
            MapToolLog.Info($"[SceneEditModule] 已按最新模板同步 {synced}/{m_PlacedObjects.Count} 个场景物件");
        }

        /// <summary>
        /// 保存/发布前，把当前场景各实例的模板级共享属性批量回写到对应内存模板。
        /// 仅回写同类共享属性；位置/尺寸/路径点/独立显示档等实例布局字段不进模板。
        /// 同一模板被多个实例引用时，当前选中实例优先、其余按顺序首个命中为准，
        /// 其余实例的差异保留在实例上，不主动覆盖其他已摆放实例。
        /// </summary>
        public void PushInstancesToTemplates()
        {
            if (m_PlacedObjects == null || m_PlacedObjects.Count == 0) return;

            var manager = ObjectTemplateManager.Instance;
            var pushed = new HashSet<string>();
            int count = 0;

            // 先处理当前选中实例，保证刚调整的物件一定进入模板
            if (m_EditTarget != null && ApplyInstanceToTemplate(m_EditTarget, manager, pushed))
                count++;

            foreach (var obj in m_PlacedObjects)
            {
                if (obj == null || ReferenceEquals(obj, m_EditTarget)) continue;
                if (ApplyInstanceToTemplate(obj, manager, pushed))
                    count++;
            }

            MapToolLog.Info($"[SceneEditModule] 已把 {count} 个场景物件的共享属性回写对应模板");
        }

        /// <summary>
        /// 按 TemplateId 优先、Name 回退匹配模板；同一模板仅回写一次。
        /// </summary>
        /// <returns>本次是否成功写入了该模板</returns>
        private bool ApplyInstanceToTemplate(PlacedObject obj, ObjectTemplateManager manager, HashSet<string> pushed)
        {
            ObjectTemplate template = manager.GetTemplateById(obj.TemplateId);
            if (template == null && !string.IsNullOrEmpty(obj.Name))
                template = manager.GetTemplateByName(obj.Name);
            if (template == null) return false;

            string key = !string.IsNullOrEmpty(template.TemplateId) ? template.TemplateId : template.TemplateName;
            if (pushed.Contains(key)) return false;
            pushed.Add(key);

            ApplySharedFieldsToTemplate(obj, template);
            return true;
        }

        /// <summary>
        /// 把实例上的模板级共享属性拷贝到模板；与 <see cref="PlacedObject.SyncFromTemplate"/> 的正向覆盖集合对称。
        /// 交互类型/朝向做中文→code 归一；交通静态类别（路灯/马路/斑马线）实例无法表达，保留模板原值。
        /// </summary>
        private static void ApplySharedFieldsToTemplate(PlacedObject obj, ObjectTemplate template)
        {
            // 动画属性
            template.AnimName = obj.AnimName;
            template.Loop = obj.Loop;
            template.Speed = obj.Speed;

            // 交互属性
            template.Interactable = obj.Interactable;
            template.InteractType = TemplateInteractType.ToCode(obj.InteractType);
            template.DialogBubble = obj.DialogBubble;

            // 渲染与物理属性
            template.SortOrder = MapToolEditorUtils.SortOrderFromInstance(template, obj);
            template.YSortEnabled = obj.YSortEnabled;
            template.YSortOffset = obj.YSortOffset;
            template.CollisionEnabled = obj.CollisionEnabled;
            template.FacingDirection = TemplateFacing.ToCode(obj.FacingDirection);
            template.ShadowEnabled = obj.ShadowEnabled;

            // 移动与循环属性
            template.IsMovable = obj.IsMovable;
            template.MoveAnimName = obj.MoveAnimName;
            template.LoopRestartDelay = obj.LoopRestartDelay;
            template.HideWhenOffMap = obj.HideWhenOffMap;

            // 传送门属性
            template.IsTeleport = obj.IsTeleport;
            template.TeleportTargetScene = obj.TeleportTargetScene;
            template.TeleportTargetX = obj.TeleportTargetX;
            template.TeleportTargetY = obj.TeleportTargetY;

            // 收集物属性
            template.IsCollectible = obj.IsCollectible;
            template.RespawnTime = obj.RespawnTime;

            // 交通类别：仅实例明确为载具/行人时回写；role==none 可能对应静态设施，保留模板原静态类别
            if (obj.IsVehicle || obj.TrafficRole == TemplateTrafficCategory.Vehicle)
                template.TrafficCategory = TemplateTrafficCategory.Vehicle;
            else if (obj.TrafficRole == TemplateTrafficCategory.Pedestrian)
                template.TrafficCategory = TemplateTrafficCategory.Pedestrian;
        }

        /// <summary>
        /// 解析用于显示的物件类型：优先模板原始类型（交通载具被归并到角色链路后，显示仍按模板归属），缺失时回退实例类型。
        /// </summary>
        private MapObjectType ResolveDisplayType(PlacedObject obj)
        {
            if (obj == null) return MapObjectType.Prop;
            ObjectTemplate template = ObjectTemplateManager.Instance.GetTemplateById(obj.TemplateId);
            return template != null ? template.Type : obj.Type;
        }

        /// <summary>
        /// 从 PNG 序列导入结果应用到编辑器（自动设置背景 + 创建物件列表）
        /// 注意：物件位置默认在原点，需要手动调整
        /// </summary>
        /// <param name="result">导入结果</param>
        public void ApplyImageImport(MapImageImportResult result)
        {
            if (result == null) return;

            // 1. 设置背景图（存完整路径，方便直接加载）
            if (!string.IsNullOrEmpty(result.BackgroundPath) && !string.IsNullOrEmpty(m_CurrentLevelId))
            {
                m_LevelBgDict[m_CurrentLevelId] = result.BackgroundPath; // 存完整 Assets 路径
                MapToolLog.Info($"背景图已设置: {result.BackgroundPath}");
            }

            // 2. 设置地图宽高
            if (result.MapWidth > 0) m_MapWidth = result.MapWidth;
            if (result.MapHeight > 0) m_MapHeight = result.MapHeight;

            // 3. 自动创建物件（位置在原点，需要手动摆）
            m_PlacedObjects.Clear();
            ClearTrafficWorkingData();
            int index = 0;
            foreach (var info in result.Objects)
            {
                Texture2D previewTex = AssetDatabase.LoadAssetAtPath<Texture2D>(info.PreviewPath);
                Vector2Int defaultSize = MapToolEditorUtils.GetDefaultObjectSize(info.Type);
                m_PlacedObjects.Add(new PlacedObject
                {
                    Id = System.Guid.NewGuid().ToString("N").Substring(0, 8),
                    TemplateId = info.ObjectId,
                    Name = info.ObjectName,
                    Type = info.Type,
                    LayerType = MapLayerCatalog.Normalize(info.LayerType),
                    Position = new Vector2(index * MapToolConstants.GridSize, index * MapToolConstants.GridSize), // 错开排列，避免全叠在原点
                    Width = defaultSize.x,
                    Height = defaultSize.y,
                    PreviewTexture = previewTex,
                });
                index++;
            }

            // 4. 刷新场景
            RefreshSceneObjects();
            m_StatusBar.Refresh(m_Zoom, m_MousePos, m_TileX, m_TileY, OccupiedCellCount, GridCols, GridRows, m_MapSavePath);
            m_StatusBar.ShowMessage($"已导入 {result.Objects.Count} 个物件，请手动调整位置");
        }

        /// <summary>
        /// 设置当前放置类型（Tab 切换 / 顶部图层 Toggle 联动）
        /// 建筑/人物/物件类型切换（仅切换左侧列表筛选，不影响图层显示开关）
        /// </summary>
        /// <param name="type">类型：0=建筑 1=人物 2=物件 3=猫猫</param>
        public void SetPlaceType(int type)
        {
            MapObjectType objectType = (MapObjectType)type;
            m_SelectedObjectType = objectType;
            // 图层显示开关独立控制，不随放置类型联动切换
            // 用户可通过顶部图层开关自由控制各层级的显示/隐藏
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
                LayerType = objectType == MapObjectType.Building ? MapLayerCatalog.Building
                    : objectType == MapObjectType.Character || objectType == MapObjectType.Cat ? MapLayerCatalog.Character
                    : MapLayerCatalog.Prop,
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
                m_PendingPlace.SortOrder = MapToolEditorUtils.SortOrderFromTemplate(m_PendingPlace, template);
                m_PendingPlace.DisplayLayer = template.DisplayLayer;
                m_PendingPlace.CollisionEnabled = template.CollisionEnabled;
                m_PendingPlace.DialogBubble = template.DialogBubble;
                m_PendingPlace.bubbleId = template.BubbleId;
                m_PendingPlace.FacingDirection = template.FacingDirection;
                m_PendingPlace.ShadowEnabled = template.ShadowEnabled;
                m_PendingPlace.IsCollectible = template.IsCollectible;
                m_PendingPlace.RespawnTime = template.RespawnTime;

                // 交通类别 → 实例默认交通角色/载具标记（汽车→载具，人物→行人，其余静态类别→不参与；实例右栏仍可单独调整）
                m_PendingPlace.TrafficRole = TemplateTrafficCategory.ResolveTrafficRole(template.TrafficCategory);
                m_PendingPlace.IsVehicle = TemplateTrafficCategory.IsVehicle(template.TrafficCategory);
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
            MapToolLog.Info($"选中物体: {objectName} ({objectId}), 类型: {type}");
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

            // 从配置表查找模板配置（如果有配置，优先使用配置表中的属性）
            ObjectTemplate template = ObjectTemplateManager.Instance.GetTemplateById(obj.TemplateId);

            EnterEditMode(obj);
            FillAnimTabFields(obj, template);
            FillOverviewFields(obj, template);
            FillPropertyFields(obj, template);

            // 显示底部属性面板并刷新联动区域
            m_PropertyPanel.SetVisible(true);
            UpdateTransformLabels();
            // ---TODO 交通规则：回填载具/行人交通属性并刷新灯绑定与校验
            RefreshTrafficPanel();
            RefreshActionAreaVisibility();

            UpdateEditTargetUI();
            string templateInfo = template != null ? $"（配置表: {template.TemplateName}）" : "（无配置表）";
            MapToolLog.Info($"选中编辑对象: {obj.Name}（类型: {MapToolEditorUtils.FormatObjectTypeName(ResolveDisplayType(obj), obj.IsVehicle)}，路径点数: {obj.PathPoints.Count}）{templateInfo}");
        }

        /// <summary>
        /// 进入编辑态：切到对象编辑面板，路径绘制仅对人物开放
        /// </summary>
        /// <param name="obj">目标物体</param>
        private void EnterEditMode(PlacedObject obj)
        {
            bool isCharacter = obj.Type == MapObjectType.Character;
            m_IsPathDrawing = isCharacter;

            m_ObjectEditPanel.SetVisible(true);
            // 人物默认切到"可编辑路径"，其他类型切到"编辑动画"
            SetActiveObjectTab(isCharacter ? 0 : 1);

            // 路径绘制按钮只在人物时显示（按钮文案复位为「停止绘制路径」）
            m_BtnPathDraw.SetVisible(isCharacter);
            if (m_BtnPathDraw != null)
                m_BtnPathDraw.text = MapToolConstants.PathDrawStopText;
        }

        /// <summary>
        /// 回填"编辑动画"切页字段（实例属性优先，无实例值时用模板默认值）
        /// </summary>
        /// <param name="obj">目标物体</param>
        /// <param name="template">模板配置，可为空</param>
        private void FillAnimTabFields(PlacedObject obj, ObjectTemplate template)
        {
            string animName = !string.IsNullOrEmpty(obj.AnimName) ? obj.AnimName : (template?.AnimName ?? MapToolConstants.DefaultAnimName);
            bool loop = obj.Loop;
            float speed = obj.Speed;

            m_AnimNameField.SetValueSafe(animName);
            m_AnimLoopToggle.SetValueSafe(loop);
            m_AnimSpeedField.SetValueSafe(speed.ToString("F1"));
        }

        /// <summary>
        /// 回填对象标识区字段（模板ID / 类型标签 / 格子坐标 / 交互态）
        /// </summary>
        /// <param name="obj">目标物体</param>
        /// <param name="template">模板配置，可为空</param>
        private void FillOverviewFields(PlacedObject obj, ObjectTemplate template)
        {
            m_TemplateIdField.SetValueSafe(obj.TemplateId);

            if (m_TagField != null)
            {
                // 标签类型优先模板原始类型；载具单独标 Vehicle，避免被角色链路归并误显示为 Character
                MapObjectType displayType = template != null ? template.Type : obj.Type;
                m_TagField.SetValueWithoutNotify(obj.IsVehicle ? MapToolConstants.VehicleTagDisplay : displayType.ToString());
            }

            Vector2Int cell = MapToolEditorUtils.WorldToCell(obj.Position);
            m_GridPosField.SetValueSafe($"{cell.x}/{cell.y}");
            m_BuildingTemplateField.SetValueSafe($"{obj.Id}#{cell.x}");
            m_GridPrefixField.SetValueSafe(MapToolConstants.PrefixPlaceholder);
            m_InteractStateField.SetValueSafe(obj.Interactable ? MapToolConstants.InteractStateOnDisplay : MapToolConstants.InteractStateOffDisplay);
        }

        /// <summary>
        /// 回填属性区字段（排序/交互类型/碰撞/气泡/朝向/显示层/阴影/收集/交通）
        /// </summary>
        /// <param name="obj">目标物体</param>
        /// <param name="template">模板配置，可为空</param>
        private void FillPropertyFields(PlacedObject obj, ObjectTemplate template)
        {
            int sortOrder = obj.SortOrder > 0 ? obj.SortOrder : (template?.SortOrder ?? 0);
            string interactType = !string.IsNullOrEmpty(obj.InteractType) ? obj.InteractType : (template?.InteractType ?? TemplateInteractType.None);
            string dialogBubble = !string.IsNullOrEmpty(obj.DialogBubble) ? obj.DialogBubble : (template?.DialogBubble ?? string.Empty);
            string facing = !string.IsNullOrEmpty(obj.FacingDirection) ? obj.FacingDirection : (template?.FacingDirection ?? TemplateFacing.Down);
            bool collectible = obj.IsCollectible;

            m_SortOrderField.SetValueSafe(sortOrder.ToString());
            m_InteractTypeField.SetValueSafe(TemplateInteractType.ToDisplay(interactType));
            m_CollisionToggle.SetValueSafe(obj.CollisionEnabled);
            m_DialogBubbleField.SetValueSafe(dialogBubble);
            m_FacingField.SetValueSafe(TemplateFacing.ToDisplay(facing));
            if (m_DisplayLayerField != null)
            {
                string effectiveLayer = MapLayerCatalog.ResolveDisplayLayer(obj.LayerType, obj.DisplayLayer);
                m_DisplayLayerField.SetValueWithoutNotify(MapLayerCatalog.GetLayerDisplayName(effectiveLayer));
            }
            m_ShadowToggle.SetValueSafe(obj.ShadowEnabled);
            m_CollectibleToggle.SetValueSafe(collectible);

            // 变换区跟随收集勾选；右栏卡片按收集/交通可见性刷新显隐，空内容不留空框
            SetCollectibleActionVisible(collectible);
            // 收集/交通摘要标记按条件显隐：未设置对应属性时整组（标签+开关）隐藏
            m_CollectibleGroup.SetVisible(collectible);
            m_TrafficGroup.SetVisible(IsTrafficRoleActive(obj));

            // ---TODO 气泡配置表通用化：回显气泡ID（引用 Tables.Bubble）
            m_BubbleIdField.SetValueSafe(obj.bubbleId);
        }

        /// <summary>
        /// 取消选中编辑目标
        /// </summary>
        public void ClearEditTarget()
        {
            m_EditTarget = null;
            m_SelectedPathPointIndex = -1;
            m_IsPathDrawing = false;
            m_ObjectEditPanel.SetVisible(false);
            // 隐藏底部属性面板
            m_PropertyPanel.SetVisible(false);
            // 同步隐藏收集/交通摘要标记分组
            m_CollectibleGroup.SetVisible(false);
            m_TrafficGroup.SetVisible(false);
            // 同步隐藏变换/交通区容器
            SetActionHostVisible(false);
            // ---TODO 交通规则：清空选中后同步清空交通属性回填
            RefreshTrafficPanel();
            MapToolLog.Info("取消选中编辑对象");
        }

    }
}
