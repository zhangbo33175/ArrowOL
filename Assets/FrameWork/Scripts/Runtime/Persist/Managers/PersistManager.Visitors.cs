namespace Honor.Runtime
{
    public sealed partial class PersistManager
    {
        private FileFragmentComponent m_FileFragmentComponent = null;

        public FileFragmentComponent FileFragmentComponent
        {
            get { return m_FileFragmentComponent; }
        }

        /// <summary>
        /// PlayerPrefs存储管理器
        /// </summary>
        private PlayerPrefsComponent m_PlayerPrefsComponent = null;

        public PlayerPrefsComponent PlayerPrefsComponent
        {
            get { return PlayerPrefsComponent; }
        }

        /// <summary>
        /// 获取条目数量。
        /// </summary>
        public int Count(PersistWayType wayType, string classifyName)
        {
            if (wayType == PersistWayType.FileFragment)
            {
                return m_FileFragmentComponent.Count(classifyName);
            }
            else if (wayType == PersistWayType.PlayerPrefs)
            {
                return m_PlayerPrefsComponent.Count(classifyName);
            }

            return 0;
        }
    }
}