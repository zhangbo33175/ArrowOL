/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameReadOnlyAttribute.cs
 * author:    云毅
 * created:   2026/5/16
 * descrip:   Inspector 面板自定义只读特性，标记字段不可编辑
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    #region 自定义只读特性
    /// <summary>
    /// 自定义只读特性
    /// 标记在序列化字段上，使其在 Inspector 面板仅展示、禁止编辑
    /// 配合自定义编辑器绘制实现全局只读效果
    /// </summary>
    public class GameReadOnlyAttribute : PropertyAttribute
    {
        //=========================================================================
        // 构造函数
        //=========================================================================
        /// <summary>
        /// 构造函数：创建只读特性实例
        /// </summary>
        public GameReadOnlyAttribute()
        {
        }
    }
    #endregion
}