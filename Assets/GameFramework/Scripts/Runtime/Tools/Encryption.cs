/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Encryption.cs
 * author:    云毅
 * created:   2026
 * descrip:   异或加密工具类 - 提供二进制数据快速异或加密/解密功能
 ***************************************************************/
using System;

namespace Honor.Runtime
{
    //=========================================================================
    // 异或加密工具类
    //=========================================================================
    /// <summary>
    /// 异或加密工具类
    /// 提供二进制流快速异或运算，用于数据加密与解密
    /// </summary>
    public static partial class Encryption
    {
        /// <summary>
        /// 快速加密长度标识（小于0表示处理整个二进制流）
        /// </summary>
        internal const int QuickEncryptLength = -1;

        #region 快速异或加密（外部调用）
        /// <summary>
        /// 将 bytes 使用 code 做异或运算的快速版本
        /// </summary>
        /// <param name="bytes">原始二进制流</param>
        /// <param name="code">异或二进制流</param>
        /// <returns>原始二进制流通过异或运算后得到的结果</returns>
        public static byte[] GetQuickXorBytes(byte[] bytes, byte[] code)
        {
            return GetXorBytes(bytes, code, QuickEncryptLength);
        }

        /// <summary>
        /// 将 bytes 使用 code 做异或运算的快速版本
        /// 此方法将复用并改写传入的 bytes 作为返回值，不额外分配内存空间
        /// </summary>
        /// <param name="bytes">原始及异或后的二进制流（即是输入也是输出）</param>
        /// <param name="code">异或二进制流</param>
        public static void GetQuickSelfXorBytes(byte[] bytes, byte[] code)
        {
            GetSelfXorBytes(bytes, code, QuickEncryptLength);
        }
        #endregion

        #region 标准异或加密（无指定长度）
        /// <summary>
        /// 将 bytes 使用 code 做异或运算
        /// </summary>
        /// <param name="bytes">原始二进制流</param>
        /// <param name="code">异或二进制流</param>
        /// <returns>原始二进制流通过异或运算后得到的结果</returns>
        public static byte[] GetXorBytes(byte[] bytes, byte[] code)
        {
            return GetXorBytes(bytes, code, QuickEncryptLength);
        }

        /// <summary>
        /// 将 bytes 使用 code 做异或运算
        /// 此方法将复用并改写传入的 bytes 作为返回值，不额外分配内存空间
        /// </summary>
        /// <param name="bytes">原始及异或后的二进制流（即是输入也是输出）</param>
        /// <param name="code">异或二进制流</param>
        public static void GetSelfXorBytes(byte[] bytes, byte[] code)
        {
            GetSelfXorBytes(bytes, code, QuickEncryptLength);
        }
        #endregion

        #region 核心异或加密实现（指定长度）
        /// <summary>
        /// 将 bytes 使用 code 做异或运算（指定处理长度）
        /// </summary>
        /// <param name="bytes">原始二进制流</param>
        /// <param name="code">异或二进制流</param>
        /// <param name="length">异或计算长度，若小于 0，则计算整个二进制流</param>
        /// <returns>原始二进制流通过异或运算后得到的结果</returns>
        public static byte[] GetXorBytes(byte[] bytes, byte[] code, int length)
        {
            if (bytes == null)
            {
                return null;
            }

            int sourceLength = bytes.Length;
            byte[] copy = new byte[sourceLength];
            Buffer.BlockCopy(bytes, 0, copy, 0, sourceLength);
            ApplySelfXor(copy, code, length);
            return copy;
        }

        /// <summary>
        /// 将 bytes 使用 code 做异或运算（指定处理长度）
        /// 此方法将复用并改写传入的 bytes 作为返回值，不额外分配内存空间
        /// </summary>
        /// <param name="bytes">原始及异或后的二进制流（即是输入也是输出）</param>
        /// <param name="code">异或二进制流</param>
        /// <param name="length">异或计算长度，若小于 0，则计算整个二进制流</param>
        /// <exception cref="GameException">异或密钥无效时抛出异常</exception>
        public static void GetSelfXorBytes(byte[] bytes, byte[] code, int length)
        {
            ApplySelfXor(bytes, code, length);
        }

        /// <summary>
        /// 原地对字节流做循环密钥异或（核心实现，加密与解密共用）
        /// </summary>
        /// <param name="bytes">待异或的字节流（输入输出同体）</param>
        /// <param name="code">循环异或密钥</param>
        /// <param name="length">处理长度，小于0或越界时按整段处理</param>
        /// <exception cref="GameException">异或密钥为空或长度无效时抛出异常</exception>
        private static void ApplySelfXor(byte[] bytes, byte[] code, int length)
        {
            if (bytes == null)
            {
                return;
            }

            if (code == null)
            {
                throw new GameException("异或二进制流Code无效。");
            }

            int keyLength = code.Length;
            if (keyLength <= 0)
            {
                throw new GameException("异或二进制流Code长度无效。");
            }

            int sourceLength = bytes.Length;
            int processLength = length < 0 || length > sourceLength ? sourceLength : length;

            for (int offset = 0; offset < processLength; offset++)
            {
                bytes[offset] ^= code[offset % keyLength];
            }
        }
        #endregion
    }
}
