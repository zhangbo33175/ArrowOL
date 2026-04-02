using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class LocalizationComponent : GameComponent
    {
        protected override void Awake()
        {
            base.Awake();

            m_LauncherComponent = GameComponentsGroup.GetComponent<LauncherComponent>();
            if (m_LauncherComponent == null)
            {
                Log.Fatal("Launcher Component 无效。");
                return;
            }

            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("Asset Component 无效。");
                return;
            }

            m_PersistComponent = GameComponentsGroup.GetComponent<PersistComponent>();
            if (m_PersistComponent == null)
            {
                Log.Fatal("Persist Component 无效。");
                return;
            }

            m_LocalizationManager = new LocalizationManager();
            if (m_LocalizationManager == null)
            {
                Log.Fatal("Localization Manager 无效。");
                return;
            }

        }

        private void Start()
        {

        }

        /// <summary>
        /// 初始化当前语言类型
        /// </summary>
        public void InitCurLanguage()
        {
            /* 如果是编辑器资源模式，并且有设定好的语言类型，则直接使用设置好的语言，否则使用系统当前语言
               如果是非编辑器资源模式，并且持久化存档中存在设定好的语言类型，则直接使用设置好的语言，否则使用系统语言，其次是通用语言-英语 */
            if (m_LauncherComponent.EditorResourceMode)
            {
                if (m_LauncherComponent.EditorLanguage == GameDefinitions.Language.Unspecified)
                {
                    Log.Info("编辑器资源模式下没有指定的语言类型，则默认使用当前系统语言 '{0}'。", SystemLanguageName);
                    m_Language = SystemLanguage;
                }
                else
                {
                    Log.Info("编辑器资源模式下有指定的语言类型 '{0}'，将不再使用其他语言类型。", m_LauncherComponent.EditorLanguage.ToString());
                    m_Language = m_LauncherComponent.EditorLanguage;
                }
            }
            else
            {
                if (m_PersistComponent.HasItem(GameConstants.Persist.Common.WayType, GameConstants.Persist.Common.ClassifyName, GameConstants.Persist.Common.ItemKey.Language))
                {
                    m_Language = (GameDefinitions.Language)Enum.Parse(typeof(GameDefinitions.Language), m_PersistComponent.GetString(GameConstants.Persist.Common.WayType, GameConstants.Persist.Common.ClassifyName, GameConstants.Persist.Common.ItemKey.Language));
                    Log.Info("非编辑器资源模式下将使用持久化存档数据中的语言类型 '{0}'。", SystemLanguageName);
                }
                else
                {
                    if (HasDefaultLanguage(SystemLanguage))
                    {
                        m_Language = SystemLanguage;
                    }
                    else
                    {
                        m_Language = GameDefinitions.Language.English;
                    }
                    Log.Info("非编辑器资源模式下持久化存档数据中的语言类型不存在，默认使用 '{0}' 类型。", m_Language.ToString());
                }
            }
        }

        /// <summary>
        /// 设置语言类型
        /// </summary>
        /// <param name="language">语言类型</param>
        /// <param name="launchSet">启动设置</param>
        public void SetLanguage(GameDefinitions.Language language, bool launchSet = false)
        {
            if (language == GameDefinitions.Language.Unspecified)
            {
                throw new GameException("Language 无效。");
            }

            // 当启动时默认的运行时前次语言类型强制为“简体中文”
            m_RuntimeLastLanguage = launchSet ? GameDefinitions.Language.ChineseSimplified : m_Language;
            m_Language = language;
            m_PersistComponent.SetString(GameConstants.Persist.Common.WayType, GameConstants.Persist.Common.ClassifyName, GameConstants.Persist.Common.ItemKey.Language, m_Language.ToString());
            m_PersistComponent.Save(GameConstants.Persist.Common.WayType, GameConstants.Persist.Common.ClassifyName);

            // 调用语言表切换逻辑
            LuaComponent luaComponent = GameComponentsGroup.GetComponent<LuaComponent>();
            if (luaComponent != null)
            {
                if (luaComponent.LuaRelateLocalizationTableDataFromCSEventDelegate != null)
                {
                    luaComponent.LuaRelateLocalizationTableDataFromCSEventDelegate();
                }
            }

            UIComponent uiComponent = GameComponentsGroup.GetComponent<UIComponent>();
            if (uiComponent != null)
            {
                uiComponent.UnloadFonts();
                uiComponent.LoadFonts();
                uiComponent.RefreshFontsForUI();
            }

            // 派发文本本地化刷新事件
            GameMainRoot.Event.Fire(this, GameEventCmd.TextLocalizingRefresh);

            Log.Info("保存语言类型 '{0}' 到持久化存档数据。", m_Language.ToString());
        }

        /// <summary>
        /// 加载默认支持语种类型集合
        /// </summary>
        public void LoadDefaultLanguages()
        {
            m_LocalizationManager.LoadDefaultLanguages();
        }

        /// <summary>
        /// 检查是否支持指定的默认语种类型
        /// </summary>
        /// <param name="language">语种类型</param>
        /// <returns>是否支持指定的默认语种类型</returns>
        public bool HasDefaultLanguage(GameDefinitions.Language language)
        {
            return m_LocalizationManager.HasDefaultLanguage(language);
        }

        /// <summary>
        /// 移除所有默认支持语种类型集合
        /// </summary>
        public void RemoveAllDefaultLanguages()
        {
            m_LocalizationManager.RemoveAllDefaultLanguages();
        }

        /// <summary>
        /// 加载默认数据
        /// </summary>
        public void LoadDefaultDatas()
        {
            m_LocalizationManager.LoadDefaultDatas(m_Language, GamePathUtils.Json.GetRootDirectoryRelativePath(), AorTxt.Format("LocalizationDefault{0}", m_Language.ToString()));
        }

        /// <summary>
        /// 检查是否存在指定的默认数据
        /// </summary>
        /// <param name="keyName">字段名称</param>
        /// <returns>是否存在指定的默认数据</returns>
        public bool HasDefaultData(string keyName)
        {
            return m_LocalizationManager.HasDefaultData(m_Language, keyName);
        }

        /// <summary>
        /// 添加指定的默认数据
        /// </summary>
        /// <param name="keyName">字段名称</param>
        /// <param name="content">数据内容</param>
        public void AddDefaultData(string keyName, string content)
        {
            m_LocalizationManager.AddDefaultData(m_Language, keyName, content);
        }

        /// <summary>
        /// 移除指定的默认数据
        /// </summary>
        /// <param name="keyName">字段名称</param>
        /// <returns>是否成功移除指定的默认数据</returns>
        public bool RemoveDefaultData(string keyName)
        {
            return m_LocalizationManager.RemoveDefaultData(m_Language, keyName);
        }

        /// <summary>
        /// 移除所有默认数据
        /// </summary>
        public void RemoveAllDefaultDatas()
        {
            m_LocalizationManager.RemoveAllDefaultDatas();
        }

        /// <summary>
        /// 获取指定语种的字段内容（默认）
        /// 数据来源于json的多语言配置文件
        /// </summary>
        /// <param name="keyName">字段名称</param>
        /// <returns></returns>
        public string GetDefaultData(string keyName)
        {
            return m_LocalizationManager.GetDefaultData(m_Language, keyName);
        }

        /// <summary>
        /// 加载字体配置数据
        /// </summary>
        public void LoadFontDatas()
        {
            if(m_AutoFontAdapt)
            {
                m_LocalizationManager.LoadFontDatas(GamePathUtils.Json.GetRootDirectoryRelativePath(), AorTxt.Format("LocalizationFonts"));
            }
        }

        /// <summary>
        /// 添加指定的字体配置数据
        /// </summary>
        /// <param name="language">语种类型</param>
        /// <param name="fontDatas">字体数据</param>
        public void AddFontData(GameDefinitions.Language language, LocalizationFontData fontData)
        {
            if (m_AutoFontAdapt)
            {
                m_LocalizationManager.AddFontData(language, fontData);
            }
        }

        /// <summary>
        /// 移除所有字体配置数据
        /// </summary>
        public void RemoveAllFontDatas()
        {
            if (m_AutoFontAdapt)
            {
                m_LocalizationManager.RemoveAllFontDatas();
            }
        }

        /// <summary>
        /// 获取指定语种的字体配置数据
        /// 数据来源于json的多语言配置文件
        /// </summary>
        /// <param name="language">语种类型</param>
        /// <param name="fontDatas">字体数据集合</param>
        /// <returns></returns>
        public void GetFontDatas(GameDefinitions.Language language, out List<LocalizationFontData> fontDatas)
        {
            if (m_AutoFontAdapt)
            {
                m_LocalizationManager.GetFontDatas(language, out fontDatas);
            }
            else
            {
                fontDatas = null;
            }
        }

    }

}


