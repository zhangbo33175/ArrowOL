using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Honor.Runtime
{
    public static class GameExtension
    {
        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// 获取富文本的长度（剔除了所有tags）
        /// </summary>
        /// <param name="richText"></param>
        /// <returns></returns>
        public static int RichTextLength(this string richText)
        {
            int richTextLength = 0;
            bool insideTag = false;

            richText = richText.Replace("<br>", "-");

            foreach (char character in richText)
            {
                if (character == '<')
                {
                    insideTag = true;
                    continue;
                }
                else if (character == '>')
                {
                    insideTag = false;
                }
                else if (!insideTag)
                {
                    richTextLength++;
                }
            }

            return richTextLength;
        }

        /// <summary>
        /// 将文本中的每个单词首字母大写
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        }

        /// <summary>
        /// 移除掉文本中的额外空格
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveExtraSpaces(this string s)
        {
            return Regex.Replace(s, @"\s+", " ");
        }

        /// <summary>
        /// 采用 hh:mm:ss:SSS 字符串并将其转换为以秒为单位的浮点值
        /// </summary>
        /// <param name="timeInStringNotation"></param>
        /// <returns></returns>
        public static float TimeStringToFloat(this string timeInStringNotation)
        {
            if (timeInStringNotation.Length != 12)
            {
                throw new GameException("The time in the TimeStringToFloat method must be specified using a hh:mm:ss:SSS syntax");
            }

            string[] timeStringArray = timeInStringNotation.Split(new string[] { ":" }, StringSplitOptions.None);

            float startTime = 0f;
            float result;
            if (float.TryParse(timeStringArray[0], out result))
            {
                startTime += result * 3600f;
            }
            if (float.TryParse(timeStringArray[1], out result))
            {
                startTime += result * 60f;
            }
            if (float.TryParse(timeStringArray[2], out result))
            {
                startTime += result;
            }
            if (float.TryParse(timeStringArray[3], out result))
            {
                startTime += result / 1000f;
            }

            return startTime;
        }

        /// <summary>
        /// 获取字符串中的汉字个数
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int GetChineseNum(this string s)
        {
            int count = 0;
            Regex regex = new Regex(@"^[\u4E00-\u9FA5]{0,}$");

            for (int i = 0; i < s.Length; i++)
            {
                if (regex.IsMatch(s[i].ToString()))
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// 将浮点数（以秒表示）转换为字符串，可选择显示小时、分钟、秒和毫秒
        /// </summary>
        /// <param name="t"></param>
        /// <param name="displayHours"></param>
        /// <param name="displayMinutes"></param>
        /// <param name="displaySeconds"></param>
        /// <param name="displayMilliseconds"></param>
        /// <returns></returns>
        public static string FloatToTimeString(this float t, bool displayHours = false, bool displayMinutes = true, bool displaySeconds = true, bool displayMilliseconds = false)
        {
            int intTime = (int)t;
            int hours = intTime / 3600;
            int minutes = intTime / 60;
            int seconds = intTime % 60;
            int milliseconds = Mathf.FloorToInt((t * 1000) % 1000);

            if (displayHours && displayMinutes && displaySeconds && displayMilliseconds)
            {
                return string.Format("{0:00}:{1:00}:{2:00}.{3:D3}", hours, minutes, seconds, milliseconds);
            }
            if (!displayHours && displayMinutes && displaySeconds && displayMilliseconds)
            {
                return string.Format("{0:00}:{1:00}.{2:D3}", minutes, seconds, milliseconds);
            }
            if (!displayHours && !displayMinutes && displaySeconds && displayMilliseconds)
            {
                return string.Format("{0:D2}.{1:D3}", seconds, milliseconds);
            }
            if (!displayHours && !displayMinutes && displaySeconds && !displayMilliseconds)
            {
                return string.Format("{0:00}", seconds);
            }
            if (displayHours && displayMinutes && displaySeconds && !displayMilliseconds)
            {
                return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            }
            if (!displayHours && displayMinutes && displaySeconds && !displayMilliseconds)
            {
                return string.Format("{0:00}:{1:00}", minutes, seconds);
            }

            return null;
        }

    }

}


