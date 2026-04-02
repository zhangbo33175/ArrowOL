namespace Honor.Runtime
{
    public sealed partial class PersistComponent : GameComponent
    {
        /// <summary>
        /// 文件片段存储管理器
        /// WebGL网页专用版
        /// </summary>
        private FileFragmentForWebGLManager m_FileFragmentForWebGLManager = null;
        public FileFragmentForWebGLManager FileFragmentForWebGLManager
        {
            get
            {
                return m_FileFragmentForWebGLManager;
            }
        }

        private FileFragmentManager m_FileFragmentManager = null;
        public FileFragmentManager FileFragmentManager
        {
            get
            {
                return m_FileFragmentManager;
            }
        }

        /// <summary>
        /// PlayerPrefs存储管理器
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
        /// 获取条目数量。
        /// </summary>
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


