using System.Collections.Generic;

namespace Honor.Runtime
{
    public partial class LocalizationComponent
    {
        /// <summary>
        /// 默认支持语种类型集合
        /// <语种类型>
        /// </summary>
        private readonly List<GameDefinitions.Language> m_DefaultLanguages;

        /// <summary>
        /// 默认数据集合
        /// <语种类型,<字段名称, 文本内容>>
        /// </summary>
        private readonly Dictionary<GameDefinitions.Language, Dictionary<string, string>> m_DefaultDatas;

        /// <summary>
        /// 字体配置数据集合
        /// <语种类型, 字体数据集合>
        /// </summary>
        private readonly Dictionary<GameDefinitions.Language, List<LocalizationFontData>> m_FontDatas;
    }
}