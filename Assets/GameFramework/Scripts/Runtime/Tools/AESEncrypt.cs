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
        /// <summary>
        /// 密钥/向量长度（固定 16 字节 = 128位）
        /// AES 要求必须为 16 字节
        /// </summary>
        private const int SecretBytesLength = 16;

        /// <summary>
        /// 临时字节缓存（复用减少 GC）
        /// </summary>
        private static List<byte> m_sTempBytes = new List<byte>();

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
            byte[] keyArray = null;
            byte[] ivArray = null;

            // 使用指定密钥 或 随机生成
            if (specialKey != null)
                keyArray = Encoding.UTF8.GetBytes(specialKey);
            else
                keyArray = GetRandomSecretBytes();

            // 使用指定向量 或 随机生成
            if (specialIv != null)
                ivArray = Encoding.UTF8.GetBytes(specialIv);
            else
                ivArray = GetRandomSecretBytes();

            // 加密主体
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(content);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            cTransform.Dispose();

            // 动态模式：将随机 Key/IV 写入密文头部
            m_sTempBytes.Clear();
            if (specialKey == null)
                m_sTempBytes.AddRange(keyArray);
            if (specialIv == null)
                m_sTempBytes.AddRange(ivArray);
            m_sTempBytes.AddRange(resultArray);

            return Convert.ToBase64String(m_sTempBytes.ToArray());
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
                byte[] bytes = Convert.FromBase64String(content);
                m_sTempBytes.Clear();
                m_sTempBytes.AddRange(bytes);

                if (m_sTempBytes.Count > 0)
                {
                    byte[] keyArray = null;
                    byte[] ivArray = null;
                    int tempSecretLength = 0;

                    // 从密文提取随机 Key 或 使用固定 Key
                    if (specialKey != null)
                        keyArray = Encoding.UTF8.GetBytes(specialKey);
                    else
                    {
                        keyArray = m_sTempBytes.GetRange(0, SecretBytesLength).ToArray();
                        tempSecretLength += SecretBytesLength;
                    }

                    // 从密文提取随机 IV 或 使用固定 IV
                    if (specialIv != null)
                        ivArray = Encoding.UTF8.GetBytes(specialIv);
                    else
                    {
                        ivArray = m_sTempBytes.GetRange(SecretBytesLength, SecretBytesLength).ToArray();
                        tempSecretLength += SecretBytesLength;
                    }

                    // 提取真实密文
                    byte[] encodedArray = m_sTempBytes.GetRange(tempSecretLength, m_sTempBytes.Count - tempSecretLength).ToArray();

                    // 解密
                    RijndaelManaged rDel = new RijndaelManaged();
                    rDel.Key = keyArray;
                    rDel.IV = ivArray;
                    rDel.Mode = CipherMode.CBC;
                    rDel.Padding = PaddingMode.PKCS7;

                    ICryptoTransform cTransform = rDel.CreateDecryptor();
                    byte[] resultArray = cTransform.TransformFinalBlock(encodedArray, 0, encodedArray.Length);
                    cTransform.Dispose();

                    return Encoding.UTF8.GetString(resultArray);
                }
            }
            catch (Exception e)
            {
                Log.Error($"DecodeFromBase64 执行出错，error = {e} content = {content} specialKey = {specialKey} specialIv = {specialIv}");
                return string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// AES 加密字节数组 → 字节数组
        /// 支持静态/动态加密
        /// </summary>
        public static byte[] EncodeToBytes(byte[] content, string specialKey = null, string specialIv = null)
        {
            byte[] keyArray = null;
            byte[] ivArray = null;

            if (specialKey != null)
                keyArray = Encoding.ASCII.GetBytes(specialKey);
            else
                keyArray = GetRandomSecretBytes();

            if (specialIv != null)
                ivArray = Encoding.ASCII.GetBytes(specialIv);
            else
                ivArray = GetRandomSecretBytes();

            // 加密
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(content, 0, content.Length);
            cTransform.Dispose();

            // 拼接 Key/IV（动态模式）
            m_sTempBytes.Clear();
            if (specialKey == null)
                m_sTempBytes.AddRange(keyArray);
            if (specialIv == null)
                m_sTempBytes.AddRange(ivArray);
            m_sTempBytes.AddRange(resultArray);

            return m_sTempBytes.ToArray();
        }

        /// <summary>
        /// AES 解密字节数组 → 字节数组
        /// 自动识别静态/动态模式
        /// </summary>
        public static byte[] DecodeToBytes(byte[] content, string specialKey = null, string specialIv = null)
        {
            try
            {
                m_sTempBytes.Clear();
                m_sTempBytes.AddRange(content);

                if (m_sTempBytes.Count > 0)
                {
                    byte[] keyArray = null;
                    byte[] ivArray = null;
                    int tempSecretLength = 0;

                    if (specialKey != null)
                        keyArray = Encoding.ASCII.GetBytes(specialKey);
                    else
                    {
                        keyArray = m_sTempBytes.GetRange(0, SecretBytesLength).ToArray();
                        tempSecretLength += SecretBytesLength;
                    }

                    if (specialIv != null)
                        ivArray = Encoding.ASCII.GetBytes(specialIv);
                    else
                    {
                        ivArray = m_sTempBytes.GetRange(SecretBytesLength, SecretBytesLength).ToArray();
                        tempSecretLength += SecretBytesLength;
                    }

                    byte[] encodedArray = m_sTempBytes.GetRange(tempSecretLength, m_sTempBytes.Count - tempSecretLength).ToArray();

                    // 解密
                    RijndaelManaged rDel = new RijndaelManaged();
                    rDel.Key = keyArray;
                    rDel.IV = ivArray;
                    rDel.Mode = CipherMode.CBC;
                    rDel.Padding = PaddingMode.PKCS7;

                    ICryptoTransform cTransform = rDel.CreateDecryptor();
                    byte[] resultArray = cTransform.TransformFinalBlock(encodedArray, 0, encodedArray.Length);
                    cTransform.Dispose();

                    return resultArray;
                }
            }
            catch (Exception e)
            {
                Log.Error($"DecodeToBytes 执行出错，error = {e}");
                return null;
            }
            return Array.Empty<byte>();
        }

        /// <summary>
        /// 生成 16 字节随机密钥/向量
        /// </summary>
        public static byte[] GetRandomSecretBytes()
        {
            byte[] iv = new byte[SecretBytesLength];
            for (int i = 0; i < iv.Length; i++)
            {
                iv[i] = (byte)UnityEngine.Random.Range(byte.MinValue, byte.MaxValue);
            }
            return iv;
        }

        /// <summary>
        /// 生成 16 字符随机密钥字符串
        /// </summary>
        public static string GetRandomSecretString()
        {
            byte[] iv = GetRandomSecretBytes();
            return Encoding.ASCII.GetString(iv);
        }
    }
}