/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ResDefExportEditorWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   资源配置导出工具窗口 - 资源索引管理、打包配置检查、导出功能
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// 资源定义导出工具窗口（Partial 分部类）
    /// 负责资源索引配置、打包检查、数据导出、错误校验
    /// </summary>
    public partial class ResDefExportEditorWindow : BaseEditorWindow<ResDefExportEditorWindow>
    {
        #region 滚动视图位置
        /// <summary>
        /// 文本滚动位置
        /// </summary>
        private Vector2 m_TextScrollViewPosition = Vector2.zero;
        
        /// <summary>
        /// 左侧面板滚动位置
        /// </summary>
        private Vector2 m_LeftScrollViewPosition = Vector2.zero;
        
        /// <summary>
        /// 右侧面板滚动位置
        /// </summary>
        private Vector2 m_RightScrollViewPosition = Vector2.zero;
        #endregion

        #region 窗口基础设置
        /// <summary>
        /// 窗口标题
        /// </summary>
        protected override GUIContent Title => new GUIContent("ResDef Exporter");

        /// <summary>
        /// 窗口最小尺寸
        /// </summary>
        protected override Vector2 MinSize => new Vector2(1700, 845);

        /// <summary>
        /// 窗口最大尺寸
        /// </summary>
        protected override Vector2 MaxSize => Vector2.zero;
        #endregion

        #region 核心数据结构
        /// <summary>
        /// 本地记录的所有资源AB信息
        /// </summary>
        protected ResDefInfos m_AllResDefInfos;

        /// <summary>
        /// 缓冲区临时数据
        /// </summary>
        private List<ResDefItem> m_TempResDefItems;

        /// <summary>
        /// 所有折叠框展开状态
        /// </summary>
        private Dictionary<string, TopItemInfo> m_AllShowFoldout;

        /// <summary>
        /// AB配置映射 <ab包名, ab路径>
        /// </summary>
        public static Dictionary<string, string> s_ABConfigs = new Dictionary<string, string>();
        #endregion

        #region 搜索与筛选
        /// <summary>
        /// 当前选择的文件类型
        /// </summary>
        protected int m_SelectResType = 0;

        /// <summary>
        /// 搜索资源名称
        /// </summary>
        private string m_SearchResName = string.Empty;

        /// <summary>
        /// 搜索结果集合
        /// </summary>
        private List<ResDefItem> m_SearchResDefItemInfo;

        /// <summary>
        /// 仅显示错误资源
        /// </summary>
        private bool m_OnlyShowError = false;
        #endregion

        #region 查找与定位
        /// <summary>
        /// 查找到的AB路径
        /// </summary>
        private string m_FindFileABPath = string.Empty;

        /// <summary>
        /// 符合条件的文件路径（未追加）
        /// </summary>
        private Dictionary<string, FileUseState> m_FindFileFullPathList;

        /// <summary>
        /// 当前查找的目标资源对象
        /// </summary>
        private UnityEngine.Object m_FindTargetAsset;
        public UnityEngine.Object FindTargetAsset
        {
            get => m_FindTargetAsset;
            set
            {
                if (m_FindTargetAsset == value)
                    return;
                
                m_FindTargetAsset = value;
                SetFindTargetAsset(m_FindTargetAsset);
            }
        }
        #endregion

        #region 删除配置
        /// <summary>
        /// 删除起始ID
        /// </summary>
        private int m_InputDelStartID = 0;

        /// <summary>
        /// 删除结束ID
        /// </summary>
        private int m_InputDelEndID = 0;
        #endregion

        #region 界面布局
        /// <summary>
        /// 左右面板宽度分割比例
        /// </summary>
        public float m_SubPercenttage = 0.64f;
        #endregion

        #region ID 管理
        /// <summary>
        /// 当前最大资源ID
        /// </summary>
        private int m_CurMaxResID = 0;

        /// <summary>
        /// 获取自增后的资源ID
        /// </summary>
        private int CurMaxResID => ++m_CurMaxResID;
        #endregion

        #region 结果数据
        /// <summary>
        /// 结果页失效路径集合
        /// </summary>
        public Dictionary<string, Dictionary<int, Boolean>> m_ResultInvalidPath;

        /// <summary>
        /// 结果页详细路径信息
        /// </summary>
        public Dictionary<string, List<ResDefItem>> m_ResultDetailInfo;

        /// <summary>
        /// 结果页同名资源标记
        /// </summary>
        public Dictionary<string, bool> m_ResultSameResInfo;

        /// <summary>
        /// 错误资源集合
        /// </summary>
        private List<ResDefItem> m_ErrorResDefItemInfo;
        #endregion

        #region 默认资源
        /// <summary>
        /// 默认路径下的所有图片
        /// </summary>
        private List<Texture2D> m_AllDefaultTextures;
        #endregion

        #region 标签与常量
        /// <summary>
        /// 搜索页标签
        /// </summary>
        private string m_SearchTag = "Search";

        /// <summary>
        /// 错误页标签
        /// </summary>
        private string m_ErrorTag = "Error";

        /// <summary>
        /// 单页最大导出数量（分页导出）
        /// </summary>
        private static int m_OneSheetMaxCount = 300;
        #endregion
    }
}