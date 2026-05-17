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
            return GetXorBytes(bytes, code, -1);
        }

        /// <summary>
        /// 将 bytes 使用 code 做异或运算
        /// 此方法将复用并改写传入的 bytes 作为返回值，不额外分配内存空间
        /// </summary>
        /// <param name="bytes">原始及异或后的二进制流（即是输入也是输出）</param>
        /// <param name="code">异或二进制流</param>
        public static void GetSelfXorBytes(byte[] bytes, byte[] code)
        {
            GetSelfXorBytes(bytes, code, -1);
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

            int bytesLength = bytes.Length;
            byte[] results = new byte[bytesLength];
            Buffer.BlockCopy(bytes, 0, results, 0, bytesLength);
            GetSelfXorBytes(results, code, length);
            return results;
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
            if (bytes == null)
            {
                return;
            }

            if (code == null)
            {
                throw new GameException("异或二进制流Code无效。");
            }

            int codeLength = code.Length;
            if (codeLength <= 0)
            {
                throw new GameException("异或二进制流Code长度无效。");
            }

            int bytesLength = bytes.Length;
            if (length < 0 || length > bytesLength)
            {
                length = bytesLength;
            }

            int codeIndex = 0;
            for (int i = 0; i < length; i++)
            {
                bytes[i] ^= code[codeIndex++];
                codeIndex %= codeLength;
            }
        }
        #endregion
    }
}