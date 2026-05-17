/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameInfoAttribute.cs
 * author:    云毅
 * created:   2026
 * descrip:   自定义Inspector提示信息特性，支持信息/警告/错误样式
 *            可控制显示位置，配合框架编辑器扩展使用
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    #region 自定义提示信息特性
    /// <summary>
    /// 自定义提示信息特性
    /// </summary>
    /// <remarks>
    /// 配合自定义编辑器绘制，在 Inspector 面板展示提示文本
    /// 支持普通信息、警告、错误三种样式，可控制显示位置
    /// </remarks>
    public class GameInfoAttribute : PropertyAttribute
    {
        //=========================================================================
        // 提示类型枚举
        //=========================================================================
        /// <summary>
        /// 提示信息类型
        /// </summary>
        public enum InfoType
        {
            /// <summary>
            /// 无特殊样式
            /// </summary>
            None,

            /// <summary>
            /// 普通信息提示
            /// </summary>
            Info,

            /// <summary>
            /// 警告提示
            /// </summary>
            Warning,

            /// <summary>
            /// 错误提示
            /// </summary>
            Error
        }

        //=========================================================================
        // 公共字段
        //=========================================================================
        /// <summary>
        /// 提示显示文本内容
        /// </summary>
        public string Message;

        /// <summary>
        /// 提示信息样式类型
        /// </summary>
        public InfoType Type;

        /// <summary>
        /// 提示文本显示位置
        /// </summary>
        /// <remarks>false：字段上方 | true：字段下方</remarks>
        public bool MessageAfterProperty;

        //=========================================================================
        // 构造函数
        //=========================================================================
        /// <summary>
        /// 自定义提示信息特性构造函数
        /// </summary>
        /// <param name="message">提示文本内容</param>
        /// <param name="type">提示样式类型</param>
        /// <param name="messageAfterProperty">是否显示在属性下方</param>
        public GameInfoAttribute(string message, InfoType type, bool messageAfterProperty)
        {
            Message = message;
            Type = type;
            MessageAfterProperty = messageAfterProperty;
        }
    }
    #endregion
}