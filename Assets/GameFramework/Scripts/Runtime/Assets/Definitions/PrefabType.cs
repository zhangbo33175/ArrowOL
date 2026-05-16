/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PrefabTypeDefine.cs
 * author:    云毅
 * created:   2026
 * descrip:   预制体类型枚举定义 - 用于资源加载、实例化、管理逻辑区分
 ***************************************************************/

using System;

namespace Honor.Runtime
{
    #region 预制体类型枚举
    /// <summary>
    /// 预制体类型枚举（用于资源加载、实例化、管理逻辑区分）
    /// </summary>
    [Flags]
    public enum PrefabType : byte
    {
        /// <summary>
        /// UI 界面预制体
        /// </summary>
        UI = 0,

        /// <summary>
        /// 游戏实体预制体（角色、怪物、特效、场景物件等）
        /// </summary>
        Entity,
    }
    #endregion
}