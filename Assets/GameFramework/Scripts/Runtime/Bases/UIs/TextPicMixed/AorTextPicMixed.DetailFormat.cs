
#if UIEXTENSION_ENABLE
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI.Extensions;

namespace Honor.Runtime
{
    public partial class AorTextPicMixed : TextPic
    {
        /// <summary>
        /// 正则过滤器-详情格式
        /// </summary>
        private static readonly Regex s_PicRegexOnDetailFormat = new Regex(AorTxt.Format(@"{0}(([\w]+/)*[\w]+){1}(?<fname>[\w]+){2}((([0-9]\d*)(.)([0-9]\d*))|([0-9]\d*)){3}((([0-9]\d*)(.)([0-9]\d*))|([0-9]\d*)){4}((([0-9]\d*)(.)([0-9]\d*))|([0-9]\d*)){5}", IDENTIFIERS_START, SEPARATOR, SEPARATOR, SEPARATOR, SEPARATOR, IDENTIFIERS_END));

        /// <summary>
        /// 解析文本-详情格式
        /// </summary>
        public void ParseTextOnDetailFormat()
        {
            List<IconName> iconList = new List<IconName>();
            if (inspectorIconList != null) iconList.AddRange(inspectorIconList);
            string textTmp = m_Text;
            Match match = s_PicRegexOnDetailFormat.Match(textTmp);
            bool parseFinished = true;
            while(match.Success)
            {
                string originalContent = match.Groups[0].Value;
                string matchedContent = originalContent.Replace(IDENTIFIERS_START, string.Empty).Replace(IDENTIFIERS_END, string.Empty);
                string[] contents = matchedContent.Split(SEPARATOR);
                bool nextMatch = true;
                if(contents.Length == 5)
                {
                    string abPath = contents[0];
                    string assetName = contents[1];
                    float scaleXY = float.Parse(contents[2]);
                    float offsetX = float.Parse(contents[3]);
                    float offsetY = float.Parse(contents[4]);
                    nextMatch = CollectIcon(iconList, originalContent, abPath, assetName, scaleXY, offsetX, offsetY);
                }
                if(nextMatch)
                {
                    textTmp = textTmp.Substring(match.Index + match.Length);
                    match = s_PicRegexOnDetailFormat.Match(textTmp);
                    continue;
                }
                else
                {
                    parseFinished = false;
                    break;
                }
            }
            if(parseFinished)
            {
                inspectorIconList = iconList.ToArray();
            }
        }
    }

}

#endif
