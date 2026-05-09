#if UIEXTENSION_ENABLE
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI.Extensions;

namespace Honor.Runtime
{
    public partial class AorTextPicMixed : TextPic
    {
        /// <summary>
        /// 详情格式图片正则匹配表达式
        /// 匹配格式：#__资源路径&图片名&缩放&X偏移&Y偏移__#
        /// 自动提取图片名称、路径、尺寸、偏移等参数
        /// </summary>
        private static readonly Regex s_PicRegexOnDetailFormat = new Regex(
            AorTxt.Format(
                @"{0}(([\w]+/)*[\w]+){1}(?<fname>[\w]+){2}((([0-9]\d*)(.)([0-9]\d*))|([0-9]\d*)){3}((([0-9]\d*)(.)([0-9]\d*))|([0-9]\d*)){4}((([0-9]\d*)(.)([0-9]\d*))|([0-9]\d*)){5}",
                IDENTIFIERS_START, 
                SEPARATOR, 
                SEPARATOR, 
                SEPARATOR, 
                SEPARATOR, 
                IDENTIFIERS_END
            )
        );

        /// <summary>
        /// 解析文本中的【详情格式】图片标签
        /// 格式规则：#__AB包路径&图片名称&缩放值&X偏移&Y偏移__#
        /// 解析成功后将图片信息存入图标列表，替换文本显示
        /// </summary>
        public void ParseTextOnDetailFormat()
        {
            // 图标列表，先合并编辑器预设的图标数据
            List<IconName> iconList = new List<IconName>();
            if (inspectorIconList != null)
                iconList.AddRange(inspectorIconList);

            string textTmp = m_Text;
            Match match = s_PicRegexOnDetailFormat.Match(textTmp);
            bool parseFinished = true;

            // 循环匹配所有图片标签
            while (match.Success)
            {
                // 获取完整匹配的原始字符串
                string originalContent = match.Groups[0].Value;

                // 去掉首尾标识符，获取内部内容
                string matchedContent = originalContent
                    .Replace(IDENTIFIERS_START, string.Empty)
                    .Replace(IDENTIFIERS_END, string.Empty);

                // 按分隔符拆分参数
                string[] contents = matchedContent.Split(SEPARATOR);
                bool nextMatch = true;

                // 参数格式：路径、名称、缩放、偏移X、偏移Y
                if (contents.Length == 5)
                {
                    string abPath = contents[0];
                    string assetName = contents[1];
                    float scaleXY = float.Parse(contents[2]);
                    float offsetX = float.Parse(contents[3]);
                    float offsetY = float.Parse(contents[4]);

                    // 收集图标信息到列表
                    nextMatch = CollectIcon(iconList, originalContent, abPath, assetName, scaleXY, offsetX, offsetY);
                }

                // 继续匹配下一个标签
                if (nextMatch)
                {
                    textTmp = textTmp.Substring(match.Index + match.Length);
                    match = s_PicRegexOnDetailFormat.Match(textTmp);
                    continue;
                }
                else
                {
                    // 解析失败，终止循环
                    parseFinished = false;
                    break;
                }
            }

            // 全部解析完成后，赋值给基类图标列表
            if (parseFinished)
            {
                inspectorIconList = iconList.ToArray();
            }
        }
    }
}
#endif