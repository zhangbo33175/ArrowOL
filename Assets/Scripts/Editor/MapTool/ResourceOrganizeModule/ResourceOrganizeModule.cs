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
            // 先从所有 Excel 配置表重新加载最新数据，确保和磁盘文件同步
            ObjectTemplateManager.Instance.LoadAllConfigTables();

            string mapDir = MapToolConstants.ResourceScanDir;
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
                        case MapToolConstants.FilterBg: return fileName.StartsWith(MapToolConstants.PrefixBg);
                        case MapToolConstants.FilterBuilding: return fileName.StartsWith(MapToolConstants.PrefixBuilding);
                        case MapToolConstants.FilterCharacter: return fileName.StartsWith(MapToolConstants.PrefixCharacter);
                        case MapToolConstants.FilterProp: return fileName.StartsWith(MapToolConstants.PrefixProp);
                        case MapToolConstants.FilterCat: return fileName.StartsWith(MapToolConstants.PrefixCat);
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
                item.userData = path; // 保存路径供后续查找高亮
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
                case MapToolConstants.FilterBg: return MapToolConstants.TableBgFileName;
                case MapToolConstants.FilterBuilding: return MapToolConstants.TableBuildingFileName;
                case MapToolConstants.FilterCharacter: return MapToolConstants.TableCharacterFileName;
                case MapToolConstants.FilterProp: return MapToolConstants.TablePropFileName;
                case MapToolConstants.FilterCat: return MapToolConstants.TableCatFileName;
                default: return "全部配置表";
            }
        }

        /// <summary>
        /// 判断模板是否匹配当前过滤分类
        /// </summary>
        private bool MatchesFilter(ObjectTemplate t, string filter)
        {
            switch (filter)
            {
                case MapToolConstants.FilterBg:
                    return t.Type == MapObjectType.Prop && t.ImagePath.ToLower().StartsWith("bg_");
                case MapToolConstants.FilterBuilding:
                    return t.Type == MapObjectType.Building;
                case MapToolConstants.FilterCharacter:
                    return t.Type == MapObjectType.Character;
                case MapToolConstants.FilterProp:
                    return t.Type == MapObjectType.Prop && !t.ImagePath.ToLower().StartsWith("bg_");
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
        /// 根据资源路径高亮列表项
        /// </summary>
        private void HighlightItemByPath(string path)
        {
            var listContainer = m_ResourcePanel?.Q<VisualElement>(name: "unconfigured-resources-list");
            if (listContainer == null) return;

            foreach (var child in listContainer.Children())
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
                // 显式捕获循环变量，确保删除按钮绑定正确的模板对象
                var capturedTemplate = t;

                var row = new VisualElement();
                row.userData = capturedTemplate; // 保存模板引用，供保存时读取
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
                ConfigTableBuilder.AddEditableCell(row, t.TemplateId, ConfigTableBuilder.ColWidthId, false); // ID 只读
                ConfigTableBuilder.AddEditableCell(row, t.ImagePath, ConfigTableBuilder.ColWidthImagePath, true); // ImagePath 可编辑
                ConfigTableBuilder.AddEditableCell(row, "GameObject_" + t.Type.ToString(), ConfigTableBuilder.ColWidthResName, false); // ResName（固定格式）
                ConfigTableBuilder.AddEditableCell(row, t.TemplateName, ConfigTableBuilder.ColWidthName, true); // TemplateName 可编辑
                ConfigTableBuilder.AddToggleCell(row, t.Loop, ConfigTableBuilder.ColWidthLoop); // Loop Toggle
                ConfigTableBuilder.AddEditableCell(row, t.Speed.ToString("0.#"), ConfigTableBuilder.ColWidthSpeed, true); // Speed 可编辑
                ConfigTableBuilder.AddEditableCell(row, t.SortOrder.ToString(), ConfigTableBuilder.ColWidthSortOrder, true); // SortOrder 可编辑
                ConfigTableBuilder.AddEditableCell(row, t.DialogBubble, ConfigTableBuilder.ColWidthDialog, true); // DialogBubble 可编辑
                ConfigTableBuilder.AddToggleCell(row, t.IsCollectible, ConfigTableBuilder.ColWidthCollectible); // IsCollectible Toggle
                ConfigTableBuilder.AddEditableCell(row, t.RespawnTime.ToString("0.#"), ConfigTableBuilder.ColWidthRespawn, true); // RespawnTime 可编辑

                // 删除按钮
                ConfigTableBuilder.AddDeleteButton(row, () => DeleteTemplate(capturedTemplate));

                container.Add(row);
            }
        }

        /// <summary>
        /// 删除模板并自动补全 ID
        /// </summary>
        private void DeleteTemplate(ObjectTemplate template)
        {
            Debug.Log($"[ResourceOrganize] 删除模板: ID={template.TemplateId}, 名称={template.TemplateName}, 类型={template.Type}");

            // 确认删除
            var confirm = EditorUtility.DisplayDialog(
                "确认删除",
                $"确定要删除模板 {template.TemplateId} ({template.TemplateName}) 吗？\n删除后同类型的 ID 将自动重新排序。",
                "删除", "取消");
            if (!confirm)
            {
                Debug.Log("[ResourceOrganize] 用户取消删除");
                return;
            }

            // 从管理器中删除
            bool removed = ObjectTemplateManager.Instance.RemoveTemplate(template.TemplateId);
            Debug.Log($"[ResourceOrganize] RemoveTemplate 返回: {removed}");

            // 按类型重新排序 ID
            TemplateIdGenerator.ReorderIds(template.Type);

            // 刷新列表
            RefreshConfigList();
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

            // 遍历配置表行，把修改写回模板对象
            var container = m_ResourcePanel?.Q<VisualElement>(name: "config-list-container");
            if (container == null)
            {
                EditorUtility.DisplayDialog("错误", "找不到配置表容器", "确定");
                return;
            }

            int savedCount = 0;
            foreach (var row in container.Children())
            {
                // 跳过非数据行（如"暂无配置数据"提示行）
                var template = row.userData as ObjectTemplate;
                if (template == null) continue;

                // 按顺序提取所有 TextField 和 Toggle
                var textFields = row.Query<TextField>().ToList();
                var toggles = row.Query<Toggle>().ToList();

                // TextField 顺序（与 RefreshConfigList 创建顺序一致）：
                // 0: ImagePath, 1: TemplateName, 2: Speed, 3: SortOrder, 4: DialogBubble, 5: RespawnTime
                if (textFields.Count >= 6)
                {
                    template.ImagePath = textFields[0].value;
                    template.TemplateName = textFields[1].value;

                    float speed;
                    if (float.TryParse(textFields[2].value, out speed))
                        template.Speed = speed;

                    int sortOrder;
                    if (int.TryParse(textFields[3].value, out sortOrder))
                        template.SortOrder = sortOrder;

                    template.DialogBubble = textFields[4].value;

                    float respawnTime;
                    if (float.TryParse(textFields[5].value, out respawnTime))
                        template.RespawnTime = respawnTime;
                }

                // Toggle 顺序：0: Loop, 1: IsCollectible
                if (toggles.Count >= 2)
                {
                    template.Loop = toggles[0].value;
                    template.IsCollectible = toggles[1].value;
                }

                savedCount++;
            }

            // 刷新列表显示
            RefreshConfigList();

            // 将当前分类的模板数据写回 Excel 文件
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
                Debug.Log($"[ResourceOrganize] 保存完成，共写入 {filteredTemplates.Count} 行配置到 Excel，{copiedImages} 张图片已整理到分类文件夹");
                EditorUtility.DisplayDialog("成功",
                    $"配置已保存到 Excel 文件：\n{System.IO.Path.GetFileName(excelPath)}\n\n共更新 {filteredTemplates.Count} 行数据。\n{copiedImages} 张图片已整理到分类文件夹。",
                    "确定");
            }
            else
            {
                Debug.LogWarning($"[ResourceOrganize] Excel 写入失败，已保存到内存");
                EditorUtility.DisplayDialog("部分成功",
                    $"已保存到内存数据，但 Excel 文件写入失败。\n\n可能原因：\n1. Excel 文件正在被占用（请先关闭 Excel）\n2. 文件路径不正确\n\n请关闭 Excel 后重试。",
                    "确定");
            }
        }

        /// <summary>
        /// 把模板对应的图片复制到 Map/{类型}/ 分类文件夹下
        /// </summary>
        /// <returns>是否执行了复制（false 表示已在目标位置或找不到图片）</returns>
        private bool CopyTemplateImageToCategoryFolder(ObjectTemplate template)
        {
            if (string.IsNullOrEmpty(template.ImagePath)) return false;

            // 根据类型确定目标文件夹名
            string targetDirName = template.Type == MapObjectType.Building ? "Building" :
                                    template.Type == MapObjectType.Character ? "Character" :
                                    template.Type == MapObjectType.Cat ? "Cat" : "Prop";
            string targetDir = $"{MapToolConstants.ResourceScanDir}/{targetDirName}";
            string fileName = template.ImagePath;

            // 确保目标文件夹存在
            if (!AssetDatabase.IsValidFolder(targetDir))
            {
                AssetDatabase.CreateFolder(MapToolConstants.ResourceScanDir, targetDirName);
            }

            string targetPath = $"{targetDir}/{fileName}.png";

            // 如果目标位置已有该图片，跳过
            if (AssetDatabase.LoadAssetAtPath<Texture2D>(targetPath) != null)
                return false;

            // 按文件名查找源图片（可能在任意子目录中）
            string[] guids = AssetDatabase.FindAssets($"{fileName} t:Texture2D", new[] { MapToolConstants.ResourceScanDir });
            if (guids.Length == 0) return false;

            string sourcePath = AssetDatabase.GUIDToAssetPath(guids[0]);

            // 如果源文件已经在目标文件夹中，跳过
            if (sourcePath == targetPath) return false;

            // 复制图片
            AssetDatabase.CopyAsset(sourcePath, targetPath);
            Debug.Log($"[ResourceOrganize] 图片已整理: {fileName} → {targetDirName}/");
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

            // 根据当前过滤分类选择对应的配置表文件
            string fileName;
            switch (m_CurrentFilter)
            {
                case MapToolConstants.FilterBuilding:
                    fileName = MapToolConstants.TableBuildingFileName;
                    break;
                case MapToolConstants.FilterCharacter:
                    fileName = MapToolConstants.TableCharacterFileName;
                    break;
                case MapToolConstants.FilterProp:
                    fileName = MapToolConstants.TablePropFileName;
                    break;
                case MapToolConstants.FilterCat:
                    fileName = MapToolConstants.TableCatFileName;
                    break;
                case MapToolConstants.FilterBg:
                    fileName = MapToolConstants.TableBgFileName;
                    break;
                default:
                    fileName = MapToolConstants.TableCharacterFileName;
                    break;
            }

            return System.IO.Path.Combine(projectRoot, MapToolConstants.ConfigTableRootDir, fileName);
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
            string typePrefix = MapToolConstants.FilterProp;
            MapObjectType objType = MapObjectType.Prop;
            if (fileName.StartsWith(MapToolConstants.PrefixBg)) { typePrefix = MapToolConstants.FilterBg; objType = MapObjectType.Prop; }
            else if (fileName.StartsWith(MapToolConstants.PrefixBuilding)) { typePrefix = MapToolConstants.FilterBuilding; objType = MapObjectType.Building; }
            else if (fileName.StartsWith(MapToolConstants.PrefixCharacter)) { typePrefix = "character"; objType = MapObjectType.Character; }
            else if (fileName.StartsWith(MapToolConstants.PrefixCat)) { typePrefix = MapToolConstants.FilterCat; objType = MapObjectType.Cat; }

            // 检查该资源是否已在配置表中（如果已存在则做修改模式）
            var existingTemplate = ObjectTemplateManager.Instance.GetAllTemplates()
                .FirstOrDefault(t => t.ImagePath.Equals(fileName, StringComparison.OrdinalIgnoreCase));

            // 字段填充
            var addBtn = m_ResourcePanel?.Q<Button>(name: "btn-add-to-config");

            if (existingTemplate != null)
            {
                // 已存在 → 修改模式，填充已有数据
                if (templateIdField != null) templateIdField.value = existingTemplate.TemplateId;
                if (imagePathField != null) imagePathField.value = existingTemplate.ImagePath;
                if (templateNameField != null) templateNameField.value = existingTemplate.TemplateName;
                if (animNameField != null) animNameField.value = existingTemplate.AnimName;

                var loopToggle = m_ResourcePanel?.Q<Toggle>(name: "field-loop");
                if (loopToggle != null) loopToggle.value = existingTemplate.Loop;
                var speedField = m_ResourcePanel?.Q<FloatField>(name: "field-speed");
                if (speedField != null) speedField.value = existingTemplate.Speed;
                var sortOrderField = m_ResourcePanel?.Q<IntegerField>(name: "field-sort-order");
                if (sortOrderField != null) sortOrderField.value = existingTemplate.SortOrder;
                var shadowToggle = m_ResourcePanel?.Q<Toggle>(name: "field-shadow");
                if (shadowToggle != null) shadowToggle.value = existingTemplate.ShadowEnabled;
                var collectibleToggle = m_ResourcePanel?.Q<Toggle>(name: "field-collectible");
                if (collectibleToggle != null) collectibleToggle.value = existingTemplate.IsCollectible;

                if (addBtn != null) addBtn.text = "修改到配置表";
            }
            else
            {
                // 不存在 → 新建模式，用默认值
                if (templateIdField != null) templateIdField.value = GetNextTemplateId(objType);
                if (imagePathField != null) imagePathField.value = fileName;
                if (templateNameField != null) templateNameField.value = fileName;
                if (animNameField != null) animNameField.value = "Idle";

                if (addBtn != null) addBtn.text = "添加到配置表";
            }

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

            // 读取字段值（空字符串也要兜底设置默认值）
            string templateId = string.IsNullOrWhiteSpace(templateIdField?.value)
                ? GetNextTemplateId(MapObjectType.Prop)
                : templateIdField.value.Trim();

            string imagePath = string.IsNullOrWhiteSpace(imagePathField?.value)
                ? Path.GetFileNameWithoutExtension(m_CurrentSelectedPath)
                : imagePathField.value.Trim();

            string templateName = string.IsNullOrWhiteSpace(templateNameField?.value)
                ? imagePath
                : templateNameField.value.Trim();

            string animName = string.IsNullOrWhiteSpace(animNameField?.value)
                ? "Idle"
                : animNameField.value.Trim();

            bool loop = loopToggle?.value ?? true;
            float speed = speedField?.value ?? 1f;
            int sortOrder = sortOrderField?.value ?? 10;
            bool shadow = shadowToggle?.value ?? true;
            bool collectible = collectibleToggle?.value ?? false;

            // 根据 imagePath 推断类型（覆盖 ID 的类型）
            MapObjectType objType = MapObjectType.Prop;
            if (imagePath.StartsWith(MapToolConstants.PrefixBuilding)) objType = MapObjectType.Building;
            else if (imagePath.StartsWith(MapToolConstants.PrefixCharacter)) objType = MapObjectType.Character;
            else if (imagePath.StartsWith(MapToolConstants.PrefixCat)) objType = MapObjectType.Cat;

            // 如果 ID 是占位符（template_0001 这种），自动生成正确的数字 ID
            int parsedId;
            if (!int.TryParse(templateId, out parsedId))
            {
                templateId = GetNextTemplateId(objType);
            }

            // 检查是否已存在（区分新建/修改模式）
            var existingTemplate = ObjectTemplateManager.Instance.GetAllTemplates()
                .FirstOrDefault(t => t.ImagePath.Equals(imagePath, StringComparison.OrdinalIgnoreCase));
            bool isUpdate = existingTemplate != null;

            // 记录当前资源在列表中的位置，新建模式下添加后自动选中下一个
            int currentIndex = m_UnconfiguredResources.IndexOf(m_CurrentSelectedPath);

            // 二级确认
            string dialogTitle = isUpdate ? "确认修改" : "确认添加";
            string dialogMsg = isUpdate
                ? $"确定要修改以下配置吗？\n\nID: {templateId}\n图片: {imagePath}\n名称: {templateName}\n类型: {objType}"
                : $"确定要添加以下配置到配置表吗？\n\nID: {templateId}\n图片: {imagePath}\n名称: {templateName}\n类型: {objType}\n动画: {animName}";
            bool confirm = EditorUtility.DisplayDialog(dialogTitle, dialogMsg, isUpdate ? "修改" : "添加", "取消");
            if (!confirm)
            {
                Debug.Log("[ResourceOrganize] 用户取消操作");
                return;
            }

            if (isUpdate)
            {
                // ===== 修改模式：更新已有模板字段 =====
                existingTemplate.TemplateId = templateId;
                existingTemplate.ImagePath = imagePath;
                existingTemplate.TemplateName = templateName;
                existingTemplate.AnimName = animName;
                existingTemplate.Loop = loop;
                existingTemplate.Speed = speed;
                existingTemplate.SortOrder = sortOrder;
                existingTemplate.ShadowEnabled = shadow;
                existingTemplate.IsCollectible = collectible;
                existingTemplate.Type = objType;

                Debug.Log($"[ResourceOrganize] 已修改: ID={templateId}, 名称={templateName}");
            }
            else
            {
                // ===== 新建模式：直接添加到内存模板 =====
                // 图片复制统一在"保存修改"时执行，这里只做内存添加

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
                    IsBuiltIn = false,
                });

                Debug.Log($"[ResourceOrganize] 已添加: ID={templateId}, 名称={templateName}（图片将在保存时整理）");
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
                if (fileName.StartsWith(MapToolConstants.PrefixBuilding)) objType = MapObjectType.Building;
                else if (fileName.StartsWith(MapToolConstants.PrefixCharacter)) objType = MapObjectType.Character;
                else if (fileName.StartsWith(MapToolConstants.PrefixCat)) objType = MapObjectType.Cat;

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
                    IsBuiltIn = false,
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
            return TemplateIdGenerator.GetNextId(type);
        }

        /// <summary>
        /// Tab 按钮引用（供主窗口 SetActiveTab 使用）
        /// </summary>
        public Button TabResource => m_TabResource;
    }
}
