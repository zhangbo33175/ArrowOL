using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class ConfigComponent
    {
        /// <summary>
        /// 获取指定的配置项
        /// </summary>
        /// <param name="configName">要获取配置项的名称</param>
        /// <returns></returns>
        private List<ConfigData> GetConfigData(string configName)
        {
            if (string.IsNullOrEmpty(configName))
            {
                throw new GameException("configName 无效。");
            }

            List<ConfigData> configData = null;

            m_ConfigDatas.TryGetValue(configName, out configData);

            return configData;
        }
    }
}