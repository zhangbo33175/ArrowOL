namespace Honor.Runtime
{
    public partial class LuaManager
    {
        /// <summary>
        /// Luac的AB是否可以加载
        /// 增量Lua脚本必须确保在Persistent读写路径下存在对应的AB资源才允许进行加载
        /// </summary>
        /// <param name="abPath">Lua脚本的ABPath</param>
        public bool Can_AB_Load(string abPath)
        {
             // if(!LauncherManager.Instance.EditorResourceMode && m_HotfixComponent.OpenVersionCheck)
             // {
             //     string formatPath = m_AssetComponent.AssetLoadManager.ABLoadManager.GetABFormatPath(abPath);
             //     // 增量Lua脚本必须确保在Persistent读写路径下存在对应的AB资源才允许进行加载
             //     if (!m_HotfixComponent.LocalVersion.GroupNamesInStreaming.Contains(m_HotfixComponent.LocalVersion.ABInfos[formatPath].GroupName))
             //     {
             //         return m_AssetComponent.AssetLoadManager.ABLoadManager.IsABExistInPersistentDataPath(abPath);
             //     }
             // }
            return true;
        } 
    }
}