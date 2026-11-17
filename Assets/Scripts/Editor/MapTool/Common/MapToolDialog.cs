/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapToolDialog.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器对话框统一门面。收敛 EditorUtility.DisplayDialog 的
 *            标题/按钮字面量（提示、成功、错误、确定、取消），调用方只描述语义。
 ***************************************************************/

using UnityEditor;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 编辑器提示框门面（标题与按钮文案唯一口径）
    /// </summary>
    public static class MapToolDialog
    {
        #region 文案常量

        /// <summary>标题：提示</summary>
        public const string TitleInfo = "提示";
        /// <summary>标题：成功</summary>
        public const string TitleSuccess = "成功";
        /// <summary>标题：错误</summary>
        public const string TitleError = "错误";
        /// <summary>标题：确认</summary>
        public const string TitleConfirm = "确认";
        /// <summary>按钮：确定</summary>
        public const string ButtonOk = "确定";
        /// <summary>按钮：取消</summary>
        public const string ButtonCancel = "取消";

        #endregion

        /// <summary>
        /// 单按钮提示（标题「提示」）
        /// </summary>
        /// <param name="message">正文</param>
        public static void Info(string message)
        {
            EditorUtility.DisplayDialog(TitleInfo, message, ButtonOk);
        }

        /// <summary>
        /// 单按钮提示（自定义标题）
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">正文</param>
        public static void Info(string title, string message)
        {
            EditorUtility.DisplayDialog(title, message, ButtonOk);
        }

        /// <summary>
        /// 成功提示（标题「成功」）
        /// </summary>
        /// <param name="message">正文</param>
        public static void Success(string message)
        {
            EditorUtility.DisplayDialog(TitleSuccess, message, ButtonOk);
        }

        /// <summary>
        /// 错误提示（标题「错误」）
        /// </summary>
        /// <param name="message">正文</param>
        public static void Error(string message)
        {
            EditorUtility.DisplayDialog(TitleError, message, ButtonOk);
        }

        /// <summary>
        /// 确认框（确定 / 取消）
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">正文</param>
        /// <returns>true=确定，false=取消</returns>
        public static bool Confirm(string title, string message)
        {
            return EditorUtility.DisplayDialog(title, message, ButtonOk, ButtonCancel);
        }

        /// <summary>
        /// 确认框（标题「确认」）
        /// </summary>
        /// <param name="message">正文</param>
        /// <returns>true=确定，false=取消</returns>
        public static bool Confirm(string message)
        {
            return EditorUtility.DisplayDialog(TitleConfirm, message, ButtonOk, ButtonCancel);
        }

        /// <summary>
        /// 单按钮提示（自定义确认按钮文案，如「知道了」）
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">正文</param>
        /// <param name="okLabel">按钮文案</param>
        public static void Info(string title, string message, string okLabel)
        {
            EditorUtility.DisplayDialog(title, message, okLabel);
        }

        /// <summary>
        /// 确认框（自定义按钮文案，如「删除 / 重新加载」）
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">正文</param>
        /// <param name="okLabel">确认按钮文案</param>
        /// <param name="cancelLabel">取消按钮文案</param>
        /// <returns>true=确认，false=取消</returns>
        public static bool Confirm(string title, string message, string okLabel, string cancelLabel)
        {
            return EditorUtility.DisplayDialog(title, message, okLabel, cancelLabel);
        }
    }
}
