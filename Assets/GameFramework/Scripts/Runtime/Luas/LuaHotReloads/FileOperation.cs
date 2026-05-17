/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  FileOperation.cs
 * author:    云毅
 * created:   2026
 * descrip:   文件操作工具类，安全读取文件，防止占用与异常，用于 Lua 热重载
 ***************************************************************/

using System;
using System.IO;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 文件操作工具类（安全读取、防止文件占用/异常）
    /// 主要用于 Lua 热重载、配置文件读取
    /// </summary>
    public static class FileOperation
    {
        //=========================================================================
        #region 公共静态方法
        //=========================================================================

        /// <summary
        /// 安全读取文件字节数组（捕获异常、防止崩溃）
        /// </summary>
        /// <param name="inFile">文件完整路径</param>
        /// <returns>文件字节数组，读取失败返回 null</returns>
        public static byte[] SafeReadAllBytes(string inFile)
        {
            try
            {
                // 空路径检查
                if (string.IsNullOrEmpty(inFile))
                    return null;

                // 文件不存在检查
                if (!File.Exists(inFile))
                    return null;

                // 设置正常属性，避免文件被占用/只读
                File.SetAttributes(inFile, FileAttributes.Normal);

                // 直接读取二进制（比 ReadAllText 更稳定、更快）
                return File.ReadAllBytes(inFile);
            }
            catch (GameException ex)
            {
                Log.Error($"[FileOperation] SafeReadAllBytes 失败 -> 路径: {inFile}, 错误: {ex.Message}");
                return null;
            }
        }

        #endregion
    }
}