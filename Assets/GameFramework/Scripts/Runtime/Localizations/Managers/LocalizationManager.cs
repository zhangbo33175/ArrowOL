/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LocalizationManager.cs
 * author:    云毅
 *  created:   2026
 * descrip:   本地化管理器 - 核心逻辑层
 ***************************************************************/

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 本地化管理器（核心逻辑层）
    /// 负责多语言文本加载、语言配置管理、字体配置管理、JSON解析
    /// </summary>
    public sealed partial class LocalizationManager
    {
        //=========================================================================
        // 构造函数
        //=========================================================================
        #region Constructor
        /// <summary>
        /// 构造函数：初始化组件与数据容器
        /// </summary>
        public LocalizationManager()
        {
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

            m_DefaultLanguages = new List<GameDefinitions.Language>();
            m_DefaultDatas = new Dictionary<GameDefinitions.Language, Dictionary<string, string>>();
            m_FontDatas = new Dictionary<GameDefinitions.Language, List<LocalizationFontData>>();
        }
        #endregion

        //=========================================================================
        // 语言列表管理
        //=========================================================================
        #region Language Management
        /// <summary>
        /// 从 JSON 加载支持的语言列表
        /// </summary>
        public void LoadDefaultLanguages()
        {
            TextAsset languagesJsonAsset = (TextAsset)m_AssetComponent.LoadAssetSync("JsonAsset", GamePathUtils.Json.GetRootDirectoryRelativePath(), "LocalizationDefaultLanguages");
            JArray jArray = JArray.Parse(languagesJsonAsset.text);
            
            foreach (var name in jArray)
            {
                m_DefaultLanguages.Add((GameDefinitions.Language)Enum.Parse(typeof(GameDefinitions.Language), name.ToString()));
            }
            
            m_AssetComponent.UnloadAsset(languagesJsonAsset, null, true);
        }

        /// <summary>
        /// 检查是否支持指定语言
        /// </summary>
        /// <param name="language">目标语言</param>
        public bool HasDefaultLanguage(GameDefinitions.Language language)
        {
            return m_DefaultLanguages.Contains(language);
        }

        /// <summary>
        /// 清空支持的语言列表
        /// </summary>
        public void RemoveAllDefaultLanguages()
        {
            m_DefaultLanguages.Clear();
        }
        #endregion

        //=========================================================================
        // 本地化文本管理
        //=========================================================================
        #region Localization Text Management
        /// <summary>
        /// 加载指定语言的本地化文本数据
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="abPath">AB 路径</param>
        /// <param name="assetName">资源名</param>
        public void LoadDefaultDatas(GameDefinitions.Language language, string abPath, string assetName)
        {
            TextAsset configJsonAsset = (TextAsset)m_AssetComponent.LoadAssetSync("JsonAsset", abPath, assetName);
            JObject jObject = JObject.Parse(configJsonAsset.text);

            foreach (var item in jObject)
            {
                AddDefaultData(language, item.Key, item.Value.ToString());
            }

            m_AssetComponent.UnloadAsset(configJsonAsset, null, true);
        }

        /// <summary>
        /// 检查是否存在指定本地化 Key
        /// </summary>
        public bool HasDefaultData(GameDefinitions.Language language, string keyName)
        {
            return GetDefaultData(language, keyName) != null;
        }

        /// <summary>
        /// 添加一条本地化文本
        /// </summary>
        public void AddDefaultData(GameDefinitions.Language language, string keyName, string content)
        {
            string data = GetDefaultData(language, keyName);
            if (data == null)
            {
                if (!m_DefaultDatas.ContainsKey(language))
                {
                    m_DefaultDatas.Add(language, new Dictionary<string, string>());
                }
                m_DefaultDatas[language].Add(keyName, content);
            }
        }

        /// <summary>
        /// 移除一条本地化文本
        /// </summary>
        public bool RemoveDefaultData(GameDefinitions.Language language, string keyName)
        {
            if (!HasDefaultData(language, keyName))
            {
                return false;
            }

            return m_DefaultDatas[language].Remove(keyName);
        }

        /// <summary>
        /// 清空所有语言的本地化数据
        /// </summary>
        public void RemoveAllDefaultDatas()
        {
            m_DefaultDatas.Clear();
        }

        /// <summary>
        /// 清空指定语言的本地化数据
        /// </summary>
        public void RemoveAllDefaultDatas(GameDefinitions.Language language)
        {
            if (m_DefaultDatas.ContainsKey(language))
            {
                m_DefaultDatas[language].Clear();
            }
        }

        /// <summary>
        /// 获取本地化文本（核心接口）
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="keyName">文本 Key</param>
        /// <returns>本地化文本内容</returns>
        public string GetDefaultData(GameDefinitions.Language language, string keyName)
        {
            if (language == GameDefinitions.Language.Unspecified)
            {
                throw new GameException("language 无效。");
            }

            if (string.IsNullOrEmpty(keyName))
            {
                throw new GameException("keyName 无效。");
            }

            string content = null;
            Dictionary<string, string> languageContents = null;
            m_DefaultDatas.TryGetValue(language, out languageContents);
            languageContents?.TryGetValue(keyName, out content);

            return content;
        }
        #endregion

        //=========================================================================
        // 字体管理
        //=========================================================================
        #region Font Management
        /// <summary>
        /// 从 JSON 加载多语言字体配置
        /// </summary>
        public void LoadFontDatas(string abPath, string assetName)
        {
            TextAsset configJsonAsset = (TextAsset)m_AssetComponent.LoadAssetSync("JsonAsset", abPath, assetName);
            JObject jObject = JObject.Parse(configJsonAsset.text);
            
            foreach (var item in jObject)
            {
                GameDefinitions.Language language = (GameDefinitions.Language)Enum.Parse(typeof(GameDefinitions.Language), item.Key);
                string fontType = item.Value["FontType"].ToString();
                int index = 0;
                
                while (item.Value[$"Mark{index}"] != null && !string.IsNullOrEmpty(item.Value[$"Mark{index}"].ToString()))
                {
                    string itemMark = item.Value[$"Mark{index}"].ToString();
                    string itemABPath = item.Value[$"ABPath{index}"].ToString();
                    string itemAssetName = item.Value[$"AssetName{index}"].ToString();
                    string itemCustomMaterialName = item.Value[$"CustomMaterialName{index}"].ToString();
                    float itemFontSizeScaleRatio = float.Parse(item.Value[$"FontSizeScaleRatio{index}"].ToString());
                    
                    LocalizationFontData fontData = new LocalizationFontData(fontType, itemMark, itemABPath, itemAssetName, itemCustomMaterialName, itemFontSizeScaleRatio);
                    AddFontData(language, fontData);
                    index++;
                }
            }

            m_AssetComponent.UnloadAsset(configJsonAsset, null, true);
        }

        /// <summary>
        /// 添加语言对应的字体数据（自动去重）
        /// </summary>
        public void AddFontData(GameDefinitions.Language language, LocalizationFontData fontData)
        {
            GetFontDatas(language, out List<LocalizationFontData> tmpfontDatas);
            
            if (tmpfontDatas != null)
            {
                foreach (var data in tmpfontDatas)
                {
                    if (data.Equals(fontData))
                        return;
                }
            }

            if (!m_FontDatas.ContainsKey(language))
            {
                m_FontDatas.Add(language, new List<LocalizationFontData>());
            }
            
            m_FontDatas[language].Add(fontData);
        }

        /// <summary>
        /// 清空所有字体配置
        /// </summary>
        public void RemoveAllFontDatas()
        {
            m_FontDatas.Clear();
        }

        /// <summary>
        /// 获取指定语言的字体列表
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="fontDatas">输出字体列表</param>
        public void GetFontDatas(GameDefinitions.Language language, out List<LocalizationFontData> fontDatas)
        {
            if (language == GameDefinitions.Language.Unspecified)
            {
                throw new GameException("language 无效。");
            }
            
            m_FontDatas.TryGetValue(language, out fontDatas);
        }
        #endregion
    }
}