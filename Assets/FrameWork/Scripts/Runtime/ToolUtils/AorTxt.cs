using System;
using System.Collections.Generic;
using System.Text;

namespace Honor.Runtime
{
    public static class AorTxt 
    {
        [ThreadStatic]
        private static StringBuilder m_CachedStringBuilder = null;
        /// <summary>
        /// typeName索引引擎的type字符串
        /// </summary>
        private static Dictionary<string, string> m_TypeNameToUnityEngine;

        /// <summary>
        /// 通过字符串获取Assembly.Type字符串
        /// </summary>
        public static string Get_Type_String_By_Name(string typeName)
        {
            m_TypeNameToUnityEngine ??= new Dictionary<string, string>();
            if (!m_TypeNameToUnityEngine.TryGetValue(typeName, out var res))
            {
                res = $"UnityEngine.{typeName}";
                m_TypeNameToUnityEngine[typeName] = res;
            }

            return res;
        }
        /// <summary>
        /// 获取格式化字符串
        /// </summary>
        /// <param name="format">字符串格式</param>
        /// <param name="arg0">字符串参数0</param>
        /// <returns>返回格式化后的字符串</returns>
        public static string Format(string format, object arg0)
        {
            if (format == null)
            {
                throw new Exception("格式无效。");
            }

            Check_Cached_String_Builder();
            m_CachedStringBuilder.Length = 0;
            m_CachedStringBuilder.AppendFormat(format, arg0);
            return m_CachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串
        /// </summary>
        /// <param name="format">字符串格式</param>
        /// <param name="arg0">字符串参数0</param>
        /// <param name="arg1">字符串参数1</param>
        /// <returns>返回格式化后的字符串</returns>
        public static string Format(string format, object arg0, object arg1)
        {
            if (format == null)
            {
                throw new Exception("格式无效。");
            }

            Check_Cached_String_Builder();
            m_CachedStringBuilder.Length = 0;
            m_CachedStringBuilder.AppendFormat(format, arg0, arg1);
            return m_CachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串
        /// </summary>
        /// <param name="format">字符串格式</param>
        /// <param name="arg0">字符串参数0</param>
        /// <param name="arg1">字符串参数1</param>
        /// <param name="arg2">字符串参数2</param>
        /// <returns>返回格式化后的字符串</returns>
        public static string Format(string format, object arg0, object arg1, object arg2)
        {
            if (format == null)
            {
                throw new Exception("格式无效。");
            }

            Check_Cached_String_Builder();
            m_CachedStringBuilder.Length = 0;
            m_CachedStringBuilder.AppendFormat(format, arg0, arg1, arg2);
            return m_CachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串
        /// </summary>
        /// <param name="format">字符串格式</param>
        /// <param name="args">字符串参数数组</param>
        /// <returns></returns>
        public static string Format(string format, params object[] args)
        {
            if (format == null)
            {
                throw new Exception("格式无效。");
            }

            if (args == null)
            {
                throw new Exception("参数无效。");
            }

            Check_Cached_String_Builder();
            m_CachedStringBuilder.Length = 0;
            m_CachedStringBuilder.AppendFormat(format, args);
            return m_CachedStringBuilder.ToString();
        }

        /// <summary>
        /// 检查缓存StringBuilder的初始化
        /// </summary>
        private static void Check_Cached_String_Builder()
        {
            if (m_CachedStringBuilder == null)
            {
                m_CachedStringBuilder = new StringBuilder(1024);
            }
        }
    }
}

