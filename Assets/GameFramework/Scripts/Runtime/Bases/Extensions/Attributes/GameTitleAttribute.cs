using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 自定义标题分组特性
    /// 用于在 Inspector 面板绘制自定义大标题，实现字段分组分隔
    /// 用法：[GameTitle("分组标题名称")]
    /// </summary>
    public class GameTitleAttribute : PropertyAttribute
    {
        /// <summary>
        /// 自定义标题文本
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title">面板展示的分组标题</param>
        public GameTitleAttribute(string title)
        {
            Title = title;
        }
    }
}