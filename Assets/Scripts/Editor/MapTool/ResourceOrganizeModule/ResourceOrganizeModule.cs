/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ResourceOrganizeModule.cs
 * author:    云毅
 * created:   2026
 * descrip:   资源整理模块 - 扫描未配置资源、分类过滤、详情编辑、配置表预览
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Honor.Editor.MapTool;
using Debug = UnityEngine.Debug;

namespace Honor.Editor.MapTool.ResourceOrganize
{
    /// <summary>
    /// 资源整理模块：扫描未配置资源、显示详情、添加到配置表
    /// </summary>
    public class ResourceOrganizeModule
    {
        // ===== 依赖引用 =====
        private VisualElement m_Root;
        private Button m_TabResource;
        private VisualElement m_ResourcePanel;

        // ===== 状态 =====
        private List<string> m_UnconfiguredResources = new List<string>();
        private string m_CurrentSelectedPath;
        private string m_CurrentFilter = MapToolConstants.FilterAll; // all/bg/building/char/prop/cat

        // ===== 子 Tab 按钮 =====
        private Button m_TabAll;
        private Button m_TabBg;
        private Button m_TabBuilding;
        private Button m_TabChar;
        private Button m_TabProp;
        private Button m_TabCat;

        // ---TODO 表单字段缓存：右侧编辑区 9 个字段在 FindElements 中查得一次，读取/回填/默认值三处共用
        private TextField m_FormTemplateIdField;
        private TextField m_FormImagePathField;
        private TextField m_FormTemplateNameField;
        private TextField m_FormAnimNameField;
        private Toggle m_FormLoopToggle;
        private FloatField m_FormSpeedField;
        private IntegerField m_FormSortOrderField;
        private Toggle m_FormShadowToggle;
        private Toggle m_FormCollectibleToggle;

        // ---TODO 容器缓存：未配置资源列表 / 配置表列表容器在 FindElements 中查得一次，多处刷新/高亮共用
        private VisualElement m_UnconfiguredListContainer;

        // ===== 回调 =====
        public Action<Button> OnSetActiveTab;
        public Action OnOtherModesHidden; // 切换到其他模式时调用

        public ResourceOrganizeModule(VisualElement root)
        {
            m_Root = root;
        }

