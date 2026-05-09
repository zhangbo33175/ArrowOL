using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 预制体详细类型（用于区分UI与普通游戏对象，做差异化管理）
    /// </summary>
    public enum PrefabDetailType : byte
    {
        /// <summary>
        /// 未定义 / 无效类型
        /// </summary>
        None = 0,

        /// <summary>
        /// UI 界面预制体（由 UIManager 管理）
        /// </summary>
        UI,

        /// <summary>
        /// 普通游戏对象预制体（3D角色/特效/场景物件等）
        /// </summary>
        GameObject,
    }
}