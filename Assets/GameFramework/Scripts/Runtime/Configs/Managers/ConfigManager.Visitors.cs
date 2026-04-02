using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class ConfigManager
    {
        /// <summary>
        /// Launcher组件
        /// </summary>
        private LauncherComponent m_LauncherComponent;

        /// <summary>
        /// Asset组件
        /// </summary>
        private AssetComponent m_AssetComponent;

        /// <summary>
        /// 配置数据集合
        /// </summary>
        private readonly Dictionary<string, List<ConfigData>> m_ConfigDatas;

        /// <summary>
        /// 获取配置项数量
        /// </summary>
        public int Count
        {
            get
            {
                return m_ConfigDatas.Count;
            }
        }

    }

}


