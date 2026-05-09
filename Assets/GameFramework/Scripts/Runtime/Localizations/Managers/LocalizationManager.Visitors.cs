using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class LocalizationManager
    {
        /// <summary>
        /// 框架启动组件
        /// </summary>
        private LauncherComponent m_LauncherComponent;

        /// <summary>
        /// 资源加载组件
        /// </summary>
        private AssetComponent m_AssetComponent;

        /// <summary>
        /// 支持的语言列表
        /// </summary>
        private readonly List<GameDefinitions.Language> m_DefaultLanguages;

        /// <summary>
        /// 多语言文本数据
        /// 结构：语言类型 → (Key → 文本内容)
        /// </summary>
        private readonly Dictionary<GameDefinitions.Language, Dictionary<string, string>> m_DefaultDatas;

        /// <summary>
        /// 多语言字体配置
        /// 结构：语言类型 → 字体数据列表
        /// </summary>
        private readonly Dictionary<GameDefinitions.Language, List<LocalizationFontData>> m_FontDatas;
    }
}