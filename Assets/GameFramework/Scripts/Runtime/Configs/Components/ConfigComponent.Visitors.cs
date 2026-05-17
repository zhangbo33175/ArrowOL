/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ConfigComponent.Fields.cs
 * author:    云毅
 *  created:   2026
 * descrip:   全局配置管理组件 - 字段定义（partial）
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 全局配置管理组件 - 字段定义部分
    /// 包含加密密钥、组件引用、管理器实例
    /// </summary>
    public sealed partial class ConfigComponent : GameComponent
    {
        //=========================================================================
        // 静态常量 & 密钥
        //=========================================================================
        #region Static - 加密密钥
        /// <summary>
        /// 配置文件加密/解密 静态密钥
        /// 用于 Config 配置文件的加解密算法
        /// </summary>
        public static byte[] s_ConfigEncrytionKey = 
        { 
            75, 110, 6, 51, 45, 78, 203, 171, 50, 31, 251, 109, 94, 77, 23, 16, 237, 43, 20, 19 
        };
        #endregion

        //=========================================================================
        // 私有字段
        //=========================================================================
        #region Field - 组件引用
        /// <summary>
        /// 资源管理组件引用
        /// 用于加载配置相关资源
        /// </summary>
        private AssetComponent m_AssetComponent;

        /// <summary>
        /// 配置管理器实例
        /// 实际执行配置加载、读取、存储、加密的核心逻辑
        /// </summary>
        private ConfigManager m_ConfigManager;
        #endregion
    }
}