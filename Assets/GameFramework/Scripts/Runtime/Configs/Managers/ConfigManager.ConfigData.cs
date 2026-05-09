namespace Honor.Runtime
{
    public sealed partial class ConfigManager
    {
        /// <summary>
        /// 配置项数据实体（不可变）
        /// 存储单个配置的布尔、整型、浮点、字符串四种类型值
        /// 采用只读结构，确保配置加载后不可修改，保证数据安全
        /// </summary>
        public class ConfigData
        {
            /// <summary>
            /// 布尔类型配置值
            /// </summary>
            private readonly bool m_BoolValue;

            /// <summary>
            /// 整数类型配置值
            /// </summary>
            private readonly int m_IntValue;

            /// <summary>
            /// 浮点数类型配置值
            /// </summary>
            private readonly float m_FloatValue;

            /// <summary>
            /// 字符串类型配置值
            /// </summary>
            private readonly string m_StringValue;

            /// <summary>
            /// 构造函数：初始化所有类型的配置值
            /// </summary>
            /// <param name="boolValue">布尔值</param>
            /// <param name="intValue">整数值</param>
            /// <param name="floatValue">浮点值</param>
            /// <param name="stringValue">字符串值</param>
            public ConfigData(bool boolValue, int intValue, float floatValue, string stringValue)
            {
                m_BoolValue = boolValue;
                m_IntValue = intValue;
                m_FloatValue = floatValue;
                m_StringValue = stringValue;
            }

            /// <summary>
            /// 获取布尔类型值
            /// </summary>
            public bool BoolValue
            {
                get { return m_BoolValue; }
            }

            /// <summary>
            /// 获取整数类型值
            /// </summary>
            public int IntValue
            {
                get { return m_IntValue; }
            }

            /// <summary>
            /// 获取浮点数类型值
            /// </summary>
            public float FloatValue
            {
                get { return m_FloatValue; }
            }

            /// <summary>
            /// 获取字符串类型值
            /// </summary>
            public string StringValue
            {
                get { return m_StringValue; }
            }
        }
    }
}