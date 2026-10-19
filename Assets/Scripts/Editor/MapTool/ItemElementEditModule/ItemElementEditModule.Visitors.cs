/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ItemElementEditModule.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   物品元件编辑模块 - 字段与属性声明部分
 ***************************************************************/

using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 物品元件编辑模块 - 字段与属性声明
    /// </summary>
    public partial class ItemElementEditModule
    {
        #region 字段
        /// <summary>
        /// 模块根节点
        /// </summary>
        private VisualElement m_Root;
        // 顶部工具栏
        /// <summary>
        /// 添加元件按钮
        /// </summary>
        private Button m_AddItemBtn;
        /// <summary>
        /// 导入配置按钮
        /// </summary>
        private Button m_ImportConfigBtn;

        /// <summary>
        /// 导出配置按钮
        /// </summary>
        private Button m_ExportConfigBtn;
        // 左侧元件列表
        /// <summary>
        /// 元件列表控件
        /// </summary>
        private ScrollView m_ItemList;
        /// <summary>
        /// 元件列表项容器
        /// </summary>
        private List<VisualElement> m_ItemItems = new List<VisualElement>();
        // 当前选中的元件索引
        /// <summary>
        /// 当前选中的元件索引
        /// </summary>
        private int m_SelectedIndex = -1;

        /// <summary>
        /// 元件类型Tab按钮
        /// </summary>
        private Button m_TabBuilding;
        private Button m_TabCharacter;
        private Button m_TabProp;

        /// <summary>
        /// 当前选中的类型Tab
        /// </summary>
        private MapObjectType m_CurrentTypeTab = MapObjectType.Building;

        // 元件属性面板字段
        private TextField m_ItemIdField;
        private DropdownField m_ItemTypeField;
        private TextField m_ItemNameField;
        private TextField m_ItemIconField;
        private TextField m_ItemResNameField;
        private TextField m_ItemAnimField;
        private Toggle m_ItemLoopToggle;
        private TextField m_ItemSpeedField;
        private Toggle m_ItemInteractableToggle;
        private DropdownField m_ItemInteractTypeField;
        private TextField m_ItemBubbleField;
        private TextField m_ItemSortOrderField;
        private DropdownField m_ItemFacingField;
        private Toggle m_ItemCollisionToggle;
        private Toggle m_ItemShadowToggle;
        private Toggle m_ItemCollectibleToggle;
        private TextField m_ItemRespawnField;
        #endregion

        #region 属性
        /// <summary>
        /// 模块根节点
        /// </summary>
        public VisualElement Root => m_Root;
        #endregion

    }
}