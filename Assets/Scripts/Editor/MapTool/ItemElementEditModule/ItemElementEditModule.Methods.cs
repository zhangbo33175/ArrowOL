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
using UnityEngine.UIElements;
using System.Collections.Generic;

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
            Debug.Log("导入配置");
        }

        /// <summary>
        /// 创建元件列表项
        /// </summary>
        private VisualElement CreateItemElement(string name, string desc, bool show, int index)
        {
            var item = new VisualElement();
            item.AddToClassList("item-element-item");
            // 图标
            var icon = new VisualElement();
            icon.AddToClassList("item-icon");
            item.Add(icon);
            // 名称和描述
            var textArea = new VisualElement();
            textArea.AddToClassList("item-text-area");
            var nameLabel = new Label(name);
            nameLabel.AddToClassList("item-name");
            textArea.Add(nameLabel);
            var descLabel = new Label(desc);
            descLabel.AddToClassList("item-desc");
            textArea.Add(descLabel);
            item.Add(textArea);
            // 显示复选框
            var showToggle = new Toggle("显示");
            showToggle.value = show;
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
            Debug.Log($"选中元件: {index}");
        }

        /// <summary>
        /// 查找UI元素
        /// </summary>
        private void FindElements()
        {
            // 顶部工具栏
            m_AddItemBtn = m_Root.Q<Button>(name: "btn-add-item");
            m_ImportConfigBtn = m_Root.Q<Button>(name: "btn-import-config");
            // 元件列表
            m_ItemList = m_Root.Q<ScrollView>(name: "item-list");
        }

    }
}