using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Honor.Runtime
{
    public static class MD5Handler
    {
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
            if(signatures != null && signatures.Length > 0)
            {
                return signatures[0].Call<byte[]>("toByteArray");
            }
#endif
            return null;
        }

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
    }
}