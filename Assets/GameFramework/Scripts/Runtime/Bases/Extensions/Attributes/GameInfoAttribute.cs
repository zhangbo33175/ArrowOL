using UnityEngine;

namespace Honor.Runtime
{
    #region 自定义提示信息特性
    /// <summary>
    /// 自定义提示信息特性
    /// 配合自定义编辑器绘制，在 Inspector 面板展示提示文本
    /// 用法：[GameInfo("提示内容", GameInfoAttribute.InfoType.Warning, false)]
    /// </summary>
    public class GameInfoAttribute : PropertyAttribute
    {
        //=========================================================================
        // 提示类型枚举
        //=========================================================================
        /// <summary>
        /// 提示类型：普通信息/警告/错误/无样式
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
        /// 提示文案内容
        /// </summary>
        public string Message;

        /// <summary>
        /// 提示样式类型
        /// </summary>
        public InfoType Type;

        /// <summary>
        /// 是否绘制在字段下方
        /// false = 绘制在字段上方
        /// true = 绘制在字段下方
        /// </summary>
        public bool MessageAfterProperty;

        //=========================================================================
        // 构造函数
        //=========================================================================
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">提示文案</param>
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