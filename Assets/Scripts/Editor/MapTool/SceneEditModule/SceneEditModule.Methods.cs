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
            // 导入图片按钮（从 PNG 序列自动生成地图）
            var importImagesBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnImportImages);
            if (importImagesBtn != null)
                importImagesBtn.clicked += OnImportImagesClicked;
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

            m_StatusBar.Refresh(m_Zoom, m_MousePos, m_TileX, m_TileY, m_MapSavePath);
            m_StatusBar.ShowMessage($"已从配置文件重新加载地图（摆放物 {m_PlacedObjects.Count}）");
            string currentBg = m_LevelBgDict.TryGetValue(m_CurrentLevelId, out string bg) ? bg : "<空>";
            Debug.Log($"[更新地图] 已从 {m_MapSavePath} 重新加载，摆放物 {m_PlacedObjects.Count}，宽高 {m_MapWidth}x{m_MapHeight}，背景 {currentBg}");
        }

        /// <summary>
        /// <summary>
        /// 导出切片按钮点击：导出所有物件图片到对应文件夹，按章节/关卡/类型分类
        /// </summary>
        private void OnExportSliceClicked()
        {
            if (m_PlacedObjects.Count == 0)
            {
                EditorUtility.DisplayDialog("导出切片", "当前地图没有任何物件", "确定");
                return;
            }

            // 1. 获取章节名和关卡名
            string chapterName = m_CurrentChapterData != null && !string.IsNullOrEmpty(m_CurrentChapterData.ChapterTitle)
                ? m_CurrentChapterData.ChapterTitle
                : (string.IsNullOrEmpty(m_CurrentChapterId) ? "DefaultChapter" : m_CurrentChapterId);
            string levelName = string.IsNullOrEmpty(m_CurrentLevelId) ? "DefaultLevel" : m_CurrentLevelId;

            // 2. 创建输出目录：Assets/Res/Textures/Map/{章节名}/{关卡名}/{类型}/
            string baseDir = $"Assets/Res/Textures/Map/{chapterName}/{levelName}";
            string buildingDir = $"{baseDir}/Building";
            string characterDir = $"{baseDir}/Character";
            string propDir = $"{baseDir}/Prop";
            string catDir = $"{baseDir}/Cat";

            // 确保目录存在
            foreach (var dir in new[] { buildingDir, characterDir, propDir, catDir })
            {
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

                // 只导出带前缀的物件（bg_/building_/char_/prop_/cat_）
                string lowerName = obj.Name.ToLower();
                bool hasPrefix = lowerName.StartsWith("bg_")
                    || lowerName.StartsWith("building_")
                    || lowerName.StartsWith("char_")
                    || lowerName.StartsWith("prop_")
                    || lowerName.StartsWith("cat_");

                // 带中文名字的跳过
                bool hasChinese = System.Text.RegularExpressions.Regex.IsMatch(obj.Name, @"[\u4e00-\u9fff]");

                if (!hasPrefix || hasChinese)
                {
                    skippedCount++;
                    continue;
                }

                // 确定输出目录
                string targetDir = propDir;
                switch (obj.Type)
                {
                    case MapObjectType.Building: targetDir = buildingDir; break;
                    case MapObjectType.Character: targetDir = characterDir; break;
                    case MapObjectType.Cat: targetDir = catDir; break;
                    case MapObjectType.Prop: targetDir = propDir; break;
                }

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
                $"章节: {chapterName}\n关卡: {levelName}\n\n导出物件数: {exportedCount}\n跳过（无前缀）: {skippedCount}\n保存目录: {baseDir}\n\n按类型分类:\n  Building/Character/Prop/Cat/",
                "确定");
        }

        /// <summary>
        /// <summary>
        /// 扫描未配置资源按钮点击
        /// </summary>
        private void OnScanResourcesClicked()
        {
            string mapDir = "Assets/Res/Textures/Map";
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

            // 调整层级节点顺序：Buildings（最底层） → Objects → Characters → Cats（最上层）
            string[] layerOrder = { "Buildings", "Objects", "Characters", "Cats" };
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

            // 每个层级节点内部按左上角 Y 坐标排序（YSort 规则）
            int totalSorted = 0;
            foreach (var layerName in layerOrder)
            {
                var layer = FindChildRecursive(zoneRoot.transform, layerName);
                if (layer == null) continue;

                var objects = layer.GetComponentsInChildren<UnityEngine.UI.RawImage>()
                    .Where(r => r.transform.parent == layer)
                    .OrderBy(r =>
                    {
                        var rt = r.GetComponent<RectTransform>();
                        // 按左上角 Y 坐标排序（YSort 规则）
                        // 左上角 Y 越小越靠上，渲染在下层；越大越靠下，渲染在上层
                        float topY = -rt.anchoredPosition.y;
                        return topY;
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
                $"已按 YSort 规则重新排序\n排序物件数: {totalSorted}\n\n遮挡关系已优化：\n- 按左上角 Y 坐标排序\n- Y 越小越靠上，渲染在下层\n- Y 越大越靠下，渲染在上层",
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
            SaveMapConfig();
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
                m_StatusBar.Refresh(m_Zoom, m_MousePos, m_TileX, m_TileY, m_MapSavePath);
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
            // 查找图层 Toggle
            m_PlaceGridToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerPlaceGrid);
            m_MapLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerMap);
            m_BuildingLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerBuilding);
            m_CharacterLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerCharacter);
            m_ObjectLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerObject);
            m_CatLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerCat);
            m_BackgroundLayerToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerBackground);
            m_PathGridToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerPathGrid);
            m_BoundingBoxToggle = m_Root.Q<Toggle>(name: MapToolUiNames.LayerBoundingBox);
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
                        m_EditTarget.InteractType = evt.newValue;
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
                        m_EditTarget.FacingDirection = evt.newValue;
                });
                facingContainer.Add(m_FacingField);
            }
            m_ShadowToggle = m_Root.Q<Toggle>(name: MapToolUiNames.PropShadow);
            if (m_ShadowToggle != null)
                m_ShadowToggle.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.ShadowEnabled = evt.newValue;
                });
            m_CollectibleToggle = m_Root.Q<Toggle>(name: MapToolUiNames.PropCollectible);
            if (m_CollectibleToggle != null)
                m_CollectibleToggle.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.IsCollectible = evt.newValue;
                });
            // 场景信息字段（保存配置时读取）
            m_SceneNameField = m_Root.Q<TextField>(name: MapToolUiNames.SceneNameField);
            m_SceneIdField = m_Root.Q<TextField>(name: MapToolUiNames.SceneIdField);
            m_WidthField = m_Root.Q<TextField>(name: MapToolUiNames.MapWidthField);
            m_HeightField = m_Root.Q<TextField>(name: MapToolUiNames.MapHeightField);
            // 绑定保存路径按钮点击事件（委托给 EditorStatusBar）
            m_StatusBar.BindSavePathClick(OnSavePathClicked);
            // 绑定图层 Toggle 事件
            if (m_PlaceGridToggle != null)
                m_PlaceGridToggle.RegisterValueChangedCallback(evt => m_ShowPlaceGrid = evt.newValue);
            if (m_MapLayerToggle != null)
                m_MapLayerToggle.RegisterValueChangedCallback(evt => m_ShowMapLayer = evt.newValue);
            if (m_BuildingLayerToggle != null)
                m_BuildingLayerToggle.RegisterValueChangedCallback(evt =>
                {
                    m_ShowBuildingLayer = evt.newValue;
                    // 勾选时联动切换放置类型（取消勾选不改变放置类型，只隐藏显示）
                    if (evt.newValue) SetPlaceType(0);
                });
            if (m_CharacterLayerToggle != null)
                m_CharacterLayerToggle.RegisterValueChangedCallback(evt =>
                {
                    m_ShowCharacterLayer = evt.newValue;
                    if (evt.newValue) SetPlaceType(1);
                });
            if (m_ObjectLayerToggle != null)
                m_ObjectLayerToggle.RegisterValueChangedCallback(evt =>
                {
                    m_ShowObjectLayer = evt.newValue;
                    if (evt.newValue) SetPlaceType(2);
                });
            if (m_CatLayerToggle != null)
                m_CatLayerToggle.RegisterValueChangedCallback(evt =>
                {
                    m_ShowCatLayer = evt.newValue;
                    if (evt.newValue) SetPlaceType(3);
                });
            if (m_BackgroundLayerToggle != null)
                m_BackgroundLayerToggle.RegisterValueChangedCallback(evt => m_ShowBackgroundLayer = evt.newValue);
            if (m_PathGridToggle != null)
                m_PathGridToggle.RegisterValueChangedCallback(evt => m_ShowPathGrid = evt.newValue);
            if (m_BoundingBoxToggle != null)
                m_BoundingBoxToggle.RegisterValueChangedCallback(evt => m_ShowBoundingBox = evt.newValue);
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
        /// 删除当前选中的物体
        /// </summary>
        private void OnDeleteSelectedObject()
        {
            if (m_EditTarget != null)
            {
                m_PlacedObjects.Remove(m_EditTarget);
                m_EditTarget = null;
                Debug.Log("删除选中物体");
            }
        }

        /// <summary>
        /// 发布按钮点击
        /// </summary>
        private void OnPublishClicked()
        {
            Debug.Log("发布地图配置");
            SaveMapConfig();
        }

        /// <summary>
        /// 清空摆放物按钮点击
        /// </summary>
        private void OnClearObjectsClicked()
        {
            if (EditorUtility.DisplayDialog("清空摆放物", "确定要清空所有已摆放的物体吗？此操作不可撤销！", "确定", "取消"))
            {
                m_PlacedObjects.Clear();
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
        /// 导入图片按钮点击：从 PNG 序列目录自动生成地图
        /// 命名规范：bg_背景图 / building_建筑 / char_人物 / prop_物件
        /// </summary>
        private void OnImportImagesClicked()
        {
            string dirPath = EditorUtility.OpenFolderPanel("选择 PNG 序列目录", "Assets/Res/Textures/Map", "");
            if (string.IsNullOrEmpty(dirPath)) return;

            // 转换为 Assets 相对路径
            string assetDir = MapToolPathHelper.ToAssetRelative(dirPath);
            if (!assetDir.StartsWith("Assets/"))
            {
                EditorUtility.DisplayDialog("导入失败", "请选择 Assets 目录下的文件夹", "确定");
                return;
            }

            MapImageImportResult result = MapImageImporter.ImportFromDirectory(assetDir);
            if (result == null)
            {
                EditorUtility.DisplayDialog("导入失败", "目录下没有有效的 PNG 文件\n（需要 bg_/building_/char_/prop_ 前缀）", "确定");
                return;
            }

            // 应用导入结果到编辑器
            ApplyImageImport(result);

            EditorUtility.DisplayDialog("导入完成",
                $"背景图: {result.BackgroundName}\n物件数量: {result.Objects.Count}\n地图尺寸: {result.MapWidth}x{result.MapHeight}\n\n注意：物件位置默认错开排列，请手动调整",
                "确定");
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
