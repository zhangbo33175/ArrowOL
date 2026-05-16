/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameTitleAttribute.cs
 * author:    云毅
 * created:   2026
 * descrip:   自定义Inspector面板标题分组特性，用于字段分组分隔展示
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    #region 自定义标题分组特性
    //=========================================================================
    // 自定义标题分组特性
    //=========================================================================
    /// <summary>
    /// 自定义标题分组特性
    /// 用于在 Inspector 面板绘制自定义大标题，实现字段分组分隔
    /// 用法：[GameTitle("分组标题名称")] 标注在字段上方
    /// </summary>
    public class GameTitleAttribute : PropertyAttribute
    {
        /// <summary>
        /// 自定义标题文本
        /// Inspector面板中显示的分组标题内容
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 特性构造函数
        /// </summary>
        /// <param name="title">Inspector面板展示的分组标题文本</param>
        public GameTitleAttribute(string title)
        {
            Title = title;
        }
    }
    #endregion
}