        /// <summary>
        /// 查找并绑定资源整理相关的 UI 元素
        /// </summary>
        public void FindElements(VisualElement visualTree)
        {
            m_TabResource = visualTree.QByName<Button>(MapToolUiNames.TabResource);
            m_ResourcePanel = visualTree.QByName<VisualElement>(MapToolUiNames.ResourcePanel);

            // 子 Tab 按钮
            m_TabAll = m_ResourcePanel.QByName<Button>(MapToolUiNames.TabAll);
            m_TabBg = m_ResourcePanel.QByName<Button>(MapToolUiNames.TabBg);
            m_TabBuilding = m_ResourcePanel.QByName<Button>(MapToolUiNames.TabBuilding);
            m_TabChar = m_ResourcePanel.QByName<Button>(MapToolUiNames.TabChar);
            m_TabProp = m_ResourcePanel.QByName<Button>(MapToolUiNames.TabProp);
            m_TabCat = m_ResourcePanel.QByName<Button>(MapToolUiNames.TabCat);

            // 右侧编辑区字段（读取/回填/默认值三处共用口径）
            m_FormTemplateIdField = m_ResourcePanel.QByName<TextField>(MapToolUiNames.FieldTemplateId);
            m_FormImagePathField = m_ResourcePanel.QByName<TextField>(MapToolUiNames.FieldImagePath);
            m_FormTemplateNameField = m_ResourcePanel.QByName<TextField>(MapToolUiNames.FieldTemplateName);
            m_FormAnimNameField = m_ResourcePanel.QByName<TextField>(MapToolUiNames.FieldAnimName);
            m_FormLoopToggle = m_ResourcePanel.QByName<Toggle>(MapToolUiNames.FieldLoop);
            m_FormSpeedField = m_ResourcePanel.QByName<FloatField>(MapToolUiNames.FieldSpeed);
            m_FormSortOrderField = m_ResourcePanel.QByName<IntegerField>(MapToolUiNames.FieldSortOrder);
            m_FormShadowToggle = m_ResourcePanel.QByName<Toggle>(MapToolUiNames.FieldShadow);
            m_FormCollectibleToggle = m_ResourcePanel.QByName<Toggle>(MapToolUiNames.FieldCollectible);

            // 列表容器（刷新/高亮/去高亮三处共用）
            m_UnconfiguredListContainer = m_ResourcePanel.QByName<VisualElement>(MapToolUiNames.UnconfiguredResourcesList);
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        public void BindEvents()
        {
            if (m_TabResource != null)
                m_TabResource.clicked += ShowResourceMode;

            if (m_ResourcePanel != null)
            {
                var scanBtn = m_ResourcePanel.QByName<Button>(MapToolUiNames.BtnScanResources);
                if (scanBtn != null)
                    scanBtn.clicked += OnScanResourcesClicked;

                var addBtn = m_ResourcePanel.QByName<Button>(MapToolUiNames.BtnAddToConfig);
                if (addBtn != null)
                    addBtn.clicked += OnAddToConfigClicked;

                var addAllBtn = m_ResourcePanel.QByName<Button>(MapToolUiNames.BtnSaveConfig);
                if (addAllBtn != null)
                    addAllBtn.clicked += OnSaveConfigClicked;

                var openConfigBtn = m_ResourcePanel.QByName<Button>(MapToolUiNames.BtnOpenConfig);
                if (openConfigBtn != null)
                    openConfigBtn.clicked += OnOpenConfigClicked;

                var openFolderBtn = m_ResourcePanel.QByName<Button>(MapToolUiNames.BtnOpenFolder);
                if (openFolderBtn != null)
                    openFolderBtn.clicked += OnOpenFolderClicked;

                // 子 Tab 切换
                if (m_TabAll != null) m_TabAll.clicked += () => SwitchFilter(MapToolConstants.FilterAll);
                if (m_TabBg != null) m_TabBg.clicked += () => SwitchFilter(MapToolConstants.FilterBg);
                if (m_TabBuilding != null) m_TabBuilding.clicked += () => SwitchFilter(MapToolConstants.FilterBuilding);
                if (m_TabChar != null) m_TabChar.clicked += () => SwitchFilter(MapToolConstants.FilterCharacter);
                if (m_TabProp != null) m_TabProp.clicked += () => SwitchFilter(MapToolConstants.FilterProp);
                if (m_TabCat != null) m_TabCat.clicked += () => SwitchFilter(MapToolConstants.FilterCat);
            }
        }

        /// <summary>
        /// 切换分类过滤
        /// </summary>
        private void SwitchFilter(string filter)
        {
            m_CurrentFilter = filter;

            // 更新按钮高亮状态
            UpdateSubTabActive(m_TabAll, filter == MapToolConstants.FilterAll);
            UpdateSubTabActive(m_TabBg, filter == MapToolConstants.FilterBg);
            UpdateSubTabActive(m_TabBuilding, filter == MapToolConstants.FilterBuilding);
            UpdateSubTabActive(m_TabChar, filter == MapToolConstants.FilterCharacter);
            UpdateSubTabActive(m_TabProp, filter == MapToolConstants.FilterProp);
            UpdateSubTabActive(m_TabCat, filter == MapToolConstants.FilterCat);

            // 重新显示列表
            ShowUnconfiguredList();
            RefreshConfigList();
        }

        private void UpdateSubTabActive(Button btn, bool active)
        {
            if (btn == null) return;
            if (active)
                btn.AddToClassList(MapToolStyleClasses.Active);
            else
                btn.RemoveFromClassList(MapToolStyleClasses.Active);
        }

        /// <summary>
        /// 切换到资源整理模式
        /// </summary>
        public void ShowResourceMode()
        {
            OnSetActiveTab?.Invoke(m_TabResource);
            OnOtherModesHidden?.Invoke();
            ShowResourcePanel(true);

            // 强制显示顶部 Tab 栏
            var topToolbar = m_Root.Q<VisualElement>(className: MapToolUiNames.TopToolbarClass);
            if (topToolbar != null)
            {
                topToolbar.style.display = DisplayStyle.Flex;
                topToolbar.style.flexGrow = 0;
                topToolbar.style.height = 30;
            }

            // 刷新配置列表
            RefreshConfigList();
        }

        /// <summary>
        /// 显示/隐藏资源整理面板
        /// </summary>
        public void ShowResourcePanel(bool show)
        {
            m_ResourcePanel.SetVisible(show);
        }

        /// <summary>
        /// 扫描未配置资源按钮点击
        /// </summary>
        private void OnScanResourcesClicked()
        {
            // 先从所有 Excel 配置表重新加载最新数据，确保和磁盘文件同步
            ObjectTemplateManager.Instance.LoadAllConfigTables();

            string mapDir = MapToolConstants.ResourceScanDir;
            if (!AssetDatabase.IsValidFolder(mapDir))
            {
                MapToolDialog.Info("扫描失败", $"目录不存在: {mapDir}");
                return;
            }

            // 收集所有 PNG 文件（按文件名去重，同名只保留一个）
            var allPngs = new List<string>();
            var seenNames = new HashSet<string>();
            foreach (var path in MapToolEditorUtils.ScanTextureAssets(mapDir, MapToolConstants.ExtPng))
            {
                string fileName = Path.GetFileNameWithoutExtension(path).ToLower();
                if (seenNames.Add(fileName)) allPngs.Add(path);
            }

            // 获取配置表中已有的图片名
            var configuredNames = new HashSet<string>();
            foreach (var t in ObjectTemplateManager.Instance.GetAllTemplates())
            {
                if (!string.IsNullOrEmpty(t.ImagePath))
                    configuredNames.Add(Path.GetFileNameWithoutExtension(t.ImagePath).ToLower());
            }

            // 找出未配置的
            m_UnconfiguredResources = allPngs
                .Where(p => !configuredNames.Contains(Path.GetFileNameWithoutExtension(p).ToLower()))
                .OrderBy(p => p)
                .ToList();

            // 显示列表
            ShowUnconfiguredList();

            MapToolLog.Info($"[资源整理] 扫描完成: 共 {allPngs.Count} 个PNG，未配置 {m_UnconfiguredResources.Count} 个");

            // 刷新右侧配置表列表
            RefreshConfigList();
        }

        /// <summary>
        /// 显示未配置资源列表（按当前分类过滤）
        /// </summary>
        private void ShowUnconfiguredList()
        {
            if (m_UnconfiguredListContainer == null) return;

            m_UnconfiguredListContainer.Clear();

            var filtered = FilterUnconfiguredResources();

            m_UnconfiguredListContainer.Add(new Label($"共发现 {filtered.Count} 个未配置资源：")
            {
                style = { color = Color.gray, fontSize = 11, marginBottom = 4 }
            });

            foreach (var path in filtered)
            {
                m_UnconfiguredListContainer.Add(CreateResourceListItem(path));
            }
        }

        /// <summary>
        /// 按当前分类过滤未配置资源（分类口径统一走 MapToolEditorUtils.MatchesCategoryPrefix）
        /// </summary>
        /// <returns>过滤后的资源路径列表</returns>
        private List<string> FilterUnconfiguredResources()
        {
            if (m_CurrentFilter == MapToolConstants.FilterAll) return m_UnconfiguredResources;

            return m_UnconfiguredResources
                .Where(p => MapToolEditorUtils.MatchesCategoryPrefix(p, m_CurrentFilter))
                .ToList();
        }

        /// <summary>
        /// 构建单个未配置资源条目：缩略图 + 文件名，点击进入详情
        /// </summary>
        /// <param name="path">资源资产路径</param>
        /// <returns>条目容器</returns>
        private VisualElement CreateResourceListItem(string path)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);

