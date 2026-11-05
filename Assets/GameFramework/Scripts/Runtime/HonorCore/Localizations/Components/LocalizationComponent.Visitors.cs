/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LocalizationComponent.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   多语言本地化组件 - 字段与属性（partial）
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 多语言本地化组件 - 字段与属性部分
    /// </summary>
    public sealed partial class LocalizationComponent 
    {
        //=========================================================================
        // 组件引用
        //=========================================================================
        #region Component References
        /// <summary>
        /// 本地化管理器
        /// </summary>
        private LocalizationManager m_LocalizationManager;
        #endregion

        //=========================================================================
        // 序列化字段
        //=========================================================================
        #region Serialized Fields
        /// <summary>
        /// 当前使用的语言类型
        /// </summary>
        [SerializeField]
        private HonorDefinitions.Language m_Language;

        /// <summary>
        /// 是否开启字体自动适配
        /// </summary>
        [SerializeField]
        private bool m_AutoFontAdapt;
        #endregion

        //=========================================================================
        // 私有字段
        //=========================================================================
        #region Private Fields
        /// <summary>
        /// 运行时上一次使用的语言（启动默认强制为简体中文）
        /// </summary>
        private HonorDefinitions.Language m_RuntimeLastLanguage = HonorDefinitions.Language.ChineseSimplified;
        #endregion

        //=========================================================================
        // 公共属性
        //=========================================================================
        #region Public Properties
        /// <summary>
        /// 获取当前语言类型
        /// </summary>
        public HonorDefinitions.Language Language => m_Language;

        /// <summary>
        /// 获取当前语言名称（字符串）
        /// </summary>
        public string LanguageName => m_Language.ToString();

        /// <summary>
        /// 获取当前语言枚举索引
        /// </summary>
        public int LanguageIndex => (int)m_Language;

        /// <summary>
        /// 获取运行时上一次语言
        /// </summary>
        public HonorDefinitions.Language RuntimeLastLanguage => m_RuntimeLastLanguage;

        /// <summary>
        /// 获取上一次语言名称（字符串）
        /// </summary>
        public string RuntimeLastLanguageName => m_RuntimeLastLanguage.ToString();

        /// <summary>
        /// 获取上一次语言枚举索引
        /// </summary>
        public int RuntimeLastLanguageIndex => (int)m_RuntimeLastLanguage;

        /// <summary>
        /// 获取字体自动适配开关状态
        /// </summary>
        public bool AutoFontAdapt => m_AutoFontAdapt;

        /// <summary>
        /// 获取系统语言名称（字符串）
        /// </summary>
        public string SystemLanguageName => SystemLanguage.ToString();

        /// <summary>
        /// 获取系统语言枚举索引
        /// </summary>
        public int SystemLanguageIndex => (int)SystemLanguage;
        #endregion

        //=========================================================================
        // 系统语言映射
        //=========================================================================
        #region System Language Mapping
        /// <summary>
        /// 获取系统当前语言，并映射为框架内部语言枚举
        /// </summary>
        public HonorDefinitions.Language SystemLanguage
        {
            get
            {
                switch (Application.systemLanguage)
                {
                    case UnityEngine.SystemLanguage.Afrikaans:           return HonorDefinitions.Language.Afrikaans;
                    case UnityEngine.SystemLanguage.Arabic:              return HonorDefinitions.Language.Arabic;
                    case UnityEngine.SystemLanguage.Basque:              return HonorDefinitions.Language.Basque;
                    case UnityEngine.SystemLanguage.Belarusian:          return HonorDefinitions.Language.Belarusian;
                    case UnityEngine.SystemLanguage.Bulgarian:           return HonorDefinitions.Language.Bulgarian;
                    case UnityEngine.SystemLanguage.Catalan:             return HonorDefinitions.Language.Catalan;
                    case UnityEngine.SystemLanguage.Chinese:             return HonorDefinitions.Language.ChineseSimplified;
                    case UnityEngine.SystemLanguage.ChineseSimplified:   return HonorDefinitions.Language.ChineseSimplified;
                    case UnityEngine.SystemLanguage.ChineseTraditional:  return HonorDefinitions.Language.ChineseTraditional;
                    case UnityEngine.SystemLanguage.Czech:               return HonorDefinitions.Language.Czech;
                    case UnityEngine.SystemLanguage.Danish:              return HonorDefinitions.Language.Danish;
                    case UnityEngine.SystemLanguage.Dutch:               return HonorDefinitions.Language.Dutch;
                    case UnityEngine.SystemLanguage.English:             return HonorDefinitions.Language.English;
                    case UnityEngine.SystemLanguage.Estonian:            return HonorDefinitions.Language.Estonian;
                    case UnityEngine.SystemLanguage.Faroese:             return HonorDefinitions.Language.Faroese;
                    case UnityEngine.SystemLanguage.Finnish:            return HonorDefinitions.Language.Finnish;
                    case UnityEngine.SystemLanguage.French:             return HonorDefinitions.Language.French;
                    case UnityEngine.SystemLanguage.German:              return HonorDefinitions.Language.German;
                    case UnityEngine.SystemLanguage.Greek:               return HonorDefinitions.Language.Greek;
                    case UnityEngine.SystemLanguage.Hebrew:              return HonorDefinitions.Language.Hebrew;
                    case UnityEngine.SystemLanguage.Hungarian:          return HonorDefinitions.Language.Hungarian;
                    case UnityEngine.SystemLanguage.Icelandic:          return HonorDefinitions.Language.Icelandic;
                    case UnityEngine.SystemLanguage.Indonesian:          return HonorDefinitions.Language.Indonesian;
                    case UnityEngine.SystemLanguage.Italian:             return HonorDefinitions.Language.Italian;
                    case UnityEngine.SystemLanguage.Japanese:            return HonorDefinitions.Language.Japanese;
                    case UnityEngine.SystemLanguage.Korean:              return HonorDefinitions.Language.Korean;
                    case UnityEngine.SystemLanguage.Latvian:             return HonorDefinitions.Language.Latvian;
                    case UnityEngine.SystemLanguage.Lithuanian:          return HonorDefinitions.Language.Lithuanian;
                    case UnityEngine.SystemLanguage.Norwegian:           return HonorDefinitions.Language.Norwegian;
                    case UnityEngine.SystemLanguage.Polish:              return HonorDefinitions.Language.Polish;
                    case UnityEngine.SystemLanguage.Portuguese:          return HonorDefinitions.Language.PortuguesePortugal;
                    case UnityEngine.SystemLanguage.Romanian:            return HonorDefinitions.Language.Romanian;
                    case UnityEngine.SystemLanguage.Russian:             return HonorDefinitions.Language.Russian;
                    case UnityEngine.SystemLanguage.SerboCroatian:       return HonorDefinitions.Language.SerboCroatian;
                    case UnityEngine.SystemLanguage.Slovak:              return HonorDefinitions.Language.Slovak;
                    case UnityEngine.SystemLanguage.Slovenian:          return HonorDefinitions.Language.Slovenian;
                    case UnityEngine.SystemLanguage.Spanish:             return HonorDefinitions.Language.Spanish;
                    case UnityEngine.SystemLanguage.Swedish:             return HonorDefinitions.Language.Swedish;
                    case UnityEngine.SystemLanguage.Thai:                return HonorDefinitions.Language.Thai;
                    case UnityEngine.SystemLanguage.Turkish:             return HonorDefinitions.Language.Turkish;
                    case UnityEngine.SystemLanguage.Ukrainian:           return HonorDefinitions.Language.Ukrainian;
                    case UnityEngine.SystemLanguage.Unknown:             return HonorDefinitions.Language.Unspecified;
                    case UnityEngine.SystemLanguage.Vietnamese:          return HonorDefinitions.Language.Vietnamese;
                    default:                                            return HonorDefinitions.Language.Unspecified;
                }
            }
        }
        #endregion
    }
}