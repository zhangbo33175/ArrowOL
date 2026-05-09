using System.Collections.Generic;

namespace Honor.Runtime
{
    public partial class AorTxt
    {
        /// <summary>
        /// 类型名称映射缓存字典
        /// 键：短名称（如：GameObject, Transform）
        /// 值：完整类型字符串（如：UnityEngine.GameObject）
        /// </summary>
        private static Dictionary<string, string> s_TypeNameToUnityEngine;

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
            
            // 尝试从缓存获取，不存在则创建并缓存
            if (!s_TypeNameToUnityEngine.TryGetValue(typeName, out var res))
            {
                res = $"UnityEngine.{typeName}";
                s_TypeNameToUnityEngine[typeName] = res;
            }

            return res;
        }
    }
}