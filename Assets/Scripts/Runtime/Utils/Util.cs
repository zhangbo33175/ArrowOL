using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameLib
{
    public static class Util
    {
        /// <summary>
        /// 获取游戏窗口分辨率
        /// </summary>
        /// <returns></returns>
        public static Vector2 GameViewSize()
        {
            Vector2 size = Vector2.zero;

#if UNITY_EDITOR
            size = UnityEditor.Handles.GetMainGameViewSize();
#else
            size.x = Screen.width;
            size.y = Screen.height;
#endif

            return size;
        }

        /// <summary>
        /// 打印List数据日志
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string PrintAll<T>(this List<T> list)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('[');
            for (int i = 0; i < list.Count; i++)
            {
                stringBuilder.Append(list[i].ToString() + (i == (list.Count - 1) ? "" : ","));
            }

            stringBuilder.Append(']');
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 使用C#的正则匹配
        /// </summary>
        /// <param name="src"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static bool IsRegexMatch(string src, string regex)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(src, regex);
        }

        /// <summary>
        /// 替换标点符号为空字符串（造成的后果是每次输入变更都会带来一次字符串的重新分配，会多消耗一些内存
        /// \p{L} 匹配了所有 Unicode 字母，\p{N} 匹配了所有 Unicode 数字，\s 匹配了所有空白字符
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static string ReplaceSymbolInName(string origin)
        {
            return System.Text.RegularExpressions.Regex.Replace(origin, @"[^\p{L}\p{N}\s]", "");
        }

        /// <summary>
        /// 设置多点触控是否可用
        /// </summary>
        /// <param name="isEnable"></param>
        public static void SetMultiTouchEnabled(bool isEnable)
        {
            Input.multiTouchEnabled = isEnable;
        }
        
        /// <summary>
        /// 获取多点触控是否可用
        /// </summary>
        public static bool GetMultiTouchEnabled()
        {
            return Input.multiTouchEnabled;
        }
    }
}