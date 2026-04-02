
namespace Honor.Runtime
{
    public sealed partial class LuaComponent : GameComponent
    {
        /// <summary>
        /// Luac的AB是否可以加载
        /// 增量Lua脚本必须确保在Persistent读写路径下存在对应的AB资源才允许进行加载
        /// </summary>
        /// <param name="abPath">Lua脚本的ABPath</param>
        public bool CanABLoad(string abPath)
        {
            if(!m_LauncherComponent.EditorResourceMode)
            {
                string formatPath = m_AssetComponent.AssetLoadManager.AssetBundleLoadManager.GetABFormatPath(abPath);
            }
            return true;
        }

    }

}


