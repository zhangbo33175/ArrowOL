using System.Collections.Generic;

namespace Honor.Runtime
{
    public partial class AorTxt
    {
        /// <summary>
        /// typeName索引引擎的type字符串
        /// </summary>
        private static Dictionary<string, string> s_TypeNameToUnityEngine;

        /// <summary>
        /// 通过字符串获取Assembly.Type字符串
        /// </summary>
        public static string GetTypeStringByName(string typeName)
        {
            s_TypeNameToUnityEngine ??= new Dictionary<string, string>();
            if (!s_TypeNameToUnityEngine.TryGetValue(typeName, out var res))
            {
                res = $"UnityEngine.{typeName}";
                s_TypeNameToUnityEngine[typeName] = res;
            }

            return res;
        }
    }
}