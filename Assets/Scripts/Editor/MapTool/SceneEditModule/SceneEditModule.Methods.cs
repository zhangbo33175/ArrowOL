/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SceneEditModule.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景编辑模块 - 私有方法（UI查找/事件绑定/状态栏/保存与导出入口）
 ***************************************************************/

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
    /// 场景编辑模块 - 私有方法（UI查找/事件绑定/状态栏/保存导出）
    /// </summary>
    public partial class SceneEditModule
    {
        /// <summary>
        /// 绑定事件
        /// </summary>
        private void BindEvents()
        {
            // 更新地图按钮
            var updateBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnUpdateMap);
            if (updateBtn != null)
                updateBtn.clicked += OnUpdateMapClicked;
            // 导出切片按钮
            var exportBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnExportSlice);
            if (exportBtn != null)
                exportBtn.clicked += OnExportSliceClicked;
            // 保存按钮
            var saveBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnSave);
            if (saveBtn != null)
                saveBtn.clicked += OnSaveClicked;
            // 预览地图按钮
            var previewBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnPreviewMap);
            if (previewBtn != null)
                previewBtn.clicked += OnPreviewClicked;

            // 刷新层级按钮
            var refreshLayerBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnRefreshLayer);
            if (refreshLayerBtn != null)
                refreshLayerBtn.clicked += OnRefreshLayerClicked;

            // 资源整理 - 扫描未配置资源按钮
            var scanResourcesBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnScanResources);
            if (scanResourcesBtn != null)
                scanResourcesBtn.clicked += OnScanResourcesClicked;
            // 清空摆放物按钮
            var clearObjectsBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnClearObjects);
            if (clearObjectsBtn != null)
                clearObjectsBtn.clicked += OnClearObjectsClicked;
            // 清空格子数据按钮
            var clearGridBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnClearGridData);
            if (clearGridBtn != null)
                clearGridBtn.clicked += OnClearGridDataClicked;
            // 导入 PSD 按钮（从 PSD 图层自动拆分生成地图）
            var importPsdBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnImportPsd);
            if (importPsdBtn != null)
                importPsdBtn.clicked += () => m_PsdImportModule.ImportPsd();
            // 导出设置按钮
            var exportSettingsBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnExportSettings);
            if (exportSettingsBtn != null)
                exportSettingsBtn.clicked += OnExportSettingsClicked;
            // 对象编辑面板事件（可编辑路径 / 编辑动画 切页）
            if (m_TabEditPath != null)
                m_TabEditPath.clicked += OnTabEditPathClicked;
            if (m_TabEditAnim != null)
                m_TabEditAnim.clicked += OnTabEditAnimClicked;
            if (m_BtnPathDraw != null)
                m_BtnPathDraw.clicked += OnPathDrawClicked;
            if (m_BtnPathUndo != null)
                m_BtnPathUndo.clicked += OnPathUndoClicked;
            if (m_BtnPathClear != null)
                m_BtnPathClear.clicked += OnPathClearClicked;
            if (m_BtnClearTarget != null)
                m_BtnClearTarget.clicked += OnClearTargetClicked;
            // 动画切页字段变更 → 写回当前编辑目标
            if (m_AnimNameField != null)
                m_AnimNameField.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.AnimName = evt.newValue;
                });
            if (m_AnimLoopToggle != null)
                m_AnimLoopToggle.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.Loop = evt.newValue;
                });
            if (m_AnimSpeedField != null)
                m_AnimSpeedField.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null && float.TryParse(evt.newValue, out float speed))
                        m_EditTarget.Speed = speed;
                });
            // 层级 SortOrder 输入框变更 → 写回当前编辑目标（同层 z-order，数值越大越靠前）
            // 钳制到所属显示层档位区间，避免手填越界值压过相邻层基线；变更后走统一收尾同步场景
            if (m_SortOrderField != null)
                m_SortOrderField.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget == null) return;
                    if (int.TryParse(evt.newValue, out int order))
                    {
                        // ---TODO [层级档位] 越界值钳制到 [基线, 基线+LayerSlotCount-1]，与运行时同层契约一致
                        m_EditTarget.SortOrder = MapToolEditorUtils.ClampSortOrderToSlot(m_EditTarget, order);
                        AfterTargetOrderChanged();
                    }
                });
            // 属性面板按钮事件
            if (m_DeleteObjectBtn != null)
                m_DeleteObjectBtn.clicked += OnDeleteSelectedObject;
            if (m_PublishBtn != null)
                m_PublishBtn.clicked += OnPublishClicked;
        }

        /// <summary>
        /// 更新地图按钮点击
        /// 流程：磁盘 JSON 可能被外部修改 → 刷新 AssetDatabase → 用最新配置文件重建地图
        /// 1. 校验当前已加载关卡
        /// 2. 二次确认（编辑器内未保存的摆放/修改会被磁盘配置覆盖）
        /// 3. AssetDatabase.Refresh 拉取最新 JSON
        /// 4. 清空当前选中态与摆放物
        /// 5. LoadMapConfig 从 JSON 还原宽高/背景/摆放物/视口
        /// 6. 同步宽高输入框、重新实例化预制体、同步场景节点
        /// </summary>
        private void OnUpdateMapClicked()
        {
            // 1. 前置校验：没有当前关卡或保存路径时不允许更新
            if (string.IsNullOrEmpty(m_CurrentLevelId) || string.IsNullOrEmpty(m_MapSavePath))
            {
                m_StatusBar.ShowMessage("请先在左侧选择一个关卡再更新地图");
                EditorUtility.DisplayDialog("更新地图", "请先在左侧章节/关卡列表中选择一个关卡，再点击更新地图。", "知道了");
                return;
            }

            // 2. 二次确认：编辑器内未保存的摆放会被磁盘 JSON 覆盖
            bool confirm = EditorUtility.DisplayDialog(
                "更新地图",
                $"将从磁盘配置文件重新加载地图：\n{m_MapSavePath}\n\n" +
                $"当前编辑器中的未保存修改（摆放物、宽高调整等）会被丢弃，是否继续？",
                "重新加载", "取消");
            if (!confirm)
            {
                m_StatusBar.ShowMessage("已取消更新地图");
                return;
            }

            // 3. 拉取磁盘最新文件（外部工具/脚本改过 JSON/配置表/PNG 时，Unity 缓存可能是旧的）
            AssetDatabase.Refresh();

            // 4. 清空当前选中态与编辑面板
            m_EditTarget = null;
            if (m_ObjectEditPanel != null)
                m_ObjectEditPanel.style.display = DisplayStyle.None;

            // 5. 先重读主关卡配置表（Level_cfg）——背景图名的权威来源，
            //    否则只看 JSON 会漏掉在 Level_cfg 里改过 Level_bg 的情况
            LoadMainLevelsConfig();

            // 6. 再从 JSON 还原地图数据（宽高、摆放物、视口定位；
            //    若 JSON 里 bgName 非空会覆盖字典，优先级高于主配置表）
            LoadMapConfig();

            // 7. 同步宽高到输入框（LoadMapConfig 只改了 m_MapWidth/Height，没写回 TextField）
            if (m_MapWidth > 0 && m_WidthField != null)
                m_WidthField.SetValueWithoutNotify(m_MapWidth.ToString());
            if (m_MapHeight > 0 && m_HeightField != null)
                m_HeightField.SetValueWithoutNotify(m_MapHeight.ToString());
            m_ConfigMapWidth = m_MapWidth;
            m_ConfigMapHeight = m_MapHeight;

            // 8. 重新实例化地图预制体到场景（按最新配置重建场景节点）
            if (m_MapPrefab != null)
            {
                InstantiateMapInScene();
            }
            // 9. 把还原后的摆放物同步到场景节点
            RefreshSceneObjects();

            m_StatusBar.Refresh(m_Zoom, m_MousePos, m_TileX, m_TileY, OccupiedCellCount, GridCols, GridRows, m_MapSavePath);
            m_StatusBar.ShowMessage($"已从配置文件重新加载地图（摆放物 {m_PlacedObjects.Count}）");
            string currentBg = m_LevelBgDict.TryGetValue(m_CurrentLevelId, out string bg) ? bg : "<空>";
            Debug.Log($"[更新地图] 已从 {m_MapSavePath} 重新加载，摆放物 {m_PlacedObjects.Count}，宽高 {m_MapWidth}x{m_MapHeight}，背景 {currentBg}");
        }

        /// <summary>
        /// 宽高输入框变更回调：解析后回写地图基准尺寸并同步配置尺寸，
        /// 网格行列、吸附、命中与边界限制按新基准铺满；非法值回退当前基准。
        /// </summary>
        private void OnMapWidthHeightChanged(ChangeEvent<string> evt)
        {
            if (m_WidthField != null)
            {
                int w = ParseInt(m_WidthField.value, m_MapWidth);
                m_MapWidth = w > 0 ? w : m_MapWidth;
            }
            if (m_HeightField != null)
            {
                int h = ParseInt(m_HeightField.value, m_MapHeight);
                m_MapHeight = h > 0 ? h : m_MapHeight;
            }
            m_ConfigMapWidth = m_MapWidth;
            m_ConfigMapHeight = m_MapHeight;
            m_GuiContainer?.MarkDirtyRepaint();
        }

        /// <summary>
        /// 导出切片按钮点击：导出所有物件图片到对应文件夹，按章节/关卡/八层类型分类
        /// </summary>
        private void OnExportSliceClicked()
        {
            if (m_PlacedObjects.Count == 0)
            {
                EditorUtility.DisplayDialog("导出切片", "当前地图没有任何物件", "确定");
                return;
            }

            // 1. 获取章节/关卡 ID（用于三级目录：Chapter{id}/Level{lv}/<Layer>）
            string chapterId = string.IsNullOrEmpty(m_CurrentChapterId) ? "DefaultChapter" : m_CurrentChapterId;
            string levelId = string.IsNullOrEmpty(m_CurrentLevelId) ? "DefaultLevel" : m_CurrentLevelId;

            // 2. 输出目录：Assets/Res/Textures/Map/Chapter{ch}/Level{lv}/<Layer>/（八层目录名对齐 MapLayerCatalog）
            string baseDir = $"{MapToolConstants.MapTextureRoot}/Chapter{chapterId}/Level{levelId}";

            // 确保八层目录存在
            foreach (var layerType in MapLayerCatalog.AllLayerTypes)
            {
                string dir = MapLayerCatalog.GetLevelDir(chapterId, levelId, layerType);
                string fullPath = MapToolPathHelper.ToAbsolutePath(dir);
                if (!System.IO.Directory.Exists(fullPath))
                    System.IO.Directory.CreateDirectory(fullPath);
            }

            // 3. 导出所有物件
            int exportedCount = 0;
            var exportedNames = new HashSet<string>(); // 去重

            int skippedCount = 0;
            foreach (var obj in m_PlacedObjects)
            {
                if (obj.PreviewTexture == null) continue;

                // 只导出带前缀的物件（bg_/grass_/road_/plant_/building_/char_/prop_/cat_/eff_/bubble_）
                string lowerName = obj.Name.ToLower();
                bool hasPrefix = lowerName.StartsWith("bg_")
                    || lowerName.StartsWith("grass_")
                    || lowerName.StartsWith("road_")
                    || lowerName.StartsWith("plant_")
                    || lowerName.StartsWith("building_")
                    || lowerName.StartsWith("char_")
                    || lowerName.StartsWith("prop_")
                    || lowerName.StartsWith("cat_")
                    || lowerName.StartsWith("eff_")
                    || lowerName.StartsWith("bubble_");

                // 带中文名字的跳过
                bool hasChinese = System.Text.RegularExpressions.Regex.IsMatch(obj.Name, @"[\u4e00-\u9fff]");

                if (!hasPrefix || hasChinese)
                {
                    skippedCount++;
                    continue;
                }

                // 按层级类型确定输出目录（八层目录名对齐 MapLayerCatalog）
                string targetDir = MapLayerCatalog.GetLevelDir(chapterId, levelId, obj.LayerType);

                // 文件名（去重）
                string fileName = obj.Name.Replace(" ", "_").Replace("/", "_");
                if (exportedNames.Contains(fileName)) continue;
                exportedNames.Add(fileName);

                // 导出 PNG
                string fullPath = System.IO.Path.Combine(
                    MapToolPathHelper.ToAbsolutePath(targetDir),
                    $"{fileName}.png"
                );

                try
                {
                    // 方案1：直接复制源 PNG 文件（最快）
                    string assetPath = AssetDatabase.GetAssetPath(obj.PreviewTexture);
                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        string srcFullPath = MapToolPathHelper.ToAbsolutePath(assetPath);
                        if (System.IO.File.Exists(srcFullPath))
                        {
                            System.IO.File.Copy(srcFullPath, fullPath, true);
                            exportedCount++;
                            continue;
                        }
                    }

                    // 方案2：如果 AssetDatabase 找不到，用 EncodeToPNG
                    Texture2D srcTex = obj.PreviewTexture as Texture2D;
                    if (srcTex == null)
                    {
                        Debug.LogWarning($"[导出切片] {obj.Name} 不是 Texture2D，跳过");
                        continue;
                    }

                    RenderTexture rt = RenderTexture.GetTemporary(srcTex.width, srcTex.height, 0);
                    Graphics.Blit(srcTex, rt);
                    RenderTexture prev = RenderTexture.active;
                    RenderTexture.active = rt;
                    Texture2D readableTex = new Texture2D(srcTex.width, srcTex.height);
                    readableTex.ReadPixels(new Rect(0, 0, srcTex.width, srcTex.height), 0, 0);
                    readableTex.Apply();
                    RenderTexture.active = prev;
                    RenderTexture.ReleaseTemporary(rt);

                    byte[] pngData = readableTex.EncodeToPNG();
                    System.IO.File.WriteAllBytes(fullPath, pngData);
                    exportedCount++;
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"[导出切片] 导出 {obj.Name} 失败: {e.Message}");
                }
            }

            // 4. 刷新 AssetDatabase
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("导出切片完成",
                $"章节: Chapter{chapterId}\n关卡: Level{levelId}\n\n导出物件数: {exportedCount}\n跳过（无前缀）: {skippedCount}\n保存目录: {baseDir}\n\n按层级分类:\n  Background/Greenery/Road/Buildings/Props/Characters/Foreground/Bubbles",
                "确定");
        }

        /// <summary>
        /// <summary>
        /// 扫描未配置资源按钮点击
        /// </summary>
        private void OnScanResourcesClicked()
        {
            string mapDir = MapToolConstants.ResourceScanDir;
            if (!AssetDatabase.IsValidFolder(mapDir))
            {
                EditorUtility.DisplayDialog("扫描资源", $"目录不存在：{mapDir}", "确定");
                return;
            }

            // 扫描所有 PNG 图片
            string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { mapDir });
            var allResources = new List<string>();
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.EndsWith(".png"))
                {
                    allResources.Add(path);
                }
            }

            // 找出配置表里没有的资源
            var unconfigured = new List<string>();
            foreach (var path in allResources)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                var template = ObjectTemplateManager.Instance.GetTemplateByName(fileName);
                if (template == null)
                {
                    unconfigured.Add(path);
                }
            }

            // 显示结果
            var listContainer = m_Root.Q<VisualElement>("unconfigured-resources-list");
            if (listContainer != null)
            {
                listContainer.Clear();
                if (unconfigured.Count == 0)
                {
                    listContainer.Add(new Label("所有资源都已在配置表中 ✓") { style = { color = Color.green } });
                }
                else
                {
                    listContainer.Add(new Label($"共发现 {unconfigured.Count} 个未配置资源："));
                    foreach (var path in unconfigured)
                    {
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                        var btn = new Button(() => ShowResourceDetail(path)) { text = fileName };
                        listContainer.Add(btn);
                    }
                }
            }

            EditorUtility.DisplayDialog("扫描完成",
                $"扫描目录: {mapDir}\n总资源数: {allResources.Count}\n未配置数: {unconfigured.Count}",
                "确定");
        }

        /// <summary>
        /// 显示资源详情
        /// </summary>
        private void ShowResourceDetail(string path)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
            var nameLabel = m_Root.Q<Label>("resource-detail-name");
            var typeLabel = m_Root.Q<Label>("resource-detail-type");
            var pathLabel = m_Root.Q<Label>("resource-detail-path");
            var sizeLabel = m_Root.Q<Label>("resource-detail-size");

            if (nameLabel != null) nameLabel.text = fileName;
            if (pathLabel != null) pathLabel.text = path;

            // 自动识别类型
            string lowerName = fileName.ToLower();
            string autoType = "未知";
            if (lowerName.StartsWith("bg_")) autoType = "背景";
            else if (lowerName.StartsWith("building_")) autoType = "建筑";
            else if (lowerName.StartsWith("char_")) autoType = "人物";
            else if (lowerName.StartsWith("prop_")) autoType = "物件";
            else if (lowerName.StartsWith("cat_")) autoType = "猫猫";

            if (typeLabel != null) typeLabel.text = autoType;

            // 显示尺寸
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (tex != null && sizeLabel != null)
            {
                sizeLabel.text = $"{tex.width} x {tex.height}";
            }
        }

        /// <summary>
        /// 刷新层级按钮点击：按 YSort 规则重新排序所有物件，优化遮挡关系
        /// </summary>
        private void OnRefreshLayerClicked()
        {
            if (m_PlacedObjects == null || m_PlacedObjects.Count == 0)
            {
                EditorUtility.DisplayDialog("刷新层级", "当前地图没有任何物件", "确定");
                return;
            }

            // 找到当前地图的 ZoneItem
            var zoneRoot = GameObject.Find("GameMainMap");
            if (zoneRoot == null)
            {
                EditorUtility.DisplayDialog("刷新层级", "找不到 GameMainMap 节点", "确定");
                return;
            }

            // 调整层级节点顺序：Background → Greenery → Road → Buildings → Props → Characters → Foreground → Bubbles（最上层）
            string[] layerOrder = MapLayerCatalog.AllLayerTypes
                .Select(MapLayerCatalog.GetLayerDir)
                .ToArray();
            var coordAdapter = FindChildRecursive(zoneRoot.transform, "CoordAdapter");
            if (coordAdapter != null)
            {
                for (int i = 0; i < layerOrder.Length; i++)
                {
                    var layer = FindChildRecursive(coordAdapter, layerOrder[i]);
                    if (layer != null)
                    {
                        layer.SetSiblingIndex(i);
                    }
                }
            }

            // 每个层级节点内部按 SortOrder + 脚底 Y 排序（与场景同步段、IMGUI 绘制口径一致）
            // 收集 Graphic 以同时覆盖 Image 与 RawImage；第一键独立 Canvas 的 sortingOrder（手动 z-order），
            // 第二键脚底 Y（YSort 近处优先），避免刷新层级把手动层级位次冲掉。
            int totalSorted = 0;
            foreach (var layerName in layerOrder)
            {
                var layer = FindChildRecursive(zoneRoot.transform, layerName);
                if (layer == null) continue;

                var objects = layer.GetComponentsInChildren<UnityEngine.UI.Graphic>()
                    .Where(g => g.transform.parent == layer)
                    .OrderBy(g =>
                    {
                        var canvas = g.GetComponent<UnityEngine.Canvas>();
                        return canvas != null ? canvas.sortingOrder : 0;
                    })
                    .ThenBy(g =>
                    {
                        var rt = g.GetComponent<RectTransform>();
                        if (rt == null) return 0f;
                        // 脚底 Y（与 GetObjectBottomY 口径一致：Position.y + 高度）
                        // 场景锚点 pivot(0,1)：anchoredPosition.y = -Position.y，sizeDelta.y = 高度
                        // 脚底 Y = -anchoredPosition.y + sizeDelta.y，越大越靠下、渲染越靠前
                        float bottomY = -rt.anchoredPosition.y + rt.sizeDelta.y;
                        return bottomY;
                    })
                    .ToList();

                for (int i = 0; i < objects.Count; i++)
                {
                    objects[i].transform.SetSiblingIndex(i);
                    totalSorted++;
                }
            }

            GUI.changed = true;

            EditorUtility.DisplayDialog("刷新层级完成",
                $"已按层级规则重新排序\n排序物件数: {totalSorted}\n\n遮挡关系已优化：\n- 先按层内 SortOrder（手动层级）\n- 再按脚底 Y 坐标（YSort 近处优先）",
                "确定");
        }

        /// <summary>
        /// 预览地图按钮点击
        /// </summary>
        private void OnPreviewClicked()
        {
            // 加载背景图
            Texture bgTexture = LoadRegionBackground(m_CurrentLevelId);

            // 获取所有模板
            var templates = ObjectTemplateManager.Instance.GetAllTemplates();

            // 打开预览窗口，传入当前编辑的地图数据和模板
            MapPreviewWindow.ShowWindow(
                m_PlacedObjects,
                templates,
                bgTexture,
                $"关卡 {m_CurrentLevelId}",
                m_MapWidth,
                m_MapHeight);
        }

        /// <summary>
        /// 保存按钮点击
        /// </summary>
        private void OnSaveClicked()
        {
            Debug.Log("保存地图");
            SaveMapConfig(false);
        }

        /// <summary>
        /// 编辑器每帧更新，请求重绘
        /// </summary>
        private void OnEditorUpdate()
        {
            if (m_GuiContainer == null) return;
            // 请求重绘
            m_GuiContainer.MarkDirtyRepaint();
        }

        /// <summary>
        /// 地图预制体 ObjectField 变化
        /// </summary>
        /// <param name="evt">变化事件</param>
        private void OnMapPrefabFieldChanged(ChangeEvent<Object> evt)
        {
            m_MapPrefab = evt.newValue as GameObject;
            if (m_MapPrefab != null)
            {
                string prefabPath = UnityEditor.AssetDatabase.GetAssetPath(m_MapPrefab);
                Debug.Log($"预制体被修改: {prefabPath}");
                OnMapPrefabChanged?.Invoke(prefabPath);
            }
            else
            {
                OnMapPrefabChanged?.Invoke("");
            }
        }

        /// <summary>
        /// 保存路径按钮点击
        /// </summary>
        private void OnSavePathClicked()
        {
            string path = UnityEditor.EditorUtility.OpenFolderPanel("选择地图保存文件夹", m_MapSavePath, "");
            if (!string.IsNullOrEmpty(path))
            {
                // 转换为相对路径
                if (path.StartsWith(Application.dataPath))
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length);
                }
                m_MapSavePath = path;
                m_StatusBar.Refresh(m_Zoom, m_MousePos, m_TileX, m_TileY, OccupiedCellCount, GridCols, GridRows, m_MapSavePath);
            }
        }

        /// <summary>
        /// 查找UI元素
        /// </summary>
        private void FindElements()
        {
            m_SceneView = m_Root.Q<VisualElement>(className: "scene-view");
            // 状态栏 UI 元素查找（委托给 EditorStatusBar）
            m_StatusBar.FindElements(m_Root);
            // 查找地图预制体 ObjectField 容器
            var prefabRow = m_Root.Q<VisualElement>(name: MapToolUiNames.MapPrefabRow);
            Debug.Log($"查找 map-prefab-row: {prefabRow != null}");
            if (prefabRow != null)
            {
                m_MapPrefabField = new ObjectField();
                m_MapPrefabField.objectType = typeof(GameObject);
                m_MapPrefabField.style.width = 200;
                m_MapPrefabField.RegisterValueChangedCallback(OnMapPrefabFieldChanged);
                prefabRow.Add(m_MapPrefabField);
                Debug.Log("ObjectField 已创建");
            }
            // 查找图层 Toggle（覆盖类：摆放网格/路径网格/地图底图/包围盒；层级八层：背景/绿化/道路/建筑/道具/角色/前景/气泡）
            m_PlaceGridToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerPlaceGrid);
            m_PathGridToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerPathGrid);
            m_MapLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerMap);
            m_BackgroundLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerBackground);
            m_GreeneryLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerGreenery);
            m_RoadLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerRoad);
            m_BuildingLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerBuilding);
            m_ObjectLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerObject);
            m_CharacterLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerCharacter);
            m_ForegroundLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerForeground);
            m_BubblesLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerBubbles);
            m_BoundingBoxToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerBoundingBox);
            // 摆放模式：启用开关 + 绘制形状下拉（矩形/菱形，默认菱形；回显 EditorPrefs 持久化值，切换即落盘）
            m_PlacementEnabledToggle = m_Root.Q<Toggle>(name: MapToolUiNames.PlacementEnabledToggle);
            if (m_PlacementEnabledToggle != null)
            {
                m_PlacementEnabledToggle.value = MapToolConstants.PlacementEnabled;
                m_PlacementEnabledToggle.RegisterValueChangedCallback(evt =>
                {
                    MapToolConstants.SetConfigBool(MapToolConstants.PlacementEnabledPrefsKey, evt.newValue);
                });
            }
            m_DrawShapeField = m_Root.Q<DropdownField>(name: MapToolUiNames.DrawShapeField);
            if (m_DrawShapeField != null)
            {
                m_DrawShapeField.choices = new List<string> { DrawShape.ToDisplay(DrawShape.Rect), DrawShape.ToDisplay(DrawShape.Diamond) };
                m_DrawShape = MapToolConstants.DrawShape;
                m_DrawShapeField.value = DrawShape.ToDisplay(m_DrawShape);
                m_DrawShapeField.RegisterValueChangedCallback(evt =>
                {
                    m_DrawShape = DrawShape.ToCode(evt.newValue);
                    MapToolConstants.SetConfigString(MapToolConstants.DrawShapePrefsKey, m_DrawShape);
                });
            }
            // DrawClear：启用开关 + 区域类型下拉（摆放区/禁区/通用区，默认摆放区；回显 EditorPrefs 持久化值，切换即落盘）
            m_DrawClearEnabledToggle = m_Root.Q<Toggle>(name: MapToolUiNames.DrawClearEnabledToggle);
            if (m_DrawClearEnabledToggle != null)
            {
                m_DrawClearEnabledToggle.value = MapToolConstants.DrawClearEnabled;
                m_DrawClearEnabledToggle.RegisterValueChangedCallback(evt =>
                {
                    MapToolConstants.SetConfigBool(MapToolConstants.DrawClearEnabledPrefsKey, evt.newValue);
                });
            }
            m_RegionTypeField = m_Root.Q<DropdownField>(name: MapToolUiNames.RegionTypeField);
            if (m_RegionTypeField != null)
            {
                m_RegionTypeField.choices = new List<string> { RegionType.ToDisplay(RegionType.Place), RegionType.ToDisplay(RegionType.Forbid), RegionType.ToDisplay(RegionType.Common) };
                m_RegionType = MapToolConstants.RegionType;
                m_RegionTypeField.value = RegionType.ToDisplay(m_RegionType);
                m_RegionTypeField.RegisterValueChangedCallback(evt =>
                {
                    m_RegionType = RegionType.ToCode(evt.newValue);
                    MapToolConstants.SetConfigString(MapToolConstants.RegionTypePrefsKey, m_RegionType);
                });
            }
            // 对象编辑面板（选中人物后显示：可编辑路径 / 编辑动画）
            m_ObjectEditPanel = m_Root.Q<VisualElement>(name: MapToolUiNames.ObjectEditPanel);
            if (m_ObjectEditPanel != null)
            {
                m_ObjEditTitle = m_ObjectEditPanel.Q<Label>(name: MapToolUiNames.ObjEditTitle);
                m_TabEditPath = m_ObjectEditPanel.Q<Button>(name: MapToolUiNames.TabEditPath);
                m_TabEditAnim = m_ObjectEditPanel.Q<Button>(name: MapToolUiNames.TabEditAnim);
                m_PathEditContent = m_ObjectEditPanel.Q<VisualElement>(name: MapToolUiNames.PathEditContent);
                m_AnimEditContent = m_ObjectEditPanel.Q<VisualElement>(name: MapToolUiNames.AnimEditContent);
                m_BtnPathDraw = m_ObjectEditPanel.Q<Button>(name: MapToolUiNames.BtnPathDraw);
                m_BtnPathUndo = m_ObjectEditPanel.Q<Button>(name: MapToolUiNames.BtnPathUndo);
                m_BtnPathClear = m_ObjectEditPanel.Q<Button>(name: MapToolUiNames.BtnPathClear);
                m_BtnClearTarget = m_ObjectEditPanel.Q<Button>(name: MapToolUiNames.BtnClearTarget);
                m_PathPointList = m_ObjectEditPanel.Q<ScrollView>(name: MapToolUiNames.PathPointList);
                // 默认隐藏，选中物体后显示
                m_ObjectEditPanel.style.display = DisplayStyle.None;
            }
            // 动画切页字段
            m_AnimNameField = m_Root.Q<TextField>(name: MapToolUiNames.AnimNameField);
            m_AnimLoopToggle = m_Root.Q<Toggle>(name: MapToolUiNames.AnimLoopToggle);
            m_AnimSpeedField = m_Root.Q<TextField>(name: MapToolUiNames.AnimSpeedField);
            // 底部属性面板字段
            m_TemplateIdField = m_Root.Q<TextField>(name: MapToolUiNames.PropTemplateId);
            m_TagField = m_Root.Q<TextField>(name: MapToolUiNames.PropTag);
            m_GridPosField = m_Root.Q<TextField>(name: MapToolUiNames.PropGridPos);
            m_BuildingTemplateField = m_Root.Q<TextField>(name: MapToolUiNames.PropBuildingTemplate);
            m_GridPrefixField = m_Root.Q<TextField>(name: MapToolUiNames.PropGridPrefix);
            m_InteractStateField = m_Root.Q<TextField>(name: MapToolUiNames.PropInteractState);
            m_DeleteObjectBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnDeleteObject);
            m_PublishBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnPublish);
            m_PropertyPanel = m_Root.Q<VisualElement>(name: MapToolUiNames.PropertyPanel);
            m_PropertyActionArea = m_Root.Q<VisualElement>(name: MapToolUiNames.PropertyActionArea);
            m_PropertyActionHost = m_Root.Q<VisualElement>(name: MapToolUiNames.PropActionHost);
            // 初始未选中任何物件：属性面板与右侧操作卡片均隐藏
            if (m_PropertyPanel != null)
                m_PropertyPanel.style.display = DisplayStyle.None;
            if (m_PropertyActionArea != null)
                m_PropertyActionArea.style.display = DisplayStyle.None;
            if (m_PropertyActionHost != null)
                m_PropertyActionHost.style.display = DisplayStyle.None;
            // 新增属性面板字段
            m_SortOrderField = m_Root.Q<TextField>(name: MapToolUiNames.PropSortOrder);
            // 交互类型下拉框
            var interactContainer = m_Root.Q<VisualElement>(name: MapToolUiNames.PropInteractTypeContainer);
            if (interactContainer != null)
            {
                m_InteractTypeField = new DropdownField();
                m_InteractTypeField.choices = new List<string> { "无", "点击", "对话", "进入", "收集" };
                m_InteractTypeField.value = "无";
                m_InteractTypeField.style.flexGrow = 1;
                m_InteractTypeField.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.InteractType = TemplateInteractType.ToCode(evt.newValue);
                });
                interactContainer.Add(m_InteractTypeField);
            }
            m_CollisionToggle = m_Root.Q<Toggle>(name: MapToolUiNames.PropCollision);
            if (m_CollisionToggle != null)
                m_CollisionToggle.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.CollisionEnabled = evt.newValue;
                });
            // 气泡类型下拉框
            var bubbleContainer = m_Root.Q<VisualElement>(name: MapToolUiNames.PropDialogBubbleContainer);
            if (bubbleContainer != null)
            {
                m_DialogBubbleField = new DropdownField();
                m_DialogBubbleField.choices = new List<string> { "无", "对话", "思考", "收集提示" };
                m_DialogBubbleField.value = "无";
                m_DialogBubbleField.style.flexGrow = 1;
                m_DialogBubbleField.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.DialogBubble = evt.newValue;
                });
                bubbleContainer.Add(m_DialogBubbleField);
            }
            // ---TODO 气泡配置表通用化：气泡ID输入框（引用 Tables.Bubble；<=0 保存时不输出 BubbleId 行）
            var bubbleIdContainer = m_Root.Q<VisualElement>(name: MapToolUiNames.PropBubbleIdContainer);
            if (bubbleIdContainer != null)
            {
                m_BubbleIdField = new IntegerField("");
                m_BubbleIdField.value = 0;
                m_BubbleIdField.style.flexGrow = 1;
                m_BubbleIdField.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.bubbleId = evt.newValue;
                });
                bubbleIdContainer.Add(m_BubbleIdField);
            }
            // 朝向下拉框
            var facingContainer = m_Root.Q<VisualElement>(name: MapToolUiNames.PropFacingContainer);
            if (facingContainer != null)
            {
                m_FacingField = new DropdownField();
                m_FacingField.choices = new List<string> { "上", "下", "左", "右" };
                m_FacingField.value = "下";
                m_FacingField.style.flexGrow = 1;
                m_FacingField.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.FacingDirection = TemplateFacing.ToCode(evt.newValue);
                });
                facingContainer.Add(m_FacingField);
            }
            // 层级档（独立显示层）下拉：八层可选，只改渲染前后档位，不改变素材归属与挂载节点
            var displayLayerContainer = m_Root.Q<VisualElement>(name: MapToolUiNames.PropDisplayLayerContainer);
            if (displayLayerContainer != null)
            {
                m_DisplayLayerField = new DropdownField();
                m_DisplayLayerField.choices = MapLayerCatalog.AllLayerTypes.Select(MapLayerCatalog.GetLayerDisplayName).ToList();
                m_DisplayLayerField.value = MapLayerCatalog.GetLayerDisplayName(MapLayerCatalog.Prop);
                m_DisplayLayerField.style.flexGrow = 1;
                m_DisplayLayerField.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                    {
                        ApplyDisplayLayerChanged(m_EditTarget, MapLayerCatalog.GetLayerKeyByDisplayName(evt.newValue));
                    }
                });
                displayLayerContainer.Add(m_DisplayLayerField);
            }
            m_ShadowToggle = m_Root.Q<Toggle>(name: MapToolUiNames.PropShadow);
            if (m_ShadowToggle != null)
                m_ShadowToggle.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.ShadowEnabled = evt.newValue;
                });
            m_CollectibleGroup = m_Root.Q<VisualElement>(name: MapToolUiNames.PropCollectibleGroup);
            m_TrafficGroup = m_Root.Q<VisualElement>(name: MapToolUiNames.PropTrafficGroup);
            m_CollectibleToggle = m_Root.Q<Toggle>(name: MapToolUiNames.PropCollectible);
            if (m_CollectibleToggle != null)
                m_CollectibleToggle.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.IsCollectible = evt.newValue;
                    // 变换区（层级/比例/角度）仅收集物显示
                    SetCollectibleActionVisible(evt.newValue);
                });
            // 场景信息字段（保存配置时读取）
            m_SceneNameField = m_Root.Q<TextField>(name: MapToolUiNames.SceneNameField);
            m_SceneIdField = m_Root.Q<TextField>(name: MapToolUiNames.SceneIdField);
            m_WidthField = m_Root.Q<TextField>(name: MapToolUiNames.MapWidthField);
            m_HeightField = m_Root.Q<TextField>(name: MapToolUiNames.MapHeightField);
            // 宽高输入框：手动修改后回写地图基准尺寸并同步配置尺寸，网格/吸附/命中按新基准铺满
            if (m_WidthField != null)
                m_WidthField.RegisterValueChangedCallback(OnMapWidthHeightChanged);
            if (m_HeightField != null)
                m_HeightField.RegisterValueChangedCallback(OnMapWidthHeightChanged);
            // 绑定保存路径按钮点击事件（委托给 EditorStatusBar）
            m_StatusBar.BindSavePathClick(OnSavePathClicked);
            // 绑定图层 Toggle 事件（覆盖类 + 层级八层，开关独立控制显隐，不联动放置类型）
            if (m_PlaceGridToggle != null)
                m_PlaceGridToggle.RegisterValueChangedCallback(evt => m_ShowPlaceGrid = evt.newValue);
            if (m_PathGridToggle != null)
                m_PathGridToggle.RegisterValueChangedCallback(evt => m_ShowPathGrid = evt.newValue);
            if (m_MapLayerToggle != null)
                m_MapLayerToggle.RegisterValueChangedCallback(evt => m_ShowMapLayer = evt.newValue);
            if (m_BackgroundLayerToggle != null)
                m_BackgroundLayerToggle.RegisterValueChangedCallback(evt => m_ShowBackgroundLayer = evt.newValue);
            if (m_GreeneryLayerToggle != null)
                m_GreeneryLayerToggle.RegisterValueChangedCallback(evt => m_ShowGreeneryLayer = evt.newValue);
            if (m_RoadLayerToggle != null)
                m_RoadLayerToggle.RegisterValueChangedCallback(evt => m_ShowRoadLayer = evt.newValue);
            if (m_BuildingLayerToggle != null)
                m_BuildingLayerToggle.RegisterValueChangedCallback(evt => m_ShowBuildingLayer = evt.newValue);
            if (m_ObjectLayerToggle != null)
                m_ObjectLayerToggle.RegisterValueChangedCallback(evt => m_ShowObjectLayer = evt.newValue);
            if (m_CharacterLayerToggle != null)
                m_CharacterLayerToggle.RegisterValueChangedCallback(evt => m_ShowCharacterLayer = evt.newValue);
            if (m_ForegroundLayerToggle != null)
                m_ForegroundLayerToggle.RegisterValueChangedCallback(evt => m_ShowForegroundLayer = evt.newValue);
            if (m_BubblesLayerToggle != null)
                m_BubblesLayerToggle.RegisterValueChangedCallback(evt => m_ShowBubblesLayer = evt.newValue);
            if (m_BoundingBoxToggle != null)
                m_BoundingBoxToggle.RegisterValueChangedCallback(evt => m_ShowBoundingBox = evt.newValue);
            // 同层层级操作按钮行（上移/下移/置顶/置底，仅作用于同层 z-order，不跨八层契约）
            if (m_PropertyPanel != null)
            {
                // 操作行挂载到右栏操作卡片；卡片缺失时回退属性流 host，再回退属性面板本体
                var actionHost = m_PropertyActionArea != null ? m_PropertyActionArea : (m_PropertyActionHost != null ? m_PropertyActionHost : m_PropertyPanel);
                // 收集物专属变换容器：仅勾选收集时显示，与交通规则区相互独立
                m_CollectibleAction = new VisualElement();
                m_CollectibleAction.AddToClassList("collectible-action");
                actionHost.Add(m_CollectibleAction);
                // 三行操作（层级/比例/角度）共用 prop-action-row 样式：标签 44px、按钮 flex 均分、统一 4px 间距
                var layerRow = new VisualElement();
                layerRow.AddToClassList("prop-action-row");
                var layerLabel = new Label("层级:");
                layerLabel.AddToClassList("prop-action-label");
                var btnForward = new Button(MoveTargetForward) { text = "上移" };
                var btnBackward = new Button(MoveTargetBackward) { text = "下移" };
                var btnFront = new Button(BringTargetToFront) { text = "置顶" };
                var btnBack = new Button(SendTargetToBack) { text = "置底" };
                btnForward.AddToClassList("prop-action-btn");
                btnBackward.AddToClassList("prop-action-btn");
                btnFront.AddToClassList("prop-action-btn");
                btnBack.AddToClassList("prop-action-btn");
                layerRow.Add(layerLabel);
                layerRow.Add(btnForward);
                layerRow.Add(btnBackward);
                layerRow.Add(btnFront);
                layerRow.Add(btnBack);
                m_CollectibleAction.Add(layerRow);

                // 比例变换按钮行（缩小 / 放大 / 重置），回显当前倍率
                var scaleRow = new VisualElement();
                scaleRow.AddToClassList("prop-action-row");
                var scaleLabel = new Label("比例:");
                scaleLabel.AddToClassList("prop-action-label");
                var btnScaleDown = new Button(() => ScaleTarget(-MapToolConstants.ScaleStep)) { text = "－" };
                m_ScaleLabel = new Label("100%");
                m_ScaleLabel.AddToClassList("prop-action-value");
                var btnScaleUp = new Button(() => ScaleTarget(MapToolConstants.ScaleStep)) { text = "＋" };
                var btnScaleReset = new Button(ResetTargetScale) { text = "重置" };
                btnScaleDown.AddToClassList("prop-action-btn");
                btnScaleUp.AddToClassList("prop-action-btn");
                btnScaleReset.AddToClassList("prop-action-btn");
                btnScaleReset.AddToClassList("secondary");
                scaleRow.Add(scaleLabel);
                scaleRow.Add(btnScaleDown);
                scaleRow.Add(m_ScaleLabel);
                scaleRow.Add(btnScaleUp);
                scaleRow.Add(btnScaleReset);
                m_CollectibleAction.Add(scaleRow);

                // 角度变换按钮行（左旋 / 右旋 / 重置），回显当前角度
                var rotationRow = new VisualElement();
                rotationRow.AddToClassList("prop-action-row");
                var rotationLabel = new Label("角度:");
                rotationLabel.AddToClassList("prop-action-label");
                var btnRotLeft = new Button(() => RotateTarget(-MapToolConstants.RotationStep)) { text = "↺" };
                m_RotationLabel = new Label("0°");
                m_RotationLabel.AddToClassList("prop-action-value");
                var btnRotRight = new Button(() => RotateTarget(MapToolConstants.RotationStep)) { text = "↻" };
                var btnRotReset = new Button(ResetTargetRotation) { text = "重置" };
                btnRotLeft.AddToClassList("prop-action-btn");
                btnRotRight.AddToClassList("prop-action-btn");
                btnRotReset.AddToClassList("prop-action-btn");
                btnRotReset.AddToClassList("secondary");
                rotationRow.Add(rotationLabel);
                rotationRow.Add(btnRotLeft);
                rotationRow.Add(m_RotationLabel);
                rotationRow.Add(btnRotRight);
                rotationRow.Add(btnRotReset);
                m_CollectibleAction.Add(rotationRow);
            }

            // ---TODO 交通规则：代码动态构建交通子面板，挂入右栏卡片变换区下方
            BuildTrafficPanel();

            // 添加IMGUI容器用于地图渲染
            if (m_SceneView != null)
            {
                m_GuiContainer = new IMGUIContainer(OnSceneGUI);
                m_GuiContainer.style.flexGrow = 1;
                m_GuiContainer.style.overflow = Overflow.Hidden;
                m_GuiContainer.style.backgroundColor = Color.clear;
                m_GuiContainer.focusable = false;
                m_SceneView.Add(m_GuiContainer);
            }
        }

        /// <summary>
        /// 切换收集物专属变换区（层级/比例/角度）显隐；仅勾选收集时显示。
        /// </summary>
        /// <param name="visible">true 显示变换区，false 隐藏。</param>
        private void SetCollectibleActionVisible(bool visible)
        {
            if (m_CollectibleAction != null)
                m_CollectibleAction.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            RefreshActionAreaVisibility();
        }

        /// <summary>
        /// 刷新右栏操作卡片显隐：收集变换区或交通配置面板任一可见时显示；两者都不可见时整块隐藏，不留空框。
        /// 卡片内超高内容由 USS 纵向滑动，不撑高下方布局。
        /// </summary>
        private void RefreshActionAreaVisibility()
        {
            bool collectibleVisible = m_EditTarget != null && m_EditTarget.IsCollectible;
            bool show = collectibleVisible || m_TrafficPanelVisible;
            var host = m_PropertyActionArea != null ? m_PropertyActionArea : m_PropertyActionHost;
            if (host != null)
                host.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }

        /// <summary>
        /// 取消选中时强制隐藏右栏操作卡片。
        /// </summary>
        private void SetActionHostVisible(bool visible)
        {
            var host = m_PropertyActionArea != null ? m_PropertyActionArea : m_PropertyActionHost;
            if (host != null)
                host.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        /// <summary>
        /// 删除当前选中的物体
        /// </summary>
        private void OnDeleteSelectedObject()
        {
            if (m_EditTarget != null)
            {
                m_PlacedObjects.Remove(m_EditTarget);
                m_EditTarget = null;
                if (m_ObjectEditPanel != null)
                    m_ObjectEditPanel.style.display = DisplayStyle.None;
                RefreshSceneObjects();
                Debug.Log("删除选中物体");
            }
        }

        #region 同层层级调整（z-order）

        /// <summary>
        /// 当前选中物件在同层内上移一层（与序列中紧邻其上的物件交换次序）
        /// </summary>
        private void MoveTargetForward() => MoveTargetInLayer(forward: true);

        /// <summary>
        /// 当前选中物件在同层内下移一层（与序列中紧邻其下的物件交换次序）
        /// </summary>
        private void MoveTargetBackward() => MoveTargetInLayer(forward: false);

        /// <summary>
        /// 同层置顶：移动到同层序列最末（最上层）
        /// </summary>
        private void BringTargetToFront() => RearrangeTargetInLayer(moveToFront: true);

        /// <summary>
        /// 同层置底：移动到同层序列最前（最下层）
        /// </summary>
        private void SendTargetToBack() => RearrangeTargetInLayer(moveToFront: false);

        /// <summary>
        /// 取与指定物件同有效显示层（ResolveDisplayLayer 解析后的渲染档位）的物件，并按编辑器绘制口径排序：
        /// 第一键显示层槽位基线，第二键 SortOrder 升序，第三键 YSort 底部 Y；序列前部在下层、后部在上层。
        /// 分组口径与挂载节点、绘制排序、运行时 Lua 一致：归属层 LayerType 不变，但同层调整以显示档为准，
        /// 避免归属层不同、显示档相同的物件被拆到不同操作序列。
        /// </summary>
        /// <param name="target">目标物件</param>
        /// <returns>同层已排序物件列表（新实例）</returns>
        private List<PlacedObject> GetSameLayerSorted(PlacedObject target)
        {
            string displayLayer = MapLayerCatalog.ResolveDisplayLayer(target.LayerType, target.DisplayLayer);
            return m_PlacedObjects
                .Where(o => MapLayerCatalog.ResolveDisplayLayer(o.LayerType, o.DisplayLayer) == displayLayer)
                .OrderBy(o => MapToolEditorUtils.GetDisplayBaseline(o))
                .ThenBy(o => o.SortOrder)
                .ThenBy(o => o.YSortEnabled ? MapToolEditorUtils.GetObjectBottomY(o) + o.YSortOffset : 0f)
                .ToList();
        }

        /// <summary>
        /// 同层内前后移动一步：在当前排序序列中与相邻物件交换位置，再按序列次序回写等差 SortOrder。
        /// 注意：同层物件初始 SortOrder 均为层基线等值，直接交换等值排序键不会改变稳定排序结果，
        /// 因此必须以序列位置为准重建次序键，上移/下移才会产生可见的遮挡变化。
        /// </summary>
        /// <param name="forward">true=向上层移动（与序列后一项交换）；false=向下层移动</param>
        private void MoveTargetInLayer(bool forward)
        {
            if (m_EditTarget == null) return;
            var layer = GetSameLayerSorted(m_EditTarget);
            int idx = layer.IndexOf(m_EditTarget);
            if (idx < 0)
            {
                m_StatusBar.ShowMessage("未在当前层找到选中物件，无法调整层级");
                return;
            }

            int swapIdx = forward ? idx + 1 : idx - 1;
            // 已到序列边界：上移到顶 / 下移到底，给出明确提示而非静默忽略
            if (swapIdx < 0)
            {
                m_StatusBar.ShowMessage(forward ? "已在当前层最上层" : "已在当前层最下层");
                return;
            }
            if (swapIdx >= layer.Count)
            {
                m_StatusBar.ShowMessage(forward ? "已在当前层最上层" : "已在当前层最下层");
                return;
            }

            (layer[idx], layer[swapIdx]) = (layer[swapIdx], layer[idx]);
            RebuildLayerSortOrder(layer);
            AfterTargetOrderChanged();
        }

        /// <summary>
        /// 同层置顶 / 置底：把目标移动到序列末位 / 首位，再按序列次序回写等差 SortOrder。
        /// </summary>
        /// <param name="moveToFront">true=置顶（序列末位，最上层）；false=置底（序列首位，最下层）</param>
        private void RearrangeTargetInLayer(bool moveToFront)
        {
            if (m_EditTarget == null) return;
            var layer = GetSameLayerSorted(m_EditTarget);
            if (layer.Count <= 1)
            {
                m_StatusBar.ShowMessage("当前层仅有一个物件，无需调整层级");
                return;
            }

            layer.Remove(m_EditTarget);
            if (moveToFront) layer.Add(m_EditTarget);
            else layer.Insert(0, m_EditTarget);

            RebuildLayerSortOrder(layer);
            AfterTargetOrderChanged();
        }

        /// <summary>
        /// 按同层序列次序回写等差 SortOrder：基线取层目录契约值，次序越靠后 SortOrder 越大；
        /// 每层仅 LayerSlotCount 个独立档位，超出部分钳到最高档同档共享（同档再按脚底 Y 排序），
        /// 避免同层物件过多时位次越界压到相邻层基线。
        /// </summary>
        /// <param name="layerSorted">已按目标次序排好的同层物件序列（前部下层、后部上层）</param>
        private void RebuildLayerSortOrder(List<PlacedObject> layerSorted)
        {
            if (layerSorted == null || layerSorted.Count == 0) return;
            for (int i = 0; i < layerSorted.Count; i++)
            {
                var entry = layerSorted[i];
                int baseline = MapToolEditorUtils.GetDisplayBaseline(entry);
                int slot = Mathf.Min(i, MapToolConstants.LayerSlotCount - 1);
                entry.SortOrder = baseline + slot * MapToolConstants.LayerOrderSeqStep;
            }
        }

        /// <summary>
        /// 层级 / SortOrder 变更后统一收尾：同步场景实例、回填面板输入框、请求重绘
        /// </summary>
        private void AfterTargetOrderChanged()
        {
            RefreshSceneObjects();
            if (m_SortOrderField != null && m_EditTarget != null)
                m_SortOrderField.SetValueWithoutNotify(m_EditTarget.SortOrder.ToString());
            m_GuiContainer?.MarkDirtyRepaint();
            if (m_EditTarget != null)
                Debug.Log($"[层级调整] 物件 {m_EditTarget.Name} 同层 SortOrder={m_EditTarget.SortOrder}");
        }

        /// <summary>
        /// 应用显示档（独立显示层）变更：归属层 LayerType 不变，仅切换渲染挂载档位。
        /// 切换后按新层基线重建合法 SortOrder（保留原层内相对位次），再同步场景重挂载/重排并回填面板。
        /// </summary>
        /// <param name="target">目标物件</param>
        /// <param name="newDisplayLayer">新显示层键（已归一化）</param>
        private void ApplyDisplayLayerChanged(PlacedObject target, string newDisplayLayer)
        {
            if (target == null) return;

            // 记录旧层内相对位次，避免跨档后旧 SortOrder 落在新档区间外压到相邻层基线
            int oldBaseline = MapToolEditorUtils.GetDisplayBaseline(target);
            int oldSlot = Mathf.Clamp(target.SortOrder - oldBaseline, 0, MapToolConstants.LayerSlotCount - 1);

            target.DisplayLayer = string.IsNullOrWhiteSpace(newDisplayLayer) ? string.Empty : newDisplayLayer;

            int newBaseline = MapToolEditorUtils.GetDisplayBaseline(target);
            int newSlot = Mathf.Min(oldSlot, MapToolConstants.LayerSlotCount - 1);
            target.SortOrder = newBaseline + newSlot * MapToolConstants.LayerOrderSeqStep;

            AfterTargetOrderChanged();
            Debug.Log($"[显示档变更] 物件 {target.Name} 显示层={MapLayerCatalog.GetLayerDisplayName(newDisplayLayer)} SortOrder={target.SortOrder}");
        }

        #endregion

        #region 物件变换（比例 / 角度）

        /// <summary>
        /// 调整当前选中物件比例倍率（在当前 Scale 上叠加 delta，夹取到上下限）
        /// </summary>
        /// <param name="delta">倍率增量（正=放大，负=缩小）</param>
        private void ScaleTarget(float delta)
        {
            if (m_EditTarget == null) return;
            m_EditTarget.Scale = Mathf.Clamp(m_EditTarget.Scale + delta, MapToolConstants.ScaleMin, MapToolConstants.ScaleMax);
            AfterTargetTransformChanged();
        }

        /// <summary>重置当前选中物件比例为原始倍率 1</summary>
        private void ResetTargetScale()
        {
            if (m_EditTarget == null) return;
            m_EditTarget.Scale = 1f;
            AfterTargetTransformChanged();
        }

        /// <summary>
        /// 旋转当前选中物件（叠加角度增量，归一化到 [-180, 180)）
        /// </summary>
        /// <param name="delta">角度增量（度，正=顺时针）</param>
        private void RotateTarget(float delta)
        {
            if (m_EditTarget == null) return;
            float angle = (m_EditTarget.Rotation + delta) % 360f;
            if (angle > 180f) angle -= 360f;
            if (angle < -180f) angle += 360f;
            m_EditTarget.Rotation = angle;
            AfterTargetTransformChanged();
        }

        /// <summary>重置当前选中物件角度为 0</summary>
        private void ResetTargetRotation()
        {
            if (m_EditTarget == null) return;
            m_EditTarget.Rotation = 0f;
            AfterTargetTransformChanged();
        }

        /// <summary>
        /// 比例 / 角度变更后统一收尾：同步场景实例、刷新回显标签、请求重绘
        /// </summary>
        private void AfterTargetTransformChanged()
        {
            RefreshSceneObjects();
            UpdateTransformLabels();
            m_GuiContainer?.MarkDirtyRepaint();
        }

        /// <summary>刷新比例 / 角度回显标签（无选中目标时复位默认显示）</summary>
        private void UpdateTransformLabels()
        {
            if (m_ScaleLabel != null)
                m_ScaleLabel.text = m_EditTarget != null ? $"{Mathf.RoundToInt(m_EditTarget.Scale * 100f)}%" : "100%";
            if (m_RotationLabel != null)
                m_RotationLabel.text = m_EditTarget != null ? $"{Mathf.Round(m_EditTarget.Rotation)}°" : "0°";
        }

        #endregion

        /// <summary>
        /// 发布按钮点击
        /// </summary>
        private void OnPublishClicked()
        {
            Debug.Log("发布地图配置");
            SaveMapConfig(true);
        }

        /// <summary>
        /// 清空摆放物按钮点击
        /// </summary>
        private void OnClearObjectsClicked()
        {
            if (EditorUtility.DisplayDialog("清空摆放物", "确定要清空所有已摆放的物体吗？此操作不可撤销！", "确定", "取消"))
            {
                m_PlacedObjects.Clear();
                ClearTrafficWorkingData();
                m_EditTarget = null;
                if (m_ObjectEditPanel != null)
                    m_ObjectEditPanel.style.display = DisplayStyle.None;
                RefreshSceneObjects();
                Debug.Log("已清空所有摆放物");
            }
        }

        /// <summary>
        /// 清空格子数据按钮点击
        /// </summary>
        private void OnClearGridDataClicked()
        {
            if (EditorUtility.DisplayDialog("清空格子数据", "确定要清空格子数据吗？此操作不可撤销！", "确定", "取消"))
            {
                // TODO: 清空格子数据（迷雾、格子标记等）
                Debug.Log("已清空格子数据");
            }
        }

        /// <summary>
        /// 导出设置按钮点击
        /// </summary>
        private void OnExportSettingsClicked()
        {
            Debug.Log("导出设置");
            // TODO: 导出地图配置设置
        }

    }
}
