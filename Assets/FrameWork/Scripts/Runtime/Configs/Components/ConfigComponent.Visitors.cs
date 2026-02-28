using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class ConfigComponent
    {
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