/***************************************************************
 * (c) copyright 2026 - 2030, GameLib
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Util.cs
 * author:    云毅
 * created:   2026
 * descrip:   通用工具类
 *            提供视图尺寸、列表打印、正则过滤、多点触控控制等通用功能
 ***************************************************************/

using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 通用工具类
    /// 提供：游戏视图尺寸、列表打印、正则匹配、字符串清理、多点触控控制
    /// </summary>
    public static class Util
    {
        #region 屏幕/视图尺寸

        //=========================================================================
        // 屏幕/视图尺寸
        //=========================================================================
        /// <summary>
        /// 获取游戏窗口完整分辨率（全屏，包含上下标题UI）
        /// 编辑器下：取Game视图大小
        /// 运行时：取设备屏幕真实宽高
        /// </summary>
        public static Vector2 GameViewSize()
        {
            Vector2 size = Vector2.zero;

#if UNITY_EDITOR
            // 编辑器模式：获取 Game 窗口大小
            size = UnityEditor.Handles.GetMainGameViewSize();
#else
            // 运行时模式：获取设备屏幕大小
            size.x = Screen.width;
            size.y = Screen.height;
#endif
            return size;
        }

        /// <summary>
        /// 获取中间地图红框视口尺寸（剔除顶部标题、底部操作UI高度）
        /// 专门用于地图相机缩放、边界约束计算
        /// </summary>
        /// <param name="topHudPixelHeight">顶部标题UI像素高度</param>
        /// <param name="bottomHudPixelHeight">底部道具/提示UI像素高度</param>
        /// <returns>仅地图可视区域宽高，宽=全屏宽，高=总高-顶部高-底部高</returns>
        public static Vector2 GetMapViewportSize(float topHudPixelHeight, float bottomHudPixelHeight)
        {
            Vector2 fullSize = GameViewSize();
            float usableHeight = fullSize.y - topHudPixelHeight - bottomHudPixelHeight;
            // 防止数值异常小于0
            usableHeight = Mathf.Max(usableHeight, 1f);
            return new Vector2(fullSize.x, usableHeight);
        }

        /// <summary>
        /// 重载：直接从MapData组件读取HUD高度，一键获取地图视口尺寸
        /// 供Lua层调用，简化传参
        /// </summary>
        /// <param name="mapData">当前地图MapData组件实例</param>
        public static Vector2 GetMapViewportSize(MapData mapData)
        {
            if (mapData == null)
                return GameViewSize();

            return GetMapViewportSize(mapData.m_TopHudPixelHeight, mapData.m_BottomHudPixelHeight);
        }
        #endregion
        
        #region 调试/日志

        //=========================================================================
        // 调试/日志
        //=========================================================================
        /// <summary>
        /// 扩展方法：打印 List 所有内容，方便日志调试
        /// 输出格式：[元素1,元素2,元素3]
        /// </summary>
        public static string PrintAll<T>(this List<T> list)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('[');
            for (int i = 0; i < list.Count; i++)
            {
                stringBuilder.Append(list[i].ToString());
                // 最后一个元素不加逗号
                if (i != list.Count - 1)
                    stringBuilder.Append(',');
            }

            stringBuilder.Append(']');
            return stringBuilder.ToString();
        }

        #endregion

        #region 正则/字符串处理

        //=========================================================================
        // 正则/字符串处理
        //=========================================================================
        /// <summary>
        /// 正则表达式匹配
        /// </summary>
        /// <param name="src">源字符串</param>
        /// <param name="regex">正则表达式</param>
        public static bool IsRegexMatch(string src, string regex)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(src, regex);
        }

        /// <summary>
        /// 清理名称中的特殊符号，只保留：字母、数字、空格
        /// 用于输入框过滤非法字符
        /// </summary>
        /// <param name="origin">原始字符串</param>
        public static string ReplaceSymbolInName(string origin)
        {
            // [^\p{L}\p{N}\s]  =  非(字母/数字/空格) 的字符都替换为空
            return System.Text.RegularExpressions.Regex.Replace(origin, @"[^\p{L}\p{N}\s]", "");
        }

        #endregion

        #region 输入控制

        //=========================================================================
        // 输入控制
        //=========================================================================
        /// <summary>
        /// 设置是否启用多点触控
        /// </summary>
        public static void SetMultiTouchEnabled(bool isEnable)
        {
            Input.multiTouchEnabled = isEnable;
        }

        /// <summary>
        /// 获取当前是否启用多点触控
        /// </summary>
        public static bool GetMultiTouchEnabled()
        {
            return Input.multiTouchEnabled;
        }

        #endregion
    }
}