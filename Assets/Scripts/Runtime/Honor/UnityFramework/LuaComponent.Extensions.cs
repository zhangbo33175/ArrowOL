using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua层Sound全局事件派发
    /// </summary>
    /// <param name="localizingKeyName">音效Sound Key字段名称</param>
    /// <returns></returns>
    public delegate LuaTable LuaSoundCSEventDelegate(string soundKeyName);

    public sealed partial class LuaComponent
    {
        /// <summary>
        /// Lua层音效
        /// </summary>
        private LuaSoundCSEventDelegate m_LuaSoundCSEventDelegate;

        public LuaSoundCSEventDelegate LuaSoundCSEventDelegate
        {
            get { return m_LuaSoundCSEventDelegate; }
        }

        /// <summary>
        /// 初始化游戏Lua绑定
        /// </summary>
        public void InitGameLuaBindings()
        {
            // 获取Lua层Sound全局事件派发
            m_LuaSoundCSEventDelegate = GetGlobalValue<LuaSoundCSEventDelegate>("GameSoundCallback");
            if (m_LuaSoundCSEventDelegate == null)
            {
                Log.Fatal("LuaSoundCSEventDelegate 无效。");
                return;
            }
        }
    }
}