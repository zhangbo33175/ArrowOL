using System;
using System.Text;

namespace Honor.Runtime
{
    /// <summary>
    /// 字符串格式化工具类（静态扩展）
    /// 功能：使用缓存的 StringBuilder 优化字符串拼接，减少 GC Alloc
    /// 特点：线程安全、高性能、无 GC
    /// </summary>
    public static partial class AorTxt
    {
        /// <summary>
        /// 线程静态缓存 StringBuilder
        /// 每个线程独立实例，避免多线程冲突，复用减少 GC
        /// </summary>
        [ThreadStatic]
        private static StringBuilder s_CachedStringBuilder = null;

        /// <summary>
        /// 格式化字符串（1个参数）
        /// </summary>
        /// <param name="format">格式字符串</param>
        /// <param name="arg0">参数</param>
        /// <returns>格式化后的字符串</returns>
        public static string Format(string format, object arg0)
        {
            if (format == null)
            {
                throw new GameException("格式无效。");
            }

            CheckCachedStringBuilder();
            s_CachedStringBuilder.Length = 0;
            s_CachedStringBuilder.AppendFormat(format, arg0);
            return s_CachedStringBuilder.ToString();
        }

        /// <summary>
        /// 格式化字符串（2个参数）
        /// </summary>
        public static string Format(string format, object arg0, object arg1)
        {
            if (format == null)
            {
                throw new GameException("格式无效。");
            }

            CheckCachedStringBuilder();
            s_CachedStringBuilder.Length = 0;
            s_CachedStringBuilder.AppendFormat(format, arg0, arg1);
            return s_CachedStringBuilder.ToString();
        }

        /// <summary>
        /// 格式化字符串（3个参数）
        /// </summary>
        public static string Format(string format, object arg0, object arg1, object arg2)
        {
            if (format == null)
            {
                throw new GameException("格式无效。");
            }

            CheckCachedStringBuilder();
            s_CachedStringBuilder.Length = 0;
            s_CachedStringBuilder.AppendFormat(format, arg0, arg1, arg2);
            return s_CachedStringBuilder.ToString();
        }

        /// <summary>
        /// 格式化字符串（多参数）
        /// </summary>
        public static string Format(string format, params object[] args)
        {
            if (format == null)
            {
                throw new GameException("格式无效。");
            }

            if (args == null)
            {
                throw new GameException("参数无效。");
            }

            CheckCachedStringBuilder();
            s_CachedStringBuilder.Length = 0;
            s_CachedStringBuilder.AppendFormat(format, args);
            return s_CachedStringBuilder.ToString();
        }

        /// <summary>
        /// 检查并初始化缓存 StringBuilder
        /// 默认容量 1024，避免频繁扩容
        /// </summary>
        private static void CheckCachedStringBuilder()
        {
            if (s_CachedStringBuilder == null)
            {
                s_CachedStringBuilder = new StringBuilder(1024);
            }
        }
    }
}