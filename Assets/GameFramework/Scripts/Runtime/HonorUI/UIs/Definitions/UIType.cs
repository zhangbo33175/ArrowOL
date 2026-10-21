/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIType.cs
 * author:    云毅
 * created:   2026
 * descrip:   UI 层级类型枚举
 ***************************************************************/
using System;

namespace Honor.Runtime
{
    /// <summary>
    /// UI 层级类型枚举
    /// 标识 UI 属于屏幕层 / 场景层
    /// </summary>
    [Flags]
    public enum UIType : byte
    {
        /// <summary>
        /// 无效 / 未设置
        /// </summary>
        None = 0,

        /// <summary>
        /// 屏幕 UI（常驻屏幕、2D 界面）
        /// </summary>
        Screen = 1,

        /// <summary>
        /// 场景 UI（世界空间、3D 界面）
        /// </summary>
        Scene = 2,
    }
}