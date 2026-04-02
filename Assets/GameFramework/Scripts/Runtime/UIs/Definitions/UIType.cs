using System;

namespace Honor.Runtime
{
    /// <summary>
    /// UI类型
    /// </summary>
    [Flags]
    public enum UIType : byte
    {
        /// <summary>
        /// 无效
        /// </summary>
        None = 0,

        /// <summary>
        /// 屏幕UI
        /// </summary>
        Screen,

        /// <summary>
        /// 场景UI
        /// </summary>
        Scene,
    }

}


