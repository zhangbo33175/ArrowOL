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
            byte[] retVal;
            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                retVal = md5.ComputeHash(file);
            }

            return retVal.ToHex("X2");
        }

        /// <summary>
        /// 计算字符串的 MD5 值（ASCII编码）
        /// </summary>
        /// <param name="input">明文字符串</param>
        /// <returns>32位大写MD5</returns>
        public static string GetMD5HashFromString(string input)
        {
            byte[] resultByte;
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputByte = Encoding.ASCII.GetBytes(input);
                resultByte = md5.ComputeHash(inputByte);
            }

            return resultByte.ToHex("X2");
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
                using (MD5 md5 = new MD5CryptoServiceProvider())
                {
                    return md5.ComputeHash(bytes).ToHex("X2");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return string.Empty;
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
            byte[] bytes = GetAndroidSignture();
            if (bytes != null)
            {
                var md5String = GetMD5HashFromBytes(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < md5String.Length; ++i) {
                    if (split != '\0' && i > 0 && i % 2 == 0)
                    {
                        sb.Append(split);
                    }
                    sb.Append(md5String[i]);
                }
                return sb.ToString();
            }
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
            var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var activity = player.GetStatic<AndroidJavaObject>("currentActivity");
            var PackageManager = new AndroidJavaClass("android.content.pm.PackageManager");
            var packageName = activity.Call<string>("getPackageName");
            var GET_SIGNATURES = PackageManager.GetStatic<int>("GET_SIGNATURES");
            var packageManager = activity.Call<AndroidJavaObject>("getPackageManager");
            var packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, GET_SIGNATURES);
            var signatures = packageInfo.Get<AndroidJavaObject[]>("signatures");
            
            if (signatures != null && signatures.Length > 0)
            {
                return signatures[0].Call<byte[]>("toByteArray");
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
            byte[] bytes = GetAndroidSignture();
            if (bytes != null)
            {
                return Convert.ToBase64String(bytes);
            }
#endif
            return string.Empty;
        }
        #endregion
    }
}