            // 每个条目：横向布局 = 缩略图 + 文件名
            var item = new VisualElement();
            item.style.flexDirection = FlexDirection.Row;
            item.style.alignItems = Align.Center;
            item.style.height = 40;
            item.style.marginBottom = 2;
            item.style.paddingLeft = 4;
            item.style.paddingRight = 4;
            item.style.backgroundColor = new Color(0.18f, 0.18f, 0.18f);
            item.style.borderTopLeftRadius = 2;
            item.style.borderTopRightRadius = 2;
            item.style.borderBottomLeftRadius = 2;
            item.style.borderBottomRightRadius = 2;

            item.Add(CreateThumbnail(path));
            item.Add(CreateResourceNameLabel(fileName));

            // 点击事件
            item.userData = path; // 保存路径供后续查找高亮
            item.RegisterCallback<MouseDownEvent>(evt =>
            {
                ShowResourceDetail(path);
                // 更新选中高亮
                HighlightSelectedItem(item);
            });

            return item;
        }

        /// <summary>
        /// 构建资源缩略图（36x36，加载失败时仅保留占位底色）
        /// </summary>
        /// <param name="path">资源资产路径</param>
        /// <returns>缩略图控件</returns>
        private static Image CreateThumbnail(string path)
        {
            var preview = new Image();
            preview.style.width = 36;
            preview.style.height = 36;
            preview.style.marginRight = MapToolConstants.ListItemMarginRight;
            preview.scaleMode = ScaleMode.ScaleToFit;
            preview.style.backgroundColor = new Color(0.3f, 0.3f, 0.5f);

            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (tex != null) preview.image = tex;

            return preview;
        }

        /// <summary>
        /// 构建资源名称标签
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>名称标签</returns>
        private static Label CreateResourceNameLabel(string fileName)
        {
            var nameLabel = new Label(fileName);
            nameLabel.style.fontSize = 12;
            nameLabel.style.color = new Color(0.9f, 0.9f, 0.9f);
            nameLabel.style.flexGrow = 1;
            return nameLabel;
        }

        /// <summary>
        /// 分类键 → 配置表文件名映射（唯一口径：标签显示与 Excel 路径共用）
        /// </summary>
        private static readonly Dictionary<string, string> s_FilterTableNames = new Dictionary<string, string>
        {
            { MapToolConstants.FilterBg, MapToolConstants.TableBgFileName },
            { MapToolConstants.FilterBuilding, MapToolConstants.TableBuildingFileName },
            { MapToolConstants.FilterCharacter, MapToolConstants.TableCharacterFileName },
            { MapToolConstants.FilterProp, MapToolConstants.TablePropFileName },
            { MapToolConstants.FilterCat, MapToolConstants.TableCatFileName },
        };

        /// <summary>
        /// 根据分类获取配置表名称
        /// </summary>
        private string GetConfigNameByFilter(string filter)
        {
            return s_FilterTableNames.TryGetValue(filter, out string name) ? name : "全部配置表";
        }

        /// <summary>
        /// 判断模板是否匹配当前过滤分类
        /// </summary>
        private bool MatchesFilter(ObjectTemplate t, string filter)
        {
            switch (filter)
            {
                case MapToolConstants.FilterBg:
                    return t.Type == MapObjectType.Prop && t.ImagePath.ToLower().StartsWith(MapToolConstants.PrefixBg);
                case MapToolConstants.FilterBuilding:
                    return t.Type == MapObjectType.Building;
                case MapToolConstants.FilterCharacter:
                    return t.Type == MapObjectType.Character;
                case MapToolConstants.FilterProp:
                    return t.Type == MapObjectType.Prop && !t.ImagePath.ToLower().StartsWith(MapToolConstants.PrefixBg);
                case MapToolConstants.FilterCat:
                    return t.Type == MapObjectType.Cat;
                default:
                    return true;
            }
        }

        /// <summary>
        /// 高亮选中的列表项
        /// </summary>
        private void HighlightSelectedItem(VisualElement selected)
        {
            if (m_UnconfiguredListContainer == null) return;

            foreach (var child in m_UnconfiguredListContainer.Children())
            {
                if (child is VisualElement ve && ve != selected)
                    ve.style.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
            }
            selected.style.backgroundColor = new Color(0.1f, 0.4f, 0.7f);
        }

        /// <summary>
        /// 根据资源路径高亮列表项
        /// </summary>
        private void HighlightItemByPath(string path)
        {
            if (m_UnconfiguredListContainer == null) return;

            foreach (var child in m_UnconfiguredListContainer.Children())
            {
                var itemPath = child.userData as string;
                bool isMatch = itemPath == path;
                child.style.backgroundColor = isMatch
                    ? new Color(0.1f, 0.4f, 0.7f)
                    : new Color(0.15f, 0.15f, 0.15f);
            }
        }

        /// <summary>
        /// 刷新已配置资源列表（显示在右侧表格中，按当前分类过滤）
        /// </summary>
        private void RefreshConfigList()
        {
            var container = m_ResourcePanel.QByName<VisualElement>(MapToolUiNames.ConfigListContainer);
            if (container == null) return;

            UpdateCurrentConfigNameLabel();
            container.Clear();

            var allTemplates = ObjectTemplateManager.Instance.GetAllTemplates();
            if (allTemplates.Count == 0)
            {
                container.Add(new Label("（暂无配置数据）")
                {
                    style = { color = Color.gray, fontSize = 11, paddingTop = 8, paddingLeft = 8 }
                });
                return;
            }

            // 按分类过滤（分类判定口径与保存时一致，统一走 MatchesFilter）
            var filtered = allTemplates;
            if (m_CurrentFilter != MapToolConstants.FilterAll)
            {
                filtered = allTemplates.Where(t => MatchesFilter(t, m_CurrentFilter)).ToList();
            }

            int rowIndex = 0;
            foreach (var t in filtered)
            {
                // 显式捕获循环变量，确保删除按钮绑定正确的模板对象
                var capturedTemplate = t;
                container.Add(ConfigTableBuilder.CreateTemplateRow(
                    capturedTemplate, rowIndex, () => DeleteTemplate(capturedTemplate)));
                rowIndex++;
            }
        }

