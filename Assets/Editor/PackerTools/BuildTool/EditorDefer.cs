/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EditorDefer.cs
 * author:    云毅
 * created:   2026
 * descrip:   编辑器阻塞操作延迟执行工具
 *            统一把"打包 / 弹窗 / 文件面板 / 资源刷新"等阻塞型编辑器操作，
 *            推迟到当前 OnGUI 布局 pass 结束之后（delayCall）再执行，
 *            避免在 OnGUI 布局作用域内重入编辑器主循环、破坏 IMGUI 布局栈，
 *            从而根除 “EndLayoutGroup: BeginLayoutGroup must be called first”。
 ***************************************************************/

using System;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// 编辑器阻塞操作延迟执行工具（静态）
    /// 说明：所有在 OnGUI / 自定义 EditorWindow 布局作用域内触发的阻塞操作，
    ///       均应经 <see cref="DeferFromOnGUI"/> 收口，禁止直接同步执行。
    /// </summary>
    public static class EditorDefer
    {
        #region 延迟执行
        /// <summary>
        /// 把阻塞操作安全地推迟到当前 OnGUI/布局 pass 结束之后执行（避免重入破坏 IMGUI 布局栈）
        /// </summary>
        /// <remarks>
        /// 实现说明：
        /// <list type="bullet">
        /// <item>经 <see cref="EditorApplication.delayCall"/> 注册，在本次 OnGUI 返回后的下一帧执行；</item>
        /// <item>对操作做异常兜底（Debug.LogException），防止单次异常中断后续未执行的 delayCall 队列；</item>
        /// <item>调用方如需感知结果（如构建成功与否），请在闭包内自行 try/catch 并记录日志。</item>
        /// </list>
        /// </remarks>
        /// <param name="action">待延迟执行的阻塞操作</param>
        public static void DeferFromOnGUI(Action action)
        {
            if (action == null)
                return;

            EditorApplication.delayCall += () =>
            {
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            };
        }
        #endregion
    }
}
