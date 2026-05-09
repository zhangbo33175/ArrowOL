namespace Honor.Runtime
{
    public sealed partial class ConfigComponent : GameComponent
    {
        /// <summary>
        /// 配置文件加密/解密 静态密钥
        /// 用于 Config 配置文件的加解密算法
        /// </summary>
        public static byte[] s_ConfigEncrytionKey = { 75, 110, 6, 51, 45, 78, 203, 171, 50, 31, 251, 109, 94, 77, 23, 16, 237, 43, 20, 19 };

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
    }
}