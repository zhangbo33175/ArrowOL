/***************************************************************
 * (c) copyright 2026 - 2030, GameLib
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  JsonHelper.cs
 * author:    云毅
 * created:   2026
 * descrip:   LitJson 封装工具类
 *            提供对象序列化/反序列化、格式化输出、GC优化
 ***************************************************************/

using System;
using System.Text;
using LitJson;

namespace GameLib
{
    /// <summary>
    /// JSON 工具类（基于 LitJson 封装）
    /// 提供：对象转JSON、JSON转对象、支持格式化输出
    /// </summary>
    public static class JsonHelper
    {
        #region 静态缓存（减少GC）
        //=========================================================================
        // 静态缓存（减少GC）
        //=========================================================================
        /// <summary>
        /// 静态字符串构建器，复用减少GC开销
        /// </summary>
        private static readonly StringBuilder m_StringBuilder = new StringBuilder();
        #endregion

        #region 序列化（对象 → JSON）
        //=========================================================================
        // 序列化（对象 → JSON）
        //=========================================================================
        /// <summary>
        /// 对象 转换为 JSON字符串
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="isPrettyPrint">是否格式化（美观输出）</param>
        /// <returns>JSON字符串</returns>
        public static string ToJson(object obj, bool isPrettyPrint = false)
        {
            // 不格式化 → 直接返回压缩JSON
            if (!isPrettyPrint)
            {
                return JsonMapper.ToJson(obj);
            }
            // 格式化 → 使用JsonWriter输出美观格式
            else
            {
                m_StringBuilder.Clear();
                JsonWriter writer = new JsonWriter(m_StringBuilder)
                {
                    PrettyPrint = true // 开启格式化（换行、缩进）
                };
                JsonMapper.ToJson(obj, writer);
                return m_StringBuilder.ToString();
            }
        }
        #endregion

        #region 反序列化（JSON → 对象）
        //=========================================================================
        // 反序列化（JSON → 对象）
        //=========================================================================
        /// <summary>
        /// JSON字符串 转换为 强类型对象
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <typeparam name="T">目标类型</typeparam>
        /// <returns>反序列化后的对象</returns>
        public static T ToObject<T>(string json)
        {
            return JsonMapper.ToObject<T>(json);
        }

        /// <summary>
        /// JSON字符串 转换为 指定类型对象
        /// </summary>
        /// <param name="objectType">目标类型</param>
        /// <param name="json">JSON字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static object ToObject(Type objectType, string json)
        {
            return JsonMapper.ToObject(objectType, json);
        }
        #endregion
    }
}