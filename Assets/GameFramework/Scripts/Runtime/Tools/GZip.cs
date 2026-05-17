/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GZip.cs
 * author:    云毅
 * created:   2026
 * descrip:   GZip压缩解压工具类，支持字节/字符串/Base64格式互转
 ***************************************************************/
using ICSharpCode.SharpZipLib.GZip;
using System;
using System.IO;
using System.Text;

namespace Honor.Runtime
{
    //=========================================================================
    // GZip 压缩解压工具类
    //=========================================================================
    /// <summary>
    /// GZip 压缩解压工具类
    /// 提供字符串/字节数组的压缩、解压，并支持 Base64 编码转换
    /// 用于网络传输、本地存档、配置文件体积优化
    /// </summary>
    public static class GZip
    {
        #region 字符串压缩解压（Base64）
        //=========================================================================
        // 字符串压缩解压（Base64）
        //=========================================================================
        /// <summary>
        /// 将字符串压缩并转换为 Base64 字符串
        /// </summary>
        /// <param name="content">原始明文字符串</param>
        /// <returns>压缩+Base64编码后的字符串</returns>
        public static string CompressToBase64(string content)
        {
            using MemoryStream ms = new MemoryStream();
            using GZipOutputStream gzip = new GZipOutputStream(ms);
            byte[] binary = Encoding.UTF8.GetBytes(content);
            gzip.Write(binary, 0, binary.Length);
            gzip.Close();
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// 将 Base64 字符串解压还原为原始字符串
        /// </summary>
        /// <param name="content">Base64压缩字符串</param>
        /// <returns>原始明文字符串</returns>
        public static string UncompressFromBase64(string content)
        {
            byte[] compressedContent = Convert.FromBase64String(content);
            
            using MemoryStream compressedMs = new MemoryStream(compressedContent);
            using GZipInputStream gzip = new GZipInputStream(compressedMs);
            using MemoryStream ms = new MemoryStream();
            
            byte[] data = new byte[256];
            int count;
            while ((count = gzip.Read(data, 0, data.Length)) != 0)
            {
                ms.Write(data, 0, count);
            }

            byte[] uncompressed = ms.ToArray();
            return Encoding.UTF8.GetString(uncompressed);
        }
        #endregion

        #region 字节数组压缩解压
        //=========================================================================
        // 字节数组压缩解压
        //=========================================================================
        /// <summary>
        /// 压缩字节数组
        /// </summary>
        /// <param name="content">原始字节数组</param>
        /// <returns>压缩后的字节数组</returns>
        public static byte[] CompressToBytes(byte[] content)
        {
            //Profiler.BeginSample("GZip");
            using MemoryStream ms = new MemoryStream();
            using GZipOutputStream gzip = new GZipOutputStream(ms);
            gzip.Write(content, 0, content.Length);
            gzip.Close();
            //Profiler.EndSample();
            return ms.ToArray();
        }

        /// <summary>
        /// 解压字节数组
        /// </summary>
        /// <param name="content">压缩字节数组</param>
        /// <returns>解压后的原始字节数组</returns>
        public static byte[] UncompressToBytes(byte[] content)
        {
            using MemoryStream compressedMs = new MemoryStream(content);
            using GZipInputStream gzip = new GZipInputStream(compressedMs);
            using MemoryStream ms = new MemoryStream();
            
            byte[] data = new byte[256];
            int count;
            while ((count = gzip.Read(data, 0, data.Length)) != 0)
            {
                ms.Write(data, 0, count);
            }

            return ms.ToArray();
        }
        #endregion
    }
}