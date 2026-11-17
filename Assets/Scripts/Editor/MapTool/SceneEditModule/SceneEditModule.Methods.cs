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
using Action = System.Action;
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
            // 工具栏按钮（元素名 → 处理器，统一注册，避免逐条重复判空）
            BindClickButton(MapToolUiNames.BtnUpdateMap, OnUpdateMapClicked);
            BindClickButton(MapToolUiNames.BtnExportSlice, OnExportSliceClicked);
            BindClickButton(MapToolUiNames.BtnSave, OnSaveClicked);
            BindClickButton(MapToolUiNames.BtnPreviewMap, OnPreviewClicked);
            BindClickButton(MapToolUiNames.BtnRefreshLayer, OnRefreshLayerClicked);
            BindClickButton(MapToolUiNames.BtnScanResources, OnScanResourcesClicked);
            BindClickButton(MapToolUiNames.BtnClearObjects, OnClearObjectsClicked);
            BindClickButton(MapToolUiNames.BtnClearGridData, OnClearGridDataClicked);
            BindClickButton(MapToolUiNames.BtnImportPsd, () => m_PsdImportModule.ImportPsd());
            BindClickButton(MapToolUiNames.BtnExportSettings, OnExportSettingsClicked);

            // 对象编辑面板：切页与路径绘制工具栏
            BindClick(m_TabEditPath, OnTabEditPathClicked);
            BindClick(m_TabEditAnim, OnTabEditAnimClicked);
            BindClick(m_BtnPathDraw, OnPathDrawClicked);
            BindClick(m_BtnPathUndo, OnPathUndoClicked);
            BindClick(m_BtnPathClear, OnPathClearClicked);
            BindClick(m_BtnClearTarget, OnClearTargetClicked);

            // 动画切页字段变更 → 写回当前编辑目标
            BindAnimFieldCallbacks();

            // 层级 SortOrder 输入框变更 → 写回当前编辑目标（同层 z-order，数值越大越靠前）
            // 钳制到所属显示层档位区间，避免手填越界值压过相邻层基线；变更后走统一收尾同步场景
            if (m_SortOrderField != null)
                m_SortOrderField.RegisterValueChangedCallback(evt => OnSortOrderFieldChanged(evt.newValue));

            // 属性面板按钮事件
            BindClick(m_DeleteObjectBtn, OnDeleteSelectedObject);
            BindClick(m_PublishBtn, OnPublishClicked);
        }

        /// <summary>
        /// 按元素名注册按钮点击（元素缺失时静默跳过）
        /// </summary>
        /// <param name="elementName">元素名（MapToolUiNames 常量）</param>
        /// <param name="handler">点击处理器</param>
        private void BindClickButton(string elementName, Action handler)
        {
            BindClick(m_Root.QByName<Button>(elementName), handler);
        }

        /// <summary>
        /// 注册按钮点击（按钮为空时静默跳过）
        /// </summary>
        /// <param name="button">目标按钮</param>
        /// <param name="handler">点击处理器</param>
        private static void BindClick(Button button, Action handler)
        {
            if (button == null || handler == null) return;
            button.clicked += handler;
        }

        /// <summary>
        /// 绑定动画切页字段（动画名 / 循环 / 速度）变更 → 回写当前编辑目标
        /// </summary>
        private void BindAnimFieldCallbacks()
        {
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
        }

        /// <summary>
        /// 层级 SortOrder 输入变更：解析并钳制到档位区间后同步场景
        /// </summary>
        /// <param name="newValue">输入文本</param>
        private void OnSortOrderFieldChanged(string newValue)
        {
            if (m_EditTarget == null) return;
            if (!int.TryParse(newValue, out int order)) return;

            // ---TODO [层级档位] 越界值钳制到 [基线, 基线+LayerSlotCount-1]，与运行时同层契约一致
            m_EditTarget.SortOrder = MapToolEditorUtils.ClampSortOrderToSlot(m_EditTarget, order);
            AfterTargetOrderChanged();
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
                MapToolDialog.Info("更新地图", "请先在左侧章节/关卡列表中选择一个关卡，再点击更新地图。", "知道了");
                return;
            }

            // 2. 二次确认：编辑器内未保存的摆放会被磁盘 JSON 覆盖
            bool confirm = MapToolDialog.Confirm(
                "更新地图",
                $"将从磁盘配置文件重新加载地图：\n{m_MapSavePath}\n\n" +
                $"当前编辑器中的未保存修改（摆放物、宽高调整等）会被丢弃，是否继续？",
                "重新加载", MapToolDialog.ButtonCancel);
            if (!confirm)
            {
                m_StatusBar.ShowMessage("已取消更新地图");
                return;
            }

            // 3. 拉取磁盘最新文件（外部工具/脚本改过 JSON/配置表/PNG 时，Unity 缓存可能是旧的）
            AssetDatabase.Refresh();

            // 4. 清空当前选中态与编辑面板
            m_EditTarget = null;
            m_ObjectEditPanel.SetVisible(false);

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
            SyncConfigMapSize();

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
            MapToolLog.Info($"[更新地图] 已从 {m_MapSavePath} 重新加载，摆放物 {m_PlacedObjects.Count}，宽高 {m_MapWidth}x{m_MapHeight}，背景 {currentBg}");
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
            SyncConfigMapSize();
            m_GuiContainer?.MarkDirtyRepaint();
        }

        /// <summary>
        /// 导出切片按钮点击：导出所有物件图片到对应文件夹，按章节/关卡/八层类型分类
        /// </summary>
        private void OnExportSliceClicked()
        {
            if (m_PlacedObjects.Count == 0)
            {
                MapToolDialog.Info("导出切片", "当前地图没有任何物件");
                return;
            }

            // 1. 章节/关卡 ID：缺省时兜底，用于三级目录 Chapter{id}/Level{lv}/<Layer>
            string chapterId = string.IsNullOrEmpty(m_CurrentChapterId) ? MapToolConstants.DefaultChapterId : m_CurrentChapterId;
            string levelId = string.IsNullOrEmpty(m_CurrentLevelId) ? MapToolConstants.DefaultLevelId : m_CurrentLevelId;

            // 2. 输出目录（八层子目录对齐 MapLayerCatalog）
            string baseDir = EnsureSliceLayerDirs(chapterId, levelId);

            // 3. 导出所有物件（同名去重）
            int exportedCount = 0;
            int skippedCount = 0;
            var exportedNames = new HashSet<string>();

            foreach (var obj in m_PlacedObjects)
            {
                if (obj.PreviewTexture == null) continue;

                if (!IsExportableObject(obj))
                {
                    skippedCount++;
                    continue;
                }

                string fileName = obj.Name.Replace(" ", "_").Replace("/", "_");
                if (!exportedNames.Add(fileName)) continue;

                string targetDir = MapLayerCatalog.GetLevelDir(chapterId, levelId, obj.LayerType);
                if (ExportObjectPng(obj, targetDir, fileName)) exportedCount++;
            }

            // 4. 刷新 AssetDatabase
            AssetDatabase.Refresh();

            MapToolDialog.Info("导出切片完成", $"章节: Chapter{chapterId}\n关卡: Level{levelId}\n\n导出物件数: {exportedCount}\n跳过（无前缀）: {skippedCount}\n保存目录: {baseDir}\n\n按层级分类:\n  Background/Greenery/Road/Buildings/Props/Characters/Foreground/Bubbles");
        }

        /// <summary>
        /// 确保八层导出目录存在并返回章节/关卡基目录
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="levelId">关卡ID</param>
        /// <returns>基目录（按章节/关卡分级的层级目录对齐 MapLayerCatalog）</returns>
        private static string EnsureSliceLayerDirs(string chapterId, string levelId)
        {
            string baseDir = $"{MapToolConstants.MapTextureRoot}/Chapter{chapterId}/Level{levelId}";

            foreach (var layerType in MapLayerCatalog.AllLayerTypes)
            {
                string dir = MapLayerCatalog.GetLevelDir(chapterId, levelId, layerType);
                string fullPath = MapToolPathHelper.ToAbsolutePath(dir);
                if (!System.IO.Directory.Exists(fullPath))
                    System.IO.Directory.CreateDirectory(fullPath);
            }

            return baseDir;
        }

        /// <summary>
        /// 判断物件是否可导出：需带美术命名前缀，且名字不含中文
        /// </summary>
        /// <param name="obj">摆放物</param>
        /// <returns>可导出返回 true</returns>
        private static bool IsExportableObject(PlacedObject obj)
        {
            // 前缀全集见 MapToolConstants.ArtPrefixes
            bool hasPrefix = MapToolConstants.HasArtPrefix(obj.Name);
            bool hasChinese = System.Text.RegularExpressions.Regex.IsMatch(obj.Name, @"[\u4e00-\u9fff]");
            return hasPrefix && !hasChinese;
        }

        /// <summary>
        /// 导出一个物件的 PNG：优先复制源文件，失败时走 RenderTexture 编码
        /// </summary>
        /// <param name="obj">摆放物（含预览纹理）</param>
        /// <param name="targetDir">目标目录（资产相对路径）</param>
        /// <param name="fileName">输出文件名（不含扩展名）</param>
        /// <returns>导出成功返回 true</returns>
        private static bool ExportObjectPng(PlacedObject obj, string targetDir, string fileName)
        {
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
                        return true;
                    }
                }

                // 方案2：AssetDatabase 找不到源文件时，用 EncodeToPNG
                if (!(obj.PreviewTexture is Texture2D srcTex))
                {
                    MapToolLog.Warn($"[导出切片] {obj.Name} 不是 Texture2D，跳过");
                    return false;
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

                System.IO.File.WriteAllBytes(fullPath, readableTex.EncodeToPNG());
                return true;
            }
            catch (System.Exception e)
            {
                MapToolLog.Warn($"[导出切片] 导出 {obj.Name} 失败: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// 扫描未配置资源按钮点击
        /// </summary>
        private void OnScanResourcesClicked()
        {
            string mapDir = MapToolConstants.ResourceScanDir;
            if (!AssetDatabase.IsValidFolder(mapDir))
            {
                MapToolDialog.Info("扫描资源", $"目录不存在：{mapDir}");
                return;
            }

            // 扫描所有 PNG 图片（统一走 MapToolEditorUtils.ScanTextureAssets）
            var allResources = MapToolEditorUtils.ScanTextureAssets(mapDir, MapToolConstants.ExtPng);

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
            var listContainer = m_Root.QByName<VisualElement>(MapToolUiNames.UnconfiguredResourcesList);
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

            MapToolDialog.Info("扫描完成", $"扫描目录: {mapDir}\n总资源数: {allResources.Count}\n未配置数: {unconfigured.Count}");
        }

        /// <summary>
        /// 显示资源详情
        /// </summary>
        private void ShowResourceDetail(string path)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
            var nameLabel = m_Root.QByName<Label>(MapToolUiNames.ResourceDetailName);
            var typeLabel = m_Root.QByName<Label>(MapToolUiNames.ResourceDetailType);
            var pathLabel = m_Root.QByName<Label>(MapToolUiNames.ResourceDetailPath);
            var sizeLabel = m_Root.QByName<Label>(MapToolUiNames.ResourceDetailSize);

            if (nameLabel != null) nameLabel.text = fileName;
            if (pathLabel != null) pathLabel.text = path;

            // 自动识别类型（前缀口径统一走公共映射）
            string autoType = MapToolEditorUtils.GetResourceTypeDisplayName(fileName);

            if (typeLabel != null) typeLabel.text = autoType;

            // 显示尺寸
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (tex != null && sizeLabel != null)
            {
                sizeLabel.text = $"{tex.width} x {tex.height}";
            }
        }

        private void OnRefreshLayerClicked()
        {
            if (m_PlacedObjects == null || m_PlacedObjects.Count == 0)
            {
                MapToolDialog.Info("刷新层级", "当前地图没有任何物件");
                return;
            }

            // 找到当前地图节点
            var zoneRoot = GameObject.Find(MapSceneNodeNames.MapInstance);
            if (zoneRoot == null)
            {
                MapToolDialog.Info("刷新层级", $"找不到 {MapSceneNodeNames.MapInstance} 节点");
                return;
            }

            // 调整层级节点顺序：Background → Greenery → Road → Buildings → Props → Characters → Foreground → Bubbles（最上层）
            string[] layerOrder = MapLayerCatalog.AllLayerTypes
                .Select(MapLayerCatalog.GetLayerDir)
                .ToArray();

            ReorderLayerSiblings(zoneRoot.transform, layerOrder);
            int totalSorted = SortGraphicsWithinLayers(zoneRoot.transform, layerOrder);

            GUI.changed = true;

            MapToolDialog.Info("刷新层级完成", $"已按层级规则重新排序\n排序物件数: {totalSorted}\n\n遮挡关系已优化：\n- 先按层内 SortOrder（手动层级）\n- 再按脚底 Y 坐标（YSort 近处优先）");
        }

        /// <summary>
        /// 按层序调整 CoordAdapter 下各层节点的同级顺序
        /// </summary>
        /// <param name="mapRootTransform">地图根节点</param>
        /// <param name="layerOrder">层节点名序列（底层在前）</param>
        private void ReorderLayerSiblings(Transform mapRootTransform, string[] layerOrder)
        {
            var coordAdapter = FindChildRecursive(mapRootTransform, MapSceneNodeNames.CoordAdapter);
            if (coordAdapter == null) return;

            for (int i = 0; i < layerOrder.Length; i++)
            {
                var layer = FindChildRecursive(coordAdapter, layerOrder[i]);
                if (layer != null) layer.SetSiblingIndex(i);
            }
        }

        /// <summary>
        /// 每个层节点内部按 SortOrder + 脚底 Y 排序（与场景同步段、IMGUI 绘制口径一致）
        /// 收集 Graphic 以同时覆盖 Image 与 RawImage；第一键独立 Canvas 的 sortingOrder（手动 z-order），
        /// 第二键脚底 Y（YSort 近处优先），避免刷新层级把手动层级位次冲掉
        /// </summary>
        /// <param name="mapRootTransform">地图根节点</param>
        /// <param name="layerOrder">层节点名序列</param>
        /// <returns>实际排序的物件数</returns>
        private int SortGraphicsWithinLayers(Transform mapRootTransform, string[] layerOrder)
        {
            var coordAdapter = FindChildRecursive(mapRootTransform, MapSceneNodeNames.CoordAdapter);
            if (coordAdapter == null) return 0;

            int totalSorted = 0;
            foreach (var layerName in layerOrder)
            {
                var layer = FindChildRecursive(coordAdapter, layerName);
                if (layer == null) continue;

                var objects = layer.GetComponentsInChildren<UnityEngine.UI.Graphic>()
                    .Where(g => g.transform.parent == layer)
                    .OrderBy(g => GetGraphicSortingOrder(g))
                    .ThenBy(g => GetGraphicBottomY(g))
                    .ToList();

                for (int i = 0; i < objects.Count; i++)
                {
                    objects[i].transform.SetSiblingIndex(i);
                    totalSorted++;
                }
            }
            return totalSorted;
        }

        /// <summary>
        /// 取节点自身 Canvas 的 sortingOrder（无 Canvas 时为 0）
        /// </summary>
        /// <param name="graphic">图形节点</param>
        /// <returns>排序值</returns>
        private static int GetGraphicSortingOrder(UnityEngine.UI.Graphic graphic)
        {
            var canvas = graphic.GetComponent<UnityEngine.Canvas>();
            return canvas != null ? canvas.sortingOrder : 0;
        }

        /// <summary>
        /// 取节点脚底 Y（与 GetObjectBottomY 口径一致：Position.y + 高度）
        /// 场景锚点 pivot(0,1)：anchoredPosition.y = -Position.y，sizeDelta.y = 高度
        /// 脚底 Y = -anchoredPosition.y + sizeDelta.y，越大越靠下、渲染越靠前
        /// </summary>
        /// <param name="graphic">图形节点</param>
        /// <returns>脚底 Y 坐标</returns>
        private static float GetGraphicBottomY(UnityEngine.UI.Graphic graphic)
        {
            var rect = graphic.GetComponent<RectTransform>();
            return rect == null ? 0f : -rect.anchoredPosition.y + rect.sizeDelta.y;
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
            MapToolLog.Info("保存地图");
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
                MapToolLog.Info($"预制体被修改: {prefabPath}");
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
                // 转换为相对路径（路径口径统一走 MapToolPathHelper）
                m_MapSavePath = MapToolPathHelper.ToAssetRelative(path);
                m_StatusBar.Refresh(m_Zoom, m_MousePos, m_TileX, m_TileY, OccupiedCellCount, GridCols, GridRows, m_MapSavePath);
            }
        }

        /// <summary>
        /// 查找UI元素
        /// </summary>
        private void FindElements()
        {
            FindCoreElements();
            FindObjectEditPanelElements();
            FindPropertyFieldElements();
            FindSceneInfoElements();
            BindLayerToggleEvents();
            BuildPropertyActionRows();
            BuildSceneView();
        }

        private void FindCoreElements()
        {
            FindCoreElementRefs();
            FindLayerToggles();
            BindPlacementModeControls();
            BindDrawClearControls();
        }

        /// <summary>
        /// 查找核心容器与状态栏元素
        /// </summary>
        private void FindCoreElementRefs()
        {
            m_SceneView = m_Root.Q<VisualElement>(className: MapToolStyleClasses.SceneView);

            // 状态栏 UI 元素查找（委托给 EditorStatusBar）
            m_StatusBar.FindElements(m_Root);

            // 地图预制体 ObjectField 容器
            var prefabRow = m_Root.QByName<VisualElement>(MapToolUiNames.MapPrefabRow);
            if (prefabRow == null) return;

            m_MapPrefabField = new ObjectField
            {
                objectType = typeof(GameObject),
                style = { width = 200 }
            };
            m_MapPrefabField.RegisterValueChangedCallback(OnMapPrefabFieldChanged);
            prefabRow.Add(m_MapPrefabField);
        }

        /// <summary>
        /// 查找图层 Toggle（覆盖类：摆放网格/路径网格/地图底图/包围盒；层级八层：背景/绿化/道路/建筑/道具/角色/前景/气泡）
        /// </summary>
        private void FindLayerToggles()
        {
            m_PlaceGridToggle = m_Root.QByName<Toggle>(MapToolUiNames.LayerPlaceGrid);
            m_PathGridToggle = m_Root.QByName<Toggle>(MapToolUiNames.LayerPathGrid);
            m_MapLayerToggle = m_Root.QByName<Toggle>(MapToolUiNames.LayerMap);
            m_BackgroundLayerToggle = m_Root.QByName<Toggle>(MapToolUiNames.LayerBackground);
            m_GreeneryLayerToggle = m_Root.QByName<Toggle>(MapToolUiNames.LayerGreenery);
            m_RoadLayerToggle = m_Root.QByName<Toggle>(MapToolUiNames.LayerRoad);
            m_BuildingLayerToggle = m_Root.QByName<Toggle>(MapToolUiNames.LayerBuilding);
            m_ObjectLayerToggle = m_Root.QByName<Toggle>(MapToolUiNames.LayerObject);
            m_CharacterLayerToggle = m_Root.QByName<Toggle>(MapToolUiNames.LayerCharacter);
            m_ForegroundLayerToggle = m_Root.QByName<Toggle>(MapToolUiNames.LayerForeground);
            m_BubblesLayerToggle = m_Root.QByName<Toggle>(MapToolUiNames.LayerBubbles);
            m_BoundingBoxToggle = m_Root.QByName<Toggle>(MapToolUiNames.LayerBoundingBox);
        }

        /// <summary>
        /// 摆放模式：启用开关 + 绘制形状下拉（矩形/菱形，默认菱形；回显 EditorPrefs 持久化值，切换即落盘）
        /// </summary>
        private void BindPlacementModeControls()
        {
            m_PlacementEnabledToggle = m_Root.QByName<Toggle>(MapToolUiNames.PlacementEnabledToggle);
            BindPrefsBackedToggle(m_PlacementEnabledToggle, MapToolConstants.PlacementEnabled,
                MapToolConstants.PlacementEnabledPrefsKey);

            m_DrawShape = MapToolConstants.DrawShape;
            m_DrawShapeField = m_Root.QByName<DropdownField>(MapToolUiNames.DrawShapeField);
            BindPrefsBackedDropdown(m_DrawShapeField,
                new List<string> { DrawShape.ToDisplay(DrawShape.Rect), DrawShape.ToDisplay(DrawShape.Diamond) },
                DrawShape.ToDisplay(m_DrawShape),
                display => m_DrawShape = DrawShape.ToCode(display),
                MapToolConstants.DrawShapePrefsKey);
        }

        /// <summary>
        /// DrawClear：启用开关 + 区域类型下拉（摆放区/禁区/通用区，默认摆放区；回显 EditorPrefs 持久化值，切换即落盘）
        /// </summary>
        private void BindDrawClearControls()
        {
            m_DrawClearEnabledToggle = m_Root.QByName<Toggle>(MapToolUiNames.DrawClearEnabledToggle);
            BindPrefsBackedToggle(m_DrawClearEnabledToggle, MapToolConstants.DrawClearEnabled,
                MapToolConstants.DrawClearEnabledPrefsKey);

            m_RegionType = MapToolConstants.RegionType;
            m_RegionTypeField = m_Root.QByName<DropdownField>(MapToolUiNames.RegionTypeField);
            BindPrefsBackedDropdown(m_RegionTypeField,
                new List<string>
                {
                    RegionType.ToDisplay(RegionType.Place),
                    RegionType.ToDisplay(RegionType.Forbid),
                    RegionType.ToDisplay(RegionType.Common)
                },
                RegionType.ToDisplay(m_RegionType),
                display => m_RegionType = RegionType.ToCode(display),
                MapToolConstants.RegionTypePrefsKey);
        }

        /// <summary>
        /// 绑定受 EditorPrefs 持久化的 Toggle：先回显初值，再注册变更即落盘
        /// </summary>
        /// <param name="toggle">目标开关</param>
        /// <param name="initialValue">初值（来自持久化配置）</param>
        /// <param name="prefsKey">EditorPrefs 键</param>
        private static void BindPrefsBackedToggle(Toggle toggle, bool initialValue, string prefsKey)
        {
            if (toggle == null) return;

            toggle.value = initialValue;
            toggle.RegisterValueChangedCallback(evt => MapToolConstants.SetConfigBool(prefsKey, evt.newValue));
        }

        /// <summary>
        /// 绑定受 EditorPrefs 持久化的枚举下拉：先回显当前显示名，再注册变更（应用 + 落盘 code）
        /// </summary>
        /// <param name="field">下拉控件</param>
        /// <param name="displays">可选显示名列表</param>
        /// <param name="currentDisplay">当前值显示名</param>
        /// <param name="apply">显示名 → 内部 code 的应用回调</param>
        /// <param name="prefsKey">EditorPrefs 键</param>
        private static void BindPrefsBackedDropdown(DropdownField field, List<string> displays, string currentDisplay,
            System.Func<string, string> apply, string prefsKey)
        {
            if (field == null || apply == null) return;

            field.choices = displays;
            field.value = currentDisplay;
            field.RegisterValueChangedCallback(evt =>
            {
                string code = apply(evt.newValue);
                MapToolConstants.SetConfigString(prefsKey, code);
            });
        }

        /// <summary>
        /// 查找对象编辑面板元素（可编辑路径 / 编辑动画两切页）
        /// </summary>
        private void FindObjectEditPanelElements()
        {
            // 对象编辑面板（选中人物后显示：可编辑路径 / 编辑动画）
            m_ObjectEditPanel = m_Root.QByName<VisualElement>(MapToolUiNames.ObjectEditPanel);
            if (m_ObjectEditPanel != null)
            {
                m_ObjEditTitle = m_ObjectEditPanel.QByName<Label>(MapToolUiNames.ObjEditTitle);
                m_TabEditPath = m_ObjectEditPanel.QByName<Button>(MapToolUiNames.TabEditPath);
                m_TabEditAnim = m_ObjectEditPanel.QByName<Button>(MapToolUiNames.TabEditAnim);
                m_PathEditContent = m_ObjectEditPanel.QByName<VisualElement>(MapToolUiNames.PathEditContent);
                m_AnimEditContent = m_ObjectEditPanel.QByName<VisualElement>(MapToolUiNames.AnimEditContent);
                m_BtnPathDraw = m_ObjectEditPanel.QByName<Button>(MapToolUiNames.BtnPathDraw);
                m_BtnPathUndo = m_ObjectEditPanel.QByName<Button>(MapToolUiNames.BtnPathUndo);
                m_BtnPathClear = m_ObjectEditPanel.QByName<Button>(MapToolUiNames.BtnPathClear);
                m_BtnClearTarget = m_ObjectEditPanel.QByName<Button>(MapToolUiNames.BtnClearTarget);
                m_PathPointList = m_ObjectEditPanel.QByName<ScrollView>(MapToolUiNames.PathPointList);
                // 默认隐藏，选中物体后显示
                m_ObjectEditPanel.style.display = DisplayStyle.None;
            }
            // 动画切页字段
            m_AnimNameField = m_Root.QByName<TextField>(MapToolUiNames.AnimNameField);
            m_AnimLoopToggle = m_Root.QByName<Toggle>(MapToolUiNames.AnimLoopToggle);
            m_AnimSpeedField = m_Root.QByName<TextField>(MapToolUiNames.AnimSpeedField);
        }

        /// <summary>
        /// 查找底部属性面板字段（标识区 / 新增属性 / 交互冒泡朝向等动态控件）
        /// </summary>
        private void FindPropertyFieldElements()
        {
            FindPropertyIdentityElements();
            BindPropertyToggleEvents();
            BuildPropertyDynamicControls();
        }

        /// <summary>
        /// 查找属性面板的固定字段与容器（标识区、操作按钮、右侧操作卡片），并初始隐藏
        /// </summary>
        private void FindPropertyIdentityElements()
        {
            // 底部属性面板字段
            m_TemplateIdField = m_Root.QByName<TextField>(MapToolUiNames.PropTemplateId);
            m_TagField = m_Root.QByName<TextField>(MapToolUiNames.PropTag);
            m_GridPosField = m_Root.QByName<TextField>(MapToolUiNames.PropGridPos);
            m_BuildingTemplateField = m_Root.QByName<TextField>(MapToolUiNames.PropBuildingTemplate);
            m_GridPrefixField = m_Root.QByName<TextField>(MapToolUiNames.PropGridPrefix);
            m_InteractStateField = m_Root.QByName<TextField>(MapToolUiNames.PropInteractState);
            m_DeleteObjectBtn = m_Root.QByName<Button>(MapToolUiNames.BtnDeleteObject);
            m_PublishBtn = m_Root.QByName<Button>(MapToolUiNames.BtnPublish);
            m_PropertyPanel = m_Root.QByName<VisualElement>(MapToolUiNames.PropertyPanel);
            m_PropertyActionArea = m_Root.QByName<VisualElement>(MapToolUiNames.PropertyActionArea);
            m_PropertyActionHost = m_Root.QByName<VisualElement>(MapToolUiNames.PropActionHost);

            // 初始未选中任何物件：属性面板与右侧操作卡片均隐藏
            m_PropertyPanel.SetVisible(false);
            m_PropertyActionArea.SetVisible(false);
            m_PropertyActionHost.SetVisible(false);

            // 新增属性面板字段
            m_SortOrderField = m_Root.QByName<TextField>(MapToolUiNames.PropSortOrder);
            m_CollectibleGroup = m_Root.QByName<VisualElement>(MapToolUiNames.PropCollectibleGroup);
            m_TrafficGroup = m_Root.QByName<VisualElement>(MapToolUiNames.PropTrafficGroup);
        }

        /// <summary>
        /// 绑定属性面板固定开关（碰撞 / 投影 / 可收集）的即时写回
        /// </summary>
        private void BindPropertyToggleEvents()
        {
            m_CollisionToggle = m_Root.QByName<Toggle>(MapToolUiNames.PropCollision);
            if (m_CollisionToggle != null)
                m_CollisionToggle.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.CollisionEnabled = evt.newValue;
                });

            m_ShadowToggle = m_Root.QByName<Toggle>(MapToolUiNames.PropShadow);
            if (m_ShadowToggle != null)
                m_ShadowToggle.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.ShadowEnabled = evt.newValue;
                });

            m_CollectibleToggle = m_Root.QByName<Toggle>(MapToolUiNames.PropCollectible);
            if (m_CollectibleToggle != null)
                m_CollectibleToggle.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.IsCollectible = evt.newValue;
                    // 变换区（层级/比例/角度）仅收集物显示
                    SetCollectibleActionVisible(evt.newValue);
                });
        }

        private void BuildPropertyDynamicControls()
        {
            // 交互类型下拉框
            m_InteractTypeField = CreatePropertyDropdown(
                MapToolUiNames.PropInteractTypeContainer,
                TemplateInteractType.DisplayChoices, TemplateInteractType.ToDisplay(TemplateInteractType.None),
                value => m_EditTarget.InteractType = TemplateInteractType.ToCode(value));

            // 气泡类型下拉框
            m_DialogBubbleField = CreatePropertyDropdown(
                MapToolUiNames.PropDialogBubbleContainer,
                new List<string> { "无", "对话", "思考", "收集提示" }, "无",
                value => m_EditTarget.DialogBubble = value);

            // ---TODO 气泡配置表通用化：气泡ID输入框（引用 Tables.Bubble；<=0 保存时不输出 BubbleId 行）
            m_BubbleIdField = CreatePropertyField(MapToolUiNames.PropBubbleIdContainer, CreateBubbleIdField());

            // 朝向下拉框
            m_FacingField = CreatePropertyDropdown(
                MapToolUiNames.PropFacingContainer,
                TemplateFacing.DisplayChoices, TemplateFacing.ToDisplay(TemplateFacing.Down),
                value => m_EditTarget.FacingDirection = TemplateFacing.ToCode(value));

            // 层级档（独立显示层）下拉：八层可选，只改渲染前后档位，不改变素材归属与挂载节点
            m_DisplayLayerField = CreatePropertyDropdown(
                MapToolUiNames.PropDisplayLayerContainer,
                MapLayerCatalog.AllLayerTypes.Select(MapLayerCatalog.GetLayerDisplayName).ToList(),
                MapLayerCatalog.GetLayerDisplayName(MapLayerCatalog.Prop),
                value => ApplyDisplayLayerChanged(m_EditTarget, MapLayerCatalog.GetLayerKeyByDisplayName(value)));
        }

        /// <summary>
        /// 在指定容器内创建属性下拉框（值变更仅在有选中目标时写回）
        /// </summary>
        /// <param name="containerName">容器元素名</param>
        /// <param name="choices">选项列表</param>
        /// <param name="defaultValue">默认显示值</param>
        /// <param name="apply">显示值应用回调（仅在 m_EditTarget 非空时调用）</param>
        /// <returns>创建的下拉框；容器缺失时返回 null</returns>
        private DropdownField CreatePropertyDropdown(string containerName, List<string> choices,
            string defaultValue, System.Action<string> apply)
        {
            return MapToolUiFactory.CreateDropdownIn(m_Root, containerName, choices, defaultValue, evt =>
            {
                if (m_EditTarget == null) return;
                apply?.Invoke(evt.newValue);
            });
        }

        /// <summary>
        /// 把已创建的控件挂到指定容器内
        /// </summary>
        /// <typeparam name="T">控件类型</typeparam>
        /// <param name="containerName">容器元素名</param>
        /// <param name="field">控件实例</param>
        /// <returns>传入的控件；容器缺失时返回 null</returns>
        private T CreatePropertyField<T>(string containerName, T field) where T : VisualElement
        {
            return MapToolUiFactory.MountIn(m_Root, containerName, field);
        }

        /// <summary>
        /// 创建气泡 ID 输入框（<=0 表示不引用气泡表，保存时不输出该行）
        /// </summary>
        /// <returns>气泡 ID 输入框</returns>
        private IntegerField CreateBubbleIdField()
        {
            var field = new IntegerField("") { value = 0 };
            field.style.flexGrow = 1;
            field.RegisterValueChangedCallback(evt =>
            {
                if (m_EditTarget != null)
                    m_EditTarget.bubbleId = evt.newValue;
            });
            return field;
        }

        /// <summary>
        /// 查找场景信息字段（场景名/场景ID/地图宽高）并绑定宽高变更
        /// </summary>
        private void FindSceneInfoElements()
        {
            // 场景信息字段（保存配置时读取）
            m_SceneNameField = m_Root.QByName<TextField>(MapToolUiNames.SceneNameField);
            m_SceneIdField = m_Root.QByName<TextField>(MapToolUiNames.SceneIdField);
            m_WidthField = m_Root.QByName<TextField>(MapToolUiNames.MapWidthField);
            m_HeightField = m_Root.QByName<TextField>(MapToolUiNames.MapHeightField);
            // 宽高输入框：手动修改后回写地图基准尺寸并同步配置尺寸，网格/吸附/命中按新基准铺满
            if (m_WidthField != null)
                m_WidthField.RegisterValueChangedCallback(OnMapWidthHeightChanged);
            if (m_HeightField != null)
                m_HeightField.RegisterValueChangedCallback(OnMapWidthHeightChanged);
            // 绑定保存路径按钮点击事件（委托给 EditorStatusBar）
            m_StatusBar.BindSavePathClick(OnSavePathClicked);
        }

        private void BindLayerToggleEvents()
        {
            // 绑定图层 Toggle 事件（覆盖类 + 层级八层，开关独立控制显隐，不联动放置类型）
            BindToggleValue(m_PlaceGridToggle, v => m_ShowPlaceGrid = v);
            BindToggleValue(m_PathGridToggle, v => m_ShowPathGrid = v);
            BindToggleValue(m_MapLayerToggle, v => m_ShowMapLayer = v);
            BindToggleValue(m_BackgroundLayerToggle, v => m_ShowBackgroundLayer = v);
            BindToggleValue(m_GreeneryLayerToggle, v => m_ShowGreeneryLayer = v);
            BindToggleValue(m_RoadLayerToggle, v => m_ShowRoadLayer = v);
            BindToggleValue(m_BuildingLayerToggle, v => m_ShowBuildingLayer = v);
            BindToggleValue(m_ObjectLayerToggle, v => m_ShowObjectLayer = v);
            BindToggleValue(m_CharacterLayerToggle, v => m_ShowCharacterLayer = v);
            BindToggleValue(m_ForegroundLayerToggle, v => m_ShowForegroundLayer = v);
            BindToggleValue(m_BubblesLayerToggle, v => m_ShowBubblesLayer = v);
            BindToggleValue(m_BoundingBoxToggle, v => m_ShowBoundingBox = v);
        }

        /// <summary>
        /// 绑定 Toggle 值变更到字段（控件缺失时静默跳过）
        /// </summary>
        /// <param name="toggle">目标开关</param>
        /// <param name="apply">值应用回调</param>
        private static void BindToggleValue(Toggle toggle, System.Action<bool> apply)
        {
            if (toggle == null || apply == null) return;
            toggle.RegisterValueChangedCallback(evt => apply(evt.newValue));
        }

        private void BuildPropertyActionRows()
        {
            if (m_PropertyPanel == null) return;

            // 操作行挂载到右栏操作卡片；卡片缺失时回退属性流 host，再回退属性面板本体
            var actionHost = m_PropertyActionArea != null ? m_PropertyActionArea : (m_PropertyActionHost != null ? m_PropertyActionHost : m_PropertyPanel);

            // 收集物专属变换容器：仅勾选收集时显示，与交通规则区相互独立
            m_CollectibleAction = new VisualElement();
            m_CollectibleAction.AddToClassList(MapToolStyleClasses.CollectibleAction);
            actionHost.Add(m_CollectibleAction);

            BuildLayerActionRow();
            BuildScaleActionRow();
            BuildRotationActionRow();
        }

        /// <summary>
        /// 同层层级操作行（上移/下移/置顶/置底，仅作用于同层 z-order，不跨八层契约）
        /// </summary>
        private void BuildLayerActionRow()
        {
            var row = CreateActionRow("层级:");
            row.Add(CreateActionButton("上移", MoveTargetForward));
            row.Add(CreateActionButton("下移", MoveTargetBackward));
            row.Add(CreateActionButton("置顶", BringTargetToFront));
            row.Add(CreateActionButton("置底", SendTargetToBack));
            m_CollectibleAction.Add(row);
        }

        /// <summary>
        /// 比例变换操作行（缩小 / 放大 / 重置），回显当前倍率
        /// </summary>
        private void BuildScaleActionRow()
        {
            m_ScaleLabel = BuildStepActionRow("比例:", "－", "＋", "重置",
                MapToolConstants.ScaleStep, ScaleTarget, ResetTargetScale, "100%");
        }

        /// <summary>
        /// 角度变换操作行（左旋 / 右旋 / 重置），回显当前角度
        /// </summary>
        private void BuildRotationActionRow()
        {
            m_RotationLabel = BuildStepActionRow("角度:", "↺", "↻", "重置",
                MapToolConstants.RotationStep, RotateTarget, ResetTargetRotation, "0°");
        }

        /// <summary>
        /// 创建操作行容器（含左侧标签；三行共用 prop-action-row 样式：标签 44px、按钮 flex 均分、统一 4px 间距）
        /// </summary>
        /// <param name="labelText">行标签文本</param>
        /// <returns>行容器</returns>
        private static VisualElement CreateActionRow(string labelText)
        {
            var row = new VisualElement();
            row.AddToClassList(MapToolStyleClasses.PropActionRow);

            var label = MapToolUiFactory.CreateLabel(labelText, MapToolStyleClasses.PropActionLabel);
            row.Add(label);

            return row;
        }

        /// <summary>
        /// 创建操作按钮（统一 prop-action-btn 样式，次要操作为次级样式）
        /// </summary>
        /// <param name="text">按钮文本</param>
        /// <param name="onClick">点击回调</param>
        /// <param name="secondary">是否为次要操作（灰色样式）</param>
        /// <returns>按钮</returns>
        private static Button CreateActionButton(string text, Action onClick, bool secondary = false)
        {
            var button = new Button(onClick) { text = text };
            button.AddToClassList(MapToolStyleClasses.PropActionBtn);
            if (secondary) button.AddToClassList(MapToolStyleClasses.Secondary);
            return button;
        }

        /// <summary>
        /// 创建操作行中的数值回显标签
        /// </summary>
        /// <param name="initialText">初始文本</param>
        /// <returns>数值标签</returns>
        private static Label CreateActionValueLabel(string initialText)
        {
            var label = MapToolUiFactory.CreateLabel(initialText, MapToolStyleClasses.PropActionValue);
            return label;
        }

        /// <summary>
        /// 构建「减 / 当前值 / 加 / 重置」四段式操作行（比例、角度等数值微调行共用）
        /// 行容器统一挂到 m_CollectibleAction，返回中间的值回显标签供调用方保存引用
        /// </summary>
        /// <param name="title">行标签文本</param>
        /// <param name="minusText">减按钮文本</param>
        /// <param name="plusText">加按钮文本</param>
        /// <param name="resetText">重置按钮文本</param>
        /// <param name="step">单次调整步长</param>
        /// <param name="applyDelta">应用增量回调（正值增/负值减）</param>
        /// <param name="reset">重置回调</param>
        /// <param name="initialValueText">值标签初始文本</param>
        /// <returns>行中的值回显标签</returns>
        private Label BuildStepActionRow(string title, string minusText, string plusText, string resetText,
            float step, System.Action<float> applyDelta, System.Action reset, string initialValueText)
        {
            var row = CreateActionRow(title);
            row.Add(CreateActionButton(minusText, () => applyDelta(-step)));
            var valueLabel = CreateActionValueLabel(initialValueText);
            row.Add(valueLabel);
            row.Add(CreateActionButton(plusText, () => applyDelta(step)));
            row.Add(CreateActionButton(resetText, reset, true));
            m_CollectibleAction.Add(row);
            return valueLabel;
        }

        /// <summary>
        /// 构建交通子面板 + 地图渲染用 IMGUI 容器
        /// </summary>
        private void BuildSceneView()
        {
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
                m_CollectibleAction.SetVisible(visible);
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
                m_ObjectEditPanel.SetVisible(false);
                RefreshSceneObjects();
                MapToolLog.Info("删除选中物体");
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
                MapToolLog.Info($"[层级调整] 物件 {m_EditTarget.Name} 同层 SortOrder={m_EditTarget.SortOrder}");
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
            MapToolLog.Info($"[显示档变更] 物件 {target.Name} 显示层={MapLayerCatalog.GetLayerDisplayName(newDisplayLayer)} SortOrder={target.SortOrder}");
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
            MapToolLog.Info("发布地图配置");
            SaveMapConfig(true);
        }

        /// <summary>
        /// 清空摆放物按钮点击
        /// </summary>
        private void OnClearObjectsClicked()
        {
            if (MapToolDialog.Confirm("清空摆放物", "确定要清空所有已摆放的物体吗？此操作不可撤销！"))
            {
                m_PlacedObjects.Clear();
                ClearTrafficWorkingData();
                m_EditTarget = null;
                m_ObjectEditPanel.SetVisible(false);
                RefreshSceneObjects();
                MapToolLog.Info("已清空所有摆放物");
            }
        }

        /// <summary>
        /// 清空格子数据按钮点击
        /// </summary>
        private void OnClearGridDataClicked()
        {
            if (MapToolDialog.Confirm("清空格子数据", "确定要清空格子数据吗？此操作不可撤销！"))
            {
                // TODO: 清空格子数据（迷雾、格子标记等）
                MapToolLog.Info("已清空格子数据");
            }
        }

        /// <summary>
        /// 导出设置按钮点击
        /// </summary>
        private void OnExportSettingsClicked()
        {
            MapToolLog.Info("导出设置");
            // TODO: 导出地图配置设置
        }

    }
}
