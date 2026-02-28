using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 资源位置来源类型
    /// </summary>
    [Flags]
    public enum ResourceType : byte
    {
        /// <summary>
        /// 无效
        /// </summary>
        None = 0,

        /// <summary>
        /// 编辑器区
        /// </summary>
        Editor,

        /// <summary>
        /// 读写区
        /// </summary>
        Persistent,

        /// <summary>
        /// 只读区
        /// </summary>
        Streaming,
    }
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