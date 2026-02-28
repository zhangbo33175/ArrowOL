namespace Honor.Runtime
{
    public sealed partial class ConfigManager
    {
        /// <summary>
        /// Config加解密秘钥Key
        /// </summary>
        public byte[] s_ConfigEncrytionKey = { 75, 110, 6, 51, 45, 78, 203, 171, 50, 31, 251, 109, 94, 77, 23, 16, 237, 43, 20, 19 };
        
        /// <summary>
        /// Config组件
        /// </summary>
        private ConfigComponent m_ConfigComponent;
    }
}