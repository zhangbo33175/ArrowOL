/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ConfigManager.Data.cs
 * author:    云毅
 * created: 2025
 * descrip:   配置管理器 - 配置数据实体（partial）
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 配置管理器 - 数据实体部分
    /// </summary>
    public sealed partial class ConfigManager
    {
        /// <summary>
        /// 配置项数据实体（不可变）
        /// 存储单个配置的布尔、整型、浮点、字符串四种类型值
        /// 采用只读结构，确保配置加载后不可修改，保证数据安全
        /// </summary>
        public class ConfigData
        {
            //=========================================================================
            // 只读字段
            //=========================================================================
            #region Readonly Fields
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
            #endregion

            //=========================================================================
            // 构造函数
            //=========================================================================
            #region Constructor
            /// <summary>
            /// 构造函数：初始化所有类型的配置值
            /// </summary>
            /// <param name="boolValue">布尔值</param>
            /// <param name="intValue">整数值</param>
            /// <param name="floatValue">浮点值</param>
            /// <param name="stringValue">字符串值</param>
            public ConfigData(bool boolValue, int intValue, float floatValue, string stringValue)
            {
                m_BoolValue    = boolValue;
                m_IntValue     = intValue;
                m_FloatValue   = floatValue;
                m_StringValue  = stringValue;
            }
            #endregion

            //=========================================================================
            // 公共属性
            //=========================================================================
            #region Properties
            /// <summary>
            /// 获取布尔类型值
            /// </summary>
            public bool BoolValue => m_BoolValue;

            /// <summary>
            /// 获取整数类型值
            /// </summary>
            public int IntValue => m_IntValue;

            /// <summary>
            /// 获取浮点数类型值
            /// </summary>
            public float FloatValue => m_FloatValue;

            /// <summary>
            /// 获取字符串类型值
            /// </summary>
            public string StringValue => m_StringValue;
            #endregion
        }
    }
}