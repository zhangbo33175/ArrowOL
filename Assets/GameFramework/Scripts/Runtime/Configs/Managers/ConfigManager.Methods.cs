using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class ConfigManager
    {
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
    }
}