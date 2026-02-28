namespace Honor.Runtime
{
    public sealed class LocalizationFontData
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="fontType">字体类型</param>
        /// <param name="mark">自定义标记</param>
        /// <param name="abPath">AB路径</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="fontSizeScaleRatio">相对于简体中文的字号缩放系数</param>
        public LocalizationFontData(string fontType, string mark, string abPath, string assetName,string customMaterialName, float fontSizeScaleRatio)
        {
            FontType = fontType;
            Mark = mark;
            ABPath = abPath;
            AssetName = assetName;
            CustomMaterialName = customMaterialName;
            FontSizeScaleRatio = fontSizeScaleRatio;
        }

        /// <summary>
        /// 是否相同
        /// </summary>
        /// <param name="other">被比较字体数据对象</param>
        /// <returns></returns>
        public bool Equals(LocalizationFontData other)
        {
            if (other == null) return false;
            return FontType == other.FontType && Mark == other.Mark && ABPath == other.ABPath && AssetName == other.AssetName && CustomMaterialName == other.CustomMaterialName && FontSizeScaleRatio == other.FontSizeScaleRatio;
        }

        /// <summary>
        /// 字体类型
        /// </summary>
        public string FontType;

        /// <summary>
        /// 自定义标记
        /// </summary>
        public string Mark;

        /// <summary>
        /// AB路径
        /// </summary>
        public string ABPath;

        /// <summary>
        /// 资源名称
        /// </summary>
        public string AssetName;

        /// <summary>
        /// 自定义字体材质资源名称
        /// </summary>
        public string CustomMaterialName;

        /// <summary>
        /// 相对于简体中文的字号缩放系数
        /// </summary>
        public float FontSizeScaleRatio; 
        
    }
}