/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameHeaderAttribute.cs
 * author:    云毅
 * created:   2036
 * descrip:   自定义Inspector头部标题特性，替代Unity原生Header
 *            用于编辑器分组标题绘制，适配框架自定义编辑器扩展
 ***************************************************************/

using System;
using UnityEngine;

namespace Honor.Runtime
{
    #region 自定义头部标题特性
    /// <summary>
    /// 自定义头部标题特性
    /// </summary>
    /// <remarks>
    /// 用于在Inspector面板自定义绘制分组标题文本
    /// 替代原生 Header，适配 Honor 框架自定义编辑器绘制逻辑
    /// </remarks>
    [Serializable]
    public class GameHeaderAttribute : PropertyAttribute
    {
        //=========================================================================
        // 公共字段
        //=========================================================================
        /// <summary>
        /// 分组标题显示文本
        /// </summary>
        public readonly string Header;

        //=========================================================================
        // 构造函数
        //=========================================================================
        /// <summary>
        /// 初始化自定义标题特性
        /// </summary>
        /// <param name="header">Inspector中显示的标题文字</param>
        public GameHeaderAttribute(string header)
        {
            Header = header;
        }
    }
    #endregion
}