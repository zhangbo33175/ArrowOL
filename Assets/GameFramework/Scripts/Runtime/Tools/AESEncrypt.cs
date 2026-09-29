/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AESEncrypt.cs
 * author:    云毅
 * created:   2026
 * descrip:   AES 加密解密工具类
 *            支持固定/随机密钥、Base64/字节数组、CBC/PKCS7
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Honor.Runtime
{
    /// <summary>
    /// AES 加密解密工具类（静态）
    /// 支持：
    /// 1. 静态加密（固定 Key/IV）
    /// 2. 动态加密（随机 Key/IV，自动拼入密文）
    /// 3. 字符串 Base64 加解密
    /// 4. 字节数组加解密
    /// 模式：CBC，填充：PKCS7
    /// </summary>
    public static class AESEncrypt
    {
        //=========================================================================
        // 常量 & 缓存
        //=========================================================================
        #region 常量 & 缓存

        /// <summary>
        /// 密钥/向量长度（固定 16 字节 = 128位）
        /// AES 要求必须为 16 字节
        /// </summary>
        private const int SecretBytesLength = 16;

        /// <summary>
        /// 临时字节缓存（复用减少 GC）
        /// </summary>
        private static readonly List<byte> m_sTempBytes = new List<byte>();

        #endregion

        //=========================================================================
        // 内部 - 对称加密对象构造
        //=========================================================================
        #region 内部 - Rijndael 构造

        /// <summary>
        /// 构造 CBC / PKCS7 模式的 Rijndael 加密对象
        /// </summary>
        /// <param name="key">密钥字节数组。</param>
        /// <param name="iv">向量字节数组。</param>
        /// <returns>已设置 Key/IV/模式/填充的加密对象。</returns>
        private static RijndaelManaged CreateCipher(byte[] key, byte[] iv)
        {
            return new RijndaelManaged
            {
                Key = key,
                IV = iv,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
            };
        }

        /// <summary>
        /// 按传入的固定密钥/向量（或为空时随机生成）解析 Key 与 IV
        /// </summary>
        /// <param name="fixedKey">外部固定密钥，为 null 表示动态随机生成</param>
        /// <param name="fixedIv">外部固定向量，为 null 表示动态随机生成</param>
        /// <param name="encoding">固定密钥/向量字符串的编码</param>
        /// <param name="keyBytes">输出密钥字节</param>
        /// <param name="ivBytes">输出向量字节</param>
        /// <param name="keyIsRandom">输出密钥是否为本次随机生成</param>
        /// <param name="ivIsRandom">输出向量是否为本次随机生成</param>
        private static void ResolveSecretsForEncoding(string fixedKey, string fixedIv, Encoding encoding,
            out byte[] keyBytes, out byte[] ivBytes, out bool keyIsRandom, out bool ivIsRandom)
        {
            keyIsRandom = fixedKey == null;
            ivIsRandom = fixedIv == null;

            keyBytes = keyIsRandom ? GetRandomSecretBytes() : encoding.GetBytes(fixedKey);
            ivBytes = ivIsRandom ? GetRandomSecretBytes() : encoding.GetBytes(fixedIv);
        }

        /// <summary>
        /// 动态加密时将随机 Key/IV 拼接到密文头部（静态模式不拼接）
        /// </summary>
        /// <param name="keyBytes">密钥字节</param>
        /// <param name="ivBytes">向量字节</param>
        /// <param name="keyIsRandom">密钥是否随机生成</param>
        /// <param name="ivIsRandom">向量是否随机生成</param>
        /// <param name="cipherBytes">已加密的密文</param>
        /// <returns>拼接后的完整输出字节</returns>
        private static byte[] AppendRandomSecrets(byte[] keyBytes, byte[] ivBytes, bool keyIsRandom, bool ivIsRandom, byte[] cipherBytes)
        {
            m_sTempBytes.Clear();
            if (keyIsRandom)
            {
                m_sTempBytes.AddRange(keyBytes);
            }
            if (ivIsRandom)
            {
                m_sTempBytes.AddRange(ivBytes);
            }
            m_sTempBytes.AddRange(cipherBytes);
            return m_sTempBytes.ToArray();
        }

        /// <summary>
        /// 从待解密字节流中解析 Key/IV（固定则直接转换，随机则从头部读取）
        /// </summary>
        /// <param name="raw">完整待解密字节流</param>
        /// <param name="fixedKey">固定密钥，为 null 表示从密文头部读取随机密钥</param>
        /// <param name="fixedIv">固定向量，为 null 表示从密文头部读取随机向量</param>
        /// <param name="encoding">固定密钥/向量字符串的编码</param>
        /// <param name="keyBytes">输出密钥字节</param>
        /// <param name="ivBytes">输出向量字节</param>
        /// <param name="secretHeaderLength">头部随机密钥/向量占用的总长度</param>
        private static void ParseSecretsFromHeader(byte[] raw, string fixedKey, string fixedIv, Encoding encoding,
            out byte[] keyBytes, out byte[] ivBytes, out int secretHeaderLength)
        {
            m_sTempBytes.Clear();
            m_sTempBytes.AddRange(raw);

            keyBytes = null;
            ivBytes = null;
            secretHeaderLength = 0;

            // 从密文提取随机 Key 或 使用固定 Key
            if (fixedKey != null)
            {
                keyBytes = encoding.GetBytes(fixedKey);
            }
            else
            {
                keyBytes = m_sTempBytes.GetRange(0, SecretBytesLength).ToArray();
                secretHeaderLength += SecretBytesLength;
            }

            // 从密文提取随机 IV 或 使用固定 IV（IV 在头部的偏移恒为一个 SecretBytesLength）
            if (fixedIv != null)
            {
                ivBytes = encoding.GetBytes(fixedIv);
            }
            else
            {
                ivBytes = m_sTempBytes.GetRange(SecretBytesLength, SecretBytesLength).ToArray();
                secretHeaderLength += SecretBytesLength;
            }
        }
        #endregion

        //=========================================================================
        // 公开接口 - 字符串加密解密
        //=========================================================================
        #region 字符串加密解密

        /// <summary>
        /// AES 加密字符串 → Base64 字符串
        /// 支持静态/动态加密模式
        /// </summary>
        /// <param name="content">明文</param>
        /// <param name="specialKey">固定密钥（null 则随机生成）</param>
        /// <param name="specialIv">固定向量（null 则随机生成）</param>
        /// <returns>Base64 密文</returns>
        public static string EncodeToBase64(string content, string specialKey = null, string specialIv = null)
        {
            ResolveSecretsForEncoding(specialKey, specialIv, Encoding.UTF8,
                out byte[] keyBytes, out byte[] ivBytes, out bool keyRandom, out bool ivRandom);

            // 加密主体
            byte[] plainBytes = Encoding.UTF8.GetBytes(content);
            using (RijndaelManaged cipher = CreateCipher(keyBytes, ivBytes))
            {
                ICryptoTransform encryptor = cipher.CreateEncryptor();
                byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                encryptor.Dispose();

                // 动态模式：将随机 Key/IV 写入密文头部
                byte[] output = AppendRandomSecrets(keyBytes, ivBytes, keyRandom, ivRandom, cipherBytes);
                return Convert.ToBase64String(output);
            }
        }

        /// <summary>
        /// AES 解密 Base64 字符串 → 明文字符串
        /// 自动识别静态/动态模式
        /// </summary>
        /// <param name="content">Base64 密文</param>
        /// <param name="specialKey">固定密钥（动态加密传 null）</param>
        /// <param name="specialIv">固定向量（动态加密传 null）</param>
        /// <returns>明文</returns>
        public static string DecodeFromBase64(string content, string specialKey = null, string specialIv = null)
        {
            try
            {
                byte[] raw = Convert.FromBase64String(content);
                if (raw.Length <= 0)
                {
                    return string.Empty;
                }

                ParseSecretsFromHeader(raw, specialKey, specialIv, Encoding.UTF8,
                    out byte[] keyBytes, out byte[] ivBytes, out int headerConsumed);

                // 提取真实密文
                byte[] cipherBytes = m_sTempBytes.GetRange(headerConsumed, m_sTempBytes.Count - headerConsumed).ToArray();

                using (RijndaelManaged cipher = CreateCipher(keyBytes, ivBytes))
                {
                    ICryptoTransform decryptor = cipher.CreateDecryptor();
                    byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                    decryptor.Dispose();
                    return Encoding.UTF8.GetString(plainBytes);
                }
            }
            catch (Exception exception)
            {
                Log.Error($"DecodeFromBase64 执行出错，error = {exception} content = {content} specialKey = {specialKey} specialIv = {specialIv}");
                return string.Empty;
            }
        }

        #endregion

        //=========================================================================
        // 公开接口 - 字节数组加密解密
        //=========================================================================
        #region 字节数组加密解密

        /// <summary>
        /// AES 加密字节数组 → 字节数组
        /// 支持静态/动态加密
        /// </summary>
        public static byte[] EncodeToBytes(byte[] content, string specialKey = null, string specialIv = null)
        {
            ResolveSecretsForEncoding(specialKey, specialIv, Encoding.ASCII,
                out byte[] keyBytes, out byte[] ivBytes, out bool keyRandom, out bool ivRandom);

            using (RijndaelManaged cipher = CreateCipher(keyBytes, ivBytes))
            {
                ICryptoTransform encryptor = cipher.CreateEncryptor();
                byte[] cipherBytes = encryptor.TransformFinalBlock(content, 0, content.Length);
                encryptor.Dispose();

                // 拼接 Key/IV（动态模式）
                return AppendRandomSecrets(keyBytes, ivBytes, keyRandom, ivRandom, cipherBytes);
            }
        }

        /// <summary>
        /// AES 解密字节数组 → 字节数组
        /// 自动识别静态/动态模式
        /// </summary>
        public static byte[] DecodeToBytes(byte[] content, string specialKey = null, string specialIv = null)
        {
            try
            {
                // 空字节直接返回空数组；null 交由下方 AddRange 抛出并被 catch 统一返回 null
                if (content != null && content.Length == 0)
                {
                    return Array.Empty<byte>();
                }

                ParseSecretsFromHeader(content, specialKey, specialIv, Encoding.ASCII,
                    out byte[] keyBytes, out byte[] ivBytes, out int headerConsumed);

                byte[] cipherBytes = m_sTempBytes.GetRange(headerConsumed, m_sTempBytes.Count - headerConsumed).ToArray();

                using (RijndaelManaged cipher = CreateCipher(keyBytes, ivBytes))
                {
                    ICryptoTransform decryptor = cipher.CreateDecryptor();
                    byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                    decryptor.Dispose();
                    return plainBytes;
                }
            }
            catch (Exception exception)
            {
                Log.Error($"DecodeToBytes 执行出错，error = {exception}");
                return null;
            }
        }

        #endregion

        //=========================================================================
        // 工具方法 - 随机密钥生成
        //=========================================================================
        #region 随机密钥生成

        /// <summary>
        /// 生成 16 字节随机密钥/向量
        /// </summary>
        public static byte[] GetRandomSecretBytes()
        {
            byte[] buffer = new byte[SecretBytesLength];
            for (int index = 0; index < buffer.Length; index++)
            {
                buffer[index] = (byte)UnityEngine.Random.Range(byte.MinValue, byte.MaxValue);
            }
            return buffer;
        }

        /// <summary>
        /// 生成 16 字符随机密钥字符串
        /// </summary>
        public static string GetRandomSecretString()
        {
            byte[] buffer = GetRandomSecretBytes();
            return Encoding.ASCII.GetString(buffer);
        }

        #endregion
    }
}
