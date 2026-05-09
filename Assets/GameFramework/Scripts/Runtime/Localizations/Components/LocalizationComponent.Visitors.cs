using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class LocalizationComponent : GameComponent
    {
        /// <summary>
        /// 启动器组件
        /// </summary>
        private LauncherComponent m_LauncherComponent;

        /// <summary>
        /// 资源管理组件
        /// </summary>
        private AssetComponent m_AssetComponent;

        /// <summary>
        /// 持久化存储组件
        /// </summary>
        private PersistComponent m_PersistComponent;

        /// <summary>
        /// 本地化管理器
        /// </summary>
        private LocalizationManager m_LocalizationManager;

        /// <summary>
        /// 当前使用的语言类型
        /// </summary>
        [SerializeField]
        private GameDefinitions.Language m_Language;

        /// <summary>
        /// 获取当前语言类型
        /// </summary>
        public GameDefinitions.Language Language => m_Language;

        /// <summary>
        /// 获取当前语言名称（字符串）
        /// </summary>
        public string LanguageName => m_Language.ToString();

        /// <summary>
        /// 获取当前语言枚举索引
        /// </summary>
        public int LanguageIndex => (int)m_Language;

        /// <summary>
        /// 运行时上一次使用的语言（启动默认强制为简体中文）
        /// </summary>
        private GameDefinitions.Language m_RuntimeLastLanguage = GameDefinitions.Language.ChineseSimplified;

        /// <summary>
        /// 获取运行时上一次语言
        /// </summary>
        public GameDefinitions.Language RuntimeLastLanguage => m_RuntimeLastLanguage;

        /// <summary>
        /// 获取上一次语言名称（字符串）
        /// </summary>
        public string RuntimeLastLanguageName => m_RuntimeLastLanguage.ToString();

        /// <summary>
        /// 获取上一次语言枚举索引
        /// </summary>
        public int RuntimeLastLanguageIndex => (int)m_RuntimeLastLanguage;

        /// <summary>
        /// 获取系统当前语言，并映射为框架内部语言枚举
        /// </summary>
        public GameDefinitions.Language SystemLanguage
        {
            get
            {
                switch (Application.systemLanguage)
                {
                    case UnityEngine.SystemLanguage.Afrikaans: return GameDefinitions.Language.Afrikaans;
                    case UnityEngine.SystemLanguage.Arabic: return GameDefinitions.Language.Arabic;
                    case UnityEngine.SystemLanguage.Basque: return GameDefinitions.Language.Basque;
                    case UnityEngine.SystemLanguage.Belarusian: return GameDefinitions.Language.Belarusian;
                    case UnityEngine.SystemLanguage.Bulgarian: return GameDefinitions.Language.Bulgarian;
                    case UnityEngine.SystemLanguage.Catalan: return GameDefinitions.Language.Catalan;
                    case UnityEngine.SystemLanguage.Chinese: return GameDefinitions.Language.ChineseSimplified;
                    case UnityEngine.SystemLanguage.ChineseSimplified: return GameDefinitions.Language.ChineseSimplified;
                    case UnityEngine.SystemLanguage.ChineseTraditional: return GameDefinitions.Language.ChineseTraditional;
                    case UnityEngine.SystemLanguage.Czech: return GameDefinitions.Language.Czech;
                    case UnityEngine.SystemLanguage.Danish: return GameDefinitions.Language.Danish;
                    case UnityEngine.SystemLanguage.Dutch: return GameDefinitions.Language.Dutch;
                    case UnityEngine.SystemLanguage.English: return GameDefinitions.Language.English;
                    case UnityEngine.SystemLanguage.Estonian: return GameDefinitions.Language.Estonian;
                    case UnityEngine.SystemLanguage.Faroese: return GameDefinitions.Language.Faroese;
                    case UnityEngine.SystemLanguage.Finnish: return GameDefinitions.Language.Finnish;
                    case UnityEngine.SystemLanguage.French: return GameDefinitions.Language.French;
                    case UnityEngine.SystemLanguage.German: return GameDefinitions.Language.German;
                    case UnityEngine.SystemLanguage.Greek: return GameDefinitions.Language.Greek;
                    case UnityEngine.SystemLanguage.Hebrew: return GameDefinitions.Language.Hebrew;
                    case UnityEngine.SystemLanguage.Hungarian: return GameDefinitions.Language.Hungarian;
                    case UnityEngine.SystemLanguage.Icelandic: return GameDefinitions.Language.Icelandic;
                    case UnityEngine.SystemLanguage.Indonesian: return GameDefinitions.Language.Indonesian;
                    case UnityEngine.SystemLanguage.Italian: return GameDefinitions.Language.Italian;
                    case UnityEngine.SystemLanguage.Japanese: return GameDefinitions.Language.Japanese;
                    case UnityEngine.SystemLanguage.Korean: return GameDefinitions.Language.Korean;
                    case UnityEngine.SystemLanguage.Latvian: return GameDefinitions.Language.Latvian;
                    case UnityEngine.SystemLanguage.Lithuanian: return GameDefinitions.Language.Lithuanian;
                    case UnityEngine.SystemLanguage.Norwegian: return GameDefinitions.Language.Norwegian;
                    case UnityEngine.SystemLanguage.Polish: return GameDefinitions.Language.Polish;
                    case UnityEngine.SystemLanguage.Portuguese: return GameDefinitions.Language.PortuguesePortugal;
                    case UnityEngine.SystemLanguage.Romanian: return GameDefinitions.Language.Romanian;
                    case UnityEngine.SystemLanguage.Russian: return GameDefinitions.Language.Russian;
                    case UnityEngine.SystemLanguage.SerboCroatian: return GameDefinitions.Language.SerboCroatian;
                    case UnityEngine.SystemLanguage.Slovak: return GameDefinitions.Language.Slovak;
                    case UnityEngine.SystemLanguage.Slovenian: return GameDefinitions.Language.Slovenian;
                    case UnityEngine.SystemLanguage.Spanish: return GameDefinitions.Language.Spanish;
                    case UnityEngine.SystemLanguage.Swedish: return GameDefinitions.Language.Swedish;
                    case UnityEngine.SystemLanguage.Thai: return GameDefinitions.Language.Thai;
                    case UnityEngine.SystemLanguage.Turkish: return GameDefinitions.Language.Turkish;
                    case UnityEngine.SystemLanguage.Ukrainian: return GameDefinitions.Language.Ukrainian;
                    case UnityEngine.SystemLanguage.Unknown: return GameDefinitions.Language.Unspecified;
                    case UnityEngine.SystemLanguage.Vietnamese: return GameDefinitions.Language.Vietnamese;
                    default: return GameDefinitions.Language.Unspecified;
                }
            }
        }

        /// <summary>
        /// 获取系统语言名称（字符串）
        /// </summary>
        public string SystemLanguageName => SystemLanguage.ToString();

        /// <summary>
        /// 获取系统语言枚举索引
        /// </summary>
        public int SystemLanguageIndex => (int)SystemLanguage;

        /// <summary>
        /// 是否开启字体自动适配
        /// </summary>
        [SerializeField]
        private bool m_AutoFontAdapt;

        /// <summary>
        /// 获取字体自动适配开关状态
        /// </summary>
        public bool AutoFontAdapt => m_AutoFontAdapt;
    }
}