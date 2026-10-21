/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ConfigManager.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   配置管理器 - 字段与属性定义（partial）
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 配置管理器 - 字段定义部分
    /// </summary>
    public sealed partial class ConfigManager
    {
        //=========================================================================
        // 私有字段
        //=========================================================================
        #region Field
        /// <summary>
        /// 配置数据字典（只读）
        /// Key：配置项名称
        /// Value：配置数据列表（0=开发模式，1=正式模式）
        /// </summary>
        private readonly Dictionary<string, List<ConfigData>> m_ConfigDatas;
        #endregion

        //=========================================================================
        // 公共属性
        //=========================================================================
        #region Property
        /// <summary>
        /// 当前已加载的配置项总数量
        /// </summary>
        public int Count => m_ConfigDatas.Count;
        #endregion
    }
}