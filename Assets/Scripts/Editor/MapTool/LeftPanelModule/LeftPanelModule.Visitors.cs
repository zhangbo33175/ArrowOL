/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LeftPanelModule.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   左侧面板模块 - 字段与属性声明部分
 ***************************************************************/

using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using Editor.MapEditor;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 左侧面板模块 - 字段与属性声明
    /// </summary>
    public partial class LeftPanelModule
    {
        #region 字段
        /// <summary>
        /// 模块根节点
        /// </summary>
        private VisualElement m_Root;
        // 顶部按钮
        /// <summary>
        /// 配置按钮
        /// </summary>
        private Button m_ConfigBtn;
        /// <summary>
        /// 删除按钮
        /// </summary>
        private Button m_DeleteBtn;
        // 章节-关卡树
        /// <summary>
        /// 章节-关卡树列表控件
        /// </summary>
        private ScrollView m_ChapterList;
        /// <summary>
        /// 章节配置表数据
        /// </summary>
        private List<TableChapterEditor> m_ChapterDataList = new List<TableChapterEditor>();
        /// <summary>
        /// 主关卡配置表数据
        /// </summary>
        private List<TableMainLevelsEditor> m_MainLevelsDataList = new List<TableMainLevelsEditor>();
        /// <summary>
        /// 按章节ID分组的关卡字典（key=章节ID, value=该章节下的关卡列表）
        /// </summary>
        private Dictionary<string, List<TableMainLevelsEditor>> m_LevelsByChapter = new Dictionary<string, List<TableMainLevelsEditor>>();
        // 章节对应的地图预制体路径（内存缓存，编辑器会话内有效）
        /// <summary>
        /// 章节对应的地图预制体路径缓存（编辑器会话内有效）
        /// </summary>
        private Dictionary<string, string> m_ChapterMapPaths = new Dictionary<string, string>();
        /// <summary>
        /// 关卡选择变化事件（章节ID、关卡ID、预制体路径）
        /// </summary>
        public System.Action<string, string, string> OnLevelSelected;
        /// <summary>
        /// 资源选中事件（资源名、类型：0=建筑 1=人物 2=物件）
        /// </summary>
        public System.Action<string, int> OnResourceSelected;
        /// <summary>
        /// Tab 切换事件（类型：0=建筑 1=人物 2=物件）——用于联动顶部图层 Toggle
        /// </summary>
        public System.Action<int> OnTabChanged;
        // 资源分类Tab按钮（索引与 GetResourceDataByTab 对应）
        /// <summary>
        /// 建筑Tab按钮
        /// </summary>
        private Button m_BuildingTabBtn;
        /// <summary>
        /// 地面Tab按钮
        /// </summary>
        private Button m_GroundTabBtn;
        /// <summary>
        /// 物件Tab按钮
        /// </summary>
        private Button m_PropTabBtn;
        /// <summary>
        /// 刷新按钮
        /// </summary>
        private Button m_RefreshBtn;
        // 资源列表
        /// <summary>
        /// 资源列表控件
        /// </summary>
        private ScrollView m_ResourceList;
        /// <summary>
        /// 资源列表项容器
        /// </summary>
        private List<VisualElement> m_ResourceItems = new List<VisualElement>();
        // 当前选中的Tab索引
        /// <summary>
        /// 当前选中的Tab索引
        /// </summary>
        private int m_CurrentTabIndex = 0;
        #endregion

        #region 属性
        /// <summary>
        /// 模块根节点
        /// </summary>
        public VisualElement Root => m_Root;
        /// <summary>
        /// 当前选中的章节ID
        /// </summary>
        public string SelectedChapterId { get; private set; }
        /// <summary>
        /// 当前选中的关卡ID
        /// </summary>
        public string SelectedLevelId { get; private set; }
        /// <summary>
        /// 当前选中的资源名称
        /// </summary>
        public string SelectedResourceName { get; private set; }
        #endregion

    }
}