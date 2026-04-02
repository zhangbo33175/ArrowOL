using System;

namespace Honor.Runtime
{
    [Flags]
    public enum PersistWayType : byte
    {
        /// <summary>
        /// 文件片段方式。
        /// 因网页端不支持文件IO，因此WebGL网页端将由PlayerPrefs-V2取代。
        /// </summary>
        FileFragment = 0,

        /// <summary>
        /// 数据库方式。
        /// </summary>
        PlayerPrefs = 1,

    }
}


