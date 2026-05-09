using System;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 自定义头部标题特性
    /// 用于在Inspector面板自定义绘制分组标题文本
    /// 替代原生 Header，适配框架自定义编辑器绘制
    /// </summary>
    [Serializable]
    public class GameHeaderAttribute : PropertyAttribute
    {
        /// <summary>
        /// 标题文本内容
        /// </summary>
        public readonly string Header;

        /// <summary>
        /// 构造自定义标题特性
        /// </summary>
        /// <param name="header">标题文字</param>
        public GameHeaderAttribute(string header)
        {
            Header = header;
        }
    }
}