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
        /// 保存全部元件按钮
        /// </summary>
        private Button m_SaveItemBtn;
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
        // 当前选中模板的唯一ID（按稳定键反查，不依赖字典快照的位置下标）
        /// <summary>
        /// 当前选中模板的唯一ID；为空表示未选中。
        /// </summary>
        private string m_SelectedTemplateId = null;

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
        private DropdownField m_ItemResNameField;
        private TextField m_ItemAnimField;
        private Toggle m_ItemLoopToggle;
        private TextField m_ItemSpeedField;
        private Toggle m_ItemInteractableToggle;
        private DropdownField m_ItemInteractTypeField;
        private TextField m_ItemBubbleField;
        private IntegerField m_ItemBubbleIdField;
        private TextField m_ItemSortOrderField;
        private DropdownField m_ItemFacingField;
        private DropdownField m_ItemLayerField;
        private Toggle m_ItemCollisionToggle;
        private Toggle m_ItemShadowToggle;
        private DropdownField m_ItemTrafficCategoryField;
        // 分组内容容器（随标题独立折叠头显隐，纯 UI 状态，不绑定业务字段）
        private VisualElement m_AnimContent;
        private VisualElement m_InteractableContent;
        private VisualElement m_LayerContent;
        private VisualElement m_TrafficContent;
        private VisualElement m_CollectibleContent;
        private VisualElement m_YSortContent;
        private VisualElement m_MovableContent;
        private VisualElement m_TeleportContent;
        private Toggle m_ItemCollectibleToggle;
        private TextField m_ItemRespawnField;
        private Toggle m_ItemYSortToggle;
        private TextField m_ItemYSortOffsetField;
        private Toggle m_ItemMovableToggle;
        private TextField m_ItemMoveAnimField;
        private TextField m_ItemLoopRestartField;
        private Toggle m_ItemHideOffMapToggle;
        private Toggle m_ItemTeleportToggle;
        private TextField m_ItemTeleportSceneField;
        private TextField m_ItemTeleportXField;
        private TextField m_ItemTeleportYField;
        #endregion

        #region 属性
        /// <summary>
        /// 模块根节点
        /// </summary>
        public VisualElement Root => m_Root;

        /// <summary>
        /// 全部元件模板保存成功后触发（供场景编辑模块同步已摆放实例）
        /// </summary>
        public event System.Action OnTemplatesSaved;
        #endregion

    }
}