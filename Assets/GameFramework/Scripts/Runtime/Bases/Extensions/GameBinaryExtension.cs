/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameBinaryExtension.cs
 * author:    云毅
 * created:   2026   2025
 * descrip:   二进制流扩展方法 - 7Bit编码读写、XOR加密字符串读写
 ***************************************************************/
using System;
using System.IO;
using UnityEngine;


namespace Honor.Runtime
{
    //=========================================================================
    // 
    //=========================================================================
    /// <summary>
    /// 二进制流扩展方法
    /// <para>提供 7Bit 编码整数读写、异或加密字符串读写功能</para>
    /// </summary>
    public static class GameBinaryExtension
    {
        #region 静态缓存
        //=========================================================================
        // 静态缓存（避免频繁 GC 分配）
        //=========================================================================
        /// <summary>
        /// 缓存字节数组（避免频繁 GC 分配）
        /// </summary>
        private static readonly byte[] s_CachedBytes = new byte[byte.MaxValue];
        #endregion

        #region 7Bit 编码 Int32 读写
        //=========================================================================
        // 7Bit 编码 Int32 / UInt32 读写
        //=========================================================================
        /// <summary>
        /// 从二进制流读取 7Bit 编码的 32 位有符号整数
        /// </summary>
        /// <param name="binaryReader">二进制读取器</param>
        /// <returns>解码后的 int 值</returns>
        /// <exception cref="ArgumentNullException">binaryReader 为 null 时抛出</exception>
        /// <exception cref="InvalidDataException">7Bit 编码数据无效时抛出</exception>
        public static int Read7BitEncodedInt32(this BinaryReader binaryReader)
        {
            if (binaryReader == null)
                throw new ArgumentNullException(nameof(binaryReader));

            int value = 0;
            int shift = 0;
            byte b;

            do
            {
                if (shift >= 35)
                    throw new InvalidDataException("7Bit 编码整数格式无效，数据已损坏或被篡改。");

                b = binaryReader.ReadByte();
                value |= (b & 0x7F) << shift;
                shift += 7;
            }
            while ((b & 0x80) != 0);

            return value;
        }

        /// <summary>
        /// 向二进制流写入 7Bit 编码的 32 位有符号整数
        /// </summary>
        /// <param name="binaryWriter">二进制写入器</param>
        /// <param name="value">要写入的 int 值</param>
        /// <exception cref="ArgumentNullException">binaryWriter 为 null 时抛出</exception>
        public static void Write7BitEncodedInt32(this BinaryWriter binaryWriter, int value)
        {
            if (binaryWriter == null)
                throw new ArgumentNullException(nameof(binaryWriter));

            uint num = (uint)value;
            while (num >= 0x80)
            {
                binaryWriter.Write((byte)(num | 0x80));
                num >>= 7;
            }
            binaryWriter.Write((byte)num);
        }

        /// <summary>
        /// 从二进制流读取 7Bit 编码的 32 位无符号整数
        /// </summary>
        /// <param name="binaryReader">二进制读取器</param>
        /// <returns>解码后的 uint 值</returns>
        public static uint Read7BitEncodedUInt32(this BinaryReader binaryReader)
        {
            return (uint)Read7BitEncodedInt32(binaryReader);
        }

        /// <summary>
        /// 向二进制流写入 7Bit 编码的 32 位无符号整数
        /// </summary>
        /// <param name="binaryWriter">二进制写入器</param>
        /// <param name="value">要写入的 uint 值</param>
        public static void Write7BitEncodedUInt32(this BinaryWriter binaryWriter, uint value)
        {
            Write7BitEncodedInt32(binaryWriter, (int)value);
        }
        #endregion

        #region 7Bit 编码 Int64 读写
        //=========================================================================
        // 7Bit 编码 Int64 / UInt64 读写
        //=========================================================================
        /// <summary>
        /// 从二进制流读取 7Bit 编码的 64 位有符号整数
        /// </summary>
        /// <param name="binaryReader">二进制读取器</param>
        /// <returns>解码后的 long 值</returns>
        /// <exception cref="ArgumentNullException">binaryReader 为 null 时抛出</exception>
        /// <exception cref="InvalidDataException">7Bit 编码数据无效时抛出</exception>
        public static long Read7BitEncodedInt64(this BinaryReader binaryReader)
        {
            if (binaryReader == null)
                throw new ArgumentNullException(nameof(binaryReader));

            long value = 0L;
            int shift = 0;
            byte b;

            do
            {
                if (shift >= 70)
                    throw new InvalidDataException("7Bit 编码长整数格式无效，数据已损坏。");

                b = binaryReader.ReadByte();
                value |= (b & 0x7FL) << shift;
                shift += 7;
            }
            while ((b & 0x80) != 0);

            return value;
        }

