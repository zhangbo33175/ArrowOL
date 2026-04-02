using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 预制体本体类型
    /// </summary>
    public enum PrefabDetailType : byte
    {
        /// <summary>
        /// 无效
        /// </summary>
        None = 0,

        /// <summary>
        /// UI对象
        /// </summary>
        UI,

        /// <summary>
        /// 游戏对象
        /// </summary>
        GameObject,
    }
}


