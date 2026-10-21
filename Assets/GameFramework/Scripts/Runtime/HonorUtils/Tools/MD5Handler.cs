/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MD5Handler.cs
 * author:    云毅
 * created:   2026
 * descrip:   MD5加密工具类，支持文件/字符串/字节/安卓签名校验
 ***************************************************************/
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Honor.Runtime
{
    //=========================================================================
    // MD5 加密工具类
    //=========================================================================
    /// <summary>
    /// MD5 加密工具类
    /// 提供：文件MD5、字符串MD5、字节数组MD5、Android签名MD5/Base64获取
    /// 用于热更校验、资源校验、安全校验
    /// </summary>
    public static class MD5Handler
    {
        #region 基础MD5计算
        /// <summary>
        /// 计算文件的 MD5 值（热更/资源校验专用）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>32位大写MD5字符串</returns>
        public static string FileMD5(string filePath)
        {
            byte[] digest;
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                using (MD5 hasher = new MD5CryptoServiceProvider())
                {
                    digest = hasher.ComputeHash(stream);
                }
            }

            return digest.ToHex("X2");
        }

        /// <summary>
        /// 计算字符串的 MD5 值（ASCII编码）
        /// </summary>
        /// <param name="input">明文字符串</param>
        /// <returns>32位大写MD5</returns>
        public static string GetMD5HashFromString(string input)
        {
            byte[] source = Encoding.ASCII.GetBytes(input);
            return ComputeDigestText(source);
        }

        /// <summary>
        /// 计算字节数组的 MD5 值
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>32位大写MD5</returns>
        public static string GetMD5HashFromBytes(byte[] bytes)
        {
            try
            {
                using (MD5 hasher = new MD5CryptoServiceProvider())
                {
                    return hasher.ComputeHash(bytes).ToHex("X2");
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return string.Empty;
        }

        /// <summary>
        /// 对原始字节计算 MD5 摘要并格式化为十六进制文本
        /// </summary>
        /// <param name="source">原始字节数据</param>
        /// <returns>32位大写十六进制摘要</returns>
        private static string ComputeDigestText(byte[] source)
        {
            using (MD5 hasher = MD5.Create())
            {
                byte[] digest = hasher.ComputeHash(source);
                return digest.ToHex("X2");
            }
        }
        #endregion

        #region 安卓签名校验
        /// <summary>
        /// 获取 Android 包签名的 MD5（可带分隔符，用于校验签名）
        /// </summary>
        /// <param name="split">分隔符，如 ':'</param>
        /// <returns>签名MD5字符串</returns>
        public static string GetAndroidSignatureMD5Hash(char split = '\0')
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            byte[] signatureBytes = GetAndroidSignture();
            if (signatureBytes == null)
            {
                return null;
            }

            string digest = GetMD5HashFromBytes(signatureBytes);
            if (split == '\0')
            {
                return digest;
            }

            StringBuilder builder = new StringBuilder(digest.Length + digest.Length / 2);
            for (int cursor = 0; cursor < digest.Length; ++cursor)
            {
                if (cursor > 0 && cursor % 2 == 0)
                {
                    builder.Append(split);
                }
                builder.Append(digest[cursor]);
            }
            return builder.ToString();
#endif
            return null;
        }

        /// <summary>
        /// 获取 Android 安装包签名原始字节数组
        /// </summary>
        /// <returns>签名字节</returns>
        public static byte[] GetAndroidSignture()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var packageManagerClass = new AndroidJavaClass("android.content.pm.PackageManager");
            var pkgName = currentActivity.Call<string>("getPackageName");
            var signaturesFlag = packageManagerClass.GetStatic<int>("GET_SIGNATURES");
            var pkgManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            var pkgInfo = pkgManager.Call<AndroidJavaObject>("getPackageInfo", pkgName, signaturesFlag);
            var sigEntries = pkgInfo.Get<AndroidJavaObject[]>("signatures");

            if (sigEntries != null && sigEntries.Length > 0)
            {
                return sigEntries[0].Call<byte[]>("toByteArray");
            }
#endif
            return null;
        }

        /// <summary>
        /// 获取 Android 签名的 Base64 字符串（防篡改校验）
        /// </summary>
        /// <returns>Base64串</returns>
        public static string GetAndroidSigntureBase64Value()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            byte[] signatureBytes = GetAndroidSignture();
            if (signatureBytes != null)
            {
                return Convert.ToBase64String(signatureBytes);
            }
#endif
            return string.Empty;
        }
        #endregion
    }
}
