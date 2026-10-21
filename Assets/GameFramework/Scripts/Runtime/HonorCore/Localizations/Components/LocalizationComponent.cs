/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LocalizationComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   多语言本地化组件 - 核心逻辑
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 多语言本地化组件
    /// 负责语言切换、本地化文本管理、字体适配、语言配置持久化
    /// </summary>
    public sealed partial class LocalizationComponent 
    {
        //=========================================================================
        // 生命周期
        //=========================================================================
        /// <summary>
        /// 初始化多语言本地化组件
        /// <para>获取门面组件引用并创建本地化管理器（由 HonorCoreManager 阶段2 调用）</para>
        /// </summary>
        public void Init()
        {
            m_LocalizationManager = new LocalizationManager();
        }

        /// <summary>
        /// 清理本地化管理器引用（由门面 Dispose 统一调用）
        /// </summary>
        public void Cleanup()
        {
            m_LocalizationManager = null;
        }

        /// <summary>
        /// 释放本地化组件资源
        /// </summary>
        public void Dispose()
        {
            Cleanup();
        }

        //=========================================================================
        // 语言初始化与设置
        //=========================================================================
        #region Language Initialize & Set
        /// <summary>
        /// 初始化当前语言类型
        /// 编辑器模式优先使用编辑器设置，真机优先使用持久化存储，否则使用系统语言
        /// </summary>
        public void InitCurLanguage()
        {
            if (HonorUIManager.Instance.EditorResourceMode)
            {
                if (HonorUIManager.Instance.EditorLanguage == GameDefinitions.Language.Unspecified)
                {
                    Log.Info("编辑器资源模式下没有指定的语言类型，则默认使用当前系统语言 '{0}'。", SystemLanguageName);
                    m_Language = SystemLanguage;
                }
                else
                {
                    Log.Info("编辑器资源模式下有指定的语言类型 '{0}'，将不再使用其他语言类型。", HonorUIManager.Instance.EditorLanguage);
                    m_Language = HonorUIManager.Instance.EditorLanguage;
                }
            }
            else
            {
                if (HonorCoreManager.Instance.HasItem(GameConstants.Persist.Common.WayType, GameConstants.Persist.Common.ClassifyName, GameConstants.Persist.Common.ItemKey.Language))
                {
                    m_Language = (GameDefinitions.Language)Enum.Parse(typeof(GameDefinitions.Language), 
                        HonorCoreManager.Instance.GetString(GameConstants.Persist.Common.WayType, 
                        GameConstants.Persist.Common.ClassifyName, GameConstants.Persist.Common.ItemKey.Language));
                    
                    Log.Info("非编辑器资源模式下将使用持久化存档数据中的语言类型 '{0}'。", SystemLanguageName);
                }
                else
                {
                    m_Language = HasDefaultLanguage(SystemLanguage) ? SystemLanguage : GameDefinitions.Language.English;
                    Log.Info("非编辑器资源模式下持久化存档数据中的语言类型不存在，默认使用 '{0}' 类型。", m_Language);
                }
            }
        }

        /// <summary>
        /// 设置当前语言并保存到持久化
        /// </summary>
        /// <param name="language">目标语言</param>
        /// <param name="launchSet">是否为启动默认设置</param>
        public void SetLanguage(GameDefinitions.Language language, bool launchSet = false)
        {
            if (language == GameDefinitions.Language.Unspecified)
            {
                throw new GameException("Language 无效。");
            }

            m_RuntimeLastLanguage = launchSet ? GameDefinitions.Language.ChineseSimplified : m_Language;
            m_Language = language;
            
            HonorCoreManager.Instance.SetString(GameConstants.Persist.Common.WayType, 
                GameConstants.Persist.Common.ClassifyName, 
                GameConstants.Persist.Common.ItemKey.Language, m_Language.ToString());
            
            HonorCoreManager.Instance.Save(GameConstants.Persist.Common.WayType, GameConstants.Persist.Common.ClassifyName);

            // 通知 Lua 刷新本地化表（cmd: 1032 本地化语言表数据关联）
            LuaProtocolRunTime.NotifyLua((int)LuaProtocolCmd.P_LuaRelateLocalizationTableDataFromCSEventDelegate,
                new RelateLocalizationTableData(),
                (int)LuaProtocolDispatcher.LuaProtocolReceiveType.Success);

            // 刷新字体
            if (HonorUIManager.Instance.UI != null)
            {
                HonorUIManager.Instance.UnloadFonts();
                HonorUIManager.Instance.LoadFonts();
                HonorUIManager.Instance.RefreshFontsForUI();
            }

            // 派发全局多语言刷新事件
            HonorCoreManager.Instance.Fire(this, GameEventCmd.TextLocalizingRefresh);

            Log.Info("保存语言类型 '{0}' 到持久化存档数据。", m_Language);
        }
        #endregion

        //=========================================================================
        // 语言列表管理
        //=========================================================================
        #region Language List Management
        /// <summary>
        /// 加载默认支持的语言列表
        /// </summary>
        public void LoadDefaultLanguages()
        {
            m_LocalizationManager.LoadDefaultLanguages();
        }

        /// <summary>
        /// 检查是否支持指定语言
        /// </summary>
        public bool HasDefaultLanguage(GameDefinitions.Language language)
        {
            return m_LocalizationManager.HasDefaultLanguage(language);
        }

        /// <summary>
        /// 清空所有默认支持语言
        /// </summary>
        public void RemoveAllDefaultLanguages()
        {
            m_LocalizationManager.RemoveAllDefaultLanguages();
        }
        #endregion

        //=========================================================================
        // 本地化文本管理
        //=========================================================================
        #region Localization Text Management
        /// <summary>
        /// 加载当前语言的默认本地化数据
        /// </summary>
        public void LoadDefaultDatas()
        {
            m_LocalizationManager.LoadDefaultDatas(m_Language, 
                GamePathUtils.Json.GetRootDirectoryRelativePath(), 
                AorTxt.Format("LocalizationDefault{0}", m_Language));
        }

        /// <summary>
        /// 检查是否存在指定本地化键
        /// </summary>
        public bool HasDefaultData(string keyName)
        {
            return m_LocalizationManager.HasDefaultData(m_Language, keyName);
        }

        /// <summary>
        /// 添加一条本地化数据
        /// </summary>
        public void AddDefaultData(string keyName, string content)
        {
            m_LocalizationManager.AddDefaultData(m_Language, keyName, content);
        }

        /// <summary>
        /// 移除一条本地化数据
        /// </summary>
        public bool RemoveDefaultData(string keyName)
        {
            return m_LocalizationManager.RemoveDefaultData(m_Language, keyName);
        }

        /// <summary>
        /// 清空所有本地化数据
        /// </summary>
        public void RemoveAllDefaultDatas()
        {
            m_LocalizationManager.RemoveAllDefaultDatas();
        }

        /// <summary>
        /// 获取本地化文本（自动使用当前语言）
        /// </summary>
        public string GetDefaultData(string keyName)
        {
            return m_LocalizationManager.GetDefaultData(m_Language, keyName);
        }
        #endregion

        //=========================================================================
        // 字体管理
        //=========================================================================
        #region Font Management
        /// <summary>
        /// 加载字体配置文件
        /// </summary>
        public void LoadFontDatas()
        {
            if (m_AutoFontAdapt)
            {
                m_LocalizationManager.LoadFontDatas(GamePathUtils.Json.GetRootDirectoryRelativePath(), "LocalizationFonts");
            }
        }

        /// <summary>
        /// 添加语言对应的字体数据
        /// </summary>
        public void AddFontData(GameDefinitions.Language language, LocalizationFontData fontData)
        {
            if (m_AutoFontAdapt)
            {
                m_LocalizationManager.AddFontData(language, fontData);
            }
        }

        /// <summary>
        /// 清空所有字体配置
        /// </summary>
        public void RemoveAllFontDatas()
        {
            if (m_AutoFontAdapt)
            {
                m_LocalizationManager.RemoveAllFontDatas();
            }
        }

        /// <summary>
        /// 获取指定语言对应的字体列表
        /// </summary>
        public void GetFontData(GameDefinitions.Language language, out List<LocalizationFontData> fontDatas)
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
        #endregion
    }
}