        /// <summary>
        /// 向二进制流写入 7Bit 编码的 64 位有符号整数
        /// </summary>
        /// <param name="binaryWriter">二进制写入器</param>
        /// <param name="value">要写入的 long 值</param>
        /// <exception cref="ArgumentNullException">binaryWriter 为 null 时抛出</exception>
        public static void Write7BitEncodedInt64(this BinaryWriter binaryWriter, long value)
        {
            if (binaryWriter == null)
                throw new ArgumentNullException(nameof(binaryWriter));

            ulong num = (ulong)value;
            while (num >= 0x80)
            {
                binaryWriter.Write((byte)(num | 0x80));
                num >>= 7;
            }
            binaryWriter.Write((byte)num);
        }

        /// <summary>
        /// 从二进制流读取 7Bit 编码的 64 位无符号整数
        /// </summary>
        /// <param name="binaryReader">二进制读取器</param>
        /// <returns>解码后的 ulong 值</returns>
        public static ulong Read7BitEncodedUInt64(this BinaryReader binaryReader)
        {
            return (ulong)Read7BitEncodedInt64(binaryReader);
        }

        /// <summary>
        /// 向二进制流写入 7Bit 编码的 64 位无符号整数
        /// </summary>
        /// <param name="binaryWriter">二进制写入器</param>
        /// <param name="value">要写入的 ulong 值</param>
        public static void Write7BitEncodedUInt64(this BinaryWriter binaryWriter, ulong value)
        {
            Write7BitEncodedInt64(binaryWriter, (long)value);
        }
        #endregion

        #region XOR 加密字符串读写
        //=========================================================================
        // XOR 自加密字符串读写扩展
        //=========================================================================
        /// <summary>
        /// 读取经过 XOR 自加密的字符串
        /// </summary>
        /// <param name="binaryReader">二进制读取器</param>
        /// <param name="encryptKey">加密密钥字节数组</param>
        /// <returns>解密后的字符串</returns>
        /// <exception cref="ArgumentNullException">参数为空时抛出</exception>
        /// <exception cref="InvalidOperationException">密钥为空时抛出</exception>
        public static string ReadEncryptedString(this BinaryReader binaryReader, byte[] encryptKey)
        {
            if (binaryReader == null)
                throw new ArgumentNullException(nameof(binaryReader));
            if (encryptKey == null || encryptKey.Length == 0)
                throw new InvalidOperationException("加密密钥不能为空！");

            byte length = binaryReader.ReadByte();
            if (length <= 0)
                return string.Empty;

            // 读取字节到缓存
            for (byte i = 0; i < length; i++)
            {
                s_CachedBytes[i] = binaryReader.ReadByte();
            }
            
            Encryption.GetQuickSelfXorBytes(s_CachedBytes, encryptKey);

            // 转换字符串
            string result = System.Text.Encoding.UTF8.GetString(s_CachedBytes, 0, length);

            // 清空缓存，防止数据残留
            Array.Clear(s_CachedBytes, 0, length);

            return result;
        }

        /// <summary>
        /// 写入经过 XOR 自加密的字符串
        /// </summary>
        /// <param name="binaryWriter">二进制写入器</param>
        /// <param name="value">要写入的字符串</param>
        /// <param name="encryptKey">加密密钥字节数组</param>
        /// <exception cref="ArgumentNullException">参数为空时抛出</exception>
        /// <exception cref="InvalidOperationException">密钥为空或字符串超长时抛出</exception>
        public static void WriteEncryptedString(this BinaryWriter binaryWriter, string value, byte[] encryptKey)
        {
            if (binaryWriter == null)
                throw new ArgumentNullException(nameof(binaryWriter));
            if (encryptKey == null || encryptKey.Length == 0)
                throw new InvalidOperationException("加密密钥不能为空！");

            // 空字符串处理
            if (string.IsNullOrEmpty(value))
            {
                binaryWriter.Write((byte)0);
                return;
            }

            // UTF8 编码
            byte[] rawBytes = System.Text.Encoding.UTF8.GetBytes(value);

            // 长度限制（byte.MaxValue = 255）
            if (rawBytes.Length > byte.MaxValue)
            {
                throw new InvalidOperationException($"字符串长度超出限制，最大支持 {byte.MaxValue} 字节，当前：{rawBytes.Length}");
            }
            
            Encryption.GetQuickSelfXorBytes(rawBytes, encryptKey);

            // 写入长度 + 数据
            binaryWriter.Write((byte)rawBytes.Length);
            binaryWriter.Write(rawBytes);
        }
        #endregion
    }
}