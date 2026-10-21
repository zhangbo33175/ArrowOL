/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LocalizationFontData.cs
 * author:    云毅
 * created:   2026
 * descrip:   多语言字体配置数据类
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 多语言字体配置数据
    /// 用于存储不同语言对应的字体、材质、路径、字号缩放等配置
    /// </summary>
    public sealed class LocalizationFontData
    {
        //=========================================================================
        // 公共字段
        //=========================================================================
        #region Public Fields
        /// <summary>
        /// 字体类型（自定义分类）
        /// </summary>
        public string FontType;

        /// <summary>
        /// 自定义标记（用于区分同类型多字体）
        /// </summary>
        public string Mark;

        /// <summary>
        /// 字体资源 AB 包路径
        /// </summary>
        public string ABPath;

        /// <summary>
        /// 字体资源名称
        /// </summary>
        public string AssetName;

        /// <summary>
        /// 自定义字体材质名称（无则为空）
        /// </summary>
        public string CustomMaterialName;

        /// <summary>
        /// 字号缩放系数（相对于中文）
        /// </summary>
        public float FontSizeScaleRatio;
        #endregion

        //=========================================================================
        // 构造函数
        //=========================================================================
        #region Constructor
        /// <summary>
        /// 构造字体数据
        /// </summary>
        /// <param name="fontType">字体类型（如：Default、Bold、Number）</param>
        /// <param name="mark">自定义标记，用于区分同类型字体</param>
        /// <param name="abPath">字体所在 AB 包路径</param>
        /// <param name="assetName">字体资源名称</param>
        /// <param name="customMaterialName">自定义材质名称（无则为空）</param>
        /// <param name="fontSizeScaleRatio">相对于中文的字号缩放比例</param>
        public LocalizationFontData(
            string fontType, 
            string mark, 
            string abPath, 
            string assetName, 
            string customMaterialName, 
            float fontSizeScaleRatio)
        {
            FontType            = fontType;
            Mark                = mark;
            ABPath              = abPath;
            AssetName           = assetName;
            CustomMaterialName  = customMaterialName;
            FontSizeScaleRatio  = fontSizeScaleRatio;
        }
        #endregion

        //=========================================================================
        // 公共方法
        //=========================================================================
        #region Public Methods
        /// <summary>
        /// 比较两个字体数据是否完全相同
        /// </summary>
        /// <param name="other">要比较的字体对象</param>
        /// <returns>是否相等</returns>
        public bool Equals(LocalizationFontData other)
        {
            if (other == null) 
                return false;

            return FontType == other.FontType 
                && Mark == other.Mark 
                && ABPath == other.ABPath 
                && AssetName == other.AssetName 
                && CustomMaterialName == other.CustomMaterialName 
                && FontSizeScaleRatio == other.FontSizeScaleRatio;
        }
        #endregion
    }
}