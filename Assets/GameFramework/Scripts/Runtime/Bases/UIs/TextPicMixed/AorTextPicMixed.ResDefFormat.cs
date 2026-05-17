/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorTextPicMixed.ResDef.cs
 * author:    云毅
 * created:   2026
 * descrip:   图文混排 - 别名格式图片标签解析（partial）
 ***************************************************************/

#if UIEXTENSION_ENABLE
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI.Extensions;
using XLua;

namespace Honor.Runtime
{
    public partial class AorTextPicMixed
    {
        //=========================================================================
        // 正则表达式
        //=========================================================================
        #region Regex - 图片别名标签匹配
        /// <summary>
        /// 别名格式图片正则匹配表达式
        /// 匹配格式：#__图片别名&缩放&X偏移&Y偏移__#
        /// 通过别名从Lua配置表中获取真实资源路径
        /// </summary>
        private static readonly Regex s_PicRegexOnResDefFormat = new Regex(
            AorTxt.Format(
                @"{0}(?<fname>[\w]+){1}((([0-9]\d*)(.)([0-9]\d*))|([0-9]\d*)){2}((([0-9]\d*)(.)([0-9]\d*))|([0-9]\d*)){3}((([0-9]\d*)(.)([0-9]\d*))|([0-9]\d*)){4}",
                IDENTIFIERS_START,
                SEPARATOR,
                SEPARATOR,
                SEPARATOR,
                IDENTIFIERS_END
            )
        );
        #endregion

        //=========================================================================
        // 私有字段
        //=========================================================================
        #region Field - Lua 组件
        /// <summary>
        /// Lua 组件引用（用于读取资源配置表）
        /// </summary>
        private LuaComponent m_LuaComponent;
        #endregion

        //=========================================================================
        // 图片标签解析（别名格式）
        //=========================================================================
        #region Method - 别名格式解析
        /// <summary>
        /// 解析文本中的【别名格式】图片标签
        /// 格式规则：#__图片别名&缩放值&X偏移&Y偏移__#
        /// 通过Lua配置表将别名解析为真实AB包路径与资源名称
        /// 【注意】仅在运行模式下生效
        /// </summary>
        public void ParseTextOnResDefFormat()
        {
            // 编辑模式下不执行解析逻辑
            if (!Application.isPlaying)
                return;

            // 获取全局Lua组件
            if (m_LuaComponent == null)
            {
                m_LuaComponent = GameComponentsGroup.GetComponent<LuaComponent>();
                if (m_LuaComponent == null)
                {
                    Log.Fatal("Lua Component 无效。");
                    return;
                }
            }

            // 图标列表，合并编辑器预设图标
            List<IconName> iconList = new List<IconName>();
            if (inspectorIconList != null)
            {
                iconList.AddRange(inspectorIconList);
            }

            string textTmp = m_Text;
            Match match = s_PicRegexOnResDefFormat.Match(textTmp);
            bool parseFinished = true;

            // 循环匹配所有图片标签
            while (match.Success)
            {
                // 获取完整匹配的原始字符串
                string originalContent = match.Groups[0].Value;

                // 去除首尾标识符，获取内部参数
                string matchedContent = originalContent
                    .Replace(IDENTIFIERS_START, string.Empty)
                    .Replace(IDENTIFIERS_END, string.Empty);

                // 按分隔符拆分参数
                string[] contents = matchedContent.Split(SEPARATOR);
                bool nextMatch = true;

                // 参数格式：图片别名、缩放、偏移X、偏移Y
                if (contents.Length == 4)
                {
                    // 通过Lua委托，根据别名获取资源配置信息
                    LuaTable resDefLuaTable = m_LuaComponent.LuaGetResDefInfoEventDelegate(contents[0]);
                    if (resDefLuaTable != null)
                    {
                        // 从Lua表中读取AB路径和资源名称
                        resDefLuaTable.Get("ABPath", out string abPath);
                        resDefLuaTable.Get("AssetName", out string assetName);

                        // 解析缩放与偏移参数
                        float scaleXY = float.Parse(contents[1]);
                        float offsetX = float.Parse(contents[2]);
                        float offsetY = float.Parse(contents[3]);

                        // 收集图标信息
                        nextMatch = CollectIcon(iconList, originalContent, abPath, assetName, scaleXY, offsetX, offsetY);
                    }
                }

                // 继续匹配下一个标签
                if (nextMatch)
                {
                    textTmp = textTmp.Substring(match.Index + match.Length);
                    match = s_PicRegexOnResDefFormat.Match(textTmp);
                    continue;
                }
                else
                {
                    // 解析失败，终止循环
                    parseFinished = false;
                    break;
                }
            }

            // 解析全部完成后，赋值给基类图标列表
            if (parseFinished)
            {
                inspectorIconList = iconList.ToArray();
            }
        }
        #endregion
    }
}
#endif