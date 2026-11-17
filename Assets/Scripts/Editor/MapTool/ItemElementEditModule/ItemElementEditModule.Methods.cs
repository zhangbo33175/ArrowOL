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
            MapToolLog.Info("添加物品元件");
        }

        /// <summary>
        /// 保存按钮点击：把当前全部元件模板按分类写回对应配置表；成功后通知场景模块同步已摆放实例。
        /// </summary>
        private void OnSaveItemClicked()
        {
            var templates = ObjectTemplateManager.Instance.GetAllTemplates();
            if (templates == null || templates.Count == 0)
            {
                MapToolDialog.Info("保存元件", "当前没有可保存的元件模板。");
                return;
            }

            var buildingGroup = templates.Where(t => t.Type == MapObjectType.Building).ToList();
            var characterGroup = templates.Where(t => t.Type == MapObjectType.Character).ToList();
            var propGroup = templates.Where(t => t.Type == MapObjectType.Prop).ToList();
            var catGroup = templates.Where(t => t.Type == MapObjectType.Cat).ToList();

            string projectRoot = MapToolPathHelper.ProjectRoot;
            int savedCount = 0;
            bool allOk = true;

            allOk &= WriteTemplateGroup(projectRoot, MapToolConstants.TableBuildingFileName, buildingGroup, ref savedCount);
            allOk &= WriteTemplateGroup(projectRoot, MapToolConstants.TableCharacterFileName, characterGroup, ref savedCount);
            allOk &= WriteTemplateGroup(projectRoot, MapToolConstants.TablePropFileName, propGroup, ref savedCount);
            allOk &= WriteTemplateGroup(projectRoot, MapToolConstants.TableCatFileName, catGroup, ref savedCount);

            if (allOk)
            {
                MapToolLog.Info($"[ItemElementEditModule] 保存全部元件成功，共 {savedCount} 个模板");
                // 通知场景编辑模块：按最新模板同步已摆放实例
                OnTemplatesSaved?.Invoke();
                MapToolDialog.Info("保存元件", $"已保存 {savedCount} 个元件模板，并同步到场景物件。");
            }
            else
            {
                MapToolDialog.Info("保存元件", "部分配置表写入失败，请先关闭占用的 Excel 再重试。");
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
                MapToolDialog.Error("配置文件读取失败或为空！\n\n提示：如果是.xlsm文件，请先在Excel中另存为CSV格式。");
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

            MapToolLog.Info($"[ItemElementEditModule] 导入配置成功: {path}，共导入 {templates.Count} 个模板");
            MapToolDialog.Success($"配置已导入，共 {templates.Count} 个模板！");
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

            MapToolLog.Info($"[ItemElementEditModule] 导出配置成功: {path}，共导出 {templates.Count} 个模板");
            MapToolDialog.Success($"配置已导出到：\n{path}\n\n共 {templates.Count} 个模板");
        }

        /// <summary>
        /// 创建元件列表项
        /// </summary>
        private VisualElement CreateItemElement(ObjectTemplate template)
        {
            var item = new VisualElement();
            item.AddToClassList(MapToolStyleClasses.ItemElementItem);

            // 图标 - 加载实际预览图，加载失败则用类型色占位
            var icon = new Image();
            icon.AddToClassList(MapToolStyleClasses.ItemIcon);
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
            textArea.AddToClassList(MapToolStyleClasses.ItemTextArea);

            var nameLabel = MapToolUiFactory.CreateLabel(template.TemplateName, MapToolStyleClasses.ItemName);
            textArea.Add(nameLabel);

            string typeName = ObjectTypeNames.Of(template.Type);
            string desc = $"{template.TemplateId} - {typeName} - {template.InteractType}";
            var descLabel = MapToolUiFactory.CreateLabel(desc, MapToolStyleClasses.ItemDesc);
            textArea.Add(descLabel);

            item.Add(textArea);

            // 显示复选框（只显示勾选框，不显示文字）
            var showToggle = new Toggle();
            showToggle.value = true;
            showToggle.AddToClassList(MapToolStyleClasses.ItemShowToggle);
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
                itemElement.RemoveFromClassList(MapToolStyleClasses.Selected);
            // 添加当前选中状态
            item.AddToClassList(MapToolStyleClasses.Selected);

            // 直接回填点击项对应的模板（不按位置下标反查快照）
            LoadTemplateToPanel(template);

            MapToolLog.Info($"选中元件: {template.TemplateId}");
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
        // 预制体目录口径统一走 MapToolConstants.ModelPrefabDir（Assets/Res/Prefabs/Model）

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

        /// <summary>绑定控件变更：写回当前选中模板（选中回填使用 SetValueWithoutNotify，不会误触发）。</summary>
        private void BindField<T>(BaseField<T> field, Action<ObjectTemplate, T> apply)
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
        /// 加载模板数据到属性面板（只读/派生字段单独回填，其余字段统一走字段表）
        /// </summary>
        /// <param name="template">模板数据</param>
        private void LoadTemplateToPanel(ObjectTemplate template)
        {
            if (template == null) return;

            // 只读/派生字段单独回填
            m_ItemIdField.SetValueSafe(template.TemplateId);
            if (m_ItemTypeField != null)
                m_ItemTypeField.SetValueWithoutNotify(ObjectTypeNames.Of(template.Type));
            m_ItemIconField.SetValueSafe(template.ImagePath);

            FillResNameField(template);
            FillLayerField(template);

            // 其余字段统一走字段表回填（与写回共用同一处定义）
            foreach (var fieldView in TemplateFieldViews)
                fieldView.LoadValue(template);

            // 按模板属性切换收集/交通/传送/移动条件分组显隐
            ApplyGroupConditionalVisibility(template);
        }

        /// <summary>
        /// 回填资源名：分类一致才保留；空值、历史占位或分类错配时按分类匹配默认预制体并写回
        /// </summary>
        /// <param name="template">模板数据</param>
        private void FillResNameField(ObjectTemplate template)
        {
            if (m_ItemResNameField == null) return;

            if (!ResNameMatchesType(template.ResName, template.Type))
                template.ResName = PickDefaultResName(template.Type, m_ItemResNameField.choices);
            if (!m_ItemResNameField.choices.Contains(template.ResName)) m_ItemResNameField.choices.Add(template.ResName);
            m_ItemResNameField.SetValueWithoutNotify(template.ResName);
        }

        /// <summary>
        /// 回填显示层：空值表示「跟随类型」，其余取层显示名（选项缺失时补入）
        /// </summary>
        /// <param name="template">模板数据</param>
        private void FillLayerField(ObjectTemplate template)
        {
            if (m_ItemLayerField == null) return;

            string layerDisplay = string.IsNullOrWhiteSpace(template.DisplayLayer)
                ? MapToolConstants.LayerFollowTypeDisplay
                : MapLayerCatalog.GetLayerDisplayName(template.DisplayLayer);
            if (!m_ItemLayerField.choices.Contains(layerDisplay)) m_ItemLayerField.choices.Add(layerDisplay);
            m_ItemLayerField.SetValueWithoutNotify(layerDisplay);
        }

        /// <summary>
        /// 注册面板各字段的写回回调；选中模板时回填使用 SetValueWithoutNotify，不会误触发本批回调。
        /// 交通类别、ResName、显示层已在控件创建处单独注册。
        /// </summary>
        private void BindTemplateFieldCallbacks()
        {
            foreach (var fieldView in TemplateFieldViews)
                fieldView.BindCallbacks();

            // 显示层写回：跟随类型显示名落空串，其余落八层键
            BindField(m_ItemLayerField, (t, v) => t.DisplayLayer = v == MapToolConstants.LayerFollowTypeDisplay
                ? string.Empty
                : MapLayerCatalog.GetLayerKeyByDisplayName(v));
        }

        /// <summary>
        /// 模板字段表（惰性构建一次）：每项包含「回填 + 写回」，字段增删只改本处
        /// </summary>
        private List<TemplateFieldView> TemplateFieldViews
        {
            get
            {
                EnsureTemplateFieldViews();
                return m_TemplateFieldViews;
            }
        }

        private List<TemplateFieldView> m_TemplateFieldViews;

        private void EnsureTemplateFieldViews()
        {
            if (m_TemplateFieldViews != null) return;

            var views = new List<TemplateFieldView>();
            Func<ObjectTemplate> target = GetCurrentTemplate;

            // 字段表：一行 = 一个字段（左为回填、右为写回），增删字段只改本处
            void Text(BaseField<string> field, Func<ObjectTemplate, string> read, Action<ObjectTemplate, string> write)
                => views.Add(TemplateFieldView.Create(field, read, write, target));

            void Bool(BaseField<bool> field, Func<ObjectTemplate, bool> read, Action<ObjectTemplate, bool> write)
                => views.Add(TemplateFieldView.Create(field, read, write, target));

            void Int(BaseField<int> field, Func<ObjectTemplate, int> read, Action<ObjectTemplate, int> write)
                => views.Add(TemplateFieldView.Create(field, read, write, target));

            // ---- 基础信息 / 动画设置 ----
            Text(m_ItemNameField, t => t.TemplateName, (t, v) => t.TemplateName = v);
            Text(m_ItemAnimField, t => t.AnimName, (t, v) => t.AnimName = v);
            Bool(m_ItemLoopToggle, t => t.Loop, (t, v) => t.Loop = v);
            Text(m_ItemSpeedField, t => t.Speed.ToString("F1"), (t, v) => t.Speed = ParseFloat(v, t.Speed));

            // ---- 交互设置 ----
            Bool(m_ItemInteractableToggle, t => t.Interactable, (t, v) => t.Interactable = v);
            Text(m_ItemInteractTypeField,
                t => TemplateInteractType.ToDisplay(t.InteractType),
                (t, v) => t.InteractType = TemplateInteractType.ToCode(v));
            Text(m_ItemBubbleField, t => t.DialogBubble, (t, v) => t.DialogBubble = v);
            Int(m_ItemBubbleIdField, t => t.BubbleId, (t, v) => t.BubbleId = v);

            // ---- 层级与碰撞 ----
            Text(m_ItemSortOrderField,
                t => t.SortOrder.ToString(),
                (t, v) => t.SortOrder = MapToolEditorUtils.ClampTemplateSortOrder(t, ParseInt(v, t.SortOrder)));
            Text(m_ItemFacingField,
                t => TemplateFacing.ToDisplay(t.FacingDirection),
                (t, v) => t.FacingDirection = TemplateFacing.ToCode(v));
            Bool(m_ItemCollisionToggle, t => t.CollisionEnabled, (t, v) => t.CollisionEnabled = v);
            Bool(m_ItemShadowToggle, t => t.ShadowEnabled, (t, v) => t.ShadowEnabled = v);

            // ---- 收集物设置 ----
            Bool(m_ItemCollectibleToggle, t => t.IsCollectible,
                (t, v) => { t.IsCollectible = v; ApplyGroupConditionalVisibility(t); });
            Text(m_ItemRespawnField, t => t.RespawnTime.ToString("F1"), (t, v) => t.RespawnTime = ParseFloat(v, t.RespawnTime));

            // ---- Y 坐标深度排序 ----
            Bool(m_ItemYSortToggle, t => t.YSortEnabled, (t, v) => t.YSortEnabled = v);
            Text(m_ItemYSortOffsetField, t => t.YSortOffset.ToString("F1"), (t, v) => t.YSortOffset = ParseFloat(v, t.YSortOffset));

            // ---- 移动与循环 ----
            Bool(m_ItemMovableToggle, t => t.IsMovable,
                (t, v) => { t.IsMovable = v; ApplyGroupConditionalVisibility(t); });
            Text(m_ItemMoveAnimField, t => t.MoveAnimName, (t, v) => t.MoveAnimName = v);
            Text(m_ItemLoopRestartField, t => t.LoopRestartDelay.ToString("F1"), (t, v) => t.LoopRestartDelay = ParseFloat(v, t.LoopRestartDelay));
            Bool(m_ItemHideOffMapToggle, t => t.HideWhenOffMap, (t, v) => t.HideWhenOffMap = v);

            // ---- 传送门设置 ----
            Bool(m_ItemTeleportToggle, t => t.IsTeleport,
                (t, v) => { t.IsTeleport = v; ApplyGroupConditionalVisibility(t); });
            Text(m_ItemTeleportSceneField, t => t.TeleportTargetScene, (t, v) => t.TeleportTargetScene = v);
            Text(m_ItemTeleportXField, t => t.TeleportTargetX.ToString(), (t, v) => t.TeleportTargetX = ParseFloat(v, t.TeleportTargetX));
            Text(m_ItemTeleportYField, t => t.TeleportTargetY.ToString(), (t, v) => t.TeleportTargetY = ParseFloat(v, t.TeleportTargetY));

            m_TemplateFieldViews = views;
        }

        /// <summary>
        /// 扫描 Model 目录下全部预制体文件名（不含扩展名）作为下拉选项；
        /// 目录缺失或为空时返回空占位项。
        /// </summary>
        private List<string> CollectModelPrefabNames()
        {
            var names = new List<string>();
            var dir = MapToolPathHelper.ToAbsolutePath(MapToolConstants.ModelPrefabDir);
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
                case MapObjectType.Building: return MapToolConstants.ResNameKeywordBuilding;
                case MapObjectType.Character: return MapToolConstants.ResNameKeywordCharacter;
                default: return MapToolConstants.ResNameKeywordProp;
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
            return MapToolConstants.ResNamePrefix + TypeKeyword(type);
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
        /// 模板面板的交通/收集/移动/传送门四组功能字段常驻可见可编辑：
        /// 主开关勾选仅写回模板数据，不再按 bool 整组隐藏分组；分组标题仍可通过折叠头手动收起。
        /// </summary>
        private void ApplyGroupConditionalVisibility(ObjectTemplate template)
        {
            if (template == null) return;
            SetGroupVisible(MapToolUiNames.ItemCollectibleGroupHeader, m_CollectibleContent, true);
            SetGroupVisible(MapToolUiNames.ItemTrafficGroupHeader, m_TrafficContent, true);
            SetGroupVisible(MapToolUiNames.ItemTeleportGroupHeader, m_TeleportContent, true);
            SetGroupVisible(MapToolUiNames.ItemMovableGroupHeader, m_MovableContent, true);
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
                if (arrow != null) arrow.text = nextExpanded ? MapToolConstants.ExpandArrowDownText : MapToolConstants.ExpandArrowRightText;
            });
        }

        /// <summary>
        /// 查找 UI 元素（工具栏 / 属性字段 / 分组容器 / 动态下拉 / 折叠头）
        /// </summary>
        private void FindElements()
        {
            BindItemToolbarElements();
            BindItemPropertyFields();
            BindItemGroupContents();
            BuildItemDropdowns();
            BindItemGroupFoldouts();
        }

        /// <summary>
        /// 绑定顶部工具栏、元件类型 Tab 与元件列表
        /// </summary>
        private void BindItemToolbarElements()
        {
            // 顶部工具栏
            m_SaveItemBtn = m_Root.QByName<Button>(MapToolUiNames.BtnSaveItem);
            m_AddItemBtn = m_Root.QByName<Button>(MapToolUiNames.BtnAddItem);
            m_ImportConfigBtn = m_Root.QByName<Button>(MapToolUiNames.BtnImportConfig);
            m_ExportConfigBtn = m_Root.QByName<Button>(MapToolUiNames.BtnExportConfig);

            // 元件类型Tab
            m_TabBuilding = m_Root.QByName<Button>(MapToolUiNames.ItemTabBuilding);
            m_TabCharacter = m_Root.QByName<Button>(MapToolUiNames.ItemTabCharacter);
            m_TabProp = m_Root.QByName<Button>(MapToolUiNames.ItemTabProp);

            // 元件列表
            m_ItemList = m_Root.QByName<ScrollView>(MapToolUiNames.ItemList);
        }

        /// <summary>
        /// 绑定元件属性面板字段
        /// </summary>
        private void BindItemPropertyFields()
        {
            // 元件属性面板
            m_ItemIdField = m_Root.QByName<TextField>(MapToolUiNames.ItemIdField);
            m_ItemNameField = m_Root.QByName<TextField>(MapToolUiNames.ItemNameField);
            m_ItemIconField = m_Root.QByName<TextField>(MapToolUiNames.ItemIconField);
            // ID 与 Icon 仅作展示，不允许在面板上修改
            if (m_ItemIdField != null) m_ItemIdField.isReadOnly = true;
            if (m_ItemIconField != null) m_ItemIconField.isReadOnly = true;
            m_ItemAnimField = m_Root.QByName<TextField>(MapToolUiNames.ItemAnimField);
            m_ItemLoopToggle = m_Root.QByName<Toggle>(MapToolUiNames.ItemLoopToggle);
            m_ItemSpeedField = m_Root.QByName<TextField>(MapToolUiNames.ItemSpeedField);
            m_ItemInteractableToggle = m_Root.QByName<Toggle>(MapToolUiNames.ItemInteractableToggle);
            m_ItemBubbleField = m_Root.QByName<TextField>(MapToolUiNames.ItemBubbleField);
            m_ItemBubbleIdField = m_Root.QByName<IntegerField>(MapToolUiNames.ItemBubbleIdField);
            m_ItemSortOrderField = m_Root.QByName<TextField>(MapToolUiNames.ItemSortOrderField);
            m_ItemCollisionToggle = m_Root.QByName<Toggle>(MapToolUiNames.ItemCollisionsToggle);
            m_ItemShadowToggle = m_Root.QByName<Toggle>(MapToolUiNames.ItemShadowToggle);
            m_ItemCollectibleToggle = m_Root.QByName<Toggle>(MapToolUiNames.ItemCollectibleToggle);
            m_ItemRespawnField = m_Root.QByName<TextField>(MapToolUiNames.ItemRespawnField);
            m_ItemYSortToggle = m_Root.QByName<Toggle>(MapToolUiNames.ItemYsSortToggle);
            m_ItemYSortOffsetField = m_Root.QByName<TextField>(MapToolUiNames.ItemYsSortOffsetField);
            m_ItemMovableToggle = m_Root.QByName<Toggle>(MapToolUiNames.ItemMovableToggle);
            m_ItemMoveAnimField = m_Root.QByName<TextField>(MapToolUiNames.ItemMoveAnimField);
            m_ItemLoopRestartField = m_Root.QByName<TextField>(MapToolUiNames.ItemLoopRestartField);
            m_ItemHideOffMapToggle = m_Root.QByName<Toggle>(MapToolUiNames.ItemHideOffmapToggle);
            m_ItemTeleportToggle = m_Root.QByName<Toggle>(MapToolUiNames.ItemTeleportToggle);
            m_ItemTeleportSceneField = m_Root.QByName<TextField>(MapToolUiNames.ItemTeleportSceneField);
            m_ItemTeleportXField = m_Root.QByName<TextField>(MapToolUiNames.ItemTeleportXField);
            m_ItemTeleportYField = m_Root.QByName<TextField>(MapToolUiNames.ItemTeleportYField);
        }

        /// <summary>
        /// 绑定各属性分组的内容容器（随折叠头独立显隐）
        /// </summary>
        private void BindItemGroupContents()
        {
            // 分组内容容器（随标题独立折叠头显隐）
            m_AnimContent = m_Root.QByName<VisualElement>(MapToolUiNames.ItemAnimContent);
            m_InteractableContent = m_Root.QByName<VisualElement>(MapToolUiNames.ItemInteractableContent);
            m_LayerContent = m_Root.QByName<VisualElement>(MapToolUiNames.ItemLayerContent);
            m_TrafficContent = m_Root.QByName<VisualElement>(MapToolUiNames.ItemTrafficContent);
            m_CollectibleContent = m_Root.QByName<VisualElement>(MapToolUiNames.ItemCollectibleContent);
            m_YSortContent = m_Root.QByName<VisualElement>(MapToolUiNames.ItemYsSortContent);
            m_MovableContent = m_Root.QByName<VisualElement>(MapToolUiNames.ItemMovableContent);
            m_TeleportContent = m_Root.QByName<VisualElement>(MapToolUiNames.ItemTeleportContent);
        }

        /// <summary>
        /// 在占位容器内动态创建下拉控件（类型 / 交互类型 / 朝向 / 显示层 / 交通类别 / 资源预制体）
        /// </summary>
        private void BuildItemDropdowns()
        {
            // 类型下拉框
            m_ItemTypeField = MapToolUiFactory.CreateDropdownIn(m_Root, MapToolUiNames.ItemTypeContainer,
                new List<string>(ObjectTypeNames.EditableTypeChoices), ObjectTypeNames.Prop);

            // 交互类型下拉框
            m_ItemInteractTypeField = MapToolUiFactory.CreateDropdownIn(m_Root, MapToolUiNames.ItemInteractTypeContainer,
                TemplateInteractType.DisplayChoices, TemplateInteractType.ToDisplay(TemplateInteractType.None));

            // 朝向下拉框
            m_ItemFacingField = MapToolUiFactory.CreateDropdownIn(m_Root, MapToolUiNames.ItemFacingContainer,
                TemplateFacing.DisplayChoices, TemplateFacing.ToDisplay(TemplateFacing.Down));

            // 层级档下拉框：跟随类型 + 场景编辑器同款八层选项
            var layerChoices = new List<string> { MapToolConstants.LayerFollowTypeDisplay };
            layerChoices.AddRange(MapLayerCatalog.AllLayerTypes.Select(MapLayerCatalog.GetLayerDisplayName));
            m_ItemLayerField = MapToolUiFactory.CreateDropdownIn(m_Root, MapToolUiNames.ItemLayerContainer,
                layerChoices, MapToolConstants.LayerFollowTypeDisplay);

            // 交通类别下拉框
            m_ItemTrafficCategoryField = MapToolUiFactory.CreateDropdownIn(m_Root, MapToolUiNames.ItemTrafficCategoryContainer,
                TemplateTrafficCategory.AllDisplayNames, TemplateTrafficCategory.ToDisplayName(TemplateTrafficCategory.None),
                OnTrafficCategoryChanged);

            // 资源预制体下拉框：选项来自 Res/Prefabs/Model 目录下的预制体文件名
            m_ItemResNameField = MapToolUiFactory.CreateDropdownIn(m_Root, MapToolUiNames.ItemResNameContainer,
                CollectModelPrefabNames(), null, OnResNameChanged);
        }

        /// <summary>
        /// 绑定分组独立折叠头：点击标题行仅切换本组内容显隐（纯 UI 折叠，不绑定业务字段；
        /// 组内的功能勾选框仍是独立业务开关，回组内原位配置）
        /// </summary>
        private void BindItemGroupFoldouts()
        {
            BindGroupFoldout(MapToolUiNames.ItemAnimGroupHeader, m_AnimContent, MapToolUiNames.ItemAnimArrow);
            BindGroupFoldout(MapToolUiNames.ItemInteractableGroupHeader, m_InteractableContent, MapToolUiNames.ItemInteractableArrow);
            BindGroupFoldout(MapToolUiNames.ItemLayerGroupHeader, m_LayerContent, MapToolUiNames.ItemLayerArrow);
            BindGroupFoldout(MapToolUiNames.ItemTrafficGroupHeader, m_TrafficContent, MapToolUiNames.ItemTrafficArrow);
            BindGroupFoldout(MapToolUiNames.ItemCollectibleGroupHeader, m_CollectibleContent, MapToolUiNames.ItemCollectibleArrow);
            BindGroupFoldout(MapToolUiNames.ItemYsSortGroupHeader, m_YSortContent, MapToolUiNames.ItemYsSortArrow);
            BindGroupFoldout(MapToolUiNames.ItemMovableGroupHeader, m_MovableContent, MapToolUiNames.ItemMovableArrow);
            BindGroupFoldout(MapToolUiNames.ItemTeleportGroupHeader, m_TeleportContent, MapToolUiNames.ItemTeleportArrow);
        }

    }
}
