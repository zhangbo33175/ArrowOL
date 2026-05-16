/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PrefabDetailTypeDefine.cs
 * author:    云毅
 * created:   2026
 * descrip:   预制体详细类型枚举 - 区分UI与普通GameObject，用于差异化管理
 ***************************************************************/

using System;

namespace Honor.Runtime
{
    #region 预制体详细类型枚举
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
    #endregion
}