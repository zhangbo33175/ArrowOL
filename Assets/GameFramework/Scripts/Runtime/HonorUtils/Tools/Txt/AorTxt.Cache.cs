/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorTxt.Cache.cs
 * author:    云毅
 * created:   2026
 * descrip:   字符串工具类 - 类型名称格式化扩展
 *            提供UnityEngine完整类型名拼接与缓存，减少GC
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 字符串工具类（类型名称格式化分部类）
    /// </summary>
    public partial class AorTxt
    {
        //=========================================================================
        // 类型名称缓存
        //=========================================================================
        #region 类型名称缓存

        /// <summary>
        /// 类型名称映射缓存字典
        /// 键：短名称（如：GameObject, Transform）
        /// 值：完整类型字符串（如：UnityEngine.GameObject）
        /// </summary>
        private static Dictionary<string, string> s_TypeNameToUnityEngine;

        #endregion

        //=========================================================================
        // 公共方法
        //=========================================================================
        #region 公共方法

        /// <summary>
        /// 根据短类型名获取 UnityEngine 下的完整类型字符串
        /// 带字典缓存，避免重复字符串拼接，提升性能并减少 GC
        /// </summary>
        /// <param name="typeName">UnityEngine 下的短类型名（例如：GameObject）</param>
        /// <returns>完整类型名（例如：UnityEngine.GameObject）</returns>
        public static string GetTypeStringByName(string typeName)
        {
            // 懒加载初始化字典
            s_TypeNameToUnityEngine ??= new Dictionary<string, string>();

            // 命中缓存直接返回
            if (s_TypeNameToUnityEngine.TryGetValue(typeName, out string fullName))
            {
                return fullName;
            }

            // 未命中则拼接并写入缓存
            fullName = "UnityEngine." + typeName;
            s_TypeNameToUnityEngine.Add(typeName, fullName);
            return fullName;
        }

        #endregion
    }
}
