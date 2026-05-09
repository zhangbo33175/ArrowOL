using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 通用字符串扩展工具类
    /// 提供首字母大写、富文本计算、时间格式化、汉字统计、空格清理等功能
    /// </summary>
    public static class GameExtension
    {
        /// <summary>
        /// 首字母大写
        /// </summary>
        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            return char.ToUpper(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// 获取富文本的真实长度（自动剔除 <color>、<b>、<size> 等标签）
        /// </summary>
        public static int RichTextLength(this string richText)
        {
            if (string.IsNullOrEmpty(richText))
                return 0;

            int length = 0;
            bool insideTag = false;

            // 替换换行标签为占位符
            richText = richText.Replace("<br>", "-");

            foreach (char c in richText)
            {
                if (c == '<')
                {
                    insideTag = true;
                }
                else if (c == '>')
                {
                    insideTag = false;
                }
                else if (!insideTag)
                {
                    length++;
                }
            }

            return length;
        }

        /// <summary>
        /// 将文本中的每个单词首字母大写
        /// </summary>
        public static string ToTitleCase(this string title)
        {
            if (string.IsNullOrEmpty(title))
                return string.Empty;

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        }

        /// <summary>
        /// 移除文本中多余空格（多个空格 → 一个空格）
        /// </summary>
        public static string RemoveExtraSpaces(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            return Regex.Replace(s, @"\s+", " ").Trim();
        }

        /// <summary>
        /// 将 hh:mm:ss:SSS 格式时间字符串 转为 秒数(float)
        /// </summary>
        public static float TimeStringToFloat(this string timeInStringNotation)
        {
            if (string.IsNullOrEmpty(timeInStringNotation) || timeInStringNotation.Length != 12)
            {
                throw new GameException("时间格式错误，必须使用 hh:mm:ss:SSS 格式");
            }

            string[] parts = timeInStringNotation.Split(':', StringSplitOptions.None);
            if (parts.Length != 4)
            {
                throw new GameException("时间格式分段错误，必须是 4 段：hh:mm:ss:SSS");
            }

            float total = 0;

            float.TryParse(parts[0], out float h);
            float.TryParse(parts[1], out float m);
            float.TryParse(parts[2], out float s);
            float.TryParse(parts[3], out float ms);

            total += h * 3600;
            total += m * 60;
            total += s;
            total += ms / 1000f;

            return total;
        }

        /// <summary>
        /// 统计字符串中的汉字数量
        /// </summary>
        public static int GetChineseNum(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return 0;

            int count = 0;
            Regex regex = new Regex(@"[\u4E00-\u9FA5]");

            foreach (char c in s)
            {
                if (regex.IsMatch(c.ToString()))
                    count++;
            }

            return count;
        }

        /// <summary>
        /// 将秒数(float) 转为格式化时间字符串
        /// </summary>
        public static string FloatToTimeString(
            this float t,
            bool displayHours = false,
            bool displayMinutes = true,
            bool displaySeconds = true,
            bool displayMilliseconds = false)
        {
            int totalSeconds = Mathf.FloorToInt(t);
            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;
            int milliseconds = Mathf.FloorToInt((t * 1000) % 1000);

            // 组合显示格式
            if (displayHours && displayMinutes && displaySeconds && displayMilliseconds)
                return $"{hours:00}:{minutes:00}:{seconds:00}.{milliseconds:D3}";

            if (!displayHours && displayMinutes && displaySeconds && displayMilliseconds)
                return $"{minutes:00}:{seconds:00}.{milliseconds:D3}";

            if (!displayHours && !displayMinutes && displaySeconds && displayMilliseconds)
                return $"{seconds:D2}.{milliseconds:D3}";

            if (!displayHours && !displayMinutes && displaySeconds && !displayMilliseconds)
                return $"{seconds:00}";

            if (displayHours && displayMinutes && displaySeconds && !displayMilliseconds)
                return $"{hours:00}:{minutes:00}:{seconds:00}";

            if (!displayHours && displayMinutes && displaySeconds && !displayMilliseconds)
                return $"{minutes:00}:{seconds:00}";

            return string.Empty;
        }
    }
}