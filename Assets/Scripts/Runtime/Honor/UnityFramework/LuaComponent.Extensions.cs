using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua 层音效全局事件委托
    /// 作用：C# 调用 Lua 播放音效
    /// </summary>
    /// <param name="soundKeyName">音效配置 Key</param>
    /// <returns>LuaTable</returns>
    public delegate LuaTable LuaSoundCSEventDelegate(string soundKeyName);

    /// <summary>
    /// Lua 交互组件（ partial 分部类）
    /// 负责初始化 Lua 绑定、提供 C# → Lua 调用委托
    /// </summary>
    public sealed partial class LuaComponent
    {
        /// <summary>
        /// Lua 层音效全局事件委托实例
        /// </summary>
        private LuaSoundCSEventDelegate m_LuaSoundCSEventDelegate;

        /// <summary>
        /// 对外只读访问：Lua 音效事件委托
        /// </summary>
        public LuaSoundCSEventDelegate LuaSoundCSEventDelegate
        {
            get { return m_LuaSoundCSEventDelegate; }
        }

        /// <summary>
        /// 初始化游戏 Lua 绑定（获取全局委托、检查有效性）
        /// </summary>
        public void InitGameLuaBindings()
        {
            // 获取 Lua 全局注册的音效回调函数：GameSoundCallback
            m_LuaSoundCSEventDelegate = GetGlobalValue<LuaSoundCSEventDelegate>("GameSoundCallback");

            // 委托为空则报错
            if (m_LuaSoundCSEventDelegate == null)
            {
                Log.Fatal("LuaSoundCSEventDelegate 无效。");
                return;
            }
        }
    }
}