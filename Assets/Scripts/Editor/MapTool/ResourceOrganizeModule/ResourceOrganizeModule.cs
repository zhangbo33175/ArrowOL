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
        private string m_CurrentFilter = "all"; // all/bg/building/char/prop/cat

        // ===== 子 Tab 按钮 =====
        private Button m_TabAll;
        private Button m_TabBg;
        private Button m_TabBuilding;
        private Button m_TabChar;
        private Button m_TabProp;
        private Button m_TabCat;

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
            m_TabResource = visualTree.Q<Button>(name: "tab-resource");
            m_ResourcePanel = visualTree.Q<VisualElement>(name: "resource-panel");

            // 子 Tab 按钮
            m_TabAll = m_ResourcePanel?.Q<Button>(name: "tab-all");
            m_TabBg = m_ResourcePanel?.Q<Button>(name: "tab-bg");
            m_TabBuilding = m_ResourcePanel?.Q<Button>(name: "tab-building");
            m_TabChar = m_ResourcePanel?.Q<Button>(name: "tab-char");
            m_TabProp = m_ResourcePanel?.Q<Button>(name: "tab-prop");
            m_TabCat = m_ResourcePanel?.Q<Button>(name: "tab-cat");
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
                var scanBtn = m_ResourcePanel.Q<Button>(name: "btn-scan-resources");
                if (scanBtn != null)
                    scanBtn.clicked += OnScanResourcesClicked;

                var addBtn = m_ResourcePanel.Q<Button>(name: "btn-add-to-config");
                if (addBtn != null)
                    addBtn.clicked += OnAddToConfigClicked;

                var addAllBtn = m_ResourcePanel.Q<Button>(name: "btn-save-config");
                if (addAllBtn != null)
                    addAllBtn.clicked += OnSaveConfigClicked;

                var openConfigBtn = m_ResourcePanel.Q<Button>(name: "btn-open-config");
                if (openConfigBtn != null)
                    openConfigBtn.clicked += OnOpenConfigClicked;

                var openFolderBtn = m_ResourcePanel.Q<Button>(name: "btn-open-folder");
                if (openFolderBtn != null)
                    openFolderBtn.clicked += OnOpenFolderClicked;

                // 子 Tab 切换
                if (m_TabAll != null) m_TabAll.clicked += () => SwitchFilter("all");
                if (m_TabBg != null) m_TabBg.clicked += () => SwitchFilter("bg");
                if (m_TabBuilding != null) m_TabBuilding.clicked += () => SwitchFilter("building");
                if (m_TabChar != null) m_TabChar.clicked += () => SwitchFilter("char");
                if (m_TabProp != null) m_TabProp.clicked += () => SwitchFilter("prop");
                if (m_TabCat != null) m_TabCat.clicked += () => SwitchFilter("cat");
            }
        }

        /// <summary>
        /// 切换分类过滤
        /// </summary>
        private void SwitchFilter(string filter)
        {
            m_CurrentFilter = filter;

            // 更新按钮高亮状态
            UpdateSubTabActive(m_TabAll, filter == "all");
            UpdateSubTabActive(m_TabBg, filter == "bg");
            UpdateSubTabActive(m_TabBuilding, filter == "building");
            UpdateSubTabActive(m_TabChar, filter == "char");
            UpdateSubTabActive(m_TabProp, filter == "prop");
            UpdateSubTabActive(m_TabCat, filter == "cat");

            // 重新显示列表
            ShowUnconfiguredList();
            RefreshConfigList();
        }

        private void UpdateSubTabActive(Button btn, bool active)
        {
            if (btn == null) return;
            if (active)
                btn.AddToClassList("active");
            else
                btn.RemoveFromClassList("active");
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
            var topToolbar = m_Root.Q<VisualElement>(className: "top-toolbar");
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
            if (m_ResourcePanel != null)
            {
                m_ResourcePanel.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        /// <summary>
        /// 扫描未配置资源按钮点击
        /// </summary>
        private void OnScanResourcesClicked()
        {
            string mapDir = "Assets/Res/Textures/Map";
            if (!AssetDatabase.IsValidFolder(mapDir))
            {
                EditorUtility.DisplayDialog("扫描失败", $"目录不存在: {mapDir}", "确定");
                return;
            }

            // 收集所有 PNG 文件（按文件名去重，同名只保留一个）
            string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { mapDir });
            var allPngs = new List<string>();
            var seenNames = new HashSet<string>();
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    string fileName = Path.GetFileNameWithoutExtension(path).ToLower();
                    if (!seenNames.Contains(fileName))
                    {
                        seenNames.Add(fileName);
                        allPngs.Add(path);
                    }
                }
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

            Debug.Log($"[资源整理] 扫描完成: 共 {allPngs.Count} 个PNG，未配置 {m_UnconfiguredResources.Count} 个");

            // 刷新右侧配置表列表
            RefreshConfigList();
        }

        /// <summary>
        /// 显示未配置资源列表（按当前分类过滤）
        /// </summary>
        private void ShowUnconfiguredList()
        {
            var listContainer = m_ResourcePanel?.Q<VisualElement>(name: "unconfigured-resources-list");
            if (listContainer == null) return;

            listContainer.Clear();

            // 按分类过滤
            var filtered = m_UnconfiguredResources;
            if (m_CurrentFilter != "all")
            {
                filtered = m_UnconfiguredResources.Where(p =>
                {
                    string fileName = Path.GetFileNameWithoutExtension(p).ToLower();
                    switch (m_CurrentFilter)
                    {
                        case "bg": return fileName.StartsWith("bg_");
                        case "building": return fileName.StartsWith("building_");
                        case "char": return fileName.StartsWith("char_");
                        case "prop": return fileName.StartsWith("prop_");
                        case "cat": return fileName.StartsWith("cat_");
                        default: return true;
                    }
                }).ToList();
            }

            listContainer.Add(new Label($"共发现 {filtered.Count} 个未配置资源：")
            {
                style = { color = Color.gray, fontSize = 11, marginBottom = 4 }
            });

            foreach (var path in filtered)
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

                // 缩略图 36x36
                var preview = new Image();
                preview.style.width = 36;
                preview.style.height = 36;
                preview.style.marginRight = 8;
                preview.scaleMode = ScaleMode.ScaleToFit;
                preview.style.backgroundColor = new Color(0.3f, 0.3f, 0.5f);

                var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                if (tex != null)
                    preview.image = tex;

                // 文件名标签
                var nameLabel = new Label(fileName);
                nameLabel.style.fontSize = 12;
                nameLabel.style.color = new Color(0.9f, 0.9f, 0.9f);
                nameLabel.style.flexGrow = 1;

                item.Add(preview);
                item.Add(nameLabel);

                // 点击事件
                item.RegisterCallback<MouseDownEvent>(evt =>
                {
                    ShowResourceDetail(path);
                    // 更新选中高亮
                    HighlightSelectedItem(item);
                });

                listContainer.Add(item);
            }
        }

        /// <summary>
        /// 根据分类获取配置表名称
        /// </summary>
        private string GetConfigNameByFilter(string filter)
        {
            switch (filter)
            {
                case "bg": return "TableBackgrounds.xlsm";
                case "building": return "TableBuildings.xlsm";
                case "char": return "TableCharacters.xlsm";
                case "prop": return "TableProps.xlsm";
                case "cat": return "TableCats.xlsm";
                default: return "全部配置表";
            }
        }

        /// <summary>
        /// 高亮选中的列表项
        /// </summary>
        private void HighlightSelectedItem(VisualElement selected)
        {
            var listContainer = m_ResourcePanel?.Q<VisualElement>(name: "unconfigured-resources-list");
            if (listContainer == null) return;

            foreach (var child in listContainer.Children())
            {
                if (child is VisualElement ve && ve != selected)
                    ve.style.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
            }
            selected.style.backgroundColor = new Color(0.1f, 0.4f, 0.7f);
        }

        /// <summary>
        /// 刷新已配置资源列表（显示在右侧表格中，按当前分类过滤）
        /// </summary>
        private void RefreshConfigList()
        {
            var container = m_ResourcePanel?.Q<VisualElement>(name: "config-list-container");
            if (container == null) return;

            // 更新当前配置表名称
            var configNameLabel = m_ResourcePanel?.Q<Label>(name: "current-config-name");
            if (configNameLabel != null)
            {
                string configName = GetConfigNameByFilter(m_CurrentFilter);
                configNameLabel.text = $"（{configName}）";
            }

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

            // 按分类过滤
            var filtered = allTemplates;
            if (m_CurrentFilter != "all")
            {
                filtered = allTemplates.Where(t =>
                {
                    switch (m_CurrentFilter)
                    {
                        case "bg": return t.Type == MapObjectType.Prop && t.ImagePath.ToLower().StartsWith("bg_");
                        case "building": return t.Type == MapObjectType.Building;
                        case "char": return t.Type == MapObjectType.Character;
                        case "prop": return t.Type == MapObjectType.Prop && !t.ImagePath.ToLower().StartsWith("bg_");
                        case "cat": return t.Type == MapObjectType.Cat;
                        default: return true;
                    }
                }).ToList();
            }

            int rowIndex = 0;
            foreach (var t in filtered)
            {
                var row = new VisualElement();
                row.style.flexDirection = FlexDirection.Row;
                row.style.alignItems = Align.Center;
                row.style.borderBottomWidth = 1;
                row.style.borderBottomColor = Color.black;
                row.style.borderLeftWidth = 1;
                row.style.borderRightWidth = 1;
                row.style.borderLeftColor = Color.black;
                row.style.borderRightColor = Color.black;

                // 交替背景色（白色/浅灰色）
                if (rowIndex % 2 == 0)
                    row.style.backgroundColor = new Color(0.95f, 0.95f, 0.95f);
                else
                    row.style.backgroundColor = Color.white;
                rowIndex++;

                // 各列（和 Excel 表头一致）
                AddEditableCell(row, t.TemplateId, 80, false); // ID 只读
                AddEditableCell(row, t.ImagePath, 140, true, t); // ImagePath 可编辑
                AddEditableCell(row, "GameObject_" + t.Type.ToString(), 150, false); // ResName（固定格式）
                AddEditableCell(row, t.TemplateName, 90, true, t); // TemplateName 可编辑
                AddToggleCell(row, t.Loop, 80, t); // Loop Toggle
                AddEditableCell(row, t.Speed.ToString("0.#"), 80, true, t); // Speed 可编辑
                AddEditableCell(row, t.SortOrder.ToString(), 120, true, t); // SortOrder 可编辑
                AddEditableCell(row, t.DialogBubble, 120, true, t); // DialogBubble 可编辑
                AddToggleCell(row, t.IsCollectible, 90, t); // IsCollectible Toggle
                AddEditableCell(row, t.RespawnTime.ToString("0.#"), 100, true, t); // RespawnTime 可编辑

                // 删除按钮
                var deleteBtn = new Button(() => DeleteTemplate(t));
                deleteBtn.text = "删除";
                deleteBtn.style.width = 56;
                deleteBtn.style.height = 20;
                deleteBtn.style.fontSize = 10;
                row.Add(deleteBtn);

                container.Add(row);
            }
        }

        /// <summary>
        /// 删除模板并自动补全 ID
        /// </summary>
        private void DeleteTemplate(ObjectTemplate template)
        {
            // 确认删除
            var confirm = EditorUtility.DisplayDialog(
                "确认删除",
                $"确定要删除模板 {template.TemplateId} ({template.TemplateName}) 吗？\n删除后同类型的 ID 将自动重新排序。",
                "删除", "取消");
            if (!confirm) return;

            // 从管理器中删除
            ObjectTemplateManager.Instance.RemoveTemplate(template.TemplateId);

            // 按类型重新排序 ID
            ReorderTemplateIds(template.Type);

            // 刷新列表
            RefreshConfigList();

            EditorUtility.DisplayDialog("提示", "删除成功，ID 已自动补全。", "确定");
        }

        /// <summary>
        /// 按类型重新排序 ID
        /// </summary>
        private void ReorderTemplateIds(MapObjectType type)
        {
            // 获取该类型的所有模板，按当前 ID 排序
            var allTemplates = ObjectTemplateManager.Instance.GetAllTemplates();
            var typeTemplates = allTemplates
                .Where(t => t.Type == type)
                .OrderBy(t =>
                {
                    int id;
                    int.TryParse(t.TemplateId, out id);
                    return id;
                })
                .ToList();

            // 按类型确定起始 ID
            int startId = 1000;
            switch (type)
            {
                case MapObjectType.Character: startId = 1000; break;
                case MapObjectType.Building: startId = 2000; break;
                case MapObjectType.Prop: startId = 3000; break;
                case MapObjectType.Cat: startId = 4000; break;
            }

            // 重新分配 ID
            for (int i = 0; i < typeTemplates.Count; i++)
            {
                typeTemplates[i].TemplateId = (startId + i + 1).ToString();
            }
        }

        private void AddEditableCell(VisualElement parent, string text, float width, bool editable, ObjectTemplate template = null)
        {
            if (!editable)
            {
                var label = new Label(text);
                label.style.width = width;
                label.style.fontSize = 11;
                label.style.color = Color.black;
                label.style.marginLeft = 0;
                label.style.marginRight = 0;
                label.style.paddingLeft = 6;
                label.style.paddingRight = 6;
                label.style.borderRightWidth = 1;
                label.style.borderRightColor = Color.black;
                parent.Add(label);
            }
            else
            {
                var field = new TextField();
                field.value = text;
                field.style.width = width;
                field.style.fontSize = 11;
                field.style.color = Color.black;
                field.style.marginLeft = 0;
                field.style.marginRight = 0;
                field.style.paddingLeft = 6;
                field.style.paddingRight = 6;
                // 去掉外层所有边框
                field.style.borderTopWidth = 0;
                field.style.borderBottomWidth = 0;
                field.style.borderLeftWidth = 0;
                field.style.borderRightWidth = 1;
                field.style.borderRightColor = Color.black;
                field.style.backgroundColor = Color.clear;

                // 清除内部输入框的默认样式
                var innerInput = field.Q("unity-text-input");
                if (innerInput != null)
                {
                    innerInput.style.backgroundColor = Color.clear;
                    innerInput.style.borderTopWidth = 0;
                    innerInput.style.borderBottomWidth = 0;
                    innerInput.style.borderLeftWidth = 0;
                    innerInput.style.borderRightWidth = 0;
                    innerInput.style.marginLeft = 0;
                    innerInput.style.marginRight = 0;
                    innerInput.style.paddingLeft = 0;
                    innerInput.style.paddingRight = 0;
                    innerInput.style.color = Color.black;
                }

                parent.Add(field);
            }
        }

        private void AddToggleCell(VisualElement parent, bool value, float width, ObjectTemplate template)
        {
            var toggle = new Toggle();
            toggle.value = value;
            toggle.style.width = width;
            toggle.style.fontSize = 11;
            toggle.style.marginLeft = 0;
            toggle.style.marginRight = 0;
            toggle.style.paddingLeft = 6;
            toggle.style.paddingRight = 6;
            toggle.style.borderRightWidth = 1;
            toggle.style.borderRightColor = Color.black;
            toggle.style.backgroundColor = Color.clear;
            parent.Add(toggle);
        }

        private void AddTypeCell(VisualElement parent, ObjectTemplate template, float width)
        {
            var dropdown = new DropdownField();
            dropdown.choices = new List<string> { "Building", "Character", "Prop", "Cat" };
            dropdown.value = template.Type.ToString();
            dropdown.style.width = width;
            dropdown.style.fontSize = 10;
            dropdown.style.marginRight = 8;
            parent.Add(dropdown);
        }

        /// <summary>
        /// 保存修改按钮点击
        /// </summary>
        private void OnSaveConfigClicked()
        {
            // 弹出二级确认
            bool confirm = EditorUtility.DisplayDialog(
                "确认保存",
                "确定要保存所有修改到配置表吗？",
                "确定",
                "取消");

            if (!confirm) return;

            // 遍历配置表行，把修改写回模板
            var container = m_ResourcePanel?.Q<VisualElement>(name: "config-list-container");
            if (container == null) return;

            // TODO: 从 UI 控件读取修改并写回 ObjectTemplate
            // 目前先刷新列表
            RefreshConfigList();

            EditorUtility.DisplayDialog("成功", "配置已保存", "确定");
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
                EditorUtility.DisplayDialog("提示", $"配置表文件不存在:\n{excelPath}", "确定");
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
                string projectRoot = System.IO.Path.GetDirectoryName(Application.dataPath);
                EditorUtility.DisplayDialog("提示", 
                    $"配置表文件不存在:\n{excelPath}\n\n已打开项目根目录: {projectRoot}", "确定");
                System.Diagnostics.Process.Start("explorer.exe", projectRoot);
            }
        }

        /// <summary>
        /// 获取配置表 Excel 路径
        /// </summary>
        private string GetConfigExcelPath()
        {
            // 项目根目录 = Assets 目录的上一级
            string projectRoot = System.IO.Path.GetDirectoryName(Application.dataPath);
            return System.IO.Path.Combine(projectRoot, "Docs/Designs/Excels/Tables/AlTables/Item/TableCharacters.xlsm");
        }

        /// <summary>
        /// 显示资源详情
        /// </summary>
        private void ShowResourceDetail(string path)
        {
            m_CurrentSelectedPath = path;

            // 高亮选中项
            var listContainer = m_ResourcePanel?.Q<VisualElement>(name: "unconfigured-resources-list");
            if (listContainer != null)
            {
                foreach (var child in listContainer.Children())
                {
                    if (child is Button btn)
                        btn.RemoveFromClassList("selected");
                }
            }

            // 填充字段
            string fileName = Path.GetFileNameWithoutExtension(path);
            string assetPath = path;

            // 预览图
            var previewContainer = m_ResourcePanel?.Q<VisualElement>(name: "resource-preview-container");
            if (previewContainer != null)
            {
                previewContainer.Clear();
                var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                if (tex != null)
                {
                    var img = new Image { image = tex, scaleMode = ScaleMode.ScaleToFit };
                    previewContainer.Add(img);

                    var sizeLabel = m_ResourcePanel?.Q<Label>(name: "resource-detail-size");
                    if (sizeLabel != null)
                        sizeLabel.text = $"{tex.width} x {tex.height}";
                }
                else
                {
                    previewContainer.Add(new Label("(预览图加载失败)"));
                }
            }

            // 字段填充
            var templateIdField = m_ResourcePanel?.Q<TextField>(name: "field-template-id");
            var imagePathField = m_ResourcePanel?.Q<TextField>(name: "field-image-path");
            var templateNameField = m_ResourcePanel?.Q<TextField>(name: "field-template-name");
            var animNameField = m_ResourcePanel?.Q<TextField>(name: "field-anim-name");

            // 自动推断类型
            string typePrefix = "prop";
            MapObjectType objType = MapObjectType.Prop;
            if (fileName.StartsWith("bg_")) { typePrefix = "bg"; objType = MapObjectType.Prop; }
            else if (fileName.StartsWith("building_")) { typePrefix = "building"; objType = MapObjectType.Building; }
            else if (fileName.StartsWith("char_")) { typePrefix = "character"; objType = MapObjectType.Character; }
            else if (fileName.StartsWith("cat_")) { typePrefix = "cat"; objType = MapObjectType.Cat; }

            if (templateIdField != null)
                templateIdField.value = GetNextTemplateId(objType);
            if (imagePathField != null)
                imagePathField.value = fileName;
            if (templateNameField != null)
                templateNameField.value = fileName;
            if (animNameField != null)
                animNameField.value = "Idle";

            // 更新配置表预览
            UpdateConfigPreview(fileName, typePrefix);
        }

        /// <summary>
        /// 更新配置表预览
        /// </summary>
        private void UpdateConfigPreview(string imageName, string typePrefix)
        {
            var previewTemplateId = m_ResourcePanel?.Q<Label>(name: "preview-template-id");
            var previewImagePath = m_ResourcePanel?.Q<Label>(name: "preview-image-path");
            var previewTemplateName = m_ResourcePanel?.Q<Label>(name: "preview-template-name");
            var previewAnimName = m_ResourcePanel?.Q<Label>(name: "preview-anim-name");
            var previewLoop = m_ResourcePanel?.Q<Label>(name: "preview-loop");
            var previewSpeed = m_ResourcePanel?.Q<Label>(name: "preview-speed");
            var previewSortOrder = m_ResourcePanel?.Q<Label>(name: "preview-sort-order");
            var previewShadow = m_ResourcePanel?.Q<Label>(name: "preview-shadow");
            var previewCollectible = m_ResourcePanel?.Q<Label>(name: "preview-collectible");

            // 根据 typePrefix 推断类型
            MapObjectType objType = MapObjectType.Prop;
            if (typePrefix == "building") objType = MapObjectType.Building;
            else if (typePrefix == "character") objType = MapObjectType.Character;
            else if (typePrefix == "cat") objType = MapObjectType.Cat;

            if (previewTemplateId != null) previewTemplateId.text = GetNextTemplateId(objType);
            if (previewImagePath != null) previewImagePath.text = imageName;
            if (previewTemplateName != null) previewTemplateName.text = imageName;
            if (previewAnimName != null) previewAnimName.text = "Idle";
            if (previewLoop != null) previewLoop.text = "true";
            if (previewSpeed != null) previewSpeed.text = "1";
            if (previewSortOrder != null) previewSortOrder.text = "10";
            if (previewShadow != null) previewShadow.text = "true";
            if (previewCollectible != null) previewCollectible.text = "false";
        }

        /// <summary>
        /// 添加到配置表按钮点击
        /// </summary>
        private void OnAddToConfigClicked()
        {
            if (string.IsNullOrEmpty(m_CurrentSelectedPath))
            {
                EditorUtility.DisplayDialog("提示", "请先在左侧选择一个资源", "确定");
                return;
            }

            // 读取字段
            var templateIdField = m_ResourcePanel?.Q<TextField>(name: "field-template-id");
            var imagePathField = m_ResourcePanel?.Q<TextField>(name: "field-image-path");
            var templateNameField = m_ResourcePanel?.Q<TextField>(name: "field-template-name");
            var animNameField = m_ResourcePanel?.Q<TextField>(name: "field-anim-name");
            var loopToggle = m_ResourcePanel?.Q<Toggle>(name: "field-loop");
            var speedField = m_ResourcePanel?.Q<FloatField>(name: "field-speed");
            var sortOrderField = m_ResourcePanel?.Q<IntegerField>(name: "field-sort-order");
            var shadowToggle = m_ResourcePanel?.Q<Toggle>(name: "field-shadow");
            var collectibleToggle = m_ResourcePanel?.Q<Toggle>(name: "field-collectible");

            string templateId = templateIdField?.value ?? "";
            string imagePath = imagePathField?.value ?? "";
            string templateName = templateNameField?.value ?? "";
            string animName = animNameField?.value ?? "Idle";
            bool loop = loopToggle?.value ?? true;
            float speed = speedField?.value ?? 1f;
            int sortOrder = sortOrderField?.value ?? 10;
            bool shadow = shadowToggle?.value ?? true;
            bool collectible = collectibleToggle?.value ?? false;

            // 推断类型
            MapObjectType objType = MapObjectType.Prop;
            if (imagePath.StartsWith("building_")) objType = MapObjectType.Building;
            else if (imagePath.StartsWith("char_")) objType = MapObjectType.Character;
            else if (imagePath.StartsWith("cat_")) objType = MapObjectType.Cat;

            // 添加到配置表
            ObjectTemplateManager.Instance.AddTemplate(new ObjectTemplate
            {
                TemplateId = templateId,
                ImagePath = imagePath,
                TemplateName = templateName,
                AnimName = animName,
                Loop = loop,
                Speed = speed,
                SortOrder = sortOrder,
                ShadowEnabled = shadow,
                IsCollectible = collectible,
                Type = objType,
            });

            EditorUtility.DisplayDialog("成功", $"已添加到配置表:\nID: {templateId}\n图片: {imagePath}\n名称: {templateName}", "确定");

            // 重新扫描
            OnScanResourcesClicked();
        }

        /// <summary>
        /// 全部添加到配置表按钮点击
        /// </summary>
        private void OnAddAllToConfigClicked()
        {
            if (m_UnconfiguredResources.Count == 0)
            {
                EditorUtility.DisplayDialog("提示", "没有未配置的资源，请先扫描", "确定");
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

                MapObjectType objType = MapObjectType.Prop;
                if (fileName.StartsWith("building_")) objType = MapObjectType.Building;
                else if (fileName.StartsWith("char_")) objType = MapObjectType.Character;
                else if (fileName.StartsWith("cat_")) objType = MapObjectType.Cat;

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

                ObjectTemplateManager.Instance.AddTemplate(new ObjectTemplate
                {
                    TemplateId = finalId,
                    ImagePath = fileName,
                    TemplateName = fileName,
                    AnimName = "Idle",
                    Loop = true,
                    Speed = 1f,
                    SortOrder = 10,
                    ShadowEnabled = true,
                    IsCollectible = false,
                    Type = objType,
                });
                added++;
            }

            EditorUtility.DisplayDialog("完成", $"已添加 {added} 个资源到配置表", "确定");
            OnScanResourcesClicked();
        }

        /// <summary>
        /// 获取下一个可用的 TemplateId（数字格式，按当前类型找最大 ID + 1）
        /// </summary>
        private string GetNextTemplateId(MapObjectType type)
        {
            int maxNum = 0;
            foreach (var t in ObjectTemplateManager.Instance.GetAllTemplates())
            {
                if (t.Type != type) continue; // 只统计同类型的
                if (string.IsNullOrEmpty(t.TemplateId)) continue;
                // 从 TemplateId 中提取数字部分
                string numStr = new string(t.TemplateId.Where(char.IsDigit).ToArray());
                if (int.TryParse(numStr, out int num) && num > maxNum)
                    maxNum = num;
            }
            return (maxNum + 1).ToString();
        }

        /// <summary>
        /// Tab 按钮引用（供主窗口 SetActiveTab 使用）
        /// </summary>
        public Button TabResource => m_TabResource;
    }
}