        /// <summary>
        /// 刷新「当前配置表名」标签
        /// </summary>
        private void UpdateCurrentConfigNameLabel()
        {
            var configNameLabel = m_ResourcePanel.QByName<Label>(MapToolUiNames.CurrentConfigName);
            if (configNameLabel == null) return;

            configNameLabel.text = $"（{GetConfigNameByFilter(m_CurrentFilter)}）";
        }

        /// <summary>
        /// 删除模板并自动补全 ID
        /// </summary>
        private void DeleteTemplate(ObjectTemplate template)
        {
            MapToolLog.Info($"[ResourceOrganize] 删除模板: ID={template.TemplateId}, 名称={template.TemplateName}, 类型={template.Type}");

            // 确认删除
            var confirm = MapToolDialog.Confirm(
                "确认删除",
                $"确定要删除模板 {template.TemplateId} ({template.TemplateName}) 吗？\n删除后同类型的 ID 将自动重新排序。",
                MapToolConstants.DeleteButtonText, MapToolDialog.ButtonCancel);
            if (!confirm)
            {
                MapToolLog.Info("[ResourceOrganize] 用户取消删除");
                return;
            }

            // 从管理器中删除
            bool removed = ObjectTemplateManager.Instance.RemoveTemplate(template.TemplateId);
            MapToolLog.Info($"[ResourceOrganize] RemoveTemplate 返回: {removed}");

            // 按类型重新排序 ID
            TemplateIdGenerator.ReorderIds(template.Type);

            // 刷新列表
            RefreshConfigList();
        }

        /// <summary>
        /// 保存修改按钮点击：回读表格编辑 → 刷新列表 → 写回 Excel 并整理图片
        /// </summary>
        private void OnSaveConfigClicked()
        {
            // 弹出二级确认
            bool confirm = MapToolDialog.Confirm(
                "确认保存",
                "确定要保存所有修改到配置表吗？");

            if (!confirm) return;

            var container = m_ResourcePanel.QByName<VisualElement>(MapToolUiNames.ConfigListContainer);
            if (container == null)
            {
                MapToolDialog.Info("错误", "找不到配置表容器");
                return;
            }

            // 把表格中用户改动回写到模板对象
            ApplyEditedRowsToTemplates(container);

            // 刷新列表显示
            RefreshConfigList();

            // 写回 Excel 并整理图片目录
            PersistTemplatesToExcel();
        }

        /// <summary>
        /// 遍历配置表行，把表格中的编辑值按列标识回写到模板对象
        /// </summary>
        /// <param name="container">配置表容器</param>
        /// <returns>回写的行数</returns>
        private static int ApplyEditedRowsToTemplates(VisualElement container)
        {
            int savedCount = 0;
            foreach (var row in container.Children())
            {
                // 跳过非数据行（如"暂无配置数据"提示行）
                if (!(row.userData is ObjectTemplate template)) continue;

                ApplyEditedRowToTemplate(row, template);
                savedCount++;
            }
            return savedCount;
        }

        /// <summary>
        /// 单行回写：按列标识取控件，不依赖子控件创建顺序（列顺序调整时无需同步改下标）
        /// </summary>
        /// <param name="row">表格行</param>
        /// <param name="template">目标模板</param>
        private static void ApplyEditedRowToTemplate(VisualElement row, ObjectTemplate template)
        {
            string imagePath = ReadCellText(row, ConfigTableBuilder.ColKeyImagePath);
            if (imagePath != null) template.ImagePath = imagePath;

            string templateName = ReadCellText(row, ConfigTableBuilder.ColKeyName);
            if (templateName != null) template.TemplateName = templateName;

            float speed;
            if (float.TryParse(ReadCellText(row, ConfigTableBuilder.ColKeySpeed), out speed))
                template.Speed = speed;

            int sortOrder;
            if (int.TryParse(ReadCellText(row, ConfigTableBuilder.ColKeySortOrder), out sortOrder))
                template.SortOrder = MapToolEditorUtils.ClampTemplateSortOrder(template, sortOrder);

            string dialogBubble = ReadCellText(row, ConfigTableBuilder.ColKeyDialog);
            if (dialogBubble != null) template.DialogBubble = dialogBubble;

            float respawnTime;
            if (float.TryParse(ReadCellText(row, ConfigTableBuilder.ColKeyRespawn), out respawnTime))
                template.RespawnTime = respawnTime;

            var loopToggle = row.Q<Toggle>(name: ConfigTableBuilder.ColKeyLoop);
            if (loopToggle != null) template.Loop = loopToggle.value;

            var collectibleToggle = row.Q<Toggle>(name: ConfigTableBuilder.ColKeyCollectible);
            if (collectibleToggle != null) template.IsCollectible = collectibleToggle.value;
        }

        /// <summary>
        /// 读取指定列标识的文本单元格值（控件缺失返回 null）
        /// </summary>
        /// <param name="row">表格行</param>
        /// <param name="columnKey">列标识</param>
        /// <returns>单元格文本，缺失返回 null</returns>
        private static string ReadCellText(VisualElement row, string columnKey)
        {
            var field = row.Q<TextField>(name: columnKey);
            return field != null ? field.value : null;
        }

