/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorTextPicMixed.cs
 * author:    云毅
 * created:   2026   2025
 * descrip:   图文混排组件 | 基于 UIExtension TextPic | 支持动态精灵加载
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

#if UIEXTENSION_ENABLE
using UnityEngine.UI.Extensions;

namespace Honor.Runtime
{
    /// <summary>
    /// 图文混排组件（扩展 TextPic）
    /// 支持在文本中通过特殊标签动态插入图片，支持运行时动态加载精灵
    /// 依赖：UIExtension 插件
    /// </summary>
    [ExecuteInEditMode]
    public partial class AorTextPicMixed : TextPic
    {
        //=========================================================================
        // 常量定义
        //=========================================================================
        #region Const - 图文标签规则
        /// <summary>
        /// 文本中图片标识起始符
        /// 格式示例：#__
        /// </summary>
        private const string IDENTIFIERS_START = "#__";

        /// <summary>
        /// 文本中图片标识结束符
        /// 格式示例：__#
        /// </summary>
        private const string IDENTIFIERS_END = "__#";

        /// <summary>
        /// 文本中图片参数分隔符
        /// 用于分割图片路径、尺寸、偏移等参数
        /// </summary>
        private const char SEPARATOR = '&';
        #endregion

        //=========================================================================
        // 私有成员
        //=========================================================================
        #region Field - 精灵缓存
        /// <summary>
        /// 运行时动态加载的精灵缓存列表
        /// </summary>
        private List<Sprite> m_SpriteListOnPlaying = new List<Sprite>();
        #endregion

        //=========================================================================
        // 重写属性
        //=========================================================================
        #region Property - 文本内容
        /// <summary>
        /// 重写文本属性
        /// 设置时自动解析图文混排格式
        /// </summary>
        public override string text
        {
            get => base.text;
            set
            {
                base.text = value;
                // 解析资源定义格式
                ParseTextOnResDefFormat();
                // 解析详细图文格式
                ParseTextOnDetailFormat();
            }
        }
        #endregion

        //=========================================================================
        // 生命周期
        //=========================================================================
        #region MonoBehaviour - 初始化与销毁
        /// <summary>
        /// 初始化
        /// 调用基类初始化并解析图文格式
        /// </summary>
        private void Start()
        {
            base.Start();
            ParseTextOnResDefFormat();
            ParseTextOnDetailFormat();
        }

        /// <summary>
        /// 销毁时释放动态加载的精灵资源
        /// 防止资源泄漏
        /// </summary>
        private void OnDestroy()
        {
            base.OnDestroy();

            // 释放所有动态加载的精灵
            m_SpriteListOnPlaying.ForEach((sprite) =>
            {
                GameMainRoot.Asset.UnloadAsset(sprite);
            });

            m_SpriteListOnPlaying.Clear();
        }
        #endregion
    }
}

#else
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// 空实现占位类
    /// 当未启用 UIEXTENSION 时，降级为普通 Text 组件保证编译正常
    /// </summary>
    [ExecuteInEditMode]
    public class AorTextPicMixed : Text
    {
        // 空实现
    }
}
#endif