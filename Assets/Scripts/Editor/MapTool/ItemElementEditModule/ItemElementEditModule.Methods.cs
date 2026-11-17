/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ItemElementEditModule.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   物品元件编辑模块 - 私有方法部分（列表构建/物体注册）
 ***************************************************************/

using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 物品元件编辑模块 - 私有方法
    /// </summary>
    public partial class ItemElementEditModule
    {
        /// <summary>
        /// 绑定事件
        /// </summary>
        private void BindEvents()
        {
            if (m_SaveItemBtn != null)
                m_SaveItemBtn.clicked += OnSaveItemClicked;
            if (m_AddItemBtn != null)
                m_AddItemBtn.clicked += OnAddItemClicked;
            if (m_ImportConfigBtn != null)
                m_ImportConfigBtn.clicked += OnImportConfigClicked;
            if (m_ExportConfigBtn != null)
                m_ExportConfigBtn.clicked += OnExportConfigClicked;

            // 元件类型Tab事件
            if (m_TabBuilding != null)
                m_TabBuilding.clicked += () => OnTypeTabChanged(MapObjectType.Building);
            if (m_TabCharacter != null)
                m_TabCharacter.clicked += () => OnTypeTabChanged(MapObjectType.Character);
            if (m_TabProp != null)
                m_TabProp.clicked += () => OnTypeTabChanged(MapObjectType.Prop);

            // 面板字段写回当前选中内存模板（交通类别/ResName 回调已在控件创建处单独注册）
            BindTemplateFieldCallbacks();
        }

        /// <summary>
        /// 类型Tab切换
        /// </summary>
        private void OnTypeTabChanged(MapObjectType type)
        {
            m_CurrentTypeTab = type;

            // 更新Tab按钮高亮状态
            if (m_TabBuilding != null)
                m_TabBuilding.EnableInClassList("active", type == MapObjectType.Building);
            if (m_TabCharacter != null)
                m_TabCharacter.EnableInClassList("active", type == MapObjectType.Character);
            if (m_TabProp != null)
                m_TabProp.EnableInClassList("active", type == MapObjectType.Prop);

            // 刷新列表
            RefreshItemList();
        }

        /// <summary>
        /// 添加物品按钮点击
        /// </summary>
        private void OnAddItemClicked()
        {
            Debug.Log("添加物品元件");
        }

        /// <summary>
        /// 保存按钮点击：把当前全部元件模板按分类写回对应配置表；成功后通知场景模块同步已摆放实例。
        /// </summary>
        private void OnSaveItemClicked()
        {
            var templates = ObjectTemplateManager.Instance.GetAllTemplates();
            if (templates == null || templates.Count == 0)
            {
                EditorUtility.DisplayDialog("保存元件", "当前没有可保存的元件模板。", "确定");
                return;
            }

            var buildingGroup = templates.Where(t => t.Type == MapObjectType.Building).ToList();
            var characterGroup = templates.Where(t => t.Type == MapObjectType.Character).ToList();
            var propGroup = templates.Where(t => t.Type == MapObjectType.Prop).ToList();
            var catGroup = templates.Where(t => t.Type == MapObjectType.Cat).ToList();

            string projectRoot = Path.GetDirectoryName(Application.dataPath);
            int savedCount = 0;
            bool allOk = true;

            allOk &= WriteTemplateGroup(projectRoot, MapToolConstants.TableBuildingFileName, buildingGroup, ref savedCount);
            allOk &= WriteTemplateGroup(projectRoot, MapToolConstants.TableCharacterFileName, characterGroup, ref savedCount);
            allOk &= WriteTemplateGroup(projectRoot, MapToolConstants.TablePropFileName, propGroup, ref savedCount);
            allOk &= WriteTemplateGroup(projectRoot, MapToolConstants.TableCatFileName, catGroup, ref savedCount);

            if (allOk)
            {
                Debug.Log($"[ItemElementEditModule] 保存全部元件成功，共 {savedCount} 个模板");
                // 通知场景编辑模块：按最新模板同步已摆放实例
                OnTemplatesSaved?.Invoke();
                EditorUtility.DisplayDialog("保存元件", $"已保存 {savedCount} 个元件模板，并同步到场景物件。", "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("保存元件", "部分配置表写入失败，请先关闭占用的 Excel 再重试。", "确定");
            }
        }

        /// <summary>
        /// 把单个分类的模板写回对应配置表（空分类跳过，避免清空既有表）。
        /// </summary>
        private bool WriteTemplateGroup(string projectRoot, string fileName, List<ObjectTemplate> group, ref int savedCount)
        {
            if (group == null || group.Count == 0) return true;
            string excelPath = Path.Combine(projectRoot, MapToolConstants.ConfigTableRootDir, fileName);
            savedCount += group.Count;
            return TemplateConfigWriter.WriteToExcel(excelPath, group);
        }

        /// <summary>
        /// 导入配置按钮点击
        /// </summary>
        private void OnImportConfigClicked()
        {
            // 打开选择文件面板
            string path = EditorUtility.OpenFilePanel(
                "导入元件配置表",
                @"E:\Game\CatGame\CatGameCodeOptimizationBuild\Docs\Designs\Excels\Tables\AlTables\Item",
                "csv,xlsm,xlsx");

            if (string.IsNullOrEmpty(path)) return;

            // 读取配置文件（ReadFromExcel同时支持.xlsm和.csv格式）
            List<ObjectTemplate> templates = TemplateConfigReader.ReadFromExcel(path);

            if (templates == null || templates.Count == 0)
            {
                EditorUtility.DisplayDialog("错误", "配置文件读取失败或为空！\n\n提示：如果是.xlsm文件，请先在Excel中另存为CSV格式。", "确定");
                return;
            }

            // 清空现有模板
            ObjectTemplateManager.Instance.ClearAll();

            // 加载导入的模板
            foreach (var template in templates)
            {
                ObjectTemplateManager.Instance.AddTemplate(template);
            }

            // 刷新列表
            RefreshItemList();

            Debug.Log($"[ItemElementEditModule] 导入配置成功: {path}，共导入 {templates.Count} 个模板");
            EditorUtility.DisplayDialog("成功", $"配置已导入，共 {templates.Count} 个模板！", "确定");
        }

        /// <summary>
        /// 导出配置按钮点击
        /// </summary>
        private void OnExportConfigClicked()
        {
            // 默认保存路径
            string defaultPath = "E:/Game/CatGame/CatGameCodeOptimizationBuild/Docs/Designs/Excels/Tables/AlTables/Item/ItemElementConfig.csv";

            // 打开保存文件面板
            string path = EditorUtility.SaveFilePanel(
                "导出元件配置表",
                Path.GetDirectoryName(defaultPath),
                "ItemElementConfig",
                "csv");

            if (string.IsNullOrEmpty(path)) return;

            // 获取所有模板数据
            var templates = ObjectTemplateManager.Instance.GetAllTemplates();

            // 构建CSV内容
            StringBuilder sb = new StringBuilder();

            // 表头（与配置表格式一致）
            sb.AppendLine("#,ID,Icon,ResName,模板名称,默认动画,是否循环,播放速度,是否可交互,交互类型,渲染层级,层级档,是否有碰撞,对话气泡,气泡ID,初始朝向,是否显示阴影,是否收集物,重生时间,交通类别,启用YSort,YSort偏移,可移动,移动动画,循环重启延迟,离开地图隐藏,是否传送门,目标场景,目标X,目标Y");
            sb.AppendLine("string,string,string,string,string,string,bool,float,bool,string,int,string,bool,string,int,string,bool,bool,float,string,bool,float,bool,string,float,bool,bool,string,float,float");

            // 按类型分组导出
            var buildingTemplates = templates.FindAll(t => t.Type == MapObjectType.Building);
            var characterTemplates = templates.FindAll(t => t.Type == MapObjectType.Character);
            var propTemplates = templates.FindAll(t => t.Type == MapObjectType.Prop);

            int index = 0;

            // 导出建筑
            foreach (var t in buildingTemplates)
            {
                sb.AppendLine($"{index++},{t.TemplateId},{t.ImagePath},{(string.IsNullOrEmpty(t.ResName) ? FallbackResName(t.Type) : t.ResName)},{t.TemplateName},{t.AnimName},{t.Loop},{t.Speed},{t.Interactable},{t.InteractType},{t.SortOrder},{t.DisplayLayer},{t.CollisionEnabled},{t.DialogBubble},{t.BubbleId},{t.FacingDirection},{t.ShadowEnabled},{t.IsCollectible},{t.RespawnTime},{t.TrafficCategory},{t.YSortEnabled},{t.YSortOffset},{t.IsMovable},{t.MoveAnimName},{t.LoopRestartDelay},{t.HideWhenOffMap},{t.IsTeleport},{t.TeleportTargetScene},{t.TeleportTargetX},{t.TeleportTargetY}");
            }

            // 导出人物
            foreach (var t in characterTemplates)
            {
                sb.AppendLine($"{index++},{t.TemplateId},{t.ImagePath},{(string.IsNullOrEmpty(t.ResName) ? FallbackResName(t.Type) : t.ResName)},{t.TemplateName},{t.AnimName},{t.Loop},{t.Speed},{t.Interactable},{t.InteractType},{t.SortOrder},{t.DisplayLayer},{t.CollisionEnabled},{t.DialogBubble},{t.BubbleId},{t.FacingDirection},{t.ShadowEnabled},{t.IsCollectible},{t.RespawnTime},{t.TrafficCategory},{t.YSortEnabled},{t.YSortOffset},{t.IsMovable},{t.MoveAnimName},{t.LoopRestartDelay},{t.HideWhenOffMap},{t.IsTeleport},{t.TeleportTargetScene},{t.TeleportTargetX},{t.TeleportTargetY}");
            }

            // 导出物件
            foreach (var t in propTemplates)
            {
                sb.AppendLine($"{index++},{t.TemplateId},{t.ImagePath},{(string.IsNullOrEmpty(t.ResName) ? FallbackResName(t.Type) : t.ResName)},{t.TemplateName},{t.AnimName},{t.Loop},{t.Speed},{t.Interactable},{t.InteractType},{t.SortOrder},{t.DisplayLayer},{t.CollisionEnabled},{t.DialogBubble},{t.BubbleId},{t.FacingDirection},{t.ShadowEnabled},{t.IsCollectible},{t.RespawnTime},{t.TrafficCategory},{t.YSortEnabled},{t.YSortOffset},{t.IsMovable},{t.MoveAnimName},{t.LoopRestartDelay},{t.HideWhenOffMap},{t.IsTeleport},{t.TeleportTargetScene},{t.TeleportTargetX},{t.TeleportTargetY}");
            }

            // 写入文件
            File.WriteAllText(path, sb.ToString(), System.Text.Encoding.UTF8);

            Debug.Log($"[ItemElementEditModule] 导出配置成功: {path}，共导出 {templates.Count} 个模板");
            EditorUtility.DisplayDialog("成功", $"配置已导出到：\n{path}\n\n共 {templates.Count} 个模板", "确定");
        }

        /// <summary>
        /// 创建元件列表项
        /// </summary>
        private VisualElement CreateItemElement(ObjectTemplate template)
        {
            var item = new VisualElement();
            item.AddToClassList("item-element-item");

            // 图标 - 加载实际预览图，加载失败则用类型色占位
            var icon = new Image();
            icon.AddToClassList("item-icon");
            icon.scaleMode = ScaleMode.ScaleToFit;
            // 强制固定尺寸，防止不同大小的图片撑开行高
            icon.style.width = 52;
            icon.style.height = 52;
            icon.style.overflow = Overflow.Hidden;
            icon.style.flexShrink = 0;

            // 根据文件名查找完整资源路径（图片可能在多级子目录中）
            Texture tex = MapToolEditorUtils.LoadTextureByName(template.ImagePath);

            if (tex != null)
            {
                icon.image = tex;
            }
            else
            {
                // 加载失败时用类型色占位
                if (template.Type == MapObjectType.Building)
                    icon.style.backgroundColor = new Color(0.27f, 0.45f, 0.78f); // 蓝色
                else if (template.Type == MapObjectType.Character)
                    icon.style.backgroundColor = new Color(0.44f, 0.68f, 0.28f); // 绿色
                else
                    icon.style.backgroundColor = new Color(1.0f, 0.75f, 0.0f); // 黄色
            }
            item.Add(icon);

            // 名称和描述
            var textArea = new VisualElement();
            textArea.AddToClassList("item-text-area");

            var nameLabel = new Label(template.TemplateName);
            nameLabel.AddToClassList("item-name");
            textArea.Add(nameLabel);

            string typeName = template.Type == MapObjectType.Building ? "建筑" :
                              template.Type == MapObjectType.Character ? "人物" : "物件";
            string desc = $"{template.TemplateId} - {typeName} - {template.InteractType}";
            var descLabel = new Label(desc);
            descLabel.AddToClassList("item-desc");
            textArea.Add(descLabel);

            item.Add(textArea);

            // 显示复选框（只显示勾选框，不显示文字）
            var showToggle = new Toggle();
            showToggle.value = true;
            showToggle.AddToClassList("item-show-toggle");
            item.Add(showToggle);

            // 点击选中（直接捕获模板引用，避免依赖列表位置下标）
            item.RegisterCallback<ClickEvent>(evt => OnItemSelected(template, item));
            return item;
        }

        /// <summary>
        /// 元件选中
        /// </summary>
        private void OnItemSelected(ObjectTemplate template, VisualElement item)
        {
            if (template == null) return;
            m_SelectedTemplateId = template.TemplateId;
            // 移除所有选中状态
            foreach (var itemElement in m_ItemItems)
                itemElement.RemoveFromClassList("selected");
            // 添加当前选中状态
            item.AddToClassList("selected");

            // 直接回填点击项对应的模板（不按位置下标反查快照）
            LoadTemplateToPanel(template);

            Debug.Log($"选中元件: {template.TemplateId}");
        }

        /// <summary>
        /// 交通类别下拉变更：把所选中文分类写回当前选中模板的交通类别键
        /// </summary>
        /// <param name="evt">下拉值变更事件（值为中文显示名）</param>
        private void OnTrafficCategoryChanged(ChangeEvent<string> evt)
        {
            var template = GetCurrentTemplate();
            if (template == null) return;
            template.TrafficCategory = TemplateTrafficCategory.ToKey(evt.newValue);
            ApplyGroupConditionalVisibility(template);
        }

        /// <summary>Model 预制体目录（相对 Assets）</summary>
        private const string ModelPrefabDir = "Res/Prefabs/Model";

        /// <summary>资源预制体下拉变更：写回当前模板 ResName</summary>
        private void OnResNameChanged(ChangeEvent<string> evt)
        {
            var template = GetCurrentTemplate();
            if (template == null) return;
            template.ResName = evt.newValue ?? string.Empty;
        }

        /// <summary>
        /// 按当前选中模板的唯一ID反查内存模板；未选中或已被移除返回 null。
        /// 不使用字典快照的位置下标，避免列表重建/字典重排后下标错位。
        /// </summary>
        private ObjectTemplate GetCurrentTemplate()
        {
            if (string.IsNullOrEmpty(m_SelectedTemplateId)) return null;
            return ObjectTemplateManager.Instance.GetTemplateById(m_SelectedTemplateId);
        }

        /// <summary>绑定布尔勾选框变更：写回当前选中模板。</summary>
        private void BindToggle(Toggle toggle, Action<ObjectTemplate, bool> apply)
        {
            if (toggle == null) return;
            toggle.RegisterValueChangedCallback(evt =>
            {
                var tpl = GetCurrentTemplate();
                if (tpl != null) apply(tpl, evt.newValue);
            });
        }

        /// <summary>绑定文本框变更：写回当前选中模板。</summary>
        private void BindText(TextField field, Action<ObjectTemplate, string> apply)
        {
            if (field == null) return;
            field.RegisterValueChangedCallback(evt =>
            {
                var tpl = GetCurrentTemplate();
                if (tpl != null) apply(tpl, evt.newValue);
            });
        }

        /// <summary>绑定下拉框变更：写回当前选中模板。</summary>
        private void BindDropdown(DropdownField field, Action<ObjectTemplate, string> apply)
        {
            if (field == null) return;
            field.RegisterValueChangedCallback(evt =>
            {
                var tpl = GetCurrentTemplate();
                if (tpl != null) apply(tpl, evt.newValue);
            });
        }

        /// <summary>绑定整数框变更：写回当前选中模板。</summary>
        private void BindInt(IntegerField field, Action<ObjectTemplate, int> apply)
        {
            if (field == null) return;
            field.RegisterValueChangedCallback(evt =>
            {
                var tpl = GetCurrentTemplate();
                if (tpl != null) apply(tpl, evt.newValue);
            });
        }

        /// <summary>浮点解析容错：解析失败保留模板原值。</summary>
        private static float ParseFloat(string raw, float fallback)
        {
            return float.TryParse(raw, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : fallback;
        }

        /// <summary>整数解析容错：解析失败保留模板原值。</summary>
        private static int ParseInt(string raw, int fallback)
        {
            return int.TryParse(raw, System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : fallback;
        }

        /// <summary>
        /// 注册面板各模板字段的写回回调；选中模板时回填使用 SetValueWithoutNotify，不会误触发本批回调。
        /// 交通类别、ResName 已在控件创建处单独注册。
        /// </summary>
        private void BindTemplateFieldCallbacks()
        {
            // 基础信息（ID 只读；名称可改并回写模板）
            BindText(m_ItemNameField, (t, v) => t.TemplateName = v);

            // 动画设置
            BindText(m_ItemAnimField, (t, v) => t.AnimName = v);
            BindToggle(m_ItemLoopToggle, (t, v) => t.Loop = v);
            BindText(m_ItemSpeedField, (t, v) => t.Speed = ParseFloat(v, t.Speed));

            // 交互设置
            BindToggle(m_ItemInteractableToggle, (t, v) => t.Interactable = v);
            BindDropdown(m_ItemInteractTypeField, (t, v) => t.InteractType = TemplateInteractType.ToCode(v));
            BindText(m_ItemBubbleField, (t, v) => t.DialogBubble = v);
            BindInt(m_ItemBubbleIdField, (t, v) => t.BubbleId = v);

            // 层级与碰撞
            BindText(m_ItemSortOrderField, (t, v) => t.SortOrder = MapToolEditorUtils.ClampTemplateSortOrder(t, ParseInt(v, t.SortOrder)));
            BindDropdown(m_ItemFacingField, (t, v) => t.FacingDirection = TemplateFacing.ToCode(v));
            BindDropdown(m_ItemLayerField, (t, v) => t.DisplayLayer = v == "跟随类型" ? string.Empty : MapLayerCatalog.GetLayerKeyByDisplayName(v));
            BindToggle(m_ItemCollisionToggle, (t, v) => t.CollisionEnabled = v);
            BindToggle(m_ItemShadowToggle, (t, v) => t.ShadowEnabled = v);

            // 收集物设置
            BindToggle(m_ItemCollectibleToggle, (t, v) => { t.IsCollectible = v; ApplyGroupConditionalVisibility(t); });
            BindText(m_ItemRespawnField, (t, v) => t.RespawnTime = ParseFloat(v, t.RespawnTime));

            // Y坐标深度排序
            BindToggle(m_ItemYSortToggle, (t, v) => t.YSortEnabled = v);
            BindText(m_ItemYSortOffsetField, (t, v) => t.YSortOffset = ParseFloat(v, t.YSortOffset));

            // 移动与循环
            BindToggle(m_ItemMovableToggle, (t, v) => { t.IsMovable = v; ApplyGroupConditionalVisibility(t); });
            BindText(m_ItemMoveAnimField, (t, v) => t.MoveAnimName = v);
            BindText(m_ItemLoopRestartField, (t, v) => t.LoopRestartDelay = ParseFloat(v, t.LoopRestartDelay));
            BindToggle(m_ItemHideOffMapToggle, (t, v) => t.HideWhenOffMap = v);

            // 传送门设置
            BindToggle(m_ItemTeleportToggle, (t, v) => { t.IsTeleport = v; ApplyGroupConditionalVisibility(t); });
            BindText(m_ItemTeleportSceneField, (t, v) => t.TeleportTargetScene = v);
            BindText(m_ItemTeleportXField, (t, v) => t.TeleportTargetX = ParseFloat(v, t.TeleportTargetX));
            BindText(m_ItemTeleportYField, (t, v) => t.TeleportTargetY = ParseFloat(v, t.TeleportTargetY));
        }

        /// <summary>
        /// 扫描 Model 目录下全部预制体文件名（不含扩展名）作为下拉选项；
        /// 目录缺失或为空时返回空占位项。
        /// </summary>
        private List<string> CollectModelPrefabNames()
        {
            var names = new List<string>();
            var dir = Path.Combine(Application.dataPath, ModelPrefabDir);
            if (Directory.Exists(dir))
            {
                foreach (var file in Directory.GetFiles(dir, "*.prefab"))
                {
                    var name = Path.GetFileNameWithoutExtension(file);
                    if (!string.IsNullOrEmpty(name) && !names.Contains(name)) names.Add(name);
                }
            }
            if (names.Count == 0) names.Add(string.Empty);
            names.Sort();
            return names;
        }

        /// <summary>分类对应的预制体名关键词。</summary>
        private static string TypeKeyword(MapObjectType type)
        {
            switch (type)
            {
                case MapObjectType.Building: return "Building";
                case MapObjectType.Character: return "Character";
                default: return "Prop";
            }
        }

        /// <summary>判断资源名是否与当前分类一致（含对应分类关键词）。</summary>
        private static bool ResNameMatchesType(string resName, MapObjectType type)
        {
            if (string.IsNullOrEmpty(resName)) return false;
            return resName.IndexOf(TypeKeyword(type), StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// 按当前分类从目录预制体选项中匹配默认资源名；匹配不到时返回历史占位兜底。
        /// </summary>
        /// <param name="type">物体分类</param>
        /// <param name="choices">目录预制体下拉选项</param>
        private string PickDefaultResName(MapObjectType type, List<string> choices)
        {
            string keyword = TypeKeyword(type);
            if (choices != null)
            {
                foreach (var choice in choices)
                {
                    if (!string.IsNullOrEmpty(choice) &&
                        choice.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                        return choice;
                }
            }
            return FallbackResName(type);
        }

        /// <summary>
        /// ResName 为空时按物体类型给出历史占位资源名。
        /// </summary>
        private static string FallbackResName(MapObjectType type)
        {
            switch (type)
            {
                case MapObjectType.Building: return "GameObject_Building";
                case MapObjectType.Character: return "GameObject_Character";
                default: return "GameObject_Prop";
            }
        }

        /// <summary>
        /// 切换分组内容容器显隐
        /// </summary>
        /// <param name="content">分组内容容器</param>
        /// <param name="visible">是否展开</param>
        private static void SetGroupVisible(VisualElement content, bool visible)
        {
            if (content != null)
                content.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        /// <summary>
        /// 条件显隐整组（标题头 + 内容容器）；未设置对应属性时整组隐藏，与场景右栏收集/交通分组口径一致。
        /// </summary>
        private void SetGroupVisible(string headerName, VisualElement content, bool visible)
        {
            var header = m_Root.Q<VisualElement>(name: headerName);
            if (header != null) header.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            if (content != null) content.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        /// <summary>
        /// <summary>
        /// 模板面板的交通/收集/移动/传送门四组功能字段常驻可见可编辑：
        /// 主开关勾选仅写回模板数据，不再按 bool 整组隐藏分组；分组标题仍可通过折叠头手动收起。
        /// </summary>
        private void ApplyGroupConditionalVisibility(ObjectTemplate template)
        {
            if (template == null) return;
            SetGroupVisible("item-collectible-group-header", m_CollectibleContent, true);
            SetGroupVisible("item-traffic-group-header", m_TrafficContent, true);
            SetGroupVisible("item-teleport-group-header", m_TeleportContent, true);
            SetGroupVisible("item-movable-group-header", m_MovableContent, true);
        }

        /// <summary>
        /// 绑定分组标题折叠头：点击标题行仅切换本组内容显隐并翻转箭头。
        /// 纯 UI 折叠，不写回模板、不影响组内业务功能勾选框。
        /// </summary>
        /// <param name="headerName">折叠头容器名</param>
        /// <param name="content">分组内容容器</param>
        /// <param name="arrowName">箭头标签名</param>
        private void BindGroupFoldout(string headerName, VisualElement content, string arrowName)
        {
            var header = m_Root.Q<VisualElement>(name: headerName);
            var arrow = m_Root.Q<Label>(name: arrowName);
            if (header == null || content == null) return;

            header.RegisterCallback<ClickEvent>(evt =>
            {
                // 当前是否处于展开态；点击后取反切换
                bool expanded = content.resolvedStyle.display != DisplayStyle.None;
                bool nextExpanded = !expanded;
                SetGroupVisible(content, nextExpanded);
                if (arrow != null) arrow.text = nextExpanded ? "▼" : "▶";
            });
        }

        /// <summary>
        /// 加载模板数据到属性面板
        /// </summary>
        private void LoadTemplateToPanel(ObjectTemplate template)
        {
            if (template == null) return;

            // 基本信息
            if (m_ItemIdField != null)
                m_ItemIdField.SetValueWithoutNotify(template.TemplateId);
            if (m_ItemTypeField != null)
            {
                string typeName = template.Type == MapObjectType.Building ? "建筑" :
                                  template.Type == MapObjectType.Character ? "人物" : "物件";
                m_ItemTypeField.SetValueWithoutNotify(typeName);
            }
            if (m_ItemNameField != null)
                m_ItemNameField.SetValueWithoutNotify(template.TemplateName);
            if (m_ItemIconField != null)
                m_ItemIconField.SetValueWithoutNotify(template.ImagePath);
            if (m_ItemResNameField != null)
            {
                // 资源名与当前分类一致才保留；空值、历史占位或分类错配时按分类匹配默认预制体并写回
                if (!ResNameMatchesType(template.ResName, template.Type))
                    template.ResName = PickDefaultResName(template.Type, m_ItemResNameField.choices);
                if (!m_ItemResNameField.choices.Contains(template.ResName)) m_ItemResNameField.choices.Add(template.ResName);
                m_ItemResNameField.SetValueWithoutNotify(template.ResName);
            }

            // 动画设置
            if (m_ItemAnimField != null)
                m_ItemAnimField.SetValueWithoutNotify(template.AnimName);
            if (m_ItemLoopToggle != null)
                m_ItemLoopToggle.SetValueWithoutNotify(template.Loop);
            if (m_ItemSpeedField != null)
                m_ItemSpeedField.SetValueWithoutNotify(template.Speed.ToString("F1"));

            // 交互设置
            if (m_ItemInteractableToggle != null)
                m_ItemInteractableToggle.SetValueWithoutNotify(template.Interactable);
            if (m_ItemInteractTypeField != null)
                m_ItemInteractTypeField.SetValueWithoutNotify(TemplateInteractType.ToDisplay(template.InteractType));
            if (m_ItemBubbleField != null)
                m_ItemBubbleField.SetValueWithoutNotify(template.DialogBubble);
            if (m_ItemBubbleIdField != null)
                m_ItemBubbleIdField.SetValueWithoutNotify(template.BubbleId);

            // 层级与碰撞
            if (m_ItemSortOrderField != null)
                m_ItemSortOrderField.SetValueWithoutNotify(template.SortOrder.ToString());
            if (m_ItemFacingField != null)
                m_ItemFacingField.SetValueWithoutNotify(TemplateFacing.ToDisplay(template.FacingDirection));
            if (m_ItemLayerField != null)
            {
                var layerDisplay = string.IsNullOrWhiteSpace(template.DisplayLayer)
                    ? "跟随类型"
                    : MapLayerCatalog.GetLayerDisplayName(template.DisplayLayer);
                if (!m_ItemLayerField.choices.Contains(layerDisplay)) m_ItemLayerField.choices.Add(layerDisplay);
                m_ItemLayerField.SetValueWithoutNotify(layerDisplay);
            }
            if (m_ItemCollisionToggle != null)
                m_ItemCollisionToggle.SetValueWithoutNotify(template.CollisionEnabled);
            if (m_ItemShadowToggle != null)
                m_ItemShadowToggle.SetValueWithoutNotify(template.ShadowEnabled);

            // 交通规则
            if (m_ItemTrafficCategoryField != null)
                m_ItemTrafficCategoryField.SetValueWithoutNotify(TemplateTrafficCategory.ToDisplayName(template.TrafficCategory));

            // 收集物设置
            if (m_ItemCollectibleToggle != null)
                m_ItemCollectibleToggle.SetValueWithoutNotify(template.IsCollectible);
            if (m_ItemRespawnField != null)
                m_ItemRespawnField.SetValueWithoutNotify(template.RespawnTime.ToString("F1"));

            // Y坐标深度排序
            if (m_ItemYSortToggle != null)
                m_ItemYSortToggle.SetValueWithoutNotify(template.YSortEnabled);
            if (m_ItemYSortOffsetField != null)
                m_ItemYSortOffsetField.SetValueWithoutNotify(template.YSortOffset.ToString("F1"));

            // 移动与循环
            if (m_ItemMovableToggle != null)
                m_ItemMovableToggle.SetValueWithoutNotify(template.IsMovable);
            if (m_ItemMoveAnimField != null)
                m_ItemMoveAnimField.SetValueWithoutNotify(template.MoveAnimName);
            if (m_ItemLoopRestartField != null)
                m_ItemLoopRestartField.SetValueWithoutNotify(template.LoopRestartDelay.ToString("F1"));
            if (m_ItemHideOffMapToggle != null)
                m_ItemHideOffMapToggle.SetValueWithoutNotify(template.HideWhenOffMap);

            // 传送门设置
            if (m_ItemTeleportToggle != null)
                m_ItemTeleportToggle.SetValueWithoutNotify(template.IsTeleport);
            if (m_ItemTeleportSceneField != null)
                m_ItemTeleportSceneField.SetValueWithoutNotify(template.TeleportTargetScene);
            if (m_ItemTeleportXField != null)
                m_ItemTeleportXField.SetValueWithoutNotify(template.TeleportTargetX.ToString());
            if (m_ItemTeleportYField != null)
                m_ItemTeleportYField.SetValueWithoutNotify(template.TeleportTargetY.ToString());

            // 按模板属性切换收集/交通/传送/移动条件分组显隐
            ApplyGroupConditionalVisibility(template);
        }

        /// <summary>
        /// 查找UI元素
        /// </summary>
        private void FindElements()
        {
            // 顶部工具栏
            m_SaveItemBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnSaveItem);
            m_AddItemBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnAddItem);
            m_ImportConfigBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnImportConfig);
            m_ExportConfigBtn = m_Root.Q<Button>(name: MapToolUiNames.BtnExportConfig);

            // 元件类型Tab
            m_TabBuilding = m_Root.Q<Button>(name: "item-tab-building");
            m_TabCharacter = m_Root.Q<Button>(name: "item-tab-character");
            m_TabProp = m_Root.Q<Button>(name: "item-tab-prop");

            // 元件列表
            m_ItemList = m_Root.Q<ScrollView>(name: "item-list");
            // 元件属性面板
            m_ItemIdField = m_Root.Q<TextField>(name: "item-id-field");
            m_ItemNameField = m_Root.Q<TextField>(name: "item-name-field");
            m_ItemIconField = m_Root.Q<TextField>(name: "item-icon-field");
            // ID 与 Icon 仅作展示，不允许在面板上修改
            if (m_ItemIdField != null) m_ItemIdField.isReadOnly = true;
            if (m_ItemIconField != null) m_ItemIconField.isReadOnly = true;
            m_ItemAnimField = m_Root.Q<TextField>(name: "item-anim-field");
            m_ItemLoopToggle = m_Root.Q<Toggle>(name: "item-loop-toggle");
            m_ItemSpeedField = m_Root.Q<TextField>(name: "item-speed-field");
            m_ItemInteractableToggle = m_Root.Q<Toggle>(name: "item-interactable-toggle");
            m_ItemBubbleField = m_Root.Q<TextField>(name: "item-bubble-field");
            m_ItemBubbleIdField = m_Root.Q<IntegerField>(name: "item-bubble-id-field");
            m_ItemSortOrderField = m_Root.Q<TextField>(name: "item-sort-order-field");
            m_ItemCollisionToggle = m_Root.Q<Toggle>(name: "item-collision-toggle");
            m_ItemShadowToggle = m_Root.Q<Toggle>(name: "item-shadow-toggle");
            m_ItemCollectibleToggle = m_Root.Q<Toggle>(name: "item-collectible-toggle");
            m_ItemRespawnField = m_Root.Q<TextField>(name: "item-respawn-field");
            m_ItemYSortToggle = m_Root.Q<Toggle>(name: "item-ysort-toggle");
            m_ItemYSortOffsetField = m_Root.Q<TextField>(name: "item-ysort-offset-field");
            m_ItemMovableToggle = m_Root.Q<Toggle>(name: "item-movable-toggle");
            m_ItemMoveAnimField = m_Root.Q<TextField>(name: "item-move-anim-field");
            m_ItemLoopRestartField = m_Root.Q<TextField>(name: "item-loop-restart-field");
            m_ItemHideOffMapToggle = m_Root.Q<Toggle>(name: "item-hide-offmap-toggle");
            m_ItemTeleportToggle = m_Root.Q<Toggle>(name: "item-teleport-toggle");
            m_ItemTeleportSceneField = m_Root.Q<TextField>(name: "item-teleport-scene-field");
            m_ItemTeleportXField = m_Root.Q<TextField>(name: "item-teleport-x-field");
            m_ItemTeleportYField = m_Root.Q<TextField>(name: "item-teleport-y-field");

            // 分组内容容器（随标题独立折叠头显隐）
            m_AnimContent = m_Root.Q<VisualElement>(name: "item-anim-content");
            m_InteractableContent = m_Root.Q<VisualElement>(name: "item-interactable-content");
            m_LayerContent = m_Root.Q<VisualElement>(name: "item-layer-content");
            m_TrafficContent = m_Root.Q<VisualElement>(name: "item-traffic-content");
            m_CollectibleContent = m_Root.Q<VisualElement>(name: "item-collectible-content");
            m_YSortContent = m_Root.Q<VisualElement>(name: "item-ysort-content");
            m_MovableContent = m_Root.Q<VisualElement>(name: "item-movable-content");
            m_TeleportContent = m_Root.Q<VisualElement>(name: "item-teleport-content");

            // 类型下拉框
            var typeContainer = m_Root.Q<VisualElement>(name: "item-type-container");
            if (typeContainer != null)
            {
                m_ItemTypeField = new DropdownField();
                m_ItemTypeField.choices = new List<string> { "建筑", "人物", "物件" };
                m_ItemTypeField.value = "物件";
                m_ItemTypeField.style.flexGrow = 1;
                typeContainer.Add(m_ItemTypeField);
            }

            // 交互类型下拉框
            var interactTypeContainer = m_Root.Q<VisualElement>(name: "item-interact-type-container");
            if (interactTypeContainer != null)
            {
                m_ItemInteractTypeField = new DropdownField();
                m_ItemInteractTypeField.choices = new List<string> { "无", "点击", "对话", "进入", "收集" };
                m_ItemInteractTypeField.value = "无";
                m_ItemInteractTypeField.style.flexGrow = 1;
                interactTypeContainer.Add(m_ItemInteractTypeField);
            }

            // 朝向下拉框
            var facingContainer = m_Root.Q<VisualElement>(name: "item-facing-container");
            if (facingContainer != null)
            {
                m_ItemFacingField = new DropdownField();
                m_ItemFacingField.choices = new List<string> { "上", "下", "左", "右" };
                m_ItemFacingField.value = "下";
                m_ItemFacingField.style.flexGrow = 1;
                facingContainer.Add(m_ItemFacingField);
            }

            // 层级档下拉框：跟随类型 + 场景编辑器同款八层选项
            var layerContainer = m_Root.Q<VisualElement>(name: "item-layer-container");
            if (layerContainer != null)
            {
                m_ItemLayerField = new DropdownField();
                var layerChoices = new List<string> { "跟随类型" };
                layerChoices.AddRange(MapLayerCatalog.AllLayerTypes.Select(MapLayerCatalog.GetLayerDisplayName));
                m_ItemLayerField.choices = layerChoices;
                m_ItemLayerField.value = "跟随类型";
                m_ItemLayerField.style.flexGrow = 1;
                layerContainer.Add(m_ItemLayerField);
            }

            // 交通类别下拉框
            var trafficCategoryContainer = m_Root.Q<VisualElement>(name: MapToolUiNames.ItemTrafficCategoryContainer);
            if (trafficCategoryContainer != null)
            {
                m_ItemTrafficCategoryField = new DropdownField();
                m_ItemTrafficCategoryField.choices = TemplateTrafficCategory.AllDisplayNames;
                m_ItemTrafficCategoryField.value = TemplateTrafficCategory.ToDisplayName(TemplateTrafficCategory.None);
                m_ItemTrafficCategoryField.style.flexGrow = 1;
                trafficCategoryContainer.Add(m_ItemTrafficCategoryField);
                m_ItemTrafficCategoryField.RegisterValueChangedCallback(OnTrafficCategoryChanged);
            }

            // 资源预制体下拉框：选项来自 Res/Prefabs/Model 目录下的预制体文件名
            var resNameContainer = m_Root.Q<VisualElement>(name: MapToolUiNames.ItemResNameContainer);
            if (resNameContainer != null)
            {
                m_ItemResNameField = new DropdownField();
                m_ItemResNameField.choices = CollectModelPrefabNames();
                m_ItemResNameField.style.flexGrow = 1;
                resNameContainer.Add(m_ItemResNameField);
                m_ItemResNameField.RegisterValueChangedCallback(OnResNameChanged);
            }

            // 分组独立折叠头：点击标题行仅切换本组内容显隐（纯 UI 折叠，不绑定业务字段；
            // 组内的功能勾选框仍是独立业务开关，回组内原位配置）
            BindGroupFoldout("item-anim-group-header", m_AnimContent, "item-anim-arrow");
            BindGroupFoldout("item-interactable-group-header", m_InteractableContent, "item-interactable-arrow");
            BindGroupFoldout("item-layer-group-header", m_LayerContent, "item-layer-arrow");
            BindGroupFoldout("item-traffic-group-header", m_TrafficContent, "item-traffic-arrow");
            BindGroupFoldout("item-collectible-group-header", m_CollectibleContent, "item-collectible-arrow");
            BindGroupFoldout("item-ysort-group-header", m_YSortContent, "item-ysort-arrow");
            BindGroupFoldout("item-movable-group-header", m_MovableContent, "item-movable-arrow");
            BindGroupFoldout("item-teleport-group-header", m_TeleportContent, "item-teleport-arrow");
        }

    }
}