        /// <summary>
        /// 把当前分类的模板写回 Excel，并同步整理分类文件夹中的图片
        /// </summary>
        private void PersistTemplatesToExcel()
        {
            string excelPath = GetConfigExcelPath();
            var allTemplates = ObjectTemplateManager.Instance.GetAllTemplates();
            var filteredTemplates = allTemplates.Where(t => MatchesFilter(t, m_CurrentFilter)).ToList();

            // 保存时把每个模板对应的图片复制到 Map/{类型}/ 分类文件夹下
            int copiedImages = 0;
            foreach (var t in filteredTemplates)
            {
                if (CopyTemplateImageToCategoryFolder(t))
                    copiedImages++;
            }

            bool excelSaved = TemplateConfigWriter.WriteToExcel(excelPath, filteredTemplates);

            if (excelSaved)
            {
                MapToolLog.Info($"[ResourceOrganize] 保存完成，共写入 {filteredTemplates.Count} 行配置到 Excel，{copiedImages} 张图片已整理到分类文件夹");
                MapToolDialog.Success($"配置已保存到 Excel 文件：\n{System.IO.Path.GetFileName(excelPath)}\n\n共更新 {filteredTemplates.Count} 行数据。\n{copiedImages} 张图片已整理到分类文件夹。");
            }
            else
            {
                MapToolLog.Warn($"[ResourceOrganize] Excel 写入失败，已保存到内存");
                MapToolDialog.Info("部分成功", $"已保存到内存数据，但 Excel 文件写入失败。\n\n可能原因：\n1. Excel 文件正在被占用（请先关闭 Excel）\n2. 文件路径不正确\n\n请关闭 Excel 后重试。");
            }
        }

        /// <summary>
        /// 自动保存到 Excel（添加模板后调用，不弹确认框）
        /// </summary>
        private void SaveToExcel()
        {
            // 刷新列表显示
            RefreshConfigList();

            var allTemplates = ObjectTemplateManager.Instance.GetAllTemplates();

            // 按物件类型分组，各自写入「类型 → 配置表文件名」对应文件（映射口径统一走 MapToolEditorUtils）
            var byType = allTemplates.GroupBy(t => t.Type).ToList();
            string projectRoot = MapToolPathHelper.ProjectRoot;
            int writtenCount = 0;

            foreach (var group in byType)
            {
                if (group.Count() == 0) continue;

                string fileName = MapToolEditorUtils.GetConfigFileNameByType(group.Key);
                string path = MapToolPathHelper.ToProjectAbsolutePath(System.IO.Path.Combine(MapToolConstants.ConfigTableRootDir, fileName));
                if (TemplateConfigWriter.WriteToExcel(path, group.ToList())) writtenCount++;
            }

            MapToolLog.Info($"[ResourceOrganize] 自动保存成功，共写入 {writtenCount} 张配置表（模板总数 {allTemplates.Count}）");
        }

        /// <summary>
        /// 把模板对应的图片复制到 Map/{类型}/ 分类文件夹下
        /// </summary>
        /// <returns>是否执行了复制（false 表示已在目标位置或找不到图片）</returns>
        private bool CopyTemplateImageToCategoryFolder(ObjectTemplate template)
        {
            if (string.IsNullOrEmpty(template.ImagePath)) return false;

            // 根据类型确定层级目录（资源整理归入全局公共目录 Shared/<Layer>，便于后续打图集）
            // 文件名前缀优先识别绿化（grass_/plant_）与道路市政（road_），枚举归并不影响归类
            string lowerImage = (template.ImagePath ?? string.Empty).ToLowerInvariant();
            string layerKey;
            if (lowerImage.StartsWith(MapToolConstants.PrefixRoad))
            {
                layerKey = MapLayerCatalog.Road;
            }
            else if (lowerImage.StartsWith(MapToolConstants.PrefixGrass) ||
                     lowerImage.StartsWith(MapToolConstants.PrefixPlant))
            {
                layerKey = MapLayerCatalog.Greenery;
            }
            else if (template.Type == MapObjectType.Building) layerKey = MapLayerCatalog.Building;
            else if (template.Type == MapObjectType.Character) layerKey = MapLayerCatalog.Character;
            else if (template.Type == MapObjectType.Cat) layerKey = MapLayerCatalog.Character;
            else layerKey = MapLayerCatalog.Prop;
            string targetDir = MapLayerCatalog.GetGlobalSharedDir(layerKey);
            string fileName = template.ImagePath;

            // 确保目标文件夹存在（Shared → <Layer> 两级）
            string sharedRoot = MapLayerCatalog.GetGlobalSharedDir(layerKey);
            string sharedParent = MapToolConstants.ResourceScanDir;
            if (!AssetDatabase.IsValidFolder($"{sharedParent}/Shared"))
            {
                AssetDatabase.CreateFolder(sharedParent, "Shared");
            }
            if (!AssetDatabase.IsValidFolder(targetDir))
            {
                AssetDatabase.CreateFolder($"{sharedParent}/Shared", MapLayerCatalog.GetLayerDir(layerKey));
            }

            string targetPath = $"{targetDir}/{fileName}.png";

            // 如果目标位置已有该图片，跳过
            if (AssetDatabase.LoadAssetAtPath<Texture2D>(targetPath) != null)
                return false;

            // 按文件名查找源图片（可能在任意子目录中）
            string[] guids = AssetDatabase.FindAssets($"{fileName} {MapToolConstants.TextureAssetFilter}", new[] { MapToolConstants.ResourceScanDir });
            if (guids.Length == 0) return false;

            string sourcePath = AssetDatabase.GUIDToAssetPath(guids[0]);

            // 如果源文件已经在目标文件夹中，跳过
            if (sourcePath == targetPath) return false;

            // 复制图片
            AssetDatabase.CopyAsset(sourcePath, targetPath);
            MapToolLog.Info($"[ResourceOrganize] 图片已整理: {fileName} → {targetDir}/");
            return true;
        }

        /// <summary>
        /// 打开配置表按钮点击
        /// </summary>
        private void OnOpenConfigClicked()
        {
            string excelPath = GetConfigExcelPath();
            if (File.Exists(excelPath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(excelPath) { UseShellExecute = true });
            }
            else
            {
                MapToolDialog.Info($"配置表文件不存在:\n{excelPath}");
            }
        }

