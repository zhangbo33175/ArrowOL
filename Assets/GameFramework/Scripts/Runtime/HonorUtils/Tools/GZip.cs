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
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(content);
            byte[] compressed = CompressRaw(utf8Bytes);
            return Convert.ToBase64String(compressed);
        }

        /// <summary>
        /// 将 Base64 字符串解压还原为原始字符串
        /// </summary>
        /// <param name="content">Base64压缩字符串</param>
        /// <returns>原始明文字符串</returns>
        public static string UncompressFromBase64(string content)
        {
            byte[] compressedBytes = Convert.FromBase64String(content);
            using MemoryStream input = new MemoryStream(compressedBytes);
            using GZipInputStream gzipStream = new GZipInputStream(input);

            byte[] plain = DrainAll(gzipStream);
            return Encoding.UTF8.GetString(plain);
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
            return CompressRaw(content);
        }

        /// <summary>
        /// 解压字节数组
        /// </summary>
        /// <param name="content">压缩字节数组</param>
        /// <returns>解压后的原始字节数组</returns>
        public static byte[] UncompressToBytes(byte[] content)
        {
            using MemoryStream input = new MemoryStream(content);
            using GZipInputStream gzipStream = new GZipInputStream(input);
            return DrainAll(gzipStream);
        }
        #endregion

        #region 内部实现
        /// <summary>
        /// 对原始字节执行 GZip 压缩
        /// </summary>
        /// <param name="raw">待压缩字节</param>
        /// <returns>GZip 压缩结果</returns>
        private static byte[] CompressRaw(byte[] raw)
        {
            using MemoryStream output = new MemoryStream();
            using (GZipOutputStream gzipStream = new GZipOutputStream(output))
            {
                gzipStream.Write(raw, 0, raw.Length);
            }
            return output.ToArray();
        }

        /// <summary>
        /// 从 GZip 输入流循环读取全部解压数据
        /// </summary>
        /// <param name="gzipStream">GZip 解压输入流</param>
        /// <returns>解压后的完整字节</returns>
        private static byte[] DrainAll(GZipInputStream gzipStream)
        {
            using MemoryStream output = new MemoryStream();
            byte[] buffer = new byte[256];
            int read;
            while ((read = gzipStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                output.Write(buffer, 0, read);
            }
            return output.ToArray();
        }
        #endregion
    }
}
