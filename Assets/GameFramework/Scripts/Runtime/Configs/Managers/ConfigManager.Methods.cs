/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ConfigManager.Utils.cs
 * author:    云毅
 * created:   2026
 * descrip:   配置管理器 - 工具方法（partial）
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 配置管理器 - 工具方法部分
    /// </summary>
    public sealed partial class ConfigManager
    {
        //=========================================================================
        // 私有工具方法
        //=========================================================================
        #region Method - 工具方法
        /// <summary>
        /// 根据配置名称获取对应的配置数据列表
        /// </summary>
        /// <param name="configName">配置项名称</param>
        /// <returns>配置数据列表，不存在则返回 null</returns>
        /// <exception cref="GameException">配置名称为空时抛出异常</exception>
        private List<ConfigData> GetConfigData(string configName)
        {
            // 校验配置名称是否有效
            if (string.IsNullOrEmpty(configName))
            {
                throw new GameException("configName 无效。");
            }

            // 尝试从字典中获取配置数据
            m_ConfigDatas.TryGetValue(configName, out List<ConfigData> configData);
            return configData;
        }
        #endregion
    }
}