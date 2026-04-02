using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 预制体本体类型
    /// </summary>
    [Flags]
    public enum PrefabType : byte
    {
        /// <summary>
        /// UI类型
        /// </summary>
        UI = 0,

        /// <summary>
        /// 实体类型
        /// </summary>
        Entity,

    }
}


