/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildLog.cs
 * author:    云毅
 * created:   2026
 * descrip:   打包流水线 - 统一日志输出
 *            统一 Debug.Log，并通过 OnLog 事件解耦转发给打包窗口日志区。
 *            PackWindow 由并行任务实现，本文件不直接引用其类型，
 *            窗口侧注册 OnLog 即可把日志追加到自身日志区。
 ***************************************************************/

using System;
using UnityEngine;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// 打包日志输出
    /// 说明：所有打包步骤的日志统一经此出口，避免直接耦合窗口类型。
    /// </summary>
    public static class BuildLog
    {
        #region 事件
        /// <summary>
        /// 日志追加事件
        /// 说明：打包窗口（PackWindow）在 OnEnable 时注册此事件、OnDisable 时注销，
        ///       收到回调后把字符串追加到自身日志区；未注册时仅写 Debug.Log。
        /// </summary>
        public static Action<string> OnLog = null;
        #endregion

        #region 输出
        /// <summary>
        /// 输出一条打包日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        public static void Log(string msg)
        {
            Debug.Log(msg);

            Action<string> handler = OnLog;
            if (handler != null)
            {
                handler.Invoke(msg);
            }
        }
        #endregion
    }
}
