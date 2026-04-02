
namespace Honor.Runtime
{
    public sealed partial class ConfigComponent : GameComponent
    {
        /// <summary>
        /// Config加解密秘钥Key
        /// </summary>
        public static byte[] s_ConfigEncrytionKey = { 75, 110, 6, 51, 45, 78, 203, 171, 50, 31, 251, 109, 94, 77, 23, 16, 237, 43, 20, 19 };

        /// <summary>
        /// Asset组件
        /// </summary>
        private AssetComponent m_AssetComponent;

        /// <summary>
        /// Config管理器
        /// </summary>
        private ConfigManager m_ConfigManager;

    }

}


