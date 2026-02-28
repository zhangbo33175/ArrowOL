
#if UIEXTENSION_ENABLE
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI.Extensions;
using XLua;

namespace Honor.Runtime
{
    public partial class TextPicMixed : TextPic
    {
        /// <summary>
        /// 正则过滤器-别名格式
        /// </summary>
        private static readonly Regex s_PicRegexOnResDefFormat = new Regex(Txt.Format(@"{0}(?<fname>[\w]+){1}((([0-9]\d*)(.)([0-9]\d*))|([0-9]\d*)){2}((([0-9]\d*)(.)([0-9]\d*))|([0-9]\d*)){3}((([0-9]\d*)(.)([0-9]\d*))|([0-9]\d*)){4}", IDENTIFIERS_START, SEPARATOR, SEPARATOR, SEPARATOR, IDENTIFIERS_END));

        /// <summary>
        /// Lua组件
        /// </summary>
        private LuaComponent m_LuaComponent;

        /// <summary>
        /// 解析文本-别名格式
        /// </summary>
        public void ParseTextOnResDefFormat()
        {
            if(!Application.isPlaying)
            {
                return;
            }

            if (m_LuaComponent == null)
            {
                m_LuaComponent = SolarComponentsGroup.GetComponent<LuaComponent>();
                if (m_LuaComponent == null)
                {
                    Log.Fatal("Lua Component 无效。");
                    return;
                }
            }

            List<IconName> iconList = new List<IconName>();
            if (inspectorIconList != null) iconList.AddRange(inspectorIconList);
            string textTmp = m_Text;
            Match match = s_PicRegexOnResDefFormat.Match(textTmp);
            bool parseFinished = true;
            while (match.Success)
            {
                string originalContent = match.Groups[0].Value;
                string matchedContent = originalContent.Replace(IDENTIFIERS_START, string.Empty).Replace(IDENTIFIERS_END, string.Empty);
                string[] contents = matchedContent.Split(SEPARATOR);
                bool nextMatch = true;
                if (contents.Length == 4)
                {
                    LuaTable resDefLuaTable = m_LuaComponent.LuaGetResDefInfoEventDelegate(contents[0]);
                    if(resDefLuaTable != null)
                    {
                        resDefLuaTable.Get("ABPath", out string abPath);
                        resDefLuaTable.Get("AssetName", out string assetName);
                        float scaleXY = float.Parse(contents[1]);
                        float offsetX = float.Parse(contents[2]);
                        float offsetY = float.Parse(contents[3]);
                        nextMatch = CollectIcon(iconList, originalContent, abPath, assetName, scaleXY, offsetX, offsetY);
                    }
                }
                if (nextMatch)
                {
                    textTmp = textTmp.Substring(match.Index + match.Length);
                    match = s_PicRegexOnResDefFormat.Match(textTmp);
                    continue;
                }
                else
                {
                    parseFinished = false;
                    break;
                }
            }
            if (parseFinished)
            {
                inspectorIconList = iconList.ToArray();
            }
        }

    }

}
#endif

