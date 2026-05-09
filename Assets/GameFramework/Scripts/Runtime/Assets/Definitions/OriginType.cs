using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 资源来源路径枚举（标记资源加载的磁盘位置）
    /// </summary>
    [Flags]
    public enum OriginType : byte
    {
        /// <summary>
        /// 无/无效路径
        /// </summary>
        None = 0,

        /// <summary>
        /// 编辑器模式（Editor 环境直接加载）
        /// </summary>
        Editor,

        /// <summary>
        /// 可读写目录（热更新资源存放位置）
        /// </summary>
        Persistent,

        /// <summary>
        /// 只读目录（安装包内置资源位置）
        /// </summary>
        Streaming,
    }
}