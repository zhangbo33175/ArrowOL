namespace Honor.Runtime
{
    public sealed partial class PersistComponent : GameComponent
    {
        /// <summary>
        /// WebGL 专用文件片段存储管理器
        /// </summary>
        private FileFragmentForWebGLManager m_FileFragmentForWebGLManager = null;
        public FileFragmentForWebGLManager FileFragmentForWebGLManager
        {
            get
            {
                return m_FileFragmentForWebGLManager;
            }
        }

        /// <summary>
        /// 常规平台文件片段存储管理器
        /// </summary>
        private FileFragmentManager m_FileFragmentManager = null;
        public FileFragmentManager FileFragmentManager
        {
            get
            {
                return m_FileFragmentManager;
            }
        }

        /// <summary>
        /// PlayerPrefs 存储管理器
        /// </summary>
        private PlayerPrefsManager m_PlayerPrefsManager = null;
        public PlayerPrefsManager PlayerPrefsManager
        {
            get
            {
                return m_PlayerPrefsManager;
            }
        }

        /// <summary>
        /// 获取指定分类下的存储条目数量
        /// </summary>
        /// <param name="wayType">存储方式</param>
        /// <param name="classifyName">分类名称</param>
        /// <returns>条目数量</returns>
        public int Count(PersistWayType wayType, string classifyName)
        {
            if (wayType == PersistWayType.FileFragment)
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                return m_FileFragmentForWebGLManager.Count(classifyName);
#else
                return m_FileFragmentManager.Count(classifyName);
#endif
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsManager.Count(classifyName);
            }
            return 0;
        }
    }
}