        /// <summary>
        /// 打开文件位置按钮点击（在资源管理器中选中配置表文件）
        /// </summary>
        private void OnOpenFolderClicked()
        {
            string excelPath = GetConfigExcelPath();
            if (File.Exists(excelPath))
            {
                // 用 explorer.exe /select 方式打开文件夹并选中文件
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{excelPath}\"");
            }
            else
            {
                // 如果文件不存在，尝试打开项目根目录
                string projectRoot = MapToolPathHelper.ProjectRoot;
                MapToolDialog.Info($"配置表文件不存在:\n{excelPath}\n\n已打开项目根目录: {projectRoot}");
                System.Diagnostics.Process.Start("explorer.exe", projectRoot);
            }
        }

        /// <summary>
        /// 获取配置表 Excel 路径
        /// </summary>
        private string GetConfigExcelPath()
        {
            // 根据当前过滤分类选择对应的配置表文件（分类 → 文件名的唯一口径见 s_FilterTableNames）
            string fileName = s_FilterTableNames.TryGetValue(m_CurrentFilter, out string mapped)
                ? mapped
                : MapToolConstants.TableCharacterFileName;

            return MapToolPathHelper.ToProjectAbsolutePath(System.IO.Path.Combine(MapToolConstants.ConfigTableRootDir, fileName));
        }

        /// <summary>
        /// 显示资源详情：清选中态 → 渲染预览 → 填充模板表单
        /// </summary>
        /// <param name="path">资源资产路径</param>
        private void ShowResourceDetail(string path)
        {
            m_CurrentSelectedPath = path;

            ClearResourceSelectionHighlight();

            string fileName = Path.GetFileNameWithoutExtension(path);
            ShowResourcePreview(path);

            // 自动推断类型（图片名前缀 → 物件类型，统一走公共映射，不再维护前缀字符串分支）
            MapObjectType objType = MapToolEditorUtils.GetObjectTypeByImageName(fileName);

            // 检查该资源是否已在配置表中（已存在则做修改模式）
            var existingTemplate = ObjectTemplateManager.Instance.GetAllTemplates()
                .FirstOrDefault(t => t.ImagePath.Equals(fileName, StringComparison.OrdinalIgnoreCase));

            FillTemplateFormFields(existingTemplate, fileName, objType);

            // 更新配置表预览
            UpdateConfigPreview(fileName, objType);
        }

        /// <summary>
        /// 清除未配置资源列表的选中高亮
        /// </summary>
        private void ClearResourceSelectionHighlight()
        {
            if (m_UnconfiguredListContainer == null) return;

            foreach (var child in m_UnconfiguredListContainer.Children())
            {
                if (child is Button btn)
                    btn.RemoveFromClassList(MapToolStyleClasses.Selected);
            }
        }

        /// <summary>
        /// 渲染资源预览图与尺寸标签
        /// </summary>
        /// <param name="assetPath">资源资产路径</param>
        private void ShowResourcePreview(string assetPath)
        {
            var previewContainer = m_ResourcePanel.QByName<VisualElement>(MapToolUiNames.ResourcePreviewContainer);
            if (previewContainer == null) return;

            previewContainer.Clear();

            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            if (tex == null)
            {
                previewContainer.Add(new Label("(预览图加载失败)"));
                return;
            }

            previewContainer.Add(new Image { image = tex, scaleMode = ScaleMode.ScaleToFit });

            var sizeLabel = m_ResourcePanel.QByName<Label>(MapToolUiNames.ResourceDetailSize);
            if (sizeLabel != null)
                sizeLabel.text = $"{tex.width} x {tex.height}";
        }

        /// <summary>
        /// 按配置表命中情况填充模板表单：命中为修改模式（回填已有值），未命中为新建模式（填默认值）
        /// </summary>
        /// <param name="existingTemplate">配置表中已存在的模板（可空）</param>
        /// <param name="fileName">图片名（不含扩展名）</param>
        /// <param name="objType">物件类型</param>
        private void FillTemplateFormFields(ObjectTemplate existingTemplate, string fileName, MapObjectType objType)
        {
            var addBtn = m_ResourcePanel.QByName<Button>(MapToolUiNames.BtnAddToConfig);

            if (existingTemplate != null)
            {
                FillFormFromTemplate(existingTemplate);
                if (addBtn != null) addBtn.text = "修改到配置表";
                return;
            }

            FillFormWithDefaults(fileName, objType);
            if (addBtn != null) addBtn.text = "添加到配置表";
        }

        /// <summary>
        /// 修改模式：把已有模板值回填到表单
        /// </summary>
        /// <param name="template">已存在的模板</param>
        private void FillFormFromTemplate(ObjectTemplate template)
        {
            m_FormTemplateIdField.SetValueSafe(template.TemplateId);
            m_FormImagePathField.SetValueSafe(template.ImagePath);
            m_FormTemplateNameField.SetValueSafe(template.TemplateName);
            m_FormAnimNameField.SetValueSafe(template.AnimName);

            m_FormLoopToggle.SetValueSafe(template.Loop);
            m_FormSpeedField.SetValueSafe(template.Speed);
            m_FormSortOrderField.SetValueSafe(template.SortOrder);
            m_FormShadowToggle.SetValueSafe(template.ShadowEnabled);
            m_FormCollectibleToggle.SetValueSafe(template.IsCollectible);
        }

        /// <summary>
        /// 新建模式：按图片名与类型填默认值
        /// </summary>
        /// <param name="fileName">图片名（不含扩展名）</param>
        /// <param name="objType">物件类型</param>
        private void FillFormWithDefaults(string fileName, MapObjectType objType)
        {
            m_FormTemplateIdField.SetValueSafe(GetNextTemplateId(objType));
            m_FormImagePathField.SetValueSafe(fileName);
            m_FormTemplateNameField.SetValueSafe(fileName);
            m_FormAnimNameField.SetValueSafe(MapToolConstants.DefaultAnimName);
        }

        /// <summary>
        /// 更新配置表预览（按图片名与物件类型填充预览标签）
        /// </summary>
        /// <param name="imageName">图片名（不含扩展名）</param>
        /// <param name="objType">物件类型</param>
        private void UpdateConfigPreview(string imageName, MapObjectType objType)
        {
            var previewTemplateId = m_ResourcePanel.QByName<Label>(MapToolUiNames.PreviewTemplateId);
            var previewImagePath = m_ResourcePanel.QByName<Label>(MapToolUiNames.PreviewImagePath);
            var previewTemplateName = m_ResourcePanel.QByName<Label>(MapToolUiNames.PreviewTemplateName);
            var previewAnimName = m_ResourcePanel.QByName<Label>(MapToolUiNames.PreviewAnimName);
            var previewLoop = m_ResourcePanel.QByName<Label>(MapToolUiNames.PreviewLoop);
            var previewSpeed = m_ResourcePanel.QByName<Label>(MapToolUiNames.PreviewSpeed);
            var previewSortOrder = m_ResourcePanel.QByName<Label>(MapToolUiNames.PreviewSortOrder);
            var previewShadow = m_ResourcePanel.QByName<Label>(MapToolUiNames.PreviewShadow);
            var previewCollectible = m_ResourcePanel.QByName<Label>(MapToolUiNames.PreviewCollectible);

            if (previewTemplateId != null) previewTemplateId.text = GetNextTemplateId(objType);
            if (previewImagePath != null) previewImagePath.text = imageName;
            if (previewTemplateName != null) previewTemplateName.text = imageName;
            if (previewAnimName != null) previewAnimName.text = MapToolConstants.DefaultAnimName;
            if (previewLoop != null) previewLoop.text = "true";
            if (previewSpeed != null) previewSpeed.text = "1";
            if (previewSortOrder != null) previewSortOrder.text = MapLayerCatalog.GetSortOrder(objType).ToString();
            if (previewShadow != null) previewShadow.text = "true";
            if (previewCollectible != null) previewCollectible.text = "false";
        }

        /// <summary>
        /// 添加到配置表按钮点击：读取表单 → 二次确认 → 更新或新建模板 → 刷新列表并选中下一个
        /// </summary>
        private void OnAddToConfigClicked()
        {
            if (string.IsNullOrEmpty(m_CurrentSelectedPath))
            {
                MapToolDialog.Info("请先在左侧选择一个资源");
                return;
            }

            TemplateFormInput form = ReadTemplateForm();

            // 检查是否已存在（区分新建/修改模式）
            var existingTemplate = ObjectTemplateManager.Instance.GetAllTemplates()
                .FirstOrDefault(t => t.ImagePath.Equals(form.ImagePath, StringComparison.OrdinalIgnoreCase));
            bool isUpdate = existingTemplate != null;

            // 记录当前资源在列表中的位置，新建模式下添加后自动选中下一个
            int currentIndex = m_UnconfiguredResources.IndexOf(m_CurrentSelectedPath);

            // 二级确认
            string dialogTitle = isUpdate ? "确认修改" : "确认添加";
            string dialogMsg = isUpdate
                ? $"确定要修改以下配置吗？\n\nID: {form.TemplateId}\n图片: {form.ImagePath}\n名称: {form.TemplateName}\n类型: {form.Type}"
                : $"确定要添加以下配置到配置表吗？\n\nID: {form.TemplateId}\n图片: {form.ImagePath}\n名称: {form.TemplateName}\n类型: {form.Type}\n动画: {form.AnimName}";
            bool confirm = MapToolDialog.Confirm(dialogTitle, dialogMsg, isUpdate ? "修改" : "添加", MapToolDialog.ButtonCancel);
            if (!confirm)
            {
                MapToolLog.Info("[ResourceOrganize] 用户取消操作");
                return;
            }

            if (isUpdate)
            {
                ApplyFormToTemplate(existingTemplate, form);
                MapToolLog.Info($"[ResourceOrganize] 已修改: ID={form.TemplateId}, 名称={form.TemplateName}");
            }
            else
            {
                // 图片复制统一在"保存修改"时执行，这里只做内存添加
                ObjectTemplate template = CreateTemplateFromForm(form);
                ObjectTemplateManager.Instance.AddTemplate(template);

                MapToolLog.Info($"[ResourceOrganize] 已添加: ID={form.TemplateId}, 名称={form.TemplateName}");

                // 添加后自动保存到 Excel
                SaveToExcel();
            }

            // 重新扫描并刷新列表
            OnScanResourcesClicked();
            RefreshConfigList();

            // 新建模式下自动选中下一个未配置资源（修改模式不跳转）
            if (!isUpdate && m_UnconfiguredResources.Count > 0)
            {
                int nextIndex = Mathf.Min(currentIndex, m_UnconfiguredResources.Count - 1);
                string nextPath = m_UnconfiguredResources[nextIndex];
                ShowResourceDetail(nextPath);
                HighlightItemByPath(nextPath);
            }
        }

        /// <summary>
        /// 配置表表单输入快照（读取右侧编辑区字段并做空值兜底）
        /// </summary>
        private struct TemplateFormInput
        {
            /// <summary>模板ID</summary>
            public string TemplateId;
            /// <summary>图片名（不含扩展名）</summary>
            public string ImagePath;
            /// <summary>模板名称</summary>
            public string TemplateName;
            /// <summary>动画名</summary>
            public string AnimName;
            /// <summary>是否循环</summary>
            public bool Loop;
            /// <summary>移动速度</summary>
            public float Speed;
            /// <summary>排序值</summary>
            public int SortOrder;
            /// <summary>是否投影</summary>
            public bool Shadow;
            /// <summary>是否可收集</summary>
            public bool Collectible;
            /// <summary>物件类型</summary>
            public MapObjectType Type;
        }

        /// <summary>
        /// 读取右侧编辑区字段为表单快照（空值按默认口径兜底，类型由图片名推断）
        /// </summary>
        /// <returns>表单输入快照</returns>
        private TemplateFormInput ReadTemplateForm()
        {
            var form = new TemplateFormInput
            {
                // 空字符串也要兜底设置默认值
                TemplateId = string.IsNullOrWhiteSpace(m_FormTemplateIdField?.value)
                    ? GetNextTemplateId(MapObjectType.Prop)
                    : m_FormTemplateIdField.value.Trim(),
                ImagePath = string.IsNullOrWhiteSpace(m_FormImagePathField?.value)
                    ? Path.GetFileNameWithoutExtension(m_CurrentSelectedPath)
                    : m_FormImagePathField.value.Trim(),
                AnimName = string.IsNullOrWhiteSpace(m_FormAnimNameField?.value)
                    ? MapToolConstants.DefaultAnimName
                    : m_FormAnimNameField.value.Trim(),
                Loop = m_FormLoopToggle?.value ?? true,
                Speed = m_FormSpeedField?.value ?? 1f,
                SortOrder = m_FormSortOrderField?.value ?? 0,
                Shadow = m_FormShadowToggle?.value ?? true,
                Collectible = m_FormCollectibleToggle?.value ?? false,
            };

            // 模板名称缺省时用图片名兜底
            form.TemplateName = string.IsNullOrWhiteSpace(m_FormTemplateNameField?.value)
                ? form.ImagePath
                : m_FormTemplateNameField.value.Trim();

            // 根据 imagePath 推断类型（覆盖 ID 的类型）
            form.Type = MapToolEditorUtils.GetObjectTypeByImageName(form.ImagePath);

            // 如果 ID 是占位符（template_0001 这种），自动生成正确的数字 ID
            if (!int.TryParse(form.TemplateId, out _))
            {
                form.TemplateId = GetNextTemplateId(form.Type);
            }

            return form;
        }

        /// <summary>
        /// 把表单快照写回已存在的模板（修改模式）
        /// </summary>
        /// <param name="template">目标模板</param>
        /// <param name="form">表单输入快照</param>
        private static void ApplyFormToTemplate(ObjectTemplate template, TemplateFormInput form)
        {
            CopyFormToTemplate(template, form);
        }

        /// <summary>
        /// 用表单快照创建新模板（新建模式，未落盘）
        /// </summary>
        /// <param name="form">表单输入快照</param>
        /// <returns>新模板实例</returns>
        private static ObjectTemplate CreateTemplateFromForm(TemplateFormInput form)
        {
            var template = new ObjectTemplate { IsBuiltIn = false };
            CopyFormToTemplate(template, form);
            return template;
        }

        /// <summary>
        /// 表单快照 → 模板字段（新建与修改共用，字段增删只改本处）
        /// </summary>
        /// <param name="template">目标模板</param>
        /// <param name="form">表单输入快照</param>
        private static void CopyFormToTemplate(ObjectTemplate template, TemplateFormInput form)
        {
            template.TemplateId = form.TemplateId;
            template.ImagePath = form.ImagePath;
            template.TemplateName = form.TemplateName;
            template.AnimName = form.AnimName;
            template.Loop = form.Loop;
            template.Speed = form.Speed;
            template.SortOrder = MapToolEditorUtils.NormalizeTemplateSortOrder(form.Type, form.SortOrder);
            template.ShadowEnabled = form.Shadow;
            template.IsCollectible = form.Collectible;
            template.Type = form.Type;
        }

        /// <summary>
        /// 全部添加到配置表按钮点击
        /// </summary>
        private void OnAddAllToConfigClicked()
        {
            if (m_UnconfiguredResources.Count == 0)
            {
                MapToolDialog.Info("没有未配置的资源，请先扫描");
                return;
            }

            int added = 0;
            // 按类型记录当前 ID 偏移，循环添加时逐个递增
            int buildingIdOffset = 0;
            int charIdOffset = 0;
            int propIdOffset = 0;
            int catIdOffset = 0;

            foreach (var path in m_UnconfiguredResources.ToList())
            {
                string fileName = Path.GetFileNameWithoutExtension(path);

                MapObjectType objType = MapToolEditorUtils.GetObjectTypeByImageName(fileName);

                // 获取基础 ID，再加上当前偏移
                string baseId = GetNextTemplateId(objType);
                int baseNum = int.Parse(baseId);
                string finalId;
                if (objType == MapObjectType.Building)
                {
                    finalId = (baseNum + buildingIdOffset).ToString();
                    buildingIdOffset++;
                }
                else if (objType == MapObjectType.Character)
                {
                    finalId = (baseNum + charIdOffset).ToString();
                    charIdOffset++;
                }
                else if (objType == MapObjectType.Cat)
                {
                    finalId = (baseNum + catIdOffset).ToString();
                    catIdOffset++;
                }
                else
                {
                    finalId = (baseNum + propIdOffset).ToString();
                    propIdOffset++;
                }

                ObjectTemplateManager.Instance.AddTemplate(
                    ObjectTemplateManager.CreateTemplate(finalId, fileName, fileName, objType));
                added++;
            }

            MapToolDialog.Info("完成", $"已添加 {added} 个资源到配置表");
            OnScanResourcesClicked();
        }

        /// <summary>
        /// 获取下一个可用的 TemplateId（数字格式，按当前类型找最大 ID + 1）
        /// </summary>
        private string GetNextTemplateId(MapObjectType type)
        {
            return TemplateIdGenerator.GetNextId(type);
        }

        /// <summary>
        /// Tab 按钮引用（供主窗口 SetActiveTab 使用）
        /// </summary>
        public Button TabResource => m_TabResource;
    }
}
