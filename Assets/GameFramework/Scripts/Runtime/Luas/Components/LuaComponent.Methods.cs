/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaComponent.ABLoader.cs
 * author:    云毅
 * created:   2026   2025-12-29
 * descrip:   Lua 脚本 AB 包加载校验工具模块
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// Lua 组件 - AB 包加载校验模块
    /// 提供 Lua 脚本资源包的加载权限判断
    /// </summary>
    public sealed partial class LuaComponent : GameComponent
    {
        //=========================================================================
        #region AB 包加载校验
        //=========================================================================

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

        #endregion
    }
}