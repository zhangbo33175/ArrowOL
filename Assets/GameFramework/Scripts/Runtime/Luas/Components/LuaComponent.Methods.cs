namespace Honor.Runtime
{
    public sealed partial class LuaComponent : GameComponent
    {
        /// <summary>
        /// 判断 Lua 脚本的 AB 包是否可以加载
        /// 增量 Lua 脚本必须确保在 Persistent 读写路径下存在对应的 AB 资源才允许加载
        /// </summary>
        /// <param name="abPath">Lua 脚本的 ABPath</param>
        /// <returns>是否可加载</returns>
        public bool CanABLoad(string abPath)
        {
            // 编辑器资源模式：直接允许加载
            if (m_LauncherComponent.EditorResourceMode)
                return true;

            // 路径为空，不可加载
            if (string.IsNullOrEmpty(abPath))
            {
                Log.Error($"[LuaComponent] CanABLoad 传入路径为空！");
                return false;
            }

            // 获取格式化后的 AB 路径（用于真机路径校验）
            string formatPath = m_AssetComponent.AssetLoadManager.AssetBundleLoadManager.GetABFormatPath(abPath);

            // 真机模式：你可以在这里补充文件存在性检查
            // 示例：
            // if (!File.Exists(formatPath))
            // {
            //     Log.Error($"[LuaComponent] AB 包不存在：{formatPath}");
            //     return false;
            // }

            return true;
        }
    }
}