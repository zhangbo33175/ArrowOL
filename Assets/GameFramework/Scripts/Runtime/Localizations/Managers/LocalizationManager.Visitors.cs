/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LocalizationManager.Fields.cs
 * author:    云毅
 * created:   2026
 * descrip:   本地化管理器 - 字段定义（partial）
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 本地化管理器 - 字段定义部分
    /// </summary>
    public sealed partial class LocalizationManager
    {
        //=========================================================================
        // 组件引用
        //=========================================================================
        #region Component References
        /// <summary>
        /// 框架启动组件
        /// </summary>
        private LauncherComponent m_LauncherComponent;

        /// <summary>
        /// 资源加载组件
        /// </summary>
        private AssetComponent m_AssetComponent;
        #endregion

        //=========================================================================
        // 数据容器
        //=========================================================================
        #region Data Containers
        /// <summary>
        /// 支持的语言列表
        /// </summary>
        private readonly List<GameDefinitions.Language> m_DefaultLanguages;

        /// <summary>
        /// 多语言文本数据
        /// 结构：语言类型 → (Key → 文本内容)
        /// </summary>
        private readonly Dictionary<GameDefinitions.Language, Dictionary<string, string>> m_DefaultDatas;

        /// <summary>
        /// 多语言字体配置
        /// 结构：语言类型 → 字体数据列表
        /// </summary>
        private readonly Dictionary<GameDefinitions.Language, List<LocalizationFontData>> m_FontDatas;
        #endregion
    }
}