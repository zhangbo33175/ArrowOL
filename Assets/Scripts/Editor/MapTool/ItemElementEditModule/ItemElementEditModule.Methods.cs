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
using System.Collections.Generic;
using System.IO;
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
            sb.AppendLine("#,ID,Icon,ResName,模板名称,默认动画,是否循环,播放速度,是否可交互,交互类型,渲染层级,是否有碰撞,对话气泡,初始朝向,是否显示阴影,是否收集物,重生时间");
            sb.AppendLine("string,string,string,string,string,string,bool,float,bool,string,int,bool,string,string,bool,bool,float");

            // 按类型分组导出
            var buildingTemplates = templates.FindAll(t => t.Type == MapObjectType.Building);
            var characterTemplates = templates.FindAll(t => t.Type == MapObjectType.Character);
            var propTemplates = templates.FindAll(t => t.Type == MapObjectType.Prop);

            int index = 0;

            // 导出建筑
            foreach (var t in buildingTemplates)
            {
                sb.AppendLine($"{index++},{t.TemplateId},{t.ImagePath},GameObject_Building,{t.TemplateName},{t.AnimName},{t.Loop},{t.Speed},{t.Interactable},{t.InteractType},{t.SortOrder},{t.CollisionEnabled},{t.DialogBubble},{t.FacingDirection},{t.ShadowEnabled},{t.IsCollectible},{t.RespawnTime}");
            }

            // 导出人物
            foreach (var t in characterTemplates)
            {
                sb.AppendLine($"{index++},{t.TemplateId},{t.ImagePath},GameObject_Character,{t.TemplateName},{t.AnimName},{t.Loop},{t.Speed},{t.Interactable},{t.InteractType},{t.SortOrder},{t.CollisionEnabled},{t.DialogBubble},{t.FacingDirection},{t.ShadowEnabled},{t.IsCollectible},{t.RespawnTime}");
            }

            // 导出物件
            foreach (var t in propTemplates)
            {
                sb.AppendLine($"{index++},{t.TemplateId},{t.ImagePath},GameObject_Prop,{t.TemplateName},{t.AnimName},{t.Loop},{t.Speed},{t.Interactable},{t.InteractType},{t.SortOrder},{t.CollisionEnabled},{t.DialogBubble},{t.FacingDirection},{t.ShadowEnabled},{t.IsCollectible},{t.RespawnTime}");
            }

            // 写入文件
            File.WriteAllText(path, sb.ToString(), System.Text.Encoding.UTF8);

            Debug.Log($"[ItemElementEditModule] 导出配置成功: {path}，共导出 {templates.Count} 个模板");
            EditorUtility.DisplayDialog("成功", $"配置已导出到：\n{path}\n\n共 {templates.Count} 个模板", "确定");
        }

        /// <summary>
        /// 创建元件列表项
        /// </summary>
        private VisualElement CreateItemElement(ObjectTemplate template, int index)
        {
            var item = new VisualElement();
            item.AddToClassList("item-element-item");

            // 图标
            var icon = new VisualElement();
            icon.AddToClassList("item-icon");
            // 根据类型设置图标背景色
            if (template.Type == MapObjectType.Building)
                icon.style.backgroundColor = new Color(0.27f, 0.45f, 0.78f); // 蓝色
            else if (template.Type == MapObjectType.Character)
                icon.style.backgroundColor = new Color(0.44f, 0.68f, 0.28f); // 绿色
            else
                icon.style.backgroundColor = new Color(1.0f, 0.75f, 0.0f); // 黄色
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

            // 显示复选框
            var showToggle = new Toggle("显示");
            showToggle.value = true;
            item.Add(showToggle);

            // 点击选中
            int idx = index;
            item.RegisterCallback<ClickEvent>(evt => OnItemSelected(idx, item));
            return item;
        }

        /// <summary>
        /// 元件选中
        /// </summary>
        private void OnItemSelected(int index, VisualElement item)
        {
            m_SelectedIndex = index;
            // 移除所有选中状态
            foreach (var itemElement in m_ItemItems)
                itemElement.RemoveFromClassList("selected");
            // 添加当前选中状态
            item.AddToClassList("selected");

            // 加载模板数据到属性面板
            var templates = ObjectTemplateManager.Instance.GetAllTemplates();
            if (index >= 0 && index < templates.Count)
            {
                var template = templates[index];
                LoadTemplateToPanel(template);
            }

            Debug.Log($"选中元件: {index}");
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
                m_ItemResNameField.SetValueWithoutNotify(template.Type == MapObjectType.Building ? "GameObject_Building" :
                                                         template.Type == MapObjectType.Character ? "GameObject_Character" : "GameObject_Prop");

            // 动画设置
            if (m_ItemAnimField != null)
                m_ItemAnimField.SetValueWithoutNotify(template.AnimName);
            if (m_ItemLoopToggle != null)
                m_ItemLoopToggle.SetValueWithoutNotify(template.Loop);
            if (m_ItemSpeedField != null)
                m_ItemSpeedField.SetValueWithoutNotify(template.Speed.ToString());

            // 交互设置
            if (m_ItemInteractableToggle != null)
                m_ItemInteractableToggle.SetValueWithoutNotify(template.Interactable);
            if (m_ItemInteractTypeField != null)
            {
                string interactName = template.InteractType == "none" ? "无" :
                                      template.InteractType == "click" ? "点击" :
                                      template.InteractType == "talk" ? "对话" :
                                      template.InteractType == "enter" ? "进入" : "收集";
                m_ItemInteractTypeField.SetValueWithoutNotify(interactName);
            }
            if (m_ItemBubbleField != null)
                m_ItemBubbleField.SetValueWithoutNotify(template.DialogBubble);

            // 层级与碰撞
            if (m_ItemSortOrderField != null)
                m_ItemSortOrderField.SetValueWithoutNotify(template.SortOrder.ToString());
            if (m_ItemFacingField != null)
            {
                string facingName = template.FacingDirection == "up" ? "上" :
                                    template.FacingDirection == "down" ? "下" :
                                    template.FacingDirection == "left" ? "左" : "右";
                m_ItemFacingField.SetValueWithoutNotify(facingName);
            }
            if (m_ItemCollisionToggle != null)
                m_ItemCollisionToggle.SetValueWithoutNotify(template.CollisionEnabled);
            if (m_ItemShadowToggle != null)
                m_ItemShadowToggle.SetValueWithoutNotify(template.ShadowEnabled);

            // 收集物设置
            if (m_ItemCollectibleToggle != null)
                m_ItemCollectibleToggle.SetValueWithoutNotify(template.IsCollectible);
            if (m_ItemRespawnField != null)
                m_ItemRespawnField.SetValueWithoutNotify(template.RespawnTime.ToString());
        }

        /// <summary>
        /// 查找UI元素
        /// </summary>
        private void FindElements()
        {
            // 顶部工具栏
            m_AddItemBtn = m_Root.Q<Button>(name: "btn-add-item");
            m_ImportConfigBtn = m_Root.Q<Button>(name: "btn-import-config");
            m_ExportConfigBtn = m_Root.Q<Button>(name: "btn-export-config");

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
            m_ItemResNameField = m_Root.Q<TextField>(name: "item-resname-field");
            m_ItemAnimField = m_Root.Q<TextField>(name: "item-anim-field");
            m_ItemLoopToggle = m_Root.Q<Toggle>(name: "item-loop-toggle");
            m_ItemSpeedField = m_Root.Q<TextField>(name: "item-speed-field");
            m_ItemInteractableToggle = m_Root.Q<Toggle>(name: "item-interactable-toggle");
            m_ItemBubbleField = m_Root.Q<TextField>(name: "item-bubble-field");
            m_ItemSortOrderField = m_Root.Q<TextField>(name: "item-sort-order-field");
            m_ItemCollisionToggle = m_Root.Q<Toggle>(name: "item-collision-toggle");
            m_ItemShadowToggle = m_Root.Q<Toggle>(name: "item-shadow-toggle");
            m_ItemCollectibleToggle = m_Root.Q<Toggle>(name: "item-collectible-toggle");
            m_ItemRespawnField = m_Root.Q<TextField>(name: "item-respawn-field");

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
        }

    }
}