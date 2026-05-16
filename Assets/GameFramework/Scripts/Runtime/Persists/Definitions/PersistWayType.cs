/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PersistWayType.cs
 * author:    云毅
 * created:
 * descrip:   持久化存储方式枚举定义
 ***************************************************************/

using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 持久化存储方式类型（可位标记）
    /// 用于区分本地存储使用哪种底层方案
    /// </summary>
    [Flags]
    public enum PersistWayType : byte
    {
        /// <summary>
        /// 文件片段存储
        /// 常规平台使用文件IO；WebGL 平台自动用 PlayerPrefs-V2 替代
        /// </summary>
        FileFragment = 0,

        /// <summary>
        /// PlayerPrefs 存储
        /// 轻量级键值对本地存储
        /// </summary>
        PlayerPrefs = 1
    }
}