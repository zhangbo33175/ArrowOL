/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  DirectoryWatcher.cs
 * author:    云毅
 * created:   2026
 * descrip:   文件夹/文件监听工具，用于 Lua 热重载
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 文件夹/文件监听工具（用于 Lua 热重载）
    /// 监听文件变化并触发回调
    /// </summary>
    public class DirectoryWatcher
    {
        //=========================================================================
        #region 字段
        private FileSystemWatcher m_Watcher;
        #endregion

        //=========================================================================
        #region 构造函数
        public DirectoryWatcher(string dirPath, string filter, FileSystemEventHandler handler)
        {
            CreateWatch(dirPath, filter, handler);
        }
        #endregion

        //=========================================================================
        #region 私有方法
        /// <summary>
        /// 创建文件监听器
        /// </summary>
        private void CreateWatch(string dirPath, string filter, FileSystemEventHandler handler)
        {
            if (string.IsNullOrEmpty(dirPath) || !Directory.Exists(dirPath))
                return;

            m_Watcher = new FileSystemWatcher
            {
                IncludeSubdirectories = true,
                Path = dirPath,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = filter,
                EnableRaisingEvents = true,
                InternalBufferSize = 10240 // 增大缓冲区防止频繁变更溢出
            };

            m_Watcher.Changed += handler;
        }
        #endregion

        //=========================================================================
        #region 公共方法
        /// <summary>
        /// 释放监听器（必须调用，防止内存泄漏）
        /// </summary>
        public void Close()
        {
            if (m_Watcher != null)
            {
                m_Watcher.EnableRaisingEvents = false;
                m_Watcher.Dispose();
                m_Watcher = null;
            }
        }
        #endregion
    }
}